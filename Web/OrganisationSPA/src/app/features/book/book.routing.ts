import { RouterModule, Routes } from "@angular/router";
import { BookManagementComponent } from "./containers/book-management/book-management.component";
import { BookCreationComponent } from "./containers/book-creation/book-creation.component";
import { ModuleWithProviders } from "@angular/core";

export const routes: Routes = [
  { path: '', component: BookManagementComponent},
  { path: 'create', component: BookCreationComponent}
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
