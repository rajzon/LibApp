export class ReloadActiveDeliveriesQueryDto {
    currentPage: number
    pageSize: number

    constructor(currentPage: number, pageSize: number) {
        this.currentPage = currentPage
        this.pageSize = pageSize
    }
}
