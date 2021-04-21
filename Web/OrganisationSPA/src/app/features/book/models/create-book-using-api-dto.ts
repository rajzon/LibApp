import {Author} from "./author";
import {FileUploader} from "ng2-file-upload";

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
  author: string;
  publisherName: string;
  categoriesNames: string[];
  publishedDate: Date;
}
