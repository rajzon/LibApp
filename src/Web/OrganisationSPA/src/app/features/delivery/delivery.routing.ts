import {RouterModule, Routes} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {DeliveryComponent} from "./containers/delivery/delivery.component";
import {DeliveryCreateComponent} from "./components/delivery-create/delivery-create.component";
import {AuthGuard} from "@core/guards/auth.guard";

export const routes: Routes = [
  { path: 'delivery', component: DeliveryComponent,
    data: {deleteActiveDeliveryClaims: [{delivery_privilege: ["create-delete"] }], deleteActiveDeliveryRoles: ['admin', 'employee'],
    addNewDeliveryClaims: [{delivery_privilege: ["create-delete"] }], addNewDeliveryRoles: ['admin', 'employee']}
  },
  { path: 'create', component: DeliveryCreateComponent, canActivate: [AuthGuard],
    data: {claims: [{delivery_privilege: ["create-delete"] }], roles: ['admin', 'employee']} },
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
