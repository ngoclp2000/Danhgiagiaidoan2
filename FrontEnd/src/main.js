import Vue from 'vue'
import App from './App.vue'
import store from './store'
import VueRouter from 'vue-router'
import {routes} from './router/index.js'
import VTooltip from 'v-tooltip'
import Skeleton from 'vue-loading-skeleton';
import Common from './store/common'


Vue.use(Skeleton);
Vue.use(VTooltip);
Vue.config.productionTip = true;

import DatePicker from 'vue2-datepicker';
Vue.use(DatePicker);
const router = new VueRouter({
  mode : 'history',
  routes,
})

/**
 * Chuyển tiêu đề trang
 * Created By TBN (5/8/2021)
 */
const DEFAULT_TITLE = 'Trang chủ';
router.afterEach((to) => {
    Vue.nextTick(() => {
        document.title = to.meta.title || DEFAULT_TITLE;
    });
});

Vue.use(VueRouter)
/**
 * Sự kiện click outside
 * Created By TBN (25/7/2021)
 */
Vue.directive("click-outside", {
  bind: function(element, binding, vnode) {
    element.clickOutsideEvent = function(event) {
      //  check that click was outside the el and his children
      if (!(element === event.target || element.contains(event.target))) {
        // and if it did, call method provided in attribute value
        vnode.context[binding.expression](event);
        // binding.value(); run the arg
      }
    };
    document.body.addEventListener("click", element.clickOutsideEvent);
  },
  unbind: function(element) {
    document.body.removeEventListener("click", element.clickOutsideEvent);
  },
});

// Kiểm tra xem đã có hàm format của string chưa, nếu chưa thì thêm vào String prototype
if (!String.prototype.format) {
  String.prototype.format = function() {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function(match, number) { 
      return typeof args[number] != 'undefined'
        ? args[number]
        : match
      ;
    });
  };
}
// Locale
Vue.prototype.$browserLocale =  Common.convertBrowserLocale( 'vi' || window.navigator.userLanguage || window.navigator.language);
new Vue({
  render: h => h(App),
  router,
  store
}).$mount('#app')
