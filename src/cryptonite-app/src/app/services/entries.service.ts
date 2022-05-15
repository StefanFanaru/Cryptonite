import { Injectable } from '@angular/core';
import { ServiceBase } from './base.service';
import { Observable } from 'rxjs';
import { BuyEntryInsert } from '../models/entries/buy-entry-insert';
import { TradeEntryInsert } from '../models/entries/trade-entry-insert';
import { ImportType } from '../models/entries/import-type';

@Injectable({
  providedIn: 'root'
})
export class EntriesService extends ServiceBase<never> {
  controller = 'entries';

  insertBuyEntry(entry: BuyEntryInsert): Observable<void> {
    return this.postAny(entry, 'buy');
  }

  insertTradeEntry(entry: TradeEntryInsert): Observable<void> {
    return this.postAny(entry, 'trade');
  }

  import(file: File, type: ImportType): Observable<void> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<never>(this.getApiUrl(`import/${type}`), formData);
  }
}
