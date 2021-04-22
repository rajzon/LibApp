import {Component, OnInit} from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {AbstractControl, FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {createFormControl} from "@shared/helpers/forms/create-form-control.function";
import {environment} from "@env";
import {isRequiredField} from '@shared/helpers/forms/is-required-field.function';
import {BehaviorSubject, Subject} from "rxjs";
import {CreateBookUsingApiDto} from "../../models/create-book-using-api-dto";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";

@Component({
  selector: 'app-book-api-edit-modal',
  templateUrl: './book-api-edit-modal.component.html',
  styleUrls: ['./book-api-edit-modal.component.sass']
})
export class BookApiEditModalComponent implements OnInit {

  addCommand$ = new Subject<CreateBookUsingApiDto>();
  volumeInfo: any;
  uploaderStyle: IFileUploaderStyle;
  uploader: FileUploader;
  editForm: FormGroup;
  bookFieldsSettings = environment.book;
  get categories() {
    return this.editForm.get('categoriesNames') as FormArray;
  }
  get authors() {
    return this.editForm.get('authorsNames') as FormArray;
  }


  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    const addCommand: CreateBookUsingApiDto = {
      book: this.volumeInfo,
      uploader: this.uploader
    };
    this.addCommand$.next(addCommand);
    this.editForm = new FormGroup({
      title: createFormControl(this.volumeInfo.title, this.bookFieldsSettings.title),
      pageCount: createFormControl(this.volumeInfo.pageCount, this.bookFieldsSettings.pageCount),
      languageName: createFormControl(this.volumeInfo.languageName, this.bookFieldsSettings.language.languageName),
      isbn10: createFormControl(this.volumeInfo.isbn10, this.bookFieldsSettings.isbn10),
      isbn13: createFormControl(this.volumeInfo.isbn13, this.bookFieldsSettings.isbn13),
      visibility: new FormControl(this.volumeInfo.visibility),
      publisherName: createFormControl(this.volumeInfo.publisherName, this.bookFieldsSettings.publisher.publisherName),
      publishedDate: createFormControl(new Date(this.volumeInfo.publishedDate), this.bookFieldsSettings.publishedDate),
      description: new FormControl(this.volumeInfo.description),
    })

    this.registerAuthorsFormArray();
    this.registerCategoriesFormArray();
  }


  private registerCategoriesFormArray() {
    this.volumeInfo.categories = this.volumeInfo.categories ?? new Array<string>();
    const controls = this.volumeInfo.categories.map(x => {
      return createFormControl(x, this.bookFieldsSettings.categories.name);
    })
    this.editForm.registerControl('categoriesNames',
      new FormArray(controls,
      this.bookFieldsSettings.categories.required? Validators.required: Validators.nullValidator));
  }

  private registerAuthorsFormArray() {
    this.volumeInfo.authors = this.volumeInfo.authors ?? new Array<string>();
    const controls = this.volumeInfo.authors.map(x => {
      return createFormControl(x.author, this.bookFieldsSettings.author)
    });
    this.editForm.registerControl('authorsNames',
      new FormArray(controls,
      this.bookFieldsSettings.author.required? Validators.required: Validators.nullValidator
      ));
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

  removeFormControlFromArray(index: number, formArray: FormArray): void {
    formArray.removeAt(index);
  }

  insertFormControlToArray(formArray: FormArray, validatorsOpts: any): void {
    const place = formArray.length;
    formArray.insert(place, createFormControl(null, validatorsOpts))
  }


  edit(): void {
    console.log(this.editForm.value);
    const addCommand: CreateBookUsingApiDto = {
      book: this.editForm.value,
      uploader: this.uploader
    };
    this.addCommand$.next(addCommand);
    this.bsModalRef.hide();
  }

}
