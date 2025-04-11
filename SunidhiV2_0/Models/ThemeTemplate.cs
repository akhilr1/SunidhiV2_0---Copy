using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ThemeTemplate
    {
        public string ThemeID { get; set; }
        public string ProjectTemplateID { get; set; }
        public string ThemeName { get; set; }
        public List<ActivityTemplate> ActivityList { get; set; }
        public List<RiskTemplate> RiskList { get; set; }
        public List<KPITemplate> KpiList { get; set; }
        public List<SurveyTemplate> SurveyList { get; set; }
        public List<BeneficiaryTypeTemplate> BeneficiaryTypeList { get; set; }
        public List<ProjectOutcome> Outcomes { get; set; }
        public List<Outputs> Outputs { get; set; }

        public string strSurveyList { get; set; }
        public string strBeneficiaryTypeList { get; set; }
        public string strOutcomes { get; set; }
        public string strOutputs { get; set; }
        public string IsSelected { get; set; }
        public string PrjtStartDate { get; set; }
        public string PrjtEndDate { get; set; }
    }

    public class RiskTemplate
    {
        public string RiskName { get; set; }
        public string RiskType { get; set; }
        public string Description { get; set; }
        public string RiskPeriodicity { get; set; }
        public string RiskRating { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string OnceDate { get; set; }
    }

    public class KPITemplate
    {
        public string KPIName { get; set; }
        public string Description { get; set; }
        public string KPIPeriodicity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string OnceDate { get; set; }
    }

    public class SurveyTemplate
    {
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SurveyPeriodicity { get; set; }
        public List<SectionTemplate> SectionList { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string OnceDate { get; set; }
    }

    public class SectionTemplate
    {
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public string SectionOrder { get; set; }
        public List<QuestionTemplate> QuestioinList { get; set; }
        public List<QuestionTemplate> SurveySecQuestions { get; set; }
    }

    public class QuestionTemplate
    {
        public string QuestionName { get; set; }
        public string QuestionTypeID { get; set; }
        public string QuestionOrder { get; set; }
        public string DefaultValue { get; set; }
        public string IsUnique { get; set; }
        public string IsSearchable { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string RegEx { get; set; }
        public string IsMandatory { get; set; }
        public string TextLength { get; set; }
        public string ErrorText { get; set; }
        public string FieldVar { get; set; }
        public List<OptionTemplate> lstQuestionOptions { get; set; }
    }

    public class OptionTemplate
    {
        //public string OptionValue { get; set; }

        public int OptionID { get; set; }
        public int QuestionID { get; set; }
        public string OptionValue { get; set; }
        public string OptionLabel { get; set; }
    }

    public class ActivityTemplate
    {
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WeightagePercentage { get; set; }
        public string ActivityType { get; set; }
        public string IsPaymentLinked { get; set; }
        public string PaymentLinkedBudget { get; set; }
        public string Periodicity { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string OnceDate { get; set; }
        public string TempActivityId { get; set; }
        public string ActualActivityId { get; set; }
        public List<SubActivityTemplate> SubActivityList { get; set; }
    }

    public class SubActivityTemplate
    {
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WeightagePercentage { get; set; }
        public string ActivityType { get; set; }
        public string IsPaymentLinked { get; set; }
        public string PaymentLinkedBudget { get; set; }
        public string Periodicity { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string OnceDate { get; set; }
        public string TempActivityId { get; set; }
        public string ActualActivityId { get; set; }
    }

    public class BeneficiaryTypeTemplate
    {
        public string BeneficiaryTypeName { get; set; }
        public string BeneficiaryTypeDescription { get; set; }
        public List<BeneficiaryQuestionTemplate> BeneficiaryQuestionList { get; set; }
        public List<BeneficiaryTemplate> BeneficiaryList { get; set; }
    }

    public class BeneficiaryQuestionTemplate
    {
        public string QuestionName { get; set; }
        public string QuestionType { get; set; }
        public string IsUnique { get; set; }
        public string IsSearchable { get; set; }
        public string IsMandatoryResponse { get; set; }
        public string QuestionHint { get; set; }
        public List<OptionTemplate> OptionList { get; set; }
    }
    public class BeneficiaryTemplate
    {
        public string BeneficiaryName { get; set; }
    }
    
    public class Outputs
    {
        public string OutputName { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string ReportingFrequencyName { get; set; }
        public string Periodicity { get; set; }
        public string SubActivities { get; set; }
        public string Type { get; set; }
        public string Survey { get; set; }
        public string Description { get; set; }
    }
}
