import { Component, OnInit } from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {Subject} from "rxjs";

@Component({
  selector: 'app-delete-delivery-canncellation-reason-modal',
  templateUrl: './delete-delivery-canncellation-reason-modal.component.html',
  styleUrls: ['./delete-delivery-canncellation-reason-modal.component.sass']
})
export class DeleteDeliveryCanncellationReasonModalComponent implements OnInit {

  private onDelete$ = new Subject<string>();

  delivery: any;
  cancellationReason: string

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }


  delete() {
    this.onDelete$.next(this.cancellationReason);
    this.bsModalRef.hide();
  }

}
