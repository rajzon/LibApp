import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import {Observable, of} from 'rxjs';
import {AuthService} from "@core/services/auth.service";
import {map} from "rxjs/operators";
import {UserInfo} from "angular-oauth2-oidc";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return  this.authService.getUserInfo$().pipe(map((userClaims: UserInfo) => {
      console.log(userClaims?.role);
      const allowedClaims = route.data['claims'] as Array<any>;
      const allowedRoles = route.data['roles'] as Array<string>;

      if (userClaims?.role === 'admin' && allowedRoles.includes(userClaims?.role))
        return true;

      if (AuthGuard.hasAllRequiredClaims(allowedClaims, userClaims))
        return true;
    }))
  }

  private static hasAllRequiredClaims(allowedClaims: any[], userClaims: UserInfo): boolean {
    for (let i = 0; i < allowedClaims.length; i++){
      let x = allowedClaims[i];
      for (let [key, values] of Object.entries(x)) {
        console.log(`Key: ${key} Value:${values}`)
        // if (! userClaimsNames.includes(key))
        //   return false;
        console.log(`User has claim ( ${key}: ${userClaims[key]} ) which value is included in:${(<string>values)} : `,(<string>values).includes(userClaims[key]));
        if (! (<string[]>values).includes(userClaims[key]))
          return false;
      }
    }
    return true;
  }


}
