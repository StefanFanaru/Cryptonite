import { ResultData } from '../portofolio/result-data';
import { Distribution } from '../portofolio/distribution';
import { AllTimeResult } from '../portofolio/all-time-result';

export interface DashboardUpdateEvent {
  todayResult: ResultData;
  distribution: Distribution;
  allTimeResult: AllTimeResult;
}
