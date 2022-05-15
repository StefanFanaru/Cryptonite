import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-diagnostics-theme',
  templateUrl: './diagnostics-theme.component.html',
  styleUrls: ['./diagnostics-theme.component.scss']
})
export class DiagnosticsThemeComponent implements OnInit {
  toggleControl = new FormControl(true);
  checked = false;
  indeterminate = false;
  labelPosition: 'before' | 'after' = 'after';
  disabled = false;
  value = 'Clear me';
  hidden = false;

  constructor() {
  }

  ngOnInit(): void {
  }

  toggleBadgeVisibility() {
    this.hidden = !this.hidden;
  }
}
