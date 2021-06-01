export class Aggregation {
  name: string
  buckets: Bucket[]
}

export class Bucket {
  key: string
  count: number
}


