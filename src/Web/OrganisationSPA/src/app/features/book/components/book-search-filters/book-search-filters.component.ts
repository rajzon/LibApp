import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  ViewChildren
} from '@angular/core';
import {BookManagementFacade} from "../../book-management.facade";
import {ActivatedRoute, Router} from "@angular/router";
import {FilterAggregationModel} from "@core/models/filter-aggregation-model";
import {FilterDateModel} from "@core/models/filter-date-model";
import {environment} from "@env";
import {SearchBookQueryDto} from "../../models/search-book-query-dto";
import {fromEvent, Observable, Subscription} from "rxjs";
import {debounceTime} from "rxjs/operators";
import {initFiltersForView} from "@shared/helpers/search/init-filters.function";


@Component({
  selector: 'app-book-search-filters',
  templateUrl: './book-search-filters.component.html',
  styleUrls: ['./book-search-filters.component.sass'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookSearchFiltersComponent implements OnInit, OnDestroy, AfterViewInit {

  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>();
  @ViewChildren('checkbox') checkboxes:  QueryList<ElementRef>;
  @ViewChildren('moreBtn') moreButtons:  QueryList<ElementRef>;

  searchSubscription: Subscription
  clickedCheckbox$: Observable<Event>;


  //Models
  bookSearchFilters: FilterAggregationModel[];
  modificationDateFilter: FilterDateModel;

  //Configs
  positionOfModificationDateFilter: number = 2
  maxInitialBucketsPerFilter: number = 5
  debounceTimeToSearch: number = 2000;
  timeFormat = environment.timeFormat;


  constructor(private bookManagementFacade: BookManagementFacade,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.searchSubscription = this.bookManagementFacade.getSearchBookResult$().subscribe(res => {

      let modificationDateRange = new Map<string, string>()
      modificationDateRange.set("modificationDateFrom", "modificationDateTo")
      const filters = initFiltersForView(res?.aggregations, this.activatedRoute, modificationDateRange);
      this.bookSearchFilters = filters[0]
      this.modificationDateFilter = filters[1]
    })

  }

  ngOnDestroy(): void {
    this.searchSubscription.unsubscribe();
  }

  ngAfterViewInit(): void  {
    this.initFiltersSelectionListener()
  }

  searchBooks(): void {
    const searchQuery = this.initSearchBookQueryDto(this.activatedRoute, this.bookSearchFilters, this.modificationDateFilter)
    this.searchEvent.emit(searchQuery);
  }

  initSearchBookQueryDto(activatedRoute: ActivatedRoute, bookSearchFilters: FilterAggregationModel[], modificationDateFilter: FilterDateModel): SearchBookQueryDto {
    return new SearchBookQueryDto(activatedRoute.snapshot.queryParamMap.get('searchTerm'),
      bookSearchFilters.find(f => f.name === 'categories').buckets.filter(b => b.isKeySelected).map(b => b.key),
      bookSearchFilters.find(f => f.name === 'authors').buckets.filter(b => b.isKeySelected).map(b => b.key),
      bookSearchFilters.find(f => f.name === 'languages').buckets.filter(b => b.isKeySelected).map(b => b.key),
      bookSearchFilters.find(f => f.name === 'publishers').buckets.filter(b => b.isKeySelected).map(b => b.key),
      <boolean[]><any[]>bookSearchFilters.find(f => f.name === 'visibility').buckets.filter(b => b.isKeySelected).map(b => b.key),
      activatedRoute.snapshot.queryParamMap.get('sortBy'),
      <number><any>activatedRoute.snapshot.queryParamMap.get('fromPage'),
      <number><any>activatedRoute.snapshot.queryParamMap.get('pageSize'),
      modificationDateFilter.value[0], modificationDateFilter.value[1]
      )
  }

  initFiltersSelectionListener(): void  {
    if (this.checkboxes.length > 0) {
      this.clickedCheckbox$ = fromEvent(this.checkboxes.map(c => c.nativeElement), 'click');
      this.clickedCheckbox$.pipe(debounceTime(this.debounceTimeToSearch))
        .subscribe(r => this.searchBooks())
    }

    if (this.moreButtons.length > 0) {
      let moreButtonsClicked$ : Observable<any> = fromEvent(this.moreButtons.map(m => m.nativeElement), 'click')
      moreButtonsClicked$.subscribe(r => {
        this.clickedCheckbox$ = fromEvent(this.checkboxes.map(c => c.nativeElement), 'click')
        this.clickedCheckbox$.pipe(debounceTime(this.debounceTimeToSearch))
          .subscribe(r => this.searchBooks())
      })
    }
  }

  isModificationDatePosition(i: number): boolean {
    return i === this.positionOfModificationDateFilter;
  }

  isBelowMaxInitialBuckets(i: number): boolean {
    return i < this.maxInitialBucketsPerFilter;
  }

  isAtMaxInitialBucket(i): boolean {
    return i === this.maxInitialBucketsPerFilter;
  }

  isAboveMaxInitialBuckets(i: number): boolean {
    return i >= this.maxInitialBucketsPerFilter;
  }


}

