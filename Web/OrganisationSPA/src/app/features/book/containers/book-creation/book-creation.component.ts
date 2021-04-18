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
import {AuthService} from "@core/services/auth.service";

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
  maxResults: number = environment.pagination.itemsPerPageDefault;

  //Uploader
  uploaderStyle: IFileUploaderStyle;

  constructor(private bookFacade: BookFacade,
              private router: Router,
              private spinner: NgxSpinnerService,
              private authService: AuthService) {
    this.categories$ = bookFacade.getCategories$();
    this.languages$ = bookFacade.getLanguages$();
    this.authors$ = bookFacade.getAuthors$();
    this.publishers$ = bookFacade.getPublisher$();
    this.isAdding$ = bookFacade.isAdding$();
    this.newlyAddedBook$ = bookFacade.getNewlyAddedBook$();
    this.isLoading$ = bookFacade.isLoading$();
  }
  reloadSearchResult: boolean;
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

    const uploaderOptions: FileUploaderOptions = {
      authToken: 'Bearer ' + this.authService.getAccessToken(),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false
    };

    this.bookFacade.setManualBookImgUploader(new FileUploader(uploaderOptions));
  }

  addBook(book: CreateManualBookDto): void {
    this.bookFacade.addBookWithPhotos(book);
  }

  addBookUsingApi(addCommand: CreateBookUsingApiDto): void {
    console.log(addCommand);
    this.bookFacade.addBookWithPhotoUsingApi(addCommand);
  }

  changePage(startIndex: number, maxResults: number) {
    this.searchBooks(this.query, this.searchParam, startIndex, maxResults)
  }

  searchBooks(query: string, searchParam: 'intitle' | 'inauthor' | 'isbn', startIndex?: number, maxResults?: number) {
    this.maxResults = maxResults?? this.maxResults;

    this.query = query
    this.searchParam = searchParam;
    this.bookFacade.searchBooks$(query, searchParam, startIndex, this.maxResults).subscribe();
    this.booksFromSearch$ = this.bookFacade.getBooksFromSearch$();
  }


}
