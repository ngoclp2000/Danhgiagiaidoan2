<template>
  <div class="header" :class="{'full-w-header' : toggle.isToggleNavbar}">
        <div class="header-left">
            <div class="header-toggle-container" :class="{'background-toggle' : toggle.isToggleNavbar}">
                <div class="header-toggle"  @click="toggleNavbar" :class="{'background-white-toggle' : toggle.isToggleNavbar}">
                    
                </div>
            </div>
            <BaseCombobox ref="company" class="cbo select-container filter-select" :isDisabledInput="true"  dataType="companyList" />
        </div>
        <div class="account-info">
            <div class="account-avatar"></div>
            <div class="account-name">Trịnh Bảo Ngọc</div>
            <div class="account-option"></div>
        </div>
    </div>
</template>

<script>
import BaseCombobox from '../components/BaseCombobox.vue'
export default {
    components:{
        BaseCombobox
    },
    mounted(){
        // Lấy dữ liệu company từ store
        this.$refs.company.setData(this.$store.state.data.companyList.map(a => Object.assign({}, a)),true);
        this.$refs.company.updateInput(this.$store.state.data.companyList[0].content);
    },
    methods: {
        toggleNavbar: function () {
                this.$store.commit('TOGGLE_NAVBAR');
        }
    },
    computed:{
        toggle() {
                return this.$store.state.toggle
        }
    }
}
</script>

<style scoped>
    .full-w-header{
        width: 100%;
        position: absolute;
        top: 0;
    }
    .background-toggle{
        background-color: #38393d;
    }
    .background-white-toggle{
        background-position: -312px -32px !important;
    }
</style>