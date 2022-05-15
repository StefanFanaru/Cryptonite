import { ResultItem } from './result-item';

export interface ResultData {
  currency: string;
  totalOutcome: number;
  items: ResultItem[];
}
