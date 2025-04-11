import { CallWebService } from "./WebService.js"

export class ManageProjectFinance {
    constructor(obj) {
        this.obj = obj;
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    //============= Budget & Funds ====================================
    async GetProjectBudgetDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetProjectBudgetDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectUsers(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetProjectUsers";
        return CallWebService("POST", url, formdata);
    }
    async GetDonorAccountDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetDonorAccountDetails";
        return CallWebService("POST", url, formdata);
    }
    async SaveProjectBudgetsDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/SaveProjectBudgetsDetails";
        return CallWebService("POST", url, formdata);
    }
    async RemoveProjectBudgetUser(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/RemoveProjectBudgetUser";
        return CallWebService("POST", url, formdata);
    }
    async UpdateProjectBudget(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdateProjectBudget";
        return CallWebService("POST", url, formdata);
    }
    async UpdateDonorAmontAndAccount(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdateDonorAmontAndAccount";
        return CallWebService("POST", url, formdata);
    }
    //============= Budget Plan ====================================
    async GetProjectBudgetPlanDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetProjectBudgetPlanDetails";
        return CallWebService("POST", url, formdata);
    }
    //async GetAllProjectExpenseTags(projectID) {
    //    var url = window.SERVER_URL + "/ProjectTags/GetAllExpenseTags?ProjectId=" + projectID;
    //    return CallWebService("GET", url);
    //}
    //async AddNewExpenseTag(formdata) {
    //    var url = window.SERVER_URL + "/ProjectTags/AddNewExpenseTag";
    //    return CallWebService("POST", url, formdata);
    //}
    //async EditExpenseTag(formdata) {
    //    var url = window.SERVER_URL + "/ProjectTags/EditExpenseTag";
    //    return CallWebService("POST", url, formdata);
    //}
    async SaveProjectBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/AddProjectBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async UpdateProjectBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdateProjectBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async UpdateProjectBudgetPlanSubList(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdateProjectBudgetPlanSubList";
        return CallWebService("POST", url, formdata);
    }

    async RemoveBudgetPlanHead(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/RemoveBudgetPlanHead";
        return CallWebService("POST", url, formdata);
    }
    async RemoveBudgetPlanSubList(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/RemoveBudgetPlanSubList";
        return CallWebService("POST", url, formdata);
    }
     //==========================Manage PaymentTerms===========================
    async SavepaymentTerms(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/SavepaymentTerms";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentTermsByprjID(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetPaymentTermsByprjID";
        return CallWebService("POST", url, formdata);
    }
    async DeletePaymentTerm(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/DeletePaymentTerm";
        return CallWebService("POST", url, formdata);
    }
    async UpdatePaymnetStatus(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdatePaymnetStatus";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentTermsByPaymentId(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetPaymentTermsByPaymentId";
        return CallWebService("POST", url, formdata);
    }
    async UpdatepaymentTerms(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/UpdatepaymentTerms";
        return CallWebService("POST", url, formdata);
    }
    async SavePaymentRequest(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/SavePaymentRequest";
        return CallWebService("POST", url, formdata);
    }
    async UploadDocument(formdata) {
        var url = window.SERVER_URL + "/Document/UplaodFiles";
        return CallWebService("POST", url, formdata);
    }
    //============= Fund Utilization ====================================
    async GetFundPaymentDeatils(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetFundPaymentDeatils";
        return CallWebService("POST", url, formdata);
    }
    //============= Audit Log====================================
    async GetAuditLog(formdata) {
        var url = window.SERVER_URL + "/ProjectFinance/GetAuditLog";
        return CallWebService("POST", url, formdata);
    }

}