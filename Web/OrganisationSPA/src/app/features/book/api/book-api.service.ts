import { Injectable } from '@angular/core';
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Book} from "../models/book";
import {CreateManualBookDto} from "../models/create-manual-book-dto";
import {CreateBookUsingApiDto} from "../models/create-book-using-api-dto";

@Injectable({
  providedIn: 'root'
})
export class BookApiService {

  readonly API: string = environment.bookApiUrl + 'v1/book';

  constructor(private http: HttpClient) { }

  createBook(book: CreateManualBookDto): Observable<Book> {
    return this.http.post<Book>(this.API + '/add-manual', book);
  }

  createBookUsingApi(book: CreateBookUsingApiDto): Observable<Book> {
    return this.http.post<Book>(this.API +'/add', book);
  }
}
