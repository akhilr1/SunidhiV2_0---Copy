import { CallWebService } from "./WebService.js"

export class Venture {
    constructor(obj) {
        this.obj = obj;
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    async GetData(url) {
        return CallWebService("GET", url, null);
    }
    //============= Search Venture ====================================
    async GetProjectListOfVenture(formdata) {
        var url = window.SERVER_URL + "/Venture/GetProjectListOfVenture";
        return CallWebService("POST", url, formdata);
    }
    async InsertCommitFund(formdata) {

        var url = window.SERVER_URL + "/Venture/InsertCommitFund";
        return CallWebService("POST", url, formdata);
    }
    async UpdateCommitFund(formdata) {

        var url = window.SERVER_URL + "/Venture/UpdateCommitFund";
        return CallWebService("POST", url, formdata);
    }
    async AddToWishlist(formdata) {

        var url = window.SERVER_URL + "/Venture/AddToWishlist";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectListOfFundedAndWishList(formdata) {
        var url = window.SERVER_URL + "/Venture/GetProjectListOfFundedAndWishList";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectAccounts() {
        var url = window.SERVER_URL + "/Venture/GetProjectAccounts";
        return CallWebService("GET", url);

    }
    async AddAccount(formdata) {
        var url = window.SERVER_URL + "/Venture/AddAccount";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectExpenses(formdata) {
        var url = window.SERVER_URL + "/Venture/GetProjectExpenses";
        return CallWebService("POST", url, formdata);

    }
    async GetTotalProjectExpenses(formdata) {
        var url = window.SERVER_URL + "/Venture/GetTotalProjectExpenses";
        return CallWebService("POST", url,formdata);

    }
    async GetFundedUsersById(formdata) {
        var url = window.SERVER_URL + "/Venture/GetFundedUsersById";
        return CallWebService("POST", url, formdata);
    }
    async GetTotalFundById(formdata) {
        var url = window.SERVER_URL + "/Venture/GetTotalFundById";
        return CallWebService("POST", url, formdata);
 

    }
    async GetLedgerBookExpenseTagsById(formdata) {
        var url = window.SERVER_URL + "/Venture/GetLedgerBookExpenseTagsById";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentList(projid, accid) {
        var url = window.SERVER_URL + "/LedgerBook/GetPaymentList?ProjectId=" + projid + "&AccountId=" + accid;
        return CallWebService("GET", url, null);
    }
    async DeleteVenture(formdata) {
        var url = window.SERVER_URL + "/Venture/DeleteVenture";
        return CallWebService("POST", url, formdata);
    }
}