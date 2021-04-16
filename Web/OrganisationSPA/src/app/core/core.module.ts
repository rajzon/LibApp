import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavComponent} from "./components/nav/nav.component";
import {RouterModule} from "@angular/router";
import {OAuthModule} from "angular-oauth2-oidc";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {AuthInterceptor} from "@core/interceptors/auth.interceptor";

@NgModule({
  declarations: [NavComponent],
  exports: [
    NavComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    OAuthModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
  ]
})
export class CoreModule { }
