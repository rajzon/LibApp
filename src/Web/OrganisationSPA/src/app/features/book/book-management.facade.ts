import {Injectable} from "@angular/core";
import {SearchBookApiService} from "./api/search-book-api.service";
import {Observable, of} from "rxjs";
import {BookManagementState} from "./state/book-management.state";
import {catchError, map} from "rxjs/operators";
import {MessagePopupService} from "@core/services/message-popup.service";
import {SearchBookQueryDto} from "./models/search-book-query-dto";
import {SearchBookResultDto} from "./models/search-book-result-dto";

@Injectable({
  providedIn: 'root'
})
export class BookManagementFacade {

  constructor(private searchBookApi: SearchBookApiService,
              private bookManagementState: BookManagementState,
              private messagePopup: MessagePopupService) {
  }

  searchBook$(query: SearchBookQueryDto): Observable<SearchBookResultDto> {
    return this.searchBookApi.searchBook$(query).pipe(map((res: SearchBookResultDto) => {
      this.bookManagementState.setSearchBookResult(res);
      this.bookManagementState.setBooksInList(res.results);

      return res
    }), catchError((err, caught) => {
      this.messagePopup.displayError(err);
      return of(err)
    }))
  }


  setHttpQueryParams(searchQueryParams: string): void {
    this.bookManagementState.setHttpSearchQueryParams(searchQueryParams);
  }


  getHttpQueryParams(): Observable<string> {
    return this.bookManagementState.getHttpSearchQueryParams();
  }
}
