export interface PageOf<T> {
  pageData: T[];
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
}
