import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CryptoDataService extends ServiceBase<never> {
  controller = 'crypto-data';

  getSymbols(): Observable<string[]> {
    return this.getAny<string[]>('symbols');
  }
}
