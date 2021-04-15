import { Component, OnInit } from '@angular/core';
// @ts-ignore
import { environment } from '@env';
import {AuthService} from "@core/services/auth.service";


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.sass']
})
export class NavComponent implements OnInit {

  userNamePlaceholder: string = "UserName";
  logoImgSrc: string = environment.logoImg;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  logout(): void {
    this.authService.logout();
  }

}
