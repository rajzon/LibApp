import { Injectable } from '@angular/core';
import {OAuthErrorEvent, OAuthService, UserInfo} from "angular-oauth2-oidc";
import {authConfig} from "@core/services/auth-config";
import {BehaviorSubject, Observable} from "rxjs";
import {filter} from "rxjs/operators";


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authenticated$ = new BehaviorSubject<boolean>(false);
  private userInfo$ = new BehaviorSubject<UserInfo>(null);

  constructor(private oauthService: OAuthService) {
    window.addEventListener('storage', (event) => {
      if (event.key !== 'access_token' && event.key !== null) {
        return;
      }

      this.setAuthenticated(this.oauthService.hasValidAccessToken());

      if (!this.oauthService.hasValidAccessToken()) {
        this.oauthService.initLoginFlow();
      }
    });

    this.oauthService.events
      .subscribe(_ => {
        this.setAuthenticated(this.oauthService.hasValidAccessToken());
      });


    this.oauthService.events
      .pipe(filter(e => ['token_refresh_error'].includes(e.type)))
      .subscribe(e => this.oauthService.logOut());

    this.oauthService.events
      .pipe(filter(e => ['token_refreshed'].includes(e.type)))
      .subscribe(_ => {
        this.getUserProfile().then((value: UserInfo) => {
          console.log(value);
          this.setUserInfo(value);
        })
      })

    this.oauthService.setupAutomaticSilentRefresh();

  }


  //Authentication Observable
  isAuthenticated$() : Observable<boolean> {
    return this.authenticated$.asObservable();
  }

  setAuthenticated(value: boolean): void {
    this.authenticated$.next(value);
  }

  //UserInfo Observable
  getUserInfo$(): Observable<UserInfo> {
    return this.userInfo$.asObservable();
  }

  setUserInfo(value: UserInfo): void {
    this.userInfo$.next(value);
  }

  initAuth(): void {
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
    });
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

  hasValidAccessToken(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  hasValidIdToken(): boolean {
    return this.oauthService.hasValidIdToken();
  }

  hasValidIdTokenAndAccessToken(): boolean {
    return this.oauthService.hasValidIdToken() &&
      this.oauthService.hasValidAccessToken()
  }

}
