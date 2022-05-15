import { Injectable } from '@angular/core';
// eslint-disable-next-line no-duplicate-imports
import * as signalR from '@microsoft/signalr';
import { HubConnectionState } from '@microsoft/signalr';
import { AuthService } from '../core/auth/auth.service';
import { environment } from '../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { ClientEvent } from '../models/client-events/client-event';
import { ToasterEventsService } from './toaster-events.service';
import { Router } from '@angular/router';

@Injectable()
export class ClientEventsService {
  public userId: string;
  public clientEventsSubject: BehaviorSubject<ClientEvent> = new BehaviorSubject<ClientEvent>(null);
  private readonly reconnectIntervals: number[];
  private retryConnectionInterval: any;
  private retryConnectionIntervalIndex = 0;
  private retryIntervalMs: number;
  private eventHubConnection: signalR.HubConnection;
  private pendingPageRoute: string;
  private currentPageRoute: string;

  constructor(private authService: AuthService, private toasterService: ToasterEventsService, private router: Router) {
    if (!environment.production) {
      this.reconnectIntervals = new Array(720);
      this.reconnectIntervals.fill(5000, 0, 720);
      this.reconnectIntervals.push(null); // signals end of reconnect retries
      return;
    }

    this.reconnectIntervals = [0, 2000, 5000, 10000, 30000, 60000, 300000, null];
  }

  public changeConnectionPageRoute(pageRoute: string, isReconnect: boolean = false) {
    console.log('changeConnectionPageRoute', pageRoute && !isReconnect)
    if (!pageRoute) {
      pageRoute = '';
    }

    pageRoute = pageRoute.split('?')[0];

    if (this.currentPageRoute === pageRoute && !isReconnect) {
      return;
    }
    console.log('here1')

    if (this.eventHubConnection && this.eventHubConnection.state === HubConnectionState.Connected) {
      console.log('here2')

      console.info('Changing connection page route to: ' + pageRoute);
      this.eventHubConnection
        .invoke('ChangeConnectionPageRoute', pageRoute)
        .then(() => {
          this.currentPageRoute = pageRoute;
        })
        .catch(error => console.error(`Error while changing signalR connection page route: ${error}`));
    } else {
      console.log('here3')
      this.pendingPageRoute = pageRoute;
    }
  }

  stopConnection() {
    if (this.eventHubConnection && this.eventHubConnection.state === HubConnectionState.Connected) {
      this.eventHubConnection
        .stop()
        .then(() => console.warn('Connection to client events Hub was closed'))
        .catch(reason => console.error(`Error while disconnecting from events hub: ${reason}`));
    }
  }

  public startConnection() {
    if (this.eventHubConnection && this.eventHubConnection.state === HubConnectionState.Connected) {
      console.error('Tried to connect but already connected');
      return;
    }
    this.eventHubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.cryptoniteApi}/hubs/client-events`, {
        accessTokenFactory: () => this.authService.accessToken
      })
      .withAutomaticReconnect(this.reconnectIntervals)
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.eventHubConnection.serverTimeoutInMilliseconds = 2 * 60 * 60 * 1000;
    this.eventHubConnection.keepAliveIntervalInMilliseconds = 5000;

    this.eventHubConnection.onreconnected(connectionId => {
      console.error('Reconnected to client events hub');
      this.onConnected(connectionId)
    });
    this.eventHubConnection.onreconnecting(error => {
      console.error('Reconnecting to Hub', error);
      this.toasterService.showToaster('Error', 'Connection lost', 'The connection to our APIs was lost. Trying to reconnect...');
    });

    this.eventHubConnection.onclose(() => {
      this.toasterService.showToaster('Error', 'Connection lost', 'The connection to our APIs was lost. Trying to reconnect...');
    });

    if (this.eventHubConnection.state !== HubConnectionState.Connected) {
      this.establishConnection();
    }
  }

  private establishConnection(): void {
    this.eventHubConnection
      .start()
      .then(() => {
        this.registerConnection();
        this.initializeListener();
        if (this.pendingPageRoute) {
          this.changeConnectionPageRoute(this.pendingPageRoute ?? this.router.url);
          this.pendingPageRoute = null;
        }
      })
      .catch(error => this.onConnectionFailure(error));

    this.changeConnectionPageRoute(this.router.url);
  }

  private registerConnection(): void {
    if (this.eventHubConnection.state === HubConnectionState.Connected) {
      this.eventHubConnection
        .invoke('RegisterConnection')
        .then(connectionId => {
          // console.log(`Connected to client events Hub. Connection Id: ${connectionId}`);
          if (this.retryConnectionInterval) {
            clearInterval(this.retryConnectionInterval);
            this.retryConnectionInterval = null;
            this.onConnected(connectionId);
          }
        })
        .catch(error => this.onConnectionFailure(error));
    }
  }

  private initializeListener(): void {
    this.eventHubConnection.on('client-events', data => {
      const event: ClientEvent = JSON.parse(data);
      // console.log('Received event with destination: ' + event.destination);
      // console.log('got client event', event);
      this.clientEventsSubject.next(event);
    });
  }

  private onConnected(connectionId: string): void {
    this.registerConnection();
    this.changeConnectionPageRoute(this.router.url, true);
    this.toasterService.showToaster('Success', 'Connection established', 'The connection to our APIs was re-established');
    this.retryConnectionIntervalIndex = 0;
  }

  private onConnectionFailure(error): void {
    if (!this.retryConnectionInterval) {
      this.toasterService.showToaster(
        'Error',
        'Unable to connect',
        'The application is unable to connect to our APIs. Re-trying...'
      );
    }
    console.error('Error while starting connection with events hub: ', error);

    this.retryIntervalMs = this.reconnectIntervals[this.retryConnectionIntervalIndex];

    clearInterval(this.retryConnectionInterval);
    this.retryConnectionIntervalIndex++;

    if (this.retryIntervalMs === null) {
      return;
    }

    this.retryConnectionInterval = setInterval(() => this.establishConnection(), this.retryIntervalMs);
  }
}
