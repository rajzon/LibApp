import {AfterViewInit, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {environment} from "@env";
import {ReloadActiveDeliveriesQueryDto} from "../../models/reload-active-deliveries-query-dto";
import {ActiveDeliveriesResultDto} from "../../models/active-deliveries-result-dto";
import {ActiveDelivery} from "../../models/active-delivery-dto";
import {AuthService} from "@core/services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {DeleteDeliveryCanncellationReasonModalComponent} from "../delete-delivery-canncellation-reason-modal/delete-delivery-canncellation-reason-modal.component";
import {Observable} from "rxjs";
import {ActiveDeliveryResultDto} from "../../models/active-delivery-result-dto";
import {DeliveryFacade} from "../../delivery.facade";

export class DeleteActiveDeliveryCommand {
  cancellationReason: string
  activeDelivery: ActiveDelivery

  constructor(activeDelivery: ActiveDelivery, cancellationReason: string) {
    this.activeDelivery = activeDelivery;
    this.cancellationReason = cancellationReason;
  }
}

@Component({
  selector: 'app-delivery-pending-list',
  templateUrl: './delivery-pending-list.component.html',
  styleUrls: ['./delivery-pending-list.component.sass']
})
export class DeliveryPendingListComponent implements OnInit, AfterViewInit {

  @Input() deliveryResult: ActiveDeliveriesResultDto
  @Output() reloadDeliveriesEvent = new EventEmitter<ReloadActiveDeliveriesQueryDto>()
  @Output() deleteDeliveryEvent = new EventEmitter<DeleteActiveDeliveryCommand>();

  modalRef: BsModalRef
  itemsPerPageOptions = environment.pagination.itemsPerPageOpts
  selectedMaxResult: number = environment.pagination.itemsPerPageDefault
  currentPage: number = 1;
  deleteFunctionalityName = environment.deleteActiveDeliveryFunctionalityName;
  addDeliveryFunctionalityName = environment.addDeliveryFunctionalityName;
  redeemDeliveryFunctionalityName = environment.redeemDeliveryFunctionalityName;

  accessModel: {hasRightsToDeleteDelivery: boolean, hasRightsToAddDelivery: boolean, hasRightsToRedeemDelivery: boolean}

  constructor(private authService: AuthService,
              private route: ActivatedRoute, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.accessModel = {hasRightsToDeleteDelivery: false, hasRightsToAddDelivery: false, hasRightsToRedeemDelivery: false};
    this.canUserAccess(this.deleteFunctionalityName)
    this.canUserAccess(this.addDeliveryFunctionalityName)
    this.canUserAccess(this.redeemDeliveryFunctionalityName)
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
    const initialState = {
      delivery: delivery
    }

    this.modalRef = this.modalService.show(DeleteDeliveryCanncellationReasonModalComponent, {initialState})
    this.modalRef.content.onDelete$.subscribe((cancellationReason: string) => {
      this.deleteDeliveryEvent.emit(new DeleteActiveDeliveryCommand(delivery, cancellationReason));
    })

  }

  canUserAccess(functionalityName: string): void{
    if (functionalityName === this.deleteFunctionalityName)
      this.authService.hasUserHaveRightsToAccess$(functionalityName, this.route).subscribe(res => this.accessModel.hasRightsToDeleteDelivery = res);
    if (functionalityName === this.addDeliveryFunctionalityName)
      this.authService.hasUserHaveRightsToAccess$(functionalityName, this.route).subscribe(res => this.accessModel.hasRightsToAddDelivery = res);
    if (functionalityName === this.redeemDeliveryFunctionalityName)
      this.authService.hasUserHaveRightsToAccess$(functionalityName, this.route).subscribe(res => this.accessModel.hasRightsToRedeemDelivery = res);
  }

}

