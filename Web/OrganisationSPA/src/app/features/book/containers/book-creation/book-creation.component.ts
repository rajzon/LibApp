import { Component, OnInit } from '@angular/core';
import {Observable} from "rxjs";
import { BookFacade } from '../../book.facade';
import {Category} from "../../models/category";
import {Language} from "../../models/language";
import {Author} from "../../models/author";
import {Publisher} from "../../models/publisher";

@Component({
  selector: 'app-book-creation',
  templateUrl: './book-creation.component.html',
  styleUrls: ['./book-creation.component.sass']
})
export class BookCreationComponent implements OnInit {

  categories$: Observable<Category[]>;
  languages$: Observable<Language[]>;
  authors$: Observable<Author[]>;
  publishers$: Observable<Publisher[]>;

  constructor(private bookFacade: BookFacade) {
    this.categories$ = bookFacade.getCategories$();
    this.languages$ = bookFacade.getLanguages$();
    this.authors$ = bookFacade.getAuthors$();
    this.publishers$ = bookFacade.getPublisher$();
  }

  ngOnInit(): void {
    this.bookFacade.loadCategories$().subscribe();
    this.bookFacade.loadLanguages$().subscribe();
    this.bookFacade.loadAuthors$().subscribe();
    this.bookFacade.loadPublishers$().subscribe();
  }


}
