import { CallWebService } from "./WebService.js"

export class MyProject {
    constructor(obj) {
        this.obj = obj;
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    async GetData(url) {
        return CallWebService("GET", url, null);
    }
    //============= Create MyProject ====================================
    async GetNextProjectCode() {
        var url = window.SERVER_URL + "/MyProject/GetNextProjectCode";
        return CallWebService("GET", url, null);
    }
    async GetCountryDetails() {
        var url = window.SERVER_URL + "/MyProject/GetCountryDetails";
        return CallWebService("GET", url, null);
    }
    async GetStateDetails(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetStateDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetDistrictDetails(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetDistrictDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetProjectThemeList(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetProjectThemeList";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTemplateList(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetProjectTemplateList";
        return CallWebService("POST", url, formdata);
    }
    async CreateProject(formdata) {
        var url = window.SERVER_URL + "/MyProject/CreateProject";
        return CallWebService("POST", url, formdata);
    }
    async GetInitiative(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetInitiative";
        return CallWebService("POST", url, null);
    }
    async InsertProjectTemplate(formdata) {
        var url = window.SERVER_URL + "/MyProject/InsertProjectTemplate";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTheme() {
        var url = window.SERVER_URL + "/Initiative/GetProjectTheme";
        return CallWebService("GET", url, null);
    }
    async GetProjectTemplateByprjtID(formdata) {
        var url = window.SERVER_URL + "/MyProject/GetProjectTemplateByprjtID";
        return CallWebService("POST", url, formdata);
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

    //============= Fetch MyProject ====================================
    async GetProjectListByInitID(formdata) {

        var url = window.SERVER_URL + "/MyProject/GetProjectListByInitID";
        return CallWebService("POST", url, formdata);
    }
    async DeleteProject(formdata) {

        var url = window.SERVER_URL + "/MyProject/DeleteProject";
        return CallWebService("POST", url, formdata);
    }
    async ChangeProjectStatus(formdata) {

        var url = window.SERVER_URL + "/MyProject/ChangeProjectStatus";
        return CallWebService("POST", url, formdata);
    }
    async ChangeState(formdata) {

        var url = window.SERVER_URL + "/MyProject/ChangeState";
        return CallWebService("POST", url, formdata);
    }
    async GetTheme() {
        var url = window.SERVER_URL + "/MyProject/GetTheme";
        return CallWebService("GET", url, null);
    }

    //===================================================================

}