import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../core/shared/shared.module';
import { PortofolioListComponent } from './portofolio-list/portofolio-list.component';

@NgModule({
  declarations: [
    PortofolioListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: PortofolioListComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class PortofolioModule {
}
