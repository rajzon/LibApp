import {Author} from "./author";
import {Language} from "./language";
import {Publisher} from "./publisher";

export class Book {
  id: number;
  title: string;
  author: Author
  pageCount: number;
  language: Language
  isbn10: string;
  isbn13: string;
  publisher: Publisher;
  publishedDate: Date;
  visibility: boolean;
  publicSiteLink: string;
  description: string;

  constructor() {
    this.author = new Author();
    this.language = new Language();
    this.publisher = new Publisher();
  }
}
