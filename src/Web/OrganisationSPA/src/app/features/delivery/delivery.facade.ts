import {Injectable} from "@angular/core";
import {
  ActiveDeliveryState
} from "./state/active-delivery-state.service";
import {EMPTY, Observable, of} from "rxjs";
import {CreateActiveDeliveryCommand, DeliveryApiService, ScanItemDeliveryCommand} from "./api/delivery-api.service";
import {catchError, map} from "rxjs/operators";
import {MessagePopupService} from "@core/services/message-popup.service";
import {ActiveDeliveriesResultDto} from "./models/active-deliveries-result-dto";
import {ActiveDelivery} from "./models/active-delivery-dto";
import {SearchBookApiService} from "./api/search-book-api.service";
import {Router} from "@angular/router";
import {ActiveDeliveryResultFromSearch} from "./models/active-delivery-result-from-search";
import {ActiveDeliveryResultDto} from "./models/active-delivery-result-dto";
import {ActiveDeliveryScanResultDto} from "./models/active-delivery-scan-result-dto";

@Injectable({
  providedIn: 'root'
})
export class DeliveryFacade {

  constructor(private activeDeliveryState: ActiveDeliveryState,
              private deliveryApi: DeliveryApiService,
              private searchApi: SearchBookApiService,
              private router: Router,
              private messagePopup: MessagePopupService) {
  }

  isLoading$(): Observable<boolean> {
    return this.activeDeliveryState.isLoading$();
  }

  isAdding$(): Observable<boolean> {
    return this.activeDeliveryState.isAdding$();
  }

  isDeleting$(): Observable<boolean> {
    return this.activeDeliveryState.isDeleting$();
  }


  loadActiveDeliveries$(currentPage: number, pageSize: number): Observable<ActiveDeliveriesResultDto> {
    this.activeDeliveryState.setLoading(true);
    return this.deliveryApi.getActiveDeliveries(currentPage, pageSize).pipe(map(activeDeliveries => {
      this.activeDeliveryState.setDeliveries(activeDeliveries);

      this.activeDeliveryState.setLoading(false);
      return activeDeliveries;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.activeDeliveryState.setLoading(false);
      return of(err)
    })));
  }

  getActiveDeliveries$(): Observable<ActiveDeliveriesResultDto> {
    return this.activeDeliveryState.getActiveDeliveries$()
  }

  deleteActiveDelivery$(activeDelivery: ActiveDelivery, cancellationReason: string): Observable<any> {
    this.activeDeliveryState.setLoading(true)
    return this.deliveryApi.deleteActiveDelivery$(activeDelivery.id, cancellationReason).pipe(map(r => {
      this.activeDeliveryState.removeActiveDelivery(activeDelivery)
      this.activeDeliveryState.setLoading(false)
    }), catchError( (err => {
      this.messagePopup.displayError(err)
      this.activeDeliveryState.setLoading(false);
      return of(err)
    })));
  }

  loadDeliveryForRedeem$(deliveryId: number): Observable<ActiveDeliveryResultDto> {
    this.activeDeliveryState.setLoading(true);
    return this.deliveryApi.getActiveDelivery(deliveryId).pipe(map(activeDelivery => {
      this.activeDeliveryState.setDeliveryForRedeem(activeDelivery);
      this.activeDeliveryState.setLoading(false);
      return activeDelivery;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.activeDeliveryState.setLoading(false);
      return of(err)
    })));
  }

  getDeliveryForRedeem$(deliveryId: number) : Observable<ActiveDeliveryResultDto> {
    return this.activeDeliveryState.getDeliveryForRedeem$()
  }

  scanDeliveryForRedeem(id: number, command: ScanItemDeliveryCommand) : Observable<ActiveDeliveryScanResultDto> {
    this.activeDeliveryState.setLoading(true);
    return this.deliveryApi.scanDeliveryForRedeem(id, command).pipe(map(res => {
      console.log(res)
      this.activeDeliveryState.setLoading(false)
      return res;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.activeDeliveryState.setLoading(false);
      return of(err);
    })));
  }



  searchBookForActiveDeliveryByEan$(ean13: string): Observable<ActiveDeliveryResultFromSearch> {
    this.activeDeliveryState.setLoading(true);

    return this.searchApi.searchBookForActiveDelivery(ean13).pipe(map(book => {
      book.itemsCount = 0;
      this.activeDeliveryState.setLoading(false);
      return book;
    }), catchError( (err => {
      this.messagePopup.displayError(err);
      this.activeDeliveryState.setLoading(false);
      return EMPTY
    })));
  }

  addNewActiveDelivery(command: CreateActiveDeliveryCommand): Observable<any> {
    this.activeDeliveryState.setAdding(true);
    return this.deliveryApi.addNewActiveDelivery(command).pipe(map(res => {
      this.activeDeliveryState.setAdding(false);
      this.router.navigateByUrl('/delivery');
      return res
    }), catchError((err => {
      this.messagePopup.displayError(err);
      this.activeDeliveryState.setAdding(false);
      this.router.navigateByUrl('/delivery');
      return EMPTY;
    })));
  }

  redeemDelivery(id: number) : Observable<any> {
    this.activeDeliveryState.setDeleting(true);
    return this.deliveryApi.redeemDelivery(id).pipe(map(res => {
      this.activeDeliveryState.removeDelivery(id);
      this.activeDeliveryState.setDeleting(false);
      this.router.navigateByUrl('/delivery')
      return res;
    }), catchError((err =>  {
      this.messagePopup.displayError(err);
      return of(err);
    })))
  }

}
