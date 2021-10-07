import Format from "./format";
class Common {
    /**
     * Chuyển locale của browser về chung một dạng
     * Created By TBN (22/8/2021)
     * @param {} string 
     * @returns 
     */
    static convertBrowserLocale(string) {
        if (Array.isArray(string)) {
            string = string[0].toLowerCase();
        } else
            string = string.toLowerCase();
        switch (string) {
            case 'vi-vn':
            case 'vi':
                return 'vi';
            case 'en':
            case 'en-us':
            case 'en-in':
            case 'en-gb':
                return 'en';
        }
    }
    /**
     * Sắp xếp theo trường dữ liệu phụ thuộc vào kiểu dữ liệu và chiều sắp xếp
     * Created By TBN (22/8/2021)
     * @param {Array} array mảng chứa đối tượng
     * @param {String} fieldName tên của trường
     * @param {Boolean} direction chiều sắp xếp false = desc, true = desc
     * @param {String} type kiểu dữ liệu
     * @returns 
     */
    static sortByFieldName(array, fieldName, direction, type) {
        return array.sort((a, b) => {
            if (a[fieldName] == null && b[fieldName] == null) {
                return 0;
            } else {
                // Kiểm tra chiều sắp xếp là tăng dần hay giảm dần
                if (a[fieldName] == null) {
                    return (!direction ? -1 : 1);
                } else if (b[fieldName] == null) {
                    return (!direction ? 1 : -1);
                } else {
                    let convertValueA = this.convertData(fieldName, a[fieldName]);
                    let convertValueB = this.convertData(fieldName, b[fieldName]);
                    if (type == 'number') {
                        // Lấy số ra khỏi chuỗi
                        convertValueA = parseInt(convertValueA.toString().replaceAll(/^\D+/g, ''), 10) || 0;
                        convertValueB = parseInt(convertValueB.toString().replaceAll(/^\D+/g, ''), 10) || 0;

                        if (convertValueA > convertValueB) {
                            return (!direction ? 1 : -1);
                        } else if (convertValueA < convertValueB) {
                            return (!direction ? -1 : 1);
                        } else
                            return 0;
                    } else {
                        return (!direction ? convertValueA.localeCompare(convertValueB) : convertValueB.localeCompare(convertValueA));
                    }
                }
            }
        });
        // }
    }
    /**
     * Format dữ liệu nếu cần thiết
     * @param {String} fieldName tên trường
     * @param {Object} data dữ liệu
     * @returns 
     */
    static convertData(fieldName, data) {
        return data;
    }
    /**
     * Lấy danh sách năm cho combobox
     * @param {Number} currentYear 
     * @returns Danh sách object chứa danh sách năm
     */
    static getRangeYear(currentYear) {
        let yearArray = [];
        for (let i = 1901; i <= currentYear; i++) {
            yearArray.push({
                content: i
            });
        }
        return yearArray;
    }
    /**
     * Lấy danh sách tháng cho combobox
     * @returns 
     */
    static getRangeMonth(monthWord){
        let monthArray = [];
        for(let i = 1 ; i <= 12 ; i++) {
            monthArray.push({
                content: monthWord + " " + i
            })
        }
        return monthArray;
    }
    /**
     * /**
     * Kiểm tra xem chuỗi có chứa chữ có dấu không
     * Created By TBN (25/9/2021)
     * @param {String} str 
     * @returns Trả về true nếu có, nếu không thì trả về false
     */
    static checkStringHasVietNameseCharacters(str){
        if(Format.changeVietNameseWordToNormal(str) != str) return true;
        else return false;
    }
    static generateUUID(){
        let
          d = new Date().getTime(),
          d2 = (performance && performance.now && (performance.now() * 1000)) || 0;
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
          let r = Math.random() * 16;
          if (d > 0) {
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
          } else {
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
          }
          return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
        });
    }
}

export default Common;