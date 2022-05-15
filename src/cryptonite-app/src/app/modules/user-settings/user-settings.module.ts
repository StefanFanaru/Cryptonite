import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../core/shared/shared.module';

@NgModule({
  declarations: [UserSettingsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: UserSettingsComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class UserSettingsModule {
}
