import {
  AfterContentChecked,
  AfterContentInit, AfterViewChecked, AfterViewInit,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {BookApiEditModalComponent} from "../book-api-edit-modal/book-api-edit-modal.component";
import {BookToCreateDto, CreateBookUsingApiDto} from "../../models/create-book-using-api-dto";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {PaginationDto} from "../../models/pagination-dto";
import {environment} from "@env";
import {SearchItemVolume, SearchResultDto} from "../../models/search-result-dto";
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {createFormControl} from "@shared/helpers/forms/create-form-control.function";
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {AuthService} from "@core/services/auth.service";

@Component({
  selector: 'app-book-api-search-result',
  templateUrl: './book-api-search-result.component.html',
  styleUrls: ['./book-api-search-result.component.sass']
})
export class BookApiSearchResultComponent implements OnInit, OnChanges, AfterViewInit {

  @Output() addEvent = new EventEmitter<CreateBookUsingApiDto>();
  @Output() changePageEvent = new EventEmitter<PaginationDto>()
  @Input() searchResult: SearchResultDto;
  @Input() uploaderStyle: IFileUploaderStyle;
  modalRef: BsModalRef;
  bookFieldsSettings = environment.book;
  addBooksForms: FormGroup[];

  //pagination
  itemsPerPageOptions: number[] = environment.pagination.itemsPerPageOpts;
  @Input() maxResults: number;
  startIndex = 0;
  initialPage: number;
  @Input() reload: boolean;
  @Output() reloadEvent = new EventEmitter<boolean>();

  uploaders: FileUploader[];

  constructor(private modalService: BsModalService,
              private cd: ChangeDetectorRef,
              private authService: AuthService) {
  }


  ngAfterViewInit(): void {
    this.cd.detectChanges();
    }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.reload?.currentValue === true) {
      this.reloadEvent.emit(false);
      this.initialPage = 1;
    }

    if (this.searchResult !== null &&
      changes.searchResult?.currentValue !== changes.searchResult?.previousValue) {
      this.initForms()

      const uploaderOptions: FileUploaderOptions = {
        authToken: 'Bearer ' + this.authService.getAccessToken(),
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: false
      };

      let uploaders = new Array<FileUploader>();

      for (let i= 0; i<this.addBooksForms.length; i++){
        uploaders.push(new FileUploader(uploaderOptions));
      }
      this.uploaders = uploaders;
      console.log(this.uploaders);
    }
  }

  ngOnInit(): void { }

  openImageInNewTab( event: any, url: string,): void {
    if (!url)
      return;
    window.open(url)
  }

  //Pagination
  pageChanged(event: any): void {
    this.startIndex = (event.page - 1) * this.maxResults;
    const searchPagination: PaginationDto = {
      startIndex: this.startIndex,
      maxResults: this.maxResults,
    }
    this.changePageEvent.emit(searchPagination)
  }

  onMaxResultsChanged(event: any): void {
    console.log(event)
    const searchPagination: PaginationDto = {
      startIndex:0,
      maxResults: event
    }
    this.cd.detectChanges();
    this.initialPage = 1
    this.changePageEvent.emit(searchPagination)
  }

  //AddEvent
  addAfterEdit(volumeInfo: any, uploader: FileUploader): void {
    const initialState = {
      volumeInfo: volumeInfo,
      uploaderStyle: this.uploaderStyle,
      uploader: uploader
    };
    this.modalRef = this.modalService.show(BookApiEditModalComponent, {initialState});
    this.modalRef.setClass('modal-lg')
    this.modalRef.content.addCommand$.subscribe((res:CreateBookUsingApiDto) => {
      this.addEvent.emit(res);
    })
  }

  add(volumeInfo: any, uploader: FileUploader): void {
    console.log(volumeInfo);

    const book: BookToCreateDto = {
      title: volumeInfo?.title,
      description: volumeInfo?.description,
      isbn10: volumeInfo.isbn10,
      isbn13: volumeInfo.isbn13,
      pageCount: volumeInfo?.pageCount,
      visibility: volumeInfo.visibility,
      languageName: volumeInfo?.languageName,
      //TODO later add all authors instead of only first
      author: volumeInfo?.authors[0].author,
      publisherName: volumeInfo?.publisherName,
      categoriesNames: volumeInfo?.categoriesNames,
      publishedDate: new Date(volumeInfo?.publishedDate)
    }
    const addCommand: CreateBookUsingApiDto = {
      uploader: uploader,
      book: book
    }
    console.log(addCommand);

    this.addEvent.emit(addCommand)
  }
  ///////////////

  ///////////Form
  getCategoriesFromForm(formIndex: any): FormArray {
    return this.addBooksForms[formIndex].controls['categoriesNames'] as FormArray;
  }

  getAuthorsFromForm(formIndex: any): FormArray {
    return this.addBooksForms[formIndex].controls['authors'] as FormArray;
  }

  private initForms() {
    this.addBooksForms = new Array<FormGroup>();

    this.searchResult.items.forEach(item => {
      let addBookForm = new FormGroup({
        title: createFormControl(item.volumeInfo.title, this.bookFieldsSettings.title),
        pageCount: createFormControl(item.volumeInfo.pageCount, this.bookFieldsSettings.pageCount),
        languageName: createFormControl(item.volumeInfo.language, this.bookFieldsSettings.language.languageName),
        isbn10: createFormControl(item.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_10')[0]?.identifier, this.bookFieldsSettings.isbn10),
        isbn13: createFormControl(item.volumeInfo.industryIdentifiers.filter(x => x.type === 'ISBN_13')[0]?.identifier, this.bookFieldsSettings.isbn13),
        visibility: new FormControl(true),
        publisherName: createFormControl(item.volumeInfo.publisher, this.bookFieldsSettings.publisher.publisherName),
        publishedDate: createFormControl(item.volumeInfo.publishedDate, this.bookFieldsSettings.publishedDate),
        description: new FormControl(item.volumeInfo.description),
        image: new FormControl(item.volumeInfo.imageLinks?.thumbnail, Validators.nullValidator)
      });
      this.registerCategoriesFormArray(item.volumeInfo, addBookForm);
      this.registerAuthorsFormArray(item.volumeInfo, addBookForm);


      this.addBooksForms.push(addBookForm);

    })
  }

  private registerCategoriesFormArray(volumeInfo: SearchItemVolume, addBookForm: FormGroup) {
    volumeInfo.categories = volumeInfo.categories ?? new Array<string>();
    const controls = volumeInfo.categories.map(x => {
      return createFormControl(x, this.bookFieldsSettings.categories.name);
    })
    addBookForm.registerControl('categoriesNames', new FormArray(controls,
      this.bookFieldsSettings.categories.required?
        Validators.required: Validators.nullValidator));
  }

  private registerAuthorsFormArray(volumeInfo: SearchItemVolume, addBookForm: FormGroup) {
    volumeInfo.authors = volumeInfo.authors ?? new Array<string>();
    const controls = volumeInfo.authors.map(x => {
      return new FormGroup({
        author: createFormControl(x, this.bookFieldsSettings.author)
      });
    })
    addBookForm.registerControl('authors', new FormArray(controls,
      this.bookFieldsSettings.author.required?
        Validators.required: Validators.nullValidator));
  }
/////////////////////////////

}
