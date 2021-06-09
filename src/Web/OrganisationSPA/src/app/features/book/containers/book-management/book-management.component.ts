import {AfterViewInit, Component, Inject, LOCALE_ID, OnInit} from '@angular/core';
import {BookManagementFacade} from "../../book-management.facade";
import {Observable} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {SearchBookResultDto} from "../../models/search-book-result-dto";
import {NgxSpinnerService} from "ngx-spinner";
import {getBoolFilterFromHttpQuery} from "@shared/helpers/search/get-bool-filter-from-httpQuery";
import {map} from "rxjs/operators";
import {formatDate} from "@angular/common";

@Component({
  selector: 'app-book-management',
  templateUrl: './book-management.component.html',
  styleUrls: ['./book-management.component.sass']
})
export class BookManagementComponent implements OnInit, AfterViewInit {

  private searchQueryFromHttp: SearchBookQueryDto

  isAdding$: Observable<boolean>
  isLoading$: Observable<boolean>


  searchBookResult$: Observable<SearchBookResultDto>

  constructor(@Inject(LOCALE_ID) private locale: string,
              private route: ActivatedRoute,
              private bookManagementFacade: BookManagementFacade,
              private spinner: NgxSpinnerService) {
    this.isAdding$ = this.bookManagementFacade.isAdding$();
    this.isLoading$ = this.bookManagementFacade.isLoading$();


  }

  ngAfterViewInit(): void {
    this.spinner.show();
  }

  ngOnInit(): void {
    this.searchQueryFromHttp = new SearchBookQueryDto(
      this.route.snapshot.queryParamMap.get('searchTerm'), this.route.snapshot.queryParamMap.getAll('categories'),
      this.route.snapshot.queryParamMap.getAll('authors'), this.route.snapshot.queryParamMap.getAll('languages'),
      this.route.snapshot.queryParamMap.getAll('publishers'),
      getBoolFilterFromHttpQuery('visibility', this.route),
      this.route.snapshot.queryParamMap.get('sortBy'), JSON.parse(this.route.snapshot.queryParamMap.get('fromPage')),
      JSON.parse(this.route.snapshot.queryParamMap.get('pageSize')),
      this.route.snapshot.queryParamMap.get('modificationDateFrom')? new Date (this.route.snapshot.queryParamMap.get('modificationDateFrom')): null,
      this.route.snapshot.queryParamMap.get('modificationDateTo')? new Date (this.route.snapshot.queryParamMap.get('modificationDateTo')): null);

    this.searchBook();
  }



  searchBook(query: SearchBookQueryDto = this.searchQueryFromHttp) {
    console.log(query)
    console.log(this.route.snapshot.queryParamMap.get('searchTerm'))
    console.log(this.route.snapshot.queryParamMap.get('categories'))

    this.searchBookResult$ = this.bookManagementFacade.searchBook$(query).pipe(map(res => {
      res.results.map(r => {
        r.modificationDate = formatDate(r.modificationDate, 'dd-MM-yyyy', this.locale)
      })
      return res;
    }))
  }

}
