import { ApiError } from './apiError';

export interface OperationResult<TResult> {
  hasResult: boolean;
  result: TResult;
  isSuccess: boolean;
  statusCode: number;
  error: ApiError;
}
