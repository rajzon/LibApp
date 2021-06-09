import {environment} from "@env";

export class SearchBookQueryDto {
  private _pageSize: number

  get PageSize(): number {
    return this._pageSize
  }

  readonly searchTerm: string
  readonly categories: string[]
  readonly authors: string[]
  readonly languages: string[]
  readonly publishers: string[]
  readonly visibility: boolean[]

  readonly sortBy: string
  readonly fromPage: number
  readonly modificationDateFrom: Date
  readonly modificationDateTo: Date

  constructor(searchTerm?: string, categories?: string[],
              authors?: string[], languages?: string[],
              publishers?: string[], visibility?: boolean[],
              sortBy?: string, fromPage?: number,
              pageSize?: number, modificationDateFrom?: Date,
              modificationDateTo?: Date) {
    console.log(searchTerm)
    this.searchTerm = searchTerm;
    console.log(this.searchTerm)
    this.categories = categories;
    this.authors = authors;
    this.languages = languages;
    this.publishers = publishers;
    this.visibility = visibility;
    console.log(this.visibility)

    this.sortBy = sortBy;
    this.fromPage = fromPage?? 1;

    const currentDate = new Date()
    this.modificationDateFrom = modificationDateFrom?? new Date(Date.UTC(2015, 0, 1))
    this.modificationDateTo = modificationDateTo?? new Date(currentDate.getUTCFullYear()
      ,currentDate.getUTCMonth(), currentDate.getUTCMinutes(),
      23, 59, 59)
    console.log(this.modificationDateFrom)

    this.initPageSize(pageSize);
  }


  private initPageSize(pageSize: number): void {
    const allowedSize = environment.pagination.itemsPerPageOpts
      if (allowedSize.some(a => pageSize === a))
        this._pageSize = pageSize
      else
        this._pageSize = environment.pagination.itemsPerPageDefault
  }


}
