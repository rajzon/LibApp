import { Injectable } from '@angular/core';
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class MessagePopupService {

  constructor(private toastr: ToastrService) { }

  public displayError(error: any): void {
    if (error instanceof Array) {
      this.toastr.error(error.toString(), '', {
        positionClass: "toast-top-full-width"
      });
    } else if (error instanceof Object) {
      this.toastr.error(error.title,'', {
        positionClass: "toast-top-full-width"
      });
    } else {
      this.toastr.error(error, '', {
        positionClass: "toast-top-full-width"
      });
    }
  }

  public displayInfo(message: string): void {
    this.toastr.info(message);
  }

  public displaySuccess(message: string): void {
    this.toastr.success(message);
  }
}
