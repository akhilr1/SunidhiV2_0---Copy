import TaskBulkActions from '@/BL/TaskBulkActions.js'

const state = {
  filterBox: "",
  editToggle: true,
  beneficiaryEdit: false,
  logicList: ["Survey Responses"],
  currentLogic: "Survey Responses",
  updationData: [],
  grids: new Map(),
  maxWidth: 0,
  mappedResponses: new Map(),
  responseContext: {},
};
const getters = {
    filterBox: state => state.filterBox,
    editToggle: state => state.editToggle,
    beneficiaryEdit: state => state.beneficiaryEdit,
    logicList: state => state.logicList,
    currentLogic: state => state.currentLogic,
    globalUpdationData: state => state.updationData,
    gridMap: state => state.grids,
    grids: state => [...state.grids.values()],
    maxWidth: state => state.maxWidth,
    mappedResponses: state => state.mappedResponses,
    responseContext: state => state.responseContext,
}
const actions = {
    updateFilterBox({commit}, val) {
        commit('setFilterBox', val)
    },
    updateEditToggle({commit}, val) {
        commit('setEditToggle', val)
    },
    updateBeneficiaryEditTogggle({commit}, val) {
        commit('setBeneficiaryEditToggle', val)
    },
    updateLogicList({commit}, val) {
        commit('setLogicList', val)
    },
    updateCurrentLogic({commit}, val) {
        commit('setCurrentLogic', val)
    },
    updateUpdationData({state, commit}, payload) {
      if(!payload) {
        // if no payload passed the updationData will be cleared
        // not necessary but fot the time being let it be
        commit('resetUpdationData')
      } else {
        const index = state.updationData.findIndex((x) => x.ID === payload.ID);
        if (index === -1) {
          commit("addUpdationData", payload);
        } else {
          commit("modifyUpdationData", { index, newValue: payload });
        }
      }
    },
    async saveGridData({state, commit}) {
      // Global Function for all grids
      // only for MyTasks Grid
      const { updateBulkData } = new TaskBulkActions();
      try {
        await updateBulkData(state.updationData)
        .then(res => {
          commit("resetUpdationData");
        })
      } catch (error) {
        console.error(error)
        return false;
      }
    },
    updateGrids({state, commit}, {grid, uid}) {
      // pass only uid to delete from gridMap
      if(!grid) {
        commit('unsetGrid', uid);
        return
      }
      if(!state.grids.has(uid)) {
        commit('setGrid', {grid, uid});
      }
    },
    clearAllGrids({commit}) {
      commit("clearGridMap");
    },
    updateMaxScrollWidth({state, commit}, val){
      if(val > state.maxWidth) {
        commit('setMaxWidth', val);
      }
    },
    updateMappedResponses({state, commit}) {},
    updateResponseContext({state, commit}) {},
}
const mutations = {
  setFilterBox(state, val) {
    state.filterBox = val;
  },
  setEditToggle(state, val) {
    state.editToggle = val;
  },
  setBeneficiaryEditToggle(state, val) {
    state.beneficiaryEdit = val;
  },
  setLogicList(state, val) {
    state.logicList.push(val);
  },
  setCurrentLogic(state, val) {
    state.currentLogic = val;
  },
  resetLogicList(state, val) {
    state.logicList = ["Survey Responses"];
  },
  resetUpdationData(state) {
    state.updationData = [];
  },
  addUpdationData(state, val) {
    state.updationData.push(val);
  },
  modifyUpdationData(state, { index, newValue }) {
    state.updationData[index] = newValue;
  },
  setGrid(state, { grid, uid }) {
    state.grids.set(uid, grid);
  },
  unsetGrid(state, uid) {
    state.grids.delete(uid);
  },
  clearGridMap(state) {
    state.grids = new Map();
  },
  setMaxWidth(state, val) {
    state.maxWidth = val;
  },
  setMappedResponses(state, val) {
    state.mappedResponses = val;
  },
  setResponseContext(state, val) {
    state.responseContext = val;
  }
};

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}