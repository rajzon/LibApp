import { Component, OnInit } from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {Subject} from "rxjs";

@Component({
  selector: 'app-delivery-edit-item-count-modal',
  templateUrl: './delivery-edit-item-count-modal.component.html',
  styleUrls: ['./delivery-edit-item-count-modal.component.sass']
})
export class DeliveryEditItemCountModalComponent implements OnInit {

  private onEdit$ = new Subject<number>();

  item: any;
  currItemsCount: number
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.currItemsCount = this.item.itemsCount;
  }

  edit(): void {
    this.onEdit$.next(this.currItemsCount);
    this.bsModalRef.hide()
  }

}
