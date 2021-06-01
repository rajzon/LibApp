import {Component, OnInit} from '@angular/core';
import {Aggregation} from "@core/models/aggregation";
import {BookManagementFacade} from "../../book-management.facade";
import {ActivatedRoute} from "@angular/router";
import {FilterAggregationModel} from "@core/models/filter-aggregation-model";
import {FilterDateModel} from "@core/models/filter-date-model";

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
    console.log(this.bookSearchFilters)
    this.bookManagementFacade.getSearchBookResult$().subscribe(res => {
      console.log(res.aggregations)
      this.initFilters(res.aggregations);
    })

    console.log(this.bookSearchFilters)

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

    console.log(this.bookSearchFilters)
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

export function CreateFilterDateRange(modificationFrom: string, modificationTo: string ): FilterDateModel {
  const maxDateAsNumber = 8640000000000000;

  if (modificationFrom && modificationTo)
    return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(modificationTo)])
  else if (modificationFrom)
    return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(maxDateAsNumber)])
  else if (modificationTo)
    return new FilterDateModel('Modification Date', [new Date(null), new Date(modificationTo)])
  else
    return new FilterDateModel('Modification Date', [new Date(null), new Date(maxDateAsNumber)])
}

