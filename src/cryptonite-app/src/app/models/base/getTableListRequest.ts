import { SortDirection } from './sortDirection';

export interface GetTableListRequest {
  pageIndex: number;
  pageSize: number;
  sortColumn: string;
  sortDirection: SortDirection;
  filterType: any;
  searchTerm: string;
}
