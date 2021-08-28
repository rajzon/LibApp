import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {ActiveDeliveriesResultDto} from "../models/active-deliveries-result-dto";
import {ActiveDelivery} from "../models/active-delivery-dto";
import {ActiveDeliveryResultDto} from "../models/active-delivery-result-dto";


@Injectable({
  providedIn: "root"
})
export class ActiveDeliveryState {

  private loading$ = new BehaviorSubject<boolean>(false);
  private adding$ = new BehaviorSubject<boolean>(false);
  private deleting$ = new BehaviorSubject<boolean>(false);

  private deliveriesResult$ = new BehaviorSubject<ActiveDeliveriesResultDto>(null);
  private deliveryForRedeem$ = new BehaviorSubject<ActiveDeliveryResultDto>(null);



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


  setDeliveries(deliveries: ActiveDeliveriesResultDto) : void {
    this.deliveriesResult$.next(deliveries)
  }

  removeDelivery(deliveryId: number): void {
    let deliveries = this.deliveriesResult$.getValue();
    deliveries.result = deliveries.result.filter(r => r.id != deliveryId)
    deliveries.total = deliveries.result.length;
    this.deliveriesResult$.next(deliveries);
  }

  setDeliveryForRedeem(delivery: ActiveDeliveryResultDto): void {
    this.deliveryForRedeem$.next(delivery);
  }

  getDeliveryForRedeem$() : Observable<ActiveDeliveryResultDto> {
    return this.deliveryForRedeem$.asObservable();
  }


  getActiveDeliveries$() : Observable<ActiveDeliveriesResultDto> {
    return this.deliveriesResult$.asObservable();
  }

  removeActiveDelivery(activeDelivery: ActiveDelivery) : void {
    const currDeliveries = this.deliveriesResult$.getValue()
    currDeliveries.result = currDeliveries.result.filter(d => d.id !== activeDelivery.id)
    this.deliveriesResult$.next(currDeliveries)

  }
}
