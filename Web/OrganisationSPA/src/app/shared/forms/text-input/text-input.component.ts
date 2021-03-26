import { Component, Input } from '@angular/core';
import {ControlValueAccessor, NgControl, ValidationErrors} from "@angular/forms";

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.sass']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label: string;
  type: string = "text";

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
