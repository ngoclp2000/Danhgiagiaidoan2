class Format {
    /**
     * Hàm định dạng tên
     * @param {String}} name 
     */
    static formatName(name) {
        // Tách tên bởi dấu cách
        let arrSplitWords = name.split(' ');
        for (let i = 0; i < arrSplitWords.length; i++) {
            if (arrSplitWords[i]) {
                let string = arrSplitWords[i];
                // in hoa chữ cái đầu và chuyển các từ phía sau về dạng thường
                arrSplitWords[i] = string[0].toUpperCase() + string.slice(1).toLowerCase();
            }
        }
        return arrSplitWords.join(' ');
    }
    /**
     * Hàm định dạng tiền tệ
     * Created By TBN
     * @param {Int} money tiền
     * @returns 
     */
    static formatMoney(money) {
        if (money) {
            return money.toString().replaceAll('.', '').replace(/\B(?=(\d{3})+(?!\d))/g, '.');
        } else return "0";
    }
    /**
     * Hàm định dạng khi nhập ngày tháng năm
     * @param {Date} date 
     * @returns 
     */
    static formatDateInput(date) {
        if (date) {
            let initialDate = date;
            date = date.replace(/\D/g, '');
            let formatDate = "";
            if (date.length < 3) {
                return (date.length == 2 && initialDate != date) ? date + "/" : date;
            } else if (date.length <= 4) {
                formatDate += date.substring(0, 2) + "/";
                formatDate += date.substr(2);
                return (date.length == 4 && initialDate.split('/').length != 2) ? formatDate + "/" : formatDate;
            } else {
                formatDate += date.substring(0, 2) + "/" + date.substring(2, 4) + "/" + date.substr(4);
                return formatDate;
            }
        }
        return "";
    }
    /**
     * Hàm tạo format số điện thoại kiểu Mỹ
     * Created By TBN (19/8/2021)
     * @param {String} telephoneNumber số điện thoại
     * @returns 
     */
    static formatTelephoneNumber(telephoneNumber) {
        if (telephoneNumber) {
            // Kiểm tra xem có phải xóa cum ") " hay không
            if (telephoneNumber.indexOf(")") == telephoneNumber.length - 1) {
                return telephoneNumber.substring(0, 3);
            }
            // Xóa bỏ cái dấu "()- "
            telephoneNumber = telephoneNumber.replace(/[() -]/g, '');
            // Khởi tạo số điện thoại sau khi format
            let formatTelephoneNumber = "";
            // Nếu xóa cuối cùng thì không thêm gì
            if (telephoneNumber != "")
                formatTelephoneNumber = "(";
            // Kiểm tra xem có phải đang nhập 3 số đầu
            if (telephoneNumber.length < 3) {
                formatTelephoneNumber += telephoneNumber;
            } else if (telephoneNumber.length <= 6) {
                // Đang nhập 3 số giữa
                formatTelephoneNumber = "(" + telephoneNumber.substring(0, 3) + ') ';
                formatTelephoneNumber += telephoneNumber.substring(3, 6);
            } else {
                // Đang nhập 4 số cuối
                formatTelephoneNumber = "(" + telephoneNumber.substring(0, 3) + ') ' +
                    telephoneNumber.substring(3, 6) + '-';
                formatTelephoneNumber += telephoneNumber.substring(6, 10);
            }
            return formatTelephoneNumber;
        } else {
            return "";
        }
    }
    /**
     * Hàm định dạng ngày tháng => day/month/year
     * Created By TBN
     * @param {Date} date ngày tháng
     * @returns 
     */
    static formatDate(date) {
        if (date) {
            let _date = new Date(date);
            let day = _date.getDate(),
                month = _date.getMonth() + 1,
                year = _date.getFullYear();
            day = day < 10 ? '0' + day : day;
            month = month < 10 ? '0' + month : month;
            return day + '/' + month + '/' + year;
        } else return "";
    }
    /**
     * Hàm chuyển về số (Dùng cho trường tiền tệ)
     * Created By TBN
     * @param {Int} number số
     * @returns 
     */
    static changeToNumber(number) {
        if (number) {
            return number.toString().replaceAll('.', '');
        } else return "0"
    }
    /**
     * Hàm chuyển có dấu thành không dấu
     * @param {String} str 
     * @returns chuỗi không dấu
     * Created By TBN(25/9/2021)
     */
    static changeVietNameseWordToNormal(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a").replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e")
        .replace(/ì|í|ị|ỉ|ĩ/g, "i")
        .replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o")
        .replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u")
        .replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y")
        .replace(/đ/g, "d")
        // Some system encode vietnamese combining accent as individual utf-8 characters
        .replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "") // Huyền sắc hỏi ngã nặng 
        .replace(/\u02C6|\u0306|\u031B/g, ""); // Â, Ê, Ă, Ơ, Ư
        return str;
    }
    
}

export default Format