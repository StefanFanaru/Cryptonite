import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-spinner-overlay',
  templateUrl: './spinner-overlay.component.html',
  styleUrls: ['./spinner-overlay.component.scss']
})
export class SpinnerOverlayComponent implements OnInit {
  navBarExpanded: boolean;

  constructor() {
  }

  ngOnInit(): void {
    this.navBarExpanded = JSON.parse(localStorage.getItem('nav-expanded'));
  }
}
