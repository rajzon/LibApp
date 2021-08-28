import {Category} from "./category";
import {Publisher} from "./publisher";
import {Image} from "./image";
import {Author} from "./author";

export class ActiveDeliveryItemDesc {
    title: string
    ean13: string
    isbn10: string
    isbn13: string
    pageCount: number
    publishedDate: Date
    categories: Category[]
    authors: Author[]
    publisher: Publisher
    image: Image;
}
