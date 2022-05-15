import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CurrenciesService } from '../../../services/currencies.service';
import { Observable } from 'rxjs';
import { observeAutocompleteChanges } from '../../../common/utils/utils';
import { UserSettingsService } from '../../../services/user-settings.service';
import { ToasterService } from '../../../services/toaster.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {
  formGroup: FormGroup;
  currencies: string[] = [];
  filteredPreferredCurrencies: Observable<string[]>;
  filteredBankAccountCurrencies: Observable<string[]>;

  constructor(
    private formBuilder: FormBuilder,
    private currenciesService: CurrenciesService,
    private userSettingsService: UserSettingsService,
    private toasterService: ToasterService
  ) {
  }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      preferredCurrency: [''],
      bankAccountCurrency: [''],
      bankConversionMargin: ['', [Validators.min(0), Validators.max(100)]]
    });

    this.currenciesService.getNames().subscribe(x => {
      this.currencies = x;
      this.filteredPreferredCurrencies = observeAutocompleteChanges(this.formGroup.controls.preferredCurrency, this.currencies);
      this.filteredBankAccountCurrencies = observeAutocompleteChanges(
        this.formGroup.controls.bankAccountCurrency,
        this.currencies
      );
    });

    this.userSettingsService.getSettings().subscribe(x => {
      this.formGroup.controls.preferredCurrency.setValue(x.preferredCurrency ?? 'USD');
      this.formGroup.controls.bankAccountCurrency.setValue(x.bankAccountCurrency ?? 'USD');
      this.formGroup.controls.bankConversionMargin.setValue(x.bankConversionMargin ?? 0);
    });
  }

  onAutocompleteBlur(inputElement: HTMLInputElement): void {
    if (!this.currencies.some(x => x === inputElement.value.toUpperCase())) {
      inputElement.value = '';
      return;
    }

    inputElement.value = inputElement.value.toUpperCase();
  }

  onSave(): void {
    if (this.formGroup.invalid) {
      return;
    }

    this.userSettingsService
      .updateSettings({ ...this.formGroup.getRawValue() })
      .subscribe(() => this.toasterService.show('Success', '', 'Your settings were saved'));
  }
}
