import BaseAPI from '@/api/base/baseapi.js'
import BaseAPIConfig from '@/api/base/baseapiconfig.js'

class PeriodSalaryAPI extends BaseAPI{
    constructor(){
        super();
        this.BaseAPIConfig = BaseAPIConfig
        this.controller = '/api/v1/PeriodSalaries/'
    }
    /**
     * Phương thức lọc dữ liệu nhân viên theo các tiêu chí
     * @param {Object} params object gửi lên request để phân trang 
     */
     async filterData(params) {
        let passParams = Object.assign({}, params)
        passParams['periodSalaryFilter'] = passParams["contentFilter"]
        delete passParams.contentFilter
        return await this.BaseAPIConfig.get(`${this.controller}PeriodSalaryFilter`,{ params :passParams})
    }
}
export default new PeriodSalaryAPI();