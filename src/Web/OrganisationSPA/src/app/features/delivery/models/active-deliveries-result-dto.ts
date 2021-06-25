import {ActiveDelivery} from "./active-delivery-dto";

export class ActiveDeliveriesResultDto {
    currentPage: number
    pageSize: number
    total: number
    result: ActiveDelivery[]
}
