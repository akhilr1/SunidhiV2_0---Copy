import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import ("@/assets/mycss.css");
import axios from  'axios';
//import 'bootstrap'
//import 'bootstrap/dist/css/bootstrap.min.css'
//import 'popper.js'
import './globals'
import vuetify from './plugins/vuetify';
//Audio reccorder
import AudioRecorder from 'vue-audio-recorder';
Vue.use(AudioRecorder);

import VueExcelEditor from 'vue-excel-editor'
//import VuePivottable from 'vue-pivottable'
//import 'vue-pivottable/dist/vue-pivottable.css'
//Vue.use(VuePivottable)
Vue.use(VueExcelEditor)

//Signature Pad
import VueSignaturePad from 'vue-signature-pad';
Vue.use(VueSignaturePad);

//GPS Capture
import VueGeolocation from 'vue-browser-geolocation';
Vue.use(VueGeolocation);

//Google Map
import * as VueGoogleMaps from 'vue2-google-maps'
Vue.use(VueGoogleMaps, {
    load: {
        key: 'AIzaSyBDC7AC24l6C-hBzxM_z5cqUDFAAs2X4Oc',//'AIzaSyAQAFnsPQlFQrdnI-iqcFeqTN2hSOJdcZs',
    },

})

// global event bus
Vue.prototype.$bus = new Vue();

Vue.config.productionTip = false

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
