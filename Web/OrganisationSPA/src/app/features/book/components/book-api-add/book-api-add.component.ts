import {Component, Input, OnInit, Output, EventEmitter} from '@angular/core';

export interface SearchDto {
  searchValue: string
  searchParam: 'title' | 'author' | 'isbn'
}

@Component({
  selector: 'app-book-api-add',
  templateUrl: './book-api-add.component.html',
  styleUrls: ['./book-api-add.component.sass']
})
export class BookApiAddComponent implements OnInit {

  @Input() booksFromSearch: any[];
  @Output() searchEvent = new EventEmitter<SearchDto>()

  searchValue: string;
  searchParam: 'title' | 'author' | 'isbn'

  constructor() { }

  ngOnInit(): void {
  }

  search(value: SearchDto) {
    console.log(value);
    this.searchEvent.emit(value);
  }

}
