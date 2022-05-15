import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/auth/auth-guard.service';
import { ShouldLoginComponent } from './components/should-login/should-login.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  { path: 'should-login', component: ShouldLoginComponent },
  {
    path: 'diagnostics',
    loadChildren: () => import('./modules/diagnostics/diagnostics.module').then(m => m.DiagnosticsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'entries',
    loadChildren: () => import('./modules/entries/entries.module').then(m => m.EntriesModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'portofolio',
    loadChildren: () => import('./modules/portofolio/portofolio.module').then(m => m.PortofolioModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'lists',
    loadChildren: () => import('./modules/lists/lists.module').then(m => m.ListsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'settings',
    loadChildren: () => import('./modules/user-settings/user-settings.module').then(m => m.UserSettingsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      anchorScrolling: 'enabled',
      onSameUrlNavigation: 'reload',
      scrollPositionRestoration: 'enabled',
      preloadingStrategy: PreloadAllModules
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
