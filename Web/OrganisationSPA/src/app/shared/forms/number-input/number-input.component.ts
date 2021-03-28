import {Component, Input, } from '@angular/core';
import {AbstractControl, ControlValueAccessor, FormGroup, NgControl} from "@angular/forms";
import {isRequiredField} from "@shared/helpers/forms/is-required-field.function";

@Component({
  selector: 'app-number-input',
  templateUrl: './number-input.component.html',
  styleUrls: ['./number-input.component.sass']
})
export class NumberInputComponent implements ControlValueAccessor {
  @Input() label: string;
  type: string = "number";

  constructor(public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }


  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  writeValue(obj: any): void {
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

}
