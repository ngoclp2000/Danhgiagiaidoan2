<template>
  <div class="toast-message" style="z-index: 100">
      <div class="toast" :class="data.type" v-for="(data,index) in toast" :key="index">
          <div class="toast-left-content">
                <div class="toast-img"></div>
                <div class="toast-content">{{data.content}}</div>
            </div>
            <div class="toast-right-content">
                <div class="toast-icon"></div>
            </div>
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