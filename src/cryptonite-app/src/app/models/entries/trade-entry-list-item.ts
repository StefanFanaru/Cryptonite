import { BaseListDto } from '../base/baseListDto';

export interface TradeEntryListItem extends BaseListDto {
  id: string;
  paidAmount: number;
  gainedAmount: number;
  paidCryptocurrency: string;
  gainedCryptocurrency: string;
  tradedAt: Date;
}