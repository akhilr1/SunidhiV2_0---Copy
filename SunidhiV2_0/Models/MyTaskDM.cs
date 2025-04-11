using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class MyTaskDM
    {
        public int SyncDBID { get; set; }
        public string Command { get; set; }
        public int TypeID { get; set; }
        public string Type { get; set; }
        public string TypeIDColumn { get; set; }
        public string SurveyName { get; set; }
        public string PeriodicityType { get; set; }
        public bool DataCollection { get; set; }
        public int WorkflowID { get; set; }
        public int TotalWorkflow { get; set; }
        public int WorkFlowLevel { get; set; }
        public int BeneficiaryTypeID { get; set; }
    }

    public class MyTaskSurveySection
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public int SectionOrder { get; set; }
        public int SurveyID { get; set; }
    }

    public class MyTaskSurveyQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int QuestionTypeID { get; set; }
        public int QuestionOrder { get; set; }
        public string UOM { get; set; }
        public string DefaultValue { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsConditionQuestion { get; set; }
        public bool IsRepeatSection { get; set; }
        public int RepeatSectionId { get; set; }
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public bool IsActive { get; set; }
        public string RegEx { get; set; }
        public bool IsMandatory { get; set; }
        public string TextLength { get; set; }
        public string XMLTag { get; set; }
        public string ErrorText { get; set; }
        public string FieldVar { get; set; }
        public bool IsFormulaQuestion { get; set; }
        public string FormulaExpression { get; set; }
        public string FormulaOperation { get; set; }
        public string FormulaFieldVariables { get; set; }
        public string HierarchyQuestionOrder { get; set; }
        public string RepeatCondition { get; set; }
        public string ValidationCondition { get; set; }
        public string QuestionHint { get; set; }
        public bool IsValidationQuestion { get; set; }
        public bool IsOtherOptionSelected { get; set; }
    }

    public class MyTaskSurveySectionLogic
    {
        public int SectionLogicID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public string LogicType { get; set; }
        public int QuestionID { get; set; }
        public int ConditionID { get; set; }
        public string Answer { get; set; }
        public int LogicOperator { get; set; }
    }

    public class MyTaskSurveyTrackDate
    {
        public int SurveyID { get; set; }
        public DateTime TrackDate { get; set; }
    }

    public class TbMyTaskTable
    {
        public int SyncTaskID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string WFLevelStatus { get; set; }
        public int WFLevel { get; set; }
        public DateTime TaskDate { get; set; }
        public DateTime TaskSubmittedDate { get; set; }
        public string TaskUpdateComment { get; set; }
        public int ParentSyncDBID { get; set; }
        public int DataCollectionSyncDBID { get; set; }
    }

    public class MyTaskProjectSurveyQSTCondition
    {
        public int QuestionID { get; set; }
        public string ValidationCondition { get; set; }
        public string ConditionValue { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string RegEx { get; set; }
        public string ErrorText { get; set; }
        public int? LogicOperator { get; set; }
        public int SurveyID { get; set; }
        public int BeneficiaryTypeID { get; set; }
    }

    public class MyTaskProjectSurveyQSTOption
    {
        public int OptionID { get; set; }
        public string OptionValue { get; set; }
        public int QuestionID { get; set; }
        public bool IsOptSelected { get; set; }
        public int SurveyID { get; set; }
    }

    public class MyTaskBeneficiarySurveyQSTOption
    {
        public int QstnAnsOptionID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerTypeID { get; set; }
        public string OptionValue { get; set; }
    }

    public class MyTaskSurveyCascadingQuestionOptions
    {
        public int OptionGroupID { get; set; }
        public string OptionValue { get; set; }
        public int HierarchyOrder { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string PrimaryColumn { get; set; }
        public string ParentItemColumn { get; set; }
    }

    public class TbMyTaskProjectLogicConditions
    {
        public int ID { get; set; }
        public string Condition { get; set; }
        public string ConditionValue { get; set; }
    }

    public class TbMyTaskProjectBeneficiaryType
    {
        public int BeneficiaryTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public int SurveyID { get; set; }
    }


    public class TbMyTaskBeneficiaryAnswerType
    {
        public int AnswerTypeID { get; set; }
        public string AnswerType { get; set; }
    }

    public class TbMyTaskBeneficiaryRegistrationDetails
    {
        public int BeneficiaryRegID { get; set; }
        public int BeneficiaryID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerTypeID { get; set; }
        public string Answer { get; set; }
        public int BeneficiaryTypeID { get; set; }
    }

    public class TbMyTaskProjectBeneficiary
    {
        public int BeneficiaryID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public string BeneficiaryName { get; set; }
        public string CreatedDate { get; set; }
        public string TokenValue { get; set; }
    }

    public class MyTaskBeneficiaryTypeQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int AnswerTypeID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsMandatoryResponse { get; set; }
        public string RegexExpression { get; set; }
        public string QuestionHint { get; set; }
        public bool IsValidationQuestion { get; set; }
        public int QuestionOrder { get; set; }
    }

    public class MyTaskMasterList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ItemTable { get; set; }
        public int ParentID { get; set; }
        public int CensusCode { get; set; }
    }


    public class MyTaskQuestionType
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int Category { get; set; }
    }

    public class MyTaskSurveyResponse
    {
        public int ID { get; set; }
        public string Answer { get; set; }
        public int ResponseID { get; set; }
        public int QuestionID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int OptionID { get; set; }
        public int RepeatOrder { get; set; }
        public string SubRepeatOrder { get; set; }
        public int SyncTaskID { get; set; }
    }

    public class ListSyncItem
    {
        public List<MyTaskBeneficiaryTypeQuestion> lstMyTaskBeneficiaryTypeQuestion { get; set; }
        public List<TbMyTaskProjectBeneficiary> lstTbMyTaskProjectBeneficiary { get; set; }
        public List<TbMyTaskBeneficiaryRegistrationDetails> lstTbMyTaskBeneficiaryRegistrationDetails { get; set; }
        public List<TbMyTaskBeneficiaryAnswerType> lstTbMyTaskBeneficiaryAnswerType { get; set; }
        public List<TbMyTaskProjectBeneficiaryType> lstTbMyTaskProjectBeneficiaryType { get; set; }
        public List<TbMyTaskProjectLogicConditions> lstTbMyTaskProjectLogicConditions { get; set; }
        public List<MyTaskSurveyCascadingQuestionOptions> lstMyTaskSurveyCascadingQuestionOptions { get; set; }
        public List<MyTaskProjectSurveyQSTOption> lstMyTaskProjectSurveyQSTOption { get; set; }
        public List<MyTaskProjectSurveyQSTCondition> lstMyTaskProjectSurveyQSTCondition { get; set; }
        public List<TbMyTaskTable> lstTbMyTaskTable { get; set; }
        public List<MyTaskSurveyTrackDate> lstMyTaskSurveyTrackDate { get; set; }
        public List<MyTaskSurveySectionLogic> lstMyTaskSurveySectionLogic { get; set; }
        public List<MyTaskSurveyQuestion> lstMyTaskSurveyQuestion { get; set; }
        public List<MyTaskSurveySection> lstMyTaskSurveySection { get; set; }
        public List<MyTaskDM> lstMyTaskDM { get; set; }
        public List<MyTaskMasterList> lstMyTaskMasterList { get; set; }
        public List<MyTask> lstMyTaskList { get; set; }
        public List<MyTaskQuestionType> lstMyTaskQuestionType { get; set; }
        public List<MyTaskSurveyResponse> lstMyTaskSurveyResponse { get; set; }
        public List<MyTaskProjectSurveyQSTCondition> lstMyTaskProjectBenSurveyQSTCondition { get; set; }
        public List<MyTaskBeneficiarySurveyQSTOption> lstMyTaskBeneficiarySurveyQSTOption { get; set; }
        public List<MyTaskSurveyCascadingQuestionOptions> lstMyTaskBeneficiarySurveyCascadingQuestionOptions { get; set; }
    }

    public class MyTaskSyncToServerData
    {
        public string Type { get; set; }
        public int WFLevel { get; set; }
        public bool IsBeneficiaryLinked { get; set; }
        public string ClientData { get; set; }
        public string Periodicity { get; set; }
        public string ClientGuid { get; set; }
        public List<Document> lstBenRegistration { get; set; }
        public List<Document> lstSurveyResponse { get; set; }
    }

    public class MyTaskSyncToServerStatus
    {
        public int TaskSyncID { get; set; }
        public string Status { get; set; }
        public string ClientGuid { get; set; }
    }

    public class MyTaskActivityLog
    {
        public DateTime SubmissionDate { get; set; }
        public string WorkflowLevelName { get; set; }
        public string WorkflowStageStatus { get; set; }
        public string SubmittedBy { get; set; }
        public string Comments { get; set; }
        public string DefaultText { get; set; }
        public string ProfileImage { get; set; }
        public bool isBulk { get; set; }
    }

    public class MyTaskResponseLog
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ParentSyncDBID { get; set; }
        public int TotalWFLevel { get; set; }
        public int WFLevel { get; set; }
        public string Command { get; set; }
        public string Status { get; set; }
        public int WorkflowID { get; set; }
        public string SubmittedBy { get; set; }
        public string TaskUpdateComment { get; set; }
        public string ProfileImage { get; set; }
        public bool isBulk { get; set; }
    }
    public class MyTaskGridData
    {
        public int ID { get; set; }
        public int SyncTaskID { get; set; }
        public int SectionID { get; set; }
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public int ResponseID { get; set; }
        public string Answer { get; set; }
        public bool IsBeneficiary { get; set; }
    }
}
