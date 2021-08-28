import {AfterViewInit, Component, Inject, LOCALE_ID, OnDestroy, OnInit} from '@angular/core';
import {DeliveryFacade} from "../../delivery.facade";
import {Observable, Subscription} from "rxjs";
import {environment} from "@env";
import {map} from "rxjs/operators";
import {formatDate} from "@angular/common";
import {NgxSpinnerService} from "ngx-spinner";
import {ReloadActiveDeliveriesQueryDto} from "../../models/reload-active-deliveries-query-dto";
import {ActiveDeliveriesResultDto} from "../../models/active-deliveries-result-dto";
import {ActiveDelivery} from "../../models/active-delivery-dto";
import {DeleteActiveDeliveryCommand} from "../../components/delivery-pending-list/delivery-pending-list.component";
import {ActiveDeliveryResultDto} from "../../models/active-delivery-result-dto";

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.sass']
})
export class DeliveryComponent implements OnInit, AfterViewInit, OnDestroy {

  activeDeliveriesSubs: Subscription;

  activeDeliveries$: Observable<ActiveDeliveriesResultDto>
  isLoading$: Observable<boolean>

  defaultPageSize = environment.pagination.itemsPerPageDefault

  constructor(@Inject(LOCALE_ID) private locale: string,
              private deliveryFacade: DeliveryFacade,
              private spinner: NgxSpinnerService) {
    this.isLoading$ = this.deliveryFacade.isLoading$();
  }

  ngOnInit(): void {
    this.activeDeliveriesSubs = this.loadDeliveries()
  }

  ngAfterViewInit(): void {
    this.spinner.show();
  }

  ngOnDestroy(): void {
    this.activeDeliveriesSubs.unsubscribe();
  }

  reloadDeliveries(query: ReloadActiveDeliveriesQueryDto): void {
    this.activeDeliveriesSubs = this.loadDeliveries(query)
  }

  deleteActiveDelivery(command: DeleteActiveDeliveryCommand): void {
    this.deliveryFacade.deleteActiveDelivery$(command.activeDelivery, command.cancellationReason).subscribe();
  }

  getActiveDelivery(id: number): Observable<ActiveDeliveryResultDto> {
    return this.deliveryFacade.loadDeliveryForRedeem$(id);
  }

  private loadDeliveries(query?: ReloadActiveDeliveriesQueryDto) : Subscription {
    const currentPage = query? query.currentPage: 1;
    const pageSize = query? query.pageSize: this.defaultPageSize

    return  this.deliveryFacade.loadActiveDeliveries$(currentPage, pageSize).subscribe(res1 => {
      this.activeDeliveries$ = this.deliveryFacade.getActiveDeliveries$().pipe(map(res2 => {
        res1.result?.map(r => {
          console.log(res1)
          r.modificationDate = formatDate(r.modificationDate, 'dd-MM-yyyy', this.locale)
          r.creationDate = formatDate(r.creationDate, 'dd-MM-yyyy', this.locale)
        })
        return res1;
      }));
    });

  }



}
