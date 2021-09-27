class Validation {
    /**
     * Hàm kiểm tra dữ liệu số
     * Created By TBN
     * @param {String} str chuỗi số điện thoại
     * @returns 
     */
    static checkValidNumber(str) {
        return /^\d+$/.test(str.replaceAll('.', ''));
    }
    static checkTelephoneNumber(str) {
        return /\([0-9]{3}\) [0-9]{3}-[0-9]{4}$/.test(str);
    }
    /**
     * Hàm kiểm tra CMND/ Căn cước
     * Created By TBN
     * @param {String} idString số CMND/ Căn cước
     * @returns 
     */
    static checkIdNumber(idString) {
        if (idString && this.checkValidNumber(idString) && (idString.length == 9 || idString.length ==
                12)) {
            return true;
        }
        return false;
    }
    /**
     * Hàm kiểm tra email 
     * Created By TBN
     * @param {String} email 
     * @returns 
     */
    static validateEmail(email) {
        if (email) {
            const re =
                /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(String(email).toLowerCase());
        } else return false;
    }
    /**
     * Hàm kiểm tra mã số thuế
     * Created By TBN
     * @param {String} taxString chuỗi mã số thuế
     * @returns 
     */
    static checkTaxNumber(taxString) {
        if (taxString) {
            let toNumberTax = parseInt(taxString, 10);
            if (String(taxString).length != 10 || (!this.checkValidNumber(taxString)) ||
                (toNumberTax < 1 || toNumberTax > 9999999999 || toNumberTax == 9999999000))
                return false;
            else return true;
        } else return false;
    }
    /**
     * Kiểm tra ngày tháng hợp lệ
     * Created By TBN (25/7/2021)
     * @param {Date} date ngày tháng
     * @returns 
     */
    static checkValidDate(date) {
        
        // if(date.split('-').length != 0)
        //     date = date.split('-').join('/')
        let _date = new Date(date);
        // Kiểm tra đúng dd/mm/yyyy hoặc yyyy/mm/dd
        // if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(date) && !/^\d{4}\/\d{1,2}\/\d{1,2}$/.test(date))
        //     return false;
        // Kiểm tra từng phần
        var day = _date.getDate();
        var month = _date.getMonth() + 1;
        var year = _date.getFullYear();
        // Kiểm tra năm và tháng
        if (year < 1000 || year > 9999 || month <= 0 || month > 12)
            return false;

        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        // Kiểm tra tháng 2 cho năm nhuận
        if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            monthLength[1] = 29;

        // Kiểm tra ngày
        return day > 0 && day <= monthLength[month - 1];
    }
    
}

export default Validation;