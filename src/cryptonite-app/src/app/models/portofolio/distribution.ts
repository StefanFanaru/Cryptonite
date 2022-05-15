import { DistributionItem } from './distribution-item';

export interface Distribution {
  currency: string;
  totalValue: number;
  items: DistributionItem[];
}
