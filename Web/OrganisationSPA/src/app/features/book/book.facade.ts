import {Injectable} from "@angular/core";
import {BookCategoryApiService} from "./api/book-category-api.service";
import {Observable} from "rxjs";
import {Category} from "./models/category";
import {BookState} from "./state/book.state";
import {map, tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class BookFacade {

  constructor(private bookCategoryApi: BookCategoryApiService, private bookState: BookState) { }

  getBookCategories$(): Observable<Category[]> {
    return this.bookState.getBookCategories$();
  }

  loadBookCategories(): Observable<Category[]> {
    return this.bookCategoryApi.getBookCategories().pipe(map(categories => {
      this.bookState.setBookCategories(categories);
      return categories
    }));

  }

}
