
const state = {
    syncFromFlag: false,
    syncToFlag: false,
    initState: true,
    TaskListStatus: false,
}

const getters = {
    initState: state => state.initState,
    TaskListStatus: state => state.TaskListStatus
}

const mutations = {
    updateSyncFromFlag(state){
        state.syncFromFlag = !(state.syncFromFlag);
    },
    updateSyncToFlag(state){
        state.syncToFlag = !(state.syncToFlag);
    },
    updateInitState(state,val){
        state.initState = val
    },
    updateTaskListStatus(state, val){
        state.TaskListStatus = val
    }
}

export default {
    namespaced: true,
    state,
    getters,
    mutations
}