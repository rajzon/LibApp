import {FilterDateModel} from "@core/models/filter-date-model";

export function CreateFilterDateRange(modificationFrom: string, modificationTo: string): FilterDateModel {
    const maxDateAsNumber = 8640000000000000;

    if (modificationFrom && modificationTo)
        return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(modificationTo)])
    else if (modificationFrom)
        return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(maxDateAsNumber)])
    else if (modificationTo)
        return new FilterDateModel('Modification Date', [new Date(null), new Date(modificationTo)])
    else
        return new FilterDateModel('Modification Date', [new Date(null), new Date(maxDateAsNumber)])
}
