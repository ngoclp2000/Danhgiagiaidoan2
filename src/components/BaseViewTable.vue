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
                    @refreshData="searchData" :tooltipContent="tooltipContent['searchBar']" noneCheck="true" />
            </div>
            <div class="view-table-content">
                <BaseTable :classListGridContainer="'grid-view'" ref="viewTable" :ths="ths" :displayFunction="false"
                    @updateMaxPageNumber="updateMaxPageNumber" @updateTotalRecord="updateTotalRecord"
                    @updatePageSize="updatePageSize" @updatePagination="updatePagination" />
            </div>
            <div class="footer">
                <div class="footer-left">

                </div>
                <div class="footer-right">
                    <BaseButton tabindex="22" @click="closeViewTable" classList="cancel m-second-btn"
                        :btnText="footerViewContent['cancel']" class="px-3" 
                        :tooltipContent="tooltipContent['footerFormButton']['cancel']" />
                    <BaseButton tabindex="23" :btnText="footerViewContent['add']" classList="m-primary-btn"
                        :tooltipContent="tooltipContent['footerFormButton']['save']" 
                        @click="addChosenData"
                        />
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

        },
        async mounted() {
            // Chỉnh functionList của bảng
            this.$refs["viewTable"].functionListData = [];
            this.$refs.viewTable.totalRecord = 1;
            this.$refs.viewTable.isReady = true;

            this.$nextTick(async () => {
                await this.refreshData();
            })
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
            }
        },
        data() {
            return {
                title: "Thêm mới",
                isShowSpinner: true,
                totalRecord: 0
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
             * Hàm thực hiện việc binding dữ liệu 
             * Created By TBN(24/9/2021)
             */
            addChosenData(){
                this.$emit("addChosenData",this.$refs.viewTable.selectedList);
                this.closeViewTable();
            },
            /**
             * Hàm sự kiện xử lý ẩn view
             * Created By TBN(24/9/2021)
             */
            closeViewTable(){
                this.$emit("closeViewTable")
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
                await this.refreshData();
                if (this.totalRecord != 0) {
                    this.$refs.viewTable.$refs.pagingBar.currentPage = 1 // Chuyển trang hiện tại về 1 
                    this.$refs.viewTable.$refs.pagingBar.pages = [1, 2, 3, 4] // Đặt lại nội dung của dãy 4 số ở paging bar
                }
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
                // Cập nhật giá trị được chọn 
                this.$refs["viewTable"].selectedList = [];
                await this.refreshData()
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
                let payload = this.$store.state.data.payload
                this.checkField(payload)
                payload["pageSize"] = parseInt(pageSize.split(' ')[0], 10); // Lọc lấy chữ số đầu tiên
                payload["pageIndex"] = 0;
                await this.refreshData();

            },
            /**
             * Cập nhật trang hiện tại 
             * Created By TBN(26/7/2021)
             */
            updateMaxPageNumber(pageNumber) {
                this.maxPageNumber = pageNumber;
                this.$refs.viewTable.maxPageNumber = pageNumber;
            },
            /**
             * Cập nhật tổng số bản ghi
             * Created By TBN(25/7/2021)
             */
            updateTotalRecord(totalRecord) {
                this.totalRecord = totalRecord;
                this.$refs.viewTable.totalRecord = totalRecord;
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
                    let res = await this.api.filterData(this.$store.state.data.payload);
                    //res.data.Data = this.formatData(res.data.Data); // format lại data
                    // Cập nhật giá trị tổng số trang, tổng số bản ghi
                    this.updateMaxPageNumber(res.data.TotalPage);
                    this.updateTotalRecord(res.data.TotalRecord);
                    // Gán dữ liệu cho bảng
                    this.$refs.viewTable.setData(res.data.Data)
                    // Cập nhật dữ liệu cho bảng: 
                    //  dòng được chọn(checkedList)
                    //  kiểu sắp xếp dữ liệu (true = Desc, false = Asc) (sortDirectionList)
                    //  Danh sách ẩn hiện cho context menu (displayFunctionList)
                    let newList = [];
                    res.data.Data.forEach(() => {
                        newList.push(false);
                    })
                    this.$refs.viewTable.checkedList = [...newList];
                    this.$refs.viewTable.sortDirectionList = [...newList];
                    this.$refs.viewTable.displayFunctionList = [...newList];

                    // Chuyển dấu tích chọn về default
                    if (this.$refs.viewTable.$refs.all.checked)
                        this.$refs.viewTable.$refs.all.click();
                    else {
                        for (const [key] of Object.entries(this.$refs.viewTable.$refs)) {
                            if (key == "all")
                                continue
                            if (this.$refs.viewTable.$refs[key][0]) {
                                if (this.$refs.viewTable.$refs[key][0].checked) {
                                    this.$refs.viewTable.$refs[key][0].click();
                                }
                            }
                        }
                    }
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
        }
    }
</script>

<style>

</style>