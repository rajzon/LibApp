import {ChangeDetectionStrategy, Component, Input, Output} from '@angular/core';
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FileUploaderComponent {

  @Input() uploaderOptions: FileUploaderOptions
  @Input() uploaderStyle:  IFileUploaderStyle
  uploader: FileUploader
  hasBaseDropZoneOver: boolean;
  response: string

  constructor() {
    this.uploader = new FileUploader(this.uploaderOptions);

    this.hasBaseDropZoneOver = false;

    this.response = '';

    //TODO: consider later on
    this.uploader.response.subscribe(res => this.response = res);
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



}
