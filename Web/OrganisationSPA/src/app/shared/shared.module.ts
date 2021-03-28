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



@NgModule({
  declarations: [TextInputComponent, NumberInputComponent, DigitsOnlyDirective],
  exports: [
    TextInputComponent,
    NumberInputComponent,
    TabsModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    BsDatepickerModule,
    QuillModule
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    QuillModule.forRoot()
  ]
})
export class SharedModule { }
