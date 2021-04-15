import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavComponent} from "./components/nav/nav.component";
import {RouterModule} from "@angular/router";
import {OAuthModule} from "angular-oauth2-oidc";

@NgModule({
  declarations: [NavComponent],
  exports: [
    NavComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OAuthModule.forRoot()
  ]
})
export class CoreModule { }
