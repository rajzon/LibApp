import { Component, OnInit } from '@angular/core';
// @ts-ignore
import { environment } from '@env';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.sass']
})
export class NavComponent implements OnInit {

  userNamePlaceholder: string = "UserName";
  logoImgSrc: string = environment.logoImg;
  constructor() { }

  ngOnInit(): void {
  }

}
