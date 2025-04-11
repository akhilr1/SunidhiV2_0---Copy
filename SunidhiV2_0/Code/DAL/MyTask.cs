
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{

    public class MyTask
    {
        //==== MyTaskList===============
        public int SyncDBID { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string TypeIDColumn { get; set; }
        public string Status { get; set; }
        public int WorkflowID { get; set; }
        public int TotalWFLevel { get; set; }
        //public int SubmittedWFLevel { get; set; }
        //public int SubmittedUser { get; set; }
        public int WFLevel { get; set; }
        public string WFLevelStatus { get; set; }
        public int ResponseSyncID { get; set; }
        public int DataCollectedBy { get; set; }
        public string TaskData { get; set; }
        //public string TaskHistory { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
        public int FinanceWorkflowId { get; set; }
        public int REQUESTID { get; set; }
        // =- Risk --- //
        public int ReportingValue { get; set; }
        // --- Fund Request --- 
        public string CommittedBy { get; set; }
        public double CommittedFund { get; set; }

        //==== TaskSyncDB===============

        public string Command { get; set; }
        public string ClientData { get; set; }

        //=========MyTask======================

        public int UserID { get; set; }
        public string TaskUpdateComment { get; set; }
        public string AttachedDocumentName { get; set; }

        public string SurveyAnswers { get; set; }
        public string NewBeneficiaries { get; set; }
        public int WorkspaceID { get; set; }
        public string TaskdataAddedBy { get; set; }
        public string DataCollectorIds { get; set; }
        public string ApproverIds { get; set; }
        //public int PeriodicityId { get; set; }
        //public DateTime PeriodicityDate { get; set; }

        ////=========Completed======================
        //public string TaskName { get; set; }
        //public string Description { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public string ProjectName { get; set; }
        ////=========AuditLog======================
        //public string Name { get; set; }
        //public string Message { get; set; }
        //public int CreatedBy { get; set; }
        public int DataCollectionSyncDBID { get; set; }
        public string AssignedTo { get; set; }
        public string WorkflowStage { get; set; }
        public string ActionRequired { get; set; }
        public string NextLevelAssignedUsers { get; set; }
        public string DataCollectorName { get; set; }
        public string AssignedToIds { get; set; }
        public string NextLevelAssignedUsersIds { get; set; }
        public int RejectDataCollectionSyncDBID { get; set; }
        public DateTime TaskAddedDate { get; set; }
        public int CurrentUserWFLevel { get; set; }
        public int ResponseCount { get; set; }
        public int BeneficiaryCount { get; set; }
        public int AnswerCount { get; set; }
        public int QuestionCount { get; set; }
        public int TotalCount { get; set; }
    }
    public class CompletedTask
    {
        public string Type { get; set; }
        public int TypeID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int WorkflowID { get; set; }
        public string WFLevelStatus { get; set; }
        public int WFLevel { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectName { get; set; }

        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
        public string Periodicity { get; set; }
        public int PeriodicityId { get; set; }
        public string TaskSubmittedBy { get; set; }
        public string TaskUpdateComment { get; set; }
        public int SyncDBID { get; set; }
    }
    public class TaskAuditLog
    {
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string WFLevelStatus { get; set; }
        public int WFLevel { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectName { get; set; }

        public string Name { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        public string TaskUpdateComment { get; set; }
        public string AttachedDocumentName { get; set; }
        public string  ProfileImage { get; set; }
        public string CreatedDateStr { get; set; }
        public string UniqueDocumentName { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTypeIcon { get; set; }
        public int TaskSyncDbID { get; set; }
    }

    public class UpdatedTaskList
    {
        public int TypeID { get; set; }
        public string Type { get; set; }
        public DateTime TaskDate { get; set; }
        public string WFLevelStatus { get; set; }
        public int WFLevel { get; set; }
    }

    public class SurveyDataCollection
    {
        public int DataCollectionSyncDBID { get; set; }
        public int SyncTaskID { get; set; }
        public int RejectId { get; set; }
    }
}


/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Code.DAL
{

    public class MyTask
    {
       
        //==== MyTaskList===============
        public int SyncDBID { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string TypeIDColumn { get; set; }
        public string Status { get; set; }
        public int WorkflowID { get; set; }
        public int TotalWFLevel { get; set; }
        public int SubmittedWFLevel { get; set; }
        public int SubmittedUser { get; set; }
        public int WFLevel { get; set; }
        public string WFLevelStatus { get; set; }
        public int ResponseSyncID { get; set; }
        public int DataCollectedBy { get; set; }
        public string TaskData { get; set; }
        public string TaskHistory { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
        public int FinanceWorkflowId { get; set; }
        public int REQUESTID { get; set; }
        //==== TaskSyncDB===============

        public string Command { get; set; }
        public string ClientData { get; set; }

        //=========MyTask======================

        public int UserID { get; set; }
        public string TaskUpdateComment { get; set; }
        public string AttachedDocumentName { get; set; }

        public string SurveyAnswers { get; set; }
        public string NewBeneficiaries { get; set; }
        //public int PeriodicityId { get; set; }
       // public DateTime PeriodicityDate { get; set; }
                       
        //=========Completed======================
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectName { get; set; }
        //=========AuditLog======================
        public string Name { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        // =- Risk --- //
        public int ReportingValue { get; set; }
        // --- Fund Request --- 
        public string CommittedBy { get; set; }
        public double CommittedFund { get; set; }
    }
    public class CompletedTask
    {
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string WFLevelStatus { get; set; }
        public int WFLevel { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectName { get; set; }


    }
}
   */
