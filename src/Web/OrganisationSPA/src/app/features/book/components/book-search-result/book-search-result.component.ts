import {Component, Input, OnInit} from '@angular/core';
import {SearchBookResultDto} from "../../models/search-book-result-dto";
import {Book} from "../../models/book";

@Component({
  selector: 'app-book-search-result',
  templateUrl: './book-search-result.component.html',
  styleUrls: ['./book-search-result.component.sass']
})
export class BookSearchResultComponent implements OnInit {

  @Input() searchResult: SearchBookResultDto

  constructor() { }

  ngOnInit(): void {
  }

  selectMainImage(book: Book): string {
    return book.images? book.images.find(i => i.isMain)?.url : ''
  }

  visibilityAsMeaningfulText(visibility: boolean): string {
    return visibility? 'Visible': 'Hidden'
  }

}
