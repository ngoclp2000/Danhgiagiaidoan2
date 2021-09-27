import BaseAPI from '@/api/base/baseapi.js'

class PositionAPI extends BaseAPI {
    constructor(){
        super();
        this.controller = '/api/v1/Departments/'
    }
}

export default new PositionAPI();