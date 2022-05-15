import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../core/shared/shared.module';
import { ListPortalComponent } from './list-portal/list-portal.component';
import { ListBuyEntriesComponent } from './list-buy-entries/list-buy-entries.component';
import { ListTradeEntriesComponent } from './list-trade-entries/list-trade-entries.component';

@NgModule({
  declarations: [ListPortalComponent, ListBuyEntriesComponent, ListTradeEntriesComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: ListPortalComponent },
      { path: 'buy', component: ListBuyEntriesComponent },
      { path: 'trade', component: ListTradeEntriesComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class ListsModule {
}
