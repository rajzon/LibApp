import { Component, OnInit } from '@angular/core';
import {environment} from "src/environments/environment";

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
