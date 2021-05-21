import {ChangeDetectionStrategy, Component, forwardRef, Input,} from '@angular/core';
import {AbstractControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl} from "@angular/forms";
import {isRequiredField} from "@shared/helpers/forms/is-required-field.function";

@Component({
  selector: 'app-number-input',
  templateUrl: './number-input.component.html',
  styleUrls: ['./number-input.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NumberInputComponent implements ControlValueAccessor {
  @Input() label: string;
  type: string = "number";
  value: number;
  changed: (value: number) => void;
  touched: () => void;

  constructor(public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }


  registerOnChange(fn: any): void {
    this.changed = fn;
  }

  registerOnTouched(fn: any): void {
    this.touched = fn;
  }

  writeValue(value: number): void {
    this.value = value;
  }

  onChange(event: Event): void {
    const value: number = parseInt((<HTMLInputElement>event.target).value);
    this.changed(value);
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

}
