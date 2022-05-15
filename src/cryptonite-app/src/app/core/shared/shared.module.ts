import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { _MatMenuDirectivesModule, MatMenuModule } from '@angular/material/menu';
import { MatSliderModule } from '@angular/material/slider';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatSortModule } from '@angular/material/sort';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatDividerModule } from '@angular/material/divider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DialogMessageComponent } from '../../components/dialog/dialog-message/dialog-message.component';
import { DialogConfirmationComponent } from '../../components/dialog/dialog-confirmation/dialog-confirmation.component';
import { DialogErrorComponent } from '../../components/dialog/dialog-error/dialog-error.component';
import { PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface, PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { InfoBoxComponent } from '../../components/info-box/info-box.component';
import { RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { SharedDirectivesModule } from '../../directives/shared-directives.module';
import { MatChipsModule } from '@angular/material/chips';
import { CustomDatePipe } from '../../common/pipes/custom-date.pipe';
import { MaterialNumberInputComponent } from '../../components/inputs/material-number-input/material-number-input.component';
import { MaterialTextInputComponent } from '../../components/inputs/material-text-input/material-text-input.component';
import { AnimatedDigitComponent } from '../../components/animated-digit/animated-digit.component';
import { NoItemsComponent } from '../../components/no-items/no-items.component';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

@NgModule({
  declarations: [
    DialogMessageComponent,
    DialogConfirmationComponent,
    DialogErrorComponent,
    InfoBoxComponent,
    CustomDatePipe,
    MaterialNumberInputComponent,
    MaterialTextInputComponent,
    AnimatedDigitComponent,
    NoItemsComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    _MatMenuDirectivesModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatToolbarModule,
    MatSidenavModule,
    MatMenuModule,
    MatAutocompleteModule,
    MatIconModule,
    MatTooltipModule,
    MatTabsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatGridListModule,
    FormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatButtonToggleModule,
    MatSortModule,
    ClipboardModule,
    MatDividerModule,
    MatSlideToggleModule,
    MatProgressBarModule,
    MatBadgeModule,
    MatFormFieldModule,
    MatSliderModule,
    PerfectScrollbarModule,
    SharedDirectivesModule,
    IonicModule,
    MatChipsModule
  ],
  exports: [
    _MatMenuDirectivesModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatToolbarModule,
    MatSidenavModule,
    MatMenuModule,
    MatAutocompleteModule,
    MatTooltipModule,
    MatTabsModule,
    FormsModule,
    MatTableModule,
    MatGridListModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatButtonToggleModule,
    MatSortModule,
    ClipboardModule,
    MatDividerModule,
    MatSlideToggleModule,
    MatProgressBarModule,
    MatBadgeModule,
    MatFormFieldModule,
    MatIconModule,
    MatSliderModule,
    PerfectScrollbarModule,
    InfoBoxComponent,
    IonicModule,
    MatChipsModule,
    CustomDatePipe,
    MaterialNumberInputComponent,
    MaterialTextInputComponent,
    AnimatedDigitComponent,
    NoItemsComponent
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    FormBuilder
  ]
})
export class SharedModule {
}
