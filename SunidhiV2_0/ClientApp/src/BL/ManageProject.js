import { CallWebService } from "./WebService.js"

export class ManageProject {
    constructor(obj) {
        this.obj = obj;     
    }
    async GetInitiativeDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Initiative/GetInitiativeDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectDetailsByID(formdata) {

        var url = window.SERVER_URL + "/Project/GetProjectDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectDate(projectId) {
        var url = window.SERVER_URL + "/Project/GetProjectDate?ProjectId=" + projectId;
        return CallWebService("GET", url);
    }

    /*** Manage Project Details functions stsrts here ***/

    async GetCountryStatesDistricts() {
        var url = window.SERVER_URL + "/Project/GetCountryStatesDistricts";
        return CallWebService("GET", url);
    }
    async UpdateEditProjectDetails(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateEditProjectDetails";
        return CallWebService("POST", url, formdata);
    }
    /*** Manage Project Details functions ends here ***/

    /*** Manage Project Workflow functions stsrts here ***/
    async GetUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
        return CallWebService("POST", url, formdata);
    }
    async GetWorkflowsListByProjectID(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByProjectID";
        return CallWebService("POST", url, formdata);
    }
    async GetWorkflowsListForFilter(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListForFilter";
        return CallWebService("POST", url, formdata);
    }
    async GetWorkflowsListByIntitiativeID(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByIntitiativeID";
        return CallWebService("POST", url, formdata);
    }
    async UpdateProjectWorkflowDetails(formdata) {
        var url = window.SERVER_URL + "/Workflow/UpdateProjectWorkflowDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateProjectWorkflowLevelDetails(formdata) {
        var url = window.SERVER_URL + "/Workflow/UpdateProjectWorkflowLevelDetails";
        return CallWebService("POST", url, formdata);
    }
    async AddProjectWorkflow(formdata) {
        var url = window.SERVER_URL + "/Workflow/AddProjectWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async RemoveProjectWorkflows(formdata) {
        var url = window.SERVER_URL + "/Workflow/RemoveProjectWorkflows";
        return CallWebService("POST", url, formdata);
    }
    async AddSelectWorkflow(formdata) {
        var url = window.SERVER_URL + "/Workflow/AddSelectWorkflow";
        return CallWebService("POST", url, formdata);
    }
    /*** Manage Project Workflow functions ends here ***/
    //==========================Manage Team===========================
    async CreateTeam(formdata) {
        var url = window.SERVER_URL + "/Project/CreateTeam";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupDetailsByprjID(formdata) {
        var url = window.SERVER_URL + "/Project/GetGroupDetailsByprjID";
        return CallWebService("POST", url, formdata);
    }
    async GetUsersListByID(formdata) {
        var url = window.SERVER_URL + "/Project/GetUsersListByID";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupUsersList(formdata) {
        var url = window.SERVER_URL + "/Project/GetGroupUsersList";
        return CallWebService("POST", url, formdata);
    }

    async GetUsersListNotInTeam(formdata) {
        var url = window.SERVER_URL + "/Project/GetUsersListNotInTeam";
        return CallWebService("POST", url, formdata);
    }
    async GetTeamUsersList(formdata) {
        var url = window.SERVER_URL + "/Project/GetTeamUsersList";
        return CallWebService("POST", url, formdata);
    }
    async DeleteTeamUser(formdata) {
        var url = window.SERVER_URL + "/Project/DeleteTeamUser";
        return CallWebService("POST", url, formdata);
    }
    async UpdateGroupInProject(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateGroupInProject";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupsListToProject(formdata) {
        var url = window.SERVER_URL + "/Project/GetGroupsListToProject";
        return CallWebService("POST", url, formdata);
    }
    
    //=============Documents============================
    async GetAllDocumentList(formdata) {
        var url = window.SERVER_URL + "/Project/GetAllDocumentList";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectName(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectName";
        return CallWebService("POST", url, formdata);
    }
    async UploadDocument(formdata) {

        var url = window.SERVER_URL + "/Document/UplaodFiles";
        return CallWebService("POST", url, formdata);
    }   

    async RenameDocument(formdata) {

        var url = window.SERVER_URL + "/Document/RenameFile";
        return CallWebService("POST", url, formdata);
    }
    async RemoveDocument(formdata) {
        var url = window.SERVER_URL + "/Document/DeleteFile";
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
    async GetAllProjectTags(WorkpaceID) {
        
        var url = window.SERVER_URL + "/ProjectTags/GetAllProjectTags?" + new URLSearchParams({ WorkspaceID: WorkpaceID });
        return CallWebService("GET", url, null);
    }
    async DeleteProjectTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/DeleteProjectTag";
        return CallWebService("POST", url, formdata);
    }
      //==========================Manage PaymentTerms===========================
    async SavepaymentTerms(formdata) {
        var url = window.SERVER_URL + "/Project/SavepaymentTerms";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentTermsByprjID(formdata) {
        var url = window.SERVER_URL + "/Project/GetPaymentTermsByprjID";
        return CallWebService("POST", url, formdata);
    }
    async DeletePaymentTerm(formdata) {
        var url = window.SERVER_URL + "/Project/DeletePaymentTerm";
        return CallWebService("POST", url, formdata);
    }
    async UpdatePaymnetStatus(formdata) {
        var url = window.SERVER_URL + "/Project/UpdatePaymnetStatus";
        return CallWebService("POST", url, formdata);
    }
    async GetPaymentTermsByPaymentId(formdata) {
        var url = window.SERVER_URL + "/Project/GetPaymentTermsByPaymentId";
        return CallWebService("POST", url, formdata);
    }
    async UpdatepaymentTerms(formdata) {
        var url = window.SERVER_URL + "/Project/UpdatepaymentTerms";
        return CallWebService("POST", url, formdata);
    }
    async GetNotifyUsers(formdata) {
        var url = window.SERVER_URL + "/Project/GetNotifyUsers";
        return CallWebService("POST", url, formdata);
    }
    //==========================Manage Risk===========================
    async CreateRisk(formdata) {
        var url = window.SERVER_URL + "/Project/CreateRisk";
        return CallWebService("POST", url, formdata);
    }
    async GetWorkflows(formdata) {
        var url = window.SERVER_URL + "/Project/GetWorkflows";
        return CallWebService("POST", url, formdata);
    }
    async GetRiskByprjID(formdata) {
        var url = window.SERVER_URL + "/Project/GetRiskByprjID";
        return CallWebService("POST", url, formdata);
    }
    async DeleteRisk(formdata) {
        var url = window.SERVER_URL + "/Project/DeleteRisk";
        return CallWebService("POST", url, formdata);
    }
    async GetRiskByriskId(formdata) {
        var url = window.SERVER_URL + "/Project/GetRiskByriskId";
        return CallWebService("POST", url, formdata);
    }
    async UpdateRisk(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateRisk";
        return CallWebService("POST", url, formdata);
    }
    async GetActivityLog(formdata) {
        var url = window.SERVER_URL + "/Project/GetActivityLog";
        return CallWebService("POST", url, formdata);
    }
    async UpdateWorkflow(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async ImportRisk(formdata) {
        var url = window.SERVER_URL + "/Project/ImportRisk";
        return CallWebService("POST", url, formdata);
    }
    async AssignUserstoRisk(formdata) {
        var url = window.SERVER_URL + "/Project/AssignUserstoRisk";
        return CallWebService("POST", url, formdata);
    }

    //==========================Manage KPI===========================
    async CreateKPI(formdata) {
        var url = window.SERVER_URL + "/Project/CreateKPI";
        return CallWebService("POST", url, formdata);
    }
    async GetKPIWorkflows(formdata) {
        var url = window.SERVER_URL + "/Project/GetKPIWorkflows";
        return CallWebService("POST", url, formdata);
    }
    async GetKPIByprjID(formdata) {
        var url = window.SERVER_URL + "/Project/GetKPIByprjID";
        return CallWebService("POST", url, formdata);
    }
    async DeleteKPI(formdata) {
        var url = window.SERVER_URL + "/Project/DeleteKPI";
        return CallWebService("POST", url, formdata);
    }
    async GetKPIByKPIId(formdata) {
        var url = window.SERVER_URL + "/Project/GetKPIByKPIId";
        return CallWebService("POST", url, formdata);
    }
    async UpdateKPI(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateKPI";
        return CallWebService("POST", url, formdata);
    }
    async GetKPIActivityLog(formdata) {
        var url = window.SERVER_URL + "/Project/GetKPIActivityLog";
        return CallWebService("POST", url, formdata);
    }
    async UpdateKPIWorkflow(formdata) {
        var url = window.SERVER_URL + "/Project/UpdateKPIWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async ImportKPI(formdata) {
        var url = window.SERVER_URL + "/Project/ImportKPI";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyName(formdata) {

        var url = window.SERVER_URL + "/Project/GetSurveyName";
        return CallWebService("POST", url, formdata);
    }
    async GetReportingFrequencyList() {
        var url = window.SERVER_URL + "/Common/GetReportingFrequencyDetails";
        return CallWebService("GET", url);
    }
    async AssignUserstoKPI(formdata) {
        var url = window.SERVER_URL + "/Project/AssignUserstoKPI";
        return CallWebService("POST", url, formdata);
    }
}
