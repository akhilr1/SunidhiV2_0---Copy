import { CallWebService } from "./WebService.js"

export class MyTask {
    constructor(obj) {
        this.obj = obj;
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }

    async GetMyTaskActivityLog(formdata) {
        var url = window.SERVER_URL + "/MyTask/GetMyTaskActivityLog";
        return CallWebService("POST", url, formdata);
    }
    async GetLoadmoreTasks(formdata) {
        var url = window.SERVER_URL + "/MyTask/GetLoadmoreTasks";
        return CallWebService("POST", url, formdata);
    }
    async GetLoadmoreTasks(formdata) {
        var url = window.SERVER_URL + "/BulkApprove/GetAllApprovalDataBulk";
        return CallWebService("POST", url, formdata);
    }
}