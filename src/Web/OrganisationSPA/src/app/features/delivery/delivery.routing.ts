import {RouterModule, Routes} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {DeliveryComponent} from "./containers/delivery/delivery.component";

export const routes: Routes = [
  { path: 'delivery', component: DeliveryComponent},
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
