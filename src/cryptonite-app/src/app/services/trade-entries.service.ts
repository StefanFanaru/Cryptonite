import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class TradeEntriesService extends ServiceBase<never> {
  controller = 'tradeEntries';
}
