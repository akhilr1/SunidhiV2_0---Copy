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

namespace SunidhiV2_0.Controllers
{
    public class AccountController : Controller
    {
        #region ManageProjectAccount
        [HttpPost]
        public IActionResult GetProjectAccountsByID(int ProjectId)
        {
            AccountDAL objAccountDAL = new AccountDAL();
            var accountList = objAccountDAL.GetProjectAccountsByID(ProjectId);
            return Json(new { data = accountList }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetBankList()
        {
            AccountDAL objAccountDAL = new AccountDAL();
            var banklist = objAccountDAL.GetBankList();
            return Json(new { data = banklist }, new JsonSerializerSettings());
        }
        public IActionResult GetBranchList(string BankName)
        {
            AccountDAL objAccountDAL = new AccountDAL();
            var branchlist = objAccountDAL.GetBranchList(BankName);
            return Json(new { data = branchlist }, new JsonSerializerSettings());
        }
        public IActionResult GetIFSC(string BankName, string BranchName)
        {
            AccountDAL objAccountDAL = new AccountDAL();
            string ifsc = objAccountDAL.GetIFSC(BankName, BranchName);
            return Json(new { data = ifsc }, new JsonSerializerSettings());
        }
        public IActionResult GetAccountTypeList()
        {
            AccountDAL objAccountDAL = new AccountDAL();
            var acctypelist = objAccountDAL.GetAccountTypeList();
            return Json(new { data = acctypelist }, new JsonSerializerSettings());
        }
        public ActionResult AddAccount(Account inputObj)
        {
            AccountDAL objAccountDAL = new AccountDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int idAccount = objAccountDAL.AddAccount(inputObj, loggedUserId);
            return Json(new { status = "success", data = idAccount }, new JsonSerializerSettings());
        }
        public ActionResult EditAccount(Account inputObj)
        {
            AccountDAL objAccountDAL = new AccountDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objAccountDAL.EditAccount(inputObj, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //remove/delete workspace
        public ActionResult RemoveAccount(string AccountIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            AccountDAL objAccountDAL = new AccountDAL();
            objAccountDAL.RemoveAccount(loggedUserId, AccountIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion ManageProjectAccount
    }
}
