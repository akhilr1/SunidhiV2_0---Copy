import { CallWebService } from "./WebService.js"

export class InitiativeTemplate {
    constructor(obj) {
        this.obj = obj;
        // this.a = "Test purpose";
    }
    async GetProjectThemeByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/GetProjectThemeByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async RemoveProjectThemeFromInitiative(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/RemoveProjectThemeFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    async GetAllProjectTheme() {
        var url = window.SERVER_URL + "/InitiativeTemplate/GetAllProjectTheme";
        return CallWebService("GET", url);
    }
    async AddProjectThemeToInitiative(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/AddProjectThemeToInitiative";
        return CallWebService("POST", url, formdata);
    }
    async RemoveProjectTemplateFromInitiative(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/RemoveProjectTemplateFromInitiative";
        return CallWebService("POST", url, formdata);
    }
    async GetAllProjectTemplateByThemeID(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/GetAllProjectTemplateByThemeID";
        return CallWebService("POST", url, formdata);
    }
    async AddProjectTemplateToInitiative(formdata) {
        var url = window.SERVER_URL + "/InitiativeTemplate/AddProjectTemplateToInitiative";
        return CallWebService("POST", url, formdata);
    }
}