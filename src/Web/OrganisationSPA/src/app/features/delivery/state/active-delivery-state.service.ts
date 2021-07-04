import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {ActiveDeliveriesResultDto} from "../models/active-deliveries-result-dto";
import {ActiveDelivery} from "../models/active-delivery-dto";


@Injectable({
  providedIn: "root"
})
export class ActiveDeliveryState {

  private loading$ = new BehaviorSubject<boolean>(false);
  private adding$ = new BehaviorSubject<boolean>(false);

  private deliveriesResult$ = new BehaviorSubject<ActiveDeliveriesResultDto>(null);



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


  setDeliveries(deliveries: ActiveDeliveriesResultDto) : void {
    this.deliveriesResult$.next(deliveries)
  }

  getActiveDeliveries$() : Observable<ActiveDeliveriesResultDto> {
    return this.deliveriesResult$.asObservable();
  }

  removeActiveDelivery(activeDelivery: ActiveDelivery) {
    const currDeliveries = this.deliveriesResult$.getValue()
    currDeliveries.result = currDeliveries.result.filter(d => d.id !== activeDelivery.id)
    this.deliveriesResult$.next(currDeliveries)

  }
}
