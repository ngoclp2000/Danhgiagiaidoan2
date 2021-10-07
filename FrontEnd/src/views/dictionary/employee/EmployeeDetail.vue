<template>
    <div class="m-dialog center-div " @keydown.ctrl.shift.83.exact.prevent.stop="storeInformation(true)"
        @keydown.ctrl.83.exact.prevent.stop="storeInformation(false)" @keydown.esc.prevent.stop="displayWarning">
        <div class="layout-blur employee-detail-layout center-div">
        </div>
        <div class="employee-detail center-div" :class="{'expand-content':(isCustomer || isSuppiler)}">
            <div class="sticky">
                <div class="header-form-left">
                    <div class="title">{{title}}</div>

                </div>
                <div class="m-btn-list">
                    <div @click="displayWarning()" class="m-btn-close" v-tooltip="tooltipContent['closeButton']">

                    </div>
                </div>
            </div>
            <div class="employee-detail-content" ref="employeeContent">
                <div class="upper-part">
                    <div class="general-information">
                        <div class="half-w pr15">
                            <div class="display-flex">
                                <div class="input-information w2p5">
                                    <div class="name-information">
                                        {{formFieldTitle['EmployeeCode']}} <span style="color: red;">*</span>
                                    </div>
                                    <BaseInput :tabindex="1" :maxlength="20" ref="EmployeeCode"
                                        classList="user-input required" type="text" refType="employeeCode"
                                        name="EmployeeCode" validType="required"
                                        @bindingDataInput="bindingDataInput('EmployeeCode',$refs['EmployeeCode'].inputContent)" />
                                </div>
                                <div class="input-information w3p5 mr0">
                                    <div class="name-information">
                                        {{formFieldTitle['EmployeeName']}} <span style="color: red;">*</span>
                                    </div>
                                    <BaseInput :tabindex="2" ref="EmployeeName" classList="user-input required"
                                        type="text" validType="required" :maxlength="100" name="EmployeeName"
                                        formatType="name"
                                        @bindingDataInput="bindingDataInput('EmployeeName',$refs['EmployeeName'].inputContent)" />
                                </div>
                            </div>
                            <div class="input-information full-w">
                                <div class="name-information">
                                    {{formFieldTitle['DepartmentName']}} <span style="color: red;">*</span>
                                </div>
                                <BaseCombobox :tabindex="3" :dataCombobox="$store.state.data.department"
                                    ref="DepartmentName" name="DepartmentName" class="cbo select-container full-w"
                                    dataType="department" validType="required" :maxlength="255"
                                    @bindingDataInput="bindingDataInput('DepartmentName',$refs['DepartmentName'].inputContent)" />
                            </div>
                            <div class="input-information full-w">
                                <div class="name-information">
                                    {{formFieldTitle['EmployeePosition']}}
                                </div>
                                <BaseInput :tabindex="4" ref="EmployeePosition" classList="user-input required full-w"
                                    type="text" name="EmployeePosition" :maxlength="255"
                                    @bindingDataInput="bindingDataInput('EmployeePosition',$refs['EmployeePosition'].inputContent)" />
                            </div>
                            <div class="input-information full-w" v-if="isCustomer || isSuppiler">
                                <div class="name-information">
                                    {{formFieldTitle['GroupName']}}
                                </div>
                                <BaseCombobox :tabindex="12" ref="GroupName" class="cbo select-container full-w"
                                    dataType="position" name="GroupName" :maxlength="255"
                                    @bindingDataInput="bindingDataInput('GroupName',$refs['GroupName'].inputContent)" />
                            </div>
                        </div>
                        <div class="half-w">
                            <div class="display-flex">
                                <div class="input-information w4p5">
                                    <div class="name-information">
                                        {{formFieldTitle['DateOfBirth']}}
                                    </div>
                                    <BaseDatePick :tabindex="5" ref="DateOfBirth" name="DateOfBirth" :maxlength="10"
                                        @bindingDataInput="bindingDataInput('DateOfBirth',$refs['DateOfBirth'].inputContent)" />
                                </div>
                                <div class="input-information w3p5 mr0">
                                    <div class="name-information">
                                        {{formFieldTitle['GenderName']}}
                                    </div>
                                    <div class="radio-container" ref="GenderName">
                                        <label class="container" v-for="(element,index) in gender" :key="index">
                                            {{element.content}}
                                            <input :tabindex="6" @change="changeGenderName($event)" type="radio"
                                                :value="element.content" :checked="element.value == 0" name="radio">
                                            <span class="checkmark"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="display-flex">
                                <div class="input-information w4p5">
                                    <div class="name-information">
                                        {{formFieldTitle['IdentityNumber']}}
                                    </div>
                                    <BaseInput :tabindex="9" :maxlength="20" ref="IdentityNumber"
                                        classList="user-input required" type="text" refType="identity"
                                        validType="identification" name="IdentityNumber"
                                        @bindingDataInput="bindingDataInput('IdentityNumber',$refs['IdentityNumber'].inputContent)" />
                                </div>
                                <div class="input-information w3p5 mr0">
                                    <div class="name-information">
                                        {{formFieldTitle['IdentityDate']}}
                                    </div>
                                    <BaseDatePick :tabindex="10" ref="IdentityDate" name="IdentityDate" :maxlength="10"
                                        @bindingDataInput="bindingDataInput('IdentityDate',$refs['IdentityDate'].inputContent)" />
                                </div>
                            </div>
                            <div class="input-information full-w">
                                <div class="name-information">
                                    {{formFieldTitle['IdentityPlace']}}
                                </div>
                                <BaseInput :tabindex="11" :maxlength="255" ref="IdentityPlace"
                                    classList="user-input required" type="text" name="IdentityPlace"
                                    @bindingDataInput="bindingDataInput('IdentityPlace',$refs['IdentityPlace'].inputContent)" />
                            </div>
                            <div class="input-information w2p5" v-show="isCustomer">
                                <div class="name-information">
                                    {{formFieldTitle['AccountDebtReceive']}}
                                </div>
                                <BaseInput :tabindex="1" :maxlength="20" ref="AccountDebtReceive"
                                    classList="user-input required" type="text" refType="employeeCode"
                                    name="AccountDebtReceive"
                                    @bindingDataInput="bindingDataInput('AccountDebtReceive',$refs['AccountDebtReceive'].inputContent)" />
                            </div>
                            <div class="input-information w2p5 mr0" v-show="isSuppiler">
                                <div class="name-information">
                                    {{formFieldTitle['AccountDebtPay']}}
                                </div>
                                <BaseInput :tabindex="1" :maxlength="14" ref="AccountDebtPay"
                                    classList="user-input required" type="text" refType="employeeCode"
                                    name="AccountDebtPay"
                                    @bindingDataInput="bindingDataInput('AccountDebtPay',$refs['AccountDebtPay'].inputContent)" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="bottom-part">
                    <div class="input-information full-w mr0">
                        <div class="name-information">
                            {{formFieldTitle['Address']}}
                        </div>
                        <BaseInput :tabindex="15" ref="Address" classList="user-input required " type="text"
                            name="Address" :maxlength="255"
                            @bindingDataInput="bindingDataInput('Address',$refs['Address'].inputContent)" />
                    </div>
                    <div class="display-flex">
                        <div class="input-information w1p3 ">
                            <div class="name-information">
                                {{formFieldTitle['PhoneNumber']}}
                            </div>
                            <BaseInput :tabindex="16" ref="PhoneNumber" classList="user-input  " type="text" refType=""
                                validType="number" name="PhoneNumber" :maxlength="20"
                                @bindingDataInput="bindingDataInput('PhoneNumber',$refs['PhoneNumber'].inputContent)" />
                        </div>
                        <div class="input-information w1p3 ">
                            <div class="name-information">
                                {{formFieldTitle['TelephoneNumber']}}
                            </div>
                            <BaseInput :tabindex="17" ref="TelephoneNumber" classList="user-input  " type="text"
                                refType="" validType="number" name="TelephoneNumber" :maxlength="20"
                                @bindingDataInput="bindingDataInput('TelephoneNumber',$refs['TelephoneNumber'].inputContent)" />
                        </div>
                        <div class="input-information w1p3 mr0">
                            <div class="name-information">
                                {{formFieldTitle['Email']}}
                            </div>
                            <BaseInput :tabindex="18" ref="Email" classList="user-input  " type="text" refType=""
                                validType="email" name="Email" :maxlength="100"
                                @bindingDataInput="bindingDataInput('Email',$refs['Email'].inputContent)" />
                        </div>
                    </div>
                    <div class="display-flex">
                        <div class="input-information w1p3 ">
                            <div class="name-information">
                                {{formFieldTitle['BankAccountNumber']}}
                            </div>
                            <BaseInput :tabindex="19" ref="BankAccountNumber" classList="user-input" type="text"
                                name="BankAccountNumber" :maxlength="20" validType="number"
                                @bindingDataInput="bindingDataInput('BankAccountNumber',$refs['BankAccountNumber'].inputContent)" />
                        </div>
                        <div class="input-information w1p3 ">
                            <div class="name-information">
                                {{formFieldTitle['BankName']}}
                            </div>
                            <BaseInput :tabindex="20" ref="BankName" classList="user-input required " type="text"
                                name="BankName" :maxlength="255"
                                @bindingDataInput="bindingDataInput('BankName',$refs['BankName'].inputContent)" />
                        </div>
                        <div class="input-information w1p3 mr0">
                            <div class="name-information">
                                {{formFieldTitle['BankBranchName']}}
                            </div>
                            <BaseInput :tabindex="21" ref="BankBranchName" classList="user-input required " type="text"
                                name="BankBranchName" :maxlength="255"
                                @bindingDataInput="bindingDataInput('BankBranchName',$refs['BankBranchName'].inputContent)" />
                        </div>
                    </div>
                    <div data-v-294a9023="" class="divide"></div>
                    <div class="footer">
                        <div class="footer-left">

                        </div>
                        <div class="footer-right">
                            <BaseButton :tabindex="22" @click="$emit('hideForm')" classList="cancel m-second-btn"
                                :btnText="footerFormButtonContent['cancel']" class="px-3"
                                :tooltipContent="tooltipContent['footerFormButton']['cancel']" />
                            <BaseButton :tabindex="23" @click="storeInformation(false)"
                                :btnText="footerFormButtonContent['save']" classList="m-primary-btn"
                                :tooltipContent="tooltipContent['footerFormButton']['save']" />
                        </div>
                    </div>
                </div>
            </div>
            <LoadingSpinner v-if="isShowSpinner" />
        </div>
        <BasePopup ref="popUp" @storeInformation="storeInformation(false)" @hideForm="$emit('hideForm')"
            @focusErrorField="focusErrorField" v-if="isShowPopUp" @hideLayout="isShowPopUp=false" />
    </div>

</template>

<script>
    import BaseButton from '../../../components/BaseButton.vue'
    import BaseInput from '../../../components/BaseInput.vue'
    import BaseCombobox from '../../../components/BaseCombobox.vue'
    import BasePopup from '../../../components/BasePopup.vue'
    import EmployeeAPI from '../../../api/components/employeeapi'
    import BaseDatePick from '../../../components/BaseDatePick.vue'
    import LoadingSpinner from '../../../components/LoadingSpinner.vue'
    import {
        MISACODE,
        STATUSMODE
    } from '../../../enum/index'
    import {
        mapActions,
    } from 'vuex'
    export default {
        async created() {
            this.title = this.resource[this.$browserLocale]['management']['entity']['Employee']['formTitle'];
        },
        async mounted() {
            // Gán dữ liệu cho combobox
            await this.getDepartmentData();
            this.$refs.DepartmentName.setData(this.$store.state.data.department.map(a => Object.assign({}, a)),
                false);
            if (this.statusMode != STATUSMODE.ADD)
                this.$refs.DepartmentName.init(this.currentEmployee["DepartmentName"]);

            // Khởi tạo giá trị radio button cho dữ liệu
            let radios = document.querySelectorAll('input[name=radio]');
            if (Array.from(radios).filter(
                    radio => radio.checked).length > 0) {
                this.keepingEmployee["GenderName"] = this.currentEmployee["GenderName"] = Array.from(radios).filter(
                    radio => radio.checked)[0].value;
            }

            // Nếu là nhân bản hoặc thêm mới sẽ lấy mã code mới
            if (this.statusMode != STATUSMODE.EDIT) {
                this.getNewEmployeeCode();
            }

            // Đưa tabindex về đầu tiên khi tabindex cuối cùng
            let tabIndexElement = this.$refs["employeeContent"].querySelectorAll('input, button');
            let lastIndex = tabIndexElement[tabIndexElement.length - 1];
            lastIndex.addEventListener('keydown', (e) => {
                if ((e.which === 9 && !e.shiftKey)) {
                    e.preventDefault();
                    this.$refs.EmployeeCode.$refs.input.focus() // Focus EmployeeCode input
                }
            })
            // Đưa tabindex về cuối khi shift tab tabindex đầu tiên
            //let tabIndexField = this.$refs["employeeContent"].querySelectorAll('input');
            //let lastTeabIndexField = tabIndexField[tabIndexField.length - 1];
            this.$refs.EmployeeCode.$refs.input.addEventListener('keydown', (e) => {
                if ((e.which === 9 && e.shiftKey)) {
                    e.preventDefault();
                    lastIndex.focus() // Focus EmployeeCode input
                }
            })
            setTimeout(() => {
                this.$refs.EmployeeCode.$refs.input.focus() // Focus EmployeeCode input
            },150);
        },
        data: () => {
            return {
                title: "",
                isCustomer: false,
                isSuppiler: false,
                statusMode: STATUSMODE.ADD,
                isShowSpinner: false,
                isShowPopUp: false,
                currentEmployee: {},
                keepingEmployee: {}
            }
        },
        components: {
            BaseButton,
            BaseInput,
            BaseCombobox,
            BasePopup,
            BaseDatePick,
            LoadingSpinner
        },
        watch: {
            isShowPopUp() {
                if (!this.isShowPopUp) {
                    if (this.objectPopUp['type'] != 'error')
                        this.$refs.EmployeeCode.$refs.input.focus() // Focus EmployeeCode input 
                }
            }
        },
        methods: {
            /**
             * Hàm lấy mã code mới
             * Created By TBN (18/8/2021)
             */
            async getNewEmployeeCode() {
                this.isShowSpinner = true; // Hiện spinner
                try {
                    // Gọi API lấy mã code mới
                    let res = await EmployeeAPI.getNewEmployeeCode();
                    // Gán dữ liệu cho ô input code
                    this.$refs.EmployeeCode.init(res.data)
                    // this.keepingEmployee.EmployeeCode = res.data;
                    this.isShowSpinner = false;
                } catch (error) {
                    // Thay đổi nội dung popup sau đó hiển thị
                    let objectPopUp = {};
                    objectPopUp.contentPopUp = this.popupContent['errorGetNewCode'];
                    objectPopUp.type = "warning-code"
                    objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

                    this.updateObjectPopUp(objectPopUp);
                    // Hiện Popup
                    this.isShowPopUp = true;
                    this.$nextTick(() => {
                        this.$refs.popUp.isShow = true;
                    })
                    this.isShowSpinner = false;
                }
                this.$refs.EmployeeCode.$refs.input.focus() // Focus EmployeeCode input
            },
            /**
             * Theo dõi sự thay đổi của radio chọn giới tính
             * Created By TBN (18/8/2021)
             */
            changeGenderName($event) {
                this.currentEmployee["GenderName"] = $event.target.value;
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
                    objectPopUp.contentPopUp = this.popupContent['dataChange'];
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
             * Cập nhập dữ liệu theo trường 
             * Created By TBN (22/8/2021)
             */
            async initByField(key) {
                let dateInput = ['DateOfBirth', 'IdentityDate'];
                if (dateInput.includes(key)) {
                    this.$refs[key].date = "";
                } else if (key == "GenderName") {
                    let radios = document.querySelectorAll('input[name=radio]');
                    radios[0].checked = true;
                    this.keepingEmployee[key] = this.currentEmployee[key] = Array.from(radios).filter(radio => radio
                        .checked)[0].value;
                } else if (key != "EmployeeCode" && key != "GenderName") {
                    if (this.$refs[key].data != undefined)
                        this.$refs[key].resetOptions()
                    this.$refs[key].init("")
                }
            },
            /**
             * Thiết lập lại giá trị ban đầu cho các components thuộc component hiện tại
             * Input, combobox
             * Created By TBN (26/7/2021)
             */
            async init() {
                this.currentEmployee = {};
                this.keepingEmployee = {};
                for (const [key] of Object.entries(this.$refs)) {
                    if (!this.$refs[key].inputContent && !this.$refs[key].date && key != "GenderName")
                        continue;
                    await this.initByField(key);
                    if (key != "GenderName") {
                        delete this.currentEmployee[key];
                    }
                }
            },
            /**
             * Kiểm tra tính đúng đắn của các trường dữ liệu trong cửa sổ nhập dữ liệu
             * Created By TBN (26/7/2021)
             */
            async checkValidity() {
                let errorContents = [] // Nội dung lỗi
                // Ditionary chuyển từ tên trường TA sang TV
                let convertTextEmployee = this.resource[this.$browserLocale]['common'][
                    'dictionaryError'
                ];
                // Loop qua các refs của input và combobox
                for (const [key] of Object.entries(this.$refs)) {
                    if (!this.$refs[key] || (this.$refs[key].inputContent == null && this.$refs[key].date == null))
                        continue;
                    let value = this.$refs[key].inputContent || this.$refs[key].date;
                    //let content = "" // Lưu nội dung của lỗi hiện tại
                    // Nếu lỗi hoặc trông thì sẽ gán lỗi
                    let requiredField = ["EmployeeCode", "EmployeeName", "DepartmentName"]
                    if (requiredField.includes(key) && (value == null || value == "")) {
                        let content = convertTextEmployee[key]['empty'];
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
                        // Kiểm tra trùng lặp
                        if (key == "EmployeeCode") {

                            let content = ""
                            // Nếu có lỗi thì sẽ đẩy vào mảng lỗi
                            try {
                                // Lấy nhân viên theo mã code
                                let res = await EmployeeAPI.getDataByCode(value);
                                if (res.status == MISACODE.NoContent)
                                    continue;
                                else {
                                    // Kiểm tra xem có trùng với mã định danh hiện tại hay không
                                    if (res.data.EmployeeId == this.currentEmployee.EmployeeId && this.statusMode ==
                                        STATUSMODE.EDIT)
                                        continue;
                                    else {
                                        content = convertTextEmployee[key]['duplicate'].format(value);
                                        this.$refs[key].errorContentData = convertTextEmployee[key]['duplicate']
                                            .format(value);
                                    }
                                }
                            } catch (error) {
                                // Thiết lập thông tin cho popup sau đó hiển thị
                                let objectPopUp = {};
                                objectPopUp.contentPopUp = this.popupContent['invalidCode'];
                                objectPopUp.type = "warning-code"
                                objectPopUp.buttonTexts = this.footerPopUpButtonContent[objectPopUp.type];

                                this.updateObjectPopUp(objectPopUp);
                                this.isShowPopUp = true;
                                this.$nextTick(() => {
                                    this.$refs.popUp.isShow = true;
                                })
                                this.isShowSpinner = false;
                            }
                            errorContents.push({
                                key,
                                content,
                            })
                        } else {
                            // Các trường dữ liệu khác
                            let inputCombobox = this.$refs[key].$refs.inputCombobox;
                            if (inputCombobox != null) {
                                inputCombobox.$refs.input.blur();
                                if (inputCombobox.isError) {
                                    let content = convertTextEmployee[key]['invalid'];
                                    errorContents.push({
                                        key,
                                        content
                                    })
                                }
                            } else {
                                if (this.$refs[key].$refs.input) {
                                    this.$refs[key].validateContent();
                                    if (this.$refs[key].isError) {
                                        let content = convertTextEmployee[key]['invalid'];
                                        errorContents.push({
                                            key,
                                            content
                                        })
                                    }
                                }
                            }
                        }
                    }
                }
                return errorContents
            },
            /**
             * Lưu dữ liệu hoặc sửa dữ liệu của nhân viên
             * Created By TBN (26/7/2021)
             */
            async storeInformation(isMore) {
                let check = await this.checkUnchangeState();
                // Kiểm tra trạng thái dữ liệu đã thay đổi hay chưa
                if (check) {
                    let index = await this.$store.dispatch('randomText', 4)
                    let content = this.toastMessageResource['unChangeData'];
                    let type = "warning"
                    let payloadToast = {
                        content,
                        type,
                        index
                    }
                    this.$store.dispatch('createToast', payloadToast)
                } else {
                    this.isShowSpinner = true;
                    this.$emit('displayLayout') // Hiện layout
                    let errorContents = await this.checkValidity(); // Nhận mảng nội dung lỗi 
                    if (errorContents.length == 0) {
                        // Không có lỗi thì format lại dữ liệu rồi gửi request
                        this.currentEmployee["Gender"] = this.genderId(this.currentEmployee["GenderName"])
                        // Department 
                        let department = this.$store.state.data.department.filter(element => element
                            .DepartmentName == this.currentEmployee["DepartmentName"])[0]
                        this.currentEmployee["DepartmentId"] = (department ? department.DepartmentId : null);
                        // Kiểm tra là sửa hay thêm mới
                        if (this.statusMode == STATUSMODE.EDIT) {
                            // Sửa dữ liệu sau đó thông báo bằng toast message
                            try {
                                let res = await EmployeeAPI.updateData(this.currentEmployee.EmployeeId, this
                                    .currentEmployee);
                                if (res.data.MISACode == MISACODE.NotValid) {
                                    // Validate lỗi nhưng mà đề bài này k cần đến
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
                                    this.focusErrorField(errorContents, true);
                                } else if (res.data.MISACode == MISACODE.Success) {
                                    // Thành công
                                    let index = await this.$store.dispatch('randomText', 4)
                                    let type = "success";
                                    let content = this.toastMessageResource['updateSuccess'];
                                    let payloadToast = {
                                        content,
                                        type,
                                        index
                                    }
                                    this.$store.dispatch('createToast', payloadToast)
                                    if (!isMore) {
                                        this.$emit('hideForm')
                                    } else {
                                        this.statusMode = STATUSMODE.ADD;
                                        await this.init();
                                        await this.getNewEmployeeCode();
                                    }
                                    this.$emit('refreshData', false);
                                } else {
                                    // Thất bại
                                    let index = await this.$store.dispatch('randomText', 4)
                                    let type = "error";
                                    let content = this.toastMessageResource['updateFail'];
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
                                let res = await EmployeeAPI.createNewData(this.currentEmployee);
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
                                    this.focusErrorField(errorContents, true);
                                } else if (res.data.MISACode == MISACODE.Success) {
                                    let index = await this.$store.dispatch('randomText', 4)
                                    let type = "success";
                                    let content = this.toastMessageResource['addSuccess']
                                    let payloadToast = {
                                        content,
                                        type,
                                        index
                                    }
                                    this.$store.dispatch('createToast', payloadToast)
                                    if (!isMore) {
                                        this.$emit('hideForm')
                                    } else {
                                        this.statusMode = STATUSMODE.ADD;
                                        await this.init();
                                        await this.getNewEmployeeCode();
                                    }
                                    this.$emit('refreshData')
                                } else {
                                    let index = await this.$store.dispatch('randomText', 4)
                                    let type = "error";
                                    let content = this.toastMessageResource['addFail']
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
            focusErrorField(errorContents, lockError) {
                let firstTime = true // Flag để chọn trường đầu tiên
                let dateInput = ['DateOfBirth', 'IdentityDate', 'JoinDate'];
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
                        // DatePick component
                        if (firstTime) {
                            this.$refs[errorContents[i].key].$el.querySelector('input').focus(); // Focus input
                            firstTime = false;
                        }
                        let dataComponent = this.$refs[errorContents[i].key];
                        dataComponent.isError = true;
                        dataComponent.isDisplayToolTip = true;
                    } else if (this.$refs[errorContents[i].key].$refs.inputCombobox != null) {
                        // Combobox Component
                        if (firstTime) {
                            this.$refs[errorContents[i].key].$refs.inputCombobox.$refs.input.blur();
                            this.$refs[errorContents[i].key].$refs.inputCombobox.$refs.input.focus(); // Focus input
                            firstTime = false;
                        }
                        let inputCombobox = this.$refs[errorContents[i].key].$refs.inputCombobox;
                        inputCombobox.isError = true; // hiện lỗi trường dữ liệu
                        inputCombobox.isDisplayToolTip = true; // hiển thị tooltip
                        inputCombobox.tooltipContentData = contentError;
                        inputCombobox.lockError = lockError;
                    } else {
                        let inputComponent = this.$refs[errorContents[i].key];
                        // Input Component
                        inputComponent.isError = true; // hiện lỗi trường dữ liệu
                        inputComponent.isDisplayToolTip = true; // hiển thị tooltip\
                        inputComponent.lockError = lockError;
                        if (inputComponent.noneCheck) {
                            inputComponent.tooltipContentData = contentError
                        } else {
                            inputComponent.errorContentData = contentError
                        }
                        if (firstTime) {
                            this.$refs[errorContents[i].key].$refs.input.focus(); // Focus input
                            firstTime = false;
                        }
                    }
                }
            },
            /**
             * Binding dữ liệu từ input vào current
             * Created By TBN (5/8/2021)
             */
            bindingDataInput(field, value) {
                this.currentEmployee[field] = value;
            },
            /**
             * Hàm loại bỏ trường null
             * Created By TBN (5/8/2021)
             */
            async checkAttributeObject() {
                for (const [key, value] of Object.entries(this.currentEmployee)) {
                    if (!value || value == "") {
                        delete this.currentEmployee[key];
                    }
                }
                for (const [key, value] of Object.entries(this.keepingEmployee)) {
                    if (!value || value == "") {
                        delete this.keepingEmployee[key];
                    }
                }
            },
            /**
             * So sánh xem có sự thay đổi về dữ liệu hay không
             * Created By TBN (10/8/2021)
             */
            async checkUnchangeState() {
                await this.checkAttributeObject();
                if (JSON.stringify(this.keepingEmployee) == JSON.stringify(this.currentEmployee))
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
                    this.keepingEmployee = {
                        ...data
                    }
                    this.currentEmployee = {
                        ...data
                    }

                    for (const [key, value] of Object.entries(this.currentEmployee)) {
                        if (this.$refs[key] != null) {
                            switch (key) {
                                case "DateOfBirth":
                                case "IdentityDate":
                                    // Các trường ngày, tháng
                                    this.$refs[key].date = value;
                                    this.keepingEmployee[key] = this.$refs[key].date;
                                    break;
                                case "GenderName": {
                                    // Nếu là radio button thì check cái lựa chọn
                                    let radios = document.querySelectorAll("input[name=radio]");
                                    if (value)
                                        Array.from(radios).filter(radio => radio.value == value)[0].checked =
                                        true;
                                    else {
                                        Array.from(radios).forEach(radio => radio.checked = false);
                                    }
                                }
                                break;
                            default:
                                this.$refs[key].init(value);
                                break;
                            }
                        }
                    }
                })
            },
            /**
             * Chuyển đổi tên thành id của nó 
             * Created By TBN (22/8/2021)
             */
            genderId(genderName) {
                if (genderName)
                    return this.gender.filter(e => e.content == genderName)[0].value;
                else return null;
            },
            ...mapActions(['getDepartmentData', 'updateObjectPopUp']),
        },
        computed: {
            toggle() {
                return this.$store.state.toggle
            },
            objectPopUp() {
                return this.$store.state.data.objectPopUp;
            },
            popupContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'popupContent'
                ];
            },
            resource() {
                return this.$store.state.resource;
            },
            formFieldTitle() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'formFieldTitle'
                ];
            },
            footerFormButtonContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ]['footerFormButton'];
            },
            footerPopUpButtonContent() {
                return this.$store.state.resource[this.$browserLocale]['management']['common']['buttons'][
                    'content'
                ]['footerPopUpButton'];
            },
            gender() {
                return this.$store.state.resource[this.$browserLocale]['common']['gender']
            },
            tooltipContent() {
                return this.$store.state.resource[this.$browserLocale]['common'][
                    'tooltipContent'
                ];
            },
            toastMessageResource() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'toastMessage'
                ]
            }
        }
    }
</script>

<style scoped>
    .show {
        display: block !important;
    }

    .fullscreen {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        margin: auto;
    }
</style>