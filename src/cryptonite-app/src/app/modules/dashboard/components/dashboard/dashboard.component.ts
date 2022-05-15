import { Component, OnInit } from '@angular/core';
import { PortofolioService } from '../../../../services/portofolio.service';
import { Distribution } from '../../../../models/portofolio/distribution';
import { ResultData } from '../../../../models/portofolio/result-data';
import { AllTimeResult } from '../../../../models/portofolio/all-time-result';
import { ClientEventsService } from 'src/app/services/client-events.service';
import { filter } from 'rxjs/operators';
import { ClientEvent } from '../../../../models/client-events/client-event';
import { DashboardUpdateEvent } from '../../../../models/client-events/dashboard-update-event';
import { HeaderService } from '../../../../services/header.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  distribution: Distribution;
  todayResult: ResultData;
  allTimeResult: AllTimeResult;
  isCompressed = false;
  oldWindowSize: { width: number; height: number };

  constructor(
    private portofolioService: PortofolioService,
    private clientEventsService: ClientEventsService,
    public headerService: HeaderService
  ) {
    if (window.outerHeight <= 547) {
      this.headerService.isShown = false;
      this.isCompressed = true;
    }
  }

  ngOnInit(): void {
    this.portofolioService.getDistributionItems().subscribe(x => (this.distribution = x));
    this.portofolioService.getTodayResult().subscribe(x => (this.todayResult = x));
    this.portofolioService.getAllTimeResult().subscribe(x => (this.allTimeResult = x));

    this.clientEventsService.clientEventsSubject
      .pipe(filter(event => event && event.destination === 'Dashboard'))
      .subscribe(event => this.handleDashboardUpdate(event));
  }

  onFullScreenClick(): void {
    this.headerService.isShown = !this.headerService.isShown;
  }

  onMiniClick(): void {
    this.isCompressed = !this.isCompressed;
    if (!this.isCompressed) {
      this.oldWindowSize ??= { width: window.screen.width, height: window.screen.height };
      window.resizeTo(this.oldWindowSize.width, this.oldWindowSize.height);
      return;
    }
    this.headerService.isShown = false;
    this.oldWindowSize = { width: window.outerWidth, height: window.outerHeight };
    window.resizeTo(974, 547);
  }

  private handleDashboardUpdate(clientEvent: ClientEvent): void {
    const dashboardUpdateEvent: DashboardUpdateEvent = clientEvent.innerEventJson;
    this.todayResult = dashboardUpdateEvent.todayResult;
    this.distribution = dashboardUpdateEvent.distribution;
    this.allTimeResult = dashboardUpdateEvent.allTimeResult;
  }
}
