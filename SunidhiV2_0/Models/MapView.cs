using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class MapView
    {
       
          
        public List<Project_list> Project_list { get; set; }
        public List<VersionData> VersionData { get; set; }
        public List<Preset_List> Preset_List { get; set; }
    }
    public class Project_list
    {

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<Survey> Survey { get; set; }

    }
    public class Survey
    {

            public int SurveyID { get; set; }
            public string SurveyName { get; set; }

    }
      public class VersionDataIDs
      {
        public int VersionID { get; set; }
        public string VersionName { get; set; }
        public int ProjectID { get; set; }
        public int SurveyID { get; set; }
        public int CreatedBy { get; set; }
        public List<VersionData> VersionData { get; set; }
    }
    public class VersionData
    {

            public string StateName { get; set; }
            public string DistrictName { get; set; }
            public bool Preset { get; set; }
            public string response_coordinates { get; set; }

    }
        public class Preset_List
        {
        public int PresetId { get; set; }
        public string Name { get; set; }
        public string SurveyId { get; set; }

        }
        public class SharelinkVersion
        {
            public string VersionName { get; set; }
            public string VersionUniqueId { get; set; }
            public int WorkspaceID { get; set; }
            public int ProjectID { get; set; }
            public int SurveyID { get; set; }
            public int loggedUserId { get; set; }
            public List<linkVersionDetails> IstlinkVersionDetails { get; set; }
            public string linkVersionDetails { get; set; }

        }
       
        public class linkVersionDetails
        {
            public string StateName { get; set; }
            public string DistrictName { get; set; }
            public bool Preset { get; set; }
            public string ResponseCoordinates { get; set; }
            public List<ResponseCoordinates> IstResponseCoordinates { get; set; }

        }
    public class ResponseCoordinates
    {
       
        public string Coordinates { get; set; }
    }

    public class StatewiseSurveyResponse
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public int ResponseCount { get; set; }
        public int QuestionTypeID { get; set; }
        public string Answer { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime ResponseStartDate { get; set; }
        public DateTime ResponseEndDate { get; set; }
        public string IsBeneficiarySurvey { get; set; }
        public string BeneficiaryName { get; set; }
        public int SyncTaskID { get; set; }
        public int SubmissionCount { get; set; }
    }

    public class ReducedStatewiseSurveyResponse
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public int ResponseCount { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public DateTime ResponseStartDate { get; set; }
        public DateTime ResponseEndDate { get; set; }
        public string IsBeneficiarySurvey { get; set; }
        public string BeneficiaryName { get; set; }
    }

    public class DistrictwiseSurveyResponse
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public int ResponseCount { get; set; }
        public int QuestionTypeID { get; set; }
        public string Answer { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public DateTime ResponseStartDate { get; set; }
        public DateTime ResponseEndDate { get; set; }
        public string IsBeneficiarySurvey { get; set; }
        public string BeneficiaryName { get; set; }
        public int SyncDBID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StateName { get; set; }
        public int SubmissionCount { get; set; }
    }

    public class ReducedDistrictwiseSurveyResponse
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public int ResponseCount { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public DateTime ResponseStartDate { get; set; }
        public DateTime ResponseEndDate { get; set; }
        public string IsBeneficiarySurvey { get; set; }
        public string BeneficiaryName { get; set; }
    }
    public class Geo_coordinate_list
    {
        public int SurveyID { get; set; }   
        public string Geo_coordinates { get; set; }
        public int SyncDBID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SubmissionCount { get; set; }
    }
    public class PresetList
    {
        public string PresetName { get; set; }
        public string PresetData { get; set; }

    }
    public class VersionDetails
    {
        public string VersionName { get; set; }
        public string VersionUniqueId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
    

    }
    public class Geo_coordinate_Details
    {
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public int QuestionTypeID { get; set; }

    }
    public class ShareMapSurvey
    {
        public string SurveyName { get; set; }
        public int SurveyID { get; set; }
        public List<ShareMapSurveySection> SurveySectionList { get; set; }
    }

    public class ShareMapSurveySection
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public List<SurveyQuestionResponse> SurveyQuestionList { get; set; }
    }

    public class SurveyQuestionResponse
    {
        public int QuestionID { get; set; }
        public int SectionID { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public bool IsConditionQuestion { get; set; }
        public string QuestionType { get; set; }
    }


    public class MapStateCollection
    {
        public int SurveyID { get; set; }
        public string StateName { get; set; }
    }

    public class MapDistrictCollection
    {
        public int SurveyID { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
    }

    public class MapDistrictCordinatesCollection
    {
        public int SurveyID { get; set; }
        public string DistrictName { get; set; }
        public string Geo_coordinates { get; set; }
    }

    public class MapShareVersionList
    {
        public List<MapStateCollection> lstState { get; set; }
        public List<MapDistrictCollection> lstDistricts { get; set; }
        public List<MapDistrictCordinatesCollection> lstDistrictCordinates { get; set; }
    }

    public class ReducedDistrictwiseSharedSurveyResponse
    {
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public int ResponseCount { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public DateTime ResponseStartDate { get; set; }
        public DateTime ResponseEndDate { get; set; }
        public string IsBeneficiarySurvey { get; set; }
        public string BeneficiaryName { get; set; }
        public string StateName { get; set; }
    }

}
