import { Component, Inject, NgZone } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-error-dialog',
  templateUrl: './dialog-error.component.html',
  styleUrls: ['./dialog-error.component.scss']
})
export class DialogErrorComponent {
  env = environment;

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: {
      content: any;
      type: ErrorType;
      status?: number;
      title: string;
    },
    public dialogRef: MatDialogRef<DialogErrorComponent>,
    private ngZone: NgZone
  ) {
  }

  close() {
    this.ngZone.run(() => {
      this.dialogRef.close();
    });
  }
}

export type ErrorType = 'Exception' | 'Http' | 'ApiValidation' | 'TypeScript';
