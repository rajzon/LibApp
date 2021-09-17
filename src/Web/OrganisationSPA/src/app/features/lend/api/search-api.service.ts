import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "@env";
import {Observable} from "rxjs";

export class SuggestCustomerResult {
  name: string
  surname: string
  email: string
}


@Injectable({
  providedIn: 'root'
})
export class SearchApiService {

  private readonly API: string = environment.searchApiUrl + 'v1/search/'

  constructor(private httpClient: HttpClient) { }

  customersSuggest(suggestValue: string) : Observable<SuggestCustomerResult[]> {
    return this.httpClient.get<SuggestCustomerResult[]>(this.API + `suggest/customer/${suggestValue}`)
  }
}
