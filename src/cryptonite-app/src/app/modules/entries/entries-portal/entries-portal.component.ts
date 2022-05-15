import { Component, OnInit } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
  selector: 'app-new-entry',
  templateUrl: './entries-portal.component.html',
  styleUrls: ['./entries-portal.component.scss']
})
export class EntriesPortal implements OnInit {
  constructor(public navController: NavController) {
  }

  ngOnInit(): void {
  }
}
