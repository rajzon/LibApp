import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-book-api-search-result',
  templateUrl: './book-api-search-result.component.html',
  styleUrls: ['./book-api-search-result.component.sass']
})
export class BookApiSearchResultComponent implements OnInit {

  @Input() books: any[];

  constructor() { }

  ngOnInit(): void {
  }

}
