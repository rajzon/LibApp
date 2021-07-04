import {ActiveDelivery} from "./active-delivery-dto";
import {ActiveDeliveryItem} from "./active-delivery-item";

export class ActiveDeliveryResultDto {
    activeDeliveryInfo: ActiveDelivery
    items: ActiveDeliveryItem[]
}
