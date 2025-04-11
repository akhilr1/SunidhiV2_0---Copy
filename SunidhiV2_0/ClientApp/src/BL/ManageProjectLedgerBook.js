import { CallWebService } from "./WebService.js"

export class ManageProjectLedgerBook {
    constructor(obj) {
        this.obj = obj;     
    }
    async GetProjectDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentList(projid, accid) {
        var url = window.SERVER_URL + "/LedgerBook/GetPaymentList?ProjectId=" + projid + "&AccountId=" + accid;
        return CallWebService("GET", url, null);
    }
    async GetActivityList(projid) {
        var url = window.SERVER_URL + "/LedgerBook/GetActivityList?ProjectId=" + projid;
        return CallWebService("GET", url, null);
    }
    async GetProjectAccountsByType(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/GetProjectAccountsByType";
        return CallWebService("POST", url, formdata);
    }
    //#region ProjectExpenseTags
    async AddNewExpenseTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/AddNewExpenseTag";
        return CallWebService("POST", url, formdata);
    }
    async EditExpenseTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/EditExpenseTag";
        return CallWebService("POST", url, formdata);
    }
    async GetAllExpenseTags(projid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllExpenseTags?" + new URLSearchParams({ ProjectId: projid });
        return CallWebService("GET", url, null);
    }
    //#endregion ProjectExpenseTags
    async UploadDocument(formdata) {
        var url = window.SERVER_URL + "/Document/UplaodFiles";
        return CallWebService("POST", url, formdata);
    }
    async AddPayment(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/AddPayment";
        return CallWebService("POST", url, formdata);
    }
    async RemovePayment(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/RemovePayment";
        return CallWebService("POST", url, formdata);
    }
    async EditPayment(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/EditPayment";
        return CallWebService("POST", url, formdata);
    }

    async GetUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
        return CallWebService("POST", url, formdata);
    }
    async GetDonorUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectUsers";
        return CallWebService("POST", url, formdata);
    }
    async AddRequestFund(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/AddRequestFund";
        return CallWebService("POST", url, formdata);
    }
    //ExportPaymentFile
    async ExportPaymentFile(formdata) {
        var url = window.SERVER_URL + "/LedgerBook/ExportPaymentFile";
        return CallWebService("POST", url, formdata);
    }
}
