import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {SearchDto} from "../../models/search-dto";
import {environment} from "@env";

@Component({
  selector: 'app-book-api-add',
  templateUrl: './book-api-add.component.html',
  styleUrls: ['./book-api-add.component.sass']
})
export class BookApiAddComponent implements OnInit {

  @Input() booksFromSearch: any[];
  @Output() searchEvent = new EventEmitter<SearchDto>()

  searchValue: string;
  searchParam: 'intitle' | 'inauthor' | 'isbn'
  maxResults: number = environment.pagination.itemsPerPageDefault

  constructor() { }

  ngOnInit(): void {
  }

  search(value: SearchDto) {
    this.searchEvent.emit(value);
  }

}
