import {ChangeDetectionStrategy, Component, Input, Output, EventEmitter} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, Validators} from "@angular/forms";
// @ts-ignore
import {environment} from "@env";
import {Book} from "../../models/book";
import {Category} from "../../models/category";
import {Language} from "../../models/language";
import {Author} from "../../models/author";
import {Publisher} from "../../models/publisher";
import {isRequiredField} from "@shared/helpers/forms/is-required-field.function";
import {createFormControl} from "@shared/helpers/forms/create-form-control.function";
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {CreateManualBookDto} from "../../models/create-manual-book-dto";
import {BookFacade} from "../../book.facade";
import {UploaderState} from "@core/state/uploader.state";

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
  @Input() uploaderStyle: IFileUploaderStyle
  @Output() createBookEvent = new EventEmitter<CreateManualBookDto>()
  uploader: FileUploader;

  bookFieldsSettings = environment.book;

  manualBookAddForm: FormGroup


  constructor(private uploaderState: UploaderState) {
    this.uploaderState.getManualBookImgUploader$().subscribe(res => {
      this.uploader = res;
    })
    this.createFormGroup();
  }


  private createFormGroup() {
    this.manualBookAddForm = new FormGroup({
      title: createFormControl(null, this.bookFieldsSettings.title),
      authorId: createFormControl(null, this.bookFieldsSettings.author),
      categoriesIds: createFormControl(null, this.bookFieldsSettings.categories),
      pageCount: createFormControl(null, this.bookFieldsSettings.pageCount),
      languageId: createFormControl(null, this.bookFieldsSettings.language),
      isbn10: createFormControl(null, this.bookFieldsSettings.isbn10),
      isbn13: createFormControl(null, this.bookFieldsSettings.isbn13),
      visibility: new FormControl(true),
      publisherId: createFormControl(null, this.bookFieldsSettings.publisher),
      publishedDate: createFormControl(null, this.bookFieldsSettings.publishedDate),
      description: new FormControl(null)
    })
  }


  isRequiredField(abstractControl: AbstractControl): boolean {
    return isRequiredField(abstractControl);
  }



  onSubmitBook() {
    console.log(this.manualBookAddForm.value);
    const book  = this.manualBookAddForm.value as CreateManualBookDto;
    console.log(book);
    console.log(book.languageId);
    console.log(book.categoriesIds);
    this.createBookEvent.emit(book)

  }
}


