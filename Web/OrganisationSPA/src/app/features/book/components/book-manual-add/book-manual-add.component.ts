import {ChangeDetectionStrategy, Component, Input, Output, EventEmitter } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
// @ts-ignore
import {environment} from "@env";
import {Book} from "../../models/book";
import {Category} from "../../models/category";
import {Language} from "../../models/language";
import {Author} from "../../models/author";
import {Publisher} from "../../models/publisher";

@Component({
  selector: 'app-book-manual-add',
  templateUrl: './book-manual-add.component.html',
  styleUrls: ['./book-manual-add.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookManualAddComponent {

  @Input() categories: Category[]
  @Input() languages: Language[]
  @Input() authors: Author[]
  @Input() publishers: Publisher[]
  @Output() createBookEvent = new EventEmitter<Book>()
  book: Book = new Book()
  bookFieldsSettings = environment.book;

  manualBookAddForm: FormGroup
  constructor() {
    this.createFormGroup();

  }

  private createFormGroup() {
    this.manualBookAddForm = new FormGroup({
      title: BookManualAddComponent.createFormControl(this.book.title, this.bookFieldsSettings.title),
      author: new FormControl(null, Validators.required),
      pageCount: BookManualAddComponent.createFormControl(this.book.pageCount, this.bookFieldsSettings.pageCount),
      languageName: BookManualAddComponent.createFormControl(this.book.language.name, this.bookFieldsSettings.languageName),
      isbn10: BookManualAddComponent.createFormControl(this.book.isbn10, this.bookFieldsSettings.isbn10),
      isbn13: BookManualAddComponent.createFormControl(this.book.isbn13, this.bookFieldsSettings.isbn13),
      publisherName: BookManualAddComponent.createFormControl(this.book.publisher.name, this.bookFieldsSettings.publisherName),
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


  onSubmitBook() {
    this.createBookEvent.emit(this.book)

  }
}


