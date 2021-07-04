import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "@env";
import {ActiveDeliveryResultFromSearch} from "../models/active-delivery-result-from-search";

@Injectable({
  providedIn: 'root'
})
export class SearchBookApiService {

  private readonly API: string = environment.searchApiUrl + 'v1/Search/book/delivery';

  constructor(private httpClient: HttpClient) { }


  searchBookForActiveDelivery(ean13: string): Observable<ActiveDeliveryResultFromSearch> {
    return this.httpClient.get<ActiveDeliveryResultFromSearch>(this.API + `/${ean13}`);
  }

}
