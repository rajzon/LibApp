import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
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
import {Router} from "@angular/router";
import {CreateBookUsingApiDto} from "../../models/create-book-using-api-dto";
import {PaginationDto} from "../../models/pagination-dto";

@Component({
  selector: 'app-book-creation',
  templateUrl: './book-creation.component.html',
  styleUrls: ['./book-creation.component.sass']
})
export class BookCreationComponent implements OnInit, AfterViewInit {

  isAdding$: Observable<boolean>;
  isLoading$: Observable<boolean>;

  categories$: Observable<Category[]>;
  languages$: Observable<Language[]>;
  authors$: Observable<Author[]>;
  publishers$: Observable<Publisher[]>;
  newlyAddedBook$: Observable<Book>;
  booksFromSearch$: Observable<any[]>;

  //search/pagination
  query: string;
  searchParam: 'intitle' | 'inauthor' | 'isbn';

  //Uploader
  uploaderOptions: FileUploaderOptions;
  uploaderStyle: IFileUploaderStyle;

  constructor(private bookFacade: BookFacade,
              private router: Router,
              private spinner: NgxSpinnerService) {
    this.categories$ = bookFacade.getCategories$();
    this.languages$ = bookFacade.getLanguages$();
    this.authors$ = bookFacade.getAuthors$();
    this.publishers$ = bookFacade.getPublisher$();
    this.isAdding$ = bookFacade.isAdding$();
    this.newlyAddedBook$ = bookFacade.getNewlyAddedBook$();
    this.isLoading$ = bookFacade.isLoading$();
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

  addBookUsingApi(book: CreateBookUsingApiDto): void {
    console.log(book);
    this.bookFacade.addBookWithPhotoUsingApi(book);
  }

  changePage(startIndex: number, maxResults: number) {
    this.searchBooks(this.query, this.searchParam, startIndex, maxResults)
  }

  searchBooks(query: string, searchParam: 'intitle' | 'inauthor' | 'isbn', startIndex?: number, maxResults?: number) {
    console.log(query);
    console.log(searchParam);
    console.log(startIndex);
    console.log(maxResults);
    maxResults = maxResults?? environment.pagination.itemsPerPageDefault

    console.log(maxResults)
    console.log(environment.pagination.itemsPerPageDefault)

    this.query = query
    this.searchParam = searchParam;
    this.bookFacade.searchBooks$(query, searchParam, startIndex, maxResults).subscribe();
    this.booksFromSearch$ = this.bookFacade.getBooksFromSearch$();
  }


}
