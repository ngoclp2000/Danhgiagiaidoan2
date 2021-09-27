<template>
    <div class="input-container" :class="classInputContainer">
        <!-- <div v-show="isDisplayX" @click="resetInput" class="x-icon"
            :class="[validType == 'money' || type == 'right-text'? 'currency-icon' : '',validType == 'search' ? 'search-icon' : '',iconClass]"></div> -->
        <input :tabindex="tabindex" :maxlength="maxlength" @mouseover="isDisplayToolTip = true" ref="input"
            @mouseleave="isDisplayToolTip=false" :placeholder="contentPlHolder" v-model="inputContent"
            @keydown="evtKeyboardChoosingOption" @input="inputChange" autocomplete="off"
            @click.stop="clickInput($event)" @keyup.enter="$emit('enterPress')" @blur="blurEvent"
            @keydown.tab="$emit('tabPress')" :type="type" :readonly="isDisabled"
            :class="[classList, isError ? 'error' : '', active?'active':'',(isReadyData != null && !isReadyData) ? 'waiting-input' : '']"
            v-tooltip="{
                content: (noneCheck == null ? errorContentData : tooltipContentData),
                delay: {
                    show: 300,
                    hide: 0,
                },
                trigger: 'manual',
                show: (isError && isDisplayToolTip) || (noneCheck != null && isDisplayToolTip),
            }">
        <!-- <div class="tooltip bottom" ref="tooltip" v-show="isError && isDisplayError && (disableToolTip == null)">
            {{errorContentData}}
        </div> -->
        <!-- <div v-show="isError && isDisplayError" class="wrong-input" style="z-index:100">{{errorContentData}}</div> -->
        <div v-show="validType == 'money'" class="currency">(VND)</div>
    </div>
</template>

<script>
    import Validation from '../store/validate'
    import Format from '../store/format'
    export default {
        created() {
            switch (this.validType) {
                case 'required':
                    this.errorContentData = this.errorContent.empty;
                    this.tooltipContentData = this.tooltipContent;
                    break;
            }
            if (this.initialValue) {
                this.inputContent = this.initialValue;
            }
        },
        props: ['maxlength', 'contentPlHolder', 'type', 'classList', 'validType',
            'iconClass', 'tabindex', 'classInputContainer', 'isDisabled', 'name', 'isReadyData', 'isShowDropdown',
            'tooltipContent', 'disableToolTip', 'noneCheck', 'formatType', 'initialValue', 'indexData'
        ],
        data() {
            return {
                isDisplayToolTip: false,
                isError: false,
                isDisplayX: false,
                inputContent: "",
                active: false,
                errorContentData: "",
                tooltipContentData: "",
                isChange: false,
            }
        },
        update() {
        },
        watch: {
            inputContent() {
                if (this.inputContent == "") {
                    setTimeout(() => {
                        this.$emit('refreshData');
                    }, 100)
                }
            },
            initialValue(){
                if(this.initialValue)
                    this.inputContent = this.initialValue;
            }
        },
        methods: {
            /**
             * Sự kiện khi blue ra khỏi input
             */
            blurEvent() {
                this.$emit('blur');
                setTimeout(() => {
                    this.validateContent();
                }, 80)
            },
            /**
             * Sự kiện click vào input
             */
            clickInput(event) {
                if(this.inputContent){
                    event.target.setSelectionRange(0, this.inputContent.toString().length);
                }
                this.$emit('click', event);
            },
            /**
             * Sự kiện kiểm tra sự thay đổi của input, format nếu yêu cầu
             * Created by TBN (22/7/2021)
             */
            inputChange() {
                this.isDisplayToolTip = false;
                this.isChange = true;
                if (this.inputContent.length != 0) {
                    if (this.validType == "required") {
                        this.validateContent();
                    }
                    // Nếu dạng tiền thì format .../,,, tùy theo yêu cầu
                    if (this.formatType == "money")
                        this.inputContent = Format.formatMoney(this.inputContent)
                    else if (this.formatType == "telephoneNumber")
                        // nếu dạng số cố định thì format theo dạng (123) 456-7891
                        this.inputContent = Format.formatTelephoneNumber(this.inputContent);
                    else if (this.formatType == "name") {
                        this.inputContent = Format.formatName(this.inputContent);
                    }
                    this.isDisplayX = true;

                } else {
                    if (this.validType == "required") {
                        this.validateContent();
                    }
                    this.isDisplayX = false;
                }
                // Emit dự kiện lọc option, cập nhật nội dung input, hiện dropdown khi nhập, sự kiện cập nhật dữ liệu đã nhập trên form
                this.$emit('filterOption', this.inputContent);
                this.$emit('updateInput', this.inputContent);
                this.$emit('updateInputSubTable', this.inputContent, this.indexData);
                this.$emit('showDropdown');
                this.$emit('bindingDataInput');
            },
            /**
             * Sự kiện Enter, mũi tên lên xuống và phải là combobox
             * Created by TBN (22/7/2021)
             */
            evtKeyboardChoosingOption(e) {
                var keycode = e.keyCode;
                var valid =
                    (keycode > 47 && keycode < 58) || // number keys
                    keycode == 32 || keycode == 13 ||
                    // spacebar & return key(s) (if you want to allow carriage returns)
                    (keycode > 64 && keycode < 91) || // letter keys
                    (keycode > 95 && keycode < 112) || // numpad keys
                    (keycode > 185 && keycode < 193) || // ;=,-./` (in order)
                    (keycode > 218 && keycode < 223); // [\]' (in order)
                if ((this.validType == 'combobox-required' || this.validType == 'combobox') && [38, 40, 13].includes(e
                        .keyCode))
                    this.$emit('evtKeyboardChoosingOption', e.keyCode, e)
                else if (valid) {
                    if (this.inputContent && this.inputContent.toString().length == this.maxlength) {
                        if (!this.isDisplayToolTip || !this.isError) {
                            this.isDisplayToolTip = true;
                            this.isError = true;
                            let errorContent = this.resource[this.$browserLocale]['common']['dictionaryError'][
                                'MaxLength'
                            ].format(this.maxlength);
                            if (this.noneCheck) {
                                this.tooltipContentData = errorContent;
                            } else {
                                this.errorContentData = errorContent;
                            }
                            setTimeout(() => {
                                this.isDisplayToolTip = false;
                                this.isError = false;
                                this.validateContent();
                            }, 2000)
                        }
                    }
                }
            },
            /**
             * Phương thức kiểm tra tính đúng đắn của dữ liệu trong input theo từng loại.
             * Created by TBN (22/7/2021)
             */
            validateContent() {
                // Kiểm tra nội dung
                if ( this.inputContent && this.inputContent.toString().length > 0) {
                    // Nếu dữ liệu không rỗng

                    // Nếu là loại combobox thì sẽ chuyển dừng phương thức vì combox sẽ kiểm tra tại component BaseCombobox
                    if (this.validType == "combobox")
                        return;
                    switch (this.validType) {
                        // Input bắt buộc nhập
                        case "required":
                            this.isError = false;
                            break;
                        case 'combobox-required':
                            if (this.isError) {
                                return;
                            }
                            break;
                            // Input CMND/ Căn cước
                        case "identification":
                            if (Validation.checkIdNumber(this.inputContent))
                                this.isError = false;
                            else {
                                this.errorContentData = this.errorContent.invalid;
                                this.isError = true;
                            }
                            break;
                            // Input email
                        case "email":
                            if (Validation.validateEmail(this.inputContent))
                                this.isError = false;
                            else {
                                this.errorContentData = this.errorContent.invalid;
                                this.isError = true;
                            }
                            break;
                            // Input tiền hoặc số 
                        case "number":
                        case "phoneNumber":
                            if (Validation.checkValidNumber(Format.changeToNumber(this.inputContent))) {
                                this.isError = false;
                            } else {
                                this.errorContentData = this.errorContent.invalid;
                                this.isError = true;
                            }
                            break;
                        case "telephoneNumber":
                            if (Validation.checkTelephoneNumber(this.inputContent)) {
                                this.isError = false;
                            } else {
                                this.errorContentData = this.errorContent.invalid;
                                this.isError = true;
                            }
                            break;
                    }
                } else {
                    // Nếu nội dung trống
                    //this.isDisplayX = false; // Tắt dấu xóa nội dung input
                    // Nếu là combox và input không cần check lỗi thì sẽ dừng phương thức
                    let requiredField = ["required", "combobox-required"]
                    if (!requiredField.includes(this.validType) || !this.isChange) {
                        this.isError = false;
                        return;
                    } else {
                        // Nếu không sẽ hiện lỗi
                        this.isError = true;
                        this.errorContentData = this.errorContent.empty
                    }

                }
            },
            /**
             * Xét dữ liệu cho input
             * Created by TBN (22/7/2021)
             */
            setInputContent(newContent) {
                this.inputContent = newContent;
                this.isDisplayX = true;
                this.isError = false;
            },
            /**
             * Thiết lập dự liệu ban đầu cho input
             * Created by TBN (22/7/2021) 
             */
            init(initialValue) {
                this.isChange = false;
                // Trigger Updated Stage
                this.inputContent = initialValue;
                if (initialValue != "" && initialValue != null)
                    this.isDisplayX = true;
                this.$emit('updateInput', this.inputContent) // Emit sự kiện cập nhật nội dung input lên component cha
                this.$emit('bindingDataInput', this.inputContent);
            }
        },
        computed: {
            toggle() {
                return this.$store.state.toggle;
            },
            resource() {
                return this.$store.state.resource;
            },
            errorContent() {
                return this.resource[this.$browserLocale]['common']['dictionaryError'][this
                    .name
                ];
            }
        }
    }
</script>

<style scoped>
    .error {
        border-color: rgb(255, 71, 71) !important;
    }

    .active {
        border-color: rgb(44, 160, 28)
    }

    .waiting-input {
        background-color: #e9e9e9e9 !important;
    }
</style>