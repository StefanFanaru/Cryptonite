import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { environment } from '../../environments/environment';
import { WebPushSubscriptionDto } from '../models/base/webPushSubscription';

@Injectable({
  providedIn: 'root'
})
export class WebPushSubscriptionsService extends ServiceBase<WebPushSubscriptionDto> {
  controller = 'webpush-subscriptions';
  origin = environment.cryptoniteApi;

  postSubscription(subscription: WebPushSubscriptionDto) {
    return this.post(subscription);
  }
}
