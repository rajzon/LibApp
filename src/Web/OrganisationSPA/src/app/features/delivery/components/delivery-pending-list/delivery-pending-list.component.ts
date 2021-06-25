import {AfterViewInit, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {environment} from "@env";
import {ReloadActiveDeliveriesQueryDto} from "../../models/reload-active-deliveries-query-dto";
import {ActiveDeliveriesResultDto} from "../../models/active-deliveries-result-dto";

@Component({
  selector: 'app-delivery-pending-list',
  templateUrl: './delivery-pending-list.component.html',
  styleUrls: ['./delivery-pending-list.component.sass']
})
export class DeliveryPendingListComponent implements OnInit, AfterViewInit {

  @Input() deliveryResult: ActiveDeliveriesResultDto
  @Output() reloadDeliveriesEvent = new EventEmitter<ReloadActiveDeliveriesQueryDto>()

  itemsPerPageOptions = environment.pagination.itemsPerPageOpts
  selectedMaxResult: number = environment.pagination.itemsPerPageDefault
  currentPage: number = 1;

  constructor() { }

  ngOnInit(): void {
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

}

