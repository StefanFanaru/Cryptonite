import { AfterViewInit, Component, DoCheck, HostBinding, HostListener, OnDestroy, OnInit } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';
import { ThemingService } from './services/theming.service';
import { ClientEventsService } from './services/client-events.service';
import { DialogService } from './services/dialog.service';
import { NavigationStart, Router } from '@angular/router';
import { filter, switchMap, tap } from 'rxjs/operators';
import { SwPush, SwUpdate } from '@angular/service-worker';
import { AuthService } from './core/auth/auth.service';
import { ClientEvent } from './models/client-events/client-event';
import { ToasterEventsService } from './services/toaster-events.service';
import { HeaderService } from './services/header.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent implements OnInit, AfterViewInit, DoCheck, OnDestroy {
  @HostBinding('class') public cssClass: string;
  searchInput: Event | undefined;
  isMobile: boolean;
  fullScreenPages = ['/landing', '/auth/endsession'];
  isAuthenticated: boolean;
  isFullScreen = true;

  // eslint-disable-next-line max-params
  constructor(
    private clientEventsService: ClientEventsService,
    private toasterEventsService: ToasterEventsService,
    private dialogService: DialogService,
    private authService: AuthService,
    private themingService: ThemingService,
    private overlayContainer: OverlayContainer,
    private router: Router,
    private swUpdate: SwUpdate,
    private swPush: SwPush,
    public headerService: HeaderService
  ) {
  }

  ngOnDestroy(): void {
    this.clientEventsService.stopConnection();
  }

  @HostListener('window:resize')
  ngDoCheck(): void {
    this.calculateVhVw();
  }

  async ngOnInit(): Promise<void> {
    this.themingService.theme.subscribe((theme: string) => {
      this.cssClass = theme;
      this.applyThemeOnOverlays();
    });

    this.router.events.pipe(filter(e => e instanceof NavigationStart)).subscribe(async (e: NavigationStart) => {
      this.isFullScreen = this.fullScreenPages.some(x => e.url.includes(x));
      this.clientEventsService.changeConnectionPageRoute(e.url);
    });

    this.authService
      .runInitialLoginSequence()
      .then(() =>
        this.authService.isDoneLoading$
          .pipe(
            filter(isDone => isDone),
            switchMap(() => this.authService.isAuthenticated$),
            tap(isAuthenticated => {
              if (isAuthenticated) {
                console.log('AUTHCHANGE')
                this.isAuthenticated = true;
                this.clientEventsService.startConnection();
                this.clientEventsService.clientEventsSubject
                  .pipe(filter((event: ClientEvent) => event && event.destination === 'Toaster'))
                  .subscribe(clientEvent => this.toasterEventsService.handleToasterEvent(clientEvent));
              }
            })
          )
          .toPromise()
      )
      .catch(error => console.error('Error while running initialization flow', error));
  }

  ngAfterViewInit(): void {
    this.registerSwUpdateDialog();
    this.calculateVhVw();
    document.addEventListener('visibilitychange', () => {
      if (!document.hidden) {
        this.calculateVhVw();
      }
    });
  }

  calculateVhVw(wait: boolean = false): void {
    this.isMobile = window.innerWidth <= 700;
    const currentHeight = document.documentElement.style.getPropertyValue('--app-height');
    if (currentHeight && currentHeight === `${window.innerHeight.toString()}px`) {
      return;
    }

    const vh = window.innerHeight * 0.01;
    const vw = window.innerWidth * 0.01;
    document.documentElement.style.setProperty('--vh', `${vh}px`);
    document.documentElement.style.setProperty('--vw', `${vw}px`);

    // wait for orientationchange animation to finish
    if (wait) {
      setTimeout(() => document.documentElement.style.setProperty('--app-height', `${window.innerHeight}px`), 50);
      return;
    }
    document.documentElement.style.setProperty('--app-height', `${window.innerHeight}px`);
  }

  onHeaderSearchInput(event: Event): void {
    this.searchInput = event;
  }

  registerSwUpdateDialog(): void {
    if (this.swUpdate.isEnabled) {
      this.swUpdate.available.subscribe(() => {
        this.dialogService.openConfirmationDialog(
          'New app version available',
          'Do you want to load the new version?',
          false,
          () => window.location.reload()
        );
      });
    }
  }

  private applyThemeOnOverlays(): void {
    const overlayContainerClasses = this.overlayContainer.getContainerElement().classList;
    const themeClassesToRemove = Array.from(this.themingService.themes);
    if (themeClassesToRemove.length) {
      overlayContainerClasses.remove(...themeClassesToRemove);
    }
    overlayContainerClasses.add(this.cssClass);
  }
}
