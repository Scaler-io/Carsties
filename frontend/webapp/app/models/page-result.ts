export interface PageResult<T> {
  data: T[];
  pageSize: number;
  pageNumber: number;
  pageCount: number;
  total: number;
}
