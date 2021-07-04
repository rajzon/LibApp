import {AfterViewInit, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {environment} from "@env";
import {ReloadActiveDeliveriesQueryDto} from "../../models/reload-active-deliveries-query-dto";
import {ActiveDeliveriesResultDto} from "../../models/active-deliveries-result-dto";
import {ActiveDelivery} from "../../models/active-delivery-dto";
import {AuthService} from "@core/services/auth.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-delivery-pending-list',
  templateUrl: './delivery-pending-list.component.html',
  styleUrls: ['./delivery-pending-list.component.sass']
})
export class DeliveryPendingListComponent implements OnInit, AfterViewInit {

  @Input() deliveryResult: ActiveDeliveriesResultDto
  @Output() reloadDeliveriesEvent = new EventEmitter<ReloadActiveDeliveriesQueryDto>()
  @Output() deleteDeliveryEvent = new EventEmitter<ActiveDelivery>();

  itemsPerPageOptions = environment.pagination.itemsPerPageOpts
  selectedMaxResult: number = environment.pagination.itemsPerPageDefault
  currentPage: number = 1;
  deleteFunctionalityName = environment.deleteActiveDeliveryFunctionalityName;
  addDeliveryFunctionalityName = environment.addDeliveryFunctionalityName;

  accessModel: {hasRightsToDeleteDelivery: boolean, hasRightsToAddDelivery: boolean}

  constructor(private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.accessModel = {hasRightsToDeleteDelivery: false, hasRightsToAddDelivery: false};
    this.canUserAccess(this.deleteFunctionalityName)
    this.canUserAccess(this.addDeliveryFunctionalityName)
  }

  pageChanged(event: any): void {
    this.currentPage = event.page;
    const query = new ReloadActiveDeliveriesQueryDto(this.currentPage, this.selectedMaxResult)
    this.reloadDeliveriesEvent.emit(query);
  }

  ngAfterViewInit(): void {
    this.selectedMaxResult = this.deliveryResult.pageSize
    this.currentPage = this.deliveryResult.currentPage
  }


  onMaxResultsChanged(): void {
    const query = new ReloadActiveDeliveriesQueryDto(1, this.selectedMaxResult)
    this.reloadDeliveriesEvent.emit(query);
  }

  deleteDelivery(delivery: ActiveDelivery) {
    this.deleteDeliveryEvent.emit(delivery);
  }

  canUserAccess(functionalityName: string): void{
    if (functionalityName === this.deleteFunctionalityName)
      this.authService.hasUserHaveRightsToAccess$(functionalityName, this.route).subscribe(res => this.accessModel.hasRightsToDeleteDelivery = res);
    if (functionalityName === this.addDeliveryFunctionalityName)
      this.authService.hasUserHaveRightsToAccess$(functionalityName, this.route).subscribe(res => this.accessModel.hasRightsToAddDelivery = res);
  }

}

