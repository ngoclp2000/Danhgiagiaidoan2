<template>
  <div class="content-container salary-content">
    <div :class="{'toggle-navbar' : toggle.isToggleNavbar}" class="content">
      <div class="content-header">
        <div class="title">{{title}}</div>
      </div>

      <div class="main-content">
        <div class="toolbar-content">
          <div class="filter-bar">
            <div class="filter-left">
              <BaseInput :contentPlHolder="placeHolder['searchBarPeriod']"
                classList="icon-search input-search user-input" type="text" errorContent="" refType=""
                validType="search" ref="searchchingInput" @enterPress="searchData" @refreshData="searchData"
                :tooltipContent="tooltipContent['searchBar']" :noneCheck="true" />
            </div>
            <div class="filter-right">
              <div class="add-icon" v-tooltip="tooltipContent['addData']" @click="displayAddForm">

              </div>
              <div class="delete-icon" v-tooltip="tooltipContent['deleteData']" @click="deleteMultipleData">
              </div>
              <div class="refresh-icon" @click="refreshDataButtonClick" v-tooltip="tooltipContent['refreshData']">
              </div>
            </div>
          </div>
        </div>
        <div class="content-grid">
          <BaseTable ref="mainTable" :summarizeField="summarizeFieldName" :type="type" :ths="thsMain"
            :classListGrid="'main-grid'" :classListGridContainer="'grid-salary h3p5 main-grid'"
            @updateTotalRecord="updateTotalRecord" @updatePageSize="updatePageSize" @updatePagination="updatePagination"
            @updateSubTable="updateSubTable" @displayConfirmationPopup="displayConfirmationPopup"
            @displayFormEdit="displayFormEdit" :keyAttribute="'PeriodSalaryId'"
            @changeLockDeleteFirstRecord="changeLockDeleteFirstRecord" />
          <div class="divide-container">
            <div class="divide"></div>
            <div class="resize-icon">

            </div>
          </div>
          <div class="h2p5 sub-grid-container">
            <div class="grid-header">
              <div class="grid-header-title">
                {{resource[this.$browserLocale]['management']['entity']['Salary']['titleSubTable']}}
              </div>
              <div class="zoom-icon" @click="toggleFullscreenSubTable" v-tooltip="tooltipContent['zoom']">

              </div>
            </div>
            <BaseTable :classListGridContainer="'sub-grid'" ref="subTable" :displayFunction="false"
              :displayPaging="false" :disableCheckBox="true" :type="type" :ths="thsSub" />
          </div>
        </div>
        <LoadingSpinner v-if="isShowSpinner && totalRecord != 0" />
      </div>

    </div>
    <SalaryEmployeeDetail v-if="isShowForm" ref="salary" @hideForm="hideForm" @refreshData="refreshDataButtonClick" />
    <BasePopup @hideLayout="isShowPopUp=false" @refreshData="refreshDataButtonClick" @deleteData="deleteData"
      ref="confirmation" v-if="isShowPopUp" />
  </div>
</template>

<script>
  import BaseInput from '../../../components/BaseInput.vue'
  import BaseTable from '../../../components/BaseTable.vue'

  import BasePopup from '../../../components/BasePopup.vue'
  import SalaryEmployeeDetail from './SalaryEmployeeDetail.vue'

  import PeriodSalaryAPI from '../../../api/components/periodsalaryapi.js'
  import PeriodSalaryEmployeeAPI from '../../../api/components/periodsalaryemployeeapi.js'
  import LoadingSpinner from '../../../components/LoadingSpinner.vue'
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
      this.thsMain = this.resource[this.$browserLocale]['management']['entity']['Salary']['tableFieldName']['main'];
      this.thsSub = this.resource[this.$browserLocale]['management']['entity']['Salary']['tableFieldName']['sub'];
      this.title = this.resource[this.$browserLocale]['management']['entity']['Salary']['title'];
    },
    async mounted() {
      this.payload = {
        contentFilter: "",
        pageSize: 10,
        pageIndex: 0
      }
      //this.$store.commit('CHANGE_PAYLOAD', payload);

      // Chỉnh functionList của bảng main và sub
      this.$refs["mainTable"].functionListData = this.$refs["mainTable"].functionListData.filter(element => element
        .key != "duplicate");

      await this.refreshData();

      // Sự kiện resize grid
      let divideContainer = document.querySelector('.salary-content .divide-container');
      divideContainer.addEventListener("mousedown", () => {
        window.onmousemove = (e) => {
          document.body.style.cursor = 'row-resize';
          this.mouseMoveFunction(e);
        }
      });
      window.addEventListener("mouseup", () => {
        document.body.style.cursor = null;
        window.onmousemove = null
      })

    },
    data() {
      return {
        isShowSpinner: true,
        isReady: true,
        isShowPopUp: false,
        isShowForm: false,
        selectedData: [],
        totalRecord: 0,
        currentPage: 1,
        isDeleteMultiple: false,
        title: "",
        type: "salary-table",
        firstRecord: null,
        lockDeleteRecord: false,
        thsMain: [],
        thsSub: [],
        payload: {}
      }
    },
    methods: {
      /**
       * Sự kiện thay đổi khóa xóa dòng đầu tiên của bảng
       * Created By TBN (3/10/2021)
       */
      changeLockDeleteFirstRecord() {
        console.log("Lock");
        this.lockDeleteRecord = true;
      },
      /**
       * Sự kiện di chuyển chuột khi thay đổi size của 2 grid
       * Created By TBN(28/9/2021)
       */
      mouseMoveFunction(event) {
        // Khởi tạo element cần dùng
        let subGrid = document.querySelector(
          '.salary-content .content .main-content .content-grid .sub-grid-container'),
          mainGrid = document.querySelector('.salary-content .content .main-content .content-grid .main-grid'),
          contentGrid = document.querySelector('.salary-content .content .main-content .content-grid');
        let contentGridRec = contentGrid.getBoundingClientRect();
        // Tính cận trên và dưới của khung content
        let upperBounder = contentGridRec.y + contentGridRec.height;
        let lowerBounder = contentGridRec.y;
        // Tính toán xem có có vượt giới hạn không
        let diff = event.y;
        if (event.y < lowerBounder) {
          diff = 0;
        } else if (event.y > upperBounder) {
          diff = upperBounder;
        }
        // Tính phần trăm của grid trên chiếm bao nhiêu
        let heightUpperGridInPercent = (diff - lowerBounder) / (upperBounder - lowerBounder) * 100;
        let limiteUpperGridInPercent = 70,
          limitLowerGridInPercent = 40;
        if (heightUpperGridInPercent > limiteUpperGridInPercent) {
          heightUpperGridInPercent = limiteUpperGridInPercent;
        } else if (heightUpperGridInPercent < limitLowerGridInPercent) {
          heightUpperGridInPercent = limitLowerGridInPercent
        }
        // Gán độ cao mới cho 2 grid
        mainGrid.style.height = (heightUpperGridInPercent) + "%";
        mainGrid.style.maxHeight = (heightUpperGridInPercent) + "%";

        subGrid.style.height = (100 - heightUpperGridInPercent) + "%";
        subGrid.style.maxHeight = (100 - heightUpperGridInPercent) + "%";

      },
      /**
       * Ẩn form nhân viên
       * Created By TBN(2/8/2021)
       */
      hideForm() {
        this.isShowForm = false;
      },
      /**
       * Hiển thị form thêm kỳ lương
       * Created By TBN(25/7/2021)
       */
      async displayAddForm() {
        if(this.isReady){
          this.isShowForm = true;
        }else {
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
       * Hàm chỉnh fulllscreen của subTable
       * Created By TBN(25/9/2021)
       */
      toggleFullscreenSubTable() {
        // Khởi tạo element trong bảng cần chỉnh lại thuộc tính
        let contentGrid = document.querySelector('.salary-content .content .main-content .content-grid');
        let mainGrid = document.querySelector('.salary-content .content .main-content .content-grid .main-grid');
        let subGrid = document.querySelector('.salary-content .content .main-content .content-grid .sub-grid-container')
        let divide = document.querySelector('.salary-content .content .main-content .content-grid .divide-container');
        let iconZoomCollapse = subGrid.querySelector('.zoom-icon');

        if (subGrid.classList.contains("full-height")) {
          // Xóa class cần thiết khi nhấn thu
          subGrid.classList.remove("full-height");
          mainGrid.classList.remove("display-none-imp");
          divide.classList.remove("display-none-imp");
          contentGrid.classList.remove("height-after-zoom");
          iconZoomCollapse.classList.remove("collapse-icon");
        } else {
          // Thêm class cần thiết  khi nhấn phóng
          subGrid.classList.add("full-height");
          mainGrid.classList.add("display-none-imp");
          divide.classList.add("display-none-imp");
          contentGrid.classList.add("height-after-zoom");
          iconZoomCollapse.classList.add("collapse-icon");
        }
      },
      /**
       * Xóa dữ liệu theo danh sách EmployeeId
       * Created by TBN (31/7/2021)
       */
      async deleteData() {
        this.isShowSpinner = true; // Hiện spinner
        let payload = [];
        try {
          let res = null;
          // Kiểm tra xem có phải là xóa nhiều bản ghi hay không
          if (this.$refs["mainTable"].selectedList.length > 1) {
            this.$refs["mainTable"].selectedList.forEach(element => payload.push(element.PeriodSalaryId));
            res = await PeriodSalaryAPI.deleteMultipleData(payload);
          } else
            res = await PeriodSalaryAPI.deleteData(this.$refs["mainTable"].selectedList[0].PeriodSalaryId);

          // Kiểm tra trạng thái thành công hay thất bại của thao tác xóa
          if (res.data.MISACode == MISACODE.Fail) {
            // Hiện Popup báo lỗi
            this.isShowSpinner = false;
            let objectPopUp = {};
            objectPopUp.contentPopUp = this.popupContent['deleteFail'];
            objectPopUp.type = "error"
            objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
            this.updateObjectPopUp(objectPopUp);
            // Hiển thi popup
            this.isShowPopUp = true;
            this.$nextTick(() => {
              this.$refs.confirmation.isShow = true;
            })
            // Đưa danh sách được chọn về rỗng
            this.$refs["mainTable"].selectedList = [];

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
            // Đưa danh sách được chọn vè rỗng 
            this.$refs["mainTable"].selectedList = [];
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
       * Hiển thị form sửa dữ liệu
       * Create By TBN(26/7/2021)
       */
      async displayFormEdit(selectedData) {
        // Lấy dữ liệu theo mã 
        try {
          // Gửi request lấy dữ liệu theo mã định danh
          let res = await PeriodSalaryAPI.getDataById(selectedData.PeriodSalaryId);
          // Kiểm tra dữ liệu trả về có không
          if (res.status != MISACODE.NoContent) {
            this.isShowForm = true; // Hiện form
            this.$nextTick(async () => {
              // Thay đổi thông số
              this.$refs.salary.statusMode = STATUSMODE.EDIT;
              // Truyền dữ liệu lên form
              await this.$refs.salary.bindingData(res.data);

              // Lấy dữ liệu cho bảng phụ
              let params = {
                periodSalaryId: selectedData.PeriodSalaryId
              };
              let resSub = await PeriodSalaryEmployeeAPI.getPeriodSalaryEmployeeListByPeriodSalaryId(params);
              // Gán dữ liệu cho bảng
              this.$refs.salary.$refs.formSalaryTable.setData(resSub.data);

              this.$refs.salary.$refs.formSalaryTable.isReady = true;
              let newList = [];
              // Tạo mới danh sách cho bảng phụ trong form
              this.$refs.salary.keepingDataSubTable = resSub.data.map(a => Object.assign({}, a));
              resSub.data.forEach(() => {
                newList.push(false);
              })
              this.$refs.salary.$refs.formSalaryTable.displayFunctionList = [...newList];
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
          // Hiện Popup lỗi hệ thống
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
       * Hiển thị form xác nhận xóa nhân viên
       * Created By TBN (2/8/2021)
       */
      async displayConfirmationPopup() {
        // Chỉnh thông tin cho popup
        let objectPopUp = {};
        // Kiểm tra xem có nhân viên nào chưa
        if (this.$refs["mainTable"].selectedList.length == 0) {
          // Nếu chưa chọn thì hiển thị toast message yêu cầu chọn 
          // Hiện toast Message thành công
          objectPopUp.contentPopUp = this.popupContent["emptyDeleteDataTable"];       
          objectPopUp.type = "warning-code";   
          objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
          this.updateObjectPopUp(objectPopUp);
          // Hiện popup
          this.isShowPopUp = true;
          this.$nextTick(() => {
            this.$refs.confirmation.isShow = true;
          })
          return;
        }

        // Nếu có kỳ lương được chọn thì sẽ hiển thị popup với nội dung tương ứng với : xóa nhiều | xóa 1 kỳ lương
        if (this.$refs["mainTable"].selectedList.length > 1) {
          objectPopUp.contentPopUp = this.popupContent['deleteMultiple'];
        } else
          objectPopUp.contentPopUp = this.popupContent['delete'].format(this.$refs["mainTable"].selectedList[
              0]
            .PeriodName);
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
       * Phương thức cập nhật dữ liệu của bảng phụ
       * Created By TBN(21/9/2021)
       */
      async updateSubTable(isDefaultChosing) {
        // Chuyển trạng thái không sẵn sàng cho bảng phụ
        this.$refs["subTable"].isReady = false;
        let periodSalaryId = ""
        if (isDefaultChosing) {
          periodSalaryId = this.$refs["mainTable"].dataTable[0].PeriodSalaryId;
        } else
          periodSalaryId = this.$refs["mainTable"].selectedList[0].PeriodSalaryId;
        try {
          // Thiết lập param cho request và gửi request lên server
          let params = {
            periodSalaryId: periodSalaryId
          };
          let res = await PeriodSalaryEmployeeAPI.getPeriodSalaryEmployeeListByPeriodSalaryId(params);
          // Gán dữ liệu cho bảng
          this.$refs.subTable.setData(res.data);
          this.$refs.subTable.isReady = true;
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
          this.$refs.subTable.isReady = true;
        }
      },
      /**
       * Hàm xử lý sự kiện bấm nút refresh dữ liệu
       * Created By TBN (22/9/2021)
       */
      async refreshDataButtonClick(isGoToFirstPage = true) {
        // Kiểm tra xem dữ liệu dã sẵn sàng chưa tránh việc bấm nhiều gửi nhiều request
        if (this.isReady) {
          // Thiết lập lại danh sách được chọn, kiểm tra xem có reset về trang đầu tiên hay không
          this.$refs.mainTable.selectedList = [];
          if (isGoToFirstPage) {
            // Tránh việc lỗi chưa render dữ liệu kịp
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
       * Cập nhật lại dữ liệu theo tiêu chí trong payload
       * Created By TBN(27/7/2021)
       */
      async refreshData(refreshSelectedList) {
        // Hiện spinner,chuyển trạng thái chưa sẵn sàng, chuyển trạng thái chưa sẵn sàng xuống table
        this.isShowSpinner = true;
        this.isReady = false;
        this.$refs.mainTable.isReady = false;
        try {
          // Gọi API lấy dữ liệu theo phân trang,tìm kiếm...
          let res = await PeriodSalaryAPI.filterData(this.payload);
          //res.data.Data = this.formatData(res.data.Data); // format lại data
          // Cập nhật totalRecord
          this.updateTotalRecord(res.data.TotalRecord);
          // Gán dữ liệu cho bảng
          this.$refs.mainTable.setData(res.data.Data, res.data.TotalPage, res.data.TotalRecord);
          this.$refs.subTable.dataTable = [];
          // Cập nhật dữ liệu cho bảng: 
          //  dòng được chọn(checkedList)
          //  kiểu sắp xếp dữ liệu (true = Desc, false = Asc) (sortDirectionList)
          //  Danh sách ẩn hiện cho context menu (displayFunctionList)
          let newList = [];
          res.data.Data.forEach(() => {
            newList.push(false);
          })
          this.$refs.mainTable.sortDirectionList = [...newList];
          this.$refs.mainTable.displayFunctionList = [...newList];
          // Kiểm tra xem có cập nhật lại danh sách được chọn hay không
          if (refreshSelectedList) {
            this.$refs.mainTable.selectedList = [];
          }
          // Kiểm tra cho việc thay đổi dữ liệu dưới bảng(phân trang, tìm kiếm) từ đó xem bản ghi đầu tiên có 
          // cần phải xóa đi khi chuyển trang hay không
          if (this.$refs.mainTable.dataTable.length > 0) {
            if (this.firstRecord != null && !this.lockDeleteRecord) {
              this.$refs.mainTable.changeSelectedData(this.firstRecord, "remove");
            }
            this.firstRecord = this.$refs.mainTable.dataTable[0];
            this.updateSubTable(true);
            this.lockDeleteRecord = false;
            this.$refs.mainTable.changeSelectedData(this.$refs.mainTable.dataTable[0], "add");
            this.$nextTick(()=>{
              this.$refs.mainTable.$refs["row0"][0].focus();
            })
          }

          // Chuyển trạng thái về sẵn sàng, dữ liệu
          this.isReady = true;
          this.$refs.mainTable.isReady = true;
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
          this.$refs.mainTable.isReady = true;
          this.$refs.subTable.isReady = true;
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
      /**
       * Cập nhật tổng số bản ghi
       * Created By TBN(25/7/2021)
       */
      updateTotalRecord(totalRecord) {
        this.totalRecord = totalRecord;
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
        // Gọi vào mutation để thay đổi payload
        //this.$store.commit('CHANGE_PAYLOAD', payload)
        await this.refreshData();
        if (this.$refs.mainTable.totalRecord != 0) {
          this.$refs.mainTable.$refs.pagingBar.currentPage = 1 // Chuyển trang hiện tại về 1 
          this.$refs.mainTable.$refs.pagingBar.pages = [1, 2, 3, 4] // Đặt lại nội dung của dãy 4 số ở paging bar
        }
      },
      /**
       * Phương thức thực hiện xóa nhiều
       * Created By TBN(1/9/2021)
       */
      deleteMultipleData() {
        this.isDeleteMultiple = true;
        this.displayConfirmationPopup();
      },
      /**
       * Cập nhật kích thước trang
       * Created By TBN (28/7/2021)
       */
      async updatePageSize(pageSize) {
        // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
        //let payload = this.$store.state.data.payload
        this.checkField(this.payload)
        this.payload["pageSize"] = parseInt(pageSize.split(' ')[0], 10); // Lọc lấy chữ số đầu tiên
        this.payload["pageIndex"] = 0;
        await this.refreshData();

        // Kiểm tra overflow
        this.checkOverFlowTable();
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
       * Cập nhật dữ liệu phân trang 
       * Created By TBN(26/7/2021)
       */
      async updatePagination(pageNumber) {
        // Xử lý tương tự như lọc dữ liệu chỉ thay đổi pageNumber
        //let payload = this.$store.state.data.payload
        this.checkField(this.payload)
        this.payload["pageIndex"] = pageNumber
        this.currentPage = pageNumber
        // Cập nhật giá trị được chọn 
        await this.refreshData();
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
      ...mapActions(['updateObjectPopUp']),
      ...mapMutations(['CHANGE_PAYLOAD'])
    },
    computed: {
      objectPopUp() {
        return this.$store.state.data.objectPopUp;
      },
      toggle() {
        return this.$store.state.toggle
      },
      resource() {
        return this.$store.state.resource;
      },
      placeHolder() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'placeHolder'
        ];
      },
      textResource() {
        return this.$store.state.resource[this.$browserLocale]['common']['text'];
      },
      tooltipContent() {
        return this.$store.state.resource[this.$browserLocale]['common'][
          'tooltipContent'
        ];
      },
      footerPopUpButtonContent() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary']['buttons'][
          'content'
        ]['footerPopUpButton'];
      },
      toastMessageResource() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'toastMessage'
        ];
      },
      summarizeFieldName() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary']['summarizeField']
      },
      popupContent() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'popupContent'
        ];
      },
    },
    components: {
      BaseInput,
      BaseTable,
      BasePopup,
      SalaryEmployeeDetail,
      LoadingSpinner
    }
  }
</script>

<style scoped>
  .toggle-navbar {
    left: 52px;
    width: calc(100% - 52px) !important;
  }
</style>