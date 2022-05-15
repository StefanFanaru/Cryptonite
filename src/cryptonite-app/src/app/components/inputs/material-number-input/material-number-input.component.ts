import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-material-number-input',
  templateUrl: './material-number-input.component.html',
  styleUrls: ['./material-number-input.component.scss']
})
export class MaterialNumberInputComponent {
  @Input() label: string;
  @Input() controlName: string;
  @Input() formGroup: FormGroup;
  @Input() min: number;
  @Input() max: number;
  @Input() unit: string;
}
