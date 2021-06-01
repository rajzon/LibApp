import {Bucket} from "@core/models/aggregation";

export class FilterAggregationModel {
    name: string
    buckets: Bucket[]
    moreClicked: boolean

    constructor(name: string, buckets: Bucket[]) {
        this.name = name;
        this.buckets = buckets;
    }
}
