import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeliveryComponent } from './containers/delivery/delivery.component';
import { DeliveryPendingListComponent } from './components/delivery-pending-list/delivery-pending-list.component';
import {SharedModule} from "@shared/shared.module";
import {routing} from "./delivery.routing";
import { DeliveryCreateComponent } from './components/delivery-create/delivery-create.component';
import { DeliveryEditItemCountModalComponent } from './components/delivery-edit-item-count-modal/delivery-edit-item-count-modal.component';
import { AddNewDeliveryConfirmationBoxModalComponent } from './components/add-new-delivery-confirmation-box-modal/add-new-delivery-confirmation-box-modal.component';



@NgModule({
  declarations: [DeliveryComponent, DeliveryPendingListComponent, DeliveryCreateComponent, DeliveryEditItemCountModalComponent, AddNewDeliveryConfirmationBoxModalComponent],
  imports: [
    CommonModule,
    routing,
    SharedModule,
  ]
})
export class DeliveryModule { }

