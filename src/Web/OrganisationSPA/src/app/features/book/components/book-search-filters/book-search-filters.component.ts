import {Component, Inject, LOCALE_ID, OnInit, Output, EventEmitter} from '@angular/core';
import {Aggregation} from "@core/models/aggregation";
import {BookManagementFacade} from "../../book-management.facade";
import {ActivatedRoute, Router} from "@angular/router";
import {FilterAggregationModel} from "@core/models/filter-aggregation-model";
import {FilterDateModel} from "@core/models/filter-date-model";
import {CreateFilterDateRange} from "@shared/helpers/search/create-filter-date-range.function";
import {formatDate} from "@angular/common";
import {environment} from "@env";
import {SearchBookQueryDto} from "../../models/search-book-query-dto";

@Component({
  selector: 'app-book-search-filters',
  templateUrl: './book-search-filters.component.html',
  styleUrls: ['./book-search-filters.component.sass']
})
export class BookSearchFiltersComponent implements OnInit {

  @Output() searchEvent = new EventEmitter<SearchBookQueryDto>();

  bookSearchFilters: FilterAggregationModel[];
  modificationDateFilter: FilterDateModel;

  positionOfModificationDateFilter: number = 2
  maxInitialBucketsPerFilter: number = 5
  timeFormat = environment.timeFormat;



  constructor(@Inject(LOCALE_ID) private locale: string,
              private bookManagementFacade: BookManagementFacade,
              private router: Router,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.bookManagementFacade.getSearchBookResult$().subscribe(res => {
      console.log(res.aggregations)
      this.initFilters(res.aggregations);
    })

  }

  initFilters(aggregations: Aggregation[]) {
    console.log(this.bookSearchFilters)
    this.bookSearchFilters = new Array<FilterAggregationModel>();
    for (let i = 0; i < aggregations.length; i++) {
      let selectedFilterBuckets = this.activatedRoute.snapshot.queryParamMap.getAll(aggregations[i].name)

      if (aggregations[i].name === 'visibility') {
        const visibility1 = aggregations[i].buckets.find(f => f.key === '1');
        if (visibility1)
          aggregations[i].buckets.find(f => f.key === '1').key = 'true'

        const visibility0 = aggregations[i].buckets.find(f => f.key === '0');
        if (visibility0)
          aggregations[i].buckets.find(f => f.key === '0').key = 'false'

        // aggregations[i].buckets.find(f => f.key === '0').key = 'false'
        // console.log('fired')


        // selectedFilterBuckets.map(v => {
        //    //v = v? (v === '1'? 'true': 'false') : null;
        //   console.log(v)
        //   return v === '1'? 'true' : 'false';
        // })
      }
      this.bookSearchFilters.push(new FilterAggregationModel(aggregations[i].name,
        aggregations[i].buckets, selectedFilterBuckets));


    }

    const modificationFrom = this.activatedRoute.snapshot.queryParamMap.get('modificationDateFrom')
    const modificationTo = this.activatedRoute.snapshot.queryParamMap.get('modificationDateTo')

    this.modificationDateFilter = CreateFilterDateRange(modificationFrom, modificationTo);

  }

  dateChanged() {
    console.log(this.modificationDateFilter.value);
    console.log(this.locale);
    const modificationFrom = formatDate(this.modificationDateFilter.value[0], this.timeFormat, this.locale)
    const modificationTo = formatDate(this.modificationDateFilter.value[1], this.timeFormat, this.locale)
    console.log(modificationFrom);
    console.log(modificationTo);
    var searchQuery = this.initSearchBookQueryDto()
    console.log(searchQuery);
    this.searchEvent.emit(searchQuery);
    //this.initSearchBookQueryDto(this.activatedRoute, this.modificationDateFilter)
    // this.searchEvent.emit(new SearchBookQueryDto(this.activatedRoute.snapshot.queryParamMap.get('searchTerm'),
    //   this.bookSearchFilters.find(f => f.name === 'categories').buckets.filter(b => b.selectedKey).map(b => b.selectedKey),
    //   ))
  }

  initSearchBookQueryDto(activatedRoute?: ActivatedRoute, bookSearchFilters?: FilterAggregationModel, modificationDateFilter?: FilterDateModel): SearchBookQueryDto {
    return new SearchBookQueryDto(this.activatedRoute.snapshot.queryParamMap.get('searchTerm'),
      this.bookSearchFilters.find(f => f.name === 'categories').buckets.filter(b => b.isKeySelected).map(b => b.key),
      this.bookSearchFilters.find(f => f.name === 'authors').buckets.filter(b => b.isKeySelected).map(b => b.key),
      this.bookSearchFilters.find(f => f.name === 'languages').buckets.filter(b => b.isKeySelected).map(b => b.key),
      this.bookSearchFilters.find(f => f.name === 'publishers').buckets.filter(b => b.isKeySelected).map(b => b.key),
      <boolean[]><any[]>this.bookSearchFilters.find(f => f.name === 'visibility').buckets.filter(b => b.isKeySelected).map(b => b.key),
      this.activatedRoute.snapshot.queryParamMap.get('sortBy'),
      <number><any>this.activatedRoute.snapshot.queryParamMap.get('fromPage'),
      <number><any>this.activatedRoute.snapshot.queryParamMap.get('pageSize'),
      this.modificationDateFilter.value[0], this.modificationDateFilter.value[1]
      )
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

