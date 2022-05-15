import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CryptoDataService } from '../../../services/crypto-data.service';
import { CurrenciesService } from '../../../services/currencies.service';
import { observeAutocompleteChanges } from '../../../common/utils/utils';
import { EntriesService } from '../../../services/entries.service';
import { ToasterService } from '../../../services/toaster.service';
import { TradeEntryInsert } from '../../../models/entries/trade-entry-insert';

@Component({
  selector: 'app-buy-entry',
  templateUrl: './trade-entry.component.html',
  styleUrls: ['./trade-entry.component.scss']
})
export class TradeEntryComponent implements OnInit {
  formGroup: FormGroup;
  filteredPaidCryptoSymbols: Observable<string[]>;
  filteredGainedCryptoSymbols: Observable<string[]>;
  cryptoSymbols: string[] = [];
  dateNow = new Date();

  constructor(
    private formBuilder: FormBuilder,
    private cryptoDataService: CryptoDataService,
    private currenciesService: CurrenciesService,
    private entriesService: EntriesService,
    private toasterService: ToasterService
  ) {
  }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      paidAmount: ['', [Validators.required, Validators.min(0)]],
      gainedAmount: ['', [Validators.required, Validators.min(0)]],
      paidCryptocurrency: ['', Validators.required],
      gainedCryptocurrency: ['', Validators.required],
      tradedAt: [this.dateNow, Validators.required]
    });

    this.cryptoDataService.getSymbols().subscribe(x => {
      this.cryptoSymbols = x;
      this.filteredPaidCryptoSymbols = observeAutocompleteChanges(this.formGroup.controls.paidCryptocurrency, this.cryptoSymbols);
      this.filteredGainedCryptoSymbols = observeAutocompleteChanges(
        this.formGroup.controls.gainedCryptocurrency,
        this.cryptoSymbols
      );
    });
  }

  onAutocompleteBlur(inputElement: HTMLInputElement, options: string[]): void {
    if (!options.some(x => x === inputElement.value.toUpperCase())) {
      inputElement.value = '';
      return;
    }

    inputElement.value = inputElement.value.toUpperCase();

    if (
      this.formGroup.controls.paidCryptocurrency.value.toUpperCase() ===
      this.formGroup.controls.gainedCryptocurrency.value.toUpperCase()
    ) {
      inputElement.value = '';
    }
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const entry: TradeEntryInsert = { ...this.formGroup.getRawValue() };
    const tradedAt: Date = this.formGroup.controls.tradedAt.value;
    entry.tradedAt = new Date(tradedAt.getFullYear(), tradedAt.getMonth(), tradedAt.getDate());
    entry.tradedAt.setUTCDate(tradedAt.getDate());
    entry.tradedAt.setUTCHours(0);

    this.entriesService
      .insertTradeEntry(entry)
      .subscribe(() => this.toasterService.show('Success', '', 'Your trade entry was added'));
  }
}
