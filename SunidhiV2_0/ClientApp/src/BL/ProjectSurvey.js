import { CallWebService } from "./WebService.js"

export class ProjectSurvey {
    constructor(obj) {
        this.obj = obj;
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
    async GetAllProjectTags(initid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllProjectTags?" + new URLSearchParams({ InitiativeId: initid });
        return CallWebService("GET", url, null);
    }
    //==========================================================

    async GetProjectSurveyList(projectId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectSurveyList?ProjectId=" + projectId;
        return CallWebService("GET", url);
    }
    async GetProjectDate(projectId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectDate?ProjectId=" + projectId;
        return CallWebService("GET", url);
    }
    async SaveProjectSurvey(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/SaveProjectSurvey";
        return CallWebService("POST", url, formdata);
    }
    async DeleteProjectSurvey(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/DeleteProjectSurvey";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectSectionList(SurveyId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectSectionList?SurveyId=" + SurveyId;
        return CallWebService("GET", url);
    }
    async GetProjectWorkFlow(projectId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectWorkFlow?projectId=" + projectId;
        return CallWebService("GET", url);
    }
    
    async GetProjectData(WorkspaceID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectData?WorkspaceID=" + WorkspaceID;
        return CallWebService("GET", url);
    }
    async GetDetailProjectData(ProjectId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetDetailProjectData?ProjectId=" + ProjectId;
        return CallWebService("GET", url);
    }
    async GetSurveyProgress(ProjectId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetSurveyProgress?ProjectId=" + ProjectId;
        return CallWebService("GET", url);
    }
    async FetchAllSurveyQuestDetail(SurveyID, QuestionID) {
        var url = window.SERVER_URL + "/ProjectSurvey/FetchAllSurveyQuestDetail?SurveyID=" + SurveyID + "&QuestionID=" + QuestionID;
        return CallWebService("GET", url);
    }

    async GetSurveyData(ProjectId, mStartDate, mEndDate) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetSurveyData?ProjectId=" + ProjectId + "&mStartDate=" + mStartDate + "&mEndDate=" + mEndDate;
        return CallWebService("GET", url);
    }

    async GetProjectActivityData(ProjectId, mStartDate, mEndDate) {
        var url = window.SERVER_URL + "/ProjectActivity/GetProjectActivityData?ProjectId=" + ProjectId + "&mStartDate=" + mStartDate + "&mEndDate=" + mEndDate;
        return CallWebService("GET", url);
    }

    async GetQuestionData(SurveyID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetQuestionData?SurveyID=" + SurveyID;
        return CallWebService("GET", url);
    }
    async GetSurveyAuditLog(SurveyID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetSurveyAuditLog?SurveyID=" + SurveyID;
        return CallWebService("GET", url);
    }
    async GetBeneficiaryData(SurveyID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetBeneficiaryData?SurveyID=" + SurveyID;
        return CallWebService("GET", url);
    }
    async GetProjectBeneficiaryType(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectBeneficiaryType";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectSurveyBySurveyId(SurveyId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectSurveyBySurveyId?SurveyId=" + SurveyId;
        return CallWebService("GET", url);
    }
    async GetProjectTeamUsersList(formdata) {
        var url = window.SERVER_URL + "/Project/GetTeamUsersList";
        return CallWebService("POST", url, formdata);
    }
    async GetGroupDetailsByprjID(formdata) {
        var url = window.SERVER_URL + "/Project/GetGroupDetailsByprjID";
        return CallWebService("POST", url, formdata);
    }
    async AssignUserstoSurvey(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/AssignUserstoSurvey";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyAssignedUsers(SurveyId) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetSurveyAssignedUsers?SurveyID=" + SurveyId;
        return CallWebService("GET", url);
    }
    async GetWorkflowsListByProjectID(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByProjectID";
        return CallWebService("POST", url, formdata);
    }
    async UpdateSurveyWorkflow(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateWorkflow";
        return CallWebService("POST", url, formdata);
    }
    async ImportProjectSurvey(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/ImportProjectSurvey";
        return CallWebService("POST", url, formdata);
    }
    async GetReportingFrequencyList() {
        var url = window.SERVER_URL + "/Common/GetReportingFrequencyDetails";
        return CallWebService("GET", url);
    }
    async UpdateSurveyBeneficiaryType(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateSurveyBeneficiaryType";
        return CallWebService("POST", url, formdata);
    } 

    async SaveDuplicateProjectSurvey(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/SaveDuplicateProjectSurvey";
        return CallWebService("POST", url, formdata);
    }   
    async GetProjectTeam(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectTeam";
        return CallWebService("POST", url, formdata);
    }
    //================Survey Section=======================//
    async GetAllSectionsBySurveyID(SurveyID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllSectionsBySurveyID?SurveyID=" + SurveyID;
        return CallWebService("GET", url, null);
    }
    async SaveProjectSurveySection(formdata) {
      
        var url = window.SERVER_URL + "/ProjectSurvey/SaveProjectSurveySection";
        return CallWebService("POST", url, formdata);
    }
    async UpdateSectionOrder(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateSectionOrder";
        return CallWebService("POST", url, formdata);
    } 
    async RemoveSurveySection(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/RemoveSurveySection";
        return CallWebService("POST", url, formdata);
    }
    async GetAllQstnsForSectionLogic(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllQstnsForSectionLogic";
        return CallWebService("POST", url, formdata);
    }
    async GetSectionsLogic(SurveyID, SectionID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetSectionsLogic?SurveyID=" + SurveyID + "&SectionID=" + SectionID;
        return CallWebService("GET", url, null);
    }
    async GetAllLogicConditions() {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllLogicConditions";
        return CallWebService("GET", url, null);
    }
    async GetAllFormulaFunctions() {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllFormulaFunctions";
        return CallWebService("GET", url, null);
    }
    async GetLogicAnswers(QstnID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetLogicAnswers?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    } 
    async UpdateSectionLogic(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateSectionLogic";
        return CallWebService("POST", url, formdata);
    }
    async DeleteSectionLogicItem(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/DeleteSectionLogicItem";
        return CallWebService("POST", url, formdata);
    }
    
    //CheckSureveyTaskIsAprovedWhileUpdate
    async CheckSureveyTaskIsAprovedWhileUpdate(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/CheckSureveyTaskIsAprovedWhileUpdate";
        return CallWebService("POST", url, formdata);
    }
    async GetAllQuestionTypes() {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllQuestionTypes";
        return CallWebService("GET", url, null);
    }
    async GetCascadingQuestionOptions() {
        var url = window.SERVER_URL + "/ProjectSurvey/GetCascadingQuestionOptions";
        return CallWebService("GET", url, null);
    }
    async GetCascadingQuestionSubOptions(OptionID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetCascadingQuestionSubOptions?ParentOptionID=" + OptionID;
        return CallWebService("GET", url, null);
    }   
    async GetQuestionDetails(QuestionID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetQuestionDetails?QuestionID=" + QuestionID;
        return CallWebService("GET", url, null);
    }   
    async AddSurveySectionQstn(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/AddSurveySectionQstn";
        return CallWebService("POST", url, formdata);
    }
    async UpdateQuestionOrder(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateQuestionOrder";
        return CallWebService("POST", url, formdata);
    } 
    async UpdateQuestionSettings(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateQuestionSettings";
        return CallWebService("POST", url, formdata);
    }
    async RemoveSurveySectionQuestion(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/RemoveSurveySectionQuestion";
        return CallWebService("POST", url, formdata);
    }  
    async UpdateQuestion(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/UpdateQuestion";
        return CallWebService("POST", url, formdata);
    }
    async GetAllResponsesBySurveyID(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllResponsesBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async GetAllResponsesBySurveyReport(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllResponsesBySurveyReport";
        return CallWebService("POST", url, formdata);
    }
    async GetAllIndividualResponseBySurveyID(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllIndividualResponseBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async GetAllSummaryResponseBySurveyID(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllSummaryResponseBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async GetPivotTableData(surveyid) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetPivotTableData?url=getpivotdata?surveyid=" + surveyid;
        return CallWebService("GET", url);
    }

    async SavePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/SavePivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async EditPivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/EditPivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async DeletePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/DeletePivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async RestorePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/RestorePivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async GetPivotPresetList(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetPivotPresetList";
        return CallWebService("POST", url, formdata);
    } 

    async SaveShareLinkStatus(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/SaveShareLinkStatus";
        return CallWebService("POST", url, formdata);
    } 

    async EncryptShareLinkURL(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/EncryptShareLinkURL";
        return CallWebService("POST", url, formdata);
    } 

    async checkFileExists(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/checkFileExists";
        return CallWebService("POST", url, formdata);
    }
    // Survey Repeat Logic

    async GetSurveyRepeatSection(SurveyId) {
        let url = window.SERVER_URL + `/ProjectSurvey/GetSurveyRepeatSection?SurveyId=${SurveyId}`
        return CallWebService("GET", url)
    }

    async GetOutputSurveyIds(actid) {
        var url = window.SERVER_URL + "/ProjectActivity/GetOutputSurveyIds?" + new URLSearchParams({ activityId: actid });
        return CallWebService("GET", url, null);
    }

    async GetOutputPivotPresetList(formdata) {
        var url = window.SERVER_URL + "/ProjectActivity/GetOutputPivotPresetList";
        return CallWebService("POST", url, formdata);
    } 
    async CreateTemplate(formdata) {
        var url = window.SERVER_URL + "/SurveyResponseImport/GetQuestionDetails";
        return CallWebService("POST", url, formdata);
    }

}
