export interface BuyEntryInsert {
  paidAmount: number;
  boughtAmount: number;
  bankConversionMargin: number;
  paymentCurrency: string;
  boughtCurrency: string;
  bankAccountCurrency: string;
  boughtAt: Date;
}
