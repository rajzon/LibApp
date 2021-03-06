import {Injectable} from '@angular/core';
// @ts-ignore
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Category} from "../models/category";

@Injectable({
  providedIn: 'root'
})
export class CategoriesApiService {

  readonly API = environment.bookApiUrl + 'v1/Category'

  constructor(private http: HttpClient) { }

  getCategories$(): Observable<Category[]> {
    return this.http.get<Category[]>(this.API);
  }
}
