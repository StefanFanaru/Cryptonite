import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const authConfig: AuthConfig = {
  issuer: environment.identityApi,
  clientId: 'cryptonite_angular',
  responseType: 'code',
  redirectUri: `${window.origin}/index.html`,
  silentRefreshRedirectUri: `${window.origin}/silent-refresh.html`,
  scope: 'openid profile cryptonite_full',
  useSilentRefresh: true,
  timeoutFactor: 0.9,
  sessionChecksEnabled: false,
  showDebugInformation: true,
  clearHashAfterLogin: false,
  nonceStateSeparator: 'semicolon'
};
