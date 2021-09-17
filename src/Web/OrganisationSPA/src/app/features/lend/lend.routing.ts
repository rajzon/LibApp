import {RouterModule, Routes} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {LendBasketComponent} from "./containers/lend-basket/lend-basket.component";

export const routes: Routes = [
  { path: 'lend', component: LendBasketComponent},
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
