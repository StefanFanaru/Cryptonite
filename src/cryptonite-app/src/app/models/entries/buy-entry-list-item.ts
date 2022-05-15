import { BaseListDto } from '../base/baseListDto';

export interface BuyEntryListItem extends BaseListDto {
  id: string;
  paidAmount: number;
  boughtAmount: number;
  paymentCurrency: string;
  boughtCryptocurrency: string;
  boughtAt: Date;
}