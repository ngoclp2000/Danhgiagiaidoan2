var APIConfig = {
    development: 'https://localhost:44318/',
    production: 'local:8080'
}

export default APIConfig[process.env.NODE_ENV]