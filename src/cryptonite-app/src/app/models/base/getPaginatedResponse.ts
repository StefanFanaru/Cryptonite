export interface GetPaginatedResponse<TEntity> {
  count: number;
  items: TEntity[];
}
