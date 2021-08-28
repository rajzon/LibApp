import {ActiveDeliveryItemDesc} from "./active-delivery-item-desc";
import {ActiveDeliveryScanInfoItem} from "./active-delivery-scan-info-item";

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

  constructor(dto: ActiveDeliveryScanInfoItem, desc: ActiveDeliveryItemDesc) {
      this.id = dto.id;
      this.bookId = dto.bookId;
      this.bookEan = dto.bookEan;
      this.itemsCount = dto.itemsCount;
      this.scannedCount = dto.scannedCount;
      this.isScanned = dto.isScanned;
      this.isAllScanned = dto.isAllScanned;
      this.modificationDate = dto.modificationDate;
      this.creationDate = dto.creationDate;
      this.isAllScanned = dto.isAllScanned;
      this.itemDescription = desc;
  }
}

