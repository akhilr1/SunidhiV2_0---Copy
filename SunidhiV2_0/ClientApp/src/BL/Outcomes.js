import { CallWebService } from "./WebService.js"

export class Outcomes {

    constructor(obj) {
        this.obj = obj;
    }

    
    async GetAllSDG() {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetAllSDG";
        return CallWebService("GET", url, null);
     }
    async FetchSDGTargetsBasedOnSDGId(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/FetchSDGTargetsBasedOnSDGId";
        return CallWebService("POST", url, formdata);
    }
    async FetchAllSDGTargets() {
        var url = window.SERVER_URL + "/ProjectOutcomes/FetchAllSDGTargets";
        return CallWebService("GET", url, null);
    }
    async FetchAllSDGTargetsIndicators() {
        var url = window.SERVER_URL + "/ProjectOutcomes/FetchAllSDGTargetsIndicators";
        return CallWebService("GET", url, null);
    }
    async FetchAllOutcomeList(projid) {
        var url = window.SERVER_URL + "/ProjectOutcomes/FetchAllOutcomeList?" + new URLSearchParams({ ProjectId: projid});
        return CallWebService("GET", url, null);
    }
    async GetOutcomeActualValue(ocmid) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetOutcomeActualValue?" + new URLSearchParams({ outcomeID: ocmid});
        return CallWebService("GET", url, null);
    }    
    async FetchAllOutcomeListBasedOnOutComeId(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/FetchAllOutcomeListBasedOnOutComeId";
        return CallWebService("POST", url, formdata);
    }

    // Create Outcome
    async CreateOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/CreateOutcome";
        return CallWebService("POST", url, formdata);
    }
    //Update Outcome
    async UpdateOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/UpdateOutcome";
        return CallWebService("POST", url, formdata);
    }
    async UpdateOutcomeWorkflow(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/UpdateOutcomeWorkflow";
        return CallWebService("POST", url, formdata);
    }
    //fnAssignUserstoOutcome
    async fnAssignUserstoOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/fnAssignUserstoOutcome";
        return CallWebService("POST", url, formdata);
    }
    //DeleteOutcomes
    async DeleteOutcomes(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/DeleteOutcomes";
        return CallWebService("POST", url, formdata);
    }


    //New section started on 18-10-2022

    async AddOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/AddOutCome";
        return CallWebService("POST", url, formdata);
    }

    async GetPeriodicityList() {
        var url = window.SERVER_URL + "/Common/GetReportingFrequencyDetails";
        return CallWebService("GET", url);
    }

    async GetLogicType() {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetLogicType";
        return CallWebService("GET", url, null);
    }

    async GetProjectSurveyList(projid) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetProjectSurveyList?" + new URLSearchParams({ ProjectId: projid });
        return CallWebService("GET", url, null);
    }

    async GetSectionsBySurveyID(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetSectionsBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async GetQstnBySectionID(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetQstnBySectionID";
        return CallWebService("POST", url, formdata);
    }

    async GetProjectDetailsByID(formdata) {
        var url = window.SERVER_URL + "/Project/GetProjectDetailsByID";
        return CallWebService("POST", url, formdata);
    }

    async GetUsersListByGroupId(formdata) {
        var url = window.SERVER_URL + "/UserGroups/GetAssignedUsersList";
        return CallWebService("POST", url, formdata);
    }

    async GetAllProjectTags(initid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllProjectTags?" + new URLSearchParams({ InitiativeId: initid });
        return CallWebService("GET", url, null);
    }

    async GetAllExpenseTags(projid) {
        var url = window.SERVER_URL + "/ProjectTags/GetAllExpenseTags?" + new URLSearchParams({ ProjectId: projid });
        return CallWebService("GET", url, null);
    }

    async GetProjectWorkflowsList(formdata) {
        var url = window.SERVER_URL + "/Workflow/GetWorkflowsListByProjectID";
        return CallWebService("POST", url, formdata);
    }

    async GetSurveyBeneficiaryType(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetSurveyBeneficiaryType";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyLinkedBeneficairyQuestionDetails(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetSurveyLinkedBeneficairyQuestionDetails";
        return CallWebService("POST", url, formdata);
    }

    async GetLogicAnswers(QstnID) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetLogicAnswers?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    }

    async GetBeneficiaryQuestionOption(QstnID) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetBeneficiaryQuestionOption?QstnID=" + QstnID;
        return CallWebService("GET", url, null);
    }

    async GetSurveyCascadingList() {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetSurveyCascadingList";
        return CallWebService("GET", url, null);
    }

    async GetCascadingHeirarchyList(qstId, isBeneficiary) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetCascadingHeirarchyList?questionid=" + qstId + "&isBeneficiary=" + isBeneficiary;
        return CallWebService("GET", url, null);
    }

    async RemoveOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/RemoveOutcome";
        return CallWebService("POST", url, formdata);
    }

    async AssignUserToOutcome(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/AssignUserToOutcome";
        return CallWebService("POST", url, formdata);
    }

    async GetOutcomeSurveyIds(outcomeid) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetOutcomeSurveyIds?" + new URLSearchParams({ outcomeid: outcomeid });
        return CallWebService("GET", url, null);
    }


    async GetOutcomePivotPresetList(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/GetOutcomePivotPresetList";
        return CallWebService("POST", url, formdata);
    }

    async SaveOutcomePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/SaveOutcomePivotPreset";
        return CallWebService("POST", url, formdata);
    }

    async RestoreOutcomePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/RestoreOutcomePivotPreset";
        return CallWebService("POST", url, formdata);
    }

    async EditOutcomePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/EditOutcomePivotPreset";
        return CallWebService("POST", url, formdata);
    }

    async DeleteOutcomePivotPreset(formdata) {
        var url = window.SERVER_URL + "/ProjectOutcomes/DeleteOutcomePivotPreset";
        return CallWebService("POST", url, formdata);
    } 

    async GetOutcomeRepeatSection(SurveyId) {
        let url = window.SERVER_URL + `/ProjectSurvey/GetSurveyRepeatSection?SurveyId=${SurveyId}`
        return CallWebService("GET", url)
    }

}
