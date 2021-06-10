import { Injectable } from '@angular/core';
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export class AllowedSorting {
  field: string
  sortType: 'asc' | 'desc'
  combinedValue: string
  description: string

  constructor(field: string, sortType: 'asc' | 'desc', description: string) {
    this.field = field
    this.sortType = sortType
    this.combinedValue = field + ':' + sortType
    this.description = description
  }
}

@Injectable({
  providedIn: 'root'
})
export class SearchSettingsApiService {

  readonly API = environment.searchApiUrl + 'v1/Settings/';

  constructor(private http: HttpClient) { }

  getBookManagementSearchAllowedSorting$(): Observable<AllowedSorting[]> {

    return this.http.get<AllowedSorting[]>(this.API + 'book-management/allowed-sorting')
  }
}
