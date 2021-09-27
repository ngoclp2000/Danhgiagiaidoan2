import BaseAPI from '@/api/base/baseapi.js'
import BaseAPIConfig from '@/api/base/baseapiconfig.js'

class PeriodSalaryEmployeeAPI extends BaseAPI{
    constructor(){
        super();
        this.BaseAPIConfig = BaseAPIConfig
        this.controller = '/api/v1/PeriodSalaryEmployees/'
    }
    /**
     * Phương thức lọc dữ liệu nhân viên theo các tiêu chí
     * @param {Object} params object gửi lên request để phân trang 
     */
     async getPeriodSalaryEmployeeListByPeriodSalaryId(params) {
        let passParams = Object.assign({}, params)
        return await this.BaseAPIConfig.get(`${this.controller}PeriodSalaryId`,{ params :passParams})
    }
}
export default new PeriodSalaryEmployeeAPI();