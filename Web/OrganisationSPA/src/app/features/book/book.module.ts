import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookManagementComponent } from './containers/book-management/book-management.component';
import { BookCreationComponent } from './containers/book-creation/book-creation.component';
import { routing } from "./book.routing";
import {TabsModule} from "ngx-bootstrap/tabs";
import { BookManualAddComponent } from './components/book-manual-add/book-manual-add.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
// @ts-ignore
import {SharedModule} from '@shared/shared.module';
import {BookCategoryApiService} from "./api/book-category-api.service";
import {BookFacade} from "./book.facade";
import {BookState} from "./state/book.state";




@NgModule({
  declarations: [BookManagementComponent, BookCreationComponent, BookManualAddComponent],
  imports: [
    CommonModule,
    routing,
    TabsModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,

  ]
})
export class BookModule { }
