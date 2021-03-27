import { Component, OnInit } from '@angular/core';
import {Observable} from "rxjs";
import { BookFacade } from '../../book.facade';
import {Category} from "../../models/category";

@Component({
  selector: 'app-book-creation',
  templateUrl: './book-creation.component.html',
  styleUrls: ['./book-creation.component.sass']
})
export class BookCreationComponent implements OnInit {

  bookCategories$: Observable<Category[]>;

  constructor(private bookFacade: BookFacade) {
    this.bookCategories$ = bookFacade.getBookCategories$();
  }

  ngOnInit(): void {
    this.bookFacade.loadBookCategories().subscribe();
  }


}
