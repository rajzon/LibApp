import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "@env";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GoogleBookApiService {

  readonly URL: string = environment.googleApiUrl + 'books/v1/volumes';

  constructor(private http: HttpClient) { }

  getBooks$(query: string, searchParam: 'title' | 'author' | 'isbn'): Observable<any[]> {
    return this.http.get<any[]>(this.URL + `?q=${query}+${searchParam}`);
  }
}
