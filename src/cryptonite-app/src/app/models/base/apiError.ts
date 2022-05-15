export interface ApiError {
  statusCode: number;
  message: string;
  target: string;
  details: ApiError[];
}
