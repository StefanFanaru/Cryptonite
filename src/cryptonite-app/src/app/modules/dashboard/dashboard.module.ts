import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../core/shared/shared.module';
import { ChartsModule } from 'ng2-charts';
import { DistributionChartComponent } from './components/distribution-chart/distribution-chart.component';
import { ResultResumeComponent } from './components/result/result-resume.component';

@NgModule({
  declarations: [DashboardComponent, DistributionChartComponent, ResultResumeComponent],
  imports: [
    CommonModule,
    ChartsModule,
    RouterModule.forChild([
      { path: '', component: DashboardComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class DashboardModule {
}
