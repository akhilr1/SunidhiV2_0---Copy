import { CallWebService } from "./WebService.js"

export class UserRole {

    constructor(obj) {
        this.obj = obj;
    }

    //-----  for user role vue -----------------
    async GetUserRolesList(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetUserRolesLists";
        return CallWebService("POST", url, formdata);
     }
    async GetScreenList() {
        var url = window.SERVER_URL + "/UserRoles/GetScreenList";
        return CallWebService("GET", url, null);
    }
    async GetAccessTypeList() {
        var url = window.SERVER_URL + "/UserRoles/GetAllAceesType";
        return CallWebService("GET", url, null);
    }
    async DeleteUserRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/DeleteUserRole";
        return CallWebService("POST", url, formdata);
    }
    async AddRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/AddRole";
        return CallWebService("POST", url, formdata);
    }

    async GetUsersToAssignRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetUsersToAssignRole";
        return CallWebService("POST", url, formdata);
    }

    async AssignUsersToRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/AssignUsersToRole";
        return CallWebService("POST", url, formdata);
    }

    async GetRoleDetails(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetRoleDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/UpdateRole";
        return CallWebService("POST", url, formdata);
    }
        
    async GetRoleBasedUsersList(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetRoleBasedUsersList";
        return CallWebService("POST", url, formdata);
    }

    async GetAllUserProjects(ID) {
        var data = new FormData();
        data.append("userId", ID);

        var url = window.SERVER_URL + "/UserAccount/GetAllUserProjectsDetails";
        return CallWebService("POST", url, data);
    }
    async DeleteAssignedUser(formdata) {
        var url = window.SERVER_URL + "/UserRoles/DeleteAssignedUser";
        return CallWebService("POST", url, formdata);
    }
    async GetScreensListByRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetScreensByRole";
        return CallWebService("POST", url, formdata);
    }
    async GetAccessTypeList() {
        var url = window.SERVER_URL + "/UserRoles/GetAllAceesType";
        return CallWebService("GET", url, null);
    }
    async GetUserWithoutScreens(formdata) {
        var url = window.SERVER_URL + "/UserRoles/GetUserWithoutScreens";
        return CallWebService("POST", url, formdata);
    }
    async DeleteScreenFromRole(formdata) {
        var url = window.SERVER_URL + "/UserRoles/DeleteScreenFromRole";
        return CallWebService("POST", url, formdata);
    }
    //----- ends here -----------------
}
