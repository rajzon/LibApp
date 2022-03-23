import { Injectable } from '@angular/core';
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable, throwError} from "rxjs";
import {Book} from "../models/book";
import {CreateManualBookDto} from "../models/create-manual-book-dto";
import {BookToCreateDto} from "../models/create-book-using-api-dto";

@Injectable({
  providedIn: 'root'
})
export class BookApiService {

  private readonly API: string = environment.bookApiUrl + 'v1/book';

  constructor(private http: HttpClient) { }

  createBook(book: CreateManualBookDto): Observable<Book> {
    return this.http.post<Book>(this.API + '/add-manual', book);
  }

  createBookUsingApi(book: BookToCreateDto): Observable<Book> {
    return this.http.post<Book>(this.API +'/add', book);
  }
}
