import {Injectable} from "@angular/core";
import {CategoriesApiService} from "./api/categories-api.service";
import {Observable} from "rxjs";
import {Category} from "./models/category";
import {BookState} from "./state/book.state";
import {map} from "rxjs/operators";
import {LanguagesApiService} from "./api/languages-api.service";
import {Language} from "./models/language";
import {AuthorsApiService} from "./api/authors-api.service";
import {PublishersApiService} from "./api/publishers-api.service";
import {Author} from "./models/author";
import {Publisher} from "./models/publisher";
import {BookApiService} from "./api/book-api.service";
import {Book} from "./models/book";
import {CreateManualBookDto} from "./models/create-manual-book-dto";
import {FileUploader} from "ng2-file-upload";
import {environment} from "@env";

@Injectable({
  providedIn: 'root'
})
export class BookFacade {

  constructor(private categoriesApi: CategoriesApiService,
              private languagesApi: LanguagesApiService,
              private authorsApi: AuthorsApiService,
              private publishersApi: PublishersApiService,
              private bookApi: BookApiService,
              private bookState: BookState) { }

  isAdding$(): Observable<boolean> {
    return this.bookState.isAdding$();
  }

  //Book
  addBookWithPhotos(book: CreateManualBookDto): void  {
    this.bookState.setAdding(true)
    this.bookApi.createBook(book)
      .subscribe((res:Book) => {
        this.bookState.setBook(res);
        this.bookState.getUploader$().subscribe((upl:FileUploader) => {
          console.log(upl);
          this.uploadPhoto(res.id, upl)
        });
      }, (error: any) => {console.log(error); this.bookState.setAdding(false); },
        () => this.bookState.setAdding(false))

  }

  private uploadPhoto(bookId: number, uploader: FileUploader) {

    if (uploader.queue.length < 1)
      return

    const imagesCount = uploader.queue.length;
    uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('IsMain', 'false')
      return {fileItem, form};
    };

    uploader.onBeforeUploadItem = (file) => {
      file.url = environment.bookApiUrl + `v1/book/${bookId}/add-photo`;
    }

    uploader.uploadAll();
    uploader.onCompleteAll = () => {
      console.log(`Uploaded ${imagesCount} requested Images`)
    }
  }

  getNewlyAddedBook$(): Observable<Book> {
    return this.bookState.getNewlyAddedBook$();
  }

  getUploader$(): Observable<FileUploader> {
    return this.bookState.getUploader$();
  }

  setUploader(uploader: FileUploader) {
    this.bookState.setUploader(uploader);
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
