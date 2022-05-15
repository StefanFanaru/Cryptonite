import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-message',
  templateUrl: './dialog-message.component.html',
  styleUrls: ['./dialog-message.component.scss']
})
export class DialogMessageComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogMessage) {
  }

  ngOnInit(): void {
  }
}

export interface DialogMessage {
  title: string;
  message: string;
}
