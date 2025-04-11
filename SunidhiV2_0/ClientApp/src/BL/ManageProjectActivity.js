import { CallWebService } from "./WebService.js"

export class ManageProjectActivity {
    constructor(obj) {
        this.obj = obj;     
    }
    async GetProjectDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectDetailsByID";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectActivityList(projid, multS) {
        var url = window.SERVER_URL + "/ProjectActivity/GetProjectActivityList?" + new URLSearchParams({ ProjectId: projid, multS: multS });
        return CallWebService("GET", url, null);
    }
    async GetSubActivityListByID(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetSubActivityListByID?" + new URLSearchParams({ ActivityId: actid });
        return CallWebService("GET", url, null);
    }
    async GetProjectActivityActualValues(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetProjectActivityActualValues?" + new URLSearchParams({ ActivityId: actid });
        return CallWebService("GET", url, null);
        
    }
    async GetProjectActivityPhysicalActualValues(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetProjectActivityPhysicalActualValues?" + new URLSearchParams({ ActivityId: actid });
        return CallWebService("GET", url, null);
        
    }
    async GetOutcomeActualValue(ocmid) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetOutcomeActualValue?" + new URLSearchParams({ outcomeID: ocmid});
        return CallWebService("GET", url, null);
    }  
    async GetProjectSurveyList(projid) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectSurveyList?" + new URLSearchParams({ ProjectId: projid });
        return CallWebService("GET", url, null);
    }
    async GetProjectWorkflowsList(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByProjectID";
        return CallWebService("POST", url, formdata);
    }
    async GetPeriodicityList() {
        var url = window.SERVER_URL + "/Common/GetReportingFrequencyDetails";
        return CallWebService("GET", url);
    }
    async GetSDGListForOutcome() {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetSDGListForOutcome";
        return CallWebService("GET", url);
    }
    async AddActivity(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/AddActivity";
        return CallWebService("POST", url, formdata);
    }
    async AddOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/AddOutCome";
        return CallWebService("POST", url, formdata);
    }
    async RemoveActivity(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/RemoveActivity";
        return CallWebService("POST", url, formdata);
    }
    async AssignUserToActivity(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/AssignUserToActivity";
        return CallWebService("POST", url, formdata);
    }
    async GetUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
        return CallWebService("POST", url, formdata);
    }

    //#region ProjectExpenseTags
    async AddNewExpenseTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/AddNewExpenseTag";
        return CallWebService("POST", url, formdata);
    }
    async EditExpenseTag(formdata) {
        var url = window.SERVER_URL + "/ProjectTags/EditExpenseTag";
        return CallWebService("POST", url, formdata);
    }
    async GetAllExpenseTags(projid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllExpenseTags?" + new URLSearchParams({ ProjectId: projid });
        return CallWebService("GET", url, null);
    }
    //#endregion ProjectExpenseTags

    //#region ProjectTags
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
    //#endregion ProjectTags

    //#region ManageProjectActivityIndividual
    async GetActivityDetailsByID(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetActivityDetailsByID?" + new URLSearchParams({ ActivityId: actid });
        return CallWebService("GET", url, null);
    }
    async GetActivityLogList(projid,actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetActivityLogList?" + new URLSearchParams({ ProjectId: projid, ActivityId: actid });
        return CallWebService("GET", url, null);
    }
    async GetSectionsBySurveyID(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetSectionsBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async GetQstnBySectionID(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetQstnBySectionID";
        return CallWebService("POST", url, formdata);
    }
    async GetLogicType() {
        var url = window.SERVER_URL + "/ProjectActivity/GetLogicType";
        return CallWebService("GET", url, null);
    }
    async GetLogicAnswers(QstnID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetLogicAnswers?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    } 
    async GetBeneficiaryAnswerOption(QstnID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetBeneficiaryAnswerOption?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    } 
    async FetchActivity(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/FetchActivity";
        return CallWebService("POST", url, formdata);
    }

    async UpdateOutputWorkflow(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/UpdateOutputWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyBeneficiaryType(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetSurveyBeneficiaryType";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyLinkedBeneficairyQuestionDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetSurveyLinkedBeneficairyQuestionDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetOutputSurveyIds(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetOutputSurveyIds?" + new URLSearchParams({ activityId: actid });
        return CallWebService("GET", url, null);
    }


    async GetOutputPivotPresetList(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetOutputPivotPresetList";
        return CallWebService("POST", url, formdata);
    } 

    async SaveOutPutPivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/SaveOutputPivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async RestoreOutPutPivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/RestoreOutputPivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async EditOutputPivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/EditOutputPivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async DeleteOutputPivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/DeleteOutputPivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async GetBeneficiaryQuestionOption(QstnID) {
        var url = window.SERVER_URL + "/ProjectActivity/GetBeneficiaryQuestionOption?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    } 

    async GetSurveyCascadingList() {
        var url = window.SERVER_URL + "/ProjectActivity/GetSurveyCascadingList";
        return CallWebService("GET", url, null);
    } 

    async GetCascadingHeirarchyList(qstId, isBeneficiary) {
        var url = window.SERVER_URL + "/ProjectActivity/GetCascadingHeirarchyList?questionid=" + qstId + "&isBeneficiary=" + isBeneficiary;
        return CallWebService("GET", url, null);
    }
    
    async GetUniqueOutputAutoGeneratedId(WsID, ProjectID) {
        let url = window.SERVER_URL + "/ProjectActivity/GetUniqueOutputAutoGeneratedId?workSpaceId=" + WsID + "&ProjectId=" + ProjectID
        return CallWebService("GET", url, null);
    }

    async GetSurveyCollectionBySurveyIds(ProjectId, SurveyIDs) {
        let url = window.SERVER_URL + "/ProjectActivity/GetSurveyCollectionBySurveyIds?ProjectId=" + ProjectId + "&surveyIds=" + SurveyIDs
        return CallWebService("GET", url, null);
    }
    
    //#endregion ManageProjectActivityIndividual

    //#startregion Outcome and Output Reports

    async GetOutputReports(ProjectId) {
        let url = window.SERVER_URL + "/ProjectActivity/GetOutPutReport?ProjectId=" + ProjectId
        return CallWebService("GET", url, null)
    }
    async GetOutcomeReports(ProjectId) {
        let url = window.SERVER_URL + "/ProjectActivity/GetOutcomeReport?ProjectId=" + ProjectId
        return CallWebService("GET", url, null)
    }

    //#endregion Outcome and Output Reports
    
}
