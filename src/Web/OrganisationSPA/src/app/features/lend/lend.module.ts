import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LendBasketComponent } from './containers/lend-basket/lend-basket.component';
import {routing} from "./lend.routing";
import {SharedModule} from "@shared/shared.module";
import { DeleteStockInLendBasketModalComponent } from './components/delete-stock-in-lend-basket-modal/delete-stock-in-lend-basket-modal.component';
import { CustomerSearchComponent } from './components/customer-search/customer-search.component';
import {TypeaheadModule} from "ngx-bootstrap/typeahead";



@NgModule({
  declarations: [LendBasketComponent, DeleteStockInLendBasketModalComponent, CustomerSearchComponent],
  imports: [
    CommonModule,
    routing,
    SharedModule,
    TypeaheadModule
  ]
})
export class LendModule { }
