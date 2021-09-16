import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Observable, of, Subscriber} from "rxjs";
import {mergeMap} from "rxjs/operators";
import {TypeaheadMatch} from "ngx-bootstrap/typeahead";
import {LendFacade} from "../../lend.facade";
import {LendBasket} from "../../api/lend-api.service";
import {SuggestCustomerResult} from "../../api/search-api.service";
import {environment} from "@env";

@Component({
  selector: 'app-customer-search',
  templateUrl: './customer-search.component.html',
  styleUrls: ['./customer-search.component.sass']
})
export class CustomerSearchComponent implements OnInit {

  @Output() searchCustomerResultEvent = new EventEmitter<LendBasket>();

  searchTerm: string
  typeaheadLoading: boolean
  suggestions$: Observable<SuggestCustomerResult[]>
  hasSuggestions: boolean = false;
  lendSettings = environment.lend;

  constructor(private lendFacade: LendFacade) { }

  ngOnInit(): void {
    this.suggestions$ = new Observable((observer: Subscriber<string>) => {
      observer.next(this.searchTerm)
    })
      .pipe(
        mergeMap((term: string) => term? this.lendFacade.customersSuggest$(term): of(null))
      )
  }

  search(email: string) {
    this.lendFacade.addCustomerForBasket$(email).subscribe((res: LendBasket) => {
      this.searchCustomerResultEvent.emit(res);
    })

  }

  suggest(suggestTerm: string) :void {
    this.lendFacade.customersSuggest$(suggestTerm).subscribe(res => {
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
    this.searchTerm = e.item.email;
  }

}
