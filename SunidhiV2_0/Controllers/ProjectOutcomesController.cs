using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Code.Common;

namespace SunidhiV2_0.Controllers
{
    public class ProjectOutcomesController : Controller
    {
        [HttpGet]
        public IActionResult GetAllSDG()
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var SDGlist = objOutcomeDAL.FetchAllSDG();
            return Json(new { data = SDGlist }, new JsonSerializerSettings());
        }
        public IActionResult FetchSDGTargetsBasedOnSDGId(int SDGId)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var SDGTargetlist = objOutcomeDAL.FetchSDGTargetsBasedOnSDGId(SDGId);

            return Json(new { data = SDGTargetlist }, new JsonSerializerSettings());
        }
        public IActionResult FetchAllSDGTargets()
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var SDGTargetlist = objOutcomeDAL.FetchAllSDGTargets();

            return Json(new { data = SDGTargetlist }, new JsonSerializerSettings());
        }
        public IActionResult FetchAllSDGTargetsIndicators()
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var SDGTargetlist = objOutcomeDAL.FetchAllSDGTargetIndicators();

            return Json(new { data = SDGTargetlist }, new JsonSerializerSettings());
        }
        public IActionResult FetchAllOutcomeList(int ProjectId)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var lstOutcomes = objOutcomeDAL.FetchAllOutcomeList(ProjectId);
            return Json(new { data = lstOutcomes, status = "Success" }, new JsonSerializerSettings());
        }
        // To Fetch Outcome Based on Outcome Id
        public IActionResult FetchAllOutcomeListBasedOnOutComeId(int ProjectId, int OutcomeId)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var lstOutcomes = objOutcomeDAL.FetchAllOutcomeListBasedOnOutComeId(ProjectId, OutcomeId);
            return Json(new { data = lstOutcomes, status = "Success" }, new JsonSerializerSettings());
        }
        //CreateOutcome
        public IActionResult CreateOutcome(ProjectOutcome objOutcome)
        {
            var Status = "Success";
            var RFStatus = "Success";
            var OutcomeId = 0;
            bool RFAvailable = true; // To Check is Reporting frequecy exist or not
            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            if (objOutcome.Periodicity != 1 && objOutcome.Periodicity != 2 && objOutcome.Periodicity != 3 && objOutcome.Periodicity != 9)
            {
                RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonth, objOutcome.Periodicity, objOutcome.Periodicity, objOutcome.ReportingMonthDateDay);
            }
            if (RFAvailable == true)
            {
                RFStatus = "Success";
                Status = "Success";
                OutcomeId = objOutcomeDAL.CreateOutcome(objOutcome, LoggedUserId);
                if (OutcomeId > 0) // Created Succesfully
                {
                    objOutcomeDAL.GenerateReportingFrequencyTrackDate(objOutcome, LoggedUserId, OutcomeId);
                }
            }
            else
            {
                RFStatus = "Reporting Frequency Not available";
                Status = "Failed";
            }



            return Json(new { status = Status, RFStatus = RFStatus }, new JsonSerializerSettings());
        }
        public IActionResult UpdateOutcome(ProjectOutcome objOutcome)
        {
            var Status = "Success";
            var RFStatus = "Success";
            var OutcomeId = 0;
            bool RFAvailable = true; // To Check is Reporting frequecy exist or not
            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            if (objOutcome.Periodicity != 1 && objOutcome.Periodicity != 2 && objOutcome.Periodicity != 3 && objOutcome.Periodicity != 9)
            {
                RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonth, objOutcome.Periodicity, objOutcome.Periodicity, objOutcome.ReportingMonthDateDay);
            }
            if (RFAvailable == true)
            {
                RFStatus = "Success";
                Status = "Success";
                OutcomeId = objOutcomeDAL.UpdateOutcome(objOutcome, LoggedUserId);
                if (OutcomeId > 0) // Updated Succesfully
                {
                    objOutcomeDAL.GenerateReportingFrequencyTrackDate(objOutcome, LoggedUserId, OutcomeId);
                }
            }
            else
            {
                RFStatus = "Reporting Frequency Not available";
                Status = "Failed";
            }



            return Json(new { status = Status, RFStatus = RFStatus }, new JsonSerializerSettings());
        }

        //UpdateOutcomeWorkflow
        public IActionResult UpdateOutcomeWorkflow(int WorkFlowID, int ProjectId, int OutcomeId)
        {
            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            objOutcomeDAL.UpdateOutcomeWorkflow(WorkFlowID, ProjectId, LoggedUserId, OutcomeId);
            return Json(new { status = "Success" }, new JsonSerializerSettings());
        }
        // Assign Users to Outcome
        public IActionResult fnAssignUserstoOutcome(ProjectOutcome objOutcome, bool notify, int OutcomeId)
        {
            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            objOutcomeDAL.fnAssignUserstoOutcome(objOutcome, LoggedUserId, OutcomeId, "Insert");
            return Json(new { status = "Success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult DeleteOutcomes(string Outcomes, int Type)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            if (Type == 1)
            {
                List<ProjectOutcome> objOutcome = JsonConvert.DeserializeObject<List<ProjectOutcome>>(Outcomes);
                foreach (ProjectOutcome item in objOutcome)
                {
                    objOutcomeDAL.DeleteOutcomes(item.ActivityId, loggedUserId);
                }
            }
            else
            {
                objOutcomeDAL.DeleteOutcomes(Convert.ToInt32(Outcomes), loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }


        #region  New Outcome

        [HttpPost]
        public ActionResult AddOutCome(ProjectOutcome inputObj, ActivityAttchments activityAttchments)
        {
            string status = "";
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool retval = true;
            int actId = 0;

            if (inputObj.Periodicity != 1 && inputObj.Periodicity != 2 && inputObj.Periodicity != 3 && inputObj.Periodicity != 9)
                retval = CommonActivity.IsReportingFrequencyAvailable(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonth, inputObj.PeriodicityMonthDay, inputObj.PeriodicityWeek, inputObj.PeriodicityMonthDateDay);

            if (retval)
            {
                if (inputObj.ActivityId > 0)
                {
                    actId = objOutcomeDAL.EditOutcome(inputObj, loggedUserId, activityAttchments);
                }
                else
                {
                    actId = objOutcomeDAL.AddOutCome(inputObj, loggedUserId, activityAttchments);
                }

                status = (actId > 0 ? "success" : "failed");
            }
            else
            {
                status = "PeriodicityFailed";
            }

            return Json(new { Status = status, outcomeId = actId }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetLogicType()
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<QuestionTypes> lst = new List<QuestionTypes>();
            lst = objOutcomeDAL.GetLogicType().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetSectionsBySurveyID(int SurveyID)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<ProjectSurveySection> lst = new List<ProjectSurveySection>();
            lst = objOutcomeDAL.GetSectionsBySurveyID(SurveyID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }


        [HttpPost]
        public IActionResult GetQstnBySectionID(int SectionID)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<SurveyQuestion> lst = new List<SurveyQuestion>();
            lst = objOutcomeDAL.GetQstnBySectionID(SectionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult GetSurveyBeneficiaryType(int SurveyID)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var BeneficairyQustionlist = objOutcomeDAL.GetSurveyBeneficiaryType(SurveyID);
            return Json(new { status = "success", data = BeneficairyQustionlist }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult GetSurveyLinkedBeneficairyQuestionDetails(int SurveyID)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            var BeneficairyQustionlist = objOutcomeDAL.GetSurveyLinkedBeneficairyQuestionDetails(SurveyID);
            return Json(new { status = "success", data = BeneficairyQustionlist }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetBeneficiaryQuestionOption(int QstnID)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            lst = objOutcomeDAL.GetBeneficiaryQuestionOption(QstnID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetSurveyCascadingList()
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<MyTaskMasterList> lst = new List<MyTaskMasterList>();
            lst = objOutcomeDAL.GetSurveyCascadingList().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetCascadingHeirarchyList(int QuestionID, bool isBeneficiary)
        {
            ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
            List<MyTaskSurveyCascadingQuestionOptions> lst = new List<MyTaskSurveyCascadingQuestionOptions>();
            lst = objOutcomeDAL.GetCascadingHeirarchyList(QuestionID, isBeneficiary).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RemoveOutcome(string OutcomeIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            objProjectOutcomeDAL.RemoveOutcome(loggedUserId, OutcomeIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AssignUserToOutcome(int OutcomeId, string OutcomeUsersList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            objProjectOutcomeDAL.AddUsersToOutcome(OutcomeId, OutcomeUsersList, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        #region GetOutcomeSurveyIds
        /// <summary>
        /// GetOutcomeSurveyIds
        /// </summary>
        /// <param name="outcomeId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetOutcomeSurveyIds(int outcomeid = 0)
        {
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            string surveyIds = objProjectOutcomeDAL.GetOutcomeSurveyIds(outcomeid);
            return Json(new { status = "success", data = surveyIds }, new JsonSerializerSettings());
        }
        #endregion GetOutcomeSurveyIds

        #region GetPivotTableData
        /// <summary>
        /// GetPivotTableData
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetPivotTableData(string url)
        {
            HttpClient _client = new HttpClient();
            url = Startup.AnalyticsURL + url;
            var resp = await _client.GetStringAsync(url);
            return resp;
        }
        #endregion GetPivotTableData

        #region GetOutcomePivotPresetList
        /// <summary>
        /// GetOutcomePivotPresetList
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public async Task<List<OutcomePivotPresetData>> GetOutcomePivotPresetList(int OutcomeId)
        {
            List<OutcomePivotPresetData> pivotList = new List<OutcomePivotPresetData>();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            pivotList = objProjectOutcomeDAL.GetOutcomePivotPresetList(loggedUserId, OutcomeId);
            return pivotList;
        }
        #endregion GetOutcomePivotPresetList

        public async Task<bool> SaveOutcomePivotPreset(string PresetName, int OutcomeId, string PresetData, int sectionId = 0)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            status = objProjectOutcomeDAL.SaveOutcomePivotPreset(loggedUserId, PresetName, OutcomeId, PresetData, sectionId);
            return status;
        }

        public async Task<string> RestoreOutcomePivotPreset(string PresetName, int OutcomeId)
        {
            string presetData = string.Empty;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            presetData = objProjectOutcomeDAL.RestoreOutcomePivotPreset(loggedUserId, PresetName, OutcomeId);
            return presetData;
        }

        public async Task<bool> EditOutcomePivotPreset(string PresetName, int OutcomeId, string PresetData)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            status = objProjectOutcomeDAL.EditOutcomePivotPreset(loggedUserId, PresetName, OutcomeId, PresetData);
            return status;
        }


        public async Task<bool> DeleteOutcomePivotPreset(string PresetName, int OutcomeId)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            status = objProjectOutcomeDAL.DeleteOutcomePivotPreset(loggedUserId, PresetName, OutcomeId);
            return status;
        }

        public IActionResult GetOutComeProjectData(int WorkspaceID = 0)
        {
            List<KeyValuePair<int, string>> lstProjectType = new List<KeyValuePair<int, string>>();

            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            lstProjectType = objProjectOutcomeDAL.GetOutComeProjectData(WorkspaceID);

            return Json(new { data = lstProjectType }, new JsonSerializerSettings());
        }

        public IActionResult GetOutComeListByProjectId(int ProjectId = 0, DateTime? mStartDate = null, DateTime? mEndDate = null)
        {
            List<KeyValuePair<int, string>> lstProjectType = new List<KeyValuePair<int, string>>();
            OutcomeOverview objOutcomeOverview = new OutcomeOverview();

            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            lstProjectType = objProjectOutcomeDAL.GetOutComeListByProjectId(ProjectId, mStartDate, mEndDate);

            objOutcomeOverview = objProjectOutcomeDAL.GetOutComeCountByProjectId(ProjectId);

            return Json(new { data = lstProjectType, OutcomeOverview = objOutcomeOverview }, new JsonSerializerSettings());
        }

        #region GetOutcomeActualValue
        /// <summary>
        /// GetOutcomeActualValue
        /// </summary>
        [HttpGet]
        public IActionResult GetOutcomeActualValue(int outcomeId = 0)
        {
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            double actualvalue = objProjectOutcomeDAL.GetOutcomeActualValue(outcomeId);

            return Json(new { Status = "Success", data = actualvalue }, new JsonSerializerSettings());
        }
        #endregion GetOutcomeActualValue

        #region GetSDGListForOutcome
        /// <summary>
        /// GetSDGListForOutcome
        /// </summary>
        [HttpGet]
        public IActionResult GetSDGListForOutcome()
        {
            List<SDG> lstSDG = new List<SDG>();
            ProjectOutcomeDAL objProjectOutcomeDAL = new ProjectOutcomeDAL();
            lstSDG = objProjectOutcomeDAL.GetSDGListForOutcome();
            return Json(new { status = "Success", data = lstSDG }, new JsonSerializerSettings());
        }
        #endregion GetOutcomeActualValue

        #endregion New Outcome
    }
}