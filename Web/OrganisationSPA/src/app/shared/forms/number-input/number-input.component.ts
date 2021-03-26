import {Component, Input, } from '@angular/core';
import {ControlValueAccessor, NgControl} from "@angular/forms";

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

}
