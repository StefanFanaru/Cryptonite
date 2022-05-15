export const environment = {
  ENABLE_PURGE: true,
  production: true,
  envName: 'production',
  vapidPublicKey: 'BJLg8-CxGZkmC8iqeziAo5AHeK20jFn8nmjgoLmCxn3Z9XPyey5n71NJD3RVn60qAAuK6rw0R-a_RAwx4GJXDWE',
  identityApi: 'https://identity.stefanaru.com',
  identityLogin: 'https://identity.stefanaru.com/Account/Login',
  cryptoniteApi: 'https://cryptonite.stefanaru.com',
  automatorApi: 'https://automator.brevien.com',
  cryptoniteApiVersion: '1.0',
  automatorApiVersion: '1.0',
  auth_config: {
    client_id: 'sator_one_app',
    server_host: 'https://identity.stefanaru.com',
    redirect_url: 'com.cryptoniteapp.prod://callback',
    end_session_redirect_url: 'com.cryptoniteapp.prod://endsession',
    scopes: 'openid profile sator_one_full automator_full offline_access',
    pkce: true
  }
};
