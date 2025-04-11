import { CallWebService } from "./WebService.js"

export class ProjectOutcomes {

    constructor(obj) {
        this.obj = obj;
    }
    
    // Report Project Outcomes Callees

    async GetOutComeProjectData(WorkspaceID) {
        let url = window.SERVER_URL + `/ProjectOutcomes/GetOutComeProjectData?WorkspaceID=${WorkspaceID}`;
        return CallWebService("GET", url, null)
    }

    async getOutcomeListByProjectId(ProjectId, mStartDate, mEndDate) {
        let url = window.SERVER_URL + `/ProjectOutcomes/GetOutComeListByProjectId?ProjectId=${ProjectId}&mStartDate=${mStartDate}&mEndDate=${mEndDate}`;
        return CallWebService("GET", url, null)
    }

    async GetOutcomeSurveyIds(outcomeid) {
        let url = window.SERVER_URL + `/ProjectOutcomes/GetOutcomeSurveyIds?outcomeid=${outcomeid}`;
        return CallWebService("GET", url, null)
    }

    async GetOutcomePivotPresetList(outcomeid) {
        let url = window.SERVER_URL + `/ProjectOutcomes/GetOutcomePivotPresetList?outcomeid=${outcomeid}`;
        return CallWebService("GET", url, null)
    }
    
    async GetPivotTableData(surveyId) {
        let url = window.SERVER_URL + `/ProjectOutcomes/GetPivotTableData?url=getpivotdata?surveyid=${surveyId}`;
        return CallWebService("GET", url, null)
    }

}