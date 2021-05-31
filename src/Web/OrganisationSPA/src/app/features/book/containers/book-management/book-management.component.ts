import { Component, OnInit } from '@angular/core';
import {BookManagementFacade} from "../../book-management.facade";
import {Observable} from "rxjs";
import {SearchBookApiService} from "../../api/search-book-api.service";
import {ActivatedRoute} from "@angular/router";
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {DatePipe} from "@angular/common";
import {SearchBookResultDto} from "../../models/search-book-result-dto";

@Component({
  selector: 'app-book-management',
  templateUrl: './book-management.component.html',
  styleUrls: ['./book-management.component.sass']
})
export class BookManagementComponent implements OnInit {

  private searchQueryFromHttp: SearchBookQueryDto

  searchBookResult$: Observable<SearchBookResultDto>

  constructor(private route: ActivatedRoute,
              private bookManagementFacade: BookManagementFacade) {

  }

  ngOnInit(): void {
    this.searchQueryFromHttp = new SearchBookQueryDto(
      this.route.snapshot.queryParamMap.get('searchTerm'), this.route.snapshot.queryParamMap.get('categories'),
      this.route.snapshot.queryParamMap.get('authors'), this.route.snapshot.queryParamMap.get('languages'),
      this.route.snapshot.queryParamMap.get('publishers'),this.route.snapshot.queryParamMap.get('visibility')?
        (this.route.snapshot.queryParamMap.get('visibility') === 'true')
          : null, this.route.snapshot.queryParamMap.get('sortBy'), JSON.parse(this.route.snapshot.queryParamMap.get('fromPage')),
      JSON.parse(this.route.snapshot.queryParamMap.get('pageSize')),
      this.route.snapshot.queryParamMap.get('modificationDateFrom')? new Date (this.route.snapshot.queryParamMap.get('modificationDateFrom')): null,
      this.route.snapshot.queryParamMap.get('modificationDateTo')? new Date (this.route.snapshot.queryParamMap.get('modificationDateTo')): null);

    this.searchBook();
  }


  searchBook(query: SearchBookQueryDto = this.searchQueryFromHttp) {
    console.log(query)
    console.log(this.route.snapshot.queryParamMap.get('searchTerm'))
    console.log(this.route.snapshot.queryParamMap.get('categories'))

    this.searchBookResult$ = this.bookManagementFacade.searchBook$(query)
  }

}
