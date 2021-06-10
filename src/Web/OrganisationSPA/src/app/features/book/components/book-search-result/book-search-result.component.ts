import {AfterViewInit, Component, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {SearchBookResultDto} from "../../models/search-book-result-dto";
import {Book} from "../../models/book";
import {AllowedSorting} from "../../api/search-settings-api.service";
import {BookManagementFacade} from "../../book-management.facade";
import {Observable, Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {EventEmitter} from "@angular/core"
import {environment} from "@env";

@Component({
  selector: 'app-book-search-result',
  templateUrl: './book-search-result.component.html',
  styleUrls: ['./book-search-result.component.sass']
})
export class BookSearchResultComponent implements OnInit, OnDestroy {

  @Input() searchResult: SearchBookResultDto
  @Input() defaultSorting: string
  @Input() selectedMaxResult: number
  @Input() currentPageNumber: number
  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>()

  allowedSortingSubscription: Subscription

  //sorting
  selectedSorting: string
  allowedSorting: AllowedSorting[];

  //pagination
  itemsPerPageOptions: number[] = environment.pagination.itemsPerPageOpts;

  constructor(private bookManagementFacade: BookManagementFacade, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.allowedSortingSubscription = this.bookManagementFacade.getBookManagementSearchAllowedSorting$().subscribe((res: AllowedSorting[]) => {
      this.initSortingDesc(res)
    })

  }

  ngOnDestroy(): void {
    this.allowedSortingSubscription.unsubscribe();
  }

  initSortingDesc(sorting: AllowedSorting[]): void {
    this.allowedSorting = new Array<AllowedSorting>()
    sorting.forEach(s => {
      let ascFieldDesc;
      let descFieldDesc;
      if (s.field === 'modificationDate') {
        descFieldDesc = 'Latest Modified'
        ascFieldDesc = 'Oldest Modified'


      } else if (s.field === 'title') {
        descFieldDesc = 'Title: A-Z'
        ascFieldDesc = 'Title: Z-A'
      }

      if (ascFieldDesc) {
        this.allowedSorting.push(new AllowedSorting(s.field, 'asc', ascFieldDesc))
      }

      if (descFieldDesc) {
        this.allowedSorting.push(new AllowedSorting(s.field, 'desc', descFieldDesc))
      }

    })

    this.selectedSorting = this.activatedRoute.snapshot.queryParamMap.get('sortBy')??
      this.allowedSorting.find(s => s.combinedValue = this.defaultSorting).combinedValue
  }

  sortAndSearch(): void {
    console.log(this.selectedSorting)
    const query = this.initSearchBookQueryDto(this.activatedRoute)
    this.searchEvent.emit(query);
  }

  initSearchBookQueryDto(activatedRoute: ActivatedRoute): SearchBookQueryDto {
    return new SearchBookQueryDto(activatedRoute.snapshot.queryParamMap.get('searchTerm'),
      activatedRoute.snapshot.queryParamMap.getAll('categories'),
      activatedRoute.snapshot.queryParamMap.getAll('authors'),
      activatedRoute.snapshot.queryParamMap.getAll('languages'),
      activatedRoute.snapshot.queryParamMap.getAll('publishers'),
      <boolean[]><any[]>activatedRoute.snapshot.queryParamMap.getAll('visibility'),
      this.selectedSorting,
      this.currentPageNumber,
      this.selectedMaxResult,
      new Date(activatedRoute.snapshot.queryParamMap.get('modificationDateFrom')), new Date(activatedRoute.snapshot.queryParamMap.get('modificationDateTo'))
    )
  }

  onMaxResultsChanged(event: any): void {
    this.currentPageNumber = 1
    const query = this.initSearchBookQueryDto(this.activatedRoute)
    this.searchEvent.emit(query)
  }

  pageChanged(event: any) {
    if (this.currentPageNumber !== event.page) {
      this.currentPageNumber = event.page
      const query = this.initSearchBookQueryDto(this.activatedRoute)
      this.searchEvent.emit(query)
    }

  }




  selectMainImage(book: Book): string {
    return book.images? book.images.find(i => i.isMain)?.url : ''
  }

  visibilityAsMeaningfulText(visibility: boolean): string {
    return visibility? 'Visible': 'Hidden'
  }

  getAllowedSorting$(): Observable<AllowedSorting[]> {
    return this.bookManagementFacade.getBookManagementSearchAllowedSorting$()
  }

}
