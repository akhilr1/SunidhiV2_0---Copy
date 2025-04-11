import Vue from 'vue'
import Vuex from 'vuex'

// importing Modules
import App from './modules/App'
import GridContext from './modules/GridContext'
import SyncBinders from './modules/SyncBinders'
import DefaultStore from './modules/DefaultStore'
import MapReport from './modules/MapReports'

Vue.use(Vuex)

export default new Vuex.Store({
    modules: {
        App,
        GridContext,
        DefaultStore,
        SyncBinders,
        MapReport,
    }
})
