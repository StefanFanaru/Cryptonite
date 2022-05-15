import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../../../services/search.service';
import { BaseList } from '../../../core/base-list';
import { DialogService } from '../../../services/dialog.service';
import { SortDirection } from '../../../models/base/sortDirection';
import { TradeEntriesService } from '../../../services/trade-entries.service';
import { TradeEntryListItem } from '../../../models/entries/trade-entry-list-item';

@Component({
  selector: 'app-lit-trade-entries',
  templateUrl: './list-trade-entries.component.html',
  styleUrls: ['../base-list.scss', './list-trade-entries.component.scss']
})
export class ListTradeEntriesComponent extends BaseList<TradeEntryListItem, TradeEntriesService> implements OnInit {
  displayedColumns: (keyof TradeEntryListItem)[] = ['selected', 'gainedAmount', 'gainedCryptocurrency', 'paidAmount', 'paidCryptocurrency', 'tradedAt'];
  rows: TradeEntryListItem[];
  pageTitle = 'Trade entries';

  constructor(
    public httpService: TradeEntriesService,
    public formBuilder: FormBuilder,
    public searchService: SearchService,
    public route: ActivatedRoute,
    public router: Router,
    public dialogService: DialogService
  ) {
    super();
    this.queryParams.sortColumn = 'TradedAt';
    this.queryParams.sortDirection = SortDirection.Dsc;
  }

  get itemSelected() {
    return this.selectedRows[0];
  }

  set itemSelected(value) {
    this.selectedRows[0] = value;
  }

  ngOnInit(): void {
    this.initialize();
  }
}
