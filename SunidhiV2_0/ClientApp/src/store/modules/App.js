import { IsAuthenticated } from "../../globals";
const serverURI = window.SERVER_URL;

const state = {
  authState: false,
  _loader: false,
};
const getters = {
  authState: (state) => state.authState,
  _loader: (state) => state._loader,
};
const actions = {
  async getAuthState({ state, commit }) {
    if (state.authState) {
      return state.authState;
    }
    try {
      let res = await IsAuthenticated();
      commit("setAuthState", res);
      return res;
    } catch (error) {
      throw error;
    }
  },
  async getShareLinkAuth({ state, commit }) {
    const url = `${serverURI}/UserAccount/ShareLinkLogin`;

    try {
      let response = await fetch(url, {
        method: "POST",
        credentials: "include",
      });
      return await response.json();
    } catch (error) {
      throw err;
    }
  },
  async getMapViewAuth({ state, commit, dispatch }, payload) {
    let { VersionId } = payload;
    let url = `${serverURI}/MapView/IsVersionExists`;

    let formData = new FormData();
    formData.append("VersionUniqueId", VersionId);

    try {
      let response = await fetch(url, {
        method: "POST",
        credentials: "include",
        body: formData,
      });
      let result = await response.json();

      if (result.status.toLowerCase() == "success") {
        commit("MapReport/SET_GUESTMODE", true, { root: true });
        dispatch("getShareLinkAuth");
        return true;
      } else if (result.status.toLowerCase() == "failed") {
        //   this.Msg = "Invalid VersionID";
        //   this.infoSnackbarMsg = true;
        throw new Error();
      }
    } catch (error) {
      throw error;
    }
  },
};
const mutations = {
  setAuthState(state, val) {
    state.authState = val;
  },
  setLoader(state, val) {
    state._loader = val;
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
