import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class BuyEntriesService extends ServiceBase<never> {
  controller = 'buyEntries';
}
