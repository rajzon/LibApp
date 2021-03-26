import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {environment} from "../../../../../environments/environment";
import {Book} from "../../models/book";

@Component({
  selector: 'app-book-manual-add',
  templateUrl: './book-manual-add.component.html',
  styleUrls: ['./book-manual-add.component.sass']
})
export class BookManualAddComponent implements OnInit {
  book: Book = new Book()
  bookFieldsSettings = environment.book;

  manualBookAddForm: FormGroup
  constructor() { }

  ngOnInit(): void {
    this.manualBookAddForm = new FormGroup({
      title: BookManualAddComponent.getFormControl(this.book.title, this.bookFieldsSettings.title),
      authorFirstName: BookManualAddComponent.getFormControl(this.book.authorFirstName, this.bookFieldsSettings.authorFirstName),
      authorLastName: BookManualAddComponent.getFormControl(this.book.authorLastName, this.bookFieldsSettings.authorLastName),
      pageCount: BookManualAddComponent.getFormControl(this.book.pageCount, this.bookFieldsSettings.pageCount),
      languageName: BookManualAddComponent.getFormControl(this.book.languageName, this.bookFieldsSettings.languageName),
      isbn10: BookManualAddComponent.getFormControl(this.book.isbn10, this.bookFieldsSettings.isbn10),
      isbn13: BookManualAddComponent.getFormControl(this.book.isbn13, this.bookFieldsSettings.isbn13),
      publisherName: BookManualAddComponent.getFormControl(this.book.publisherName, this.bookFieldsSettings.publisherName),
      publishedDate: BookManualAddComponent.getFormControl(this.book.publishedDate, this.bookFieldsSettings.publishedDate),
      publicSiteLink: BookManualAddComponent.getFormControl(this.book.publicSiteLink, this.bookFieldsSettings.publicSiteLink),
    })
  }

  private static getFormControl<T>(field: T, opts: any) {
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
