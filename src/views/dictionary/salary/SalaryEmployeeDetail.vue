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
            <div class="half-w pr15">
              <div class="input-information full-w">
                <div class="name-information">
                  {{formFieldTitle['PeriodName']}} <span style="color: red;">*</span>
                </div>
                <BaseInput :tabindex="1" :maxlength="255" ref="PeriodName" classList="user-input required" type="text"
                  refType="employeeCode" name="PeriodName" validType="required"
                  @bindingDataInput="bindingDataInput('PeriodName',$refs['PeriodName'].inputContent)" />
              </div>
            </div>
            <div class="half-w">
              <div class="display-flex">
                <div class="input-information half-w">
                  <div class="name-information">
                    {{formFieldTitle['Month']}} <span style="color: red;">*</span>
                  </div>
                  <BaseCombobox :tabindex="2" ref="Month" class="cbo select-container full-w" dataType="month"
                    name="Month" :maxlength="255" validType="required" :isDisabledInput="true" :noneCheck="true"
                    @bindingDataInput="bindingDataInput('Month',$refs['Month'].inputContent)" />
                </div>
                <div class="input-information half-w mr0">
                  <div class="name-information">
                    {{formFieldTitle['Year']}} <span style="color: red;">*</span>
                  </div>
                  <BaseCombobox :tabindex="3" ref="Year" class="cbo select-container full-w" dataType="year" name="Year"
                    :maxlength="255" validType="required"
                    @bindingDataInput="bindingDataInput('Year',$refs['Year'].inputContent)" />
                </div>
              </div>
            </div>
          </div>


          <div class="input-information full-w mr0">
            <div class="name-information">
              {{formFieldTitle['Note']}}
            </div>
            <BaseInput :tabindex="4" ref="Note" classList="user-input required " type="text" name="Note"
              :maxlength="255" @bindingDataInput="bindingDataInput('Note',$refs['Note'].inputContent)" />
          </div>
        </div>

        <div class="bottom-part">
          <div class="grid-table">
            <div class="grid-table-header">
              <div class="grid-table-header-left">
                <div class="grid-table-header-title">
                  {{resource[this.$browserLocale]['management']['entity']['Salary']['titleSubTable']}}</div>
                <BaseInput :contentPlHolder="placeHolder['searchBarPeriodEmployee']"
                  classList="icon-search input-search " type="text" errorContent="" refType="" validType="search"
                  ref="searchchingInput" @enterPress="searchData" @refreshData="searchData"
                  :tooltipContent="tooltipContent['searchBar']" :noneCheck="true" />
              </div>
              <div class="btn-container">
                <div class="add-icon" v-tooltip="tooltipContent['addData']" @click="isShowViewTable=true">

                </div>
                <div class="delete-icon" v-tooltip="tooltipContent['deleteData']" @click="deleteData">
                </div>
              </div>
            </div>
            <BaseTable :classListGridContainer="'grid-form'" :ths="ths" ref="formSalaryTable" type="employeeSalaryForm"
              @displayConfirmationPopup="deleteData" :displayPaging="false" :keyAttribute="'EmployeeId'" />
          </div>

        </div>
        <div class="footer">
          <div class="footer-left">

          </div>
          <div class="footer-right">
            <BaseButton :tabindex="9999" @click="$emit('hideForm')" classList="cancel m-second-btn"
              :btnText="footerFormButtonContent['cancel']" class="px-3"
              :tooltipContent="tooltipContent['footerFormButton']['cancel']" />
            <BaseButton :tabindex="10000" :btnText="footerFormButtonContent['save']" classList="m-primary-btn"
              :tooltipContent="tooltipContent['footerFormButton']['save']" @click="storeInformation(false)" />
          </div>
        </div>
      </div>
    </div>
    <BasePopup ref="popUp" @hideForm="$emit('hideForm')" @focusErrorField="focusErrorField" v-if="isShowPopUp"
      @hideLayout="isShowPopUp=false" @deleteData="deleteData" />
    <BaseViewTable :ths="thsViewTable" :title="'Chọn nhân viên'" :api="api" @closeViewTable="isShowViewTable=false"
      v-if="isShowViewTable" :placeHolder="placeHolder['searchBarEmployeeView']" @addChosenData="addChosenData"
      :keyAttribute="'EmployeeId'" />
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
  import Validation from '../../../store/validate'
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
      let monthArray = Common.getRangeMonth(this.resource[this.$browserLocale]['common']['text']['month']);

      let currentYear = new Date().getFullYear(),
        yearArray = Common.getRangeYear(currentYear + 2);

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
      //let tabIndexField = this.$refs["salaryContent"].querySelectorAll('input.user-input');
      this.$refs.PeriodName.$refs.input.addEventListener('keydown', (e) => {
        if ((e.which === 9 && e.shiftKey)) {
          e.preventDefault();
          lastIndex.focus()
        }
      })

      
      // Đặt giá trị ban đầu cho combobox ngày, tháng là tháng, năm hiện tại
      let currentDate = new Date();
      if (this.statusMode == STATUSMODE.ADD) {
        this.$refs.Month.setInputContent(this.resource[this.$browserLocale]['common']['text']['month'] + " " + (
          currentDate.getMonth() + 1));
        this.$refs.Year.setInputContent(currentDate.getFullYear());
        this.keepingPeriodSalary["Month"] = this.resource[this.$browserLocale]['common']['text']['month'] + " " + (
          currentDate.getMonth() + 1);
        this.keepingPeriodSalary["Year"] = currentDate.getFullYear();
      }
      setTimeout(() => {
        // Đặt sự kiện tab cho input trong bảng
        let inputs = this.$refs["formSalaryTable"].$el.querySelectorAll("input:not([type=checkbox])");
        let tabindex = this.tabindex;
        for (let i = 0; i < inputs.length; i++) {
          if (inputs[i].tabIndex == 0)
            inputs[i].tabIndex = tabindex + i;
        }
      }, 500)
      setTimeout(()=>{
        // Focus vào input đầu tiền
         this.$refs.PeriodName.$refs.input.focus();
      },200)
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
        tabindex: 5
      }
    },
    methods: {
      /**
       * Hàm xóa dữ liệu của bảng
       * Created by TBN(25/9/2021)
       */
      deleteData() {
        if (this.$refs["formSalaryTable"].selectedList.length > 0) {
          let arrayDeleted = this.$refs["formSalaryTable"].selectedList.map(element => element.EmployeeId);
          let dataTable = this.$refs["formSalaryTable"].dataTable;
          // Lọc lại nhân viên sau khi xóa
          this.$refs["formSalaryTable"].dataTable = [...dataTable.filter(element => !arrayDeleted.includes(element
            .EmployeeId))];
          this.$refs["formSalaryTable"].keepingDataTable = this.$refs["formSalaryTable"].dataTable;
          this.$refs["formSalaryTable"].selectedList = []; // Đưa danh sách được chọn về rỗng
        } else {
          let objectPopUp = {};
          objectPopUp.contentPopUp = this.popupContentSalary['emptyDataTable'];
          objectPopUp.type = "warning-code"
          objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

          this.updateObjectPopUp(objectPopUp);
          this.isShowPopUp = true;

          this.$nextTick(() => {
            this.$refs.popUp.isShow = true;
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

        let newDataTable = [...dataTable, ...newData];
        let initialDataCount = dataTable.length;
        // Cập nhật dữ liệu cho bảng
        this.$refs["formSalaryTable"].dataTable = newDataTable;
        this.$refs["formSalaryTable"].keepingDataTable = newDataTable;
        this.$nextTick(() => {
          if (initialDataCount == 0) {
            // Nếu danh sách trống thì focus vào bản ghi đầu tiên
            this.$refs["formSalaryTable"].selectedList.push(newDataTable[0]);
            this.$refs["formSalaryTable"].$refs["row0"][0].focus();
          } else {
            // Nếu danh sách được chọn đã có thì sẽ focus vào phần tử được chọn cuối cùng
            this.$nextTick(() => {
              let lastElement = this.$refs["formSalaryTable"].selectedList.at(-1);
              let lastIndex = newDataTable.indexOf(lastElement);
              if (lastIndex != -1)
                this.$refs["formSalaryTable"].$refs["row" + lastIndex][0].focus();
              else this.$refs["formSalaryTable"].$refs["row0"][0].focus();
            })
          }
          // Đặt tabindex cho index trong bảng
          this.$nextTick(() => {
            let inputs = this.$refs["formSalaryTable"].$el.querySelectorAll("input:not([type=checkbox])");
            let tabindex = this.tabindex;
            for (let i = 0; i < inputs.length; i++) {
                inputs[i].tabIndex = tabindex + i;
            }
          })

        })

      },
      /**
       * Hàm validate input trong bảng
       * Created By TBN (1/10/2021)
       */
      validateInputInTable(validType, convertText, content) {
        if (validType.search("required") != -1) {
          if (content == "0" || content == "") {
            return convertText["empty"];
          }
        }
        if (validType.search("number") != -1 || validType.search("float") != -1) {
          if (!Validation.checkValidNumber(content.toString().replaceAll('.', ''))) {
            return convertText["invalid"];
          }
        }
        return "";
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
          } else {
            // Các trường dữ liệu khác
            let inputCombobox = this.$refs[key].$refs.inputCombobox;
            if (inputCombobox != null) {
              inputCombobox.$refs.input.blur();
              if (inputCombobox.isError) {
                let content = convertText[key]['invalid'];
                errorContents.push({
                  key,
                  content
                })
              }
            } else {
              if (this.$refs[key].$refs.input) {
                this.$refs[key].validateContent();
                if (this.$refs[key].isError) {
                  let content = convertText[key]['invalid'];
                  errorContents.push({
                    key,
                    content
                  })
                }
              }
            }
          }
        }

        // Custom Validate : Phần này sẽ tùy thuộc vào component hiện tại đang làm gì
        // Kiểm tra bảng có dữ liệu trong bảng hay không
        if (this.$refs["formSalaryTable"].keepingDataTable.length == 0) {
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
        // Kiểm tra hợp lệ trong bảng
        let firstTime = true;
        let listFieldToCheck = ["CoefficientsSalary", "Subsidy", "SocialInsuranceCost"];
        for (let i = 0; i < this.$refs["formSalaryTable"].keepingDataTable.length; i++) {
          for (let j = 0; j < listFieldToCheck.length; j++) {
            let inputContent = this.$refs["formSalaryTable"].keepingDataTable[i][listFieldToCheck[j]];
            let th = this.ths.filter(element => element.fieldName == listFieldToCheck[j])[0];
            let checkContent = this.validateInputInTable(th.validType, convertText[listFieldToCheck[j]],
              inputContent);
            if (checkContent != "") {
              // Khi có dữ liệu trong bảng bị lỗi phải đưa bảng về trạng thái ban đầu để kiểm tra
              if(firstTime){
                this.$refs["searchchingInput"].inputContent = "";
                this.searchData();
                firstTime = false;
              }
              errorContents.push({
                key: listFieldToCheck[j],
                ref: listFieldToCheck[j] + i,
                content: checkContent,
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
        let arrayNewEmployeeId = this.$refs["formSalaryTable"].keepingDataTable.map(element => element.EmployeeId);
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
        let arrayOldChangedEmployee = this.$refs["formSalaryTable"].keepingDataTable.filter(element =>
          arrayOldEmployeeId
          .includes(element.EmployeeId));
        // Danh sách nhân viên được thêm mới
        this.arrayAddedEmployee = this.$refs["formSalaryTable"].keepingDataTable.filter(element => arrayAddedEmployeeId
          .includes(element.EmployeeId))
        // Kiểm tra sự thay đổi dữ liệu của nhân viên cũ
        this.arrayChangedSalaryPeriodEmployee = [];

        arrayOldEmployee.forEach(element => {
          let checkedEmployee = arrayOldChangedEmployee.filter(e => e.EmployeeId == element.EmployeeId)[0];
          checkedEmployee["PeriodSalaryEmployeeId"] = element["PeriodSalaryEmployeeId"];
          if (JSON.stringify(checkedEmployee) != JSON.stringify(element)) {
            // Nếu có sự thay đổi thì thêm mới vào  mảng nhân viên thay đổi
            this.arrayChangedSalaryPeriodEmployee.push(checkedEmployee);
          }
        })
        // Mảng mã định danh nhân viên bị xóa
        this.arrayDeleteSalaryPeriodEmployeeId = keepingDataSubTable.filter(element => arrayDeleteEmployeeId
          .includes(element.EmployeeId));

        // Kiểm tra xem có sự thay đổi của dữ liệu trong bảng
        if (this.arrayDeleteSalaryPeriodEmployeeId.length != 0 || this.arrayAddedEmployee.length != 0 || this
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
          await PeriodSalaryEmployeeAPI.deleteData(element.PeriodSalaryEmployeeId);
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
      async prepareListPeriodSalaryEmployee() {
        let arrayObject = {
          EmployeeNewList: "",
          EmployeeDeleteList: "",
          EmployeeUpdateList: "",
        };
        // Xóa dữ liệu nhân viên
        let string = "";
        this.arrayDeleteSalaryPeriodEmployeeId.forEach(async function (element) {
          if (string == "") {
            string += JSON.stringify(element);
          } else {
            string += "," + JSON.stringify(element);
          }
        })
        if (string != "")
          arrayObject["EmployeeDeleteList"] = "[" + string + "]";
        else arrayObject["EmployeeDeleteList"] = null;
        string = "";
        // Thêm mới dữ liệu nhân viên
        this.arrayAddedEmployee.forEach(async function (element) {
          if (string == "") {
            string += JSON.stringify(element);
          } else {
            string += "," + JSON.stringify(element);
          }
        })
        if (string != "")
          arrayObject["EmployeeNewList"] = "[" + string + "]";
        else arrayObject["EmployeeNewList"] = null;
        string = "";
        // Sửa thông tin nhân viên cũ
        this.arrayChangedSalaryPeriodEmployee.forEach(async function (element) {
          if (string == "") {
            string += JSON.stringify(element);
          } else {
            string += "," + JSON.stringify(element);
          }
        })
        if (string != "")
          arrayObject["EmployeeUpdateList"] = "[" + string + "]";
        else arrayObject["EmployeeUpdateList"] = null;
        return arrayObject;
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
                setTimeout(() => {
                  this.$emit('hideForm');
                  this.$emit('refreshData', true);
                }, 20)
              } catch (e) {
                console.log(e);
              }
            } else {
              this.focusErrorField(errorContents,true);
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
                let object = null;
                if (this.checkChangeSubTable()) {
                  object = await this.prepareListPeriodSalaryEmployee();
                  this.currentPeriodSalary["EmployeeDeleteList"] = object["EmployeeDeleteList"];
                  this.currentPeriodSalary["EmployeeNewList"] = object["EmployeeNewList"];
                  this.currentPeriodSalary["EmployeeUpdateList"] = object["EmployeeUpdateList"];
                }
                let res = await PeriodSalaryAPI.updateData(this.currentPeriodSalary.PeriodSalaryId, this
                  .currentPeriodSalary);
                if (res.data.MISACode == MISACODE.NotValid) {
                  let convertText = this.resource[this.$browserLocale]['common'][
                    'dictionaryError'
                  ];
                  // Validate lỗi nhưng mà đề bài này k cần đến
                  let arrError = res.data.Message;
                  let errorContents = [];
                  arrError.forEach(async (element) => {
                    errorContents.push({
                      content: convertText[element.fieldName],
                      key: element.fieldName,
                      type: element.type
                    })
                  });
                  this.focusErrorField(errorContents,true);

                } else if (res.data.MISACode == MISACODE.Success) {
                  // Cập nhật dữ liệu của bảng nếu có thay đổi xảy ra
                  // if (this.checkChangeSubTable()) {
                  //   await this.updateSubInformation();
                  // }
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
                    setTimeout(() => {
                      this.$emit('hideForm');
                      this.$emit('refreshData', true);
                    }, 0)
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
                let object = null;
                if (this.checkChangeSubTable()) {
                  object = await this.prepareListPeriodSalaryEmployee();
                  this.currentPeriodSalary["EmployeeDeleteList"] = object["EmployeeDeleteList"];
                  this.currentPeriodSalary["EmployeeNewList"] = object["EmployeeNewList"];
                  this.currentPeriodSalary["EmployeeUpdateList"] = object["EmployeeUpdateList"];
                }

                let res = await PeriodSalaryAPI.createNewData(this.currentPeriodSalary);
                // Kiểm tra mã code tương tự sửa
                if (res.data.MISACode == MISACODE.NotValid) {
                  let convertText = this.resource[this.$browserLocale]['common'][
                    'dictionaryError'
                  ];
                  // Validate lỗi nhưng mà đề bài này k cần đến
                  let arrError = res.data.Message;
                  let errorContents = [];
                  arrError.forEach(async (element) => {
                    errorContents.push({
                      content: convertText[element.fieldName],
                      key: element.fieldName,
                      type: element.type
                    })
                  });
                  this.focusErrorField(errorContents,true);
                } else if (res.data.MISACode == MISACODE.Success) {
                  // if (this.checkChangeSubTable()) {
                  //   await this.updateSubInformation();
                  // }
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
                    setTimeout(() => {
                      this.$emit('hideForm');
                      this.$emit('refreshData', true);
                    }, 0)
                  } else {
                    this.statusMode = STATUSMODE.ADD;
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
            this.focusErrorField(errorContents,true);
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
        let inputTableFieldList = ["CoefficientsSalary", "Subsidy", "SocialInsuranceCost"];

        let input = null;
        for (let i = 0; i < errorContents.length; i++) {
          if (errorContents[i].ignored) continue;

          let contentError = "";
          if (errorContents[i].type != null) {
            contentError = errorContents[i].content[errorContents[i].type];
          } else {
            contentError = errorContents[i].content;
          }

          // Kiểm tra các component
          if (dateInput.includes(errorContents[i].key)) {
            input = this.$refs[errorContents[i].key].$el.querySelector('input');
            // DatePick component
            if (firstTime) {
              input.focus(); // Focus input
              firstTime = false;
            }
            let datePick = this.$refs[errorContents[i].key];
            datePick.isError = true;
            datePick.isDisplayToolTip = true;
          } else if (inputTableFieldList.includes(errorContents[i].key)) {
            
            // input trong bảng
            input = this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0].$el.querySelector('input');
            let inputComponent = this.$refs["formSalaryTable"].$refs[errorContents[i].ref][0];
            if (firstTime) {
              input.blur();
              inputComponent.isDisplayToolTip = true;
              input.focus();
              firstTime = false;
            }
            // Thiết lập lỗi cho input trong form
            setTimeout(() => {
              inputComponent.isError = true;
              inputComponent.errorContentData = contentError;
              inputComponent.lockError = true;
            }, 50)
          } else if (this.$refs[errorContents[i].key].$refs.inputCombobox != null) {
            // Combobox Component
            input = this.$refs[errorContents[i].key].$refs.inputCombobox.$refs.input;
            let inputComponent = this.$refs[errorContents[i].key].$refs.inputCombobox;
            if (firstTime) {
              input.focus(); // Focus input
              firstTime = false;
            }
            inputComponent.isError = true; // hiện lỗi trường dữ liệu
            inputComponent.isDisplayToolTip = true; // hiển thị tooltip
            inputComponent.tooltipContentData = contentError;
            inputComponent.isChange = true;
            true; // trạng thái thay đổi dữ liệu = true
          } else {
            // Input Component
            input = this.$refs[errorContents[i].key].$refs.input;
            let inputComponent = this.$refs[errorContents[i].key];
            if (firstTime) {
              input.focus(); // Focus input
              firstTime = false;
            }
            inputComponent.isError = true; // hiện lỗi trường dữ liệu
            inputComponent.isDisplayToolTip = true; // hiển thị tooltip

            if (inputComponent.noneCheck) {
              inputComponent.tooltipContentData = contentError
            } else {
              inputComponent.errorContentData = contentError
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
      },
      /**
       * Hiện popup close form
       * Created By TBN (26/7/2021)
       */
      async displayWarning() {
        let check = await this.checkUnchangeState();
        if (check && !this.checkChangeSubTable()) {
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