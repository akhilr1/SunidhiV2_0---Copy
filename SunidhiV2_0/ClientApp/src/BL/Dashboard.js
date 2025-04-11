import { CallWebService } from "./WebService.js"
export class Dashboard {


    async GetProjectListByWSID(formdata) {
        var url = window.SERVER_URL + "/Dashboard/GetProjectListByWSID";
        return CallWebService("POST", url, formdata);
    }
    async GetActivityCount(formdata) {
        var url = window.SERVER_URL + "/Dashboard/GetActivityCount";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectProgress(formdata) {
        var url = window.SERVER_URL + "/Dashboard/GetProjectProgress";
        return CallWebService("POST", url, formdata);
    }
    async FillDashboard(formdata) {
        var url = window.SERVER_URL + "/Dashboard/FillDashboard";
        return CallWebService("POST", url, formdata);
    }
    async FillDashboardMyTeam(formdata) {
        var url = window.SERVER_URL + "/Dashboard/FillDashboardMyTeam";
        return CallWebService("POST", url, formdata);
    }
    async FillDashboardMyTask(formdata) {
        var url = window.SERVER_URL + "/Dashboard/FillDashboardMyTask";
        return CallWebService("POST", url, formdata);
    }
}