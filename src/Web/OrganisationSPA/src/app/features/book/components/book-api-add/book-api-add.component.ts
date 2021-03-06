import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GoogleSearchDto} from "../../models/google-search-dto";
import {environment} from "@env";

@Component({
  selector: 'app-book-api-add',
  templateUrl: './book-api-add.component.html',
  styleUrls: ['./book-api-add.component.sass']
})
export class BookApiAddComponent implements OnInit {

  @Output() searchEvent = new EventEmitter<GoogleSearchDto>()
  @Output() clearSearchResultEvent = new EventEmitter<boolean>();

  searchValue: string;
  searchParam: 'intitle' | 'inauthor' | 'isbn';

  constructor() { }

  ngOnInit(): void {
  }

  search(value: GoogleSearchDto) {
    this.clearSearchResultEvent.emit(true);
    this.searchEvent.emit(value);
  }

}
