import { CallWebService } from "./WebService.js"

export class ProjectBeneficiary {
    constructor(obj) {
        this.obj = obj;
    }
    async PostData(url, formdata) {
        return CallWebService("POST", url, formdata);
    }
    //============= Beneficiary  Type====================================
       
    async GetAllBeneficiaryTypes(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetAllBeneficiaryTypes";
        return CallWebService("POST", url, formdata);
    }
    async GetProjectBeneficiaryTypeForFilter(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetProjectBeneficiaryTypeForFilter";
        return CallWebService("POST", url, formdata);
    }
    async GetAnswerTypes() {       
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetAnswerTypes";
        return CallWebService("GET", url, null);
    }
    async GetCascadingQuestionOptions() {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetCascadingQuestionOptions";
        return CallWebService("GET", url, null);
    }
    async GetCascadingQuestionSubOptions(OptionID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetCascadingQuestionSubOptions?ParentOptionID=" + OptionID;
        return CallWebService("GET", url, null);
    }
    async GetCascadingOptionsList(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetCascadingOptionsList";
        return CallWebService("POST", url, formdata);
    }
    async CreateBeneficiaryType(formdata) {     
        var url = window.SERVER_URL + "/ProjectBeneficiary/CreateBeneficiaryType";
        return CallWebService("POST", url, formdata);
    }

    async RemoveBeneficiaryType(formdata) {   
        
        var url = window.SERVER_URL + "/ProjectBeneficiary/RemoveBeneficiaryType";
        return CallWebService("POST", url, formdata);
    }

    async RemoveBeneficiaryTypeQuestion(formdata) {

        var url = window.SERVER_URL + "/ProjectBeneficiary/RemoveBeneficiaryTypeQuestion";
        return CallWebService("POST", url, formdata);
    }

    async UpdateBeneficiaryTypeDetails(formdata) {

        var url = window.SERVER_URL + "/ProjectBeneficiary/UpdateBeneficiaryTypeDetails";
        return CallWebService("POST", url, formdata);
    }
    async UpdateBeneficiaryTypeQuestions(formdata) {

        var url = window.SERVER_URL + "/ProjectBeneficiary/UpdateBeneficiaryTypeQuestions";
        return CallWebService("POST", url, formdata);
    }
    async CopyBeneficiaryQuestion(formdata) {

        var url = window.SERVER_URL + "/ProjectBeneficiary/CopyBeneficiaryQuestion";
        return CallWebService("POST", url, formdata);
    }
    //============= Project Beneficiaries ====================================
    async GetBeneficiaryTypeDetails(BenefTID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetBeneficiaryTypeDetails?BeneficiaryTypeID=" + BenefTID;
        return CallWebService("GET", url, null);
    }
    async GetAllBeneficiaries(BenefTID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetAllBeneficiaries?BeneficiaryTypeID=" + BenefTID;
        return CallWebService("GET", url, null);

    }
        
    // API //
    async FetchAllBeneSurveyQuestDetailAPI(SyncTaskID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/FetchAllBeneSurveyQuestDetailAPI?SyncTaskID=" + SyncTaskID;
        return CallWebService("GET", url, null);
         
    }
    // API //

    async RegisterBeneficiary(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/RegisterBeneficiary";
        return CallWebService("POST", url, formdata);
    }
    async ImportRegisterBeneficiary(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/ImportRegisterBeneficiary";
        return CallWebService("POST", url, formdata);
    }
    async RemoveBeneficiary(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/RemoveBeneficiary";
        return CallWebService("POST", url, formdata);
    }
    async IsUniqueResponse(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/IsUniqueResponse";
        return CallWebService("POST", url, formdata);
    }

    //============= BeneficiaryIndividual ====================================
    
    async GetBeneficiaryDetails(BenefID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetBeneficiaryDetails?BeneficiaryID=" + BenefID;
        return CallWebService("GET", url, null);
    }
    async UpdateBeneficiary(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/UpdateBeneficiary";
        return CallWebService("POST", url, formdata);
    }
    async GetSurveyDetailsByBenefTypeID(BenefTID) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetSurveyDetailsByBenefTypeID?BeneficiaryTypeID=" + BenefTID;
        return CallWebService("GET", url, null);
    }
    
    async GetSurveyResponses(formdata) {
        var url = window.SERVER_URL + "/ProjectSurvey/GetAllResponsesBySurveyID";
        return CallWebService("POST", url, formdata);
    }
    async UpdateBeneficiaryQstnSettings(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/UpdateBeneficiaryQstnSettings";
        return CallWebService("POST", url, formdata);
    }


    //============= ProjectTags====================================
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
    //============= ProjectTags====================================
    async UploadFiles(formData) {

        var url = window.SERVER_URL + "/Document/UplaodQuestionnaireAnsFiles";
        return CallWebService("POST", url, formData);
    }  
    async RemoveQuestionnaireFiles(formData) {

        var url = window.SERVER_URL + "/Document/RemoveQuestionnaireFiles";
        return CallWebService("POST", url, formData);
    }   

    // Download Summary

    async GetBeneficiaryQstnAnsw(BeneficiaryIds) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/GetBeneficiaryQstnAnsw?BeneficiaryIDs=" + BeneficiaryIds;
        return CallWebService("GET", url, null);
    }

    async UpdateQuestionOrder(formdata) {
        var url = window.SERVER_URL + "/ProjectBeneficiary/UpdateQuestionOrder";
        return CallWebService("POST", url, formdata);
    }
}