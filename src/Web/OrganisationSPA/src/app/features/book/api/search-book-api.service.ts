import {Injectable} from '@angular/core';
import {environment} from "@env";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {SearchBookQueryDto} from "../models/search-book-query-dto";
import {ActivatedRoute, Router} from "@angular/router";
import {SearchBookResultDto} from "../models/search-book-result-dto";

@Injectable({
  providedIn: 'root'
})
export class SearchBookApiService {

  readonly API: string = environment.searchApiUrl + 'v1/Search/book/management';

  constructor(private http: HttpClient, private router: Router, private activatedRoute: ActivatedRoute) { }

  searchBook$(query: SearchBookQueryDto): Observable<SearchBookResultDto> {
    const modificationDateFrom = query.modificationDateFrom?.getFullYear() + '-' + (query.modificationDateFrom?.getMonth() + 1) + '-' + query.modificationDateFrom?.getDate();
    const modificationDateTo = query.modificationDateTo?.getFullYear() + '-' + (query.modificationDateTo?.getMonth() + 1) + '-' + query.modificationDateTo?.getDate();

    let params = new HttpParams()

    params = query.searchTerm ? params.append('searchTerm', query.searchTerm): params
    params = query.categories ? params.append('categories', query.categories): params
    params = query.authors ? params.append('authors', query.authors): params
    params =  query.languages ? params.append('languages', query.languages): params
    params =  query.publishers ? params.append('publishers', query.publishers): params
    params =  query.visibility !== undefined && query.visibility !== null ? params.append('visibility', query.visibility.toString()): params

    params = query.sortBy ? params.append('sortBy', query.sortBy): params
    params = query.fromPage ? params.append('fromPage', query.fromPage.toString()): params
    params = query.PageSize ? params.append('pageSize', query.PageSize.toString()): params
    params = query.modificationDateFrom ? params.append('modificationDateFrom', modificationDateFrom): params
    params = query.modificationDateTo ? params.append('modificationDateTo', modificationDateTo): params

    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {'searchTerm' : params.get('searchTerm'), 'fromPage': params.get('fromPage'), 'pageSize': params.get('pageSize')},
      queryParamsHandling: "merge"
    });
    return this.http.get<SearchBookResultDto>(this.API, {params: params});
  }



}

