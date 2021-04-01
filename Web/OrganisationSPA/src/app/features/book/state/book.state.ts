import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {Category} from "../models/category";
import {Language} from "../models/language";
import {Author} from "../models/author";
import {Publisher} from "../models/publisher";
import {Book} from "../models/book";
import {FileUploader} from "ng2-file-upload";

@Injectable({
  providedIn: "root"
})
export class BookState {


  private adding$ = new BehaviorSubject<boolean>(false);

  private categories$ = new BehaviorSubject<Category[]>(null);
  private languages$ = new BehaviorSubject<Language[]>(null);
  private authors$ = new BehaviorSubject<Author[]>(null);
  private publishers$ = new BehaviorSubject<Publisher[]>(null);
  private newlyAddedBook$ = new BehaviorSubject<Book>(null);

  isAdding$() {
    return this.adding$.asObservable();
  }

  setAdding(isAdding: boolean) : void {
    this.adding$.next(isAdding);
  }

  //Book
  getNewlyAddedBook$() {
    return this.newlyAddedBook$.asObservable();
  }

  setBook(book: Book): void {
    this.newlyAddedBook$.next(book);
  }


  //Categories
  getCategories$() {
    return this.categories$.asObservable();
  }

  setCategories(categories: Category[]): void {
    return this.categories$.next(categories)
  }

  //Languages
  getLanguages$() {
    return this.languages$.asObservable();
  }

  setLanguages(languages: Language[]): void {
    return this.languages$.next(languages);
  }

  //Authors
  getAuthors$() {
    return this.authors$.asObservable();
  }

  setAuthors(authors: Author[]): void {
    return this.authors$.next(authors);
  }

  //Publishers
  getPublishers$() {
    return this.publishers$.asObservable();
  }

  setPublishers(publishers: Publisher[]): void {
    return this.publishers$.next(publishers);
  }



}
