import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "@env";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GoogleBookApiService {

  readonly URL: string = environment.googleApiUrl + 'books/v1/volumes';

  constructor(private http: HttpClient) { }

  getBooks$(query: string, searchParam: 'intitle' | 'inauthor' | 'isbn', startIndex?: number, maxResults?: number): Observable<any[]> {

    let params = new HttpParams();

    if (startIndex == null) {
      startIndex = 0;
    }

    if (maxResults == null) {
      maxResults = environment.pagination.itemsPerPageDefault;
    }

    params = params.append('startIndex', startIndex.toString());
    params = params.append('maxResults', maxResults.toString());
    // params = params.append('orderBy', 'newest');


    return this.http.get<any[]>(this.URL + `?q=${query}+${searchParam}`,{params: params});
  }
}
