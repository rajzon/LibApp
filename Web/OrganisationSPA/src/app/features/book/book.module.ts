import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookManagementComponent } from './containers/book-management/book-management.component';
import { BookCreationComponent } from './containers/book-creation/book-creation.component';
import { routing } from "./book.routing";



@NgModule({
  declarations: [BookManagementComponent, BookCreationComponent],
  imports: [
    CommonModule,
    routing
  ]
})
export class BookModule { }
