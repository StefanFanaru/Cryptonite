import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../core/shared/shared.module';
import { EntriesPortal } from './entries-portal/entries-portal.component';
import { BuyEntryComponent } from './buy-entry/buy-entry.component';
import { TradeEntryComponent } from './trade-entry/trade-entry.component';
import { ImportEntriesComponent } from './imports/import-entries/import-entries.component';

@NgModule({
  declarations: [EntriesPortal, BuyEntryComponent, TradeEntryComponent, ImportEntriesComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: EntriesPortal },
      { path: 'buy', component: BuyEntryComponent },
      { path: 'trade', component: TradeEntryComponent },
      { path: 'import-entries/:importType', component: ImportEntriesComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class EntriesModule {
}
