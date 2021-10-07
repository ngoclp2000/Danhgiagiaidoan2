<template>
    <div class="m-dialog center-div">
        <div class="layout-blur salary-detail-layout center-div">
        </div>
        <div class="view-table center-div">
            <div class="sticky">
                <div class="header-form-left">
                    <div class="title">{{title}}</div>
                </div>
                <div class="m-btn-list">
                    <div @click="closeViewTable" class="m-btn-close" v-tooltip="tooltipContent['closeButton']">

                    </div>
                </div>
            </div>
            <div class="view-table-search-bar">
                <BaseInput :contentPlHolder="placeHolder" classList="icon-search input-search " type="text"
                    errorContent="" refType="" validType="search" ref="searchchingInput" @enterPress="searchData"
                    @refreshData="searchData" :tooltipContent="tooltipContent['searchBar']" :noneCheck="true" />
            </div>
            <div class="view-table-content">
                <BaseTable :classListGridContainer="'grid-view'" ref="viewTable" :ths="ths" :displayFunction="false"
                    @updateMaxPageNumber="updateMaxPageNumber" @updateTotalRecord="updateTotalRecord"
                    @updatePageSize="updatePageSize" @updatePagination="updatePagination"
                    :keyAttribute="keyAttribute" @changeLockDeleteFirstRecord="changeLockDeleteFirstRecord"/>
            </div>
            <div class="footer">
                <div class="footer-left">

                </div>
                <div class="footer-right">
                    <BaseButton tabindex="22" @click="closeViewTable" classList="cancel m-second-btn"
                        :btnText="footerViewContent['cancel']" class="px-3"
                        :tooltipContent="tooltipContent['footerFormButton']['cancel']" />
                    <BaseButton tabindex="23" :btnText="footerViewContent['add']" classList="m-primary-btn"
                        :tooltipContent="tooltipContent['footerFormButton']['save']" @click="addChosenData" />
                </div>
            </div>
            <LoadingSpinner v-if="isShowSpinner && totalRecord != 0" />
        </div>
    </div>
</template>

<script>
    import BaseInput from './BaseInput.vue'
    import BaseTable from './BaseTable.vue'
    import BaseButton from './BaseButton.vue'
    import LoadingSpinner from './LoadingSpinner.vue'
    export default {
        created() {
            this.payload = {
                contentFilter: "",
                pageSize: 10,
                pageIndex: 0
            }
        },
        async mounted() {
            // Chỉnh functionList của bảng
            this.$refs["viewTable"].functionListData = [];
            this.$refs.viewTable.isReady = true;
            await this.refreshData();
        },
        props: {
            api: {
                type: Object,
            },
            ths: {
                type: Array,
                default: null
            },
            placeHolder: {
                type: String,
                default: ""
            },
            keyAttribute: {
                type: String,
                default: ""
            },
            title: {
                type: String,
                default: ""
            }
        },
        data() {
            return {
                isShowSpinner: true,
                totalRecord: 1,
                payload: {},
                lockDeleteRecord: false,
                firstRecord: null
            }
        },
        components: {
            BaseInput,
            BaseTable,
            BaseButton,
            LoadingSpinner,
        },
        methods: {
            /**
             * Thay đổi khóa xóa dữ liệu đầu tiên khi chuyển trang
             * Created By TBN (3/10/2021)
             */
            changeLockDeleteFirstRecord() {
                this.lockDeleteRecord = true;
            },
            /**
             * Hàm xử lý sự kiện bấm nút refresh dữ liệu
             * Created By TBN (22/9/2021)
             */
            async refreshDataButtonClick(isGoToFirstPage = true) {
                if (this.isReady) {
                    this.$refs.viewTable.selectedList = [];
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
             * Hàm thực hiện việc binding dữ liệu 
             * Created By TBN(24/9/2021)
             */
            addChosenData() {
                this.$emit("addChosenData", this.$refs.viewTable.selectedList);
                this.closeViewTable();
            },
            /**
             * Hàm sự kiện xử lý ẩn view
             * Created By TBN(24/9/2021)
             */
            closeViewTable() {
                this.$emit("closeViewTable")
            },
            /**
             * Lọc dữ liệu theo tiêu chí
             * Created By TBN(27/7/2021)
             */
            async searchData() {
                // Lấy payload từ kho dữ liệu
                this.checkField(this.payload)
                // Mặc định trang đầu tiên
                this.payload["pageIndex"] = 0
                await this.refreshData();
                if (this.totalRecord != 0) {
                    this.$refs.viewTable.$refs.pagingBar.currentPage = 1 // Chuyển trang hiện tại về 1 
                    this.$refs.viewTable.$refs.pagingBar.pages = [1, 2, 3,
                        4
                    ] // Đặt lại nội dung của dãy 4 số ở paging bar
                }
            },
            /**
             * Cập nhật dữ liệu phân trang 
             * Created By TBN(26/7/2021)
             */
            async updatePagination(pageNumber) {
                // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
                this.checkField(this.payload)
                this.payload["pageIndex"] = pageNumber
                this.currentPage = pageNumber
                // Cập nhật giá trị được chọn 
                await this.refreshData();
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
             * Cập nhật kích thước trang
             * Created By TBN (28/7/2021)
             */
            async updatePageSize(pageSize) {
                // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
                this.checkField(this.payload)
                this.payload["pageSize"] = parseInt(pageSize.split(' ')[0], 10); // Lọc lấy chữ số đầu tiên
                this.payload["pageIndex"] = 0;
                await this.refreshData();
            },
            /**
             * Cập nhật trang hiện tại 
             * Created By TBN(26/7/2021)
             */
            updateMaxPageNumber(pageNumber) {
                this.$nextTick(() => {
                    this.maxPageNumber = pageNumber;
                    this.$refs.viewTable.maxPageNumber = pageNumber;
                })
            },
            /**
             * Cập nhật tổng số bản ghi
             * Created By TBN(25/7/2021)
             */
            updateTotalRecord(totalRecord) {
                this.$nextTick(() => {
                    this.totalRecord = totalRecord;
                    this.$refs.viewTable.totalRecord = totalRecord;
                })
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
                console.log(data);
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
                this.$refs.viewTable.isReady = false;
                try {
                    // Gọi API lấy dữ liệu theo phân trang,tìm kiếm...
                    let res = await this.api.filterData(this.payload);
                    //res.data.Data = this.formatData(res.data.Data); // format lại data
                    // Cập nhật giá trị tổng số trang, tổng số bản ghi
                    this.updateMaxPageNumber(res.data.TotalPage);
                    this.updateTotalRecord(res.data.TotalRecord);
                    res.data.Data = this.formatData(res.data.Data);
                    // Gán dữ liệu cho bảng
                    this.$refs.viewTable.setData(res.data.Data, res.data.TotalPage, res.data.TotalRecord);
                    //res.data.Data = await this.formatData(res.data.Data); // format lại data
                    // Chọn dòng đầu tiên
                    this.$refs.viewTable.selectedList.push(res.data.Data[0]);
                    this.$nextTick(()=>{
                        if (this.$refs.viewTable.dataTable.length > 0) {
                        if (this.firstRecord != null && !this.lockDeleteRecord) {
                            this.$refs.viewTable.changeSelectedData(this.firstRecord, "remove");
                        }
                        this.firstRecord = this.$refs.viewTable.dataTable[0];
                        this.lockDeleteRecord = false;
                        this.$refs.viewTable.changeSelectedData(this.$refs.viewTable.dataTable[0], "add");
                        this.$refs.viewTable.$refs["row0"][0].focus();
                    }
                    })
                    // Cập nhật dữ liệu cho bảng: 
                    //  kiểu sắp xếp dữ liệu (true = Desc, false = Asc) (sortDirectionList)
                    //  Danh sách ẩn hiện cho context menu (displayFunctionList)
                    let newList = [];
                    res.data.Data.forEach(() => {
                        newList.push(false);
                    })
                    this.$refs.viewTable.sortDirectionList = [...newList];
                    this.$refs.viewTable.displayFunctionList = [...newList];

                    // Chuyển trạng thái về sẵn sàng, dữ liệu
                    this.isReady = true;
                    this.$refs.viewTable.isReady = true;
                   
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
             * Ẩn spinner
             * Created By TBN(27/7/2021)
             */
            hideSpinner() {
                this.isShowSpinner = false
            },
        },
        computed: {
            tooltipContent() {
                return this.$store.state.resource[this.$browserLocale]['common'][
                    'tooltipContent'
                ];
            },
            footerViewContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ]['viewButton'];
            },
            gender() {
                return this.$store.state.resource[this.$browserLocale]['common']['gender']
            },
        }
    }
</script>

<style>

</style>