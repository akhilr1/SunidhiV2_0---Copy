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
    public class ProjectBeneficiaryController : Controller
    {
        #region Get

        [HttpGet]
        public IActionResult GetAnswerTypes()
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<BeneficiaryAnswerType> lstAnswerType = new List<BeneficiaryAnswerType>();
            lstAnswerType = objProject.GetAnswerTypes().ToList();
            return Json(new { data = lstAnswerType }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetCascadingQuestionOptions()
        {
            ProjectBeneficiaryDAL objBen = new ProjectBeneficiaryDAL();
            List<BeneficiaryTypeCascadingQstnOptions> lst = new List<BeneficiaryTypeCascadingQstnOptions>();
            lst = objBen.GetCascadingQuestionOptions().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetCascadingQuestionSubOptions(int ParentOptionID)
        {
            ProjectBeneficiaryDAL objBen = new ProjectBeneficiaryDAL();
            List<BeneficiaryTypeCascadingQstnOptions> lst = new List<BeneficiaryTypeCascadingQstnOptions>();
            lst = objBen.GetCascadingQuestionSubOptions(ParentOptionID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetCascadingOptionsList(string optionValue, int parentID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<BeneficiaryTypeCascadingQstnOptions> lstBType = new List<BeneficiaryTypeCascadingQstnOptions>();
            lstBType = objProject.GetCascadingOptionsList(optionValue,  parentID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }
        [HttpPost]        
        public IActionResult GetAllBeneficiaryTypes(int ProjectID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<ProjectBeneficiaryType> lstBType = new List<ProjectBeneficiaryType>();
            lstBType = objProject.GetAllBeneficiaryTypes(ProjectID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetProjectBeneficiaryTypeForFilter(int WorkspaceID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<ProjectBeneficiaryType> lstBType = new List<ProjectBeneficiaryType>();
            lstBType = objProject.GetProjectBeneficiaryTypeForFilter(WorkspaceID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetAllBeneficiaries(int BeneficiaryTypeID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<ProjectBeneficiary> lstBType = new List<ProjectBeneficiary>();
            lstBType = objProject.GetAllBeneficiaries(BeneficiaryTypeID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetBeneficiaryTypeDetails(int BeneficiaryTypeID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<ProjectBeneficiaryType> lstBType = new List<ProjectBeneficiaryType>();
            lstBType = objProject.GetBeneficiaryTypeDetails(BeneficiaryTypeID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetBeneficiaryDetails(int BeneficiaryID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<BeneficiaryRegistration> lstBType = new List<BeneficiaryRegistration>();
            lstBType = objProject.GetBeneficiaryDetails(BeneficiaryID).ToList();
            return Json(new { data = lstBType }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetBeneficiaryQstnAnsw(string BeneficiaryIDs)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<BeneficiaryRegistration> lstBeneficiaryQstnAnsw = new List<BeneficiaryRegistration>();
            lstBeneficiaryQstnAnsw = objProject.GetBeneficiaryQstnAnsw(BeneficiaryIDs).ToList();
            return Json(new { data = lstBeneficiaryQstnAnsw }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetSurveyDetailsByBenefTypeID(int BeneficiaryTypeID)
        {
            ProjectBeneficiaryDAL objProject = new ProjectBeneficiaryDAL();
            List<ProjectSurvey> lst= new List<ProjectSurvey>();
            lst = objProject.GetSurveyDetailsByBenefTypeID(BeneficiaryTypeID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        #endregion Get

        #region CreateProjectBeneficiary

        [HttpPost]
        public ActionResult CreateBeneficiaryType(ProjectBeneficiaryType objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "";

            int id = objPrjDAL.CreateBeneficiaryType(objProject.ProjectID, objProject.Name, objProject.Description, objProject.BeneficiaryTypeTags, 
                                            objProject.BeneficiaryTypeQuestions, loggedUserId);
            retStatus = id > 0 ? "success" : "Failed";
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RegisterBeneficiary(ProjectBeneficiary objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "";

            DateTime now = DateTime.Now;
            
            var TokenValue = "";
            TokenValue = "Beneficiary_" + now.Year.ToString() + now.Month.ToString()+ now.Day.ToString() + now.Hour.ToString() + now.Millisecond.ToString() + now.Second.ToString() + now.Millisecond.ToString();
            int id = objPrjDAL.CreateBeneficiary(objProject.BeneficiaryName,objProject.BeneficiaryTypeID, objProject.BeneficiaryQstnAnswers, loggedUserId, TokenValue);
            retStatus = id > 0 ? "success" : "Failed";
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult ImportRegisterBeneficiary(string BenefImportList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "";

            int id = objPrjDAL.ImportRegisterBeneficiary(BenefImportList, loggedUserId);
            retStatus = id > 0 ? "success" : "Failed";
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult UpdateBeneficiary(string BenefUpdateList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "success";

            if (BenefUpdateList != null)
            {
                List<BeneficiaryRegistration> objList = JsonConvert.DeserializeObject<List<BeneficiaryRegistration>>(BenefUpdateList);
                string BenefName = objList[0].Answer;

                foreach (BeneficiaryRegistration lstItem in objList)
                {
                    retStatus = objPrjDAL.UpdateBeneficiary(lstItem.BeneficiaryTypeID, lstItem.BeneficiaryID, lstItem.QuestionID, lstItem.AnswerTypeID,
                                lstItem.Answer, BenefName, loggedUserId);
                }
            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult UpdateBeneficiaryTypeDetails(ProjectBeneficiaryType objProjectBT)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "";

            retStatus = objPrjDAL.UpdateBeneficiaryTypeDetails(objProjectBT.BeneficiaryTypeID, objProjectBT.Name, objProjectBT.Description,
                                    objProjectBT.BeneficiaryTypeTags, loggedUserId);
            
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateBeneficiaryTypeQuestions(ProjectBeneficiaryType objProjectBT)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "";

            if (objProjectBT.BeneficiaryTypeQuestions != null)
            {
                List<BeneficiaryTypeQuestions> objList = JsonConvert.DeserializeObject<List<BeneficiaryTypeQuestions>>(objProjectBT.BeneficiaryTypeQuestions);
                foreach (BeneficiaryTypeQuestions lstItem in objList)
                {
                    retStatus = objPrjDAL.UpdateBeneficiaryTypeQuestions(lstItem.BeneficiaryTypeID, lstItem.QuestionID, lstItem.QuestionName,
                                lstItem.AnswerTypeID, lstItem.IsUnique, lstItem.IsSearchable, lstItem.IsMandatoryResponse, lstItem.QuestionHint, lstItem.IsValidationQuestion, lstItem.RegexExpression,
                                lstItem.BTQstnAnsOptions, lstItem.BTQstnValidation, loggedUserId, lstItem.QuestionOrder);
                }
            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
       [HttpPost]
        public IActionResult IsUniqueResponse(string Answer)
        {
            string status = "success";
            if (Answer.Length > 0)
            {
                ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
                bool isExists = objPrjDAL.IsUniqueResponse(Answer);

                if (!isExists)
                {
                    status = "failed";
                }
            }
            return Json(new { status }, new JsonSerializerSettings());
        }

        public IActionResult CopyBeneficiaryQuestion(ProjectBeneficiaryType ObjBeneficiaryType)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            bool retStatus = false;
            if (ObjBeneficiaryType.BeneficiaryTypeQuestions != null)
            {
                //BeneficiaryTypeQuestions objList = JsonConvert.DeserializeObject<BeneficiaryTypeQuestions>(ObjBeneficiaryType.BeneficiaryTypeQuestions);
                retStatus = objPrjDAL.CopyBeneficiaryQuestion(ObjBeneficiaryType.BeneficiaryTypeID, ObjBeneficiaryType.BeneficiaryTypeQuestions, loggedUserId);
            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        #endregion CreateProjectBeneficiary        

        #region DeleteProjectBeneficiary

        [HttpPost]
        public ActionResult RemoveBeneficiaryType(string BeneficiaryTypeList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "Failed";
            if (BeneficiaryTypeList != null)
            {
                bool isExist = objPrjDAL.CheckBeneficiaryTypeInSurvey(BeneficiaryTypeList);
                if (isExist == false)
                {
                    List<ProjectBeneficiaryType> objList = JsonConvert.DeserializeObject<List<ProjectBeneficiaryType>>(BeneficiaryTypeList);
                    foreach (ProjectBeneficiaryType delItem in objList)
                    {
                        retStatus = objPrjDAL.RemoveBeneficiaryType(delItem.ProjectID, delItem.BeneficiaryTypeID, loggedUserId);
                    }
                }
            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RemoveBeneficiary(string BeneficiaryList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "success";
            if (BeneficiaryList != null)
            {
                List<ProjectBeneficiary> objList = JsonConvert.DeserializeObject<List<ProjectBeneficiary>>(BeneficiaryList);
                foreach (ProjectBeneficiary delItem in objList)
                { 
                    retStatus = objPrjDAL.RemoveBeneficiary(delItem.BeneficiaryID, loggedUserId);
                }
            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RemoveBeneficiaryTypeQuestion(int QuestionID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
            var retStatus = "success";

            retStatus = objPrjDAL.RemoveBeneficiaryTypeQuestion(QuestionID, loggedUserId);


            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        #endregion CreateProjectBeneficiary

        #region GetProjectBeneficiarySurveyQuestDataAPI
        /// <summary>
        /// GetProject Beneficiary questions & answers, survey questions & answers based on SyncTaskID
        /// </summary>
        [HttpGet]
        public IActionResult FetchAllBeneSurveyQuestDetailAPI(int SyncTaskID)
        {
            string status = "Success";

            ProjectBeneficiaryDAL objBenSurvDAL = new ProjectBeneficiaryDAL();
            var lstProjectBeneSurveyQuestAPI = objBenSurvDAL.GetDownloadSummaryForMyTask(SyncTaskID);
            if (lstProjectBeneSurveyQuestAPI == null)
            {
                status = "failed";
            }
            return Json(new { data = lstProjectBeneSurveyQuestAPI, status = status }, new JsonSerializerSettings());
        }
        #endregion GetProjectBeneficiarySurveyQuestDataAPI

        #region UpdateQuestionOrder
        /// <summary>
        /// UpdateQuestionOrder
        /// </summary>
        /// <param name="QstnList"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateQuestionOrder(string QstnList)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            ProjectBeneficiaryDAL objProjectBeneficiaryDAL = new ProjectBeneficiaryDAL();
            retStatus = objProjectBeneficiaryDAL.UpdateQuestionOrder(QstnList, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        #endregion UpdateQuestionOrder

        #region GetBeneficiaryAnswerOption
        /// <summary>
        /// GetBeneficiaryAnswerOption
        /// </summary>
        /// <param name="QstnID"></param>
        /// <returns></returns>
        public IActionResult GetBeneficiaryAnswerOption(int QstnID)
        {
            ProjectBeneficiaryDAL objProjectBeneficiaryDAL = new ProjectBeneficiaryDAL();
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            lst = objProjectBeneficiaryDAL.GetBeneficiaryAnswerOption(QstnID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }
        #endregion GetBeneficiaryAnswerOption
    }
}
