import {Component, OnInit} from '@angular/core';
import {Aggregation} from "@core/models/aggregation";
import {BookManagementFacade} from "../../book-management.facade";
import {ActivatedRoute} from "@angular/router";
import {FilterAggregationModel} from "@core/models/filter-aggregation-model";
import {FilterDateModel} from "@core/models/filter-date-model";
import {CreateFilterDateRange} from "@shared/helpers/search/create-filter-date-range.function";

@Component({
  selector: 'app-book-search-filters',
  templateUrl: './book-search-filters.component.html',
  styleUrls: ['./book-search-filters.component.sass']
})
export class BookSearchFiltersComponent implements OnInit {

  bookSearchFilters: FilterAggregationModel[];
  modificationDateFilter: FilterDateModel;

  positionOfModificationDateFilter: number = 2
  maxInitialBucketsPerFilter: number = 5


  constructor(private bookManagementFacade: BookManagementFacade,
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
      this.bookSearchFilters.push(new FilterAggregationModel(aggregations[i].name,
        aggregations[i].buckets));
    }

    const modificationFrom = this.activatedRoute.snapshot.queryParamMap.get('modificationDateFrom')
    const modificationTo = this.activatedRoute.snapshot.queryParamMap.get('modificationDateTo')

    this.modificationDateFilter = CreateFilterDateRange(modificationFrom, modificationTo);

  }

  dateChanged($event: any) {
    console.log($event);
  }

  isModificationDatePosition(i: number): boolean {
    return i === this.positionOfModificationDateFilter;
  }

  isBelowMaxInitialBuckets(i: number): boolean {
    return i < this.maxInitialBucketsPerFilter;
  }

  isAboveMaxInitialBuckets(i: number): boolean {
    return i >= this.maxInitialBucketsPerFilter;
  }



}

