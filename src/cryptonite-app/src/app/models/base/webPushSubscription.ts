export interface WebPushSubscriptionDto {
  endpoint: string;
  expirationTime?: any;
  p256dh: number[];
  auth: number[];
}
