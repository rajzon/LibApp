import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './forms/text-input/text-input.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { NumberInputComponent } from './forms/number-input/number-input.component';
import { DigitsOnlyDirective } from './directives/digits-only/digits-only.directive';
import {TabsModule} from "ngx-bootstrap/tabs";
import {NgSelectModule} from "@ng-select/ng-select";
import {BsDatepickerModule} from "ngx-bootstrap/datepicker";
import {QuillModule} from "ngx-quill";
import {FileUploadModule} from "ng2-file-upload";
import { FileUploaderComponent } from './file-uploader/file-uploader.component';
import {NgxSpinnerModule} from "ngx-spinner";
import {ToastrModule} from "ngx-toastr";
import {ModalModule} from "ngx-bootstrap/modal";
import {PaginationModule} from "ngx-bootstrap/pagination";



@NgModule({
  declarations: [TextInputComponent, NumberInputComponent, DigitsOnlyDirective, FileUploaderComponent],
  exports: [
    TextInputComponent,
    NumberInputComponent,
    TabsModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    BsDatepickerModule,
    QuillModule,
    FileUploadModule,
    FileUploaderComponent,
    NgxSpinnerModule,
    ToastrModule,
    ModalModule,
    PaginationModule
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    QuillModule.forRoot(),
    FileUploadModule,
    NgxSpinnerModule,
    ToastrModule.forRoot(),
    ModalModule.forRoot(),
    PaginationModule.forRoot()
  ]
})
export class SharedModule { }
