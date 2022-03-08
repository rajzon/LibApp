import {Injectable} from "@angular/core";
import {LendApiService, LendBasket} from "./api/lend-api.service";
import {EMPTY, Observable, of} from "rxjs";
import {LendStateService} from "./state/lend-state.service";
import {catchError, map} from "rxjs/operators";
import {MessagePopupService} from "@core/services/message-popup.service";
import {SearchApiService, SuggestCustomerResult} from "./api/search-api.service";

@Injectable({
  providedIn: 'root'
})
export class LendFacade {

  constructor(private lendApi: LendApiService,
              private searchApi: SearchApiService,
              private lendState: LendStateService, private messagePopup: MessagePopupService) {
  }

  isLoading$(): Observable<boolean> {
    return this.lendState.isLoading$();
  }

  isAdding$(): Observable<boolean> {
    return this.lendState.isAdding$();
  }

  isDeleting$(): Observable<boolean> {
    return this.lendState.isDeleting$();
  }

  getBasketForLend$() : Observable<LendBasket> {
    this.lendState.setLoading(true)
    return this.lendApi.getBasketForLend().pipe(map(result => {
      this.lendState.setLoading(false);
      var date = new Date(result.customer.dateOfBirth)
      result.customer.dateOfBirth= date.toLocaleDateString()

      for (var i = 0; i < result.stockWithBooks.length; i++)
      {
        result.stockWithBooks[i].returnDate = new Date(result.stockWithBooks[i].returnDate)
      }
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setLoading(false);
      return of(err);
    })));
  }

  addStockForBasket$(stockId: number) : Observable<LendBasket> {
    this.lendState.setAdding(true)
    return this.lendApi.addStockForBasket(stockId).pipe(map(result => {
      this.lendState.setAdding(false);
      var date = new Date(result.customer.dateOfBirth)
      result.customer.dateOfBirth= date.toLocaleDateString()

      for (var i = 0; i < result.stockWithBooks.length; i++)
      {
        result.stockWithBooks[i].returnDate = new Date(result.stockWithBooks[i].returnDate)
      }
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setAdding(false);
      return EMPTY;
    })));
  }

  addCustomerForBasket$(email: string) : Observable<LendBasket> {
    this.lendState.setAdding(true)
    return this.lendApi.addCustomerForBasket(email).pipe(map(result => {
      this.lendState.setAdding(false);
      var date = new Date(result.customer.dateOfBirth)
      result.customer.dateOfBirth= date.toLocaleDateString()

      for (var i = 0; i < result.stockWithBooks.length; i++)
      {
        result.stockWithBooks[i].returnDate = new Date(result.stockWithBooks[i].returnDate)
      }
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setAdding(false);
      return EMPTY;
    })));
  }

  customersSuggest$(suggestValue: string) : Observable<SuggestCustomerResult[]> {
    this.lendState.setAdding(true)
    return this.searchApi.customersSuggest(suggestValue).pipe(map(result => {
      this.lendState.setAdding(false);
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setAdding(false);
      return of(err);
    })));
  }

  lendBasket$() : Observable<LendBasket> {
    this.lendState.setAdding(true)
    return this.lendApi.lendBasket().pipe(map(result => {
      this.lendState.setAdding(false);
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setAdding(false);
      return EMPTY;
    })));
  }

  editReturnDateForStockInBasket$(stockId: number, returnDate: Date | string) : Observable<LendBasket> {
    this.lendState.setAdding(true)
    return this.lendApi.editReturnDateForStockInBasket(stockId, returnDate).pipe(map(result => {
      this.lendState.setAdding(false);
      var date = new Date(result.customer.dateOfBirth)
      result.customer.dateOfBirth= date.toLocaleDateString()

      for (var i = 0; i < result.stockWithBooks.length; i++)
      {
        result.stockWithBooks[i].returnDate = new Date(result.stockWithBooks[i].returnDate)
      }
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setAdding(false);
      return EMPTY;
    })));
  }

  deleteStockInBasket$(stockId: number, ean: string) : Observable<LendBasket> {
    this.lendState.setDeleting(true)
    return this.lendApi.deleteStockInBasket(stockId, ean).pipe(map(result => {
      this.lendState.setDeleting(false);
      var date = new Date(result.customer.dateOfBirth)
      result.customer.dateOfBirth= date.toLocaleDateString()

      for (var i = 0; i < result.stockWithBooks.length; i++)
      {
        result.stockWithBooks[i].returnDate = new Date(result.stockWithBooks[i].returnDate)
      }
      return result;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.lendState.setDeleting(false);
      return EMPTY;
    })));
  }
}
