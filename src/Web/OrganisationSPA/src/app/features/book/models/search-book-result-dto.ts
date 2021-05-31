import {SearchResultDto} from "@core/models/search-result-dto";
import {Book} from "./book";
import {Aggregation} from "@core/models/aggregation";

export class SearchBookResultDto implements SearchResultDto<Book> {
    total: number;
    results: Book[];
    aggregations: Aggregation[];

}
