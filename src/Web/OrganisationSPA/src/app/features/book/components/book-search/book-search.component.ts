import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {SearchBookResultDto} from "../../models/search-book-result-dto";
import {ActivatedRoute} from "@angular/router";
import {getBoolFilterFromHttpQuery} from "@shared/helpers/search/get-bool-filter-from-httpQuery";
import {BookManagementFacade} from "../../book-management.facade";
import {Observable, of, Subscriber} from "rxjs";
import {mergeMap} from "rxjs/operators";
import {TypeaheadMatch} from "ngx-bootstrap/typeahead";

export class BookManagementSuggestion {
  title: string
  authors: string[]
  categories: string[]
}

@Component({
  selector: 'app-book-search',
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.sass']
})
export class BookSearchComponent implements OnInit {

  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>();

  searchTerm: string
  typeaheadLoading: boolean
  suggestions$: Observable<BookManagementSuggestion[]>
  hasSuggestions: boolean = false;

  constructor(private activatedRoute: ActivatedRoute, private bookManagementFacade: BookManagementFacade) { }

  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Subscriber<string>) => {
      observer.next(this.searchTerm)
    })
      .pipe(
        mergeMap((term: string) => term? this.bookManagementFacade.getSuggestionBook$(term): of(null))
      )
  }

  search() {
    this.searchEvent.emit(new SearchBookQueryDto(
      this.searchTerm, this.activatedRoute.snapshot.queryParamMap.getAll('categories'),
      this.activatedRoute.snapshot.queryParamMap.getAll('authors'), this.activatedRoute.snapshot.queryParamMap.getAll('languages'),
      this.activatedRoute.snapshot.queryParamMap.getAll('publishers'),
      getBoolFilterFromHttpQuery('visibility', this.activatedRoute),
      this.activatedRoute.snapshot.queryParamMap.get('sortBy'), JSON.parse(this.activatedRoute.snapshot.queryParamMap.get('fromPage')),
      JSON.parse(this.activatedRoute.snapshot.queryParamMap.get('pageSize')),
      this.activatedRoute.snapshot.queryParamMap.get('modificationDateFrom')? new Date (this.activatedRoute.snapshot.queryParamMap.get('modificationDateFrom')): null,
      this.activatedRoute.snapshot.queryParamMap.get('modificationDateTo')? new Date (this.activatedRoute.snapshot.queryParamMap.get('modificationDateTo')): null));
  }

  suggest(suggestTerm: string) :void {
    this.bookManagementFacade.getSuggestionBook$(suggestTerm).subscribe(res => {
      console.log(res);
    })
  }
  changeTypeaheadLoading(e: boolean): void {
    this.typeaheadLoading = e;
  }

  changeTypeaheadHasSuggestions(e: boolean): void {
    console.log(!e)
    this.hasSuggestions = !e
  }

  onSelectOpt(e: TypeaheadMatch): void {
    this.searchTerm = e.item.title;
  }
}
