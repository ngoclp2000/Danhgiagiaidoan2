<template>
    <router-link @click.native="toggleChosen" tag="div" :to="{path : link}" active-class="active"
        class="navbar-item-text " :class="['navbar-item',{'toggle-navbar' : toggle.isToggleNavbar}]" 
    >
        <div class="navbar-icon" :class="classItem"></div>
        <div :class="[toggle.isToggleNavbar ? 'hide' : 'display-inline-block-imp']" v-if="!toggle.isToggleNavbar">{{content}}</div>
    </router-link>
</template>

<script>
    export default {
        props: ['classItem', 'content', 'isChosen', 'link', 'classItemActive'],
        created() {},
        data() {
            return {}
        },
        methods: {
            /**
             * Sự kiện active lại option native khi người dùng load lại trang
             * Created By TBN (27/7/2021)
             */
            toggleChosen() {
                this.$store.commit('TOGGLE_ITEM_ACTIVE', this.classItem);
                sessionStorage.setItem('active', this.classItem);
            }
        },
        computed: {
            toggle() {
                return this.$store.state.toggle;
            },
        }
    }
</script>

<style scoped>
    .active {
        background-color: #5d5e61 !important;
        color: #ffffff !important;
        border-left-color: #2ca01c !important;
        font-weight: 700;
    }
</style>