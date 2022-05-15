/* eslint-disable @typescript-eslint/member-ordering */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, combineLatest, Observable, ReplaySubject } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { getGUID } from '../../common/utils/utils';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isAuthenticatedSubject$ = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject$.asObservable();
  private isDoneLoadingSubject$ = new ReplaySubject<boolean>();
  public isDoneLoading$ = this.isDoneLoadingSubject$.asObservable();
  /**
   * Publishes `true` if and only if (a) all the asynchronous initial
   * login calls have completed or errorred, and (b) the user ended up
   * being authenticated.
   *
   * In essence, it combines:
   *
   * - the latest known state of whether the user is authorized
   * - whether the ajax calls for initial log in have all been done
   */
  public canActivateProtectedRoutes$: Observable<boolean> = combineLatest([this.isAuthenticated$, this.isDoneLoading$]).pipe(
    map(values => values.every(b => b))
  );

  constructor(private oauthService: MyOAuthService, private router: Router) {
    //   // Useful for debugging:
    // this.oauthService.events.subscribe(event => {
    //   if (event instanceof OAuthErrorEvent) {
    //     console.error('OAuthErrorEvent Object:', event);
    //   } else {
    //     console.log('OAuthEvent Object:', event);
    //   }
    // });
    //
    if (environment.disableAuth) {
      this.isAuthenticatedSubject$.next(true);
      this.isDoneLoadingSubject$.next(true);
    }

    window.addEventListener('storage', event => {
      if ((event.key === 'auth_data_updated' && event.newValue !== null) || event.key === null) {
        this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken());

        if (!this.oauthService.hasValidAccessToken()) {
          this.navigateToLoginPage();
        }
      }
    });

    this.oauthService.events.subscribe(_ => {
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken());
    });

    this.oauthService.events
      .pipe(filter(e => ['token_received'].includes(e.type)))
      .subscribe(() => this.oauthService.loadUserProfile());

    this.oauthService.events
      .pipe(filter(e => ['session_terminated', 'session_error'].includes(e.type)))
      .subscribe(() => this.navigateToLoginPage());

    this.oauthService.setupAutomaticSilentRefresh({}, 'access_token');
  }

  // for debugging it makes sense.
  public get accessToken() {
    return this.oauthService.getAccessToken();
  }

  public get identityClaims() {
    return this.oauthService.getIdentityClaims();
  }

  public get logoutUrl() {
    return this.oauthService.logoutUrl;
  }

  public get userId() {
    const claims = this.oauthService.getIdentityClaims();
    if (claims) {
      // @ts-ignore
      return claims.sub;
    }
  }

  public runInitialLoginSequence(): Promise<void> {
    if (environment.disableAuth) {
      return Promise.resolve();
    }

    if (location.hash) {
      console.log('Encountered hash fragment, plotting as table...');
      console.table(
        location.hash
          .substr(1)
          .split('&')
          .map(kvp => kvp.split('='))
      );
    }

    // 0. LOAD CONFIG:
    // First we have to check to see how the IdServer is
    // currently configured:
    return (
      this.oauthService
        .loadDiscoveryDocument()

        // 1. HASH LOGIN:
        // Try to log in via hash fragment after redirect back
        // from IdServer from initImplicitFlow:
        .then(() => this.oauthService.tryLogin())

        .then(() => {
          if (this.oauthService.hasValidAccessToken()) {
            return Promise.resolve();
          }

          // 2. SILENT LOGIN:
          // Try to log in via a refresh because then we can prevent
          // needing to redirect the user:
          return this.oauthService
            .silentRefresh()
            .then(() => Promise.resolve())
            .catch(result => {
              // Subset of situations from https://openid.net/specs/openid-connect-core-1_0.html#AuthError
              // Only the ones where it's reasonably sure that sending the
              // user to the IdServer will help.
              const errorResponsesRequiringUserInteraction = [
                'interaction_required',
                'login_required',
                'account_selection_required',
                'consent_required'
              ];

              if (result && result.reason && errorResponsesRequiringUserInteraction.indexOf(result.reason.error) >= 0) {
                // 3. ASK FOR LOGIN:
                // At this point we know for sure that we have to ask the
                // user to log in, so we redirect them to the IdServer to
                // enter credentials.
                //
                // Enable this to ALWAYS force a user to login.
                // this.login();
                //
                // Instead, we'll now do this:
                console.warn('User interaction is needed to log in, we will wait for the user to manually log in.');
                return Promise.resolve();
              }

              // We can't handle the truth, just pass on the problem to the
              // next handler.
              return Promise.reject(result);
            });
        })

        .then(() => {
          this.isDoneLoadingSubject$.next(true);

          // Check for the strings 'undefined' and 'null' just to be sure. Our current
          // login(...) should never have this, but in case someone ever calls
          // initImplicitFlow(undefined | null) this could happen.
          if (this.oauthService.state && this.oauthService.state !== 'undefined' && this.oauthService.state !== 'null') {
            let stateUrl = this.oauthService.state;
            if (stateUrl.startsWith('/') === false) {
              stateUrl = decodeURIComponent(stateUrl);
            }
            // console.log(`There was state of ${this.oauthService.state}, so we are sending you to: ${stateUrl}`);
            this.router.navigateByUrl(stateUrl);
          }
        })
        .catch(() => this.isDoneLoadingSubject$.next(true))
    );
  }

  // These normally won't be exposed from a service like this, but

  public login(targetUrl?: string) {
    // Note: before version 9.1.0 of the library you needed to
    // call encodeURIComponent on the argument to the method.
    this.oauthService.initLoginFlow(targetUrl || this.router.url);
  }

  // public get refreshToken() {
  //   return this.oauthService.getRefreshToken();
  // }

  public logout() {
    this.oauthService.logOut();
    localStorage.setItem('auth_data_updated', getGUID());
    localStorage.removeItem('auth_data_updated');
  }

  // public get idToken() {
  //   return this.oauthService.getIdToken();
  // }

  public refresh() {
    this.oauthService.silentRefresh();
  }

  public hasValidToken() {
    return this.oauthService.hasValidAccessToken();
  }

  private navigateToLoginPage(): void {
    this.router.navigateByUrl('/should-login');
  }
}

@Injectable()
export class MyOAuthService extends OAuthService {
  protected storeAccessTokenResponse(accessToken: string, refreshToken: string, expiresIn: number, grantedScopes: string) {
    super.storeAccessTokenResponse(accessToken, refreshToken, expiresIn, grantedScopes);
    // Signal that the auth data is updated only after __all__ auth-related fields are written to the storage.
    // eslint-disable-next-line no-underscore-dangle
    this._storage.setItem('auth_data_updated', getGUID());
    // eslint-disable-next-line no-underscore-dangle
    this._storage.removeItem('auth_data_updated');
  }
}
