﻿import {Injectable} from "@angular/core";
import {SearchBookApiService} from "./api/search-book-api.service";
import {Observable, of} from "rxjs";
import {BookManagementState} from "./state/book-management.state";
import {catchError, map} from "rxjs/operators";
import {MessagePopupService} from "@core/services/message-popup.service";
import {SearchBookQueryDto} from "./models/search-book-query-dto";
import {SearchBookResultDto} from "./models/search-book-result-dto";
import {AllowedSorting, SearchSettingsApiService} from "./api/search-settings-api.service";
import {BookManagementSuggestion} from "./components/book-search/book-search.component";

@Injectable({
  providedIn: 'root'
})
export class BookManagementFacade {

  constructor(private searchBookApi: SearchBookApiService,
              private searchSettingsApi: SearchSettingsApiService,
              private bookManagementState: BookManagementState,
              private messagePopup: MessagePopupService) {
  }

  isAdding$(): Observable<boolean> {
    return this.bookManagementState.isAdding$();
  }

  isLoading$(): Observable<boolean> {
    return this.bookManagementState.isLoading$();
  }

  searchBook$(query: SearchBookQueryDto): Observable<SearchBookResultDto> {
    this.bookManagementState.setLoading(true);
    return this.searchBookApi.searchBook$(query).pipe(map((res: SearchBookResultDto) => {
      this.bookManagementState.setSearchBookResult(res);
      this.bookManagementState.setBooksInList(res.results);

      this.bookManagementState.setLoading(false);
      return res
    }), catchError((err, caught) => {
      this.messagePopup.displayError(err);
      this.bookManagementState.setLoading(false);
      return of(err)
    }))
  }

  getSearchBookResult$(): Observable<SearchBookResultDto> {
    return this.bookManagementState.getSearchBookResult$();
  }

  getBookManagementSearchAllowedSorting$(): Observable<AllowedSorting[]> {
    return this.searchSettingsApi.getBookManagementSearchAllowedSorting$();
  }

  getSuggestionBook$(suggestTerm: string) : Observable<BookManagementSuggestion[]> {
    return this.searchBookApi.getSuggestions$(suggestTerm);
  }
}
