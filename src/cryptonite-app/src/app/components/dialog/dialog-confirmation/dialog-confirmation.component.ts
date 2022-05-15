import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-confirmation',
  templateUrl: './dialog-confirmation.component.html',
  styleUrls: ['./dialog-confirmation.component.scss']
})
export class DialogConfirmationComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogConfirmation) {
  }

  ngOnInit(): void {
  }

  confirm() {
    this.data.callback();
  }
}

export interface DialogConfirmation {
  title: string;
  question: string;
  isDanger: boolean;
  callback: () => any;
}
