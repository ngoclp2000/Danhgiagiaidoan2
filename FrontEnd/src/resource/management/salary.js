export const salary = {
    'vi': {
        title: 'Lương',
        titleSubTable: 'Danh sách nhân viên',
        formTitle: 'Thông tin kỳ lương',
        placeHolder: {
            searchBarPeriod: "Tìm theo tên kỳ",
            searchBarPeriodEmployee: "Tìm kiếm nhân viên...",
            searchBarEmployeeView: "Tìm kiểm nhân viên",
            date: "DD/MM/YYYY"
        },
        buttons: {
            content: {
                footerPopUpButton: {
                    warning: ["Không", "", "Có"],
                    confirmation: ["Hủy", "Không", "Có"],
                    error: ["", "", "Đóng"],
                    'warning-code': ["", "", "Đồng ý"]
                }
            }
        },
        toastMessage: {
            unReadyData: 'Dữ liệu chưa sẵn sàng',
            emptySelectData: 'Bạn chưa chọn kỳ lương nào, vui lòng chọn và thử lại sau',
            errorFieldData: 'Sai trường dữ liệu khi gửi request',
            deleteFail: 'Xóa kỳ lương thất bại',
            deleteSuccess: 'Xóa kỳ lương thành công',
            unChangeData: "Dữ liệu của quý khách không thay đổi so với ban đầu",
            updateSuccess: "Cập nhật dữ liệu thành công",
            updateFail: "Cập nhật dữ liệu thất bại",
            addSuccess: "Thêm mới dữ liệu thành công",
            addFail: "Thêm mới dữ liệu thất bại",
        },
        popupContent: {
            delete: 'Bạn có thực sự muốn xóa kỳ lương <b>{0}</b> không?',
            deleteMultiple: 'Bạn có muốn xóa những kỳ lương đã chọn không?',
            dataChange: 'Dữ liệu đã bị thay đổi. Bạn có muốn cất không?',
            invalidCode: 'Mã không hợp lệ',
            deleteFail: 'Xóa kỳ lương thất bại',
            deletedData: 'Dữ liệu đã bị xóa',
            errorSystem: 'Có lỗi xảy ra vui lòng liên hệ MISA',
            emptyDataTable: 'Vui lòng chọn ít nhất một nhân viên trong bảng',
            emptyDeleteDataTable: 'Vui lòng chọn ít nhất một kỳ lương để xóa'
        },
        formFieldTitle: {
            PeriodName: "Tên kỳ",
            Month: "Tháng",
            Year: "Năm",
            Note: "Ghi chú"
        },
        summarizeField: [
            "TotalSalary",
            "NumberOfEmployee",
            "Salary"
        ],
        tableFieldName: {
            main: [{
                fieldName: "PeriodName",
                value: "Tên kỳ",
                colWidth: "300px",
            }, {
                fieldName: "Month",
                value: "Tháng",
                textAlign: "left",
                colWidth: "80px",
            }, {
                fieldName: "Year",
                value: "Năm",
                textAlign: "right",
                colWidth: "60px",
            }, {
                fieldName: "NumberOfEmployee",
                value: "Số lượng nhân viên",
                type: "number",
                textAlign: "right",
                colWidth: "130px",
            }, {
                fieldName: "TotalSalary",
                type: "number",
                value: "Tổng tiền lương",
                textAlign: "right",
                colWidth: "150px"
            }, {
                fieldName: "Note",
                value: "Ghi chú",
                colWidth: "742px",
            }],
            sub: [{
                fieldName: "EmployeeCode",
                value: "Mã nhân viên",
                colWidth: "100px",
            }, {
                fieldName: "EmployeeName",
                value: "Tên nhân viên",
                colWidth: "300px",
            }, {
                fieldName: "CoefficientsSalary",
                value: "Hệ số lương",
                type: "number",
                textAlign: "right",
                colWidth: "100px",
            }, {
                fieldName: "Subsidy",
                value: "Trợ cấp",
                type: "number",
                textAlign: "right",
                colWidth: "120px",
            }, {
                fieldName: "SocialInsuranceCost",
                type: "number",
                value: "Tiền bảo hiểm",
                textAlign: "right",
            }, {
                fieldName: "Salary",
                value: "Tiền lương",
                type: "number",
                colWidth: "130px",
                textAlign: "right",
            }],
            form: [{
                fieldName: "EmployeeCode",
                value: "Mã nhân viên",
                colWidth: "100px",
            }, {
                fieldName: "EmployeeName",
                value: "Tên nhân viên",
                colWidth: "140px",
            }, {
                fieldName: "CoefficientsSalary",
                value: "Hệ số lương",
                type: "float",
                textAlign: "right",
                colWidth: "100px",
                validType:"required float",
                isInput: true,
            }, {
                fieldName: "Subsidy",
                value: "Trợ cấp",
                type: "money",
                textAlign: "right",
                colWidth: "100px",
                validType:"number",
                isInput: true,
            }, {
                fieldName: "SocialInsuranceCost",
                type: "money",
                value: "Tiền bảo hiểm",
                textAlign: "right",
                colWidth: "110px",
                validType:"number",
                isInput: true,
            }, {
                fieldName: "Salary",
                value: "Tiền lương",
                type: "money",
                dependent: true,
                colWidth: "100px",
                textAlign: "right",
            }]
        }
    }
}