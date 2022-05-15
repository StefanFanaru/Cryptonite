export interface TradeEntryInsert {
  paidAmount: number;
  gainedAmount: number;
  paidCryptocurrency: string;
  gainedCryptocurrency: string;
  tradedAt: Date;
}
