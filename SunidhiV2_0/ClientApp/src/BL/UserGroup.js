import { CallWebService } from "./WebService.js"

export class UserGroup {

    constructor(obj) {
        this.obj = obj;
    }

    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }

    async GetData(url) {
        return CallWebService("GET", url, null);
    }
}
