import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "@env";
import {Author} from "../models/author";

@Injectable({
  providedIn: 'root'
})
export class AuthorsApiService {

  private readonly API: string = environment.bookApiUrl + 'v1/author'

  constructor(private http: HttpClient) { }

  getAuthors$(): Observable<Author[]> {
    return this.http.get<Author[]>(this.API);
  }
}
