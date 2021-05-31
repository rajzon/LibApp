import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {SearchBookResultDto} from "../../models/search-book-result-dto";

@Component({
  selector: 'app-book-search',
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.sass']
})
export class BookSearchComponent implements OnInit {

  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>();

  searchTerm: string

  constructor() { }

  ngOnInit(): void {
  }

  search() {
    this.searchEvent.emit(new SearchBookQueryDto(this.searchTerm))
  }

}
