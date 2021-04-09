import {Injectable} from "@angular/core";
import {CategoriesApiService} from "./api/categories-api.service";
import {Observable, of} from "rxjs";
import {Category} from "./models/category";
import {BookState} from "./state/book.state";
import {catchError, map} from "rxjs/operators";
import {LanguagesApiService} from "./api/languages-api.service";
import {Language} from "./models/language";
import {AuthorsApiService} from "./api/authors-api.service";
import {PublishersApiService} from "./api/publishers-api.service";
import {Author} from "./models/author";
import {Publisher} from "./models/publisher";
import {BookApiService} from "./api/book-api.service";
import {Book} from "./models/book";
import {CreateManualBookDto} from "./models/create-manual-book-dto";
import {FileUploader, FileUploaderOptions} from "ng2-file-upload";
import {environment} from "@env";
import {UploaderState} from "@core/state/uploader.state";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {GoogleBookApiService} from "./api/google-book-api.service";
import {BookToCreateDto, CreateBookUsingApiDto} from "./models/create-book-using-api-dto";

@Injectable({
  providedIn: 'root'
})
export class BookFacade {

  constructor(private categoriesApi: CategoriesApiService,
              private languagesApi: LanguagesApiService,
              private authorsApi: AuthorsApiService,
              private publishersApi: PublishersApiService,
              private bookApi: BookApiService,
              private googleApi: GoogleBookApiService,
              private router: Router,
              private toastr: ToastrService,
              private uploaderState: UploaderState,
              private bookState: BookState) { }

  isAdding$(): Observable<boolean> {
    return this.bookState.isAdding$();
  }

  isLoading$(): Observable<boolean> {
    return this.bookState.isLoading$();
  }

  //Book
  addBookWithPhotos(book: CreateManualBookDto): void {
    this.bookState.setAdding(true)
    this.bookApi.createBook(book)
      .subscribe((res:Book) => {
        this.bookState.setBook(res);
          console.log(res);
        this.uploaderState.getManualBookImgUploader$().subscribe((upl:FileUploader) => {
          console.log(upl);
          this.uploadPhoto(res.id, upl)
        });
      }, (error: any) => {
          console.log(error);
          this.bookState.setAdding(false);
          this.toastr.error('TODO: info should came from server')
        },
        () => {
          this.bookState.setAdding(false);
          //TODO info should came from server
          this.toastr.success('TODO: info should came from server');
          this.router.navigateByUrl('/book');
      })

  }

  getNewlyAddedBook$(): Observable<Book> {
    return this.bookState.getNewlyAddedBook$();
  }



  searchBooks$(query: string, searchParam: 'intitle' | 'inauthor' | 'isbn', startIndex?: number, maxResults?: number): Observable<any[]> {
    this.bookState.setLoading(true);
    return this.googleApi.getBooks$(query, searchParam, startIndex, maxResults).pipe(map(books => {
      this.bookState.setGoogleBooks(books);
      this.bookState.setLoading(false);
      return books;
    }),catchError((err, caught) => {
      this.bookState.setLoading(false);
      this.toastr.error(err);
      return of(err);
    }));
  }

  getBooksFromSearch$(): Observable<any[]> {
    return this.bookState.getBooksFromSearch$();
  }


  addBookWithPhotoUsingApi(addCommand: CreateBookUsingApiDto): void {
    this.bookState.setAdding(true)
    console.log(addCommand.book);
    this.bookApi.createBookUsingApi(addCommand.book).subscribe((res:Book) => {
      this.bookState.setBook(res);
      console.log(res);
      this.uploadPhoto(res.id, addCommand.uploader);
    }, (error: any) => {
        console.log(error);
        this.bookState.setAdding(false);
        this.toastr.error(error.error.errors)
    },
      () => {
        this.bookState.setAdding(false);
        this.toastr.success('TODO: info should came from server');
        this.router.navigateByUrl('/book');
      })
  }

  //Uploader
  private uploadPhoto(bookId: number, uploader: FileUploader): void {

    const initalQueue = uploader.queue.length;
    if (initalQueue < 1)
      return


    let imagesCount = 0;
    uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('IsMain', 'false')
      return {fileItem, form};
    };

    uploader.onBeforeUploadItem = (file) => {
      file.url = environment.bookApiUrl + `v1/book/${bookId}/add-photo`;
    }
    uploader.onSuccessItem = (file, response, status, headers) => {
      imagesCount++;
    }

    uploader.onErrorItem = (item, response, status, header) => {
      this.toastr.error(`error occured during uploading file ${item._file.name}`);
    }

    uploader.uploadAll();
    uploader.onCompleteAll = () => {
      imagesCount === initalQueue? this.toastr.success(`Uploaded ${imagesCount} requested Images`):
        this.toastr.info(`Uploaded ${imagesCount} requested Images but ${initalQueue-imagesCount} images wasn't uploaded`)

      console.log(`Uploaded ${imagesCount} requested Images`)
    }
  }

  getManualBookImgUploader$(): Observable<FileUploader> {
    return this.uploaderState.getManualBookImgUploader$();
  }

  setManualBookImgUploader(uploader: FileUploader): void {
    this.uploaderState.setManualBookImgUploader(uploader);
  }




  //Categories
  getCategories$(): Observable<Category[]> {
    return this.bookState.getCategories$();
  }

  loadCategories$(): Observable<Category[]> {
    return this.categoriesApi.getCategories$().pipe(map(categories => {
      this.bookState.setCategories(categories);
      return categories
    }));

  }

  //Languages
  getLanguages$(): Observable<Language[]> {
    return this.bookState.getLanguages$();
  }

  loadLanguages$(): Observable<Language[]> {
    return this.languagesApi.getLanguages$().pipe(map(languages => {
      this.bookState.setLanguages(languages);
      return languages
    }));

  }

  //Authors
  getAuthors$(): Observable<Author[]> {
    return this.bookState.getAuthors$();
  }

  loadAuthors$(): Observable<Author[]> {
    return this.authorsApi.getAuthors$().pipe(map(authors => {
     authors = authors.map(author => ({
        ...author,
        fullName: `${author.firstName} ${author.lastName}`
      }));
      this.bookState.setAuthors(authors);
      return authors;
    }))
  }

  //Publishers
  getPublisher$(): Observable<Publisher[]> {
    return this.bookState.getPublishers$();
  }

  loadPublishers$(): Observable<Publisher[]> {
    return this.publishersApi.getPublishers$().pipe(map(publishers => {
      this.bookState.setPublishers(publishers);
      return publishers;
    }))
  }

}
