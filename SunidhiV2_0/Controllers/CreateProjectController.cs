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
    public class CreateProjectController : Controller
    {

        #region CreateProject
        private IHostingEnvironment _hostingEnvironment;

        public CreateProjectController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost]
        public IActionResult GetProjectThemeList(int InitiativeID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ProjectTemplate> lstTheme = new List<ProjectTemplate>();
            lstTheme = objProject.GetProjectThemeList(InitiativeID).ToList();
            return Json(new { data = lstTheme }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetProjectTemplateList(int InitiativeID, int ThemeID, int NoOfTemplates)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            lstPrjTemp = objProject.GetProjectTemplateList(InitiativeID, ThemeID, NoOfTemplates).ToList();
            return Json(new { data = lstPrjTemp }, new JsonSerializerSettings());

        }


        [HttpPost]
        public ActionResult CreateProjectAsyncNew(Project objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            CreateProjectDAL objPrjDAL = new CreateProjectDAL();
            ProjectDAL objProjectDAL = new ProjectDAL();
            UserDAL objUser = new UserDAL();
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            InitiativeDAL objInitDAL = new InitiativeDAL();
            var id = 0;
            var idGroup = 0;
            var Result = "Failed";
            var retStatus = "";

            string password = string.Empty;
            int projectId = 0;

            try
            {
                using (var _scope = new System.Transactions.TransactionScope())
                {
                    id = objPrjDAL.CreateProject(objProject.ProjectName, objProject.Description, objProject.StartDate,
                            objProject.EndDate, objProject.ProjectTags,
                            objProject.InitiativeId, loggedUserId, objProject.WorkspaceID);

                    if (id > 0)
                    {
                        projectId = id;
                        idGroup = objUserGroupsDAL.AddGroup("Team" + id, objProject.Description, 0, loggedUserId, objProject.WorkspaceID);

                        var idUser = 0;
                        objProjectDAL.UpdateGroupInProject(id, idGroup, loggedUserId);
                        objInitDAL.AddGroupToInitiative(objProject.InitiativeId, idGroup, loggedUserId);
                        List<UsersDetails> objUserList = JsonConvert.DeserializeObject<List<UsersDetails>>(objProject.UserList);
                        foreach (UsersDetails userItem in objUserList)
                        {
                            idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, idGroup, loggedUserId);
                        }
                        Result = "success";
                    }
                }

                List<AssignedUserListDuringProjectCreation> objAssignedUsers =
                    JsonConvert.DeserializeObject<List<AssignedUserListDuringProjectCreation>>(objProject.AssignedUserListDuringProjectCreation);

                if (objProject != null && objAssignedUsers != null &&
                    objAssignedUsers.Count > 0)
                {
                    foreach (var assignedUser in objAssignedUsers)
                    {
                        bool bStatus = false;

                        UsersRoleInfo objUserRole = JsonConvert.DeserializeObject<UsersRoleInfo>(assignedUser.UserRole);
                        int roleId = objUserRole != null ? objUserRole.userRoleID : 0;
                        string roleName = objUserRole != null ? objUserRole.userRole : "";

                        var objWorkspaceInfo = objUser.GetWSInfoOfInvitedUser(assignedUser.EmailId);

                        if (assignedUser.TypeOfUser == "invite")
                        {
                            password = Code.Utils.CryptoUtils.RandomPasswordGenerator(8, false);

                            retStatus = objUser.AddInviteUser(assignedUser.EmailId, roleId,
                                loggedUserId, objProject.WorkspaceID, "", 0, projectId, password);

                            if (retStatus == "")
                            {
                                if (objWorkspaceInfo != null && objWorkspaceInfo.WorkspaceID != 0 &&
                                                                objWorkspaceInfo.WorkspaceID != assignedUser.WorkspaceID)
                                {
                                    if (objWorkspaceInfo.Name == null)
                                    {
                                        string[] names = !string.IsNullOrEmpty(assignedUser.EmailId) ? assignedUser.EmailId.Split("@") : null;

                                        if (names != null && names.Length > 0)
                                        {
                                            objWorkspaceInfo.Name = names[0];
                                        }
                                    }
                                    bStatus = objUser.SendEmailtoAssignedUsersFromWorkspace(assignedUser.EmailId, objWorkspaceInfo.Name,
                                        loggedUserId, assignedUser.WorkspaceID, projectId);
                                }
                                else
                                    bStatus = objUser.InviteUserFromWorkspaceOrProject(assignedUser.EmailId, roleName,
                                        User.Identity.Name, loggedUserId, password, "", 0, assignedUser.WorkspaceID, projectId);
                            }
                        }
                        else
                        {
                            bStatus = objUser.SendEmailtoAssignedUsersFromWorkspace(assignedUser.EmailId, objWorkspaceInfo.Name,
                                        loggedUserId, assignedUser.WorkspaceID, projectId);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Result = "Failed - " + ex.Message;
            }

            return Json(new { status = Result, ID = id, GroupID = idGroup, PrjtStartDate = objProject.StartDate, PrjtEndDate = objProject.EndDate }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateEditProjectDetails(Project inputObjProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var idUser = 0;
            string result = "success";
            CreateProjectDAL objPrjDAL = new CreateProjectDAL();
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();

            try
            {
                using (var _scope = new System.Transactions.TransactionScope())
                {
                    objPrjDAL.UpdateEditProjectDetails(inputObjProject, loggedUserId);
                    List<UsersDetails> objUserList = JsonConvert.DeserializeObject<List<UsersDetails>>(inputObjProject.UserList);
                    foreach (UsersDetails userItem in objUserList)
                    {
                        idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, inputObjProject.GroupId, loggedUserId);
                    }

                    _scope.Complete();
                }

                result = "success";
            }

            catch (Exception ex)
            {
                result = "failure";
            }

            return Json(new { status = result }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult FetchTemplatePreDefinedData(int TemplateId, int ThemeId)
        {
            ///var PrjectId = _ProjectID;
            List<ThemeTemplate> lstProjectclass = new List<ThemeTemplate>();

            string path = _hostingEnvironment.WebRootPath + "/Upload/Template_Data_Json/Template_json-format-data";
            var JSON = System.IO.File.ReadAllText(path);
            lstProjectclass = JsonConvert.DeserializeObject<List<ThemeTemplate>>(JSON);
            var template = lstProjectclass.Where(x => x.ThemeID == ThemeId.ToString() && x.ProjectTemplateID == TemplateId.ToString()).FirstOrDefault();

            return Json(new { data = template }, new JsonSerializerSettings());

        }

        public IActionResult fnSaveTemplateDatatoProject(ThemeTemplate objThemeTemplate, int ProjectId)
        {

            int LoggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int SurveyId = 0;
            CreateProjectDAL objPrjDAL = new CreateProjectDAL();
            string status = "Success";

            try
            {
                using (var _scope = new System.Transactions.TransactionScope())
                {
                    // Insert into Survey List
                    List<ProjectSurvey> ObjSurvey = JsonConvert.DeserializeObject<List<ProjectSurvey>>(objThemeTemplate.strSurveyList);
                    if (ObjSurvey.Count > 0)
                    {
                        var nSurveyCount = 0;
                        for (nSurveyCount = 0; nSurveyCount <= ObjSurvey.Count - 1; nSurveyCount++)
                        {
                            // SurveyId = objPrjDAL.InsertIntoSurvey(ObjSurvey[nSurveyCount], LoggedUserId, ProjectId);
                            // New Change  - Setting Project Start Date and EndDate as Surveys StartDate and EndDate
                            SurveyId = objPrjDAL.InsertIntoSurvey(ObjSurvey[nSurveyCount], LoggedUserId, ProjectId, objThemeTemplate.PrjtStartDate, objThemeTemplate.PrjtEndDate);
                        }
                    }

                    // Insert into Outcomes
                    List<ProjectOutcome> ObjOutcome = JsonConvert.DeserializeObject<List<ProjectOutcome>>(objThemeTemplate.strOutcomes);
                    if (ObjOutcome.Count > 0)
                    {
                        var nOutcomeCnt = 0;
                        for (nOutcomeCnt = 0; nOutcomeCnt <= ObjOutcome.Count - 1; nOutcomeCnt++)
                        {
                            objPrjDAL.InsertIntoOutcomes(ObjOutcome[nOutcomeCnt], LoggedUserId, ProjectId, SurveyId, objThemeTemplate.PrjtStartDate, objThemeTemplate.PrjtEndDate);
                        }
                    }

                    // insert into output
                    List<ProjectActivity> ObjOutput = JsonConvert.DeserializeObject<List<ProjectActivity>>(objThemeTemplate.strOutputs);
                    if (ObjOutput.Count > 0)
                    {
                        var nOutputCnt = 0;
                        for (nOutputCnt = 0; nOutputCnt <= ObjOutcome.Count - 1; nOutputCnt++)
                        {
                            objPrjDAL.InsertIntoOutput(ObjOutput[nOutputCnt], LoggedUserId, ProjectId, SurveyId, objThemeTemplate.PrjtStartDate, objThemeTemplate.PrjtEndDate);
                        }
                    }

                    _scope.Complete();
                    status = "Success";
                }
            }

            catch (Exception ex)
            {
                status = "failure : " + ex.Message;
            }

            return Json(new { status = status }, new JsonSerializerSettings());
        }

        #endregion CreateProject

        [HttpPost]
        public ActionResult SaveInputs(Double BudgetAmount, string Donors, string PaymnetTerms, string BudgetPlan, int ProjectId, int NoOfBudgetPlans)
        {

            // insert into Budget Plan
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            FinanceDAL objFinanceDAL = new FinanceDAL();
            ProjectDAL objProjectDAL = new ProjectDAL();
            CreateProjectDAL objCreateProjectDAL = new CreateProjectDAL();
            //ProjectFinanceDAL objPrjDAL = new ProjectFinanceDAL();
            var isValid = false;
            var idPayment = 0;
            var returnValue = "success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            string retSTatus = "";
            try
            {
                using (var _scope = new System.Transactions.TransactionScope())
                {
                    retSTatus = objCreateProjectDAL.CompleteProjectCreation(ProjectId, loggedUserId, BudgetAmount);

                    objCreateProjectDAL.DeleteBudgetPlanHead(ProjectId); // deleting existing projectBudgetplan  
                    objCreateProjectDAL.DeletePaymentTerms(ProjectId); // deleting existing PaymentTerms
                    objCreateProjectDAL.DeleteDonorsBudget(ProjectId); // deleting existing Donorsbudget
                    var id = objProject.AddProjectBudgetPlan(ProjectId, NoOfBudgetPlans, BudgetPlan, loggedUserId);
                    retSTatus = id == 0 ? "Error in Budget Plan" : "Success";
                    isValid = retSTatus == "Success" ? true : false;
                    // insert Payment Terms
                    List<PaymentTerms> objPaymentTermList = JsonConvert.DeserializeObject<List<PaymentTerms>>(PaymnetTerms);

                    foreach (PaymentTerms inputObj in objPaymentTermList)
                    {
                        if ((inputObj.PaymentTermName != "") && (inputObj.Amount != "0"))
                        {
                            idPayment = objProjectDAL.SavepaymentTerms(ProjectId, inputObj.PaymentTermName, inputObj.Logic, inputObj.paymentdate, inputObj.Target, inputObj.Amount, loggedUserId);
                        }
                    }
                    objProjectDAL.UpdatePaymentTermInProject(ProjectId, objPaymentTermList.Count, loggedUserId);
                    if (isValid && objPaymentTermList.Count > 0)
                    {
                        retSTatus = idPayment == 0 ? "Failed to save Payment Terms" : "Success";
                        isValid = retSTatus == "Success" ? true : false;
                    }

                    List<ProjectBudgetAndFundsDetails> objList = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(Donors);
                    foreach (ProjectBudgetAndFundsDetails usrItem in objList)
                    {
                        if (usrItem.UserID != 0)
                        {
                            returnValue = objProject.SaveProjectBudgetsDetails(ProjectId, usrItem.UserID, "D", loggedUserId, usrItem.DonorBudgetAmount, 0);
                        }
                    }
                    if (isValid && objList.Count > 0)
                    {
                        retSTatus = returnValue != "success" ? "Failed to save Payment Terms" : "Success";
                    }

                    retSTatus = "Success";
                    _scope.Complete();
                }
            }

            catch (Exception ex)
            {
                retSTatus = "failure: " + ex.Message;
            }


            return Json(new { status = retSTatus }, new JsonSerializerSettings());
        }


        public IActionResult GetUsersListByID(int WorkspaceID)
        {
            CreateProjectDAL objCreateProjectDAL = new CreateProjectDAL();
            var Users = objCreateProjectDAL.GetUsersListByID(WorkspaceID);
            return Json(new { data = Users }, new JsonSerializerSettings());
        }
    }
}
