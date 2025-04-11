using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SunidhiV2_0.Models
{
    public class Project
    {
       
        public int InitiativeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime  EndDate { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public string ProjectTags { get; set; }
        public int GroupId { get; set; }
        public int ProjectTemplateID { get; set; }
        public int WorkspaceID { get; set; }       
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string PaymentTermAmount { get; set; }
        public string Initiative { get; set; }
        public string FundedAmount { get; set; }
        public string FundUtilized { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string BudgetHeadAmount { get; set; }
        public string UserList { get; set; }
        public double Progress { get; set; }
        public string ProjectStatus { get; set; }
        public string ProfileImage { get; set; }
        public int ProjectCunt { get; set; }
        public int TotalBeneficiary { get; set; }
        public List<AssignedList> AssignedList { get; set; }
        public List<ActiveList> ActiveList { get; set; }        
        public int CompletedCount { get; set; }
        public List<ProjectStatusDetail> ProjectStatusDetail { get; set; }
        public List<ProjectList> ProjectList { get; set; }
        public int ProjectSummaryCount { get; set; }
        public int ArchivedCount { get; set; }
        public int DeployCount { get; set; }
        public int DraftCount { get; set; }
        public int CompCount { get; set; }
        public string ExecutedDateTime { get; set; }
        public string AssignedUserListDuringProjectCreation { get; set; }
        //public List<AssignedUserListDuringProjectCreation> AssignedUserListDuringProjectCreation { get; set; }
    }


    public class ProjectList
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public double Progress { get; set; }
    }

    public class AssignedList
    {
        public int ProjectId { get; set; }
        public int AssignedCunt { get; set; }
    }

    public class ActiveList
    {
        public int ProjectId { get; set; }
        public int ActiveCunt { get; set; }
    }

    public class CompletedList
    {
        public int ProjectId { get; set; }
        public int CompletedCount { get; set; }
    }

    public class MyTaskList
    {
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
        public string AssignedDate { get; set; }
        public string TaskFrequency { get; set; }
        public string AssignedByUser { get; set; }
        public string Beneficiary { get; set; }
        public string Status { get; set; }
        public int TypeID { get; set; }
        public int ProjectId { get; set; }
        public int SubmittedCount { get; set; }

    }

    public class ProjectStatusDetail
    {
        public int StatusCunt { get; set; }
        public string StatusName { get; set; }


    }

    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    public class State
    {
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
    public class District
    {
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
    }
    public class Document
    {
        public int ProjectId { get; set; }

        public int DocumentID { get; set; }
        public string UniqueDocumentName { get; set; }
        public string DocumentName { get; set; }
        public string DocumentOriginalName { get; set; }
        public string DocumentSize { get; set; }
        public string DocumentTypeIcon { get; set; }
        public string DocumentType { get; set; }

        public string DocumentLastModifiedDate { get; set; }
        public DateTime UploadedDate { get; set; }
        public int UploadedBy { get; set; }
        public string UploadedUser { get; set; }
        public string UploadedFrom { get; set; }
        public string LinkedTo { get; set; }
        public bool IsDeleted { get; set; }
        public int LinkedToID { get; set; }
        public int TaskSyncID { get; set; } = 0;
        public string ResponseFrom { get; set; } // For Questionnaire 
        public string ProfileImage { get; set; }
        public string OriginalFileName { get; set; }
    }    

    public class ProjectBudgetAndFundsDetails
    {
        public int ProjectID { get; set; }
        public int GroupId { get; set; }
        public double Budget { get; set; }
        public string ProjectUsers { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string UserType { get; set; }

        public double DonorBudgetAmount { get; set; } = 0;
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public bool IsBudgetAmtAdded { get; set; } = false;
        public int BudgetFundID { get; set; }
        public string ProfileImage { get; set; }
    }
    public class ProjectBudgetPlan
    {
        public int ProjectID { get; set; }
        public int BudgetHeadID { get; set; }
        public string BudgetHead { get; set; }
        public double HeadAmount { get; set; }
        public string BudgetPlanAmount { get; set; }
        public string BudgetPlanSubItems { get; set; }       
        public List<ProjectBudgetPlanDetails> BudgetPlanSubList { get; set; }
        public List<ProjectPaymentList> FundPaymentList { get; set; }
    }
    public class ProjectBudgetPlanDetails
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int BudgetHeadID { get; set; }
        public string ItemName { get; set; }
        public double Amount { get; set; }
        
    }
    public class PaymentTerms
    {

        public int ProjectId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentTermName { get; set; }
        public string Logic { get; set; }
        public string Target { get; set; }
        public DateTime paymentdate { get; set; }
        public string Amount { get; set; }
        public string PaymentStatus { get; set; }
        public List<UsersDetails> UserList { get; set; }
        public List<ProjectBudgetAndFundsDetails> NotifyUserList { get; set; }
        public string users { get; set; }
        public bool Notify { get; set; }
        public string Color { get; set; }
        


    }
	public class ProjectPaymentList
    {
        public int ProjectId { get; set; }
        public int PaymentId { get; set; }
        public string Title { get; set; }
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }      
        public string TransactionDocName { get; set; }
        public int BudgetHeadID  { get; set; }
        public string BudgetHead { get; set; }   
        public List<ProjectExpenseTags> ExpenseTags { get; set; }
       
    }
    public class Risk
    {
        public int ProjectID { get; set; }
        public int riskId { get; set; }
        public string RiskName { get; set; }
        public string Description { get; set; }
        public int RiskPeriodicity { get; set; }
        public int RiskRating { get; set; }
        public string RiskType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkflowID { get; set; }
        public string WorkflowName { get; set; }
        public List<UsersDetails> UserList { get; set; }
        public string Users { get; set; }
        public string ReportingFrequencyName { get; set; }
        public int ReportingMonth { get; set; }
        public int ReportingMonthDay { get; set; }
        public int ReportingWeek { get; set; }
        public int ReportingMonthDateDay { get; set; }
        public string ReportingOnceDate { get; set; }
        public string ProfileImage { get; set; }

    }
    public class UsersDetailsRisk
    {
        public List<UsersDetails> UsersDetail { get; set; }
    }
    public class ActivityLog
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public string ProfileImage { get; set; }

    }
    public class KPI
    {
        public int ProjectID { get; set; }
        public int KPIId { get; set; }
        public string KPIName { get; set; }
        public string Description { get; set; }
        public int KPIPeriodicity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkflowID { get; set; }
        public string WorkflowName { get; set; }
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public string UpdatedDate { get; set; }
        public List<UsersDetails> UserList { get; set; }
        public string Users { get; set; }
        public string Tags { get; set; }
        public string ReportingFrequencyName { get; set; }
        public int ReportingMonth { get; set; }
        public int ReportingMonthDay { get; set; }
        public int ReportingWeek { get; set; }
        public int ReportingMonthDateDay { get; set; }
        public string ReportingOnceDate { get; set; }
    }
    public class ProjectAuditLog
    {
        public int LogID { get; set; }
        public int ProjectID { get; set; }        
        public string AuditFrom { get; set; }
        public string AuditEvent { get; set; }
        public int AuditFromID { get; set; }
        public string FromIDColumnName { get; set; }       
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        

    }

    public class AssignedUserListDuringProjectCreation
    {
        public string EmailId { get; set; }
        public string UserRole { get; set; }
        public int WorkspaceID { get; set; }
        public string TypeOfUser { get; set; }
        public int? UserId { get; set; }
    }
}
