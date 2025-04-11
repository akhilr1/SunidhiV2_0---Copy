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
using System.Data;
using OfficeOpenXml;
using System.Net.Http.Headers;

namespace SunidhiV2_0.Controllers
{
    public class LedgerBookController : Controller
    {
        #region ManageProjectLedgerBook
        [HttpGet] 
        public ActionResult GetPaymentList(int ProjectId, int AccountId)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();
            var paymentlist = objLedgerBookDAL.GetPaymentList(ProjectId,AccountId);

            return Json(new { status = "success", data = paymentlist }, new JsonSerializerSettings());
        }

        [HttpGet]
        public ActionResult GetActivityList(int ProjectId)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();
            var activitylist = objLedgerBookDAL.GetActivityList(ProjectId);

            return Json(new { status = "success", data = activitylist }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetProjectAccountsByType(int ProjectId, string AccountType)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();
            var accountList = objLedgerBookDAL.GetProjectAccountsByType(ProjectId,AccountType);
            return Json(new { data = accountList }, new JsonSerializerSettings());
        }

        
        [HttpPost]
        public ActionResult AddPayment(LedgerBook inputObj, string ExpenseTagsList)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int idPayment = objLedgerBookDAL.AddPayment(inputObj, ExpenseTagsList, loggedUserId);
            
            return Json(new { status = "success", data = idPayment }, new JsonSerializerSettings());
        }

        [HttpPost] 
        public ActionResult RemovePayment(string PaymentIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();
            objLedgerBookDAL.RemovePayment(loggedUserId, PaymentIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult EditPayment(LedgerBook inputObj, string ExpenseTagsList)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objLedgerBookDAL.EditPayment(inputObj, ExpenseTagsList, loggedUserId);
            
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AddRequestFund(ReuestFund inputObj)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int id = objLedgerBookDAL.AddRequestFund(inputObj, loggedUserId);

            UserDAL objUserDAL = new UserDAL();
            LoggedUserInfo donoruserdetails = objUserDAL.GetLogUser(inputObj.DonorUser);

            List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.NotifyUserList);
            foreach (UsersInfo userItem in objUserList)
            {
                LoggedUserInfo userItemDetails = objUserDAL.GetLogUser(userItem.ID);
                bool bStatus = false;
                EmailActivity objEmail = new EmailActivity();
                RequestFundNotify obj = new RequestFundNotify();
                obj.UserName = userItemDetails.Name;
                obj.FundAmount = inputObj.RequestAmount.ToString();
                obj.DonorUser = donoruserdetails.Name;
                obj.ProjectName = inputObj.ProjectName;
                obj.UserEmailId = userItemDetails.EmailId;
                bStatus = objEmail.SendMail("Request Fund", obj);
            }
            return Json(new { status = "success", data = id }, new JsonSerializerSettings());
        }
        //ExportPaymentFile
        public ActionResult ExportPaymentFile(int ProjectId, int AccountId)
        {
            LedgerBookDAL objLedgerBookDAL = new LedgerBookDAL();
            var paymentlist = objLedgerBookDAL.ExportPaymentFile(ProjectId, AccountId);
            //var json = JsonConvert.SerializeObject(paymentlist);

            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

            //string sWebRootFolder = "C:/Users/kisho/Desktop/ExportToExcel"; //_hostingEnvironment.WebRootPath;
            //string sFileName = @"PaymnetFile.xlsx";
            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            //FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //if (file.Exists)
            //{
            //    file.Delete();
            //    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //}
            //ExcelPackage package = new ExcelPackage(file);
            //ExcelWorksheet ws = package.Workbook.Worksheets.Add("Report");

            //if (dt.Columns.Count > 0)
            //{
            //    // Header Name 
            //    var headerRow = ws.Row(1);
            //    ws.SelectedRange["A1:K1"].Style.Font.Size = 13;
            //    ws.SelectedRange["A1:K1"].Style.Font.Bold = true;
            //    for (int ColCnt = 0; ColCnt <= dt.Columns.Count - 1; ColCnt++)
            //    {
            //        ws.Cells[1, ColCnt + 1].Value = dt.Columns[ColCnt].ToString()+ "\t";
            //    }
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    for (int RowCnt = 0; RowCnt <= dt.Rows.Count - 1; RowCnt++)
            //    {
            //        for (int ColCnt = 0; ColCnt <= dt.Columns.Count - 1; ColCnt++)
            //        {
            //            ws.Cells[RowCnt + 2, ColCnt + 1].Value = dt.Rows[RowCnt][ColCnt].ToString().ToUpper() + "\t";
            //        }

            //    }
            //}
            //package.Save();

            //var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = file.Name
            //}.ToString();
            //return result;

            return Json(new { status = "Success", data = paymentlist }, new JsonSerializerSettings());
        }
        #endregion ManageProjectLedgerBook
    }
}
