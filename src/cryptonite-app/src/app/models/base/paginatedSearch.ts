import { SortDirection } from './sortDirection';

export interface PaginatedSearch {
  pageIndex: number;
  pageSize: number;
  sortColumn: string;
  sortDirection: SortDirection;
  filterType: any;
  term: string;
  id?: string;
  relation?: string;
  isFormOpen?: boolean;
  underEditId?: string;
}
