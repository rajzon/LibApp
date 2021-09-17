import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LendStateService {

  private loading$ = new BehaviorSubject<boolean>(false);
  private adding$ = new BehaviorSubject<boolean>(false);
  private deleting$ = new BehaviorSubject<boolean>(false);


  isLoading$() {
    return this.loading$.asObservable();
  }

  setLoading(isLoading: boolean): void {
    this.loading$.next(isLoading);
  }

  isAdding$() {
    return this.adding$.asObservable();
  }

  setAdding(isAdding: boolean): void {
    this.adding$.next(isAdding);
  }

  isDeleting$() {
    return this.deleting$.asObservable();
  }

  setDeleting(isDeleting: boolean): void {
    this.deleting$.next(isDeleting);
  }

}
