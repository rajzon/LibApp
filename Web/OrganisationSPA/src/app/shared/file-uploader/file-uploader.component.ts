import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  HostListener,
  Input, OnInit,
  Output
} from '@angular/core';
import {FileItem, FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {BookFacade} from "../../features/book/book.facade";
import {UploaderState} from "@core/state/uploader.state";
import {BehaviorSubject, Subscription} from "rxjs";

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.sass'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class FileUploaderComponent implements AfterViewInit{

  @Input() uploaderStyle: IFileUploaderStyle
  @Input() uploader: FileUploader;
  hasBaseDropZoneOver: boolean;

  constructor(private uploaderState: UploaderState) {
    this.hasBaseDropZoneOver = false;

  }


  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  isRemoveOnly(): boolean {
    return this.uploaderStyle.style === 'removeOnly';
  }

  isAll(): boolean {
    return this.uploaderStyle.style === 'all';
  }

  callback() {

  }

  ngAfterViewInit(): void {

    this.uploader? this.uploader.onErrorItem = (item, response, status, header) => {
      console.log("Error occured during uploading image");
      console.log(this.uploader);
    }:

    console.log(this.uploader?.progress);
    console.log(this.uploader);
  }



}
