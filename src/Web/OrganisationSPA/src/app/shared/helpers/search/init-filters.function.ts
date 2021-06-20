import {Aggregation, AggregationType} from "@core/models/aggregation";
import {ActivatedRoute} from "@angular/router";
import {FilterAggregationModel} from "@core/models/filter-aggregation-model";
import {FilterDateModel} from "@core/models/filter-date-model";
import {CreateFilterDateRange} from "@shared/helpers/search/create-filter-date-range.function";

export function initFiltersForView(aggregations: Aggregation[], activatedRoute: ActivatedRoute, dateRange?: Map<string, string>): [FilterAggregationModel[], FilterDateModel] {
    if (! aggregations) return [[], null];

    let bookSearchFilters = new Array<FilterAggregationModel>();

    for (let i = 0; i < aggregations.length; i++) {

        let selectedFilterBuckets = activatedRoute.snapshot.queryParamMap.getAll(aggregations[i].name)

        if (aggregations[i].type === AggregationType.Boolean) {
            const visibility1 = aggregations[i].buckets.find(f => f.key === '1');
            if (visibility1)
                aggregations[i].buckets.find(f => f.key === '1').key = 'true'

            const visibility0 = aggregations[i].buckets.find(f => f.key === '0');
            if (visibility0)
                aggregations[i].buckets.find(f => f.key === '0').key = 'false'

        }
        bookSearchFilters.push(new FilterAggregationModel(aggregations[i].name,
            aggregations[i].buckets, selectedFilterBuckets));

    }

    let modificationDateFilter;
    dateRange.forEach((value, key) => {

        const modificationFrom = activatedRoute.snapshot.queryParamMap.get(key)
        const modificationTo = activatedRoute.snapshot.queryParamMap.get(value)
        modificationDateFilter = CreateFilterDateRange(modificationFrom, modificationTo);

        return [bookSearchFilters, modificationDateFilter]
    })


    return [bookSearchFilters, modificationDateFilter]
}
