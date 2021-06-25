import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeliveryComponent } from './containers/delivery/delivery.component';
import { DeliveryPendingListComponent } from './components/delivery-pending-list/delivery-pending-list.component';
import {SharedModule} from "@shared/shared.module";
import {routing} from "./delivery.routing";



@NgModule({
  declarations: [DeliveryComponent, DeliveryPendingListComponent],
  imports: [
    CommonModule,
    routing,
    SharedModule,
  ]
})
export class DeliveryModule { }

