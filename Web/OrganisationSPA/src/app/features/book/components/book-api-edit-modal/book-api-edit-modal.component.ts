import { Component, OnInit } from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {AbstractControl, FormControl, FormGroup} from "@angular/forms";
import {createFormControl} from "@shared/helpers/forms/create-form-control.function";
import {environment} from "@env";
import { isRequiredField } from '@shared/helpers/forms/is-required-field.function';

@Component({
  selector: 'app-book-api-edit-modal',
  templateUrl: './book-api-edit-modal.component.html',
  styleUrls: ['./book-api-edit-modal.component.sass']
})
export class BookApiEditModalComponent implements OnInit {

  volumeInfo: any;
  editForm: FormGroup;
  bookFieldsSettings = environment.book;
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.editForm = new FormGroup({
      title: createFormControl(this.volumeInfo.title, this.bookFieldsSettings.title),
      authorFirstName: createFormControl(this.volumeInfo.authors, this.bookFieldsSettings.author.authorFirstName),
      authorLastName: createFormControl(this.volumeInfo.authors, this.bookFieldsSettings.author.authorLastName),
      categoriesNames: createFormControl(this.volumeInfo.categories, this.bookFieldsSettings.categories.name),
      pageCount: createFormControl(this.volumeInfo.pageCount, this.bookFieldsSettings.pageCount),
      language: createFormControl(this.volumeInfo.language, this.bookFieldsSettings.language.languageName),
      isbn10: createFormControl(this.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_10')[0]?.identifier, this.bookFieldsSettings.isbn10),
      isbn13: createFormControl(this.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_13')[0]?.identifier, this.bookFieldsSettings.isbn13),
      visibility: new FormControl(true),
      publisher: createFormControl(this.volumeInfo.publisher, this.bookFieldsSettings.publisher.publisherName),
      publishedDate: createFormControl(new Date(this.volumeInfo.publishedDate), this.bookFieldsSettings.publishedDate),
      description: new FormControl(this.volumeInfo.description)
    })

    this.convertDate();
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

  convertDate(): void {
    var a = new Date(this.volumeInfo.publishedDate,0);
    console.log(a);
  }

  edit(): void {
    this.bsModalRef.hide();
  }

}
