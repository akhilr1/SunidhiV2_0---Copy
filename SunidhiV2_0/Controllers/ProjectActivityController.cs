using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ubiety.Dns.Core;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace SunidhiV2_0.Controllers
{
    public class ProjectActivityController : Controller
    {
        #region ManageProjectActivity
        [HttpGet]
        public ActionResult GetProjectActivityList(int ProjectId, bool multS)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var activitylist = objProjectActivityDAL.GetProjectActivityList(ProjectId, multS);

            return Json(new { status = "success", data = activitylist }, new JsonSerializerSettings());
        }

        [HttpGet]
        public ActionResult GetSubActivityListByID(int ActivityId)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var activitylist = objProjectActivityDAL.GetSubActivityListByID(ActivityId);

            return Json(new { status = "success", data = activitylist }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AddActivity(ProjectActivity inputObj, ActivityAttchments activityAttchments)
        {
            string status = "";
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool retval = true;
            int activityId = 0;

            if (inputObj.Periodicity != 1 && inputObj.Periodicity != 2 && inputObj.Periodicity != 3 && inputObj.Periodicity != 9)
                retval = CommonActivity.IsReportingFrequencyAvailable(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonth, inputObj.PeriodicityMonthDay, inputObj.PeriodicityWeek, inputObj.PeriodicityMonthDateDay);

            if (retval)
            {
                if (inputObj.ActivityId > 0)
                {
                    activityId = objProjectActivityDAL.EditActivity(inputObj, loggedUserId, activityAttchments);
                }
                else
                {
                    activityId = objProjectActivityDAL.AddActivity(inputObj, loggedUserId, activityAttchments);
                }

                status = (activityId > 0 ? "success" : "failed");
            }
            else
            {
                status = "PeriodicityFailed";
            }

            return Json(new { Status = status ,ActivityId = activityId }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RemoveActivity(string ActivityIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            objProjectActivityDAL.RemoveActivity(loggedUserId, ActivityIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AssignUserToActivity(int ActivityId, string ActivityUsersList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            objProjectActivityDAL.AddUsersToActivity(ActivityId, ActivityUsersList, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        public IActionResult UpdateOutputWorkflow(int WorkFlowID, int ProjectId, int OutputId)
        {
            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objOutputDAL = new ProjectActivityDAL();
            objOutputDAL.UpdateOutputWorkflow(WorkFlowID, ProjectId, LoggedUserId, OutputId);
            return Json(new { status = "Success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult GetSurveyBeneficiaryType(int SurveyID)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var BeneficairyQustionlist = objProjectActivityDAL.GetSurveyBeneficiaryType(SurveyID);

            return Json(new { status = "success", data = BeneficairyQustionlist }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult GetSurveyLinkedBeneficairyQuestionDetails(int SurveyID)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var BeneficairyQustionlist = objProjectActivityDAL.GetSurveyLinkedBeneficairyQuestionDetails(SurveyID);

            return Json(new { status = "success", data = BeneficairyQustionlist }, new JsonSerializerSettings());
        }
        #endregion ManageProjectActivity

        #region ManageProjectActivityIndividual
        [HttpGet] //get activity details by ActivityId
        public ActionResult GetActivityDetailsByID(int ActivityId)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var activityDetails = objProjectActivityDAL.GetActivityDetailsByID(ActivityId);

            return Json(new { status = "success", data = activityDetails }, new JsonSerializerSettings());
        }
        [HttpGet] //get activity details by ActivityId
        public ActionResult GetActivityLogList(int ProjectId, int ActivityId)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            var actloglist = objProjectActivityDAL.GetActivityLogList(ProjectId, ActivityId);

            return Json(new { status = "success", data = actloglist }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetSectionsBySurveyID(int SurveyID)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<ProjectSurveySection> lst = new List<ProjectSurveySection>();
            lst = objProjectActivityDAL.GetSectionsBySurveyID(SurveyID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetQstnBySectionID(int SectionID)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<SurveyQuestion> lst = new List<SurveyQuestion>();
            lst = objProjectActivityDAL.GetQstnBySectionID(SectionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }


        [HttpGet]
        public IActionResult GetLogicType()
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<QuestionTypes> lst = new List<QuestionTypes>();
            lst = objProjectActivityDAL.GetLogicType().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpGet]
        public IActionResult GetParionentActivitySurveyDataCollection()
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<QuestionTypes> lst = new List<QuestionTypes>();
            lst = objProjectActivityDAL.GetLogicType().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        #region GetOutputSurveyIds
        /// <summary>
        /// GetOutputSurveyIds
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetOutputSurveyIds(int activityId = 0)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            string surveyIds = objProjectActivityDAL.GetOutputSurveyIds(activityId);
            return Json(new { status = "success", data = surveyIds }, new JsonSerializerSettings());
        }
        #endregion GetOutputSurveyIds

        public async Task<List<OutputPivotPresetData>> GetOutputPivotPresetList(int ActivityId)
        {
            List<OutputPivotPresetData> pivotList = new List<OutputPivotPresetData>();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            pivotList = objProjectActivityDAL.GetOutputPivotPresetList(loggedUserId, ActivityId);
            return pivotList;
        }

        public async Task<bool> SaveOutputPivotPreset(string PresetName, int ActivityId, string PresetData, int sectionId = 0)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            status = objProjectActivityDAL.SaveOutputPivotPreset(loggedUserId, PresetName, ActivityId, PresetData, sectionId);
            return status;
        }

        public async Task<string> RestoreOutputPivotPreset(string PresetName, int ActivityId)
        {
            string presetData = string.Empty;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            presetData = objProjectActivityDAL.RestoreOutputPivotPreset(loggedUserId, PresetName, ActivityId);
            return presetData;
        }

        public async Task<bool> EditOutputPivotPreset(string PresetName, int ActivityId, string PresetData)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            status = objProjectActivityDAL.EditOutputPivotPreset(loggedUserId, PresetName, ActivityId, PresetData);
            return status;
        }


        public async Task<bool> DeleteOutputPivotPreset(string PresetName, int ActivityId)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            status = objProjectActivityDAL.DeleteOutputPivotPreset(loggedUserId, PresetName, ActivityId);
            return status;
        }

        [HttpGet]
        public IActionResult GetBeneficiaryQuestionOption(int QstnID)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            lst = objProjectActivityDAL.GetBeneficiaryQuestionOption(QstnID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }


        [HttpGet]
        public IActionResult GetSurveyCascadingList()
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<MyTaskMasterList> lst = new List<MyTaskMasterList>();
            lst = objProjectActivityDAL.GetSurveyCascadingList().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetCascadingHeirarchyList(int QuestionID, bool isBeneficiary)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            List<MyTaskSurveyCascadingQuestionOptions> lst = new List<MyTaskSurveyCascadingQuestionOptions>();
            lst = objProjectActivityDAL.GetCascadingHeirarchyList(QuestionID, isBeneficiary).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        #endregion ManageProjectActivityIndividual

        #region GetUniqueOutputAutoGeneratedId
        /// <summary>
        /// GetUniqueOutputAutoGeneratedId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public JsonResult GetUniqueOutputAutoGeneratedId(int workSpaceId = 0, int ProjectId = 0)
        {
            string autoGeneratedId = string.Empty;

            Random rnd = new Random();
            int num = rnd.Next(10000, 99999);

            autoGeneratedId = "OT" + workSpaceId.ToString() + ProjectId.ToString() + num.ToString();

            return Json(new { autoGeneratedId }, new JsonSerializerSettings());
        }
        #endregion GetUniqueOutputAutoGeneratedId  

        #region GetSurveyCollectionBySurveyIds
        /// <summary>
        /// GetSurveyCollectionBySurveyIds
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IActionResult GetSurveyCollectionBySurveyIds(int ProjectId = 0, string surveyIds = "")
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();

            List<ProjectSurveyCollection> lst = new List<ProjectSurveyCollection>();
            lst = objProjectActivityDAL.GetSurveySectionsQuestionsCollectionBySurveyIds(ProjectId, surveyIds).ToList();

            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        #endregion GetSurveyCollectionBySurveyIds  


        #region GetOutputReport
        /// <summary>
        ///GetOutputReport
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IActionResult GetOutputReport(int ProjectId = 0, int PageNumber = 0)
        {
            OutputReportDAL objOutputReportDAL = new OutputReportDAL();

            List<OutputReport> lst = new List<OutputReport>();
            lst = objOutputReportDAL.GetOutputReport(ProjectId, PageNumber).ToList();

            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        #endregion GetOutputReport  

        #region GetOutComeReport
        /// <summary>
        ///GetOutComeReport
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IActionResult GetOutComeReport(int ProjectId = 0, int PageNumber = 0)
        {
            OutcomeReportDAL objOutcomeReportDAL = new OutcomeReportDAL();

            List<OutcomeReport> lstOutcomeReport = new List<OutcomeReport>();
            lstOutcomeReport = objOutcomeReportDAL.GetOutComeReport(ProjectId, PageNumber).ToList();

            return Json(new { data = lstOutcomeReport }, new JsonSerializerSettings());
        }
        #endregion GetOutComeReport  

        #region GetProjectActivityData
        /// <summary>
        /// GetProjectActivityData
        /// </summary>

        [HttpGet]
        public IActionResult GetProjectActivityData(int ProjectId = 0, DateTime? mStartDate = null, DateTime? mEndDate = null)
        {
            List<KeyValuePair<int, string>> lstSurveyType = new List<KeyValuePair<int, string>>();
            OutputOverview objOutputOverview = new OutputOverview();

            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            lstSurveyType = objProjectActivityDAL.GetProjectActivityData(ProjectId, mStartDate, mEndDate);

            objOutputOverview = objProjectActivityDAL.GetOutPutCountByProjectId(ProjectId);

            return Json(new { data = lstSurveyType, OutputOverview = objOutputOverview }, new JsonSerializerSettings());
        }
        #endregion GetSurveyData


        #region GetProjectActivityActualValues
        /// <summary>
        /// GetProjectActivityActualValues
        /// </summary>
        [HttpGet]
        public IActionResult GetProjectActivityActualValues(int ActivityId = 0)
        {
            ProjectActivityActualValue objProjectActivityActualValue = new ProjectActivityActualValue();
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            objProjectActivityActualValue = objProjectActivityDAL.GetProjectActivityActualValues(ActivityId);

            return Json(new { data = objProjectActivityActualValue }, new JsonSerializerSettings());
        }
        #endregion GetProjectActivityActualValues


        #region GetProjectActivityPhysicalActualValues
        /// <summary>
        /// GetProjectActivityPhysicalActualValues
        /// </summary>
        [HttpGet]
        public IActionResult GetProjectActivityPhysicalActualValues(int ActivityId = 0)
        {
            ProjectActivityDAL objProjectActivityDAL = new ProjectActivityDAL();
            double physicalActualValue = objProjectActivityDAL.GetProjectActivityPhysicalActualValues(ActivityId);

            return Json(new { Status = "Success", data = physicalActualValue }, new JsonSerializerSettings());
        }
        #endregion GetProjectActivityPhysicalActualValues

    }
}
