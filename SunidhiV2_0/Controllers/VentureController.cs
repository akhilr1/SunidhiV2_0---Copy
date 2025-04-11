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
    public class VentureController : Controller
    {
        #region SearchVenture

        [HttpPost]
        public IActionResult GetProjectListOfVenture(int WorkspaceID)
        {
            int loggedUserId = 0;
            if (User.Claims.Count() > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            VentureDAL objVentureDAL = new VentureDAL();
            List<Project> lstProjects = new List<Project>();
            lstProjects = objVentureDAL.GetProjectListOfVenture(loggedUserId, WorkspaceID).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult InsertCommitFund(int ProjectId, string ProjectType, double Amount)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objVentureDAL.InsertCommitFund(ProjectId, ProjectType, Amount, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateCommitFund(int ProjectId, string ProjectType, double Amount)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objVentureDAL.UpdateCommitFund(ProjectId, ProjectType, Amount, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult AddToWishlist(string ProjectId, string ProjectType, double Amount, int Type)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            if (Type == 1)
            {
                List<Project> objProjectList = JsonConvert.DeserializeObject<List<Project>>(ProjectId);
                foreach (Project objProject in objProjectList)
                {
                    objVentureDAL.InsertCommitFund(objProject.ProjectId, ProjectType,Amount, loggedUserId);
                }
            }
            else
            {
                objVentureDAL.InsertCommitFund(Convert.ToInt32(ProjectId), ProjectType, Amount, loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetProjectListOfFundedAndWishList(string ProjectType, int WorkspaceID)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            List<Project> lstProjects = new List<Project>();
            lstProjects = objVentureDAL.GetProjectListOfFundedAndWishList(ProjectType, loggedUserId, WorkspaceID).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpGet]
        public IActionResult GetProjectAccounts()
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var accountList = objVentureDAL.GetProjectAccounts(loggedUserId);
            return Json(new { data = accountList }, new JsonSerializerSettings());
        }
        public ActionResult AddAccount(Account inputObj)
        {
            AccountDAL objAccountDAL = new AccountDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            inputObj.UserId = loggedUserId;

            int idAccount = objAccountDAL.AddAccount(inputObj, loggedUserId);
            return Json(new { status = "success", data = idAccount }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult GetProjectExpenses(int WorkspaceId)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var ExpensesList = objVentureDAL.GetProjectExpenses(loggedUserId,WorkspaceId);
            return Json(new { status = "success", data = ExpensesList }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult GetTotalProjectExpenses(int WorkspaceId)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var ExpensesList = objVentureDAL.GetTotalProjectExpenses(loggedUserId, WorkspaceId);
            return Json(new { status = "success", data = ExpensesList }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetFundedUsersById(int ProjectId)
        {
            VentureDAL objVentureDAL = new VentureDAL();
          
            List<Project> lstProjects = new List<Project>();
            lstProjects = objVentureDAL.GetFundedUsersById(ProjectId).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetTotalFundById(int ProjectId)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            var lstProjects = objVentureDAL.GetTotalFundById(ProjectId);
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetLedgerBookExpenseTagsById(int ProjectId)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            var lstProjects = objVentureDAL.GetLedgerBookExpenseTagsById(ProjectId);
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        #endregion SearchVenture

        #region MyVenture
        [HttpPost]
        public IActionResult DeleteVenture(string ProjectID, int Type)
        {
            VentureDAL objVentureDAL = new VentureDAL();
            if (Type == 1)
            {
                List<Project> objProjectList = JsonConvert.DeserializeObject<List<Project>>(ProjectID);
                foreach (Project objProject in objProjectList)
                {
                    objVentureDAL.DeleteVenture(objProject.ProjectId);
                }
            }
            else
            {
                objVentureDAL.DeleteVenture(Convert.ToInt32(ProjectID));

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion MyVenture
    }
}