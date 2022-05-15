import { Component, OnInit } from '@angular/core';
import { BaseList } from '../../../core/base-list';
import { FormBuilder } from '@angular/forms';
import { SearchService } from '../../../services/search.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from '../../../services/dialog.service';
import { SortDirection } from '../../../models/base/sortDirection';
import { PortofolioCryptoCurrency } from '../../../models/entries/portofolio-crypto-currency';
import { PortofolioService } from '../../../services/portofolio.service';

@Component({
  selector: 'app-portofolio-list',
  templateUrl: './portofolio-list.component.html',
  styleUrls: ['../../lists/base-list.scss', './portofolio-list.component.scss']
})
export class PortofolioListComponent extends BaseList<PortofolioCryptoCurrency, PortofolioService> implements OnInit {
  displayedColumns: (keyof PortofolioCryptoCurrency)[] = ['symbol', 'value', 'amount', 'insertedAt', 'updatedAt'];
  rows: PortofolioCryptoCurrency[];
  pageTitle = 'Portofolio';
  stableCoins = ['USDT', 'DAI', 'USDC', 'TUSD', 'BUSD'];

  constructor(
    public httpService: PortofolioService,
    public formBuilder: FormBuilder,
    public searchService: SearchService,
    public route: ActivatedRoute,
    public router: Router,
    public dialogService: DialogService
  ) {
    super();
    this.queryParams.sortColumn = 'Value';
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

