import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'info-box',
  templateUrl: './info-box.component.html',
  styleUrls: ['./info-box.component.scss']
})
export class InfoBoxComponent implements OnInit {
  @Input() type: InfoBoxType;
  @Input() text: string;

  matIcon: string;
  backgroundColor: string;

  constructor() {
  }

  ngOnInit(): void {
    switch (this.type) {
      case 'Information':
        this.matIcon = 'info';
        this.backgroundColor = 'var(--color-primary-700)';
        break;
      case 'Success':
        this.matIcon = 'check_circle';
        this.backgroundColor = 'rgb(0 206 35 / 10%)';
        break;
      case 'Error':
        this.matIcon = 'cancel';
        this.backgroundColor = 'rgb(206 0 0 / 10%)';
        break;
      case 'Warning':
        this.matIcon = 'error';
        this.backgroundColor = 'rgb(255 153 0 / 10%)';
        break;
    }
  }
}

export type InfoBoxType = 'Information' | 'Error' | 'Warning' | 'Success';
