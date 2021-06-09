import {Author} from "./author";
import {Language} from "./language";
import {Publisher} from "./publisher";
import {Category} from "./category";

export class Book {
  id: number;
  ean13: string;
  title: string;
  authors: Author[]
  pageCount: number;
  language: Language
  isbn10: string;
  isbn13: string;
  publisher: Publisher;
  publishedDate: Date | string;
  visibility: boolean;
  publicSiteLink: string;
  description: string;
  images: Image[];
  categories: Category[]

  modificationDate: Date | string

  constructor() {
    this.authors = new Array<Author>();
    this.language = new Language();
    this.publisher = new Publisher();
  }
}

export class Image {
  url: string
  isMain: boolean
}
