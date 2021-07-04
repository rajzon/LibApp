import { Component, OnInit } from '@angular/core';
import {Subject} from "rxjs";
import {BsModalRef} from "ngx-bootstrap/modal";

@Component({
  selector: 'app-add-new-delivery-confirmation-box-modal',
  templateUrl: './add-new-delivery-confirmation-box-modal.component.html',
  styleUrls: ['./add-new-delivery-confirmation-box-modal.component.sass']
})
export class AddNewDeliveryConfirmationBoxModalComponent implements OnInit {
  public onClose$ = new Subject<boolean>();

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  confirm(): void {
    this.onClose$.next(true)
    this.bsModalRef.hide()
  }

  decline(): void {
    this.onClose$.next(false)
    this.bsModalRef.hide()
  }

}
