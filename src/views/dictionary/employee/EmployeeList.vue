<template>
    <div class="content-container employee-content">
        <div :class="{'toggle-navbar' : toggle.isToggleNavbar}" class="content">
            <div class="content-header">
                <div class="title">{{title}}</div>
                <BaseButton @click="displayAddForm" classList="m-btn-default m-primary-btn" :btnText="btnAddText" />
            </div>
            <div class="main-content">
                <div class="toolbar-content">
                    <div class="filter-bar">
                        <div class="filter-left">
                            <BaseInput :contentPlHolder="placeHolder['searchBar']"
                                classList="icon-search input-search user-input" type="text" errorContent="" refType=""
                                validType="search" ref="searchchingInput" @enterPress="searchData"
                                @refreshData="searchData" :tooltipContent="tooltipContent['searchBar']"
                                noneCheck="true" />
                        </div>
                        <div class="filter-right">
                            <div class="delete-icon" v-tooltip="tooltipContent['deleteData']"
                                @click="deleteMultipleData">
                            </div>
                            <div class="refresh-icon" @click="refreshDataButtonClick"
                                v-tooltip="tooltipContent['refreshData']">
                            </div>
                        </div>
                    </div>
                </div>
                <!-- <div class="grid"> -->
                    <BaseTable @hideSpinner="hideSpinner" 
                        :classListGridContainer="'grid-employee'"
                        @updateTotalRecord="updateTotalRecord" @updatePageSize="updatePageSize"
                        @updatePagination="updatePagination"
                        @displayFormEdit="displayFormEdit" @displayConfirmationPopup="displayConfirmationPopup"
                        @displayFormDuplicate="displayFormDuplicate" ref="table" :type="type" :ths="ths" />
                    <!-- <div class="no-content" v-if="totalRecord == 0">
                        <div class="no-content-img">

                        </div>
                        <div class="no-content-text">
                            {{textResource['noContent']}}
                        </div>
                        <LoadingSpinner v-if="isShowSpinner" />
                    </div> -->
                </div>
                <LoadingSpinner v-if="isShowSpinner && totalRecord != 0" />
                <!-- <BasePagingBar v-if="totalRecord != 0" type="nhân viên" @updatePageSize="updatePageSize"
                    :pageSize="$store.state.data.payload.pageSize" ref="pagingBar" @updatePagination="updatePagination"
                    :totalRecord="totalRecord" :maxPageNumber="maxPageNumber" :isReady="isReady" /> -->
            <!-- </div> -->
        </div>
        <EmployeeDetail @hideForm="hideForm" v-if="isShowForm" @refreshData="refreshDataButtonClick" ref="employee" />
        <BasePopup @hideLayout="isShowPopUp=false" @refreshData="refreshDataButtonClick" @deleteData="deleteData"
            ref="confirmation" v-if="isShowPopUp" />
    </div>
</template>

<script>
    import BaseTable from '../../../components/BaseTable.vue'
    import EmployeeDetail from './EmployeeDetail.vue'
    import BaseButton from '../../../components/BaseButton.vue'
    import BasePopup from '../../../components/BasePopup.vue'
    import BaseInput from '../../../components/BaseInput.vue'
    // import BasePagingBar from '../../../components/BasePagingBar.vue'
    import LoadingSpinner from '../../../components/LoadingSpinner.vue'
    import EmployeeAPI from '../../../api/components/employeeapi'

    import {
        MISACODE,
        STATUSMODE
    } from '../../../enum/index'
    import {
        mapActions,
        mapMutations
    } from 'vuex'

    export default {
        created() {
            // Trường dữ liệu cho Employe
            this.ths = this.resource[this.$browserLocale]['management']['entity']['Employee']['tableFieldName']['main'];
            this.title = this.resource[this.$browserLocale]['management']['entity']['Employee']['title'];
            this.btnAddText = this.buttonContentResource['commonButton']['addNew']
        },
        async mounted() {
            let payload = {
                contentFilter: "",
                pageSize: 10,
                pageIndex: 0
            }
            this.$store.commit('CHANGE_PAYLOAD', payload);
            await this.refreshData();
            this.checkOverFlowTable();
        },
        data() {
            return {
                isShowSpinner: true,
                isReady: true,
                selectedData: [],
                isShowForm: false,
                isShowPopUp: false,
                maxPageNumber: 0,
                totalRecord: 0,
                currentPage: 1,
                isDeleteMultiple: false,
                dataDepartments: [],
                dataPositions: [],
                btnAddText: "",
                title: "",
                type: "employee-table",
                ths: [],
            }
        },
        components: {
            BaseTable,
            EmployeeDetail,
            BaseButton,
            BasePopup,
            BaseInput,
            // BasePagingBar,
            LoadingSpinner
        },
        methods: {
            /**
             * Phương thức thực hiện xóa nhiều
             * Created By TBN(1/9/2021)
             */
            deleteMultipleData() {
                this.isDeleteMultiple = true;
                this.displayConfirmationPopup();
            },
            /**
             * Cập nhật tổng số bản ghi
             * Created By TBN(25/7/2021)
             */
            updateTotalRecord(totalRecord) {
                this.totalRecord = totalRecord;
            },
            /**
             * Hiển thị form thêm nhân viên
             * Created By TBN(25/7/2021)
             */
            async displayAddForm() {
                this.isShowForm = true;
                setTimeout(() => {
                    //this.$refs.employee.init();
                }, 0)
            },
            /**
             * Ẩn spinner
             * Created By TBN(27/7/2021)
             */
            hideSpinner() {
                this.isShowSpinner = false
            },
            /**
             * Ẩn form nhân viên
             * Created By TBN(2/8/2021)
             */
            hideForm() {
                this.isShowForm = false;
            },
            /**
             * Hàm kiểm tra các trường dữ liệu đưa lên request 
             * Created By TBN (20/8/2021)
             */
            checkField(payload) {
                // Lấy dữ liệu departmentId,positionId,employeeFilter từ các trường tìm kiếm
                let contentFilter = this.$refs.searchchingInput.inputContent;
                if (contentFilter != null) {
                    payload["contentFilter"] = contentFilter.trim().toLowerCase();
                } else payload["contentFilter"] = null;
                // Kiểm tra xem có chọn phòng ban,vị trí cụ thể hay không
                //contentFilter != "" ? payload["contentFilter"] = contentFilter : payload["contentFilter"] = "NV"
            },
            /**
             * Lọc dữ liệu theo tiêu chí
             * Created By TBN(27/7/2021)
             */
            async searchData() {
                // Lấy payload từ kho dữ liệu
                let payload = this.$store.state.data.payload
                this.checkField(payload)
                // Mặc định trang đầu tiên
                payload["pageIndex"] = 0
                // Gọi vào mutation để thay đổi payload
                this.$store.commit('CHANGE_PAYLOAD', payload)
                await this.refreshData();
                if (this.totalRecord != 0) {
                    this.$refs.table.$refs.pagingBar.currentPage = 1 // Chuyển trang hiện tại về 1 
                    this.$refs.table.$refs.pagingBar.pages = [1, 2, 3, 4] // Đặt lại nội dung của dãy 4 số ở paging bar
                }
            },
            /**
             * Sự kiện bấm nút refresh
             * Created By TBN(20/8/2021)
             */
            async refreshDataButtonClick(isGoToFirstPage = true) {
                if (this.isReady) {
                    if (isGoToFirstPage) {
                        if (this.$refs.pagingBar)
                            this.$refs.pagingBar.currentPage = 1;
                        this.updatePagination(0);
                    } else {
                        this.refreshData();
                    }
                } else {
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = this.toastMessageResource['unReadyData']
                    let type = "warning"
                    let payloadToast = {
                        content,
                        type,
                        index
                    }
                    this.$store.dispatch('createToast', payloadToast)
                }
            },
            /**
             * Tính toán lại một số trường dữ liệu
             * Created By TBN (28/8/2021)
             */
            formatData(data) {
                if (Array.isArray(data)) {
                    data.forEach(element => {
                        element['GenderName'] = this.genderText(element['Gender']);
                    })
                } else {
                    data['GenderName'] = this.genderText(data['Gender']);
                }
                return data;
            },
            /**
             * Cập nhật lại dữ liệu theo tiêu chí trong payload
             * Created By TBN(27/7/2021)
             */
            async refreshData() {
                // Hiện spinner,chuyển trạng thái chưa sẵn sàng, chuyển trạng thái chưa sẵn sàng xuống table
                this.isShowSpinner = true;
                this.isReady = false;
                this.$refs.table.isReady = false;
                try {
                    // Gọi API lấy dữ liệu theo phân trang,tìm kiếm...
                    let res = await EmployeeAPI.filterData(this.$store.state.data.payload);
                    res.data.Data = this.formatData(res.data.Data); // format lại data
                    // Gán dữ liệu cho bảng
                    this.$refs.table.setData(res.data.Data,res.data.TotalPage,res.data.TotalRecord)

                    // Cập nhật giá trị tổng số trang, tổng số bản ghi
                    this.updateMaxPageNumber(res.data.TotalPage);
                    this.updateTotalRecord(res.data.TotalRecord);
                    
                    // Cập nhật dữ liệu cho bảng: 
                    //  dòng được chọn(checkedList)
                    //  kiểu sắp xếp dữ liệu (true = Desc, false = Asc) (sortDirectionList)
                    //  Danh sách ẩn hiện cho context menu (displayFunctionList)
                    let newList = [];
                    res.data.Data.forEach(() => {
                        newList.push(false);
                    })
                    this.$refs.table.checkedList = [...newList];
                    this.$refs.table.sortDirectionList = [...newList];
                    this.$refs.table.displayFunctionList = [...newList];
                    
                    // Chuyển dấu tích chọn về default
                    if (this.$refs.table.$refs.all.checked)
                        this.$refs.table.$refs.all.click();
                    // Chuyển trạng thái về sẵn sàng, dữ liệu
                    this.isReady = true;
                    this.$refs.table.isReady = true;
                    this.$refs.table.selectedList = [];
                } catch (error) {
                    if (error.response.data.Message != null) {
                        let index = await this.$store.dispatch('randomText', 4)
                        let content = error.response.data.Message.usrMsg
                        let type = "error"
                        let payloadToast = {
                            content,
                            type,
                            index
                        }
                        this.$store.dispatch('createToast', payloadToast)
                    } else {
                        let index = await this.$store.dispatch('randomText', 4)
                        let content = this.toastMessageResource['errorFieldData']
                        let type = "error"
                        let payloadToast = {
                            content,
                            type,
                            index
                        }
                        this.$store.dispatch('createToast', payloadToast)
                    }
                }
                this.hideSpinner(); // Ẩn spinner
            },
            /**
             * Xóa dữ liệu theo danh sách EmployeeId
             * Created by TBN (31/7/2021)
             */
            async deleteData() {
                this.isShowSpinner = true; // Hiện spinner
                try {
                    let res = null;
                    if (this.isDeleteMultiple) {
                        let payload = [];
                        this.$refs["table"].selectedList.forEach(element => payload.push(element.EmployeeId));
                        res = await EmployeeAPI.deleteMultipleData(payload);
                    } else
                        res = await EmployeeAPI.deleteData(this.$refs["table"].selectedList[0].EmployeeId);
                    // Kiểm tra trạng thái thành công hay thất bại của thao tác xóa
                    if (res.data.MISACode == MISACODE.Fail) {
                        // Hiện Popup báo lỗi
                        this.isShowSpinner = false;
                        let objectPopUp = {};
                        objectPopUp.contentPopUp = this.popupContent['deleteFail'];
                        objectPopUp.type = "error"
                        objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
                        this.updateObjectPopUp(objectPopUp);
                        this.isShowPopUp = true;
                        this.$nextTick(() => {
                            this.$refs.confirmation.isShow = true;
                        })
                        this.$refs["table"].selectedList = [];
                    } else {
                        this.isShowSpinner = false;
                        // Hiện toast Message thành công
                        let index = await this.$store.dispatch('randomText', 4)
                        let content = this.toastMessageResource['deleteSuccess']
                        let type = "success"
                        let payloadToast = {
                            content,
                            type,
                            index
                        }
                        this.$store.dispatch('createToast', payloadToast);
                        this.$refs["table"].selectedList = [];
                        await this.refreshData();
                        this.isShowSpinner = false;
                    }
                } catch (error) {
                    // Hiện lỗi trả về (500)
                    this.isShowSpinner = false;
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = error.data.Message.usrMsg
                    let type = "error"
                    let payloadToast = {
                        content,
                        type,
                        index
                    }
                    this.$store.dispatch('createToast', payloadToast)
                }
            },
            /**
             * Hiển thị form xác nhận xóa nhân viên
             * Created By TBN (2/8/2021)
             */
            async displayConfirmationPopup() {
                // Kiểm tra xem có nhân viên nào chưa
                if (this.$refs["table"].selectedList.length == 0) {
                    // Hiện toast Message thành công
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = this.toastMessageResource['emptySelectData']
                    let type = "warning"
                    let payloadToast = {
                        content,
                        type,
                        index
                    }
                    this.$store.dispatch('createToast', payloadToast);
                    return;
                }

                // Chỉnh thông tin cho popup
                let objectPopUp = {};
                if (this.isDeleteMultiple) {
                    objectPopUp.contentPopUp = this.popupContent['deleteMultiple'];
                } else
                    objectPopUp.contentPopUp = this.popupContent['delete'].format(this.$refs["table"].selectedList[
                            0]
                        .EmployeeCode);
                objectPopUp.type = "warning"
                objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

                this.updateObjectPopUp(objectPopUp);
                // Hiện popup
                this.isShowPopUp = true;
                this.$nextTick(() => {
                    this.$refs.confirmation.isShow = true;
                })
            },
            /**
             * Hiển thị form sửa nhân viên
             * Create By TBN(26/7/2021)
             */
            async displayFormEdit(selectedData) {
                // Lấy dữ liệu theo mã 
                try {
                    // Gửi request lấy dữ liệu theo mã định danh
                    let res = await EmployeeAPI.getDataById(selectedData.EmployeeId);
                    // Kiểm tra dữ liệu trả về có không
                    if (res.status != MISACODE.NoContent) {
                        this.isShowForm = true; // Hiện form
                        this.$nextTick(async () => {
                            // Thay đổi thông số
                            this.$refs.employee.statusMode = STATUSMODE.EDIT;
                            // Truyền dữ liệu lên form
                            await this.$refs.employee.bindingData(this.formatData(res.data));
                        })
                    } else {
                        // Hiện Popup dữ liệu đã bị xóa
                        let objectPopUp = {};
                        objectPopUp.contentPopUp = this.popupContent['deletedData'];
                        objectPopUp.type = "error"
                        objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
                        this.updateObjectPopUp(objectPopUp);
                        this.isShowPopUp = true;
                        this.$nextTick(() => {
                            this.$refs.confirmation.isShow = true;
                        })
                    }
                } catch (error) {
                    // Hiện Popup dữ liệu đã thay đổi
                    let objectPopUp = {};
                    objectPopUp.contentPopUp = this.popupContent['errorSystem'];
                    objectPopUp.type = "error"
                    objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
                    this.updateObjectPopUp(objectPopUp);
                    this.isShowPopUp = true;
                    this.$nextTick(() => {
                        this.$refs.confirmation.isShow = true;
                    })
                }
            },
            /**
             * Hiển thị form nhân bản
             * Created BaseTable TBN(21/8/2021)
             */
            async displayFormDuplicate(selectedData) {
                // Lấy dữ liệu theo mã 
                try {
                    // Gửi request lấy dữ liệu theo mã định danh
                    let res = await EmployeeAPI.getDataById(selectedData.EmployeeId);
                    // Kiểm tra dữ liệu trả về có không
                    if (res.status != MISACODE.NoContent) {
                        this.isShowForm = true; // Hiện form
                        this.$nextTick(async () => {
                            // Thay đổi thông số
                            this.$refs.employee.statusMode = STATUSMODE.CLONE;
                            // Truyền dữ liệu lên form
                            await this.$refs.employee.bindingData(this.formatData(res.data));
                        })
                    } else {
                        // Nếu dữ liệu bị xóa thì bind dữ liệu cũ
                        this.isShowForm = true; // Hiện form
                        this.$nextTick(async () => {
                            // Thay đổi thông số
                            this.$refs.employee.statusMode = STATUSMODE.CLONE;
                            // Truyền dữ liệu lên form
                            await this.$refs.employee.bindingData(selectedData);
                        })
                    }
                } catch (error) {
                    // Hiện Popup dữ liệu đã thay đổi
                    let objectPopUp = {};
                    objectPopUp.contentPopUp = this.popupContent['errorSystem'];
                    objectPopUp.type = "error"
                    objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
                    this.updateObjectPopUp(objectPopUp);
                    this.isShowPopUp = true;
                    this.$nextTick(() => {
                        this.$refs.confirmation.isShow = true;
                    })
                }
            },
            // /**
            //  * Cập nhật mảng employeeId muốn xóa
            //  * Created By TBN(26/7/2021)
            //  */
            // updateSelectedData(data, type) {
            //     // hàm này giúp sau này xử lý việc xóa nhiều nhưng vẫn hoạt đọng được với xóa từng bản ghi
            //     if (type == "remove")
            //         this.selectedData = this.selectedData.filter(element => element.EmployeeId != data.EmployeeId)
            //     else {
            //         if (this.selectedData.filter(element => element.EmployeeId == data.EmployeeId).length == 0)
            //             this.selectedData.push(data)
            //     }
            // },
            /**
             * Cập nhật trang hiện tại 
             * Created By TBN(26/7/2021)
             */
            updateMaxPageNumber(pageNumber) {
                this.maxPageNumber = pageNumber;
                this.$refs.table.maxPageNumber = pageNumber;
            },
            /**
             * Cập nhật dữ liệu phân trang 
             * Created By TBN(26/7/2021)
             */
            async updatePagination(pageNumber) {
                // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
                let payload = this.$store.state.data.payload
                this.checkField(payload)
                payload["pageIndex"] = pageNumber
                this.currentPage = pageNumber
                
                await this.refreshData()
            },
            /**
             * Cập nhật kích thước trang
             * Created By TBN (28/7/2021)
             */
            async updatePageSize(pageSize) {
                // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
                let payload = this.$store.state.data.payload
                this.checkField(payload)
                payload["pageSize"] = parseInt(pageSize.split(' ')[0], 10); // Lọc lấy chữ số đầu tiên
                payload["pageIndex"] = 0;
                await this.refreshData();

                // Kiểm tra overflow
                this.checkOverFlowTable();
            },
            /**
             * Kiểm tra xem bảng có bị overflow hay không
             * Nếu có điều chỉnh lại giao diện
             * Created By TBN (19/8/2021)
             */
            checkOverFlowTable() {
                // Đay chỉ là 1 hàm phụ khi bảng bị overflow chỉnh lại vị trí của header sao cho phù hợp
                let contentHeader = document.querySelector('.content .content-header');
                let toolbarContent = document.querySelector('.content .main-content .toolbar-content');
                let rectContentHeader = contentHeader.getBoundingClientRect();
                let reactToolbarContent = toolbarContent.getBoundingClientRect();
                let amountDistance = 30,
                    widthScrollBar = 10;
                let diff = reactToolbarContent.right + amountDistance - rectContentHeader.right;
                let initailPaddingRight = window.getComputedStyle(contentHeader, null).getPropertyValue('padding-right')
                    .replaceAll(/[^0-9]/g, '');
                // Nếu khoảng lệch bằng với độ dài cảu scrollBar và paddingRight của header = với độ lệch ban đầu của 2 thành phần
                if (Math.abs(diff) == widthScrollBar && initailPaddingRight == amountDistance) {
                    contentHeader.style.paddingRight = (parseInt(initailPaddingRight, 10) - diff) + "px";
                } else {
                    if (diff == 0) {
                        contentHeader.style.paddingRight = amountDistance + "px";
                    }
                }
            },
            /**
             * Chuyển đổi tên thành id của nó 
             * Created By TBN (22/8/2021)
             */
            genderText(genderId) {
                let genderText = this.gender.filter(e => e.value == genderId);
                if (genderText.length > 0) {
                    return genderText[0].content;
                } else return null;
            },
            ...mapActions(['getDepartmentData', 'updateObjectPopUp']),
            ...mapMutations(['CHANGE_PAYLOAD'])
        },
        computed: {
            toggle() {
                return this.$store.state.toggle
            },
            objectPopUp() {
                return this.$store.state.data.objectPopUp;
            },
            gender() {
                return this.$store.state.resource[this.$browserLocale]['common']['gender']
            },
            resource() {
                return this.$store.state.resource;
            },
            popupContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'popupContent'
                ];
            },
            footerPopUpButtonContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ]['footerPopUpButton'];
            },
            placeHolder() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'placeHolder'
                ];
            },
            tooltipContent() {
                return this.$store.state.resource[this.$browserLocale]['common'][
                    'tooltipContent'
                ];
            },
            textResource() {
                return this.$store.state.resource[this.$browserLocale]['common']['text'];
            },
            toastMessageResource() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'toastMessage'
                ];
            },
            buttonContentResource() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ];
            }
        }

    }
</script>

<style scoped>
    .toggle-navbar {
        left: 52px;
        width: calc(100% - 221px + 169px);
    }
</style>