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
    public class ProjectController : Controller
    {

        #region CreateProject

        [HttpGet]
        public ActionResult GetNextProjectCode()
        {
            ProjectDAL objProject = new ProjectDAL();
            var nextCode =  objProject.GetNextProjectCode();
            return Json(new { data = nextCode }, new JsonSerializerSettings());

        }
        [HttpGet]
        public IActionResult GetCountryDetails()
        {
            ProjectDAL objProject = new ProjectDAL();
            List<Country> lst = new List<Country>();
            lst = objProject.GetCountryDetails().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetStateDetails(int CountryID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<State> lst = new List<State>();
            lst = objProject.GetStateDetails(CountryID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetDistrictDetails(int StateID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<District> lst = new List<District>();
            lst = objProject.GetDistrictDetails(StateID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

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
        public ActionResult CreateProject(Project objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL(); 
            var retStatus = "";

            retStatus = objPrjDAL.CreateProject(objProject.ProjectName, objProject.Description, objProject.Code, objProject.Budget, objProject.StartDate, 
                                            objProject.EndDate, objProject.CountryID, objProject.StateID, objProject.DistrictID, objProject.ProjectTags, 
                                            objProject.ProjectTemplateID, objProject.InitiativeId, loggedUserId);                
            
            return Json(new { status = retStatus }, new JsonSerializerSettings());
           
        }

        #endregion CreateProject

        #region Document
        [HttpPost]
        public IActionResult GetProjectName(int ProjectID)
        {
            ProjectDAL objProject = new ProjectDAL();
            var ProjectName = objProject.GetProjectName(ProjectID);
            return Json(new { data = ProjectName }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetAllDocumentList(int ProjectID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<Document> lst = new List<Document>();
            lst = objProject.GetAllDocumentList(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        #endregion Documents

        #region Finance_BudgetAndFunds

        [HttpPost]
        public IActionResult GetProjectBudgetDetails(int ProjectID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetProjectBudgetDetails(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetProjectUsers(int GroupID, string Type)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetProjectUsers(GroupID, Type).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetDonorAccountDetails(int ProjectID, int UserID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetDonorAccountDetails(ProjectID, UserID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult SaveProjectBudgetsDetails(ProjectBudgetAndFundsDetails objProject, bool ChkNotify)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            var retStatus = "";
            bool bStatus = true;

            List<ProjectBudgetAndFundsDetails> objList = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(objProject.ProjectUsers);
            foreach (ProjectBudgetAndFundsDetails usrItem in objList)
            {
                retStatus = objPrjDAL.SaveProjectBudgetsDetails(objProject.ProjectID, usrItem.UserID, usrItem.UserType, loggedUserId, usrItem.DonorBudgetAmount, usrItem.AccountId);
                //Send Notification     
                if (ChkNotify)
                {
                    EmailActivity objEmail = new EmailActivity();
                    UsersInfo objUsr = new UsersInfo();
                    objUsr.Name = usrItem.UserName;
                    objUsr.EmailId = usrItem.EmailId;
                    bStatus = objEmail.SendMail("AssignFinanceUser", objUsr, loggedUserId);
                    if (!bStatus)
                        return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                }

            }

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult RemoveProjectBudgetUser(ProjectBudgetAndFundsDetails objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            var retStatus = "";

            List<ProjectBudgetAndFundsDetails> objList = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(objProject.ProjectUsers);
            foreach (ProjectBudgetAndFundsDetails usrItem in objList)
            {
                retStatus = objPrjDAL.RemoveProjectBudgetUser(objProject.ProjectID, usrItem.UserID, loggedUserId);
            }

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateProjectBudget(int ProjectID, double TotalBudgetAmt)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            var retStatus = "";

            retStatus = objPrjDAL.UpdateProjectBudget(ProjectID, TotalBudgetAmt, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateDonorAmontAndAccount(ProjectBudgetAndFundsDetails objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            var retStatus = "";
            retStatus = objPrjDAL.UpdateDonorAmontAndAccount(objProject.ProjectID, objProject.UserID, objProject.DonorBudgetAmount, objProject.AccountId, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }

        #endregion Finance_BudgetAndFunds

        #region DeployAndArchive

        [HttpPost]
        public IActionResult GetProjectListByInitID(string stage, int InitiativeId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            List<Project> lstProjects = new List<Project>();
            lstProjects = objProjectDAL.GetProjectListByInitID(stage, InitiativeId).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult DeleteProject(string ProjectID, int Type)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            if (Type == 1)
            {
                List<Project> objProjectList = JsonConvert.DeserializeObject<List<Project>>(ProjectID);
                foreach (Project prjt in objProjectList)
                {
                    objProjectDAL.DeleteProject(prjt.ProjectId);
                }
            }
            else
            {
                objProjectDAL.DeleteProject(Convert.ToInt32(ProjectID));

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ChangeProjectStatus(string ProjectID, string  ProjectStatus)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
       
                objProjectDAL.ChangeProjectStatus(Convert.ToInt32(ProjectID), ProjectStatus);
       
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ChangeState(string ProjectID, bool State)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);


            objProjectDAL.ChangeState(Convert.ToInt32(ProjectID), loggedUserId, State);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion DeployAndArchive
        #region ManageProjectDetails
        [HttpGet]
        public IActionResult GetProjectDate(int projectId = 0)
        {


            ProjectDAL objProjectDAL = new ProjectDAL();
            dynamic obj = objProjectDAL.GetProjectDate(projectId);

            return Json(new { PrjStartDate = obj.StartDate, PrjEndDate = obj.EndDate }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetProjectDetailsByID(int ProjectId, int InitiativeId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var project = objProjectDAL.GetProjectDetailsByID(ProjectId, InitiativeId);
            return Json(new { data = project }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetCountryStatesDistricts()
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var countrylist = objProjectDAL.GetCountryDetails();
            var statesList = objProjectDAL.GetStateList();
            var districtsList = objProjectDAL.GetDistrictList();
            return Json(new { data = new { countries = countrylist, states = statesList, districts = districtsList } }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateEditProjectDetails(Project inputObjProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
            objProjectDAL.UpdateEditProjectDetails(inputObjProject, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion ManageProjectDetails
        #region ManageTeam

        [HttpPost]
        public IActionResult GetUsersListByID(int WorkspaceID, int InitiativeId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var project = objProjectDAL.GetUsersListByID(WorkspaceID, InitiativeId);
            return Json(new { data = project }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult CreateTeam(AddGroup inputObj,int ProjectId,int InitID)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            InitiativeDAL objInitDAL = new InitiativeDAL();

            var idGroup = inputObj.GroupId;
            var notify = inputObj.Notify;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            

            if (idGroup == 0)
            {
                idGroup = objUserGroupsDAL.AddGroup(inputObj.GroupName, inputObj.Description, 0, loggedUserId, inputObj.WorkspaceId);
            }
            var idUser = 0;
            objProjectDAL.UpdateGroupInProject(ProjectId, idGroup, loggedUserId);
            objInitDAL.AddGroupToInitiative(InitID, idGroup, loggedUserId);
            List<userGroupList> objUserList = JsonConvert.DeserializeObject<List<userGroupList>>(inputObj.UserList);
            foreach (userGroupList userItem in objUserList)
            {
                idUser = objUserGroupsDAL.AddUserToGroup(userItem.userId, idGroup, loggedUserId);
                if (notify)
                {
                    bool bStatus = false;
                    EmailActivity objEmail = new EmailActivity();

                    //Get ClientName by userID- logged userID
                    //string ClientName = "TestClient"; //GetClientNameById(userID);
                    GroupNotify obj = new GroupNotify();
                    obj.UserName = userItem.userName;
                    obj.GroupName = inputObj.GroupName;
                    obj.UserEmailId = userItem.EmailId;
                    bStatus = objEmail.SendMail("Group Add Users", obj);
                }
            }
           
            return Json(new { status = "success", data = idGroup }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetGroupDetailsByprjID(int ProjectId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var group = objProjectDAL.GetGroupDetailsByprjID(ProjectId);
            return Json(new { data = group }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetTeamUsersList(int WorkspaceID,int GroupId, int InitiativeId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var team = objProjectDAL.GetTeamUsersList(WorkspaceID, GroupId, InitiativeId);
            return Json(new { data = team }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetGroupUsersList(int WorkspaceID, int GroupId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var team = objProjectDAL.GetGroupUsersList(WorkspaceID, GroupId);
            return Json(new { data = team }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult DeleteTeamUser(int ProjectID,int GroupId, string UserID)
        {
            var retStatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
            List<UsersDetails> objUserList = JsonConvert.DeserializeObject<List<UsersDetails>>(UserID);
            foreach (UsersDetails user in objUserList)
                retStatus = objProjectDAL.DeleteTeamUser(ProjectID,GroupId, user.ID, loggedUserId);
          
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetGroupsListToProject(int WorkspaceID)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var lstInitiativeGroups = objProjectDAL.GetGroupsListToProject(WorkspaceID);
            return Json(new { status = "success", data = lstInitiativeGroups }, new JsonSerializerSettings());
        }
        public IActionResult UpdateGroupInProject(int ProjectId,int GroupId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
           objProjectDAL.UpdateGroupInProject(ProjectId, GroupId, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetUsersListNotInTeam(int WorkspaceID, int InitiativeId, int GroupId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var lstUsers = objProjectDAL.GetUsersListNotInTeam(WorkspaceID, InitiativeId, GroupId);
            return Json(new { data = lstUsers }, new JsonSerializerSettings());

        }


        #endregion ManageTeam
        #region ManagePaymentTerm
        [HttpPost]
        public ActionResult SavepaymentTerms(string PaymentTermList, int ProjectId, int NoOfPaymentTerms, bool notify)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
           

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            List<PaymentTerms> objPaymentTermList = JsonConvert.DeserializeObject<List<PaymentTerms>>(PaymentTermList);

            foreach (PaymentTerms inputObj in objPaymentTermList)
            {
                 var idPayment =objProjectDAL.SavepaymentTerms(ProjectId, inputObj.PaymentTermName, inputObj.Logic, inputObj.paymentdate, inputObj.Target, inputObj.Amount, loggedUserId);

       

                foreach (UsersDetails userItem in inputObj.UserList)
                {
                    objProjectDAL.InsertpaymentTermsNotifyUsers(ProjectId, idPayment, userItem.ID, loggedUserId);
                    if (notify)
                    {
                        bool bStatus = false;
                        EmailActivity objEmail = new EmailActivity();

                        GroupNotify obj = new GroupNotify();
                        obj.UserName = userItem.Name;
                        obj.UserEmailId = userItem.EmailId;
                        bStatus = objEmail.SendMail("PaymentTerm", obj);
                    }
                }
            }

     
            objProjectDAL.UpdatePaymentTermInProject(ProjectId, NoOfPaymentTerms, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetPaymentTermsByprjID(int ProjectId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var group = objProjectDAL.GetPaymentTermsByprjID(ProjectId);

            return Json(new { data = group }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult DeletePaymentTerm(string PaymentId, int Type)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
            if (Type == 1)
            {
                List<PaymentTerms> objPaymentList = JsonConvert.DeserializeObject<List<PaymentTerms>>(PaymentId);
                foreach (PaymentTerms payment in objPaymentList)
                {
                    objProjectDAL.DeletePaymentTerm(payment.PaymentId, loggedUserId);
                }
            }
            else
            {
                objProjectDAL.DeletePaymentTerm(Convert.ToInt32(PaymentId), loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        public IActionResult UpdatePaymnetStatus(int PaymentId, string PaymnetStatus)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            {
                objProjectDAL.UpdatePaymnetStatus(PaymentId, PaymnetStatus, loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetPaymentTermsByPaymentId(int PaymentId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var group = objProjectDAL.GetPaymentTermsByPaymentId(PaymentId);

            return Json(new { data = group }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdatepaymentTerms(PaymentTerms objPayment, int ProjectId)
            //int PaymentId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, UsersDetails objUserList, int ProjectId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
         
            objProjectDAL.UpdatepaymentTerms(objPayment.PaymentId, objPayment.PaymentTermName, objPayment.Logic, objPayment.paymentdate, objPayment.Target, objPayment.Amount, loggedUserId);
            objProjectDAL.DeletePaymentTermNotifyUsers(objPayment.PaymentId);
            List<UsersDetails> lstNotifyUser = JsonConvert.DeserializeObject<List<UsersDetails>>(objPayment.users);
            foreach (UsersDetails userItem in lstNotifyUser)
            {
                objProjectDAL.InsertpaymentTermsNotifyUsers(ProjectId, objPayment.PaymentId, userItem.ID, loggedUserId);
                    bool bStatus = false;
                    EmailActivity objEmail = new EmailActivity();

                    GroupNotify obj = new GroupNotify();
                    obj.UserName = userItem.Name;
                    obj.UserEmailId = userItem.EmailId;
                    bStatus = objEmail.SendMail("PaymentTerm", obj);
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetNotifyUsers(int GroupID, string Type, int WorkspaceID)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<UsersDetails> lst = new List<UsersDetails>();
            lst = objProject.GetNotifyUsers(GroupID, Type, WorkspaceID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        #endregion ManagePaymentTerm
        #region ManageProjectRisk
        [HttpPost]
        public IActionResult CreateRisk(Risk objRisk, int ProjectId, bool notify)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var idrisk = objRisk.riskId;
            string status = "Failed";
            bool RFAvailable = true;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            if (idrisk == 0)
            {
                    if (objRisk.RiskPeriodicity != 1 && objRisk.RiskPeriodicity != 2 && objRisk.RiskPeriodicity != 3 && objRisk.RiskPeriodicity != 9)
                        RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonth, objRisk.ReportingMonthDay, objRisk.ReportingWeek, objRisk.ReportingMonthDateDay);
               
                     if (RFAvailable)
                    {
                        idrisk = objProjectDAL.CreateRisk(ProjectId, objRisk.RiskName, objRisk.RiskType, objRisk.Description,objRisk.RiskPeriodicity, objRisk.RiskRating, objRisk.StartDate, objRisk.EndDate, objRisk.WorkflowID, loggedUserId);
                    if (idrisk > 0)
                    {
                        status = "success";
                        objRisk.riskId = idrisk;
                        objProjectDAL.AssignUserstoRisk(objRisk, loggedUserId, notify);
                        objProjectDAL.GenerateReportingFrequencyTrackDateRisk(objRisk, loggedUserId, objRisk.riskId);
                    }
                }
                else
                {
                    status = "rptfrequencyfailed";
                }
            }
            return Json(new { status  }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult AssignUserstoRisk(Risk objRisk, bool notify)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                bool isAssigned = false;
                ProjectDAL objProjectDAL = new ProjectDAL();
                isAssigned = objProjectDAL.AssignUserstoRisk(objRisk, loggedUserId, notify);

                status = (isAssigned ? "success" : "failed");
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetWorkflows(int ProjectId, int WorkspaceID)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var workflowlist = objProjectDAL.GetWorkflows(ProjectId, WorkspaceID);

            return Json(new { data = new { workflows = workflowlist } }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetRiskByprjID(int ProjectId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var risk = objProjectDAL.GetRiskByprjID(ProjectId);

            return Json(new { data = risk }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult DeleteRisk(string riskId, int Type)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
            if (Type == 1)
            {
                List<Risk> objRiskList = JsonConvert.DeserializeObject<List<Risk>>(riskId);
                foreach (Risk objRisk in objRiskList)
                {
                    objProjectDAL.DeleteRisk(objRisk.riskId, loggedUserId);
                }
            }
            else
            {
                objProjectDAL.DeleteRisk(Convert.ToInt32(riskId), loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetRiskByriskId(int riskId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var risk = objProjectDAL.GetRiskByriskId(riskId);

            return Json(new { data = risk }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateRisk(Risk objRisk, int ProjectId, bool notify)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            string status = "Failed";
            bool RFAvailable = true;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (objRisk.RiskPeriodicity != 1 && objRisk.RiskPeriodicity != 2 && objRisk.RiskPeriodicity != 3 && objRisk.RiskPeriodicity != 9)
                RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonth, objRisk.ReportingMonthDay, objRisk.ReportingWeek, objRisk.ReportingMonthDateDay);

            if (RFAvailable)
            {
                objProjectDAL.GenerateReportingFrequencyTrackDateRisk(objRisk, loggedUserId, objRisk.riskId);

                objProjectDAL.UpdateRisk(objRisk, loggedUserId);
                if (objRisk.riskId > 0)
                {
                    status = "success";
                    objProjectDAL.AssignUserstoRisk(objRisk, loggedUserId, notify);
                }
            }
            else
            {
                status = "rptfrequencyfailed";
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetActivityLog(int ProjectId, int RiskId)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ActivityLog> lst = new List<ActivityLog>();
            lst = objProject.GetActivityLog(ProjectId, RiskId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult UpdateWorkflow(int WorkFlowID, int ProjectId, int RiskId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objProjectDAL.UpdateWorkflow(WorkFlowID, ProjectId, loggedUserId, RiskId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ImportRisk(string ImportRiskList, int ProjectID)
        {
            var retMsg = "success";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            try
            {
                ProjectDAL objProjectDAL = new ProjectDAL();
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                List<Risk> lstRisk = JsonConvert.DeserializeObject<List<Risk>>(ImportRiskList);
                var rptFrequency = "";
                foreach (Risk Item in lstRisk)
                {
                    Risk objRisk = new Risk();

                    if (Item != null)
                    {
                        objRisk.ProjectID = ProjectID;
                        objRisk.RiskName = Item.RiskName;
                        objRisk.Description = Item.Description;
                        objRisk.RiskType = Item.RiskType;
                        objRisk.RiskRating = Item.RiskRating;
                        rptFrequency = char.ToUpper(Item.ReportingFrequencyName[0]) + Item.ReportingFrequencyName.Substring(1);

                        objRisk.WorkflowID = (!string.IsNullOrEmpty(Item.WorkflowName) ? objProjectSurveyDAL.GetWorkFlowIdByName(ProjectID, Item.WorkflowName) : 0);
                        objRisk.RiskPeriodicity = (int)Enum.Parse(typeof(CommonActivity.ReportingFrequency), rptFrequency);


                        objProjectDAL.CreateRisk(ProjectID, objRisk.RiskName, objRisk.RiskType, objRisk.Description, objRisk.RiskPeriodicity, objRisk.RiskRating, Item.StartDate, Item.EndDate, objRisk.WorkflowID, loggedUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while saving data";


            }
            return Json(new { status = retMsg }, new JsonSerializerSettings());       

        }
        #endregion ManageProjectRisk

        #region ManageProjectKPI
        [HttpPost]
        public IActionResult CreateKPI(KPI objKPI, int ProjectId, bool notify)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var idKPI = objKPI.KPIId;
            string status = "Failed";

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool RFAvailable = true;
            if (idKPI==0)
            {
                if (objKPI.KPIPeriodicity != 1 && objKPI.KPIPeriodicity != 2 && objKPI.KPIPeriodicity != 3 && objKPI.KPIPeriodicity != 9)
                    RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonth, objKPI.ReportingMonthDay, objKPI.ReportingWeek, objKPI.ReportingMonthDateDay);
                if (RFAvailable) 
                {
                                   
                    idKPI = objProjectDAL.CreateKPI(ProjectId, objKPI.KPIName,objKPI.Description, objKPI.KPIPeriodicity, objKPI.StartDate, objKPI.EndDate, objKPI.WorkflowID,objKPI.SurveyID, loggedUserId);
                    
                    if (idKPI > 0)
                            {
                                status = "success";
                                objKPI.KPIId= idKPI;
                                objProjectDAL.AssignTagstoKPI(objKPI, loggedUserId);
                                objProjectDAL.AssignUserstoKPI(objKPI, loggedUserId, notify);
                                objProjectDAL.GenerateReportingFrequencyTrackDate(objKPI, loggedUserId, idKPI);
                    }
                }
                else
                {
                    status = "rptfrequencyfailed";
                }
            }
            
            return Json(new { status }, new JsonSerializerSettings());
        }
        public IActionResult AssignUserstoKPI(KPI objKPI, bool notify)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (loggedUserId > 0)
            {
                bool isAssigned = false;
                ProjectDAL objProjectDAL = new ProjectDAL();
                isAssigned = objProjectDAL.AssignUserstoKPI(objKPI, loggedUserId, notify);

                status = (isAssigned ? "success" : "failed");
            }

            return Json(new { status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetKPIWorkflows(int ProjectId, int WorkspaceID)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var workflowlist = objProjectDAL.GetWorkflows(ProjectId, WorkspaceID);

            return Json(new { data = new { workflows = workflowlist } }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetKPIByprjID(int ProjectId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var KPI = objProjectDAL.GetKPIByprjID(ProjectId);

            return Json(new { data = KPI }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult DeleteKPI(string KPIId, int Type)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objProjectDAL = new ProjectDAL();
            if (Type == 1)
            {
                List<KPI> objRiskList = JsonConvert.DeserializeObject<List<KPI>>(KPIId);
                foreach (KPI objKPI in objRiskList)
                {
                    objProjectDAL.DeleteKPI(objKPI.KPIId, loggedUserId);
                }
            }
            else
            {
                objProjectDAL.DeleteKPI(Convert.ToInt32(KPIId), loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetKPIByKPIId(int KPIId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var KPI = objProjectDAL.GetKPIByKPIId(KPIId);

            return Json(new { data = KPI }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateKPI(KPI objKPI, int ProjectId, bool notify)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            string status = "Failed";
            bool RFAvailable = true;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (objKPI.KPIPeriodicity != 1 && objKPI.KPIPeriodicity != 2 && objKPI.KPIPeriodicity != 3 && objKPI.KPIPeriodicity != 9)
                RFAvailable = CommonActivity.IsReportingFrequencyAvailable(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonth, objKPI.ReportingMonthDay, objKPI.ReportingWeek, objKPI.ReportingMonthDateDay);
            if (RFAvailable)
            {

                objProjectDAL.GenerateReportingFrequencyTrackDate(objKPI, loggedUserId, objKPI.KPIId);

                objProjectDAL.UpdateKPI(objKPI, loggedUserId); 

                if (objKPI.KPIId > 0)
                {
                    status = "success";
                   
                    objProjectDAL.AssignTagstoKPI(objKPI, loggedUserId);
                    objProjectDAL.AssignUserstoKPI(objKPI, loggedUserId, notify);
                }
            }
            else
            {
                status = "rptfrequencyfailed";
            }


            return Json(new { status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetKPIActivityLog(int ProjectId, int OutcomeId)
        {
            ProjectDAL objProject = new ProjectDAL();
            List<ActivityLog> lst = new List<ActivityLog>();
            lst = objProject.GetKPIActivityLog(ProjectId, OutcomeId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult UpdateKPIWorkflow(int WorkFlowID, int ProjectId, int KPIId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objProjectDAL.UpdateKPIWorkflow(WorkFlowID, ProjectId, loggedUserId, KPIId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ImportKPI(string ImportKPIList, int ProjectID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var retMsg = "success";
            try
            {
               
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                ProjectDAL objProjectDAL = new ProjectDAL();
                List<KPI> lstKPI = JsonConvert.DeserializeObject<List<KPI>>(ImportKPIList);
                var rptFrequency = "";

                foreach (KPI Item in lstKPI)
                {
                    KPI objKPI = new KPI();
                    if (Item != null)
                    {
                        objKPI.ProjectID = ProjectID;
                        objKPI.KPIName = Item.KPIName;
                        objKPI.Description = Item.Description;
                        rptFrequency = char.ToUpper(Item.ReportingFrequencyName[0]) + Item.ReportingFrequencyName.Substring(1);

                        objKPI.WorkflowID = (!string.IsNullOrEmpty(Item.WorkflowName) ? objProjectSurveyDAL.GetWorkFlowIdByName(ProjectID, Item.WorkflowName) : 0);
                        objKPI.KPIPeriodicity = (int)Enum.Parse(typeof(CommonActivity.ReportingFrequency), rptFrequency);
                        objKPI.SurveyID = (!string.IsNullOrEmpty(Item.SurveyName) ? objProjectDAL.GetSurveyIdByName(ProjectID, Item.SurveyName) : 0);

                        objProjectDAL.CreateKPI(ProjectID, objKPI.KPIName, objKPI.Description, objKPI.KPIPeriodicity, Item.StartDate, Item.EndDate, objKPI.WorkflowID, objKPI.SurveyID, loggedUserId);
                    }


                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while saving data";
                

            }       
            return Json(new { status = retMsg }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult UpdateTagDetails(int TagID, string TagName, string TagColor)
        {
            TagsDAL objTag = new TagsDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objTag.UpdateTagDetails(TagID, TagName, TagColor, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult CreateNewTag(string TagName, string TagColor, int InitiativeId)
        {
            TagsDAL objTag = new TagsDAL();
            var Tagstatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            Tagstatus=objTag.AddNewProjectTag(TagName, TagColor, loggedUserId, InitiativeId);

            return Json(new { status = Tagstatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetSurveyName(int ProjectId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            var Surveylist = objProjectDAL.GetSurveyName(ProjectId);

            return Json(new { data = new { Survey = Surveylist } }, new JsonSerializerSettings());
        }
        #endregion ManageProjectKPI     

    }
}
