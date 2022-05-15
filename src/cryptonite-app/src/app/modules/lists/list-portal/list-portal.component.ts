import { Component, OnInit } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
  selector: 'app-new-entry',
  templateUrl: './list-portal.component.html',
  styleUrls: ['./list-portal.component.scss']
})
export class ListPortalComponent implements OnInit {
  constructor(public navController: NavController) {
  }

  ngOnInit(): void {
  }
}
