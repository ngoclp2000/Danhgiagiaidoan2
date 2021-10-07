<template>
  <div class="toast-message" style="z-index: 100">
      <div class="toast" :class="data.type" v-for="(data,index) in toast" :key="index">
                <div class="toast-img center-vertical"></div>
                <div class="toast-content center-vertical">{{data.content}}</div>
      </div>
  </div>
</template>

<script>
export default {
    computed:{
        toast(){
            return this.$store.state.data.toast
        }
    }
}
</script>

<style>

</style>