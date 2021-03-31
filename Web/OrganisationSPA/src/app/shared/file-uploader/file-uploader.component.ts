import {AfterViewInit, ChangeDetectionStrategy, Component, Input, Output} from '@angular/core';
import {FileItem, FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {BookFacade} from "../../features/book/book.facade";

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FileUploaderComponent implements AfterViewInit{

  @Input() uploaderOptions: FileUploaderOptions
  @Input() uploaderStyle:  IFileUploaderStyle

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  response: string

  constructor(private bookFacade: BookFacade) {
    // this.uploader = new FileUploader(this.uploaderOptions);
    bookFacade.getUploader$().subscribe(res => this.uploader = res);
    this.hasBaseDropZoneOver = false;

    // this.response = '';
    //
    // //TODO: consider later on
    // this.uploader.response.subscribe(res => this.response = res);

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

  setUploader() {
    this.bookFacade.setUploader(this.uploader);
  }

  ngAfterViewInit(): void {
    this.uploader.onErrorItem = (item, response, status, header) => {
      console.log("Error occured during uploading image");
      console.log(this.uploader);
    }
    console.log(this.uploader);
  }



}
