import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {Category} from "../models/category";

@Injectable({
  providedIn: "root"
})
export class BookState {

  private bookCategories$ = new BehaviorSubject<Category[]>(null);

  getBookCategories$() {
    return this.bookCategories$.asObservable();
  }

  setBookCategories(bookCategories: Category[]) {
    return this.bookCategories$.next(bookCategories)
  }
}
