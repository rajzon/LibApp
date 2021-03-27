import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookManagementComponent } from './containers/book-management/book-management.component';
import { BookCreationComponent } from './containers/book-creation/book-creation.component';
import { routing } from "./book.routing";
import { BookManualAddComponent } from './components/book-manual-add/book-manual-add.component';
// @ts-ignore
import {SharedModule} from '@shared/shared.module';


@NgModule({
  declarations: [BookManagementComponent, BookCreationComponent, BookManualAddComponent],
  imports: [
    CommonModule,
    routing,
    SharedModule,
  ]
})
export class BookModule { }
