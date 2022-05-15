/* eslint-disable @typescript-eslint/ban-types */
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from '../helpers/guid';
import { ToasterService } from './toaster.service';
import { environment } from '../../environments/environment';
import { PaginatedSearch } from '../models/base/paginatedSearch';
import { PageOf } from '../models/base/pageOf';

@Injectable({
  providedIn: 'root'
})
export abstract class ServiceBase<TEntity> {
  public apiVersion = '1.0';
  public origin = environment.cryptoniteApi;
  abstract controller: string;
  protected http: HttpClient;
  protected toasterService: ToasterService;

  constructor(@Inject(HttpClient) http: any, @Inject(ToasterService) toasterService) {
    this.http = http;
    this.toasterService = toasterService;
  }

  get(route: string, id: string = null, params: {} = {}): Observable<TEntity> {
    return this.http.get<TEntity>(this.getApiUrl(route, id), {
      params,
      headers: this.buildHeaders()
    });
  }

  async getAsync(id: string, route: string = null, params: {} = {}): Promise<TEntity> {
    const promise = this.http
      .get<TEntity>(this.getApiUrl(route, id), {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((error: any) => {
      this.handleError(error);
    });
    return await promise;
  }

  getAny<T>(id: string = null, route: string = null, params: {} = {}, hideLoader = false): Observable<T> {
    return this.http.get<T>(this.getApiUrl(route, id), {
      params,
      headers: this.buildHeaders(hideLoader)
    });
  }

  getTableList(request: PaginatedSearch): Observable<PageOf<TEntity>> {
    return this.getAny<PageOf<TEntity>>('table-list', null, { ...request });
  }

  getTableListOf(request: PaginatedSearch, route: string): Observable<PageOf<TEntity>> {
    return this.getAny<PageOf<TEntity>>(`table-list/${route}`, null, { ...request });
  }

  query(route: string = null, params: {} = {}): Observable<TEntity[]> {
    return this.http.get<TEntity[]>(this.getApiUrl(route), {
      params,
      headers: this.buildHeaders()
    });
  }

  queryAny<TModel>(route: string, params: {} = {}): Observable<TModel[]> {
    return this.http.get<TModel[]>(this.getApiUrl(route), {
      params,
      headers: this.buildHeaders()
    });
  }

  async queryRouteAsync(route: string = null, params: {} = {}): Promise<TEntity[]> {
    const promise = this.http
      .get<TEntity[]>(this.getApiUrl(route), {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((error: any) => {
      this.handleError(error);
    });
    return await promise;
  }

  post<TResult>(item: TEntity, route: string = null, params: {} = {}, hideLoader = false) {
    return this.http.post<TResult>(this.getApiUrl(route), item, {
      params,
      headers: this.buildHeaders(hideLoader)
    });
  }

  postAny<TResult>(item: any, route: string = null, params: {} = {}) {
    return this.http.post<TResult>(this.getApiUrl(route), item, {
      params,
      headers: this.buildHeaders()
    });
  }

  async postAsync<TResult>(item: TEntity, route: string = null, params: {} = {}) {
    const promise = this.http
      .post<TResult>(this.getApiUrl(route), item, {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((response: any) => this.handleError(response));
    await promise;
  }

  async postAnyAsync<TResult>(route: string, item: any, params: {} = {}) {
    const promise = this.http.post<TResult>(this.getApiUrl(route), item).toPromise();
    promise.catch((response: any) => this.handleError(response));
    await promise;
  }

  postBatch(route: string, ids?: string[], items?: TEntity[]) {
    let body = ids?.length ? { ids } : { items };
    return this.http.post<TEntity>(this.getApiUrl(route), body, {
      headers: this.buildHeaders()
    });
  }

  put<TResult>(item: TEntity, route: string = null, params: {} = {}) {
    // @ts-ignore
    return this.http.put<TResult>(`${this.getApiUrl(route)}/${item.id}`, item, {
      params,
      headers: this.buildHeaders()
    });
  }

  async putAsync<TResult>(item: TEntity, route: string = null, params: {} = {}) {
    // @ts-ignore
    if (!item.id) {
      // @ts-ignore
      item.id = Guid.newGuid();
    }
    const promise = this.http
      // @ts-ignore
      .put<TResult>(`${this.getApiUrl(route)}/${item.id}`, item, {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((error: any) => this.handleError(error));
    await promise;
  }

  async putAnyAsync<TResult>(id: string, route: string, params: {} = {}) {
    const promise = this.http
      .put<TResult>(this.getApiUrl(route, id), params, {
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((error: any) => this.handleError(error));
    await promise;
  }

  patch<TResult>(item: TEntity, route: string = null, params: {} = {}) {
    return this.http.patch<TResult>(this.getApiUrl(route), item, {
      params,
      headers: this.buildHeaders()
    });
  }

  patchAny<TResult>(item: any, route: string = null, params: {} = {}) {
    return this.http.patch<TResult>(this.getApiUrl(route), item, {
      params,
      headers: this.buildHeaders()
    });
  }

  async patchAsync<TResult>(item: TEntity, route: string = null, params: {} = {}) {
    const promise = this.http
      .patch<TResult>(this.getApiUrl(route), item, {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((response: any) => this.handleError(response));
    await promise;
  }

  async patchAnyAsync<TResult>(route: string, item: any, params: {} = {}) {
    const promise = this.http
      .patch<TResult>(this.getApiUrl(route), item, {
        params,
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((response: any) => this.handleError(response));
    await promise;
  }

  delete<TResult>(id: string = null, route: string = null) {
    return this.http.delete<TResult>(this.getApiUrl(route, id), {
      headers: this.buildHeaders()
    });
  }

  patchBatch<TResult>(route: string, ids: string[] = null, items: TEntity[] = null) {
    return this.http.patch<TResult>(this.getApiUrl(route), ids ?? items, {
      headers: this.buildHeaders()
    });
  }

  async deleteAsync<TResult>(route: string = null, id: string) {
    const promise = this.http
      .delete<TResult>(this.getApiUrl(route, id), {
        headers: this.buildHeaders()
      })
      .toPromise();
    promise.catch((error: any) => {
      this.handleError(error);
    });
    await promise;
  }

  getBlobResource(path: string): Observable<any> {
    return this.http.get(`${this.getApiUrl()}/${path}`, {
      responseType: 'blob',
      headers: this.buildHeaders()
    });
  }

  protected getApiUrl(route: string = null, id: string = null): string {
    return `${this.origin}/api/v${this.apiVersion}/${this.controller}${route ? `/${route}` : ''}${id ? `/${id}` : ''}`;
  }

  protected handleError(response: any) {
    if (response.status !== 400) {
      if (
        !(response.status === 200 && response.url && response.url.toLowerCase().indexOf('account/login') >= 0) &&
        response.status !== 401 &&
        response.status !== 402
      ) {
        if (!response.error || !response.error.custom) {
          console.log('An error occured during saving the requested information.');
        }
      }
    } else {
      console.log(response.error);
    }
  }

  protected buildHeaders(hideLoader = false): HttpHeaders {
    let headers = new HttpHeaders();
    headers = headers.append('Accept', '*/*');
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    if (hideLoader) {
      headers = headers.append('ignoreLoadingBar', '');
    }
    return headers;
  }
}
