import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {DeliveryFacade} from "../../delivery.facade";
import {Observable, Subscription} from "rxjs";
import {NgxSpinnerService} from "ngx-spinner";
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {DeliveryEditItemCountModalComponent} from "../delivery-edit-item-count-modal/delivery-edit-item-count-modal.component";
import {CreateBookUsingApiDto} from "../../../book/models/create-book-using-api-dto";
import {MessagePopupService} from "@core/services/message-popup.service";
import {ActiveDeliveryItemForCreationDto, CreateActiveDeliveryCommand} from "../../api/delivery-api.service";
import {environment} from "@env";
import {Router} from "@angular/router";
import {ActiveDeliveryResultFromSearch} from "../../models/active-delivery-result-from-search";

@Component({
  selector: 'app-delivery-create',
  templateUrl: './delivery-create.component.html',
  styleUrls: ['./delivery-create.component.sass']
})
export class DeliveryCreateComponent implements OnInit, AfterViewInit {


  searchTerm: string
  deliveryItems: ActiveDeliveryResultFromSearch[] = new Array<ActiveDeliveryResultFromSearch>()
  isLoading$: Observable<boolean>
  isAdding$: Observable<boolean>
  modalRef: BsModalRef
  dataNeededToAddDelivery: CreateActiveDeliveryCommand = new CreateActiveDeliveryCommand();

  deliveryConfigs = environment.delivery

  get isAllDeliveryItemsAreCorrect(): boolean {
    const isNotEmpty = this.deliveryItems.length > 0;
    const isAllItemsHaveProperItemsCount = this.deliveryItems.every(d => d.itemsCount > 0);
    return isNotEmpty && isAllItemsHaveProperItemsCount;
  }

  constructor(private deliveryFacade: DeliveryFacade,
              private spinner: NgxSpinnerService,
              private modalService: BsModalService,
              private messageService: MessagePopupService) {
    this.isLoading$ = this.deliveryFacade.isLoading$();
    this.isAdding$ = this.deliveryFacade.isAdding$();
  }

  ngOnInit(): void {
    this.deliveryItems = new Array<ActiveDeliveryResultFromSearch>();
    this.dataNeededToAddDelivery.itemsInfo = new Array<ActiveDeliveryItemForCreationDto>()
  }

  ngAfterViewInit(): void {
    this.spinner.show();
  }



  search(): void {
    console.log('FIRED')
    if (this.deliveryItems.some(d => d.ean13 === this.searchTerm))
      this.messageService.displayInfo(`Delivery already have item with bookEan ${this.searchTerm}`)
    else
      this.deliveryFacade.searchBookForActiveDeliveryByEan$(this.searchTerm).subscribe((res: ActiveDeliveryResultFromSearch) => {
          this.deliveryItems.push(res);
      })
    console.log(this.deliveryItems)
  }

  editItemCount(item: ActiveDeliveryResultFromSearch): void {
    const initialState = {
      item: item
    }
    this.modalRef = this.modalService.show(DeliveryEditItemCountModalComponent, {initialState});
    this.modalRef.content.onEdit$.subscribe((res:number) => {
      item.itemsCount = res;
    })
  }

  deleteItem(item: ActiveDeliveryResultFromSearch): void {
    this.deliveryItems = this.deliveryItems.filter(d => d !== item);
  }

  addDelivery(): void {
    for (let i = 0; i < this.deliveryItems.length; i++) {
      const deliveryItemSrc = this.deliveryItems[i];
      this.dataNeededToAddDelivery.itemsInfo[i] = new ActiveDeliveryItemForCreationDto(deliveryItemSrc.id, deliveryItemSrc.ean13, deliveryItemSrc.itemsCount);
    }
    console.log(this.dataNeededToAddDelivery);
    this.deliveryFacade.addNewActiveDelivery(this.dataNeededToAddDelivery).subscribe();
  }



}
