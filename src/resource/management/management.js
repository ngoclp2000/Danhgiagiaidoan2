import {common} from './common'
import {employee} from './employee'
import {salary} from './salary'
export const management = {
    'vi': {
        common : common['vi'],
        entity: {
            Employee: employee['vi'],
            Salary: salary['vi']
        }
    },
    'en': {
        common : common['en'],
        entity: {
            Employee: employee['en'],
            Salary: salary['en']
        }
    }
}