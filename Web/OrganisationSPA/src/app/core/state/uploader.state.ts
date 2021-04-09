import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {FileUploader} from "ng2-file-upload";

@Injectable({
  providedIn: "root"
})
export class UploaderState {

  private manualBookImgUploader$ = new BehaviorSubject<FileUploader>(null);
  private googleBookImgUploaders$ = new BehaviorSubject<FileUploader[]>(null);

  //Manual Book
  getManualBookImgUploader$() {
    return this.manualBookImgUploader$.asObservable();
  }

  setManualBookImgUploader(uploader: FileUploader): void {
    this.manualBookImgUploader$.next(uploader);
  }




}
