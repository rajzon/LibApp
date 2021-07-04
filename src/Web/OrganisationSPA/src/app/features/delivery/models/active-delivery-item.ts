import {ActiveDeliveryItemDesc} from "./active-delivery-item-desc";

export class ActiveDeliveryItem {
    id: number
    bookId: number
    bookEan: string
    itemsCount: number
    scannedCount: number
    isScanned: boolean
    isAllScanned: boolean
    modificationDate: Date
    creationDate: Date
    itemDescription: ActiveDeliveryItemDesc
}
