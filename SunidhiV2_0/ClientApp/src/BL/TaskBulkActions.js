import { CallWebService } from "./WebService.js";

export default class TaskBulkActions {

    async getPendingTasks(props) {
        let {
            UserID,
            SyncServerID,
            WorkspaceID,
            TypeID
        } = props
        let url = window.SERVER_URL + "/BulkApprove/GetAllApprovalDataBulk";
        let formData = new FormData
        formData.append("UserID", UserID)
        formData.append("SyncServerID", 0)
        formData.append("WorkspaceID", WorkspaceID)
        formData.append("TypeID", TypeID)

        try {
            let {status, data} = await CallWebService("POST", url,  formData)
            if(status !== 'success') {
                throw new Error('Invalid Response')
            }
            else if(status === 'success') {
                return data.MyTaskList
            }
        } catch (error) {
            console.error(error)
            return []
        }
    }

    async updateBulkData(gridData) {

        let formData = new FormData
        formData.append("gridData", JSON.stringify(gridData));
        let url = window.SERVER_URL + "/BulkApprove/UpdateBulkData";

        try {
            let response = await CallWebService("POST", url, formData)
            if(response.data) return response.data

            // need better error handling
        } catch (error) {
            throw error
        }
    }

    async authorizeTasks(payload) {
        let { UserID, TaskSyncData, isBulk } = payload;

        console.log(JSON.stringify(TaskSyncData))
        const url = window.SERVER_URL + "/MobileWebService/SyncTaskToServer";

        let formData = new FormData()

        formData.append("loggedUserID", UserID);
        formData.append("isBulk", isBulk);
        for(let i = 0; i < TaskSyncData.length; i++) {
            formData.append('lstMyTaskSyncToServerData[' + i + '].Type', TaskSyncData[i].Type);
            formData.append('lstMyTaskSyncToServerData[' + i + '].WFLevel', TaskSyncData[i].WFLevel);
            formData.append('lstMyTaskSyncToServerData[' + i + '].IsBeneficiaryLinked', TaskSyncData[i].IsBeneficiaryLinked);
            formData.append('lstMyTaskSyncToServerData[' + i + '].ClientData', TaskSyncData[i].ClientData);
            formData.append('lstMyTaskSyncToServerData[' + i + '].Periodicity', TaskSyncData[i].Periodicity);
            formData.append('lstMyTaskSyncToServerData[' + i + '].ClientGuid', TaskSyncData[i].ClientGUID);
        }

        try {
            let result = await CallWebService("POST", url, formData)
            return result
        } catch (error) {
            console.error(error)
            throw error
        }
    }

    async getApprovalResponses(props) {
        const url = window.SERVER_URL + `/BulkApprove/GetApprovalResponse`;
        let {
            TypeID,
            SyncTaskID,
            WorkspaceID
        } = props
        try {
            let formData = new FormData
            formData.append("TypeID", TypeID);
            formData.append("syncTaskID", SyncTaskID);
            formData.append("workspaceId", WorkspaceID);

            let result = await CallWebService("POST", url, formData)
            
            if(result.status === 'success') {
                return result.data
            }
            else {
                throw new Error('Invalid Response')
            }
        } catch (error) {
            console.error(error)
            throw error
        }
    }

    async getSurveyResponsesByID(props) {
        let {SurveyID} = props

        const url = window.SERVER_URL + `/ProjectSurvey/GetResponsesBySurveyID`;
        try {
            let formData = new FormData()
            formData.append('SurveyID', SurveyID)
            let result = await CallWebService('POST', url, formData)
            return result
        } catch (error) {
           console.error(error)
           throw error 
        }
    }

    async deleteSurveyResponses(syncTaskIDs) {
        try {
            const formData = new FormData() 

            formData.append("gridData", JSON.stringify(syncTaskIDs));

            const url = window.SERVER_URL + `/BulkApprove/DeleteBulkData`;

            let result = await CallWebService("POST", url, formData)
            return result.data
        } catch (error) {
            throw error
        }
    }
    
    async getTaskActivityLog(syncIDList) {
        console.log(JSON.stringify(syncIDList))
        const url = window.SERVER_URL + `/BulkApprove/GetActivityLog`;

        const formData = new FormData();

        formData.append("syncTaskID", syncIDList.join(','));

        try {
            let response = await CallWebService("POST", url, formData);
            return response
        } catch (error){
            console.error(error)
            throw error;
        }
    }

    async fetchAllBenefQnA(syncTaskID) {
        const url = `${window.SERVER_URL}/ProjectBeneficiary/FetchAllBeneSurveyQuestDetailAPI?SyncTaskID=${syncTaskID}`;

        try {
            const result = await CallWebService("GET", url, null);
            if(result.status === 'Success') {
                return result.data
            } else {
                throw new Error('Invalid Response')
            }
        } catch (error) {
            console.error(error)
            throw error
        }
    }

}