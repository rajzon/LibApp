import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "@env";
import {ActiveDeliveriesResultDto} from "../models/active-deliveries-result-dto";
import {ActiveDeliveryResultDto} from "../models/active-delivery-result-dto";
import {ActiveDeliveryScanResultDto} from "../models/active-delivery-scan-result-dto";

export class ActiveDeliveryItemForCreationDto {
  bookId: number
  bookEan: string
  itemsCount: number

  constructor(bookId: number, bookEan: string, itemsCount: number) {
    this.bookId = bookId
    this.bookEan = bookEan
    this.itemsCount = itemsCount
  }
}

export class CreateActiveDeliveryCommand {
  name: string
  itemsInfo: ActiveDeliveryItemForCreationDto[]
}

export class ScanItemDeliveryCommand {
  scanMode: boolean
  requestedEan: string

  constructor(requestedEan: string, scanMode: boolean) {
    this.scanMode = scanMode;
    this.requestedEan = requestedEan;
  }
}

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

  deleteActiveDelivery$(deliveryId: number, cancellationReason: string): Observable<any> {
    let params = new HttpParams();
    params = params.append("cancellationReason", cancellationReason)
    return this.httpClient.delete<any>(this.API + `/active/delete/${deliveryId}`, {params: params})
  }

  getActiveDelivery(deliveryId: number): Observable<ActiveDeliveryResultDto> {
    return this.httpClient.get<any>(this.API + `/active/${deliveryId}`)
  }

  addNewActiveDelivery(activeDeliveryCommand: CreateActiveDeliveryCommand): Observable<any> {
    return this.httpClient.post<any>(this.API + '/active/create', activeDeliveryCommand)
  }

  redeemDelivery(id: number): Observable<any> {
    return this.httpClient.delete(this.API + `/${id}/redeem`);
  }

  scanDeliveryForRedeem(id: number, command: ScanItemDeliveryCommand) : Observable<ActiveDeliveryScanResultDto> {
    return this.httpClient.post<ActiveDeliveryScanResultDto>(this.API + `/${id}/scan`, command);
  }
}
