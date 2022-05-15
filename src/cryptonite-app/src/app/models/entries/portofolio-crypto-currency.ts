import { BaseListDto } from '../base/baseListDto';

export interface PortofolioCryptoCurrency extends BaseListDto {
  symbol: string;
  value: number;
  valueCurrency: string;
  amount: number;
  insertedAt: Date;
  updatedAt: Date;
}