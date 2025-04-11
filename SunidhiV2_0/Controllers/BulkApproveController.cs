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
    public class BulkApproveController : Controller
    {
      
        [HttpPost]
        public IActionResult GetAllApprovalDataBulk(int UserID, int SyncServerID, int WorkspaceID, int TypeID = 0)
        {
            BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL();
            List<BulkApprove> lstBulkApprove = new List<BulkApprove>();
   

            lstBulkApprove = objBulkApproveDAL.GetAllApprovalDataBulk(UserID, SyncServerID, WorkspaceID, TypeID);

        
            return Json(new { status = "success", data = new { MyTaskList = lstBulkApprove } }, new JsonSerializerSettings());
        }

        public ActionResult GetApprovalResponse(int TypeID, int syncTaskID, int workspaceId)
        {
            ListSyncItem lstTaskList = new ListSyncItem();
            BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL();
            lstTaskList = objBulkApproveDAL.GetApprovalResponse(TypeID, syncTaskID, workspaceId);
            return Json(new { status = "success", data = lstTaskList }, new JsonSerializerSettings());
        }
        #region Update Bulk Data
        /// <summary>
        /// UpdateBulkData
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateBulkData(string gridData)
        {
            int loggedUserId = 0;

            var ClaimCount = User.Claims.Count();

            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }

            BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL(); 
            bool status = objBulkApproveDAL.UpdateBulkData(gridData,loggedUserId);
            return Json(new { data = status }, new JsonSerializerSettings());
        }
        #endregion Update Bulk Data

        #region Delete Bulk Data
        /// <summary>
        /// DeleteBulkData
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteBulkData(string gridData)
        {
            int loggedUserId = 0;

            var ClaimCount = User.Claims.Count();

            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }

            BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL();
            bool status = objBulkApproveDAL.DeleteBulkData(gridData, loggedUserId);
            return Json(new { data = status }, new JsonSerializerSettings());
        }
        #endregion Delete Bulk Data


        public ActionResult BulkApproveAndReject(List<MyTaskSyncToServerData> lstMyTaskSyncToServerData, int loggedUserID)
        {
            var retStatus = "";
            int id = 1;
            int loggedUserId = loggedUserID;
            var claimlist = User.Claims.ToList();
            List<MyTaskSyncToServerStatus> lstMyTaskSyncToServerStatus = new List<MyTaskSyncToServerStatus>();
            var Files = Request.Form.Files;
             BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL();

            int syncDBID = 0;
            if (claimlist.Count > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }

            if (lstMyTaskSyncToServerData != null && lstMyTaskSyncToServerData.Count() > 0)
            {
                foreach (var item in lstMyTaskSyncToServerData)
                {
                    MobileWebServiceDAL objMobService = new MobileWebServiceDAL();
                    syncDBID = objMobService.GetSyncDBIDByClientGuid(item.ClientGuid);

                        if (item.Type == "Survey")
                        {
                            id = objBulkApproveDAL.BulkApproveAndReject(item.WFLevel, item.IsBeneficiaryLinked, item.ClientData, item.Periodicity, loggedUserId, item.ClientGuid);

                      
                        }
                       

                        retStatus = id > 0 ? "success" : "Failed";

                  

                    MyTaskSyncToServerStatus objMyTaskSyncToServerStatus = new MyTaskSyncToServerStatus();
                    objMyTaskSyncToServerStatus.ClientGuid = item.ClientGuid;
                    objMyTaskSyncToServerStatus.TaskSyncID = id;
                    objMyTaskSyncToServerStatus.Status = retStatus;
                    lstMyTaskSyncToServerStatus.Add(objMyTaskSyncToServerStatus);
                }
            }

    
            return Json(new { status = "success", Data = lstMyTaskSyncToServerStatus }, new JsonSerializerSettings());
        }

        #region GetActivityLog
        public ActionResult GetActivityLog( string syncTaskID)
        {
            int[] syncTaskIDs = syncTaskID.Split(',').Select(int.Parse).ToArray();
            BulkApproveDAL objBulkApproveDAL = new BulkApproveDAL();
            List<MyTaskActivityLog> lst = new List<MyTaskActivityLog>();
            List<MyTaskActivityLog> lst1 = new List<MyTaskActivityLog>();
            foreach (int synID in syncTaskIDs)
            {
                lst1 = objBulkApproveDAL.GetMyTaskActivityLog(synID).ToList();
                lst.AddRange(lst1);
            }
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion

    }
}
