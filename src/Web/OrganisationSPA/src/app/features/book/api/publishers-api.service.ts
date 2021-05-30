import {Injectable} from '@angular/core';
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Publisher} from "../models/publisher";

@Injectable({
  providedIn: 'root'
})
export class PublishersApiService {

  readonly API: string = environment.bookApiUrl + 'v1/publisher';

  constructor(private http: HttpClient) { }

  getPublishers$(): Observable<Publisher[]> {
    return this.http.get<Publisher[]>(this.API);
  }
}
