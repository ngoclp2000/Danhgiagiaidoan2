import BaseAPI from '@/api/base/baseapi.js'
import BaseAPIConfig from '@/api/base/baseapiconfig.js'
class EmployeeAPI extends BaseAPI {
    constructor(){
        super();
        this.BaseAPIConfig = BaseAPIConfig
        this.controller = 'api/v1/Employees/'
    }
    /**
     * Phương thức lấy mã nhân viên mới nhất
     */
    async getNewEmployeeCode() {
        return await this.BaseAPIConfig.get(`${this.controller}NewEmployeeCode`)
    }
    /**
     * Phương thức lọc dữ liệu nhân viên theo các tiêu chí
     * @param {Object} params object gửi lên request để phân trang 
     */
    async filterData(params) {
        let passParams = Object.assign({}, params)
        passParams['employeeFilter'] = passParams["contentFilter"]
        delete passParams.contentFilter
        return await this.BaseAPIConfig.get(`${this.controller}EmployeeFilter`,{ params :passParams})
    }
}

export default new EmployeeAPI();