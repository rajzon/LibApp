import {Injectable} from "@angular/core";
import {
  ActiveDeliveryState
} from "./state/active-delivery-state.service";
import {Observable, of} from "rxjs";
import {DeliveryApiService} from "./api/delivery-api.service";
import {catchError, map} from "rxjs/operators";
import {MessagePopupService} from "@core/services/message-popup.service";
import {ActiveDeliveriesResultDto} from "./models/active-deliveries-result-dto";

@Injectable({
  providedIn: 'root'
})
export class DeliveryFacade {

  constructor(private activeDeliveryState: ActiveDeliveryState,
              private deliveryApi: DeliveryApiService,
              private messagePopup: MessagePopupService) {
  }

  isLoading$(): Observable<boolean> {
    return this.activeDeliveryState.isLoading$();
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

}
