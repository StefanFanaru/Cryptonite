import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Distribution } from '../models/portofolio/distribution';
import { ResultData } from '../models/portofolio/result-data';
import { AllTimeResult } from '../models/portofolio/all-time-result';

@Injectable({
  providedIn: 'root'
})
export class PortofolioService extends ServiceBase<never> {
  controller = 'portofolio';
  origin = environment.cryptoniteApi;

  getDistributionItems(): Observable<Distribution> {
    return this.getAny<Distribution>('distribution');
  }

  getTodayResult(): Observable<ResultData> {
    return this.getAny<ResultData>('result/today');
  }

  getAllTimeResult(): Observable<AllTimeResult> {
    return this.getAny<AllTimeResult>('result/alltime');
  }
}
