export class CreateManualBookDto {
  title: string
  authorId: number;
  categoriesIds: number[];
  languageId: number;
  publisherId: number;
  publishedDate: Date;
  pageCount: number;
  isbn10: string;
  isbn13: string;
  visibility: boolean;
  description: string;
}
