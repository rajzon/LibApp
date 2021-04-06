import {Component, OnInit} from '@angular/core';
import {BsModalRef} from "ngx-bootstrap/modal";
import {AbstractControl, FormArray, FormControl, FormGroup} from "@angular/forms";
import {createFormControl} from "@shared/helpers/forms/create-form-control.function";
import {environment} from "@env";
import {isRequiredField} from '@shared/helpers/forms/is-required-field.function';
import {BehaviorSubject, Subject} from "rxjs";
import {CreateBookUsingApiDto} from "../../models/create-book-using-api-dto";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";

@Component({
  selector: 'app-book-api-edit-modal',
  templateUrl: './book-api-edit-modal.component.html',
  styleUrls: ['./book-api-edit-modal.component.sass']
})
export class BookApiEditModalComponent implements OnInit {

  volumeInfo$ = new Subject<CreateBookUsingApiDto>();
  volumeInfo: any;
  uploaderStyle: IFileUploaderStyle;
  editForm: FormGroup;
  bookFieldsSettings = environment.book;
  get categories() {
    return this.editForm.get('categoriesNames') as FormArray;
  }

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.volumeInfo$.next(this.volumeInfo);

    this.editForm = new FormGroup({
      title: createFormControl(this.volumeInfo.title, this.bookFieldsSettings.title),
      author: new FormGroup({
        firstName: createFormControl(this.volumeInfo.authors, this.bookFieldsSettings.author.authorFirstName),
        lastName: createFormControl(this.volumeInfo.authors, this.bookFieldsSettings.author.authorLastName),
      }),
      pageCount: createFormControl(this.volumeInfo.pageCount, this.bookFieldsSettings.pageCount),
      languageName: createFormControl(this.volumeInfo.language, this.bookFieldsSettings.language.languageName),
      isbn10: createFormControl(this.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_10')[0]?.identifier, this.bookFieldsSettings.isbn10),
      isbn13: createFormControl(this.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_13')[0]?.identifier, this.bookFieldsSettings.isbn13),
      visibility: new FormControl(true),
      publisherName: createFormControl(this.volumeInfo.publisher, this.bookFieldsSettings.publisher.publisherName),
      publishedDate: createFormControl(new Date(this.volumeInfo.publishedDate), this.bookFieldsSettings.publishedDate),
      description: new FormControl(this.volumeInfo.description),
    })
    this.volumeInfo.categories.push('adadadawda');
    const controls = this.volumeInfo.categories?.map(x => {
      return createFormControl(x, this.bookFieldsSettings.categories.name);
    })
    this.editForm.registerControl('categoriesNames', new FormArray(controls));
  }

  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }



  edit(): void {
    console.log(this.editForm.value);
    this.volumeInfo$.next(this.editForm.value);
    this.bsModalRef.hide();
  }

}
