import { CallWebService } from "./WebService.js"

export class Workspace {
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
    //============= Create Workspace ====================================
    async GetWSTagDetails() {
        var url = window.SERVER_URL + "/Workspace/GetWSTagDetails";
        return CallWebService("GET", url, null);
    }
    async AddNewWsTag(formdata) {
        var url = window.SERVER_URL + "/Workspace/AddNewWsTag";
        return CallWebService("POST", url, formdata);
    }
    async CreateWorkspace(formdata) {
        var url = window.SERVER_URL + "/Workspace/CreateWorkspace";
        return CallWebService("POST", url, formdata);
    }
    async UpdateWSTagDetails(formdata) {
        var url = window.SERVER_URL + "/Workspace/UpdateWSTagDetails";
        return CallWebService("POST", url, formdata);
    }
    //============= Manage Workspace ====================================
    async GetWorkspaceDetails(formdata) {
       
        var url = window.SERVER_URL + "/Workspace/GetWorkspaceDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetWSTags(formdata) {
        var url = window.SERVER_URL + "/Workspace/GetWSTags";
        return CallWebService("POST", url, formdata);
    }  
    async DeleteWSTagDetails(formdata) {
        var url = window.SERVER_URL + "/Workspace/DeleteWSTagDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateWorkspace(formdata) {
        
        var url = window.SERVER_URL + "/Workspace/UpdateWorkspace";
        return CallWebService("POST", url, formdata);
    }
    async SaveWSVisibility(formdata) {
       
        var url = window.SERVER_URL + "/Workspace/SaveWSVisibility";
        return CallWebService("POST", url, formdata);
    }
}