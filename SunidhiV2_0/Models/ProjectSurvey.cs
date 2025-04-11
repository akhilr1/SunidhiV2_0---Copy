using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace SunidhiV2_0.Models
{
    public class ProjectSurvey
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int ProjectId { get; set; }
        public int WorkflowID { get; set; }
        public int ReportingFrequency { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int ActivityID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string AssignedUsers { get; set; }
        public string AssignedTags { get; set; }
        public int ResponseCount { get; set; }
        public string WorkFlowName { get; set; }
        public string BeneficiaryTypeName { get; set; }
        public string ReportingFrequencyName { get; set; }
        public bool Notify { get; set; }

        public int ReportingMonth { get; set; }
        public int ReportingMonthDay { get; set; }
        public int ReportingWeek { get; set; }
        public int ReportingMonthDateDay { get; set; }
        public string ReportingOnceDate { get; set; }

        public List<Tags> SurveyTags { get; set; }
        public List<UsersList> SurveyAssignedUsers { get; set; }

        public List<ProjectSurveySection> SectionList { get; set; }
        public string ProfileImage { get; set; }

        public int SurveyTotResponse { get; set; }
        public int SurveyTotComplete { get; set; }

        public int SurveyQuestionTotResponse { get; set; }
        public string SurveyPeriodicity { get; set; }
        public string SurveyBeneficiary { get; set; }
        public string QuestionCreateDate { get; set; }
        public string QuestionUpdateDate { get; set; }
        public int SurveyProgress { get; set; }
        public string EnumName { get; set; }
        public string EnumImage { get; set; }

        public string SectionName { get; set; }
        public int SurveyTotResponseCount { get; set; }
        public int SurveyTotCompleteCount { get; set; }
        public int TotalWFLevel { get; set; }
        public int WorkspaceID { get; set; }
    }

    public class ProjectSurveySection
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public int SectionOrder { get; set; }
        public int SurveyID { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string SectionORBeneficiary { get; set; }
        public List<SurveyQuestion> SurveySecQuestions { get; set; }
        public int BeneficiaryTypeID { get; set; }
    }

    public class SurveyQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int QuestionTypeID { get; set; }
        public string QuestionType { get; set; }
        public int QuestionCategory { get; set; }
        public int QuestionOrder { get; set; }

        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public string QuestionHint { get; set; }
        public string DefaultValue { get; set; }
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsMandatory { get; set; }

        public bool IsFormulaQuestion { get; set; }
        public string FormulaExpression { get; set; }
        public string FormulaOperation { get; set; }
        public string FormulaFieldVariables { get; set; }
        public string FieldVar { get; set; }

        public bool IsConditionQuestion { get; set; }
        public string RegEx { get; set; }
        public string ErrorText { get; set; }
        public string ValidationConditions { get; set; }
        public bool IsValidationQuestion { get; set; }
        public string UOM { get; set; }

        public bool IsRepeatSection { get; set; }
        public int RepeatSectionId { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int TextLength { get; set; }
        public string XMLTag { get; set; }

        public string HierarchyQuestionOrder { get; set; }
        public string RepeatCondition { get; set; }

        public bool IsCalcavailable { get; set; }
        public bool IsValavailable { get; set; }
        public List<QuestionCondition> lstQuestionCondition { get; set; }
        public List<QuestionOptionList> lstQuestionOptions { get; set; }
        public List<SurveySectionLogic> lstSurveySectionLogic { get; set; }
        public bool IsOtherOptionSelected { get; set; }
    }
    public class SurvayResponse
    {
        public int SyncDBID { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string TypeIDColumn { get; set; }
        public string Status { get; set; }
        public int WorkflowID { get; set; }
        public int TotalWFLevel { get; set; }
        public int WFLevel { get; set; }
        public string WFLevelStatus { get; set; }
        public string TaskUpdateComment { get; set; }
        public string AttachedDocumentName { get; set; }
        public string SurveyAnswers { get; set; }
        public int ResponseSyncID { get; set; }
        public int DataCollectedBy { get; set; }
        public string NewBeneficiaries { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
        public int WorkspaceID { get; set; }
    }
    public class QuestionCondition
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string ValidationCondition { get; set; }
        public string ConditionValue { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int LogicOperator { get; set; }
    }
    public class QuestionOptionList
    {
        public int OptionID { get; set; }
        public int QuestionID { get; set; }
        public string OptionValue { get; set; }
        public string OptionLabel { get; set; }

    }
    public class SurveySectionLogic
    {
        public int SectionLogicID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public string LogicType { get; set; }
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionType { get; set; }
        public int ConditionID { get; set; }
        public string Condition { get; set; }
        public string Answer { get; set; }
        public int LogicOperator { get; set; }
        public List<QuestionOptionList> lstQuestionOptions { get; set; }
        public List<QuestionCondition> lstQuestionCondition { get; set; }
    }
    public class ExcelSurvayQuestion
    {
        public int BeneficiaryInd { get; set; }
        public int ResponseID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public string FieldVar { get; set; }
        public int RQstInd { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }

    }
    public class LogicConditions
    {
        public int ConditionID { get; set; }
        public string Condition { get; set; }
        public string ConditionValue { get; set; }
    }
    public class QuestionTypes
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int QuestionCategory { get; set; }
        public bool IsCalcavailable { get; set; }
        public bool IsValavailable { get; set; }
        public bool IsPrimeLogic { get; set; }
        public bool IsNumeric { get; set; }
        public bool IsNonNumeric { get; set; }
    }
    public class CascadingQuestionOptions
    {
        public int ID { get; set; }
        public int OptionGroupID { get; set; }
        public string OptionValue { get; set; }
        public int HierarchyOrder { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string PrimaryColumn { get; set; }
        public string ParentItemColumn { get; set; }

    }


    public class HierarchyGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }

    public class MasterHierarchy
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string TableName { get; set; }
        public string HierarchyOrder { get; set; }
        public string GroupId { get; set; }
        public string PrimaryKeyId { get; set; }
        public string FieldName { get; set; }
        public string ParentColumnId { get; set; }


    }

    public class BeneficiaryType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CreatedDate { get; set; }
        public bool IsSelected { get; set; }
    }
    public class SurveyResponse
    {

        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public int ResponseID { get; set; }
        public string SurveyName { get; set; }
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionType { get; set; }
        public List<AnswerList> AnswerList { get; set; }
        public List<SurveyResponses> QuestionResponses { get; set; }
        public List<QuestionOptionList> QuestionOptionList { get; set; }


        public class SurveyResponseDetails
        {
            public List<MyTaskSurveyQuestion> lstMyTaskSurveyQuestion { get; set; }
            public List<MyTaskQuestionType> lstMyTaskQuestionType { get; set; }
            public List<MyTaskBeneficiaryTypeQuestion> lstMyTaskBeneficiaryTypeQuestion { get; set; }
            public List<MyTaskSurveySectionLogic> lstMyTaskSurveySectionLogic { get; set; }
            public List<TbMyTaskBeneficiaryRegistrationDetails> lstTbMyTaskBeneficiaryRegistrationDetails { get; set; }
            public List<TbMyTaskProjectBeneficiaryType> lstTbMyTaskProjectBeneficiaryType { get; set; }
            public List<MyTaskProjectSurveyQSTCondition> lstMyTaskProjectBenSurveyQSTCondition { get; set; }
            public List<MyTaskBeneficiarySurveyQSTOption> lstMyTaskBeneficiarySurveyQSTOption { get; set; }
            public List<MyTaskSurveyCascadingQuestionOptions> lstMyTaskBeneficiarySurveyCascadingQuestionOptions { get; set; }
            public List<MyTaskProjectSurveyQSTOption> lstMyTaskProjectSurveyQSTOption { get; set; }
            public List<MyTaskResponseDetails> MyTaskResponseDetails { get; set; }
            public List<MyTaskSummary> MyTaskSummary { get; set; }
        }
    }
    public class MyTaskSummary
    {
        public DateTime SubmittedDateTime { get; set; }
        public string SubmittedBy { get; set; }

        public int SurveyID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int WorkflowID { get; set; }
        public int SyncTaskID { get; set; }
        public int WFLevel { get; set; }
    }
    public class MyTaskResponseDetails
    {
        public int SyncTaskID { get; set; }
        public List<SurveyQuestions> SurveyQuestions { get; set; }
    }
    public class SurveyResponses
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int ResponseID { get; set; }
        public int SyncTaskID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionType { get; set; }
        public string Answer { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public int SubmissionCount { get; set; }
        public int BeneficiaryInd { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
    }
    public class AnswerList
    {
        public int count { get; set; }
        public string Answer { get; set; }
        public int QuestionID { get; set; }
        public int ResponseID { get; set; }

    }
    public class TaskProgress
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int ResponseID { get; set; }
        public int SyncTaskID { get; set; }
        public List<TaskProgressQuestions> Questions { get; set; }

    }
    public class TaskProgressQuestions
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int ResponseID { get; set; }
        public int SyncTaskID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionType { get; set; }
        public string Answers { get; set; }
        public string Questions { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }

    }

    public class PivotPresetData
    {
        public string PresetName { get; set; }
        public string PresetData { get; set; }
        public int SectionId { get; set; }
    }

    public class SurveyQuestionVariable
    {
        public string NewVariable { get; set; }
        public string OldVariable { get; set; }
    }

    public class SurveyResponseGPSLocation
    {
        public int SurveyID { get; set; }
        public int SyncTaskID { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public int SubmissionCount { get; set; }
        public string GPS { get; set; }
    }

    public class ProjectSurveyCollection
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public List<ProjectSurveyCollection> ProjectSurveyList { get; set; }
        public List<ProjectSurveySectionCollection> ProjectSurveySectionList { get; set; }
    }

    public class ProjectSurveySectionCollection
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public List<ProjectSurveySectionCollection> ProjectSurveySectionList { get; set; }
        public List<ProjectSurveySectionQuestionsCollection> ProjectSurveySectionQuestionsList { get; set; }
    }

    public class ProjectSurveySectionQuestionsCollection
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int? QuestionTypeID { get; set; }
        public string Type { get; set; }
        public string SectionORBeneficiary { get; set; }
    }

    public class MasterSurveyCollection
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int? QuestionTypeID { get; set; }
        public string Type { get; set; }
        public string SectionORBeneficiary { get; set; }
    }

    public class TotalCompletedCount
    {
        public int SurveyID { get; set; }
        public int SyncTaskID { get; set; }
        public int WorkflowID { get; set; }
    }

    public class TotalCompletedResposeCount
    {
        public int submissionCount { get; set; }
        public int ResponseID { get; set; }
    }
}