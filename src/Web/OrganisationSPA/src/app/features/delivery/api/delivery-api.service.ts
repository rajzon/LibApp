import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "@env";
import {ActiveDeliveriesResultDto} from "../models/active-deliveries-result-dto";

@Injectable({
  providedIn: 'root'
})
export class DeliveryApiService {

  private readonly API: string = environment.stockDeliveryApiUrl + 'v1/delivery'

  constructor(private httpClient: HttpClient) { }


  getActiveDeliveries(currentPage: number, pageSize: number) : Observable<ActiveDeliveriesResultDto> {
    let params = new HttpParams();
    params = params.append("currentPage", currentPage.toString())
    params = params.append("pageSize", pageSize.toString())

    return this.httpClient.get<ActiveDeliveriesResultDto>(this.API + "/active", {params: params});
  }
}
