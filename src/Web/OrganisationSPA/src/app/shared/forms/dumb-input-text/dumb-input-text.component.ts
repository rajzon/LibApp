import {Component, forwardRef, Input} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl} from "@angular/forms";

@Component({
  selector: 'app-dumb-input-text',
  templateUrl: './dumb-input-text.component.html',
  styleUrls: ['./dumb-input-text.component.sass'],
  providers: [
    { provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DumbInputTextComponent),
      multi: true
    }
  ]
})
export class DumbInputTextComponent implements ControlValueAccessor{
@Input() disabled: boolean;
@Input() label: string;
@Input() value: string;
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
