import {Component, OnInit} from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {AbstractControl, FormArray, FormControl, FormGroup} from "@angular/forms";
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


  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    const addCommand: CreateBookUsingApiDto = {
      book: this.volumeInfo,
      uploader: this.uploader
    };
    this.addCommand$.next(addCommand);
    this.editForm = new FormGroup({
      title: createFormControl(this.volumeInfo.title, this.bookFieldsSettings.title),
      //TODO later create form which allow authors as collection instead of only 1
      author: createFormControl(this.volumeInfo.authors[0]?.author, this.bookFieldsSettings.author),
      pageCount: createFormControl(this.volumeInfo.pageCount, this.bookFieldsSettings.pageCount),
      languageName: createFormControl(this.volumeInfo.languageName, this.bookFieldsSettings.language.languageName),
      isbn10: createFormControl(this.volumeInfo.isbn10, this.bookFieldsSettings.isbn10),
      isbn13: createFormControl(this.volumeInfo.isbn13, this.bookFieldsSettings.isbn13),
      visibility: new FormControl(this.volumeInfo.visibility),
      publisherName: createFormControl(this.volumeInfo.publisherName, this.bookFieldsSettings.publisher.publisherName),
      publishedDate: createFormControl(new Date(this.volumeInfo.publishedDate), this.bookFieldsSettings.publishedDate),
      description: new FormControl(this.volumeInfo.description),
    })

    this.registerCategoriesFormArray();
  }


  private registerCategoriesFormArray() {
    this.volumeInfo.categories = this.volumeInfo.categories ?? new Array<string>();
    const controls = this.volumeInfo.categories.map(x => {
      return createFormControl(x, this.bookFieldsSettings.categories.name);
    })
    this.editForm.registerControl('categoriesNames', new FormArray(controls));
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }

  removeCategory(index: number): void {
    let categoriesNames = this.editForm.controls['categoriesNames'] as FormArray;
    categoriesNames.removeAt(index)
  }

  insertCategory(): void {
    let categoriesNames = this.editForm.controls['categoriesNames'] as FormArray;
    const place = categoriesNames.length;
    categoriesNames.insert(place, createFormControl(null, this.bookFieldsSettings.categories.name))
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
