import { CallWebService } from "./WebService.js"

export class InitiativeWorkflow {
    constructor(obj) {
        this.obj = obj;
        // this.a = "Test purpose";
    }
    async GetUsersListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetUsersListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupsListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetGroupsListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async GetWorkflowsListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async UpdateWorkflowDetails(formdata) {
        var url = window.SERVER_URL + "/Workflow/UpdateWorkflowDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateWorkflowLevelDetails(formdata) {
        var url = window.SERVER_URL + "/Workflow/UpdateWorkflowLevelDetails";
        return CallWebService("POST", url, formdata);
    }
    async AddWorkflow(formdata) {
        var url = window.SERVER_URL + "/Workflow/AddWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async RemoveWorkflows(formdata) {
        var url = window.SERVER_URL + "/Workflow/RemoveWorkflows";
        return CallWebService("POST", url, formdata);
    }
}