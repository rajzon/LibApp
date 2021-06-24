import {AuthConfig} from "angular-oauth2-oidc";

export const authConfig: AuthConfig = {
  // Url of the Identity Provider
    issuer: 'https://localhost:8001',
    skipIssuerCheck: true,

    // URL of the SPA to redirect the user to after login
    redirectUri: window.location.origin,
    postLogoutRedirectUri: 'https://localhost:8001/auth/login',
    logoutUrl: 'https://localhost:8001/auth/logout',

    // The SPA's id. The SPA is registerd with this id at the auth-server
    // clientId: 'server.code',
    clientId: 'organisation_spa_client',


    // Just needed if your auth server demands a secret. In general, this
    // is a sign that the auth server is not configured with SPAs in mind
    // and it might not enforce further best practices vital for security
    // such applications.
    // dummyClientSecret: 'secret',
    responseType: 'code',
    // useSilentRefresh: true,

    // set the scope for the permissions the client should request
    // The first four are defined by OIDC.
    // Important: Request offline_access to get a refresh token
    // The api scope is a usecase specific one
    scope: 'openid profile offline_access book_api search_api stock_delivery_api role.scope book_privilege.scope delivery_privilege.scope',

    showDebugInformation: true,
}
