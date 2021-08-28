import {RouterModule, Routes} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {DeliveryComponent} from "./containers/delivery/delivery.component";
import {DeliveryCreateComponent} from "./components/delivery-create/delivery-create.component";
import {AuthGuard} from "@core/guards/auth.guard";
import {RedeemDeliveryComponent} from "./components/redeem-delivery/redeem-delivery.component";

export const routes: Routes = [
  { path: 'delivery', component: DeliveryComponent,
    data: {deleteActiveDeliveryClaims: [{delivery_privilege: ["create-delete", "full"] }], deleteActiveDeliveryRoles: ['admin', 'employee'],
    addNewDeliveryClaims: [{delivery_privilege: ["create-delete", "full"] }], addNewDeliveryRoles: ['admin', 'employee'],
      redeemDeliveryClaims: [{delivery_privilege: ["redeem", "edit", "create-delete", "full"] }], redeemDeliveryRoles: ['admin', 'employee']
  }
  },
  { path: 'create', component: DeliveryCreateComponent, canActivate: [AuthGuard],
    data: {claims: [{delivery_privilege: ["create-delete", "full"] }], roles: ['admin', 'employee']}},
  { path: 'redeem/:id', component: RedeemDeliveryComponent, canActivate: [AuthGuard],
    data: {claims: [{delivery_privilege: ["redeem", "edit", "create-delete", "full"] }], roles: ['admin', 'employee']}
  }
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
