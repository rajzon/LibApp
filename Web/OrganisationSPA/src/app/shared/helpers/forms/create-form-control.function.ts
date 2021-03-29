import {FormControl, Validators} from "@angular/forms";

export function createFormControl<T>(formState: T, opts: any): FormControl {
  return new FormControl(formState, [
    opts.required ? Validators.required : Validators.nullValidator,
    opts.minLength ? Validators.minLength(opts.minLength) : Validators.nullValidator,
    opts.maxLength ? Validators.maxLength(opts.maxLength) : Validators.nullValidator,
    opts.min ? Validators.min(opts.min) : Validators.nullValidator,
    opts.max ? Validators.max(opts.max) : Validators.nullValidator,
    opts.pattern ? Validators.pattern(opts.pattern) : Validators.nullValidator,
  ]);
}
