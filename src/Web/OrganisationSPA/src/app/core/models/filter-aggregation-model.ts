import {Bucket} from "@core/models/aggregation";

export class FilterAggregationModel {
    name: string
    buckets: Bucket[]
    moreClicked: boolean

    constructor(name: string, buckets: Bucket[], selectedBuckets: string[]) {
      this.name = name;
      this.buckets = buckets;
      const bucketsToBeSelected = this.buckets.filter(b => selectedBuckets.includes(b.key));
      if (bucketsToBeSelected)
          bucketsToBeSelected.forEach(b => {
            b.isKeySelected = true
          })

    }
}
