import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { EntriesService } from '../../../../services/entries.service';
import { ImportType } from '../../../../models/entries/import-type';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-import-entries',
  templateUrl: './import-entries.component.html',
  styleUrls: ['./import-entries.component.scss']
})
export class ImportEntriesComponent implements OnInit {
  importType: ImportType;
  binanceExportPage: string;
  isImporting: boolean;
  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement>;

  constructor(private entriesService: EntriesService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params);
      this.importType = parseInt(params['importType']);
      switch (this.importType) {
        case ImportType.Buy:
          this.binanceExportPage = 'Buy Crypto History';
          break;
        case ImportType.Trade:
          this.binanceExportPage = 'Trade History';
          break;
        case ImportType.Sell:
          this.binanceExportPage = 'Payment History';
          break;
      }
    });
  }

  onFileInputChange(event: Event) {
    const element = event.currentTarget as HTMLInputElement;
    if (!element.files?.length) {
      return;
    }
    this.isImporting = true;
    this.entriesService.import(element.files.item(0), this.importType).subscribe(x => {
      this.isImporting = false;
      this.fileInput.nativeElement.value = '';
      this.router.navigate([`/lists/${ImportType[this.importType].toLowerCase()}`]);
    }, x => {
      this.isImporting = false;
      this.fileInput.nativeElement.value = '';
    });
  }
}
