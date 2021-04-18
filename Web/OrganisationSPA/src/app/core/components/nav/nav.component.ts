import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
// @ts-ignore
import {environment} from '@env';
import {AuthService} from "@core/services/auth.service";
import {map} from "rxjs/operators";
import {UserInfo} from "angular-oauth2-oidc";
import {Observable} from "rxjs";


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavComponent implements OnInit {

  userName: string
  logoImgSrc: string = environment.logoImg;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    console.log('NAV INIT');
  }

  getUserName$(): Observable<string> {
    return this.authService.getUserInfo$().pipe(map((res: UserInfo) => {
      console.log(res);
      return res?.name
    }));
  }

  getUserName$(): Observable<string> {
    return this.authService.getUserInfo$().pipe(map((res: UserInfo) => {
      console.log('userInfo From Nav ',res);
      return res?.name
    }));
  }

  logout(): void {
    this.authService.logout();
  }

}
