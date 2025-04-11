import { CallWebService } from "./WebService.js"

export class User {
 constructor(obj) {
    this.obj = obj;
    // this.a = "Test purpose";
  }

    async Login(email, passwd, notificationtoken) {
        var url = window.SERVER_URL + "/UserAccount/Login";
        var data = new FormData();
        data.append("username", email);
        data.append("password", passwd);
        data.append("notificationtoken", notificationtoken);

        return CallWebService("POST", url, data);
    }

    //async Signup({fullName,lastName,email,password}){
    //    let url = window.SERVER_URL + "/UserAccount/Signup"
    //    let formData = new FormData();
    //    formData.append("FirstName", fullName);
    //    formData.append("LastName", lastName);
    //    formData.append("EmailId", email);
    //    formData.append("Password", password);

    //    return CallWebService("POST", url, formData)
    //}

    async Signup({ Name, email, password }) {
        let url = window.SERVER_URL + "/UserAccount/Signup"
        let formData = new FormData();
        formData.append("Name", Name);
        formData.append("EmailId", email);
        formData.append("Password", password);

        return CallWebService("POST", url, formData)
    }

    async VerifySignup({email, otpValue}){
        let url = window.SERVER_URL + "/UserAccount/VerifySignupOTP"

        let formData = new FormData
        formData.append("EmailId", email)
        formData.append("OTP", otpValue)

        return CallWebService("POST", url, formData)
    }

    async ResetPassword({ password, key }) {
        let url = window.SERVER_URL + "/UserAccount/ResetPassword"

        let formData = new FormData();
        formData.append("Password", password)
        formData.append("key1", key)

        return CallWebService("POST", url, formData)
    }

    async ForgotPassword({emailId}) {
        let url = window.SERVER_URL + "/UserAccount/ForgotPassword?emailId=" + emailId
        return CallWebService("GET", url, null)
    }
    //===========UserList=====================
   
    async GetAllUsers(formdata) {
        var url = window.SERVER_URL + "/UserAccount/GetAllUserDetails";
        return CallWebService("POST", url, formdata);
    }
    //async GetAllUsersList(formdata) {
    //    var url = window.SERVER_URL + "/UserAccount/GetAllUsersList";
    //    return CallWebService("POST", url, formdata);
    //}
    async GetUserRoleDetails(formdata) {
        var url = window.SERVER_URL + "/UserAccount/GetUserRoleDetails";
        return CallWebService("POST", url, formdata);
    }
    async GetAllActiveUserGroup(formdata) {
      
        var url = window.SERVER_URL + "/UserGroups/GetUserGroupsList";
        return CallWebService("POST", url, formdata);
    }
   
    //-------------POST--------------------------------------------
   
    async GetUserScreenDetails(formdata) {
        
        var url = window.SERVER_URL + "/UserAccount/GetUserScreenDetails";
        return CallWebService("POST", url, formdata);
    }
    async AssignUserRole(formdata) {
        var url = window.SERVER_URL + "/UserAccount/AssignUserRole";
        return CallWebService("POST", url, formdata);
    }
    
    async AssignUserToGroup(formdata) {
        var url = window.SERVER_URL + "/UserAccount/AssignUserToGroup";
        return CallWebService("POST", url, formdata);
    }
  
   
    async DeleteUser(formdata) {
        var url = window.SERVER_URL + "/UserAccount/DeleteUser";
        return CallWebService("POST", url, formdata);
    }
    async ChangeActiveStatus(formdata) {
        var url = window.SERVER_URL + "/UserAccount/ChangeActiveStatus";
        return CallWebService("POST", url, formdata);
    }  
    

    async SendInvite(formdata) {        
        var url = window.SERVER_URL + "/UserAccount/InviteUser";
        return CallWebService("POST", url, formdata);
    }
    //===========UserIndividual =====================


    async GetUserIndividualDetails(data) {        
        var url = window.SERVER_URL + "/UserAccount/GetUserIndividualDetails";        
        return CallWebService("POST", url, data);
    }
    async GetAllActiveUserGroup_Ind(data) {
       
        var url = window.SERVER_URL + "/UserAccount/GetUserGroupList";
        return CallWebService("POST", url, data);
    }
    async GetAllUserProjects(data) {        

        var url = window.SERVER_URL + "/UserAccount/GetAllUserProjectsDetails";
        return CallWebService("POST", url, data);
    }
    //==============================================

    //===============INSERT NOTIFICATION TOKEN========

    async ManageNotificationToken(formdata) {
        var url = window.SERVER_URL + "/UserAccount/ManageNotificationToken";
        return CallWebService("POST", url, formdata);
    }
    //FetchUserAccessPagesList
    async FetchUserAccessPagesList(formdata) {
        var url = window.SERVER_URL + "/UserAccount/FetchUserAccessPagesList";
        return CallWebService("POST", url, formdata);
    }

    //ShareLink
    async GetShareLinkTaskFromServer(SurveyID, WorkspaceID) {
        var url = window.SERVER_URL + "/UserAccount/GetShareLinkTaskFromServer";
        var data = new FormData();
        data.append("surveyid", SurveyID);
        data.append("workspaceid", WorkspaceID);
        return CallWebService("POST", url, data);
    }
    async GetShareLinkStatus(SurveyID) {
        var url = window.SERVER_URL + "/UserAccount/GetShareLinkStatus";
        var data = new FormData();
        data.append("surveyid", SurveyID);
        return CallWebService("POST", url, data);
    }
    async DecryptShareLinkURL(formdata) {
        var url = window.SERVER_URL + "/UserAccount/DecryptShareLinkURL";
        return CallWebService("POST", url, formdata);
    } 
    async GetWorkflowLevelOneUsers(SurveyID) {
        var url = window.SERVER_URL + "/UserAccount/GetWorkflowLevelOneUsers";
        var data = new FormData();
        data.append("surveyid", SurveyID);
        return CallWebService("POST", url, data);
    }


}
