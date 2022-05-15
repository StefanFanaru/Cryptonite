import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-material-text-input',
  templateUrl: './material-text-input.component.html',
  styleUrls: ['./material-text-input.component.scss']
})
export class MaterialTextInputComponent {
  @Input() label: string;
  @Input() controlName: string;
  @Input() formGroup: FormGroup;
}
