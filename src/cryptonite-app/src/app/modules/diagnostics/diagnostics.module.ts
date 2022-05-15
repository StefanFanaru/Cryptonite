import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DiagnosticsThemeComponent } from './diagnostics-theme/diagnostics-theme.component';
import { SharedModule } from '../../core/shared/shared.module';
import { DiagnosticsGeneralComponent } from './diagnostics-general/diagnostics-general.component';
import { DiagnosticsAuthComponent } from './diagnostics-auth/diagnostics-auth.component';

@NgModule({
  declarations: [DiagnosticsAuthComponent, DiagnosticsThemeComponent, DiagnosticsGeneralComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: DiagnosticsGeneralComponent },
      { path: 'auth', component: DiagnosticsAuthComponent },
      { path: 'theme', component: DiagnosticsThemeComponent },
      { path: '*', redirectTo: '' }
    ]),
    SharedModule
  ]
})
export class DiagnosticsModule {
}
