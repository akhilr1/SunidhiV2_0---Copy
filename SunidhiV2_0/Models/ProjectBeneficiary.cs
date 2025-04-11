using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ProjectBeneficiaryType
    {
        public int BeneficiaryTypeID { get; set; }
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BeneficiaryTypeTags { get; set; }
        public string BeneficiaryTypeQuestions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<Tags> BTTags { get; set; }
        public List<BeneficiaryTypeQuestions> BTQuestions { get; set; }
       
    }
    public class ProjectBeneficiary
    {
        public int BeneficiaryID { get; set; }
        public string BeneficiaryName { get; set; }
        public int BeneficiaryTypeID { get; set; }        
        public string BeneficiaryQstnAnswers { get; set; }
        public List<BeneficiaryTypeQuestions> BenefQuestions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string TokenValue { get; set; }
    }
    public class BeneficiaryRegistration
    {
        public int BeneficiaryID { get; set; }
        public string BeneficiaryName { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public string BeneficiaryTypeName { get; set; }
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int AnswerTypeID { get; set; }
        public string AnswerType { get; set; }
        public string Answer { get; set; }
        public string AnsBlob { get; set; }
        public int IsUpload { get; set; }
        public int IsNew { get; set; }
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsMandatoryResponse { get; set; }
        public int IsValidationQuestion { get; set; }
        public string RegexExpression { get; set; }
        public string QuestionHint { get; set; }

        public List<BeneficiaryTypeQstnAnsOptionList> BenefAnswerOptions { get; set; }
        public List<BeneficiaryTypeCascadingQstnOptions> BenefAnswerCascadingOptionList { get; set; }
        public List<BeneficiaryTypeValidation> BTQstnValidationList { get; set; }
    }
    public class BeneficiaryTypeQuestions
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }     
        public int AnswerTypeID { get; set; }
        public string AnswerType { get; set; }
        public string Answer { get; set; }
        public bool IsUnique { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsMandatoryResponse { get; set; }
        public int IsValidationQuestion { get; set; }
        public string RegexExpression { get; set; }
        public string QuestionHint { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int BeneficiaryID { get; set; }
        public string BTQstnAnsOptions { get; set; }
        public string BTQstnValidation { get; set; }
        public List<BeneficiaryTypeQstnAnsOptionList> BTQstnAnsOptionList { get; set; }
        public List<BeneficiaryTypeCascadingQstnOptions> BTQstnAnsCascadingOptionList { get; set; }
        public List<BeneficiaryTypeValidation> BTQstnValidationList { get; set; }
        public int IsRegistrationDone { get; set; }
        public int QuestionOrder { get; set; }

    }
    public class BeneficiaryAnswerType
    {
        public int AnswerTypeID { get; set; }
        public string AnswerType { get; set; }       

    }
    public class BeneficiaryTypeQstnAnsOptionList
    {
        public int QstnAnsOptionID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerTypeID { get; set; }

        public string OptionValue { get; set; }
        public bool IsOptSelected { get; set; }
    }
    public class BeneficiaryTypeCascadingQstnOptions
    {
        public int ID { get; set; }
        public int OptionGroupID { get; set; }
        public string OptionValue { get; set; }
        public int HierarchyOrder { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string PrimaryColumn { get; set; }
        public string ParentItemColumn { get; set; }
        public List<BeneficiaryTypeCascadingList> BTQstnAnsCascadingListItems { get; set; }
    }
    public class BeneficiaryTypeCascadingList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }

    }
    public class BeneficiarySurveyList
    {
        public int BeneficiaryID { get; set; }
        public string BeneficiaryName { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public string BeneficiaryQstnAnswers { get; set; }
        public List<BeneficiaryTypeQuestions> BenefQuestions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class BeneficiaryTypeValidation
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string ValidationCondition { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string ConditionValue { get; set; }
        public int LogicOperator { get; set; }
        public string CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }

    // API //
    public class BeneficiarySurveySummary
    {
        public DateTime SubmittedDateTime { get; set; }
        public string SubmittedBy { get; set; }
        public string WorkflowLevelUsers { get; set; }
        public string WorkflowStage { get; set; }
        public string WorkflowStatus { get; set; }
        public List<BeneficiaryQuestions> BeneficiaryQuestions { get; set; }
        public List<SurveyQuestions> SurveyQuestions { get; set; }
    }
    public class BeneficiaryQuestions
    {
        public int BeneficiaryId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }

    } 
    public class SurveyQuestions
    {
        public int ID { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public int BeneficiaryId { get; set; }
        public bool IsConditionQuestion { get; set; }
        public int SectionID { get; set; }
        public int SyncTaskID { get; set; }
    }


    public class DownloadMyTaskSummary
    {
        public DateTime SubmittedDateTime { get; set; }
        public string SubmittedBy { get; set; }
        public List<ResponseDetails> ResponseDetails { get; set; }
        public string WorkflowStageName { get; set; }
        public string WorkflowStageUser { get; set; }
        public string WorkflowStatus { get; set; }
        public int SurveyID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public int WorkflowID { get; set; }
        public int DataCollectionSyncDBID { get; set; }
        public int WFLevel { get; set; }
    }


    public class ResponseDetails
    {
        public int BeneficiaryID { get; set; }
        public List<BeneficiaryQuestions> BeneficiaryQuestions { get; set; }
        public List<SurveyQuestions> SurveyQuestions { get; set; }
    }


    public class DownloadSurveySectionLogic
    {
        public int SectionLogicID { get; set; }
        public int SurveyID { get; set; }
        public int SectionID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    }

    // API //
}
