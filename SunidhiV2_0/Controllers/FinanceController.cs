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
    public class FinanceController : Controller
    {
        #region SearchFinance

        [HttpPost]
        public IActionResult GetProjectListOfFinance(int WorkspaceID)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            List<Project> lstProjects = new List<Project>();
            lstProjects = objFinanceDAL.GetProjectListOfFinance(loggedUserId, WorkspaceID).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult SaveDonarFund(int ProjectID, int UserID,  double DonorBudgetAmount, int AccountId, int BudgetFundID,string UserName,string AccountName)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var retStatus = "";
            if (BudgetFundID > 0)
            {
                retStatus = objFinanceDAL.UpdateDonarFund(ProjectID, BudgetFundID, UserID,  DonorBudgetAmount, AccountId,loggedUserId, UserName, AccountName);
            }
            else
            {
                retStatus = objPrjDAL.SaveProjectBudgetsDetails(ProjectID, UserID, "D", loggedUserId, DonorBudgetAmount, AccountId);
            }

           

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult RemoveBudgetUser(int ProjectID, int BudgetFundID,string DonorName)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var retStatus = "";

            retStatus = objFinanceDAL.RemoveBudgetUser(ProjectID, BudgetFundID, DonorName, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        #endregion SearchFinance
        #region BudgetPlan
        [HttpPost]
        public ActionResult SaveBudgetPlan(string BudgetDetails, int ProjectId,  int NoOfBudgetPlans)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            FinanceDAL objFinanceDAL = new FinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objProject.AddProjectBudgetPlan(ProjectId, NoOfBudgetPlans, BudgetDetails, loggedUserId);
            
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult UpdateBudgetPlan(int BudgetId,string BudgetPlanHeader,int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
          
             objFinanceDAL.UpdateBudgetPlan(ProjectId, BudgetId, BudgetPlanHeader,  loggedUserId);
          

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult UpdateBudgetPlanitem(int BudgetItemID, string ItemName, string Amount, int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            objFinanceDAL.UpdateBudgetPlanitem(BudgetItemID, ItemName, Amount, ProjectId, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        public ActionResult RemoveBudgetPlan(int BudgetId,string BudgetPlanHeader, int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int PaymnetId =objFinanceDAL.RemoveBudgetPlan(BudgetId, BudgetPlanHeader, ProjectId ,loggedUserId);
           

            return Json(new { status = "success", data = PaymnetId }, new JsonSerializerSettings());
        }
        public ActionResult RemoveBudgetPlanItems(int BudgetItemId,string ItemName,int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var paymnetId = objFinanceDAL.RemoveBudgetPlanItems(BudgetItemId, ItemName, ProjectId, loggedUserId);

            return Json(new { status = "success", data = paymnetId }, new JsonSerializerSettings());
        }
    
        [HttpPost]
        public IActionResult FetchBudgetPlan(int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var lstProjects = objFinanceDAL.FetchBudgetPlan(ProjectId);
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }


        #endregion BudgetPlan
        #region FinanceActivity
        [HttpPost]
        public ActionResult GetFinanceActivityList(int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var activitylist = objFinanceDAL.GetFinanceActivityList(ProjectId);

            return Json(new { status = "success", data = activitylist }, new JsonSerializerSettings());
        }

        [HttpGet]
        public ActionResult GetSubFinanceActivityListByID(int ActivityId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var activitylist = objFinanceDAL.GetSubFinanceActivityListByID(ActivityId); 
            return Json(new { status = "success", data = activitylist }, new JsonSerializerSettings());
        }
      
        [HttpPost]
        public IActionResult UpdateFinanceWFToActivity(int ProjectId,int ActivityId,string ActivityName, int FinanceWorkflowId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            FinanceDAL objFinanceDAL = new FinanceDAL();
             objFinanceDAL.UpdateFinanceWFToActivity(ProjectId,ActivityId, ActivityName, FinanceWorkflowId, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult GetPaymentListOfActivicty(int ProjectId, int ActivityId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var paymentlist = objFinanceDAL.GetPaymentListOfActivicty(ProjectId, ActivityId);

            return Json(new { status = "success", data = paymentlist }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetBudgetHeadDetails(int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            List<Finance> lst = new List<Finance>();
            lst = objFinanceDAL.GetBudgetHeadDetails(ProjectId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetBudgetItemDetails(int BudgetHeadID)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            List<BudgetDetails> lst = new List<BudgetDetails>();
            lst = objFinanceDAL.GetBudgetItemDetails(BudgetHeadID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetPaymentBudgetHead(int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            List<Finance> lst = new List<Finance>();
            lst = objFinanceDAL.GetPaymentBudgetHead(ProjectId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion FinanceActivity
        #region FinanceWorkflow
        [HttpPost]
        public IActionResult GetWorkflowsListToactivity(int ProjectId, int WorkspaceId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            var lstProjectWorkflows = objFinanceDAL.GetWorkflowsListToactivity(ProjectId, WorkspaceId);
            return Json(new { status = "success", data = lstProjectWorkflows }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateFinanceWorkflowLevelDetails(UpdateWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            FinanceDAL objFinanceDAL = new FinanceDAL();
            objFinanceDAL.UpdateFinanceWorkflowLevelCount(inputObj.WorkflowID, inputObj.WfNoOfLevels, loggedUserId);
            objFinanceDAL.DeleteFinanceWorkflowLevel(inputObj.WorkflowID, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objFinanceDAL.UpdateFinanceWorkflowLevelDetails(inputObj.WorkflowID, wfLevelItem.WfLevel, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
  
        [HttpPost]
        public IActionResult UpdateFinanceWorkflowDetails(Workflow inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            FinanceDAL objFinanceDAL = new FinanceDAL();
            objFinanceDAL.UpdateFinanceWorkflowDetails(inputObj.WorkflowID, inputObj.WfName, inputObj.WfDescription, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult AddFinanceWorkflow(AddWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            FinanceDAL objFinanceDAL = new FinanceDAL();
            var wfID = objFinanceDAL.AddFinanceWorkflow(inputObj, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objFinanceDAL.UpdateFinanceWorkflowLevelDetails(wfID, wfLevelItem.WfLevel, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost] 
        public ActionResult RemoveFinanceWorkflows(string WorkflowIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            FinanceDAL objFinanceDAL = new FinanceDAL();
            objFinanceDAL.RemoveFinanceWorkflows(loggedUserId, WorkflowIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion FinanceWorkflow
        #region AuditLog
        [HttpPost]
        public IActionResult GetAuditLog(int ProjectId)
        {
            FinanceDAL objFinanceDAL = new FinanceDAL();
            List<ActivityLog> lst = new List<ActivityLog>();
            lst = objFinanceDAL.GetAuditLog(ProjectId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion AuditLog
    }
}