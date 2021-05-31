import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {Book} from "../models/book";
import {SearchResultDto} from "@core/models/search-result-dto";
import {SearchBookResultDto} from "../models/search-book-result-dto";

@Injectable({
  providedIn: "root"
})
export class BookManagementState {

  private searchBookResult$ = new BehaviorSubject<SearchBookResultDto>(null);
  private booksInList$ = new BehaviorSubject<Book[]>(null);
  private httpSearchQueryParams$ = new BehaviorSubject<string>(null);

  /////Search
  ///
  setSearchBookResult(searchResult: SearchBookResultDto): void {
    this.searchBookResult$.next(searchResult);
  }

  getSearchBookResult$(): Observable<SearchBookResultDto> {
    return this.searchBookResult$.asObservable();
  }

  /////HttpSearchQueryParams
  ///
  setHttpSearchQueryParams(searchQueryParams: string): void {
    this.httpSearchQueryParams$.next(searchQueryParams);
  }

  getHttpSearchQueryParams(): Observable<string> {
    return this.httpSearchQueryParams$.asObservable();
  }

  /////Book
  ///
  setBooksInList(books: Book[]): void {
    this.booksInList$.next(books);
  }

  getBooksInList$(): Observable<Book[]> {
    return this.booksInList$.asObservable();
  }


}
