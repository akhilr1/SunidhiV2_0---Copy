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
    public class ProjectFinanceController : Controller
    {
        #region Finance_BudgetAndFunds

        [HttpPost]
        public IActionResult GetProjectBudgetDetails(int ProjectID)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetProjectBudgetDetails(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetProjectUsers(int WorkspaceID, int GroupID, string Type)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetProjectUsers( WorkspaceID, GroupID, Type).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetDonorAccountDetails(int ProjectID, int UserID)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            List<ProjectBudgetAndFundsDetails> lst = new List<ProjectBudgetAndFundsDetails>();
            lst = objProject.GetDonorAccountDetails(ProjectID, UserID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult SaveProjectBudgetsDetails(ProjectBudgetAndFundsDetails objProject, bool ChkNotify)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objPrjDAL = new ProjectFinanceDAL();
            var retStatus = "";
            bool bStatus = true;
            ProjectDAL objProjectDAL = new ProjectDAL();
            string prjName = objProjectDAL.GetProjectName(objProject.ProjectID);

            List<ProjectBudgetAndFundsDetails> objList = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(objProject.ProjectUsers);
            foreach (ProjectBudgetAndFundsDetails usrItem in objList)
            {
                retStatus = objPrjDAL.SaveProjectBudgetsDetails(objProject.ProjectID, usrItem.UserID, usrItem.UserType, loggedUserId, usrItem.DonorBudgetAmount, usrItem.AccountId);
                //Send Notification     
                if (ChkNotify)
                {
                    EmailActivity objEmail = new EmailActivity();
                    NotifyExpenseTags objUsr = new NotifyExpenseTags();
                    objUsr.UserName = usrItem.UserName;
                    objUsr.ProjectName = prjName;
                    objUsr.UserEmailId = usrItem.EmailId;
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
            ProjectFinanceDAL objPrjDAL = new ProjectFinanceDAL();
            var retStatus = "";

            List<ProjectBudgetAndFundsDetails> objList = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(objProject.ProjectUsers);
            foreach (ProjectBudgetAndFundsDetails usrItem in objList)
            {
                retStatus = objPrjDAL.RemoveProjectBudgetUser(objProject.ProjectID, usrItem.UserID, loggedUserId, usrItem.UserType, usrItem.UserName);
            }

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateProjectBudget(int ProjectID, double TotalBudgetAmt)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objPrjDAL = new ProjectFinanceDAL();
            var retStatus = "";

            retStatus = objPrjDAL.UpdateProjectBudget(ProjectID, TotalBudgetAmt, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateDonorAmontAndAccount(ProjectBudgetAndFundsDetails objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objPrjDAL = new ProjectFinanceDAL();
            var retStatus = "";
            retStatus = objPrjDAL.UpdateDonorAmontAndAccount(objProject.ProjectID, objProject.UserID, objProject.DonorBudgetAmount, objProject.AccountId, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }

        #endregion Finance_BudgetAndFunds

        #region BudgetPlan
        [HttpPost]
        public IActionResult GetProjectBudgetPlanDetails(int ProjectID)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            List<ProjectBudgetPlan> lst = new List<ProjectBudgetPlan>();
            lst = objProject.GetProjectBudgetPlanDetails(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult AddProjectBudgetPlan(int ProjectID, int NoOfBudgetPlan, string BudgetPlanList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            var retStatus = "";

            int id = objProject.AddProjectBudgetPlan(ProjectID, NoOfBudgetPlan, BudgetPlanList, loggedUserId);
            retStatus = id > 0 ? "success" : "Failed";
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult UpdateProjectBudgetPlan(int ProjectID, int BudgetHeadID, string BudgetHead)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            var retStatus = "";

            retStatus = objProject.UpdateProjectBudgetPlan( ProjectID, BudgetHeadID, BudgetHead, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult UpdateProjectBudgetPlanSubList(int ProjectID, int BudgetHeadID, int BudgetHeadItemID, string BudgetHeadItemName, double ItemAmount)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            var retStatus = "";

            retStatus = objProject.UpdateProjectBudgetPlanSubList(ProjectID, BudgetHeadID, BudgetHeadItemID, BudgetHeadItemName, ItemAmount, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());

        }
        [HttpPost]
        public ActionResult RemoveBudgetPlanHead(int ProjectID, int BudgetHeadID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            var retStatus = objProject.RemoveBudgetPlanHead(ProjectID,BudgetHeadID, loggedUserId);           
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult RemoveBudgetPlanSubList(int ProjectID, int BudgetHeadID,int BudgetHeadItemID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            var retStatus = objProject.RemoveBudgetPlanSubList(ProjectID, BudgetHeadID,BudgetHeadItemID, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
        #endregion

        #region PaymentTerm
        [HttpPost]
        public ActionResult SavepaymentTerms(string PaymentTermList, int ProjectId, int NoOfPaymentTerms, bool notify)
        {
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
            ProjectDAL objProject = new ProjectDAL();
            string prjName = objProject.GetProjectName(ProjectId);


            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            List<PaymentTerms> objPaymentTermList = JsonConvert.DeserializeObject<List<PaymentTerms>>(PaymentTermList);

            foreach (PaymentTerms inputObj in objPaymentTermList)
            {
                var idPayment = objProjectDAL.SavepaymentTerms(ProjectId, inputObj.PaymentTermName, inputObj.Logic, inputObj.paymentdate, inputObj.Target, inputObj.Amount, loggedUserId);
                               
                foreach (ProjectBudgetAndFundsDetails userItem in inputObj.NotifyUserList)
                {
                    objProjectDAL.InsertpaymentTermsNotifyUsers(ProjectId, idPayment, userItem.UserID, loggedUserId);
                    //if (notify)
                    //{
                        bool bStatus = false;
                        EmailActivity objEmail = new EmailActivity();

                        NotifyExpenseTags objUsr = new NotifyExpenseTags();
                        objUsr.UserName = userItem.UserName;
                        objUsr.ProjectName = prjName;
                        objUsr.UserEmailId = userItem.EmailId;
                        //bStatus = objEmail.SendMail("PaymentTerm", obj);
                        bStatus = objEmail.SendMail("PaymentTermNotifyUsers", objUsr, loggedUserId);
                        if (!bStatus)
                            return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                    //}
                }
            }


            objProjectDAL.UpdatePaymentTermInProject(ProjectId, NoOfPaymentTerms, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetPaymentTermsByprjID(int ProjectId)
        {
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
            var group = objProjectDAL.GetPaymentTermsByprjID(ProjectId);

            return Json(new { data = group }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult DeletePaymentTerm(string PaymentId, int Type)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
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
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
            objProjectDAL.UpdatePaymnetStatus(PaymentId, PaymnetStatus, loggedUserId);
           
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetPaymentTermsByPaymentId(int PaymentId)
        {
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
            var group = objProjectDAL.GetPaymentTermsByPaymentId(PaymentId);

            return Json(new { data = group }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdatepaymentTerms(PaymentTerms objPayment, int ProjectId)
        //int PaymentId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, UsersDetails objUserList, int ProjectId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();
            ProjectDAL objProject = new ProjectDAL();
            string prjName = objProject.GetProjectName(ProjectId);

            objProjectDAL.UpdatepaymentTerms(objPayment.PaymentId, objPayment.PaymentTermName, objPayment.Logic, objPayment.paymentdate, objPayment.Target, objPayment.Amount, loggedUserId);
            objProjectDAL.DeletePaymentTermNotifyUsers(objPayment.PaymentId);

            List<ProjectBudgetAndFundsDetails> lstNotifyUser = JsonConvert.DeserializeObject<List<ProjectBudgetAndFundsDetails>>(objPayment.users);
            foreach (ProjectBudgetAndFundsDetails userItem in lstNotifyUser)
            {
                objProjectDAL.InsertpaymentTermsNotifyUsers(ProjectId, objPayment.PaymentId, userItem.UserID, loggedUserId);
                bool bStatus = false;
                EmailActivity objEmail = new EmailActivity();

                NotifyExpenseTags objUsr = new NotifyExpenseTags();
                objUsr.UserName = userItem.UserName;
                objUsr.ProjectName = prjName;
                objUsr.UserEmailId = userItem.EmailId;             
                bStatus = objEmail.SendMail("PaymentTermNotifyUsers", objUsr, loggedUserId);
                if (!bStatus)
                    return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
            }
            
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult SavePaymentRequest(ReuestFund inputObj)
        {
            ProjectFinanceDAL objProjectDAL = new ProjectFinanceDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int id = objProjectDAL.SavePaymentRequest(inputObj, loggedUserId);

            UserDAL objUserDAL = new UserDAL();
            LoggedUserInfo donoruserdetails = objUserDAL.GetLogUser(inputObj.DonorUser);

           
                bool bStatus = false;
                EmailActivity objEmail = new EmailActivity();
                RequestFundNotify obj = new RequestFundNotify();
                obj.UserName = donoruserdetails.Name;
                obj.FundAmount = inputObj.RequestAmount.ToString();
                obj.DonorUser = donoruserdetails.Name;
                obj.ProjectName = inputObj.ProjectName;
                obj.UserEmailId = donoruserdetails.EmailId;
                bStatus = objEmail.SendMail("Request Payment", obj);
           
            return Json(new { status = "success", data = id }, new JsonSerializerSettings());
        }
        #endregion PaymentTerm

        #region FundUtilisation
        [HttpPost]
        public IActionResult GetFundPaymentDeatils(int ProjectID)
        {
            ProjectFinanceDAL objProject = new ProjectFinanceDAL();
            List<ProjectBudgetPlan> lst = new List<ProjectBudgetPlan>();
            lst = objProject.GetFundPaymentDeatils(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion FundUtilisation
        #region AuditLog
        [HttpPost]
        public IActionResult GetAuditLog(int ProjectID)
        {
            ProjectFinanceDAL objFinanceDAL = new ProjectFinanceDAL();
            List<ActivityLog> lst = new List<ActivityLog>();
            lst = objFinanceDAL.GetAuditLog(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion AuditLog
    }
}
