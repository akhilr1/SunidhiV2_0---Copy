import { CallWebService } from "./WebService.js"

export class ManageProjectAccount {
    constructor(obj) {
        this.obj = obj;     
    }
    async GetProjectDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async GetUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectUsers";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectAccountsByID(formdata) {
        var url = window.SERVER_URL + "/Account/GetProjectAccountsByID";
        return CallWebService("POST", url, formdata);
    }
    async AddAccount(formdata) {
        var url = window.SERVER_URL + "/Account/AddAccount";
        return CallWebService("POST", url, formdata);
    }
    async GetBankList() {
        var url = window.SERVER_URL + "/Account/GetBankList";
        return CallWebService("GET", url);
    }
    async GetBranchList(formdata) {
        var url = window.SERVER_URL + "/Account/GetBranchList";
        return CallWebService("POST", url, formdata);
    }
    async GetAccountTypeList() {
        var url = window.SERVER_URL + "/Account/GetAccountTypeList";
        return CallWebService("GET", url);
    }
    async GetIFSC(formdata) {
        var url = window.SERVER_URL + "/Account/GetIFSC";
        return CallWebService("POST", url, formdata);
    }
    async EditAccount(formdata) {
        var url = window.SERVER_URL + "/Account/EditAccount";
        return CallWebService("POST", url, formdata);
    }
    async RemoveAccount(formdata) {
        var url = window.SERVER_URL + "/Account/RemoveAccount";
        return CallWebService("POST", url, formdata);
    }
}
