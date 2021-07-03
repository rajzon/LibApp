import { Injectable } from '@angular/core';
import {OAuthService, UserInfo} from "angular-oauth2-oidc";
import {authConfig} from "@core/services/auth-config";
import {BehaviorSubject, Observable} from "rxjs";
import {filter, map} from "rxjs/operators";
import {ActivatedRoute} from "@angular/router";


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

    //disabled for Dev only
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
    console.log('FIRED')
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
    console.log('FIRED LOGIN')
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
        this.getUserProfile().then((res: UserInfo) => {
          this.setUserInfo(res);
        })
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

  hasUserHaveRightsToAccess$(functionalityName: string, route: ActivatedRoute): Observable<boolean> {
    return this.getUserInfo$().pipe(map((userClaims: UserInfo) => {
      const allowedClaims = route.snapshot.data[functionalityName + 'Claims'] as Array<any>;
      const allowedRoles = route.snapshot.data[functionalityName + 'Roles'] as Array<string>;

      if (userClaims?.role === 'admin' && allowedRoles.includes(userClaims?.role))
        return true;

      if (AuthService.hasAllRequiredClaims(allowedClaims, userClaims))
        return true;

    }));


  }

  private static hasAllRequiredClaims(allowedClaims: any[], userClaims: UserInfo): boolean {
    for (let i = 0; i < allowedClaims.length; i++){
      let x = allowedClaims[i];
      for (let [key, values] of Object.entries(x)) {
        console.log(`Key: ${key} Value:${values}`)
        // if (! userClaimsNames.includes(key))
        //   return false;
        console.log(`User has claim ( ${key}: ${userClaims[key]} ) which value is included in:${(<string>values)} : `,(<string>values).includes(userClaims[key]));
        if (! (<string>values).includes(userClaims[key]))
          return false;
      }
    }
    return true;
  }

}
