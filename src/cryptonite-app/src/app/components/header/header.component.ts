import { Component, Input, OnInit } from '@angular/core';
import { HeaderService } from '../../services/header.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  segment = 'home';
  @Input() isMobile: boolean;

  constructor(public headerService: HeaderService) {
  }

  ngOnInit() {
  }
}
