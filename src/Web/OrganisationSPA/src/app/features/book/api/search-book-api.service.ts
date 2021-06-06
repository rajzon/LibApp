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
    for (let i = 0; i < query.categories?.length; i++) {
      params = params.append('categories', query.categories[i])
    }

    for (let i = 0; i < query.authors?.length; i++) {
      params = params.append('authors', query.authors[i])
    }

    for (let i = 0; i < query.languages?.length; i++) {
      params = params.append('languages', query.languages[i])
    }

    for (let i = 0; i < query.publishers?.length; i++) {
      params = params.append('publishers', query.publishers[i])
    }

    for (let i = 0; i < query.visibility?.length; i++) {
      params = params.append('visibility', query.visibility[i].toString())
    }

    params = query.sortBy ? params.append('sortBy', query.sortBy): params
    params = query.fromPage ? params.append('fromPage', query.fromPage.toString()): params
    params = query.PageSize ? params.append('pageSize', query.PageSize.toString()): params
    params = query.modificationDateFrom ? params.append('modificationDateFrom', modificationDateFrom): params
    params = query.modificationDateTo ? params.append('modificationDateTo', modificationDateTo): params

    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {'searchTerm' : params.get('searchTerm'), 'fromPage': params.get('fromPage'), 'pageSize': params.get('pageSize'),
      'sortBy': params.get('sortBy'), 'categories': params.getAll('categories'), 'authors': params.getAll('authors'), 'languages': params.getAll('languages'),
        'publishers': params.getAll('publishers'), 'visibility': params.getAll('visibility'), 'modificationDateFrom': params.get('modificationDateFrom'), 'modificationDateTo': params.get('modificationDateTo'),
      },
      queryParamsHandling: "merge"
    });
    return this.http.get<SearchBookResultDto>(this.API, {params: params});
  }



}

