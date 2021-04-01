import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {FileUploader} from "ng2-file-upload";

@Injectable({
  providedIn: "root"
})
export class UploaderState {

  private uploader$ = new BehaviorSubject<FileUploader>(null);


  getUploader$() {
    console.log('getUploader')
    return this.uploader$.asObservable();
  }

  setUploader(uploader: FileUploader): void {
    console.log('setUploader')
    this.uploader$.next(uploader);
  }

}
