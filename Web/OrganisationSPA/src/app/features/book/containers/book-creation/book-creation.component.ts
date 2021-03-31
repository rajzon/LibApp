import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import { BookFacade } from '../../book.facade';
import {Category} from "../../models/category";
import {Language} from "../../models/language";
import {Author} from "../../models/author";
import {Publisher} from "../../models/publisher";
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {IFileUploaderStyle} from "@shared/file-uploader/IFileUploaderStyle";
import {CreateManualBookDto} from "../../models/create-manual-book-dto";
import {NgxSpinnerService} from "ngx-spinner";
import {environment} from "@env";
import {Book} from "../../models/book";

@Component({
  selector: 'app-book-creation',
  templateUrl: './book-creation.component.html',
  styleUrls: ['./book-creation.component.sass']
})
export class BookCreationComponent implements OnInit, AfterViewInit {

  isAdding$: Observable<boolean>;

  categories$: Observable<Category[]>;
  languages$: Observable<Language[]>;
  authors$: Observable<Author[]>;
  publishers$: Observable<Publisher[]>;
  newlyAddedBook$: Observable<Book>;

  //Uploader
  uploaderOptions: FileUploaderOptions;
  uploaderStyle: IFileUploaderStyle;
  readonly URL:string = environment.bookApiUrl + 'v1/book/';

  constructor(private bookFacade: BookFacade, private spinner: NgxSpinnerService) {
    this.categories$ = bookFacade.getCategories$();
    this.languages$ = bookFacade.getLanguages$();
    this.authors$ = bookFacade.getAuthors$();
    this.publishers$ = bookFacade.getPublisher$();
    this.isAdding$ = bookFacade.isAdding$();
    this.newlyAddedBook$ = bookFacade.getNewlyAddedBook$();
  }

  ngAfterViewInit(): void {
        this.spinner.show();
    }


  ngOnInit(): void {
    this.bookFacade.loadCategories$().subscribe();
    this.bookFacade.loadLanguages$().subscribe();
    this.bookFacade.loadAuthors$().subscribe();
    this.bookFacade.loadPublishers$().subscribe();


    //Uploader
    this.uploaderStyle = {style: "removeOnly"};

    this.uploaderOptions = {
      // disableMultipart: true, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      // formatDataFunctionIsAsync: true,
      // allowedFileType: ['image'],
      // removeAfterUpload: true,
      // autoUpload: false,
      // formatDataFunction: async (item) => {
      //   return new Promise( (resolve, reject) => {
      //     resolve({
      //       name: item._file.name,
      //       length: item._file.size,
      //       contentType: item._file.type,
      //       date: new Date()
      //     });
      //   });
      // }
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false
    };

    this.bookFacade.setUploader(new FileUploader(this.uploaderOptions));
  }

  addBook(book: CreateManualBookDto): void {
    this.bookFacade.addBookWithPhotos(book);
  }

  uploadImage() {
    this.bookFacade.getNewlyAddedBook$().subscribe(res => {

    });

  }


}
