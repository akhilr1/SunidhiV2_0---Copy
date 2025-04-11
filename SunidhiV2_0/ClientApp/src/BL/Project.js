import { CallWebService } from "./WebService.js"

export class Project {
    constructor(obj) {
        this.obj = obj;     
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    async GetData(url) {
        return CallWebService("GET", url, null);
    }
    //============= Create Project ====================================
    async GetNextProjectCode() {
        var url = window.SERVER_URL + "/Project/GetNextProjectCode";
        return CallWebService("GET", url, null);
    }
    async GetCountryDetails() {
        var url = window.SERVER_URL + "/Project/GetCountryDetails";
        return CallWebService("GET", url, null);
    }
    async GetStateDetails(formdata) {
        var url = window.SERVER_URL + "/Project/GetStateDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetDistrictDetails(formdata) {
        var url = window.SERVER_URL + "/Project/GetDistrictDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetProjectThemeList(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectThemeList";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTemplateList(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectTemplateList";
        return CallWebService("POST", url, formdata);
    }
    async CreateProject(formdata) {
        var url = window.SERVER_URL + "/Project/CreateProject";
        return CallWebService("POST", url, formdata);
    }
    async GetUsersListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetUsersListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async GetUserListInWorkspace() {
        var url = window.SERVER_URL + "/Project/GetUsersListByID";
        return CallWebService("GET", url, null);
    }
    //====================Tags===============================
    async CreateNewTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/CreateNewTag";
        return CallWebService("POST", url, formdata);
    }
    async UpdateTagDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/UpdateTagDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetAllProjectTags(initid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllProjectTags?" + new URLSearchParams({ InitiativeId: initid });
        return CallWebService("GET", url, null);
    }

    //===================================================================

    //============= Fetch Project ====================================
    async GetProjectListByInitID(formdata) {

        var url = window.SERVER_URL + "/Project/GetProjectListByInitID";
        return CallWebService("POST", url, formdata);
    }
    async DeleteProject(formdata) {

        var url = window.SERVER_URL + "/Project/DeleteProject";
        return CallWebService("POST", url, formdata);
    }
    async ChangeProjectStatus(formdata) {

        var url = window.SERVER_URL + "/Project/ChangeProjectStatus";
        return CallWebService("POST", url, formdata);
    }
    async ChangeState(formdata) {

        var url = window.SERVER_URL + "/Project/ChangeState";
        return CallWebService("POST", url, formdata);
    }
  
    //===================================================================

}