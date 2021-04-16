import { Injectable } from '@angular/core';
import {AuthConfig, OAuthService, UserInfo} from "angular-oauth2-oidc";


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private oauthService: OAuthService) {
  }

  initAuth(): void {
    const authConfig: AuthConfig = {
      // Url of the Identity Provider
      issuer: 'https://localhost:8001',

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

      // set the scope for the permissions the client should request
      // The first four are defined by OIDC.
      // Important: Request offline_access to get a refresh token
      // The api scope is a usecase specific one
      scope: 'openid profile offline_access book_api role.scope',

      showDebugInformation: true,
    };
    this.oauthService.configure(authConfig);
    this.login();
  }

  login(): void {
    this.oauthService.loadDiscoveryDocumentAndLogin().then(_ => {
      if(! this.oauthService.hasValidIdToken() || ! this.oauthService.hasValidAccessToken()) {
        console.log('Trying to Login: Id Token or Access Token is invalid')
        this.oauthService.initLoginFlow();
        console.log("when token is invalid access token: ", this.oauthService.getAccessToken());
        console.log("when token is invalid id token: ", this.oauthService.getIdToken());
        console.log("when token is invalid IdTokenClaims: ", this.oauthService.getIdentityClaims());
        console.log("when token is invalid UserProfileClaims: ", this.oauthService.loadUserProfile());

      } else {
        console.log("after initLoginFlow access token: ", this.oauthService.getAccessToken());
        console.log("after initLoginFlow id token: ", this.oauthService.getIdToken());
        console.log("after initLoginFlow IdTokenClaims: ", this.oauthService.getIdentityClaims());
        console.log("after initLoginFlow UserProfileClaims: ", this.oauthService.loadUserProfile());
      }
    })
  }

  logout(): void {
    console.log('Trying to Logout')
    this.oauthService.revokeTokenAndLogout();
  }

  getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  getUserProfile(): Promise<UserInfo> {
    return this.oauthService.loadUserProfile();
  }

}
