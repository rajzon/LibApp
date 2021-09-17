import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {LendFacade} from "../../lend.facade";
import {IdentityType, LendBasket} from "../../api/lend-api.service";
import {Observable, Subscription} from "rxjs";
import {SuggestCustomerResult} from "../../api/search-api.service";
import {NgxSpinnerService} from "ngx-spinner";
import {environment} from "@env";
import {DeleteDeliveryCanncellationReasonModalComponent} from "../../../delivery/components/delete-delivery-canncellation-reason-modal/delete-delivery-canncellation-reason-modal.component";
import {DeleteActiveDeliveryCommand} from "../../../delivery/components/delivery-pending-list/delivery-pending-list.component";
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {DeleteStockInLendBasketModalComponent} from "../../components/delete-stock-in-lend-basket-modal/delete-stock-in-lend-basket-modal.component";

@Component({
  selector: 'app-lend-basket',
  templateUrl: './lend-basket.component.html',
  styleUrls: ['./lend-basket.component.sass']
})
export class LendBasketComponent implements OnInit, AfterViewInit, OnDestroy {

  isLoading$: Observable<boolean>
  isAdding$: Observable<boolean>
  isDeleting$: Observable<boolean>

  modalRef: BsModalRef
  basketSubs: Subscription
  addStockForBasketSubs: Subscription
  addCustomerForBasketSubs: Subscription
  lendBasketSubs: Subscription
  editReturnDateForStockInBasketSubs: Subscription
  deleteStockInBasketSubs: Subscription
  customersSuggestSubs: Subscription

  basket: LendBasket
  stockSearchTerm: number
  lendSettings = environment.lend;
  today: Date;
  identityVal : string

  constructor(private lendFacade: LendFacade,
              private spinner: NgxSpinnerService,
              private modalService: BsModalService) {
    this.isLoading$ = this.lendFacade.isLoading$()
    this.isAdding$ = this.lendFacade.isAdding$()
    this.isDeleting$ = this.lendFacade.isDeleting$()
  }

  ngAfterViewInit(): void {
    this.spinner.show();
  }

  ngOnInit(): void {
    this.today = new Date();

    this.basketSubs = this.lendFacade.getBasketForLend$().subscribe((res:LendBasket) => {
      this.identityVal = IdentityType[res.customer?.identityType]
      this.basket = res;
    })
  }


  ngOnDestroy(): void {
    this.basketSubs != null? this.basketSubs.unsubscribe() : {}
    this.addStockForBasketSubs != null? this.addStockForBasketSubs.unsubscribe() : {}
    this.addCustomerForBasketSubs != null? this.addCustomerForBasketSubs.unsubscribe() : {}
    this.customersSuggestSubs != null? this.customersSuggestSubs.unsubscribe() : {}
    this.lendBasketSubs != null? this.lendBasketSubs.unsubscribe() : {}
    this.editReturnDateForStockInBasketSubs != null? this.editReturnDateForStockInBasketSubs.unsubscribe() : {}
    this.deleteStockInBasketSubs != null? this.deleteStockInBasketSubs.unsubscribe() : {}
  }

  addStockForBasket(stockId: number) : void {
    this.addStockForBasketSubs = this.lendFacade.addStockForBasket$(stockId).subscribe((res:LendBasket) => {
      this.identityVal = IdentityType[res.customer?.identityType]
      this.basket = res;
    })
  }

  addCustomerForBasket(editedBasket: LendBasket) : void {
    this.identityVal = IdentityType[editedBasket.customer?.identityType]
    this.basket = editedBasket;
  }

  lendBasket() : void {
    this.lendBasketSubs = this.lendFacade.lendBasket$().subscribe(res=> {
      //TODO clear basket or return to other page?
      for (const prop of Object.getOwnPropertyNames(this.basket)) {
        delete this.basket[prop];
      }
    })
  }

  editReturnDateForStockInBasket(stockId: number, returnDate: Date | string) : void {
    console.log(stockId)
    this.editReturnDateForStockInBasketSubs = this.lendFacade.editReturnDateForStockInBasket$(stockId, returnDate).subscribe((res:LendBasket) => {
      this.identityVal = IdentityType[res.customer?.identityType]
      this.basket = res;
    })
  }

  deleteStockInBasket(stockId: number) : void {
    const initialState = {
      requiredConfirmId: stockId,
      passedStockId: -1
    }

    this.modalRef = this.modalService.show(DeleteStockInLendBasketModalComponent, {initialState})
    this.modalRef.content.onDelete$.subscribe(() => {
      this.deleteStockInBasketSubs = this.lendFacade.deleteStockInBasket$(stockId).subscribe((res:LendBasket) => {
        this.identityVal = IdentityType[res.customer?.identityType]
        this.basket = res;
      })
    })


  }


  onShown($event: Date, stockId: number) {
    console.log('fired')
    const newReturnDate = $event.toDateString()
    this.editReturnDateForStockInBasket(stockId, newReturnDate)
  }

  isBasketInvalid(): boolean
  {
    return this.basket == null || this.basket.businessErrors?.length > 0 || this.basket.stockWithBooks == null || this.basket.stockWithBooks.length < 1 || this.basket.customer == null;
  }

}
