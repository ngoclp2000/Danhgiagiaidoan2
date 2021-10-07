<template>
    <div class="navbar" :class="{'toggle-navbar-container' : toggle.isToggleNavbar}" >
        <div class="logo-site" v-show="!toggle.isToggleNavbar" >
            <div class="toggle-icon" @click="toggleNavbar"></div>
            <div class="misa-logo">
                <img src="../assets/image/logo.svg" alt="">
            </div>
        </div>
       
        <div class="navbar-content" :class="{'toggle-navbar-content' : toggle.isToggleNavbar}">
            <BaseItem  v-for="(navbar,index) in navbarItems" :classItem="navbar.classItem" :content="navbar.content" :isChosen="navbar.isChosen" :link="navbar.link" :key="index">
            </BaseItem>
        </div>
    </div>
</template>

<script>
    //import $ from 'jquery'
    import BaseItem from '../components/BaseItem.vue'
    export default {
        data() {
            return {
                
            }
        },
        created() {
            if(sessionStorage.getItem("active") != null) {
                this.$store.commit('TOGGLE_ITEM_ACTIVE',sessionStorage.getItem("active"))
            }
        },
        mounted() {
            if(window.innerWidth <= 1024){
                this.$store.commit('TOGGLE_NAVBAR');
            }
        }
        ,
        components: {
            BaseItem
        },
        methods: {
            /**
             * Gọi mutation toggle navbar trong store
             * Created by TBN 21/7/2021
             */
            toggleNavbar: function () {
                this.$store.commit('TOGGLE_NAVBAR');
            }
        },
        computed: {
            toggle() {
                return this.$store.state.toggle;
            },
            navbarItems(){
                return this.$store.state.navbarItems;
            }
        }
    }
</script>

<style scoped>
    .toggle-navbar-container{
        margin-top: 48px;
        width: 52px !important;
        height: calc(100vh - 48px) !important;
    }
    .toggle-navbar-content {
        width: 52px !important;
        height: 100% !important;
        padding-top: 0 !important;
    }
</style>