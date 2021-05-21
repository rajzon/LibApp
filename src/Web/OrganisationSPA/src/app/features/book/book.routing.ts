import { RouterModule, Routes } from "@angular/router";
import { BookManagementComponent } from "./containers/book-management/book-management.component";
import { BookCreationComponent } from "./containers/book-creation/book-creation.component";
import { ModuleWithProviders } from "@angular/core";
import {AuthGuard} from "@core/guards/auth.guard";

export const routes: Routes = [
  { path: 'book', component: BookManagementComponent},
  { path: 'create', component: BookCreationComponent,
      data: {claims: [{book_privilege: ["write", "full"] }], roles: ['admin', 'employee']},
      canActivate: [AuthGuard]
  }
]

export const routing: ModuleWithProviders<any> = RouterModule.forChild(routes);
