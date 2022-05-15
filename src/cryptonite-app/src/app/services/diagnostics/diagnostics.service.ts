import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ServiceBase } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class DiagnosticsService extends ServiceBase<any> {
  controller = 'diagnostics';
  origin = environment.cryptoniteApi;

  getCredentials() {
    return this.getAny('credentials');
  }
}
