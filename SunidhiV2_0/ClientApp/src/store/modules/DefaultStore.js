const state = {
    LoggedUserID: 0,
    SelectedCurrency: JSON.parse(localStorage.getItem('SelectedCurrency')) || {
        code: "INR",
        name: "Indian Rupee",
        name_plural: "Indian rupees",
        symbol: "Rs.",
        symbol_native: "₹",
        decimal_digits: 2,
        rounding: 0
    },
    SelectedFilter: JSON.parse(localStorage.getItem('SelectedFilter')) || {
        startdate: "",
        enddate: "",
        taskfiltervalue: [],
        workflowid: [],
        beneficiaryid: [],
        projectname: [],
        action: []
    },
}

const getters = {
    LoggedUserID: state => state.LoggedUserID,  // this.$store.getters.cnt
    getSelectedCurrency: state => state.SelectedCurrency,   // this.$store.getters.cnt
    getSelectedFilter: state => state.SelectedFilter
}

const actions = {  //Actions are not synchronous ,they indirectly commit . this.$store.dispatch("AddCnt",20)
    SaveLoggedUser(state, userid) {
        state.commit("SaveLoggedUser", userid);

    },
    SaveSelectedCurrency(state, currency) {
        state.commit("SaveSelectedCurrency", currency);

    },
    SaveSelectedFilter(state, workflowname) {
        state.commit("SaveSelectedFilter", workflowname);
    },
}

const mutations = {  //Mutations are synchronous.Cannot be used with asynchronous methods . this.$store.commit(“AddCnt”,10)
    SaveLoggedUser(state, userid) {
        state.LoggedUserID = userid;
    },
    SaveSelectedCurrency(state, payload) {
        state.SelectedCurrency = payload;
        localStorage.setItem('SelectedCurrency', JSON.stringify(payload))
    },
    SaveSelectedFilter(state, workflowname) {
        //debugger;
        state.SaveSelectedFilter = workflowname;
        localStorage.setItem('SelectedFilter', JSON.stringify(workflowname))
    },
}

export default {
    namespaced:true,
    state,
    getters,
    actions,
    mutations,
}