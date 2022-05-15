import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CryptoDataService } from '../../../services/crypto-data.service';
import { CurrenciesService } from '../../../services/currencies.service';
import { observeAutocompleteChanges } from '../../../common/utils/utils';
import { EntriesService } from '../../../services/entries.service';
import { ToasterService } from '../../../services/toaster.service';
import { UserSettingsService } from '../../../services/user-settings.service';
import { BuyEntryInsert } from '../../../models/entries/buy-entry-insert';

@Component({
  selector: 'app-buy-entry',
  templateUrl: './buy-entry.component.html',
  styleUrls: ['./buy-entry.component.scss']
})
export class BuyEntryComponent implements OnInit {
  formGroup: FormGroup;
  filteredPaymentCurrencies: Observable<string[]>;
  filteredCryptoSymbols: Observable<string[]>;
  filteredBankAccountCurrencies: Observable<string[]>;
  currencies: string[] = [];
  cryptoSymbols: string[] = [];
  dateNow = new Date();

  constructor(
    private formBuilder: FormBuilder,
    private cryptoDataService: CryptoDataService,
    private currenciesService: CurrenciesService,
    private entriesService: EntriesService,
    private toasterService: ToasterService,
    private userSettingsService: UserSettingsService
  ) {
  }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      paidAmount: ['', [Validators.required, Validators.min(0)]],
      boughtAmount: ['', [Validators.required, Validators.min(0)]],
      bankConversionMargin: [
        {
          value: '',
          disabled: true
        },
        [Validators.min(0), Validators.max(100)]
      ],
      paymentCurrency: ['', Validators.required],
      boughtCryptocurrency: ['', Validators.required],
      bankAccountCurrency: ['', Validators.required],
      boughtAt: [this.dateNow, Validators.required]
    });

    this.currenciesService.getNames().subscribe(x => {
      this.currencies = x;
      this.filteredPaymentCurrencies = observeAutocompleteChanges(this.formGroup.controls.paymentCurrency, this.currencies);
      this.filteredBankAccountCurrencies = observeAutocompleteChanges(
        this.formGroup.controls.bankAccountCurrency,
        this.currencies
      );
    });

    this.cryptoDataService.getSymbols().subscribe(x => {
      this.cryptoSymbols = x;
      this.filteredCryptoSymbols = observeAutocompleteChanges(this.formGroup.controls.boughtCryptocurrency, this.cryptoSymbols);
    });

    this.userSettingsService.getSettings().subscribe(x => {
      this.formGroup.controls.bankAccountCurrency.setValue(x.bankAccountCurrency ?? '');
      this.formGroup.controls.bankConversionMargin.setValue(x.bankConversionMargin === 0 ? '' : x.bankConversionMargin);
      this.formGroup.controls.paymentCurrency.setValue(x.preferredCurrency ?? '');

      if (x.preferredCurrency && x.bankAccountCurrency) {
        this.formGroup.controls.bankConversionMargin.enable();
      }
    });
  }

  onAutocompleteBlur(inputElement: HTMLInputElement, options: string[]): void {
    if (!options.some(x => x === inputElement.value.toUpperCase())) {
      inputElement.value = '';
      return;
    }

    inputElement.value = inputElement.value.toUpperCase();
  }

  onMoneyCurrencyBlur(bankAccountCurrencyInput: HTMLInputElement, otherCurrency: string): void {
    this.onAutocompleteBlur(bankAccountCurrencyInput, this.currencies);

    const otherCurrencyValue = this.formGroup.controls[otherCurrency].value;
    if (bankAccountCurrencyInput.value && otherCurrencyValue && otherCurrencyValue !== bankAccountCurrencyInput.value) {
      this.formGroup.controls.bankConversionMargin.enable();
      return;
    }

    this.formGroup.controls.bankConversionMargin.disable();
  }

  onBankAccountCurrencyKeyDown(inputElement: HTMLInputElement): void {
    const paymentCurrency = this.formGroup.controls.paymentCurrency.value;
    const currentCurrency = inputElement.value.toUpperCase();
    if (!paymentCurrency || paymentCurrency === currentCurrency) {
      return;
    }

    if (this.currencies.some(x => x === currentCurrency)) {
      this.formGroup.controls.bankConversionMargin.enable();
      return;
    }

    this.formGroup.controls.bankConversionMargin.disable();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const buyEntry: BuyEntryInsert = { ...this.formGroup.getRawValue() };
    const boughtAt: Date = this.formGroup.controls.boughtAt.value;
    buyEntry.boughtAt = new Date(boughtAt.getFullYear(), boughtAt.getMonth(), boughtAt.getDate());
    buyEntry.boughtAt.setUTCDate(boughtAt.getDate());
    buyEntry.boughtAt.setUTCHours(0);

    this.entriesService
      .insertBuyEntry(buyEntry)
      .subscribe(() => this.toasterService.show('Success', '', 'Your buy entry was added'));
  }
}
