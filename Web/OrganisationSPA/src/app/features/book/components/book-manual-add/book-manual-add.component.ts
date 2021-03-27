import {ChangeDetectionStrategy, Component, Input} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
// @ts-ignore
import {environment} from "@env";
import {Book} from "../../models/book";
import {Category} from "../../models/category";
import {BookCategoryApiService} from "../../api/book-category-api.service";

@Component({
  selector: 'app-book-manual-add',
  templateUrl: './book-manual-add.component.html',
  styleUrls: ['./book-manual-add.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookManualAddComponent {

  @Input() categories: Category[]
  book: Book = new Book()
  bookFieldsSettings = environment.book;

  manualBookAddForm: FormGroup
  constructor() {
    this.createFormGroup();
  }

  private createFormGroup() {
    this.manualBookAddForm = new FormGroup({
      title: BookManualAddComponent.createFormControl(this.book.title, this.bookFieldsSettings.title),
      authorFirstName: BookManualAddComponent.createFormControl(this.book.authorFirstName, this.bookFieldsSettings.authorFirstName),
      authorLastName: BookManualAddComponent.createFormControl(this.book.authorLastName, this.bookFieldsSettings.authorLastName),
      pageCount: BookManualAddComponent.createFormControl(this.book.pageCount, this.bookFieldsSettings.pageCount),
      languageName: BookManualAddComponent.createFormControl(this.book.languageName, this.bookFieldsSettings.languageName),
      isbn10: BookManualAddComponent.createFormControl(this.book.isbn10, this.bookFieldsSettings.isbn10),
      isbn13: BookManualAddComponent.createFormControl(this.book.isbn13, this.bookFieldsSettings.isbn13),
      publisherName: BookManualAddComponent.createFormControl(this.book.publisherName, this.bookFieldsSettings.publisherName),
      publishedDate: BookManualAddComponent.createFormControl(this.book.publishedDate, this.bookFieldsSettings.publishedDate),
      publicSiteLink: BookManualAddComponent.createFormControl(this.book.publicSiteLink, this.bookFieldsSettings.publicSiteLink),
    })
  }


  private static createFormControl<T>(field: T, opts: any) {
    return new FormControl(field, [
      opts.required ? Validators.required : Validators.nullValidator,
      opts.minLength ? Validators.minLength(opts.minLength) : Validators.nullValidator,
      opts.maxLength ? Validators.maxLength(opts.maxLength) : Validators.nullValidator,
      opts.min ? Validators.min(opts.min) : Validators.nullValidator,
      opts.max ? Validators.max(opts.max) : Validators.nullValidator,
      opts.pattern ? Validators.pattern(opts.pattern) : Validators.nullValidator,
    ]);
  }


  add() {

  }
}


