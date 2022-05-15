import { ServiceBase } from '../services/base.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { SearchService } from '../services/search.service';
import { DialogService } from '../services/dialog.service';
import { DateTime } from 'luxon';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import 'src/app/helpers/stringExtensions';
import { BaseListDto } from '../models/base/baseListDto';
import { PaginatedSearch } from '../models/base/paginatedSearch';
import { SortDirection } from '../models/base/sortDirection';
import { PageOf } from '../models/base/pageOf';

export class BaseList<TItem extends BaseListDto, ItemService extends ServiceBase<TItem>> {
  dialogService: DialogService;
  httpService: ItemService;
  router: Router;
  route: ActivatedRoute;
  searchService: SearchService;
  rows: TItem[];
  rowsLoaded: Subject<boolean> = new BehaviorSubject<boolean>(false);
  selectedRows: TItem[] = [];
  submitResponse: Observable<any>;
  selectMultiple = false;
  allSelected: boolean;
  selectedItemsAreCompatible: boolean;
  visibleOneTimeCompatible = true;
  dateTime = DateTime;
  totalItems: number;
  filterSelectHasValue: boolean;
  currentSection: string;
  initQueryParams = true;
  queryParams: PaginatedSearch = {
    filterType: null,
    pageIndex: 0,
    pageSize: 15,
    sortColumn: '',
    sortDirection: SortDirection.Dsc,
    term: ''
  };

  protected constructor() {
  }

  get itemSelected() {
    return this.selectedRows[0];
  }

  set itemSelected(value) {
    this.selectedRows[0] = value;
  }

  initialize() {
    this.resetPagination();
    this.searchService.$searchInput
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(term => this.onSearchTermChange(term))
      )
      .subscribe();
  }

  refresh(reselectRows = true) {
    if (this.initQueryParams) {
      this.initQueryParams = false;
      this.queryParams = Object.assign({}, this.route.snapshot.queryParams as PaginatedSearch);
      this.queryParams.pageIndex ??= 0;
      this.queryParams.pageSize ??= 15;
      this.queryParams.sortDirection ??= SortDirection.Dsc;
    }

    if (this.currentSection) {
      this.queryParams.relation = this.currentSection; // cheated the type a bit here...
    }

    this.router
      .navigate([], {
        relativeTo: this.route,
        queryParams: this.queryParams
      })
      .then(() => this.getData(reselectRows));
  }

  resetPagination() {
    this.queryParams = {
      filterType: null,
      pageIndex: 0,
      pageSize: 15,
      sortColumn: null,
      sortDirection: SortDirection.Dsc,
      term: null,
      id: null,
      relation: null
    };
  }

  ////////// TABLE MANIPULATION //////////

  selectRow(item: TItem) {
    if (item.isReadOnly) {
      return;
    }
    if (this.selectMultiple) {
      if (item.selected) {
        // Filtering out the row that is now unselected
        this.selectedRows = this.selectedRows.filter(x => x.id !== item.id);
      } else {
        this.selectedRows.push(item);
      }
      item.selected = !item.selected; // Toggle selection
      this.checkSelectedItemsCompatible();
    } else {
      // No multiple selection available
      if (!item.selected) {
        // Unselecting all rows because we don't know witch one was selected beforehand
        this.rows.forEach(x => (x.selected = false));

        // Select the desired row
        item.selected = true;
        this.selectedRows = [item];
      } else {
        item.selected = false;
        this.selectedRows = [];
      }
    }
  }

  changeAllSelectionStates(value: boolean) {
    this.allSelected = value;
    this.rows.forEach(x => (x.selected = value));
    this.selectedRows = value ? this.rows : [];
    this.checkSelectedItemsCompatible();
  }

  selectMultipleToggle() {
    this.selectMultiple = !this.selectMultiple;
    if (!this.selectMultiple) {
      this.changeAllSelectionStates(false);
    }
  }

  // Checks to see if all the selected items follow the desired compatibility rules
  checkSelectedItemsCompatible() {
    this.visibleOneTimeCompatible = true;
    this.selectedItemsAreCompatible = true;
    for (let i = 1; i < this.selectedRows.length; i++) {
      // All selected items must share the same enabled state
      if (this.selectedRows[i].enabled !== this.selectedRows[0].enabled) {
        this.selectedItemsAreCompatible = false;
      }
    }
  }

  throwIfReadOnly(item: TItem) {
    if (item.isReadOnly) {
      throw new Error('Tried to change readonly item!');
    }
  }

  ////////// HTTP CALLS //////////
  getData(reselectRows = true) {
    let data: Observable<PageOf<TItem>>;

    Object.keys(this.queryParams).forEach(key => {
      if (!this.queryParams[key]) {
        delete this.queryParams[key];
      }
    });

    console.log(this.queryParams)

    if (this.queryParams.id) {
      data = this.httpService.getTableListOf(this.queryParams, `${this.queryParams.relation}/${this.queryParams.id}`);
    } else {
      data = this.httpService.getTableList(this.queryParams);
    }

    data.subscribe(response => {
      this.rows = response.pageData;
      this.totalItems = response.totalItems;
      this.selectedItemsAreCompatible = true;
      // Re-selecting the rows that were selected before the pagination event
      if (response.pageData.length > 0 && reselectRows) {
        this.selectedRows = this.rows.filter(row => this.selectedRows.map(x => x.id).includes(row.id));
        this.selectedRows.forEach(x => (x.selected = true));
      } else {
        this.selectedRows = [];
      }
      this.rowsLoaded.next(true);
    });
  }

  delete(itemName: string) {
    const isDeletingMultipleItems = this.selectedRows.length > 1;
    this.dialogService.openConfirmationDialog(
      `Delete ${isDeletingMultipleItems ? 'multiple items' : itemName}`,
      `Are you sure you want to delete ${isDeletingMultipleItems ? 'multiple items' : itemName}?`,
      true,
      () => {
        if (this.selectMultiple) {
          this.selectedRows.forEach(x => this.throwIfReadOnly(x));
          const ids = this.selectedRows.map(x => x.id);
          this.httpService.postBatch('delete-batch', ids).subscribe(() => {
            this.allSelected = false;
            this.getData();
          });
        } else {
          this.throwIfReadOnly(this.itemSelected);
          this.httpService.delete(this.itemSelected.id).subscribe(() => this.getData());
        }
      }
    );
  }

  onTablePageChange(event: PageEvent) {
    this.queryParams.pageIndex = event.pageIndex;
    this.queryParams.pageSize = event.pageSize;
    this.refresh();
  }

  getFilteredData(filter: number) {
    this.filterSelectHasValue = true;
    this.queryParams.filterType = filter;
    this.refresh();
  }

  sortData(sort: Sort) {
    this.queryParams.sortColumn = !sort.direction ? null : sort.active.firstCharUpper();
    this.queryParams.sortDirection = sort.direction === 'asc' ? SortDirection.Asc : SortDirection.Dsc;

    this.refresh();
  }

  private onSearchTermChange(term: string) {
    if (!term) {
      delete this.queryParams.term;
    } else {
      this.queryParams.term = term;
    }
    if (this.queryParams.pageIndex !== 0) {
      this.queryParams.pageIndex = 0;
    }
    this.refresh();
  }
}
