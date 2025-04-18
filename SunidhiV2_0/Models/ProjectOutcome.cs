﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ProjectOutcome
    {
        public int pk_OutcomeID { get; set; }
        public int ProjectID { get; set; }
        public string OutcomeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string DatacollectionType { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public int SurveySectionId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyLogic { get; set; }
        public string SurveyTarget { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int Periodicity { get; set; }
       // public int IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string ReportingFrequencyName { get; set; }
        public int ReportingMonth { get; set; }
        public int ReportingMonthDay { get; set; }
        public int ReportingWeek { get; set; }
        public int ReportingMonthDateDay { get; set; }
        public string ReportingOnceDate { get; set; }
        public List<SDG> lstSDGList { get; set; }
        public List<SDGTargets> lstSDGTargetsList { get; set; }
        public List<UsersDetails> lstUserList { get; set; }
        public string SDGList { get; set; }
        public string SDGTargetsList { get; set; }
        public string UserList { get; set; }
        public string Tags { get; set; }

        public string QuestionName { get; set; }
        public string SectionName { get; set; }
        public string SurveyLogicName { get; set; }
        public string SurveyTargetName { get; set; }
        public string ProfileImage { get; set; }
        public int BeneficiaryTypeQuestionID { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public List<OutcomeQuantitativeLogic> OutcomeQuantitativeLogic { get; set; }
        public string Description { get; set; }
        public bool IsPaymentLinked { get; set; }
        public int PaymentLinkedBudget { get; set; }
        public int PeriodicityMonth { get; set; }
        public int PeriodicityMonthDay { get; set; }
        public int PeriodicityWeek { get; set; }
        public int PeriodicityMonthDateDay { get; set; }
        public DateTime PeriodicityOnceDate { get; set; }
        public int ActivityId { get; set; } // outcomeId
        public int Progress { get; set; }
        public List<Tags> ActivityTags { get; set; }
        public List<Tags> ExpenseTags { get; set; }
        public List<User> ExpenseTagsNotifyUsers { get; set; }
        public List<User> ActivityUsers { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; }
        public List<OutcomeCount> lstOutcomeCount { get; set; }
        public List<ProjectOutcomeMonthwiseData> lstProjectOutcomeMonthwiseData { get; set; }
        public string UnitOfMeasure { get; set; }
        public int AdditionalSectionId { get; set; }
        public int AdditionalQuestionId { get; set; }
        public bool? AdditionalQuestionType { get; set; }
        public string AutoGeneratedActivityId { get; set; }
        public double ActualValue { get; set; }
        public double SDGID { get; set; }
    }
    public class SDG
    {
        public int pk_SDGID { get; set; }
        public int SDGID { get; set; }
        public string SDG_Name { get; set; }
        public int Status { get; set; }
        public string ViewAllPanel { get; set; }
        public bool IsSelected { get; set; }
        public List<SDGTargets> Target { get; set; }
        public string SDG_Goal { get; set; }
        public string SDGImage { get; set; }
    }
    public class SDGTargets
    {
        public int pk_SDGTargetID { get; set; }
        public int fk_SDGID { get; set; }
        public string Target_Name { get; set; }
        public int Status { get; set; }
        public List<SDGTargetIndicators> Target_Indicators { get; set; }
    }
    public class SDGTargetIndicators
    {
        public int pk_Indicaters { get; set; }
        public int fk_SDGTargetID { get; set; }
        public int SDGID { get; set; }
        public string Indicator { get; set; }
        public int Status { get; set; }
    }

    public class OutcomeQuantitativeLogic
    {
        public int ID { get; set; }
        public int QuestionId { get; set; }
        public int SectionId { get; set; }
        public int SurveyId { get; set; }
        public int BeneficiaryTypeQstnID { get; set; }
        public string QuestionName { get; set; }
        public string SectionName { get; set; }
        public string SurveyName { get; set; }
        public List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic { get; set; }
    }

    public class OutcomeSurveyPrimeLogic
    {
        public int SurveyLogicPrime { get; set; }
        public string SurveyLogicPrimeAnswer { get; set; }
        public string SurveyLogicPrimeType { get; set; }
        public int BeneficiaryQuestionID { get; set; }
        public int SurveyLogicCondition { get; set; }
    }

    public class OutComePivotPresetData
    {
        public string PresetName { get; set; }
        public string PresetData { get; set; }
    }

    public enum OutcomeLogicType
    {
        Count_of_Unique_Reponses = 1,
        Cumulative_Sum = 2,
        Equal_To = 3,
        Greater_Than = 4,
        Greater_Than_Or_Equal_To = 5,
        Less_Than = 6,
        Less_Than_Or_Equal_To = 7,
        Not_Equal_To = 8,
        increase_decrease = 9,
        change_over_time = 10,
        Average = 11
    }


    public class OutcomeDataCollectionDetails
    {
        public int ID { get; set; }
        public int ActivityId { get; set; }
        public int SurveyId { get; set; }
        public int SectionId { get; set; }
        public int QuestionId { get; set; }
        public int BeneficiaryTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BeneficiaryQuestionID { get; set; }
        public int SurveyAnswerType { get; set; }
        public int BenAnswerType { get; set; }
        public int SurveyLogicPrime { get; set; }
        public string SurveyLogicPrimeAnswer { get; set; }
        public string SurveyLogicPrimeType { get; set; }
        public int LogicCondition { get; set; }
    }

    public class OutcomeSurveyResponse
    {
        public int ResponseID;
        public string Answer;
        public int QuestionID;
        public int SurveyID;
        public int SyncTaskID;
        public DateTime CreatedDate;
        public int SubmissionCount { get; set; }
    }

    public class OutcomeBeneficiaryResponse
    {
        public int ResponseID;
        public double Anwer;
        public int SurveyID;
        public DateTime CreatedDate;
        public string AnwerString;
        public int QuestionId;
    }

    public class OutcomeCount
    {
        public double AnswerSum { get; set; }
        public int ResponseCount { get; set; }
        public int SurveyID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ResponseId { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
        public int QuestionID { get; set; }
    }


    public class OutcomePivotPresetData
    {
        public string PresetName { get; set; }
        public string PresetData { get; set; }
        public int SectionId { get; set; }
    }

    public class OutcomeOverview
    {
        public int OutcomeID { get; set; }
        public int OutcomeProgress { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int TotalCount { get; set; }
        public string LastExecutedDateTime { get; set; }
    }

    public class ProjectOutcomeMonthwiseData
    {
        public int ID { get; set; }
        public int OutcomeId { get; set; }
        public double Target { get; set; }
        public double Budget { get; set; }
        public int ActMonth { get; set; }
        public int ActYear { get; set; }
    }

    public class ProjectOutcomeInitialData
    {
        public int SyncTaskID { get; set; }
        public int SubmissionCount { get; set; }
        public int ResponseID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public int SurveyID { get; set; }
    }


    public class ProjectOutcomeBeneficiaryData
    {
        public int ResponseID { get; set; }
        public double TotalSum { get; set; }
    }
}
