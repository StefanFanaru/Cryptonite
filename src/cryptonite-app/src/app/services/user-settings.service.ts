import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { Observable } from 'rxjs';
import { UserSettings } from '../models/user-settings/user-settings';

@Injectable({
  providedIn: 'root'
})
export class UserSettingsService extends ServiceBase<never> {
  controller = 'user-settings';

  getSettings(): Observable<UserSettings> {
    return this.getAny<UserSettings>();
  }

  updateSettings(settings: UserSettings): Observable<void> {
    return this.postAny(settings);
  }
}
