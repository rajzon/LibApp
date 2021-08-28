import {ActiveDelivery} from "./active-delivery-dto";
import {ActiveDeliveryScanInfoItem} from "./active-delivery-scan-info-item";

export class ActiveDeliveryScanResultDto {
    activeDeliveryInfo: ActiveDelivery
    items: ActiveDeliveryScanInfoItem[]
}
