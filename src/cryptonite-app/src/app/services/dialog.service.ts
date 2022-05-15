import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogErrorComponent, ErrorType } from '../components/dialog/dialog-error/dialog-error.component';
import { DialogConfirmationComponent } from '../components/dialog/dialog-confirmation/dialog-confirmation.component';

@Injectable()
export class DialogService {
  private opened = false;

  constructor(public dialog: MatDialog) {
  }

  openErrorDialog(content: string, type: ErrorType, status?: number): void {
    if (!this.opened) {
      this.opened = true;
      const dialogRef = this.dialog.open(DialogErrorComponent, {
        data: { content, type, status },
        width: this.getErrorMessageWidth(type),
        disableClose: false,
        hasBackdrop: true
      });

      dialogRef.afterClosed().subscribe(() => {
        this.opened = false;
      });
    }
  }

  openConfirmationDialog(title: string, question: string, isDanger: boolean, callback: () => void) {
    if (!this.opened) {
      this.opened = true;
      const dialogRef = this.dialog.open(DialogConfirmationComponent, {
        data: { title, question, isDanger, callback },
        width: '540px',
        disableClose: true,
        hasBackdrop: true
      });

      dialogRef.afterClosed().subscribe(() => {
        this.opened = false;
      });
    }
  }

  getErrorMessageWidth(type: ErrorType): string {
    switch (type) {
      case 'Exception':
        return '1200px';
      case 'TypeScript':
        return '900px';
      default:
        return '540px';
    }
  }
}
