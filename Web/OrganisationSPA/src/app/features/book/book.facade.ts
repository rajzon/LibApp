﻿import {Injectable} from "@angular/core";
import {CategoriesApiService} from "./api/categories-api.service";
import {Observable} from "rxjs";
import {Category} from "./models/category";
import {BookState} from "./state/book.state";
import {map} from "rxjs/operators";
import {LanguagesApiService} from "./api/languages-api.service";
import {Language} from "./models/language";
import {AuthorsApiService} from "./api/authors-api.service";
import {PublishersApiService} from "./api/publishers-api.service";
import {Author} from "./models/author";
import {Publisher} from "./models/publisher";

@Injectable({
  providedIn: 'root'
})
export class BookFacade {

  constructor(private categoriesApi: CategoriesApiService,
              private languagesApi: LanguagesApiService,
              private authorsApi: AuthorsApiService,
              private publishersApi: PublishersApiService,
              private bookState: BookState) { }

  //Categories
  getCategories$(): Observable<Category[]> {
    return this.bookState.getCategories$();
  }

  loadCategories$(): Observable<Category[]> {
    return this.categoriesApi.getCategories$().pipe(map(categories => {
      this.bookState.setCategories(categories);
      return categories
    }));

  }

  //Languages
  getLanguages$(): Observable<Language[]> {
    return this.bookState.getLanguages$();
  }

  loadLanguages$(): Observable<Language[]> {
    return this.languagesApi.getLanguages$().pipe(map(languages => {
      this.bookState.setLanguages(languages);
      return languages
    }));

  }

  //Authors
  getAuthors$(): Observable<Author[]> {
    return this.bookState.getAuthors$();
  }

  loadAuthors$(): Observable<Author[]> {
    return this.authorsApi.getAuthors$().pipe(map(authors => {
     authors = authors.map(author => ({
        ...author,
        fullName: `${author.firstName} ${author.lastName}`
      }));
      this.bookState.setAuthors(authors);
      return authors;
    }))
  }

  //Publishers
  getPublisher$(): Observable<Publisher[]> {
    return this.bookState.getPublishers$();
  }

  loadPublishers$(): Observable<Publisher[]> {
    return this.publishersApi.getPublishers$().pipe(map(publishers => {
      this.bookState.setPublishers(publishers);
      return publishers;
    }))
  }

}
