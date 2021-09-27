export const common = {
    'vi': {
        gender: [{
            content: "Nam",
            value: 0
        }, {
            content: "Nữ",
            value: 1
        }, {
            content: "Khác",
            value: 2
        }],
        text: {
            noContent: 'Không có dữ liệu',
            total: 'Tổng số',
            record: 'bản ghi',
            edit: 'Sửa',
            function: 'Chức năng',
            month: "Tháng",
            year: "Năm"
        },
        functionTableList: [{
                key: 'edit',
                value: 'Sửa'
            },
            {
                key: 'delete',
                value: 'Xóa'
            }, {
                key: 'duplicate',
                value: 'Nhân bản'
            }
        ],
        tooltipContent: {
            searchBar: 'Nhập thông tin tìm kiếm',
            refreshData: 'Cập nhật dữ liệu',
            editData: 'Sửa dữ liệu',
            duplicateData: 'Nhân bản dữ liệu',
            deleteData: 'Xóa dữ liệu',
            addData: 'Thêm mới dữ liệu',
            zoom: 'Thu/Phóng',
            
            footerFormButton: {
                cancel: 'Hủy',
                save: 'Cất (Ctrl + S)',
                saveAndMore: 'Cất và Thêm (Ctrl + Shift + S)'
            },
            closeButton: 'Đóng (ESC)',
            popup: {
                warning: {
                    primary: 'Có',
                    secondary: 'Không',
                },
                'warning-code': {
                    primary: 'Đồng ý',
                },
                error: {
                    primary: 'Đóng',
                },
                confirmation: {
                    primary: 'Có',
                    subPrimary: 'Không',
                    secondary: 'Hủy'
                }
            }
        },
        dictionaryError: {
            MaxLength: 'Độ dài không được vượt quá {0}',
            PeriodName: {
                empty: 'Tên kỳ lương không được để trống',
            },
            CoefficientsSalary:{
                empty: "Chỉ số lương không được để trống"
            },
            Subsidy:{
                empty: "Trợ cấp không được để trống"
            },
            SocialInsuranceCost:{
                empty: "Tiền bảo hiểm không được để trống"
            },
            Month: {
                empty: 'Tháng không được để trống',
                noOption: "Dữ liệu [Tháng] không có trong danh mục"
            },
            Year: {
                empty: 'Năm không được để trống',
                noOption: "Dữ liệu [Năm] không có trong danh mục"
            },
            EmployeeCode: {
                empty: 'Mã không được để trống',
                duplicate: 'Mã nhân viên <{0}> đã tồn tại trong hệ thống, vui lòng kiểm tra lại'
            },
            EmployeeName: {
                empty: 'Tên không được để trống',
            },
            DepartmentName: {
                empty: 'Đơn vị không được để trống',
                noOption: 'Dữ liệu [Đơn vị] không có trong danh mục'
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
            Email: {
                invalid: 'Email không đúng định dạng'
            },
            PhoneNumber: {
                invalid: 'Số điện thoại không đúng định dạng'
            },
            TelephoneNumber: {
                invalid: 'Số điện thoại cố định không đúng định dạng'
            }
        },
        navbarItems: [{
                content: 'Tổng quan',
                classItem: 'navbar-item-icon-dashboard',
                link: '/dashboard'
            },
            {
                content: 'Lương',
                classItem: 'navbar-item-icon-cash',
                link: '/CAProcess'
            },
            {
                content: 'Nhân viên',
                classItem: 'navbar-item-icon-employee',
                link: '/BAProcess',

            },
            {
                content: 'Mua hàng',
                classItem: 'navbar-item-icon-buy',
                link: '/PUProcess',

            },
            {
                content: 'Bán hàng',
                classItem: 'navbar-item-icon-sell',
                link: '/SAProcess',

            }, {
                content: 'Quản lý hóa đơn',
                classItem: 'navbar-item-icon-receipt',
                link: '/IPProcess',

            }, {
                content: 'Kho',
                classItem: 'navbar-item-icon-warehouse',
                link: '/INProcess',

            }, {
                content: 'Công cụ dụng cụ',
                classItem: 'navbar-item-icon-tool',
                link: '/SUProcess',

            }, {
                content: 'Tài sản cố định',
                classItem: 'navbar-item-icon-asset',
                link: '/FAProcess',

            }, {
                content: 'Thuế',
                classItem: 'navbar-item-icon-tax',
                link: '/TAList',

            }, {
                content: 'Giá thành',
                classItem: 'navbar-item-icon-price',
                link: '/JCProcessSimple',

            }, {
                content: 'Tổng hợp',
                classItem: 'navbar-item-icon-synthesis',
                link: '/GLProcess',
            }, {
                content: 'Ngân sách',
                classItem: 'navbar-item-icon-budget',
                link: '/BUDDashboard',

            }, {
                content: 'Báo cáo',
                classItem: 'navbar-item-icon-report',
                link: '/ReportList',

            }, {
                content: 'Phân tích tài chính',
                classItem: 'navbar-item-icon-analysis',
                link: '/ReportAnalysis',
            }
        ],
    },
    'en': {
        gender: [{
            content: "Male",
            value: 0
        }, {
            content: "Female",
            value: 1
        }, {
            content: "Other",
            value: 2
        }],
        text: {
            noContent: 'No content',
            total: 'Total',
            record: 'records',
            edit: 'Edit'
        },
        navbarItems: [{
                content: 'Dashboard',
                classItem: 'navbar-item-icon-dashboard',
                link: '/dashboard'
            },
            {
                content: 'Cash',
                classItem: 'navbar-item-icon-cash',

                link: '/CAProcess'
            },
            {
                content: 'Employee',
                classItem: 'navbar-item-icon-employee',
                link: '/BAProcess',

            },
            {
                content: 'Purchase',
                classItem: 'navbar-item-icon-buy',
                link: '/PUProcess',

            },
            {
                content: 'Sell',
                classItem: 'navbar-item-icon-sell',
                link: '/SAProcess',

            }, {
                content: 'Order',
                classItem: 'navbar-item-icon-receipt',
                link: '/IPProcess',

            }, {
                content: 'Warehouse',
                classItem: 'navbar-item-icon-warehouse',
                link: '/INProcess',

            }, {
                content: 'Tool',
                classItem: 'navbar-item-icon-tool',
                link: '/SUProcess',

            }, {
                content: 'Assets',
                classItem: 'navbar-item-icon-asset',
                link: '/FAProcess',

            }, {
                content: 'Tax',
                classItem: 'navbar-item-icon-tax',
                link: '/TAList',

            }, {
                content: 'Price',
                classItem: 'navbar-item-icon-price',
                link: '/JCProcessSimple',

            }, {
                content: 'Synthesis',
                classItem: 'navbar-item-icon-synthesis',
                link: '/GLProcess',
            }, {
                content: 'Budget',
                classItem: 'navbar-item-icon-budget',
                link: '/BUDDashboard',

            }, {
                content: 'Report',
                classItem: 'navbar-item-icon-report',
                link: '/ReportList',

            }, {
                content: 'Analysis',
                classItem: 'navbar-item-icon-analysis',
                link: '/ReportAnalysis',
            }
        ],
    }
}