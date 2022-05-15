import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/auth/auth.service';
import { LoadingBarService } from '@ngx-loading-bar/core';

@Component({
  selector: 'app-diagnostics',
  templateUrl: './diagnostics-auth.component.html',
  styleUrls: ['./diagnostics-auth.component.scss']
})
export class DiagnosticsAuthComponent implements OnInit {
  isAuthenticated: Observable<boolean>;
  canActivateProtectedRoutes: Observable<boolean>;
  claims: any;

  constructor(private authService: AuthService, public loadingBar: LoadingBarService) {
    this.isAuthenticated = this.authService.isAuthenticated$;
    this.canActivateProtectedRoutes = this.authService.canActivateProtectedRoutes$;
  }

  get hasValidToken() {
    return this.authService.hasValidToken();
  }

  get accessToken() {
    return this.authService.accessToken;
  }

  get identityClaims() {
    return this.authService.identityClaims;
  }

  ngOnInit(): void {
    this.claims = this.authService.identityClaims;
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }

  refresh() {
    this.authService.refresh();
  }

  reload() {
    window.location.reload();
  }

  //
  // get refreshToken() {
  //   return this.authService.refreshToken;
  // }

  clearStorage() {
    localStorage.clear();
  }

  // get idToken() {
  //   return this.authService.idToken;
  // }
}
