import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookManagementComponent } from './containers/book-management/book-management.component';
import { BookCreationComponent } from './containers/book-creation/book-creation.component';
import { routing } from "./book.routing";
import { BookManualAddComponent } from './components/book-manual-add/book-manual-add.component';
// @ts-ignore
import {SharedModule} from '@shared/shared.module';
import {QuillModule} from "ngx-quill";
import { BookApiAddComponent } from './components/book-api-add/book-api-add.component';
import { BookApiSearchResultComponent } from './components/book-api-search-result/book-api-search-result.component';
import { BookApiEditModalComponent } from './components/book-api-edit-modal/book-api-edit-modal.component';


@NgModule({
  declarations: [BookManagementComponent, BookCreationComponent, BookManualAddComponent, BookApiAddComponent, BookApiSearchResultComponent, BookApiEditModalComponent],
  imports: [
    CommonModule,
    routing,
    SharedModule
  ]
})
export class BookModule { }
