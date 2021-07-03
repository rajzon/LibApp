import {RouterModule, Routes} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {DeliveryComponent} from "./containers/delivery/delivery.component";

export const routes: Routes = [
  { path: 'delivery', component: DeliveryComponent,
    data: {deleteActiveDeliveryClaims: [{delivery_privilege: ["create-delete"] }], deleteActiveDeliveryRoles: ['admin', 'employee']}
  }
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
