import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {ActiveDeliveriesResultDto} from "../models/active-deliveries-result-dto";


@Injectable({
  providedIn: "root"
})
export class ActiveDeliveryState {

  private loading$ = new BehaviorSubject<boolean>(false);

  private deliveriesResult$ = new BehaviorSubject<ActiveDeliveriesResultDto>(null);


  isLoading$() {
    return this.loading$.asObservable();
  }

  setLoading(isLoading: boolean): void {
    this.loading$.next(isLoading);
  }


  setDeliveries(deliveries: ActiveDeliveriesResultDto) : void {
    this.deliveriesResult$.next(deliveries)
  }

  getActiveDeliveries$() : Observable<ActiveDeliveriesResultDto> {
    return this.deliveriesResult$.asObservable();
  }
}
