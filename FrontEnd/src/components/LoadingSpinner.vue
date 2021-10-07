<template>
  <div class="loader-model">
      <object :data="spinnerSVG" type=""></object>
  </div>
</template>
<script>
export default {
  name: "BaseSpinner"
  ,data(){
    return{
      spinnerSVG: require("../assets/icon/loading.svg")
    }
  }
}
</script>
<style>
.loader-model {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  /* background-color: #fff; */
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2;
}
</style>