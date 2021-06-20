import {Aggregation} from "@core/models/aggregation";

export interface SearchResultDto<T> {
  total: number
  results: T[]
  aggregations: Aggregation[]
}
