import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./features/home/containers/home/home.component";
import {ModuleWithProviders} from "@angular/core";

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home'},
  { path: 'home', component: HomeComponent},
  {
    path: 'book',
    loadChildren: './features/book/book.module#BookModule'
  },
  {
    path: 'delivery',
    loadChildren: './features/delivery/delivery.module#DeliveryModule'
  },
  { path: '**', redirectTo: 'home'},
]

export const routing: ModuleWithProviders<any> = RouterModule.forRoot(routes);
