<template>
  <div id="app">
    <TheMenu />
    <TheHeader/>
    <router-view/>
    <BaseToastMessage/>
  </div>
</template>

<script>
import TheMenu from './layout/TheMenu.vue'
import TheHeader from './layout/TheHeader.vue'
import BaseToastMessage from './components/BaseToastMessage.vue'
export default {
  created(){
    
  },
  mounted(){
    
  },
  name: 'App',
  data(){
    return{
      
    }
  }
  ,
  components: {
    TheMenu,
    TheHeader,
    BaseToastMessage,
  },
  destroyed(){
    
  }
}
</script>

<style>
  @import url("./static/css/common/main.css");

</style>
