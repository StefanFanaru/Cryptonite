import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrenciesService extends ServiceBase<never> {
  controller = 'currencies';
  origin = environment.cryptoniteApi;

  getNames(): Observable<string[]> {
    return this.getAny<string[]>('names');
  }
}
