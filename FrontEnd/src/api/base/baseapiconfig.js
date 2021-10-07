import APIConfig from '@/api/config/apiconfig.js';
import axios from "axios";

var BaseApiConfig = axios.create({
    baseURL: APIConfig,
    headers: {'Content-Type': 'application/json'}
})

export default BaseApiConfig