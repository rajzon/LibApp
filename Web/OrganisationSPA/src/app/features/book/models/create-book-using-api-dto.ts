import {Author} from "./author";

export class AuthorDto {
  firstName: string;
  lastName: string;
}

export class CreateBookUsingApiDto {
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
