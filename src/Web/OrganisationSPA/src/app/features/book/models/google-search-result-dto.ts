export class GoogleSearchItemVolume {
  title: string;
  //TODO: consider change to array when API will accept collection of authors
  authors: string[]
  categories: any[];
  industryIdentifiers: any[];
  language: string;
  publishedDate: string;
  publisher: string;
  pageCount: number
  imageLinks: any;
  description: string;
}

export interface GoogleSearchItem {
  volumeInfo: GoogleSearchItemVolume
}

export interface GoogleSearchResultDto {
    totalItems: number;
    items: GoogleSearchItem[];
}
