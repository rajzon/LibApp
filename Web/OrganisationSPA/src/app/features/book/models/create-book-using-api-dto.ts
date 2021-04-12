import {Author} from "./author";
import {FileUploader} from "ng2-file-upload";

export class AuthorDto {
  firstName: string;
  lastName: string;
}

export class CreateBookUsingApiDto {
    book: BookToCreateDto
    uploader: FileUploader;
}

export class BookToCreateDto {
  title: string;
  description: string;
  isbn10: string;
  isbn13: string;
  pageCount: number;
  visibility: boolean;
  languageName: string;
  author: AuthorDto;
  publisherName: string;
  categoriesNames: string[];
  publishedDate: Date;
}
