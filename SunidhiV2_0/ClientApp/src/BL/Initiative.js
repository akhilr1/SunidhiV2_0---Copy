import { CallWebService } from "./WebService.js"

export class Initiative {
    constructor(obj) {
        this.obj = obj;
        // this.a = "Test purpose";
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    async GetData(url) {
        return CallWebService("GET", url, null);
    }
    //============= Create Initiative ====================================
    async GetNextInitiativeCode() {
        var url = window.SERVER_URL + "/Initiative/GetNextInitiativeCode";
        return CallWebService("GET", url, null);
    }
    async GetInitTagDetails(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitTagDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetAllTags(formdata) {

        var url = window.SERVER_URL + "/Initiative/GetTagDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetInitUsersDetails(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitUsersDetails";
        return CallWebService("POST", url, formdata);
    }
    
    async GetInitUserGroups(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitUserGroups";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTheme() {
        var url = window.SERVER_URL + "/Initiative/GetProjectTheme";
        return CallWebService("GET", url, null);
    }
    async GetProjectTemplateItems(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetProjectTemplateItems";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTemplateDetails(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetProjectTemplateDetails";
        return CallWebService("POST", url, formdata);
    }
   
    async AddWorkflow(formdata) {
        var url = window.SERVER_URL + "/Initiative/AddWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async AddNewInitTag(formdata) {
        var url = window.SERVER_URL + "/Initiative/AddNewTag";
        return CallWebService("POST", url, formdata);
    }
    async CreateInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/CreateInitiative";
        return CallWebService("POST", url, formdata);
    }
    async UpdateInitTagDetails(formdata) {
        var url = window.SERVER_URL + "/Initiative/UpdateInitTagDetails";
        return CallWebService("POST", url, formdata);
    }
    //===================================================================

    //============= Fetch Initiative ====================================
    async GetInitiativeDetails(formdata) {

        var url = window.SERVER_URL + "/Initiative/GetInitiativeDetails";
        return CallWebService("POST", url, formdata);
    }

    async DeleteInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/DeleteInitiative";
        return CallWebService("POST", url, formdata);
    }

    async ChangeInitiativeStatus(formdata) {
        var url = window.SERVER_URL + "/Initiative/ChangeInitiativeStatus";
        return CallWebService("POST", url, formdata);
    }
    async ChangeStatus(formdata) {
        var url = window.SERVER_URL + "/Initiative/ChangeStatus";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectsDetails() {

        var url = window.SERVER_URL + "/Initiative/GetProjectsDetails";
        return CallWebService("GET", url, null);
    }

    //Manage Initiative functions starts here
    async GetInitiativeDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitiativeDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async UpdateInitiativeDetails(formdata) {
        var url = window.SERVER_URL + "/Initiative/UpdateInitiativeDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupsListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetGroupsListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async AddGroupToInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/AddGroupToInitiative";
        return CallWebService("POST", url, formdata);
    }
    async DeleteGroupFromInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/DeleteGroupFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    async GetUsersListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetUsersListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async DeleteUserFromInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/DeleteUserFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    //AddUsersButtom
    async AddUserInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/AddUserInitiative";
        return CallWebService("POST", url, formdata);
    }
    //get Initiative tags
    async GetInitiativeTags(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitiativeTags";
        return CallWebService("POST", url, formdata);
    }
    //Bulk remove user
    async DeleteBulkUserFromInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/DeleteBulkUserFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    //Bulk remove Group
    async DeleteBulkGroupFromInitiative(formdata) {
        var url = window.SERVER_URL + "/Initiative/DeleteBulkGroupFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    //Manage Initiative functiosn ends here
    
    //===================================================================
   
}