import {ChangeDetectionStrategy, Component, Input} from '@angular/core';
import {AbstractControl, ControlValueAccessor, NgControl} from "@angular/forms";
import {isRequiredField} from "@shared/helpers/forms/is-required-field.function";

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
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

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

}
