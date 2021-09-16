import { Injectable } from '@angular/core';
import {environment} from "@env";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export class AuthorForBasket {
  id: number
  firstName: string
  lastName: string
  fullName: string
}

export class ImageForBasket {
  url: string
  isMain: boolean
}

export class PublisherForBasket {
  id: number
  name: string
}

export class CategoryForBasket {
  id: number
  name: string
}

export class StockWithBooksForBasket {
  stockId: number
  title: string
  ean13: string
  isbn10: string
  isbn13: string
  publishedDate: Date
  returnDate: Date

  categories: CategoryForBasket[]
  authors: AuthorForBasket[]
  publisher: PublisherForBasket
  image: ImageForBasket
}


export class AddressCorrespondenceBasket {
  adres: string
  city: string
  postCode: string
  post: string
  country: string
}

export class AddressBasket {
  adres: string
  city: string
  postCode: string
  post: string
  country: string
}

export enum IdentityType {
  PersonIdCard,
  Passport
}

export class CustomerForBasket {
  name: string
  surname: string
  email: string
  personIdCard: string
  identityType: IdentityType
  nationality: string
  phone: number
  dateOfBirth: string
  address: AddressBasket
  correspondenceAddress: AddressCorrespondenceBasket
}

export class LendBasket {
  customer: CustomerForBasket
  stockWithBooks: StockWithBooksForBasket[]

  businessErrors: string[]
  businessWarnings: string[]
}

@Injectable({
  providedIn: 'root'
})
export class LendApiService {

  private readonly API: string = environment.lendApiUrl + 'v1/lend/'

  constructor(private httpClient: HttpClient) { }

  getBasketForLend() : Observable<LendBasket> {
    return this.httpClient.get<LendBasket>(this.API + "basket");
  }

  addStockForBasket(stockId: number): Observable<LendBasket> {
    return this.httpClient.post<LendBasket>(this.API + `basket/stock/${stockId}`, { });
  }

  addCustomerForBasket(email: string): Observable<LendBasket> {
    return this.httpClient.post<LendBasket>(this.API + `basket/customer/${email}`, { });
  }

  lendBasket(): Observable<any> {
    return this.httpClient.post<LendBasket>(this.API + `basket/lend`, { });
  }

  editReturnDateForStockInBasket(stockId: number, returnDate: Date | string): Observable<LendBasket> {
    return this.httpClient.put<LendBasket>(this.API + `basket/stock/${stockId}/returnDate/${returnDate}`, { });
  }

  deleteStockInBasket(stockId: number): Observable<LendBasket> {
    return this.httpClient.delete<LendBasket>(this.API + `basket/stock/${stockId}`);
  }
}
