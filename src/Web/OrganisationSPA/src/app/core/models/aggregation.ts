export class Aggregation {
  name: string
  type: AggregationType
  buckets: Bucket[]
}

export class Bucket {
  key: string
  count: number

  isKeySelected: boolean
}

export enum AggregationType {
  Boolean = 'System.Boolean',
  String = 'System.String'
}


