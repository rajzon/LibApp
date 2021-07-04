import {Image} from "./image";
import {Category} from "./category";
import {Author} from "./author";
import {Publisher} from "./publisher";

export class ActiveDeliveryResultFromSearch {
    id: number
    title: string
    ean13: string
    isbn10: string
    isbn13: string
    image: Image
    categories: Category[]
    authors: Author[]
    publisher: Publisher
    publishedDate: Date

    itemsCount: number = 0
}
