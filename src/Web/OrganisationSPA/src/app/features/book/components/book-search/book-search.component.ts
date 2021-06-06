import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {SearchBookResultDto} from "../../models/search-book-result-dto";
import {ActivatedRoute} from "@angular/router";
import {getBoolFilterFromHttpQuery} from "@shared/helpers/search/get-bool-filter-from-httpQuery";

@Component({
  selector: 'app-book-search',
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.sass']
})
export class BookSearchComponent implements OnInit {

  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>();

  searchTerm: string

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
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

}
