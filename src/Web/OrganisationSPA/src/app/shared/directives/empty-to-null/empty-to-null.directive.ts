import {Directive, HostListener, Self} from '@angular/core';
import {FormControl, NgControl} from "@angular/forms";

@Directive({
  selector: '[appEmptyToNull]'
})
export class EmptyToNullDirective {

  constructor(@Self() private formControl: NgControl) { }

  @HostListener('keyup', ['$event']) onKeyDowns(event: KeyboardEvent) {
    if (this.formControl.value?.trim() === '') {
      this.formControl.reset(null);
    }
  }

}
