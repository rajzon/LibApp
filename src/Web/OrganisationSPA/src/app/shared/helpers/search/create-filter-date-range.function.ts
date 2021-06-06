import {FilterDateModel} from "@core/models/filter-date-model";

export function CreateFilterDateRange(modificationFrom: string, modificationTo: string): FilterDateModel {
    const currentDate = Date.now();
    const minDefaultDate = Date.UTC(2015, 0, 1);

    if (modificationFrom && modificationTo)
        return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(modificationTo)])
    else if (modificationFrom)
        return new FilterDateModel('Modification Date', [new Date(modificationFrom), new Date(currentDate)])
    else if (modificationTo)
        return new FilterDateModel('Modification Date', [new Date(minDefaultDate), new Date(modificationTo)])
    else
        return new FilterDateModel('Modification Date', [new Date(minDefaultDate), new Date(currentDate)])
}
