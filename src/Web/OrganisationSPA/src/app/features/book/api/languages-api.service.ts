import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "@env";
import {Language} from "../models/language";

@Injectable({
  providedIn: 'root'
})
export class LanguagesApiService {

  readonly API: string = environment.bookApiUrl + 'v1/language';

  constructor(private http: HttpClient) { }

  getLanguages$(): Observable<Language[]> {
    return this.http.get<Language[]>(this.API);
  }
}
