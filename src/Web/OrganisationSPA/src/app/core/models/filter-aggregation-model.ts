import {Bucket} from "@core/models/aggregation";

export class FilterAggregationModel {
    name: string
    buckets: Bucket[]
    moreClicked: boolean

    constructor(name: string, buckets: Bucket[], selectedBuckets: string[]) {
      this.name = name;
      this.buckets = buckets;
      console.log(selectedBuckets)
      const bucketsToBeSelected = this.buckets.filter(b => selectedBuckets.includes(b.key));
      if (bucketsToBeSelected)
          bucketsToBeSelected.forEach(b => {
            // b.selectedKey = selectedBuckets.find(s => s == b.key);
            b.isKeySelected = true
          })

    }
}
