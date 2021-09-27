import EmployeeList from '../views/dictionary/employee/EmployeeList.vue'
import SalaryEmployee from '../views/dictionary/salary/SalaryEmployee.vue'
export const routes = [
    {path: '/BAProcess', component: EmployeeList,meta: { title: 'Quản lý nhân viên' }},
    {path: '/CAProcess', component: SalaryEmployee,meta: { title: 'Quản lý lương nhân viên' }}
]