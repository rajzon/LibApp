import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
// @ts-ignore
import { CoreModule } from "@core/core.module";
import { routing } from "./app.routing";
import {BookModule} from "./features/book/book.module";
import {HttpClientModule} from "@angular/common/http";
import {DeliveryModule} from "./features/delivery/delivery.module";
import {LendModule} from "./features/lend/lend.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule,
    routing,
    BookModule,
    DeliveryModule,
    LendModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
