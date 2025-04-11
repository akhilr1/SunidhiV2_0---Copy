import { CallWebService } from "./WebService.js"

export class Finance {
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
    async GetProjectListOfFinance(formdata) {
        var url = window.SERVER_URL + "/Finance/GetProjectListOfFinance";
        return CallWebService("POST", url, formdata);
    }
    async SaveDonarFund(formdata) {
        var url = window.SERVER_URL + "/Finance/SaveDonarFund";
        return CallWebService("POST", url, formdata);
    }
    async RemoveBudgetUser(formdata) {
        var url = window.SERVER_URL + "/Finance/RemoveBudgetUser";
        return CallWebService("POST", url, formdata);
    }
    //============= Budget plan ====================================
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
    async SaveBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/Finance/SaveBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async UpdateBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/Finance/UpdateBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async UpdateBudgetPlanitem(formdata) {
        var url = window.SERVER_URL + "/Finance/UpdateBudgetPlanitem";
        return CallWebService("POST", url, formdata);
    }
    async GetBudgetPlanExpenseTagsById(formdata) {
        var url = window.SERVER_URL + "/Finance/GetBudgetPlanExpenseTagsById";
        return CallWebService("POST", url, formdata);
    }
    async FetchBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/Finance/FetchBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async FetchBudgetPlanTags(formdata) {
        var url = window.SERVER_URL + "/Finance/FetchBudgetPlanTags";
        return CallWebService("POST", url, formdata);
    }
    async RemoveBudgetPlan(formdata) {
        var url = window.SERVER_URL + "/Finance/RemoveBudgetPlan";
        return CallWebService("POST", url, formdata);
    }
    async RemoveBudgetPlanItems(formdata) {
        var url = window.SERVER_URL + "/Finance/RemoveBudgetPlanItems";
        return CallWebService("POST", url, formdata);
    }
    //============= FinanceActivity====================================
    async GetFinanceActivityList(formdata) {
        var url = window.SERVER_URL + "/Finance/GetFinanceActivityList";
        return CallWebService("POST", url, formdata);
    }
    async GetSubFinanceActivityListByID(actid) {
        var url = window.SERVER_URL + "/Finance/GetSubFinanceActivityListByID?" + new URLSearchParams({ ActivityId: actid });
        return CallWebService("GET", url, null);
    }
    async UpdateFinanceWFToActivity(formdata) {
        var url = window.SERVER_URL + "/Finance/UpdateFinanceWFToActivity";
        return CallWebService("POST", url, formdata);
    }
     //============= FinanceWorkflow====================================
    async GetWorkflowsListToactivity(formdata) {
        var url = window.SERVER_URL + "/Finance/GetWorkflowsListToactivity";
        return CallWebService("POST", url, formdata);
    }
    async AddFinanceWorkflow(formdata) {
        var url = window.SERVER_URL + "/Finance/AddFinanceWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async UpdateFinanceWorkflowDetails(formdata) {
        var url = window.SERVER_URL + "/Finance/UpdateFinanceWorkflowDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateFinanceWorkflowLevelDetails(formdata) {
        var url = window.SERVER_URL + "/Finance/UpdateFinanceWorkflowLevelDetails";
        return CallWebService("POST", url, formdata);
    }
    async RemoveFinanceWorkflows(formdata) {
        var url = window.SERVER_URL + "/Finance/RemoveFinanceWorkflows";
        return CallWebService("POST", url, formdata);
    }
   //============= Payment====================================
    async GetBudgetHeadDetails(formdata) {
        var url = window.SERVER_URL + "/Finance/GetBudgetHeadDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetBudgetItemDetails(formdata) {
        var url = window.SERVER_URL + "/Finance/GetBudgetItemDetails";
    return CallWebService("POST", url, formdata);
    }
    async GetPaymentBudgetHead(formdata) {
        var url = window.SERVER_URL + "/Finance/GetPaymentBudgetHead";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentListOfActivicty(formdata) {
        var url = window.SERVER_URL + "/Finance/GetPaymentListOfActivicty";
        return CallWebService("POST", url, formdata);
    }
      //============= Audit Log====================================
    async GetAuditLog(formdata) {
        var url = window.SERVER_URL + "/Finance/GetAuditLog";
        return CallWebService("POST", url, formdata);
    }
}