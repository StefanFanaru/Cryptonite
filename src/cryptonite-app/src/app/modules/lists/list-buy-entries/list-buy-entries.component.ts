import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../../../services/search.service';
import { BaseList } from '../../../core/base-list';
import { DialogService } from '../../../services/dialog.service';
import { SortDirection } from '../../../models/base/sortDirection';
import { BuyEntriesService } from '../../../services/buy-entries.service';
import { BuyEntryListItem } from '../../../models/entries/buy-entry-list-item';

@Component({
  selector: 'app-lit-buy-entries',
  templateUrl: './list-buy-entries.component.html',
  styleUrls: ['../base-list.scss', './list-buy-entries.component.scss']
})
export class ListBuyEntriesComponent extends BaseList<BuyEntryListItem, BuyEntriesService> implements OnInit {
  displayedColumns: (keyof BuyEntryListItem)[] = ['selected', 'paidAmount', 'paymentCurrency', 'boughtAmount', 'boughtCryptocurrency', 'boughtAt'];
  rows: BuyEntryListItem[];
  pageTitle = 'Buy entries';

  constructor(
    public httpService: BuyEntriesService,
    public formBuilder: FormBuilder,
    public searchService: SearchService,
    public route: ActivatedRoute,
    public router: Router,
    public dialogService: DialogService
  ) {
    super();
    this.queryParams.sortColumn = 'BoughtAt';
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
