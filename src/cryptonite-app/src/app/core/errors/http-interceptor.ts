import { Observable, throwError } from 'rxjs';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { DialogService } from '../../services/dialog.service';
import { ErrorType } from '../../components/dialog/dialog-error/dialog-error.component';
import { ToasterEventsService } from '../../services/toaster-events.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {
  constructor(private dialogService: DialogService, private toasterService: ToasterEventsService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (environment.production) {
          this.toasterService.showToaster('Error', 'Error', `An error occured (${error.status})`);
          return throwError(error);
        }

        let content;
        let validationErrors;
        let exception;

        if (error.error) {
          if (error.error.error && !error.error.error.details?.length) {
            this.toasterService.showToaster('Error', 'Error', error.error.error.message);
            return throwError(error);
          }

          exception = error.error.Message !== undefined || typeof error.error === 'string';
          if (error.error.error?.details?.length) {
            validationErrors = error.error.error.details.map(x => x.message);
          }

          if (error.error?.errors) {
            validationErrors = this.extractValidationErrors(error);
          }
        }

        let errorType: ErrorType = exception ? 'Exception' : validationErrors ? 'ApiValidation' : 'Http';

        switch (errorType) {
          case 'Exception':
            content = JSON.stringify(error.error, null, 2);
            break;
          case 'ApiValidation':
            content = validationErrors;
            break;
          case 'Http':
            content = error?.error?.message || error?.message || 'Unknown HTTP error';
            break;
        }

        if (content) {
          this.dialogService.openErrorDialog(content, errorType, error.status);
        }

        return throwError(error);
      })
    ) as Observable<HttpEvent<any>>;
  }

  extractValidationErrors(error: HttpErrorResponse) {
    let errors = error.error.errors;
    if (errors.length > 0) {
      return errors;
    }
  }
}
