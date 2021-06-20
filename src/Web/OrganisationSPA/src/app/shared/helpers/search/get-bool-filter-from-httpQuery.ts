//TODO make generic method
import {ActivatedRoute} from "@angular/router";

export function getBoolFilterFromHttpQuery(filterName: string, route: ActivatedRoute): boolean[] {
    const hasTrueValue = route.snapshot.queryParamMap.getAll(filterName).includes('true');
    const hasFalseValue =route.snapshot.queryParamMap.getAll(filterName).includes('false');

    if (hasTrueValue && !hasFalseValue)
        return [true]
    if (hasFalseValue && !hasTrueValue)
        return [false]
    else
        return null

    // return this.route.snapshot.queryParamMap.getAll('visibility').includes('true')
    // && !this.route.snapshot.queryParamMap.getAll('visibility').includes('false') ?
    //   [true] :
    //   this.route.snapshot.queryParamMap.getAll('visibility').find(v => v === 'false')
    //   && !this.route.snapshot.queryParamMap.getAll('visibility').find(v => v === 'true') ?
    //     [false]
    //     : null;
}
