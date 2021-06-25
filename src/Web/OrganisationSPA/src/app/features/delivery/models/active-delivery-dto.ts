export class ActiveDelivery {
    id: number
    name: string
    booksCount: number
    itemsCount: number
    IsAllDeliveryItemsScanned: boolean
    IsAnyDeliveryItemsScanned: boolean

    deliveryStatus: number
    modificationDate: Date | string
    creationDate: Date | string
}
