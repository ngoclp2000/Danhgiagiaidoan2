<template>
  <div class="m-dialog center-div" @keydown.ctrl.83.exact.prevent.stop="storeInformation(false)"
    @keydown.esc.prevent.stop="displayWarning">
    <div class="layout-blur salary-detail-layout center-div">
    </div>

    <div class="salary-detail center-div">
      <div class="sticky">
        <div class="header-form-left">
          <div class="title">{{title}}</div>
        </div>
        <div class="m-btn-list">
          <div @click="displayWarning()" class="m-btn-close" v-tooltip="tooltipContent['closeButton']">

          </div>
        </div>
      </div>

      <div class="salary-detail-content" ref="salaryContent">
        <div class="upper-part">
          <div class="display-flex">
            <div class="input-information w1p3">
              <div class="name-information">
                {{formFieldTitle['PeriodName']}} <span style="color: red;">*</span>
              </div>
              <BaseInput tabindex="1" maxlength="255" ref="PeriodName" classList="user-input required" type="text"
                refType="employeeCode" name="PeriodName" validType="required"
                @bindingDataInput="bindingDataInput('PeriodName',$refs['PeriodName'].inputContent)" />
            </div>
            <div class="input-information w1p3">
              <div class="name-information">
                {{formFieldTitle['Month']}} <span style="color: red;">*</span>
              </div>
              <BaseCombobox tabindex="12" ref="Month" class="cbo select-container full-w" dataType="month" name="Month"
                maxlength="255" validType="required"
                @bindingDataInput="bindingDataInput('Month',$refs['Month'].inputContent)" />
            </div>
            <div class="input-information w1p3 mr0">
              <div class="name-information">
                {{formFieldTitle['Year']}} <span style="color: red;">*</span>
              </div>
              <BaseCombobox tabindex="12" ref="Year" class="cbo select-container full-w" dataType="year" name="Year"
                maxlength="255" validType="required"
                @bindingDataInput="bindingDataInput('Year',$refs['Year'].inputContent)" />
            </div>
          </div>
          <div>
            <div class="input-information full-w mr0">
              <div class="name-information">
                {{formFieldTitle['Note']}}
              </div>
              <BaseInput tabindex="15" ref="Note" classList="user-input required " type="text" name="Note"
                maxlength="255" @bindingDataInput="bindingDataInput('Note',$refs['Note'].inputContent)" />
            </div>
          </div>
        </div>

        <div class="bottom-part">
          <div class="grid-table">
            <div class="grid-table-header">
              <div class="grid-table-header-title">
                {{resource[this.$browserLocale]['management']['entity']['Salary']['titleSubTable']}}
              </div>
              <div class="btn-container">
                <div class="add-icon" v-tooltip="tooltipContent['addData']" @click="isShowViewTable=true">

                </div>
                <div class="delete-icon" v-tooltip="tooltipContent['deleteData']" @click="displayConfirmationPopup">
                </div>
              </div>
            </div>
            <div class="grid-table-search-bar">
              <BaseInput :contentPlHolder="placeHolder['searchBarPeriodEmployee']" classList="icon-search input-search "
                type="text" errorContent="" refType="" validType="search" ref="searchchingInput"
                @enterPress="searchData" @refreshData="searchData" :tooltipContent="tooltipContent['searchBar']"
                noneCheck="true" />
            </div>
            <BaseTable :classListGridContainer="'grid-form'" :ths="ths" ref="formSalaryTable" type="employeeSalaryForm"
              @displayConfirmationPopup="displayConfirmationPopup" :summarizeField="summarizeFieldName"
              :displayPaging="false" />
          </div>

        </div>
        <div class="footer">
          <div class="footer-left">

          </div>
          <div class="footer-right">
            <BaseButton tabindex="22" @click="$emit('hideForm')" classList="cancel m-second-btn"
              :btnText="footerFormButtonContent['cancel']" class="px-3"
              :tooltipContent="tooltipContent['footerFormButton']['cancel']" />
            <BaseButton tabindex="23" :btnText="footerFormButtonContent['save']" classList="m-primary-btn"
              :tooltipContent="tooltipContent['footerFormButton']['save']" @click="storeInformation(false)" />
          </div>
        </div>
      </div>
    </div>
    <BasePopup ref="popUp" @hideForm="$emit('hideForm')" @focusErrorField="focusErrorField" v-if="isShowPopUp"
      @hideLayout="isShowPopUp=false" @deleteData="deleteData" />
    <BaseViewTable :ths="thsViewTable" :api="api" @closeViewTable="isShowViewTable=false" v-if="isShowViewTable"
      :placeHolder="placeHolder['searchBarEmployeeView']" @addChosenData="addChosenData" />
  </div>
</template>

<script>
  import BaseInput from '../../../components/BaseInput.vue'
  import BaseCombobox from '../../../components/BaseCombobox.vue'
  import BaseTable from '../../../components/BaseTable.vue'
  import BaseButton from '../../../components/BaseButton.vue'
  import BasePopup from '../../../components/BasePopup.vue'
  import BaseViewTable from '../../../components/BaseViewTable.vue'
  import Format from '../../../store/format'

  import PeriodSalaryAPI from '../../../api/components/periodsalaryapi'
  import PeriodSalaryEmployeeAPI from '../../../api/components/periodsalaryemployeeapi'
  import EmployeeAPI from '../../../api/components/employeeapi'

  import {
    mapActions,
  } from 'vuex'
  import {
    STATUSMODE,
    MISACODE
  } from '../../../enum/index'
  import Common from '../../../store/common';
  export default {
    created() {
      if (this.statusMode == STATUSMODE.ADD) {
        this.keepingPeriodSalary["PeriodSalaryId"] = Common.generateUUID();
        this.currentPeriodSalary["PeriodSalaryId"] = this.keepingPeriodSalary["PeriodSalaryId"];
      }
      this.ths = this.resource[this.$browserLocale]['management']['entity']['Salary']['tableFieldName']['form'];
      this.thsViewTable = this.resource[this.$browserLocale]['management']['entity']['Employee']['tableFieldName'][
        'viewTable'
      ];
      this.title = this.resource[this.$browserLocale]['management']['entity']['Salary']['formTitle'];
    },
    mounted() {
      this.api = EmployeeAPI;
      // Lấy dữ liệu cho combobox tháng, năm
      let monthArray = Common.getRangeMonth();

      let currentYear = new Date().getFullYear(),
        yearArray = Common.getRangeYear(currentYear);

      this.$refs.Month.setData(monthArray);
      this.$refs.Year.setData(yearArray);

      // Chỉnh functionList của bảng main và sub
      this.$refs["formSalaryTable"].functionListData = this.$refs["formSalaryTable"].functionListData.filter(element =>
        element
        .key == "delete");

      // Chuyển trạng thái ready cho bảng
      if (this.statusMode == STATUSMODE.ADD) {
        this.$refs["formSalaryTable"].isReady = true;
      }

      // Đưa tabindex về đầu tiên khi tabindex cuối cùng
      let tabIndexElement = this.$refs["salaryContent"].querySelectorAll('input.user-input, button');
      let lastIndex = tabIndexElement[tabIndexElement.length - 1];
      lastIndex.addEventListener('keydown', (e) => {
        if ((e.which === 9 && !e.shiftKey)) {
          e.preventDefault();
          this.$refs.PeriodName.$refs.input.focus() // Focus PeriodName input
        }
      })

      // Đưa tabindex về cuối khi shift tab tabindex đầu tiên
      let tabIndexField = this.$refs["salaryContent"].querySelectorAll('input.user-input');
      let lastTeabIndexField = tabIndexField[tabIndexField.length - 1];
      this.$refs.PeriodName.$refs.input.addEventListener('keydown', (e) => {
        if ((e.which === 9 && e.shiftKey)) {
          e.preventDefault();
          lastTeabIndexField.focus() // Focus EmployeeCode input
        }
      })

      // Focus vào input đầu tiền
      this.$refs.PeriodName.$refs.input.focus();
      // Đặt giá trị ban đầu cho combobox ngày, tháng là tháng, năm hiện tại
      let currentDate = new Date();
      if (this.statusMode == STATUSMODE.ADD) {
        this.$refs.Month.setInputContent(currentDate.getMonth() + 1);
        this.$refs.Year.setInputContent(currentDate.getFullYear());
        this.keepingPeriodSalary["Month"] = currentDate.getMonth() + 1;
        this.keepingPeriodSalary["Year"] = currentDate.getFullYear();
      }

    },
    data() {
      return {
        title: "",
        ths: "",
        thsViewTable: null,
        currentPeriodSalary: {},
        keepingPeriodSalary: {},
        keepingDataSubTable: null,
        statusMode: STATUSMODE.ADD,
        isShowSpinner: false,
        isShowPopUp: false,
        isShowViewTable: false,
        api: null,
        arrayDeleteSalaryPeriodEmployeeId: [],
        arrayAddedEmployee: [],
        arrayChangedSalaryPeriodEmployee: [],
      }
    },
    methods: {
      /**
       * Hàm xóa dữ liệu của bảng
       * Created by TBN(25/9/2021)
       */
      deleteData() {
        let arrayDeleted = this.$refs["formSalaryTable"].selectedList.map(element => element.EmployeeId);
        let dataTable = this.$refs["formSalaryTable"].dataTable;
        // Lọc lại nhân viên sau khi xóa
        this.$refs["formSalaryTable"].dataTable = [...dataTable.filter(element => !arrayDeleted.includes(element
          .EmployeeId))];

        this.$refs["formSalaryTable"].selectedList = []; // Đưa danh sách được chọn về rỗng

        // Cập nhật mảng checkbox của bảng
        let newList = [];
        dataTable.forEach(() => {
          newList.push(false);
        })
        this.$refs["formSalaryTable"].checkedList = newList;

        // Xóa hết các dấu tick
        for (let i = 0; i < newList.length; i++) {
          this.$refs["formSalaryTable"].$refs["checkbox" + i][0].checked = false;
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
        switch (this.$refs["formSalaryTable"].selectedList.length) {
          case 0: {
            // Hiện toast Message thành công
            let index = await this.$store.dispatch('randomText', 4)
            let content = this.toastMessageEmployeeResource['emptySelectData']
            let type = "warning"
            let payloadToast = {
              content,
              type,
              index
            }
            this.$store.dispatch('createToast', payloadToast);
          }
          return;
        case 1: {
          objectPopUp.contentPopUp = this.popupContentEmployee['delete'].format(this.$refs["formSalaryTable"]
            .selectedList[
              0]
            .EmployeeCode);
        }
        break;
        default:
          objectPopUp.contentPopUp = this.popupContentEmployee['deleteMultiple'];
          break;
        }
        // Xét dữ liệu cho popup
        objectPopUp.type = "warning"
        objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];
        this.updateObjectPopUp(objectPopUp);
        // Hiện popup
        this.isShowPopUp = true;
        this.$nextTick(() => {
          this.$refs.popUp.isShow = true;
        })
      },
      /**
       * Binding dữ liệu đã chọn xuống bảng trong form
       * Created By TBN (25/9/2021)
       */
      addChosenData(data) {
        let dataTable = this.$refs["formSalaryTable"].dataTable;
        // Lọc dữ liệu được thêm vào mà không trùng với dữ liệu có sẵn
        let newData = data.filter(element => !dataTable.map(e => e.EmployeeId).includes(element.EmployeeId));

        // Binding dữ liệu tính toán cho dữ liệu mới
        newData.forEach(element => {
          element.CoefficientsSalary = 0;
          element.Subsidy = 0;
          element.SocialInsuranceCost = 0;
          element.Salary = 0;
          element.PeriodSalaryId = this.keepingPeriodSalary.PeriodSalaryId
        });

        let newDataTable = [...dataTable, ...newData]
        // Cập nhật dữ liệu cho bảng
        this.$refs["formSalaryTable"].dataTable = newDataTable;
        this.$refs["formSalaryTable"].keepingDataTable = newDataTable;
        // Cập nhật mảng được chọn cho bảng
        let newList = [];
        newDataTable.forEach(() => {
          newList.push(false);
        })
        this.$refs["formSalaryTable"].checkedList = newList;
      },
      /**
       * Kiểm tra tính đúng đắn của các trường dữ liệu trong cửa sổ nhập dữ liệu
       * Created By TBN (26/7/2021)
       */
      async checkValidity() {

        let errorContents = [] // Nội dung lỗi
        // Ditionary chuyển từ tên trường TA sang TV
        let convertText = this.resource[this.$browserLocale]['common'][
          'dictionaryError'
        ];
        // Loop qua các refs của input và combobox
        for (const [key] of Object.entries(this.$refs)) {
          if (!this.$refs[key] || (this.$refs[key].inputContent == null && this.$refs[key].date == null))
            continue;
          let value = this.$refs[key].inputContent || this.$refs[key].date;

          //let content = "" // Lưu nội dung của lỗi hiện tại
          // Nếu lỗi hoặc trông thì sẽ gán lỗi
          if (this.$refs[key].validType == "required" && (value == null || value == "")) {
            let content = convertText[key]['empty']
            this.$refs[key].isError = true;
            // Nếu có lỗi thì sẽ đẩy vào mảng lỗi
            errorContents.push({
              key,
              content,
            })
            // Đẩy lỗi cho combobox
            if (this.$refs[key].$refs.inputCombobox != null) {
              this.$refs[key].$refs.inputCombobox.errorContentData = content;
            } else {
              this.$refs[key].errorContentData = content;
            }
          }
        }

        // Custom Validate : Phần này sẽ tùy thuộc vào component hiện tại đang làm gì
        // Kiểm tra bảng có dữ liệu trong bảng hay không
        if (this.$refs["formSalaryTable"].dataTable.length == 0) {
          // Thiết lập thông tin cho popup sau đó hiển thị
          let objectPopUp = {};
          objectPopUp.contentPopUp = this.popupContentSalary['emptyDataTable'];
          objectPopUp.type = "warning-code"
          objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

          this.updateObjectPopUp(objectPopUp);
          this.isShowPopUp = true;
          errorContents.push({
            ignored: true,
          })
          this.$nextTick(() => {
            this.$refs.popUp.isShow = true;
            this.$refs.popUp.errorContents = errorContents;
          })
        }
        // Kiểm tra input trong bảng
        let listFieldToCheck = ["CoefficientsSalary"];
        for (let i = 0; i < this.$refs["formSalaryTable"].dataTable.length; i++) {
          for (let j = 0; j < listFieldToCheck.length; j++) {
            let inputContent = this.$refs["formSalaryTable"].$refs[listFieldToCheck[j] + i][0].$el.querySelector(
              'input').value;
            if (inputContent == "0" || !inputContent) {
              let content = convertText[listFieldToCheck[j]]['empty'];

              errorContents.push({
                key: listFieldToCheck[j],
                ref: listFieldToCheck[j] + i,
                content,
              })
            }
          }
        }

        return errorContents
      },
      /**
       * Hàm kiểm tra sự thay đổi của bảng 
       * Created By TBN (26/9/2021)
       */
      checkChangeSubTable() {
        // Dữ liệu ban đầu của bảng
        let keepingDataSubTable = (this.keepingDataSubTable ? this.keepingDataSubTable : []);
        // Mảng mã định danh nhân viên ban đầu
        let arrayInitialEmployeeId = keepingDataSubTable.map(element => element.EmployeeId);
        // Mảng mã định danh nhân viên mới sau thêm, sửa hoặc xóa
        let arrayNewEmployeeId = this.$refs["formSalaryTable"].dataTable.map(element => element.EmployeeId);
        // Mảng mã định danh nhân viên cũ, xóa hoặc được thêm mới
        let arrayOldEmployeeId = [],
          arrayDeleteEmployeeId = [],
          arrayAddedEmployeeId = [];

        // Tìm những mã định danh nhân viên đã bị xóa khỏi bảng để xóa khỏi database
        arrayDeleteEmployeeId = arrayInitialEmployeeId.filter(element => !arrayNewEmployeeId.includes(element));
        // Tìm những mã định danh nhân viên vẫn được giữ lại
        arrayOldEmployeeId = arrayInitialEmployeeId.filter(element => arrayNewEmployeeId.includes(element));
        // Tìm những mã định danh nhân viên mới được thêm vào
        arrayAddedEmployeeId = arrayNewEmployeeId.filter(element => !arrayInitialEmployeeId.includes(element));

        // Danh sách nhân viên cũ
        let arrayOldEmployee = keepingDataSubTable.filter(element => arrayOldEmployeeId.includes(element
          .EmployeeId));
        // Danh sách nhân viên cũ nhưng trong dữ liệu mới
        let arrayOldChangedEmployee = this.$refs["formSalaryTable"].dataTable.filter(element => arrayOldEmployeeId
          .includes(element.EmployeeId));
        // Danh sách nhân viên được thêm mới
        this.arrayAddedEmployee = this.$refs["formSalaryTable"].dataTable.filter(element => arrayAddedEmployeeId
          .includes(element.EmployeeId))
        // Kiểm tra sự thay đổi dữ liệu của nhân viên cũ
        arrayOldEmployee.forEach(element => {
          let checkedEmployee = arrayOldChangedEmployee.filter(e => e.EmployeeId == element.EmployeeId)[0];
          if (JSON.stringify(checkedEmployee) != JSON.stringify(element)) {
            // Nếu có sự thay đổi thì thêm mới vào  mảng nhân viên thay đổi
            checkedEmployee["PeriodSalaryEmployeeId"] = element. ["PeriodSalaryEmployeeId"];
            this.arrayChangedSalaryPeriodEmployee.push(checkedEmployee);
          }
        })
        // Mảng mã định danh nhân viên bị xóa
        this.arrayDeleteSalaryPeriodEmployeeId = keepingDataSubTable.filter(element => arrayDeleteEmployeeId
          .includes(element.EmployeeId)).map(e => e.PeriodSalaryEmployeeId);
        // Kiểm tra xem có sự thay đổi của dữ liệu trong bảng
        if (arrayDeleteEmployeeId.length != 0 || this.arrayAddedEmployee.length != 0 || this
          .arrayChangedSalaryPeriodEmployee
          .length != 0) {
          return true;
        }
        return false;
      },
      /**
       * Cập nhật dữ liệu bảng vào database
       * Created By TBN (23/9/2021)
       */
      async updateSubInformation() {
        // Xóa dữ liệu nhân viên
        this.arrayDeleteSalaryPeriodEmployeeId.forEach(async function (element) {
          await PeriodSalaryEmployeeAPI.deleteData(element);
        })
        // Thêm mới dữ liệu nhân viên
        this.arrayAddedEmployee.forEach(async function (element) {
          await PeriodSalaryEmployeeAPI.createNewData(element);
        })
        // Sửa thông tin nhân viên cũ
        this.arrayChangedSalaryPeriodEmployee.forEach(async function (element) {
          await PeriodSalaryEmployeeAPI.updateData(element.PeriodSalaryEmployeeId, element);
        })
        // Chuyển dữ liệu về rỗng
        this.arrayDeleteSalaryPeriodEmployeeId = [];
        this.arrayAddedEmployee = [];
        this.arrrChangedSalaryPeriodEmployee = [];
      },
      /**
       * Lưu dữ liệu hoặc sửa dữ liệu của nhân viên
       * Created By TBN (26/7/2021)
       */
      async storeInformation(isMore) {
        // Kiểm tra trạng thái dữ liệu đã thay đổi hay chưa
        let check = await this.checkUnchangeState();
        if (check) {
          // Kiểm tra sự thay đổi dữ liệu của bảng 
          if (this.checkChangeSubTable()) {
            let errorContents = await this.checkValidity(); // Nhận mảng nội dung lỗi 
            if (errorContents.length == 0) {
              try {
                await this.updateSubInformation();
                // Thành công
                let index = await this.$store.dispatch('randomText', 4)
                let type = "success";
                let content = this.toastMessageSalaryResource['updateSuccess'];
                let payloadToast = {
                  content,
                  type,
                  index
                }
                this.$store.dispatch('createToast', payloadToast);
                this.$emit('hideForm');
                this.$emit('refreshData', false);
              } catch (e) {
                console.log(e);
              }
            } else {
              this.focusErrorField(errorContents);
            }

          } else {
            let index = await this.$store.dispatch('randomText', 4)
            let content = this.toastMessageSalaryResource['unChangeData'];
            let type = "warning"
            let payloadToast = {
              content,
              type,
              index
            }
            this.$store.dispatch('createToast', payloadToast)
          }

        } else {
          this.isShowSpinner = true;
          this.$emit('displayLayout') // Hiện layout
          let errorContents = await this.checkValidity(); // Nhận mảng nội dung lỗi 
          if (errorContents.length == 0) {
            // Không có lỗi thì format lại dữ liệu rồi gửi request

            // Kiểm tra là sửa hay thêm mới
            if (this.statusMode == STATUSMODE.EDIT) {
              // Sửa dữ liệu sau đó thông báo bằng toast message
              try {
                let res = await PeriodSalaryAPI.updateData(this.currentPeriodSalary.PeriodSalaryId, this
                  .currentPeriodSalary);
                if (res.data.MISACode == MISACODE.NotValid) {
                  // Validate lỗi nhưng mà đề bài này k cần đến
                  let arrError = res.data.Message;
                  arrError.forEach(async (element) => {
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = element.usrMsg
                    let type = "error"
                    let payloadToast = {
                      content,
                      type,
                      index
                    }
                    this.$store.dispatch('createToast', payloadToast)
                  });
                } else if (res.data.MISACode == MISACODE.Success) {
                  // Cập nhật dữ liệu của bảng nếu có thay đổi xảy ra
                  if (this.checkChangeSubTable()) {
                    await this.updateSubInformation();
                  }
                  // Thành công
                  let index = await this.$store.dispatch('randomText', 4)
                  let type = "success";
                  let content = this.toastMessageSalaryResource['updateSuccess'];
                  let payloadToast = {
                    content,
                    type,
                    index
                  }
                  this.$store.dispatch('createToast', payloadToast)
                  if (!isMore) {
                    this.$emit('hideForm');
                    this.$emit('refreshData', false);
                  } else {
                    this.statusMode = STATUSMODE.ADD;
                    await this.init();
                  }

                } else {
                  // Thất bại
                  let index = await this.$store.dispatch('randomText', 4)
                  let type = "error";
                  let content = this.toastMessageSalaryResource['updateFail'];
                  let payloadToast = {
                    content,
                    type,
                    index
                  }
                  this.$store.dispatch('createToast', payloadToast);
                }
              } catch (error) {
                console.error(error);
              }
            } else {
              // Thêm mới nhân viên sau đó thông báo bằng toast message
              try {
                let res = await PeriodSalaryAPI.createNewData(this.currentPeriodSalary);
                // Kiểm tra mã code tương tự sửa
                if (res.data.MISACode == MISACODE.NotValid) {
                  let arrError = res.data.Message;
                  arrError.forEach(async (element) => {
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = element.usrMsg
                    let type = "error"
                    let payloadToast = {
                      content,
                      type,
                      index
                    }
                    this.$store.dispatch('createToast', payloadToast)
                  });
                } else if (res.data.MISACode == MISACODE.Success) {
                  if (this.checkChangeSubTable()) {
                    await this.updateSubInformation();
                  }
                  let index = await this.$store.dispatch('randomText', 4)
                  let type = "success";
                  let content = this.toastMessageSalaryResource['addSuccess']
                  let payloadToast = {
                    content,
                    type,
                    index
                  }
                  this.$store.dispatch('createToast', payloadToast)
                  if (!isMore) {
                    this.$emit('hideForm');
                    this.$emit('refreshData');
                  } else {
                    this.statusMode = STATUSMODE.ADD;
                    await this.init();
                    await this.getNewEmployeeCode();
                  }

                } else {
                  let index = await this.$store.dispatch('randomText', 4)
                  let type = "error";
                  let content = this.toastMessageSalaryResource['addFail']
                  let payloadToast = {
                    content,
                    type,
                    index
                  }
                  this.$store.dispatch('createToast', payloadToast)
                }
              } catch (error) {
                console.log(error);
              }
            }
          } else {
            this.focusErrorField(errorContents);
          }
        }
        this.isShowSpinner = false;
      },
      /**
       * Sau khi tắt popup thông báo lỗi sẽ focus vào trường lỗi đầu tiên để
       * người dùng tiếp tục nhập lại
       * Created By TBN (26/7/2021)
       */
      focusErrorField(errorContents) {
        let firstTime = true // Flag để chọn trường đầu tiên
        let dateInput = ['DateOfBirth', 'IdentityDate', 'JoinDate'];
        let inputTableFieldList = ["CoefficientsSalary"];
        for (let i = 0; i < errorContents.length; i++) {
          if (errorContents[i].ignored) continue;
          // Kiểm tra các component
          if (dateInput.includes(errorContents[i].key)) {
            // DatePick component
            if (firstTime) {
              this.$refs[errorContents[i].key].$el.querySelector('input').focus(); // Focus input
              firstTime = false;
            }
            this.$refs[errorContents[i].key].isError = true;
            this.$refs[errorContents[i].key].isDisplayToolTip = true;
          } else if (inputTableFieldList.includes(errorContents[i].key)) {
            if (firstTime) {
              this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0].$el.querySelector('input').blur();
              setTimeout(() => {
                this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0].$el.querySelector('input').focus();
                this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0].isError = true;
                this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0].isDisplayToolTip = true;
              }, 50)

              firstTime = false;
            }

          } else if (this.$refs[errorContents[i].key].$refs.inputCombobox != null) {
            // Combobox Component
            if (firstTime) {
              this.$refs[errorContents[i].key].$refs.inputCombobox.$refs.input.focus(); // Focus input
              firstTime = false;
            }
            this.$refs[errorContents[i].key].$refs.inputCombobox.isError = true; // hiện lỗi trường dữ liệu
            this.$refs[errorContents[i].key].$refs.inputCombobox.isDisplayToolTip = true; // hiển thị tooltip
            this.$refs[errorContents[i].key].$refs.inputCombobox.isChange = true;
            true; // trạng thái thay đổi dữ liệu = true
          } else {
            // Input Component
            if (firstTime) {
              this.$refs[errorContents[i].key].$refs.input.focus(); // Focus input
              firstTime = false;
            }
            if (this.$refs[errorContents[i].key].validType != "none") {
              this.$refs[errorContents[i].key].isChange = true; // trạng thái thay đổi dữ liệu = true
              this.$refs[errorContents[i].key].isError = true; // hiện lỗi trường dữ liệu
              this.$refs[errorContents[i].key].isDisplayToolTip = true; // hiển thị tooltip
            }
          }
        }
      },
      /**
       * Binding dữ liệu từ input vào current
       * Created By TBN (5/8/2021)
       */
      bindingDataInput(field, value) {
        this.currentPeriodSalary[field] = value;
      },
      /**
       * Tìm kiếm nội dung có trong bảng danh sách nhân viên
       * Created By TBN (22/9/2021)
       */
      searchData() {
        let inputContent = this.$refs["searchchingInput"].inputContent;
        let data = this.$refs["formSalaryTable"].keepingDataTable,
          listToSearch = ["EmployeeCode", "EmployeeName"],
          filteredData = [];
        // Kiểm tra xem trong mảng có chưa kí tự có dáu không
        let isVietNameseWord = false;
        if (Common.checkStringHasVietNameseCharacters(inputContent)) {
          isVietNameseWord = true;
        }
        data.forEach(element => {
          let check = false;
          for (let [key, value] of Object.entries(element)) {
            if (listToSearch.includes(key)) {
              // Nếu dữ liệu không dấu thì sẽ tìm kiếm không quan tâm về dấu
              // Còn nếu tồn tại dấu thì cần tìm kiếm đúng giá trị
              if (!isVietNameseWord) {
                value = Format.changeVietNameseWordToNormal(value);
                inputContent = Format.changeVietNameseWordToNormal(inputContent);
              }
              if (value.search(inputContent) != -1) {
                check = true;
                break;
              }
            }

          }
          if (check) {
            filteredData.push(element);
          }
        })

        this.$refs["formSalaryTable"].dataTable = filteredData;
        let newList = [];
        for (let i = 0; i < filteredData.length; i++) {
          newList.push(false);
        }
        this.$refs["formSalaryTable"].checkedList = [...newList];
        for (const [key] of Object.entries(this.$refs.formSalaryTable.$refs)) {
          if (key == "all")
            continue
          if (this.$refs.formSalaryTable.$refs[key][0]) {
            if (this.$refs.formSalaryTable.$refs[key][0].checked) {
              this.$refs.formSalaryTable.$refs[key][0].click();
            }
          }
        }
      },
      /**
       * Hiện popup close form
       * Created By TBN (26/7/2021)
       */
      async displayWarning() {
        let check = await this.checkUnchangeState();
        if (check) {
          this.$emit('hideForm');
        } else {
          // Thiết lập thông tin cho popup
          let objectPopUp = {};
          objectPopUp.contentPopUp = this.popupContentSalary['dataChange'];
          objectPopUp.type = "confirmation"
          objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

          this.updateObjectPopUp(objectPopUp);
          this.isShowPopUp = true;
          this.$nextTick(() => {
            this.$refs.popUp.isShow = true;
          })
          this.$emit('displayLayout')
        }
      },
      /**
       * Hàm xóa nhưng thuộc tính null hoặc rỗng để thực hiện so sánh JSON
       * Created By TBN (26/9/2021)
       */
      async checkAttributeObject() {
        for (const [key, value] of Object.entries(this.currentPeriodSalary)) {
          if (!value || value == "") {
            delete this.currentPeriodSalary[key];
          }
        }
        for (const [key, value] of Object.entries(this.keepingPeriodSalary)) {
          if (!value || value == "") {
            delete this.keepingPeriodSalary[key];
          }
        }
      },
      /**
       * So sánh xem có sự thay đổi về dữ liệu hay không
       * Created By TBN (10/8/2021)
       */
      async checkUnchangeState() {
        await this.checkAttributeObject();
        if (JSON.stringify(this.keepingPeriodSalary) === JSON.stringify(this.currentPeriodSalary))
          return true;
        else return false;
      },
      /**
       * Áp dữ liệu từ bảng vào form
       * Created By TBN (25/7/2021)
       */
      bindingData(data) {
        this.$nextTick(() => {
          // Bind dữ liệu vào 2 object để kiểm tra sự thay đổi
          this.keepingPeriodSalary = {
            ...data
          }
          this.currentPeriodSalary = {
            ...data
          }

          for (const [key, value] of Object.entries(this.currentPeriodSalary)) {
            if (this.$refs[key] != null) {
              switch (key) {
                default:
                  this.$refs[key].init(value);
                  break;
              }
            }
          }
        })
      },
      ...mapActions(['updateObjectPopUp']),
    },
    computed: {
      tooltipContent() {
        return this.$store.state.resource[this.$browserLocale]['common'][
          'tooltipContent'
        ];
      },
      resource() {
        return this.$store.state.resource;
      },
      formFieldTitle() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'formFieldTitle'
        ];
      },
      placeHolder() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'placeHolder'
        ];
      },
      footerFormButtonContent() {
        return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
          'content'
        ]['footerFormButton'];
      },
      popupContentSalary() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'popupContent'
        ];
      },
      popupContentEmployee() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
          'popupContent'
        ];
      },
      footerPopUpButtonContent() {
        return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
          'content'
        ]['footerPopUpButton'];
      },
      toastMessageSalaryResource() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary'][
          'toastMessage'
        ]
      },
      toastMessageEmployeeResource() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
          'toastMessage'
        ]
      },
      summarizeFieldName() {
        return this.$store.state.resource[this.$browserLocale]['management']['entity']['Salary']['summarizeField']
      },
    },
    components: {
      BaseInput,
      BaseCombobox,
      BaseTable,
      BaseButton,
      BasePopup,
      BaseViewTable
    }
  }
</script>

<style>

</style>