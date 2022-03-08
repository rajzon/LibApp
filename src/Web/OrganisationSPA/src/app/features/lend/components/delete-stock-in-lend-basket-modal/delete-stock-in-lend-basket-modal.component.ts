import { Component, OnInit } from '@angular/core';
import {Subject} from "rxjs";
import {BsModalRef} from "ngx-bootstrap/modal";

@Component({
  selector: 'app-delete-stock-in-lend-basket-modal',
  templateUrl: './delete-stock-in-lend-basket-modal.component.html',
  styleUrls: ['./delete-stock-in-lend-basket-modal.component.sass']
})
export class DeleteStockInLendBasketModalComponent implements OnInit {

  private onDelete$ = new Subject<string>();

  requiredConfirmEan: string
  insertedEan: string

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }


  delete() {
    this.onDelete$.next();
    this.bsModalRef.hide();
  }

}
