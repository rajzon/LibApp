import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './forms/text-input/text-input.component';
import {ReactiveFormsModule} from "@angular/forms";
import { NumberInputComponent } from './forms/number-input/number-input.component';
import { DigitsOnlyDirective } from './directives/digits-only/digits-only.directive';



@NgModule({
  declarations: [TextInputComponent, NumberInputComponent, DigitsOnlyDirective],
  exports: [
    TextInputComponent,
    NumberInputComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
