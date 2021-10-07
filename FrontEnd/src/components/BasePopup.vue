<template>
    <div v-if="isShow">
        <div class="popup-layout center-div" ref="layout">
        </div>
        <div class=" popup">
            <div class="popup-header">
                <div class="popup-header-title">
                    Xác nhận
                </div>
                <div class="popup-header-btn">
                    <div class="m-btn-close" @click="cancelButtonClick">

                    </div>
                </div>
            </div>

            <div class="divide"></div>
            <div class="popup-content" ref="popupContent">
                
                <div class="content-popup">
                    <!-- <div :class="objectPopUp.type" class="icon-popup">
                    </div> -->
                    <div ref="content" class="content-popup-msg">
                        
                    </div>
                </div>
                
                <div class="popup-footer"
                    :class="{ 'flex-center' : numberOfButton == 1,'flex-end' : objectPopUp.type == 'warning-code'}">
                    <div class="popup-footer-left" v-if="objectPopUp.buttonTexts[0] != ''">
                        <BaseButton tabindex="90" 
                            @click="cancelButtonClick" classList="m-btn-default m-second-btn" bgColor="#fff"
                            textColor="#111111" :btnText="objectPopUp.buttonTexts[0]" ref="secondary" v-if="numberOfButton != 2"/>
                    </div>
                    <div class="popup-footer-right">
                        <BaseButton tabindex="90" 
                            @click="cancelButtonClick" classList="m-btn-default m-second-btn" bgColor="#fff"
                            textColor="#111111" :btnText="objectPopUp.buttonTexts[0]" ref="secondary" v-if="numberOfButton == 2"/>
                        <BaseButton tabindex="90" 
                            @click="subPrimaryButtonClick" v-if="objectPopUp.buttonTexts[1] != ''"
                            classList="m-btn-default m-second-btn" bgColor="#fff" textColor="#111111"
                            :btnText="objectPopUp.buttonTexts[1]" />
                        <BaseButton tabindex="90" 
                            ref="primary" @keydown.enter="primaryButtonClick" :class="{'mr0' : numberOfButton == 1}"
                            @click="primaryButtonClick" :classList="(objectPopUp.type == 'warning' ? 'm-btn-default m-btn-delete': 'm-btn-default m-primary-btn')"
                            textColor="#ffffff"
                            :btnText="objectPopUp.buttonTexts[2]" />
                    </div>
                </div>
            </div>

        </div>
    </div>

</template>

<script>
    import BaseButton from './BaseButton.vue'
    export default {
        props: [],
        components: {
            BaseButton
        },
        created() {

        },
        mounted() {
            setTimeout(() => {
                this.$refs["content"].innerHTML = this.objectPopUp.contentPopUp
                // Focus vào nút primary
                this.$refs.primary.$refs.button.focus();
                // Đặt sự kiện tab cho nút primary khi tab sẽ back về nút secondary
                this.$refs.primary.$refs.button.addEventListener('keydown', (e) => {
                    if ((e.which === 9 && !e.shiftKey)) {
                        e.preventDefault();
                        if(this.$refs.secondary){
                            this.$refs.secondary.$refs.button.focus() // Focus secondary button 
                        }
                    }else if(e.which === 9 && e.shiftKey){
                        if(!this.$refs.secondary){
                            this.$refs.primary.$refs.button.focus() // Focus primary button
                        }
                    }
                })
                // Đăt sự kiện shift tab cho secondary button
                if(this.$refs.secondary){
                    this.$refs.secondary.$refs.button.addEventListener('keydown', (e) => {
                    if ((e.which === 9 && e.shiftKey)) {
                        e.preventDefault();
                        this.$refs.primary.$refs.button.focus() // Focus primary button
                    }
                })
                }
            }, 200)
        },
        data() {
            return {
                isShow: false,
                errorContents: [],
                contentTooltip: "",
                isOverflowContent: false,
            }
        },
        methods: {
            /**
             * Sự kiện bấm nút primary trong popup
             * Created by TBN (21/7/2021)
             */
            primaryButtonClick() {
                if (this.objectPopUp.type == 'confirmation') {
                    this.$emit('storeInformation'); // Lưu thông tin
                } else if (this.objectPopUp.type == 'error' || this.objectPopUp.type == 'warning-code') {
                    this.$emit('focusErrorField', this.errorContents) // Focus vào lỗi đầu tiên
                    this.$emit('refreshData'); // Cập nhật lại dữ liệu
                } else {
                    this.$emit('deleteData'); // Xóa dữ liệu
                }
                this.isShow = false;
                this.$emit('hideLayout');
            },
            /**
             * Sự kiện bấm nút sub primary trong popup
             * Created By TBN (18/8/2021)
             */
            subPrimaryButtonClick() {
                if (this.objectPopUp.type == 'confirmation') {
                    this.$emit('hideForm'); // Ẩn form
                }
            },
            /**
             * Sự kiện bấm nút bên trái trong popup
             * Created by TBN (21/7/2021)
             */
            cancelButtonClick() {
                this.$emit('hideLayout');
                this.isShow = false;
            },
            /**
             * Đặt nội dung lỗi cho popup error list
             * Created by TBN (21/7/2021)
             */
            setContentError(errorContents) {
                this.errorContents = errorContents
            }
        },
        computed: {
            objectPopUp() {
                return this.$store.state.data.objectPopUp;
            },
            numberOfButton() {
                return this.objectPopUp.buttonTexts.filter(btnText => btnText != "").length;
            },
            tooltipPopUp() {
                return this.$store.state.resource[this.$browserLocale]['management']['entity']['Employee'][
                    'tooltipContent'
                ]['popup'];
            }
        }
    }
</script>

<style scoped>
    .flex-center {
        justify-content: center !important;
        align-content: center !important;
    }

    .flex-end {
        justify-content: flex-end !important;
        align-content: center !important;
    }
</style>