#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Code.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using static SunidhiV2_0.Models.SurveyResponse;
#endregion NameSpaces

namespace SunidhiV2_0.Controllers
{
    public class ProjectSurveyController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public ProjectSurveyController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        #region SurveyLanding

        #region GetProjectSurveyList
        /// <summary>
        /// GetProjectSurveyList
        /// </summary>
        /// <returns>Survey List - Json</returns>
        [HttpGet]
        public IActionResult GetProjectSurveyList(int ProjectId = 0)
        {
            List<ProjectSurvey> lstProjectSurvey = new List<ProjectSurvey>();

            if (ProjectId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                lstProjectSurvey = objProjectSurveyDAL.GetProjectSurveyList(ProjectId);
            }

            return Json(new { data = lstProjectSurvey }, new JsonSerializerSettings());
        }
        #endregion GetProjectSurveyList

        #region SaveProjectSurvey
        /// <summary>
        /// SaveProjectSurvey
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <returns>status - success / failed /nameexists</returns>
        [HttpPost]
        public IActionResult SaveProjectSurvey(ProjectSurvey objProjectSurvey)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool RFAvailable = true;
            dynamic obj = null;
            DateTime startDate;
            DateTime endDate;

            if (loggedUserId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                bool isExists = objProjectSurveyDAL.IsSurveyNameExists(objProjectSurvey.ProjectId, objProjectSurvey.SurveyName, objProjectSurvey.SurveyID);

                if (!isExists)
                {

                    bool isSaved = false;
                    if (objProjectSurvey.StartDate != "null" && objProjectSurvey.EndDate != "null")
                    {
                        startDate = Convert.ToDateTime(objProjectSurvey.StartDate);
                        endDate = Convert.ToDateTime(objProjectSurvey.EndDate);

                    }
                    else
                    {
                        obj = objProjectSurveyDAL.GetProjectDate(objProjectSurvey.ProjectId);
                        startDate = obj.StartDate;
                        endDate = obj.EndDate;
                    }


                    if (objProjectSurvey.ReportingFrequency != 1 && objProjectSurvey.ReportingFrequency != 2 && objProjectSurvey.ReportingFrequency != 3 && objProjectSurvey.ReportingFrequency != 9)
                        RFAvailable = CommonActivity.IsReportingFrequencyAvailable(startDate, endDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingMonth, objProjectSurvey.ReportingMonthDay, objProjectSurvey.ReportingWeek, objProjectSurvey.ReportingMonthDateDay);
                    if (RFAvailable)
                    {
                        if (objProjectSurvey.SurveyID > 0)
                        {
                            isSaved = objProjectSurveyDAL.UpdateProjectSurvey(objProjectSurvey, loggedUserId, startDate, endDate);
                        }
                        else
                        {
                            isSaved = objProjectSurveyDAL.InsertProjectSurvey(objProjectSurvey, loggedUserId, startDate, endDate);
                        }

                        status = (isSaved ? "success" : "failed");
                    }
                    else
                    {
                        status = "rptfrequencyfailed";
                    }
                }
                else
                {
                    status = "nameexists";
                }
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion SaveProjectSurvey


        #region GetProjectSurveyBySurveyId
        /// <summary>
        /// GetProjectSurveyBySurveyId
        /// </summary>
        /// <returns>Survey Items - Json</returns>
        [HttpGet]
        public IActionResult GetProjectSurveyBySurveyId(int SurveyId = 0)
        {
            ProjectSurvey objProjectSurvey = new ProjectSurvey();

            if (SurveyId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                objProjectSurvey = objProjectSurveyDAL.GetProjectSurveyBySurveyId(SurveyId);
            }

            return Json(new { data = objProjectSurvey }, new JsonSerializerSettings());
        }
        #endregion GetProjectSurveyBySurveyId

        #region DeleteProjectSurvey
        /// <summary>
        /// DeleteProjectSurvey
        /// </summary>
        /// <param name="SurveyID">SurveyId</param>
        /// <returns> success / failed - json</returns>
        [HttpPost]
        public IActionResult DeleteProjectSurvey(string SurveyID, int Type)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                bool isExist = objProjectSurveyDAL.IsProjectSurveyExists(loggedUserId, SurveyID, Type);
                bool isDeleted = false;
                if (isExist == false)
                {
                    if (Type == 1) // Bulk Delete 
                    {
                        List<ProjectSurvey> objSurveyList = JsonConvert.DeserializeObject<List<ProjectSurvey>>(SurveyID);
                        foreach (ProjectSurvey objSurvey in objSurveyList)
                        {
                            isDeleted = objProjectSurveyDAL.DeleteProjectSurvey(objSurvey.SurveyID, loggedUserId);
                        }
                    }
                    else // individual delete
                    {
                        isDeleted = objProjectSurveyDAL.DeleteProjectSurvey(Convert.ToInt32(SurveyID), loggedUserId);
                    }
                    status = (isDeleted ? "success" : "failed");
                }
                else
                {
                    //isDeleted = false;
                    status = (isExist ? "Survey(s) Already used by Output / Outcome" : "success");
                }

            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion DeleteProjectSurvey       

        #region NotUsed
        #region GetSurveyHierarchyGroup
        /// <summary>
        /// GetSurveyHierarchyGroup
        /// </summary>
        /// <returns> HierarchyGroup List - Json</returns>
        [HttpGet]
        public IActionResult GetSurveyHierarchyGroup()
        {
            List<KeyValuePair<int, string>> lstHierarchyGroup = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstHierarchyGroup = objProjectSurveyDAL.GetSurveyHierarchyGroup();

            return Json(new { data = lstHierarchyGroup }, new JsonSerializerSettings());
        }
        #endregion GetSurveyHierarchyGroup

        #region GetHierarchyDrpValues
        /// <summary>
        /// GetHierarchyDrpValues
        /// </summary>
        /// <param name="mainDrpValue">mainDrpValue</param>
        /// <param name="type">type</param>
        /// <returns> json </returns>
        [HttpGet]
        public IActionResult GetHierarchyDrpValues(string mainDrpValue, string type)
        {
            List<KeyValuePair<int, string>> lstHierarchyGroup = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstHierarchyGroup = objProjectSurveyDAL.GetHierarchyDrpValues(mainDrpValue, type);

            return Json(new { data = lstHierarchyGroup }, new JsonSerializerSettings());
        }
        #endregion GetHierarchyDrpValues

        #region GetHierarchyGroupItems
        /// <summary>
        /// GetHierarchyGroupItems
        /// </summary>
        /// <param name="GroupId">GroupId</param>
        /// <param name="Order">Order</param>
        /// <param name="ParentId">ParentId</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHierarchyGroupItems(int GroupId, int Order, int ParentId = 0)
        {
            List<KeyValuePair<int, string>> lstHierarchyGroupItems = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstHierarchyGroupItems = objProjectSurveyDAL.GetHierarchyGroupItems(GroupId, Order, ParentId);

            return Json(new { data = lstHierarchyGroupItems }, new JsonSerializerSettings());
        }
        #endregion GetQuestionType

        #endregion

        #region GetProjectBeneficiaryType
        /// <summary>
        /// GetProjectBeneficiaryType
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="surveyId">surveyId</param>
        /// <returns> beneficiary type list - json </returns>
        [HttpPost]
        public IActionResult GetProjectBeneficiaryType(int projectId = 0, int surveyId = 0)
        {
            List<BeneficiaryType> lstBeneficiaryType = new List<BeneficiaryType>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstBeneficiaryType = objProjectSurveyDAL.GetProjectBeneficiaryType(projectId, surveyId);

            return Json(new { data = lstBeneficiaryType }, new JsonSerializerSettings());
        }
        #endregion GetProjectBeneficiaryType


        #region GetProjectData
        /// <summary>
        /// GetProjectData
        /// </summary>

        [HttpGet]
        public IActionResult GetProjectData(int WorkspaceID = 0)
        {
            List<KeyValuePair<int, string>> lstProjectType = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstProjectType = objProjectSurveyDAL.GetProjectData(WorkspaceID);

            return Json(new { data = lstProjectType }, new JsonSerializerSettings());
        }
        #endregion GetProjectData


        #region GetProjectSurveyData
        /// <summary>
        /// GetProjectData
        /// </summary>

        [HttpGet]
        public IActionResult GetDetailProjectData(int ProjectId)
        {
            ProjectSurveyDAL objSurvDAL = new ProjectSurveyDAL();
            var lstProjectSurvey = objSurvDAL.GetDetailProjectData(ProjectId);
            return Json(new { data = lstProjectSurvey, status = "Success" }, new JsonSerializerSettings());
        }
        #endregion GetProjectSurveyData


        [HttpGet]
        public ActionResult GetSurveyProgress(int ProjectId)
        {
            ProjectSurveyDAL objSurveyProgress = new ProjectSurveyDAL();
            double Progress = 0;
            Progress = objSurveyProgress.GetSurveyProgress(ProjectId);
            return Json(new { data = Progress }, new JsonSerializerSettings());

        }


        #region GetProjectSurveyQuestData
        /// <summary>
        /// GetProjectData
        /// </summary>
        [HttpGet]
        public IActionResult FetchAllSurveyQuestDetail(int SurveyID, int QuestionID)
        {
            ProjectSurveyDAL objSurvDAL = new ProjectSurveyDAL();
            var lstProjectSurveyQuest = objSurvDAL.FetchAllSurveyQuestDetail(SurveyID, QuestionID);
            return Json(new { data = lstProjectSurveyQuest, status = "Success" }, new JsonSerializerSettings());
        }
        #endregion GetProjectSurveyQuestData


        #region GetSurveyData
        /// <summary>
        /// GetSurveyData
        /// </summary>

        [HttpGet]
        public IActionResult GetSurveyData(int ProjectId = 0, DateTime? mStartDate = null, DateTime? mEndDate = null)
        {
            List<KeyValuePair<int, string>> lstSurveyType = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstSurveyType = objProjectSurveyDAL.GetSurveyData(ProjectId, mStartDate, mEndDate);

            return Json(new { data = lstSurveyType }, new JsonSerializerSettings());
        }
        #endregion GetSurveyData



        #region GetQuestionData
        /// <summary>
        /// GetQuestionData
        /// </summary>

        [HttpGet]
        public IActionResult GetQuestionData(int SurveyID = 0)
        {
            List<KeyValuePair<int, string>> lstQuestionType = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstQuestionType = objProjectSurveyDAL.GetQuestionData(SurveyID);

            return Json(new { data = lstQuestionType }, new JsonSerializerSettings());
        }
        #endregion GetQuestionData



        #region GetSurveyAuditLog
        /// <summary>
        /// GetSurveyAuditLog
        /// </summary>

        [HttpGet]
        public IActionResult GetSurveyAuditLog(int SurveyID = 0)
        {
            List<TaskAuditLog> lstSurveyAuditType = new List<TaskAuditLog>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstSurveyAuditType = objProjectSurveyDAL.GetSurveyAuditLog(SurveyID).ToList();
            return Json(new { data = lstSurveyAuditType }, new JsonSerializerSettings());
        }
        #endregion GetSurveyAuditLog



        #region GetBeneficiaryData
        /// <summary>
        /// GetBeneficiaryData
        /// </summary>

        [HttpGet]
        public IActionResult GetBeneficiaryData(int SurveyID = 0)
        {
            List<KeyValuePair<int, string>> lstBeneficiaryType = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstBeneficiaryType = objProjectSurveyDAL.GetBeneficiaryData(SurveyID);

            return Json(new { data = lstBeneficiaryType }, new JsonSerializerSettings());
        }
        #endregion GetBeneficiaryData




        #region GetProjectWorkFlow
        /// <summary>
        /// GetProjectWorkFlow
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns> ProjectWorkFlow - list </returns>
        [HttpGet]
        public IActionResult GetProjectWorkFlow(int projectId = 0)
        {
            List<KeyValuePair<int, string>> lstBeneficiaryType = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstBeneficiaryType = objProjectSurveyDAL.GetProjectWorkFlow(projectId);

            return Json(new { data = lstBeneficiaryType }, new JsonSerializerSettings());
        }
        #endregion GetProjectWorkFlow

        #region GetProjectDate
        /// <summary>
        /// GetProjectWorkFlow
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns> ProjectWorkFlow - list </returns>
        [HttpGet]
        public IActionResult GetProjectDate(int projectId = 0)
        {


            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            dynamic obj = objProjectSurveyDAL.GetProjectDate(projectId);

            return Json(new { PrjStartDate = obj.StartDate, PrjEndDate = obj.EndDate }, new JsonSerializerSettings());
        }
        #endregion GetProjectWorkFlow       

        #region GetReportingFrequencyList
        /// <summary>
        /// GetReportingFrequencyList
        /// </summary>
        /// <returns>ReportingFrequencyList - json </returns>
        [HttpGet]
        public IActionResult GetReportingFrequencyList()
        {
            List<KeyValuePair<int, string>> lstReportingFrequencyList = new List<KeyValuePair<int, string>>();
            lstReportingFrequencyList = CommonActivity.GetReportingFrequencyList();
            return Json(new { data = lstReportingFrequencyList }, new JsonSerializerSettings());
        }
        #endregion GetReportingFrequencyList

        #region AssignUserstoSurvey
        /// <summary>
        /// AssignUserstoSurvey
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AssignUserstoSurvey(ProjectSurvey objProjectSurvey)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                bool isAssigned = false;
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                isAssigned = objProjectSurveyDAL.AssignUserstoSurvey(objProjectSurvey, loggedUserId, 0,true);

                status = (isAssigned ? "success" : "failed");
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion AssignUserstoSurvey

        #region GetSurveyAssignedUsers
        /// <summary>
        /// GetSurveyAssignedUsers
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSurveyAssignedUsers(int SurveyID = 0)
        {
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            string AssignedUsers = objProjectSurveyDAL.GetSurveyAssignedUsers(SurveyID);
            return Json(new { data = AssignedUsers }, new JsonSerializerSettings());
        }
        #endregion GetSurveyAssignedUsers

        #region UpdateWorkflow
        /// <summary>
        /// UpdateWorkflow
        /// </summary>
        /// <param name="WorkFlowID"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateWorkflow(int WorkFlowID, int SurveyID)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                bool isUpdated = false;
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                isUpdated = objProjectSurveyDAL.UpdateWorkflow(WorkFlowID, loggedUserId, SurveyID);

                status = (isUpdated ? "success" : "failed");
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion UpdateWorkflow

        #region ImportProjectSurvey
        /// <summary>
        /// ImportProjectSurvey
        /// </summary>
        /// <param name="ImportSurveyList"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportProjectSurvey(string ImportSurveyList, int ProjectID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                List<ProjectSurvey> lstProjectSurvey = JsonConvert.DeserializeObject<List<ProjectSurvey>>(ImportSurveyList);
                bool isSaved = false;
                var rptFrequency = "";
                foreach (ProjectSurvey itm in lstProjectSurvey)
                {
                    ProjectSurvey objProjectSurvey = new ProjectSurvey();
                    if (itm != null)
                    {
                        objProjectSurvey.ProjectId = ProjectID;
                        objProjectSurvey.SurveyName = itm.SurveyName;
                        objProjectSurvey.SurveyDescription = itm.SurveyName;

                        objProjectSurvey.StartDate = Convert.ToDateTime(itm.StartDate).ToString("yyyy-MM-dd");//ConvertToDate(itm.StartDate);//;
                        objProjectSurvey.EndDate = Convert.ToDateTime(itm.EndDate).ToString("yyyy-MM-dd"); //itm.EndDate; //ConvertToDate(itm.EndDate);//
                        //rptFrequency = char.ToUpper(itm.ReportingFrequencyName[0]) + itm.ReportingFrequencyName.Substring(1);

                        //if (!(rptFrequency =="Never" || rptFrequency == "Once"|| rptFrequency == "Daily" || rptFrequency == "Weekly"||
                        //rptFrequency == "Monthly" || rptFrequency == "Quarterly" || rptFrequency == "Half_Yearly" || rptFrequency == "Yearly" || rptFrequency == "Anytime"))
                        //{
                        //return Json(new { status = "failed" }, new JsonSerializerSettings());
                        // }
                        objProjectSurvey.WorkflowID = (!string.IsNullOrEmpty(itm.WorkFlowName) ? objProjectSurveyDAL.GetWorkFlowIdByName(ProjectID, itm.WorkFlowName) : 0);
                        //objProjectSurvey.ReportingFrequency = (int)Enum.Parse(typeof(CommonActivity.ReportingFrequency), itm.ReportingFrequencyName);
                        //objProjectSurvey.ReportingFrequency = (int)Enum.Parse(typeof(CommonActivity.ReportingFrequency), rptFrequency);

                        objProjectSurvey.BeneficiaryTypeID = (!string.IsNullOrEmpty(itm.WorkFlowName) ? objProjectSurveyDAL.GetBeneficiaryTypeIdByName(ProjectID, itm.BeneficiaryTypeName) : 0);
                        dynamic obj = objProjectSurveyDAL.GetProjectDate(ProjectID);

                        isSaved = objProjectSurveyDAL.InsertProjectSurvey(objProjectSurvey, loggedUserId, obj.StartDate, obj.EndDate);
                    }
                }
            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion ImportProjectSurvey

        #region UpdateSurveyBeneficiaryType
        /// <summary>
        /// UpdateSurveyBeneficiaryType
        /// </summary>
        /// <param name="BenficiaryTypeId"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateSurveyBeneficiaryType(int BenficiaryTypeId, int SurveyID)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                bool isUpdated = false;
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                isUpdated = objProjectSurveyDAL.UpdateSurveyBeneficiaryType(BenficiaryTypeId, loggedUserId, SurveyID);

                status = (isUpdated ? "success" : "failed");
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion UpdateSurveyBeneficiaryType

        #region SaveDuplicateProjectSurvey

        [HttpPost]
        public IActionResult SaveDuplicateProjectSurvey(ProjectSurvey objProjectSurvey, bool duplicateSecAndQstns, int OldSurveyID)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                bool isExists = objProjectSurveyDAL.IsSurveyNameExists(objProjectSurvey.ProjectId, objProjectSurvey.SurveyName, objProjectSurvey.SurveyID);

                if (!isExists)
                {

                    bool isSaved = false;


                    dynamic obj = objProjectSurveyDAL.GetSurveyDate(OldSurveyID);

                    objProjectSurvey.StartDate = obj.StartDate;
                    objProjectSurvey.EndDate = obj.EndDate;
                    isSaved = objProjectSurveyDAL.InsertProjectSurvey(objProjectSurvey, loggedUserId, obj.StartDateD, obj.EndDateD);

                    if (duplicateSecAndQstns)
                    {
                        isSaved = objProjectSurveyDAL.SaveDuplicateProjectSurvey(objProjectSurvey.SurveyID, OldSurveyID, loggedUserId);

                        if (isSaved)
                        {
                            isSaved = objProjectSurveyDAL.UpdateQuestionIdSectionIdForDuplicatedSurvey(objProjectSurvey.SurveyID, OldSurveyID, loggedUserId);
                        }

                        status = (isSaved ? "success" : "failed");

                    }
                }
                else
                {
                    status = "nameexists";
                }
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        #endregion SaveDuplicateProjectSurvey

        #region Check Survey Task is Approved or Not
        [HttpPost]
        public IActionResult CheckSureveyTaskIsAprovedWhileUpdate(string Type, int WorkspaceID, int TypeID)
        {
            string status = "Success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                var ApprovedLevel = 0;
                CommonActivity ObjCommonActivity = new CommonActivity();
                ApprovedLevel = ObjCommonActivity.GetTaskApprovedOrNot(Type, TypeID, WorkspaceID);

                if (ApprovedLevel > 0)
                {
                    status = "Failed";
                }
            }
            return Json(new { status }, new JsonSerializerSettings());
        }

        #endregion
        #endregion SurveyLanding

        #region SurveySection

        #region Get
        [HttpGet]
        public IActionResult GetAllSectionsBySurveyID(int SurveyID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<ProjectSurveySection> lst = new List<ProjectSurveySection>();
            lst = objSurvey.GetAllSectionsBySurveyID(SurveyID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetAllResponsesBySurveyID(int SurveyID, int offset = 0)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyResponse> lstSummary = new List<SurveyResponse>();
            List<SurveyResponse> lstInd = new List<SurveyResponse>();
            lstSummary = objSurvey.GetAllQstnsResponsesBySurveyID(SurveyID, offset).ToList();
            lstInd = objSurvey.GetAllQstnsIndResponsesBySurveyID(SurveyID).ToList();
            return Json(new { SummaryResponse = lstSummary, IndResponses = lstInd }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetResponsesBySurveyID(int SurveyID, int offset = 0)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyResponse> lstInd = new List<SurveyResponse>();
            SurveyResponseDetails lstDetails = new SurveyResponseDetails();
            lstInd = objSurvey.GetAllQstnsIndResponsesBySurveyID(SurveyID).ToList();
            lstDetails = objSurvey.GetResponsesBySurveyID(SurveyID);
            return Json(new { IndResponses = lstInd, lstDetails = lstDetails }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetAllResponsesBySurveyReport(int SurveyID, int offset = 0, int QuestionID = 0)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyResponse> lstSummary = new List<SurveyResponse>();
            lstSummary = objSurvey.GetAllResponsesBySurveyReport(SurveyID, offset, QuestionID).ToList();
            return Json(new { SummaryResponse = lstSummary }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetAllSummaryResponseBySurveyID(int SurveyID, int offset = 0)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyResponse> lstSummary = new List<SurveyResponse>();
            lstSummary = objSurvey.GetAllQstnsResponsesBySurveyID(SurveyID, offset).ToList();
            return Json(new { SummaryResponse = lstSummary }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetAllIndividualResponseBySurveyID(int SurveyID, int offset = 0)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyResponse> lstInd = new List<SurveyResponse>();
            lstInd = objSurvey.GetAllQstnsIndResponsesBySurveyID(SurveyID).ToList();
            return Json(new { IndResponses = lstInd }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetAllQuestionTypes()
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<QuestionTypes> lst = new List<QuestionTypes>();
            lst = objSurvey.GetAllQuestionTypes().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetCascadingQuestionOptions()
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<CascadingQuestionOptions> lst = new List<CascadingQuestionOptions>();
            lst = objSurvey.GetCascadingQuestionOptions().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetCascadingQuestionSubOptions(int ParentOptionID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<CascadingQuestionOptions> lst = new List<CascadingQuestionOptions>();
            lst = objSurvey.GetCascadingQuestionSubOptions(ParentOptionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetAllLogicConditions()
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<LogicConditions> lst = new List<LogicConditions>();
            lst = objSurvey.GetAllLogicConditions().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetLogicAnswers(int QstnID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            lst = objSurvey.GetLogicAnswers(QstnID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetAllQstnsForSectionLogic(int SurveyID, int SectionID, string LogicType)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveySectionLogic> lst = new List<SurveySectionLogic>();
            lst = objSurvey.GetAllQstnsForSectionLogic(SurveyID, SectionID, LogicType).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetSectionsLogic(int SurveyID, int SectionID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveySectionLogic> lst = new List<SurveySectionLogic>();
            lst = objSurvey.GetSectionsLogic(SurveyID, SectionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetProjectTeam(int ProjectID, int WorkspaceID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<InitUserList> lst = new List<InitUserList>();
            lst = objSurvey.GetProjectTeam(ProjectID, WorkspaceID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetQuestionDetails(int QuestionID)
        {
            ProjectSurveyDAL objSurvey = new ProjectSurveyDAL();
            List<SurveyQuestion> lst = new List<SurveyQuestion>();
            lst = objSurvey.GetQuestionDetails(QuestionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetAllFormulaFunctions()
        {
            List<KeyValuePair<int, string>> lstCommonFn = new List<KeyValuePair<int, string>>();
            lstCommonFn = CommonActivity.GetCommonCalcFunctionList();
            return Json(new { data = lstCommonFn }, new JsonSerializerSettings());
        }
        #endregion


        [HttpPost]
        public IActionResult SaveProjectSurveySection(ProjectSurveySection objSurveySection)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool isSaved = false;

            if (loggedUserId > 0)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                if (objSurveySection.SectionID > 0)
                {
                    isSaved = objProjectSurveyDAL.UpdateProjectSurveySection(objSurveySection, loggedUserId);
                    status = (isSaved ? "success" : "failed");
                }
                else
                {
                    bool isExists = objProjectSurveyDAL.IsSectionNameExists(objSurveySection.SurveyID, objSurveySection.SectionName, objSurveySection.SectionID);

                    if (!isExists)
                    {

                        isSaved = objProjectSurveyDAL.AddProjectSurveySection(objSurveySection, loggedUserId);
                        status = (isSaved ? "success" : "failed");
                    }
                    else
                        status = "nameexists";
                }


            }
            return Json(new { status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult RemoveSurveySection(int SectionID)
        {
            var retStatus = "success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.RemoveSurveySection(SectionID, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateSectionOrder(string SectionOrderList)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.UpdateSectionOrder(SectionOrderList, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult AddSurveySectionQstn(string SecQstnList)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            int id = objProjectSurveyDAL.AddSurveySectionQstn(SecQstnList, loggedUserId);
            retStatus = id > 0 ? "success" : "Failed";
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateQuestion(int QuestionID, string QuestionName, int QuestionTypeID, string QuestionOptionList, int SurveyID, int SectionID,bool IsOtherOptionSelected)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.UpdateQuestion(QuestionID, QuestionName, QuestionTypeID, QuestionOptionList, loggedUserId, SurveyID, SectionID, IsOtherOptionSelected);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateQuestionOrder(string QstnList)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.UpdateQuestionOrder(QstnList, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateSectionLogic(int SurveyID, int SectionID, string LogicType, string LogicConditionList)
        {
            var retStatus = "";
            bool isNestedQuestion = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            // Delete and Insert Section Logic , Making IsActive = 0

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            List<SurveySectionLogic> lstlogicCondition = JsonConvert.DeserializeObject<List<SurveySectionLogic>>(LogicConditionList);
            if (lstlogicCondition != null)
            {

                List<SurveyQuestion> lstSurveyQuestion = objProjectSurveyDAL.GetSurveySectionLogicSectionId(SurveyID);

                if (lstSurveyQuestion != null && lstSurveyQuestion.Count > 0)
                {
                    foreach (SurveySectionLogic lgItem in lstlogicCondition)
                    {
                        if (lstSurveyQuestion.Where(x => x.QuestionID == lgItem.QuestionID).Count() > 0)
                        {
                            isNestedQuestion = true;
                            break;
                        }
                    }
                }


                if (!isNestedQuestion)
                {
                    foreach (SurveySectionLogic lgItem in lstlogicCondition)
                    {
                        retStatus = objProjectSurveyDAL.UpdateSectionLogic(SurveyID, SectionID, LogicType, lgItem.SectionLogicID, lgItem.QuestionID,
                                lgItem.ConditionID, lgItem.Answer, lgItem.LogicOperator, loggedUserId);
                        if (retStatus != "")
                            return Json(new { status = retStatus }, new JsonSerializerSettings());
                    }
                }
            }

            if(isNestedQuestion)
            {
                retStatus = "nestedQuestion";
            } 
            else
            {
                retStatus = retStatus == "" ? "success" : retStatus;
            }
   
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateQuestionSettings(SurveyQuestion objQstn)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.UpdateQuestionSettings(objQstn, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }



        [HttpPost]
        public IActionResult RemoveSurveySectionQuestion(int SectionID, int QuestionID)
        {
            var retStatus = "success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.RemoveSurveySectionQuestion(SectionID, QuestionID, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult DeleteSectionLogicItem(int SectionLogicID)
        {
            var retStatus = "success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            retStatus = objProjectSurveyDAL.DeleteSectionLogicItem(SectionLogicID, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        #endregion SurveySection
        public string ConvertToDate(string strDate)
        {
            string DateString = string.Empty;
            if (strDate != "")
            {
                string[] DateVal = strDate.Split("-");
                DateString = DateVal[2] + "-" + DateVal[0] + "-" + DateVal[1];
            }
            return DateString;
        }

        public async Task<string> GetPivotTableData(string url,int sectionid=0,int repeatenable = 0)
        {
            HttpClient _client = new HttpClient();
           
            if(sectionid >0)
            {
                url += "&sectionid=" + sectionid;
            }

            if (repeatenable > 0)
            {
                url += "&repeatenable=" + repeatenable;
            }

            url = Startup.AnalyticsURL + url;
            var resp = await _client.GetStringAsync(url);
            return resp;
        }

        public async Task<bool> SavePivotPreset(string PresetName, int SurveyID, string PresetData,int sectionId=0)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            status = objProjectSurveyDAL.SavePivotPreset(loggedUserId, PresetName, SurveyID, PresetData, sectionId);
            return status;
        }

        public async Task<bool> EditPivotPreset(string PresetName, int SurveyID, string PresetData)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            status = objProjectSurveyDAL.EditPivotPreset(loggedUserId, PresetName, SurveyID, PresetData);
            return status;
        }

        public async Task<bool> DeletePivotPreset(string PresetName, int SurveyID)
        {
            bool status = false;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            status = objProjectSurveyDAL.DeletePivotPreset(loggedUserId, PresetName, SurveyID);
            return status;
        }

        public async Task<string> RestorePivotPreset(string PresetName, int SurveyID)
        {
            string presetData = string.Empty;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            presetData = objProjectSurveyDAL.RestorePivotPreset(loggedUserId, PresetName, SurveyID);
            return presetData;
        }

        public async Task<List<PivotPresetData>> GetPivotPresetList(int SurveyID)
        {
            List<PivotPresetData> pivotList = new List<PivotPresetData>();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            pivotList = objProjectSurveyDAL.GetPivotPresetList(loggedUserId, SurveyID);
            return pivotList;
        }

        public async Task<bool> SaveShareLinkStatus(int SurveyID, string ShareLinkStatus)
        {
            bool status = false;
            int UserID = Convert.ToInt32(Startup.ShareLinkUserID);
            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            status = objProjectSurveyDAL.SaveShareLinkStatus(SurveyID, UserID, ShareLinkStatus);
            return status;
        }

        public async Task<List<string>> EncryptShareLinkURL(string SurveyID, string WorkspaceID)
        {
            EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
            string strEncryptionKey = objEncryptDecrypt.Encryptionkey();
            List<string> encryptedids = new List<string>();
            string surveyid = !string.IsNullOrEmpty(SurveyID) ? objEncryptDecrypt.EncryptData(SurveyID.Replace(" ", string.Empty), strEncryptionKey) : null;
            encryptedids.Add(surveyid);
            string workspaceid = !string.IsNullOrEmpty(WorkspaceID) ? objEncryptDecrypt.EncryptData(WorkspaceID.Replace(" ", string.Empty), strEncryptionKey) : null;
            encryptedids.Add(workspaceid);
            return encryptedids;
        }

        public int checkFileExists(string FileName, int ProjectID)
        {
            int isFileExists = 0;
            string surveyfilePath = _hostingEnvironment.WebRootPath + "/Upload/SureyQuestionnaire/" + ProjectID.ToString() + "/"+ FileName;
            string beneficiaryfilePath = _hostingEnvironment.WebRootPath + "/Upload/BeneficiaryRegistration/" + ProjectID.ToString() + "/"+ FileName;
            if (System.IO.File.Exists(surveyfilePath))
            {
                isFileExists = 1;
            }
            else if (System.IO.File.Exists(beneficiaryfilePath))
            {
                isFileExists = 2;
            }
            else
            {
                isFileExists = 0;
            }
            return isFileExists;
        }


        public IActionResult GetSurveyRepeatSection(string SurveyId)
        {
            List<KeyValuePair<int, string>> lstSection = new List<KeyValuePair<int, string>>();

            ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
            lstSection = objProjectSurveyDAL.GetSurveyRepeatSection(SurveyId);

            return Json(new { data = lstSection }, new JsonSerializerSettings());
        }
    }
}