<template>
    <div>
        <date-picker @click.native="toggleCalendar($event)" :disabled-date="isFutureDate"
            @input.native="formatDateInput($event)" ref="datePick" v-model="date" value-type="YYYY-MM-DD"
            :format="format" :clearable="false" v-click-outside="hideCalendar"
             @mouseover.native="isDisplayToolTip=true" @mouseleave.native="isDisplayToolTip=false"
            :input-attr="{
                tabindex : tabindex,
                maxlength: maxlength,
                placeholder : placeholder,
            }"
             v-tooltip="{
                content: errorContent,
                delay: {
                    show: 300,
                    hide: 0,
                },
                trigger: 'manual',
                show: (isError && isDisplayToolTip),
            }">
            >
            <template v-slot:header="{ emit }">
                <button class="mx-btn mx-btn-text" @click="emit(new Date())">Hôm nay</button>
            </template>
        </date-picker>
        <!-- <div v-show="isError && isDisplayError" class="wrong-input">{{errorContent}}</div> -->
    </div>
</template>

<script>
    import 'vue2-datepicker/index.css';
    import 'vue2-datepicker/locale/vi';
    import Format from '../store/format.js'
    import Validation from '../store/validate.js'
    export default {
        created() {
            this.errorContent = this.resource[this.$browserLocale]['common'][
                'dictionaryError'
            ][this.name].invalid;
        },
        mounted() {
            let inputDate = this.$refs.datePick.$el.querySelector('.mx-input');
            inputDate.addEventListener('mousedown', () => {
                let datePickContent = document.querySelector('.mx-datepicker-content');
                if(datePickContent)
                    datePickContent.classList.remove('display-none-imp');
                this.isToggleCalendar = false;
            });
        },
        destroyed() {},
        components: {
            
        },
        props: {
            tabindex: String,
            name: String,
            maxlength: String,
        },
        data: () => ({
            date: null,
            inputContent: "",
            format: "DD/MM/YYYY",
            placeholder: "DD/MM/YYYY",
            isError: false,
            isDisplayToolTip: false,
            errorContent: "",
            isToggleCalendar: true,
            triggerCheckValid: false,
        }),
        watch: {
            /**
             * Cập nhật khi dữ liệu ngày tháng thay đổi
             * Created By TBN (22/8/2021)
             */
            date() {
                // Trường input của datepick
                let input = this.$refs.datePick.$el.querySelector('input');
                if(this.date)
                    this.inputContent = this.date.replaceAll("'", ""); // Xóa ' do thư viện tự thêm vào

                if (this.date == "") {
                    this.isError = false;
                    input.classList.remove('border-red')
                }
                this.$emit("bindingDataInput");
            }
        },
        computed: {
            resource() {
                return this.$store.state.resource;
            }
        },
        methods: {
            /**
             * Ẩn lịch
             * Created By TBN (22/8/2021)
             */
            hideCalendar() {
                this.isToggleCalendar = true;
            },
            /**
             * Ẩn hiện nội dung datepick
             * Created By TBN (22/8/2021)
             */
            toggleCalendar($event) {
                // Nội dung của datepick
                let datePickContent = document.querySelector('.mx-datepicker-content');
                // Chỉ lấy sự kiện khi bấm vào icon lịch hoặc container của nó
                if ($event.target.classList.length == 0 || $event.target.classList.contains('mx-icon-calendar')) {
                    // Phụ thuộc vào biến isToggleCalendar
                    if(this.isToggleCalendar){
                        datePickContent.classList.remove('display-none-imp');
                        this.$refs.datePick.$el.querySelector('input').focus(); // Focus vào input khi hiển thị lịch
                    }else{
                        datePickContent.classList.add('display-none-imp');
                    }
                    this.isToggleCalendar = !this.isToggleCalendar;
                }
            },
            /**
             * Chặn không cho chọn ngày trong tương lai
             */
            isFutureDate(date) {
                const currentDate = new Date();
                return date > currentDate;
            },
            /**
             * Format lại giúp người
             */
            formatDateInput($event) {
                let dateInput = $event.target;
                if (dateInput.value) {
                    let newDate = Format.formatDateInput(dateInput.value,this.format);
                    dateInput.value = newDate;
                }
            },
            /**
             * Sự kiện check lỗi nếu cần thiết 
             * Created By TBN (22/8/2021)
             */
            checkBlur($event) {
                let dateInput = $event.target;
                let temp = dateInput.value;
                if (this.date != null && this.date != "" && !Validation.checkValidDate(temp)) {
                    this.isError = true;
                    dateInput.classList.add('border-red')
                } else {
                    this.isError = false;
                    dateInput.classList.remove('border-red')
                }
            }
        }
    };
</script>
<style scoped>

</style>