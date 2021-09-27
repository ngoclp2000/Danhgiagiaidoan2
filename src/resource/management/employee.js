export const employee = {
    'vi': {
        title: 'Nhân viên',
        formTitle: 'Thông tin nhân viên',
        popupContent: {
            delete: 'Bạn có thực sự muốn xóa Nhân viên <{0}> không?',
            deleteMultiple: 'Bạn có muốn xóa những nhân viên đã chọn không?',
            dataChange: 'Dữ liệu đã bị thay đổi. Bạn có muốn cất không?',
            errorGetNewCode: 'Lấy mã nhân viên thất bại',
            invalidCode: 'Mã không hợp lệ',
            deleteFail: 'Xóa nhân viên thất bại',
            deletedData : 'Dữ liệu đã bị xóa',
            errorSystem : 'Có lỗi xảy ra vui lòng liên hệ MISA'
        },
        toastMessage: {
            unReadyData: 'Dữ liệu chưa sẵn sàng',
            emptySelectData: 'Bạn chưa chọn nhân viên nào, vui lòng chọn và thử lại sau',
            errorFieldData: 'Sai trường dữ liệu khi gửi request',
            deleteFail: 'Xóa nhân viên thất bại',
            deleteSuccess: 'Xóa nhân viên thành công',
            unChangeData: "Dữ liệu của quý khách không thay đổi so với ban đầu",
            updateSuccess: "Cập nhật dữ liệu thành công",
            updateFail : "Cập nhật dữ liệu thất bại",
            addSuccess: "Thêm mới dữ liệu thành công",
            addFail : "Thêm mới dữ liệu thất bại",
        },
        placeHolder: {
            searchBar: "Tìm theo mã, tên nhân viên, số điện thoại",
            date: "DD/MM/YYYY"
        },
        formFieldTitle: {
            IsCustomer: 'Là khách hàng',
            IsSupplier: 'Là nhà cung cấp',
            EmployeeCode: 'Mã nhân viên',
            EmployeeName: 'Tên',
            DepartmentName: 'Đơn vị',
            EmployeePosition: 'Chức vụ',
            DateOfBirth: 'Ngày sinh',
            GenderName: 'Giới tính',
            IdentityNumber: "Số CMND",
            IdentityDate: "Ngày cấp",
            IdentityPlace: "Nơi cấp",
            Address: 'Địa chỉ',
            PhoneNumber: 'ĐT di động',
            TelephoneNumber: 'ĐT cố định',
            Email: 'Email',
            BankAccountNumber: 'Tài khoản ngân hàng',
            BankName: 'Tên ngân hàng',
            BankBranchName: 'Chi nhánh',
            GroupName: 'Nhóm khách hàng, nhà cung cấp',
            AccountDebtReceive: 'TK công nợ phải thu',
            AccountDebtPay: 'Tk công nợ phải trả'
        },
        dictionaryError: {
            EmployeeCode: {
                empty: 'Mã không được để trống',
                duplicate: 'Mã nhân viên <{0}> đã tồn tại trong hệ thống, vui lòng kiểm tra lại'
            },
            EmployeeName: {
                empty: 'Tên không được để trống',
            },
            DepartmentName: {
                empty: 'Đơn vị không được để trống',
                noOption: 'Dữ liệu <Đơn vị> không có trong danh mục'
            },
            DateOfBirth: {
                invalid: 'Ngày sinh không đúng định dạng'
            },
            IdentityDate: {
                invalid: 'Ngày cấp CMND không đúng định dạng'
            },
            IdentityNumber: {
                invalid: 'Số CMND không đúng định dạng'
            },
            Email:{
                invalid : 'Email không đúng định dạng'
            },
            PhoneNumber:{
                invalid : 'Số điện thoại không đúng định dạng'
            },
            TelephoneNumber:{
                invalid : 'Số điện thoại cố định không đúng định dạng'
            }
        },
        summarizeField:[
            
        ],
        tableFieldName: {
            main: [{
                    fieldName: "EmployeeCode",
                    value: "Mã nhân viên",
                    type: "number",
                    colWidth: "120px",
                }, {
                    fieldName: "EmployeeName",
                    value: "Tên nhân viên",
                    colWidth: "200px",
                }, {
                    fieldName: "GenderName",
                    value: "Giới tính",
                    colWidth: "100px",
                }, {
                    fieldName: "DateOfBirth",
                    value: "Ngày sinh",
                    textAlign: "center",
                    colWidth: "120px",
                }, {
                    fieldName: "PhoneNumber",
                    value: "Số ĐT di động",
                    hasToolTip: true,
                    tooltipContent: "Số điện thoại di động"
                }, {
                    fieldName: "IdentityNumber",
                    value: "Số CMND",
                    type: "number",
                    hasToolTip: true,
                    tooltipContent: "Số chứng minh nhân dân",
                    colWidth: "130px",
                },
                {
                    fieldName: "EmployeePosition",
                    value: "Chức vụ",
                    colWidth: "250px",
                }, {
                    fieldName: "DepartmentName",
                    value: "Tên đơn vị",
                    colWidth: "250px",
                }, {
                    fieldName: "BankAccountNumber",
                    value: "Số tài khoản",
                }, {
                    fieldName: "BankName",
                    value: "Tên ngân hàng",
                    colWidth: "250px",
                }, {
                    fieldName: "BankBranchName",
                    value: "Chi nhánh tài khoản ngân hàng",
                    colWidth: "250px",
                },
            ],
            'viewTable':[
                {
                    fieldName: "EmployeeCode",
                    value: "Mã nhân viên",
                    type: "number",
                    colWidth: "120px",
                }, {
                    fieldName: "EmployeeName",
                    value: "Tên nhân viên",
                    colWidth: "200px",
                }, {
                    fieldName: "GenderName",
                    value: "Giới tính",
                    colWidth: "100px",
                }, {
                    fieldName: "DateOfBirth",
                    value: "Ngày sinh",
                    textAlign: "center",
                    colWidth: "120px",
                }, {
                    fieldName: "PhoneNumber",
                    value: "Số ĐT di động",
                    hasToolTip: true,
                    tooltipContent: "Số điện thoại di động"
                }, {
                    fieldName: "IdentityNumber",
                    value: "Số CMND",
                    type: "number",
                    hasToolTip: true,
                    tooltipContent: "Số chứng minh nhân dân",
                    colWidth: "120px",
                },
                {
                    fieldName: "EmployeePosition",
                    value: "Chức vụ",
                    colWidth: "200px",
                }, {
                    fieldName: "DepartmentName",
                    value: "Tên đơn vị",
                    colWidth: "200px",
                }, 
            ]
        }
    },
    'en': {
        title: 'Employee',
        formTitle: 'Employee Information',
        popupContent: {
            delete: 'Do you really want to delete Employee <{0}>?',
            dataChange: 'Data has changed. Do you want to store information?',
            errorGetNewCode: 'Get new employee code fail',
            invalidCode: 'Invalid code',
            deleteFail: 'Delete employee fail',
            deletedData : 'The data has been deleted',
            errorSystem : 'There are any errors, please connect to MISA'
        },
        toastMessage: {
            unReadyData: 'Data is not ready',
            errorFieldData: 'Error field while sending request',
            deleteFail: 'Delete employee fail',
            deleteSuccess: 'Delete employee successfully',
            unChangeData: "Data is not changed",
            updateSuccess: "Update the data successfully",
            updateFail : "Update the data fail",
            addSuccess: "Adding new data successfully",
            addFail : "Adding new data fail",
        },
        tooltipContent: {
            searchBar: 'Enter the search information',
            refreshData: 'Reload the data',
            footerFormButton: {
                cancel: 'Cancel',
                save: 'Save (Ctrl + S)',
                saveAndMore: 'Save and More (Ctrl + Shift + S)'
            },
            closeButton: 'Close (ESC)',
            popup: {
                warning: {
                    primary: 'Yes',
                    secondary: 'No',
                },
                'warning-code': {
                    primary: 'Accept',
                },
                error: {
                    primary: 'Close',
                },
                confirmation: {
                    primary: 'Yes',
                    subPrimary: 'No',
                    secondary: 'Cancel'
                }
            }
        },
        placeHolder: {
            searchBar: "Find by the code,name and phonenumber of the employee",
            date: "DD/MM/YYYY"
        },
        buttons: {
            content: {
                commonButton: {
                    addNewEmployee: 'Add Employee',
                },
                footerFormButton: {
                    cancel: 'Cancel',
                    save: 'Save',
                    saveAndMore: 'Save and More'
                },
                footerPopUpButton: {
                    warning: ["No", "", "Yes"],
                    confirmation: ["Cancel", "No", "Yes"],
                    error: ["", "", "Close"],
                    'warning-code': ["", "", "Accept"]
                },
                pagingBarButton: {
                    previous: 'Previous',
                    next: 'Next'
                }
            }
        },
        formFieldTitle: {
            IsCustomer: 'Is Customer',
            IsSupplier: 'Is Supplier',
            EmployeeCode: 'Employee code',
            EmployeeName: 'Employee name',
            DepartmentName: 'Department name',
            EmployeePosition: 'Employee position',
            DateOfBirth: 'Date of birth',
            GenderName: 'Gender',
            IdentityNumber: "Identity number",
            IdentityDate: "Identity date",
            IdentityPlace: "Identity place",
            Address: 'Address',
            PhoneNumber: 'Phone number',
            TelephoneNumber: 'Telephone number',
            Email: 'Email',
            BankAccountNumber: 'Bank account number',
            BankName: 'Bank name',
            BankBranchName: 'Bank branch name',
            GroupName: 'Group name',
            AccountDebtReceive: 'Account debt receive',
            AccountDebtPay: 'Account debt pay'
        },

        dictionaryError: {
            EmployeeCode: {
                empty: 'Employee code can not be empty',
                duplicate: 'Employee Code <{0}> existed in the system, please try again'
            },
            EmployeeName: {
                empty: 'Name can not be empty',
            },
            DepartmentName: {
                empty: 'Department name can not be empty',
                noOption: 'Data <Department name> is not available in the list'
            },
            DateOfBirth: {
                invalid: 'Date of birth is invalid',
            },
            IdentityDate: {
                invalid: 'Identity Date is invalid'
            },
            IdentityNumber: {
                invalid: 'Identity Number is invalid'
            },
            Email:{
                invalid : 'Email không đúng định dạng'
            },
            PhoneNumber:{
                invalid : 'Số điện thoại không đúng định dạng'
            },
            TelephoneNumber:{
                invalid : 'Số điện thoại cố định không đúng định dạng'
            }
        },
        tableFieldName: {
            main: [{
                    fieldName: "EmployeeCode",
                    value: "EMPLOYEE CODE",
                    type: "number",
                }, {
                    fieldName: "EmployeeName",
                    value: "EMPLOYEE NAME",
                    colWidth: "250px",
                }, {
                    fieldName: "GenderName",
                    value: "GENDER NAME",
                }, {
                    fieldName: "DateOfBirth",
                    value: "DATE OF BIRTH",
                    textAlign: "center",
                }, {
                    fieldName: "PhoneNumber",
                    value: "PHONE NUMBER",
                    textAlign: "center",
                }, {
                    fieldName: "IdentityNumber",
                    value: "IDENTITY NUMBER",
                    type: "number",
                },
                {
                    fieldName: "EmployeePosition",
                    value: "EMPLOYEE POSITION",
                    colWidth: "250px",
                }, {
                    fieldName: "DepartmentName",
                    value: "DEPARTMENT NAME",
                    colWidth: "250px",
                }, {
                    fieldName: "BankAccountNumber",
                    value: "BANK ACCOUNT NAME",
                }, {
                    fieldName: "BankName",
                    value: "BANK NAME",
                    colWidth: "250px",
                }, {
                    fieldName: "BankBranchName",
                    value: "BANK BRANCH NAME",
                    colWidth: "250px",
                },
            ],
            sub: {
                fieldName: "Function",
                value: "FUNCTION",
                colWidth: "120px",
                functionList: [{
                        key: 'duplicate',
                        value: 'Duplicate'
                    },
                    {
                        key: 'delete',
                        value: 'Delete'
                    }
                ]
            }
        }
    }
}