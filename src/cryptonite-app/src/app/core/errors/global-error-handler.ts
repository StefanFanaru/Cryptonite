import { ErrorHandler, Injectable } from '@angular/core';
import { DialogService } from '../../services/dialog.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private dialogService: DialogService) {
  }

  handleError(error: Error) {
    if (error.stack) {
      this.dialogService.openErrorDialog(error.stack, 'TypeScript');
    }

    console.error(error);
  }
}
