export class ActiveDelivery {
    id: number
    name: string
    booksCount: number
    itemsCount: number
    isAllDeliveryItemsScanned: boolean
    isAnyDeliveryItemsScanned: boolean

    deliveryStatus: number
    modificationDate: Date | string
    creationDate: Date | string
}
