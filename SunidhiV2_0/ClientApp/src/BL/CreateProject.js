import { CallWebService } from "./WebService.js"

export class CreateProject {
    constructor(obj) {
        this.obj = obj;     
    }
  
    //============= Create Project ====================================
  

    async GetProjectThemeList(formdata) {
        var url = window.SERVER_URL + "/CreateProject/GetProjectThemeList";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectTemplateList(formdata) {
        var url = window.SERVER_URL + "/CreateProject/GetProjectTemplateList";
        return CallWebService("POST", url, formdata);
    }
    async CreateProjectAsync(formdata) {
        var url = window.SERVER_URL + "/CreateProject/CreateProjectAsyncNew";
        return CallWebService("POST", url, formdata);
    }
    async FetchTemplatePreDefinedData(formdata) {
        var url = window.SERVER_URL + "/CreateProject/FetchTemplatePreDefinedData";
        return CallWebService("POST", url, formdata);
    }
    //fnSaveTemplateDatatoProject
    async fnSaveTemplateDatatoProject(formdata) {
        var url = window.SERVER_URL + "/CreateProject/fnSaveTemplateDatatoProject";
        return CallWebService("POST", url, formdata);
    }
    async UpdateEditProjectDetails(formdata) {
        var url = window.SERVER_URL + "/CreateProject/UpdateEditProjectDetails";
        return CallWebService("POST", url, formdata);
    }
    async SaveInputs(formdata) {
        var url = window.SERVER_URL + "/CreateProject/SaveInputs";
        return CallWebService("POST", url, formdata);
    }
    async GetUserListInWorkspace(formdata) {
       
        var url = window.SERVER_URL + "/CreateProject/GetUsersListByID";
        return CallWebService("POST", url, formdata);
    }
}