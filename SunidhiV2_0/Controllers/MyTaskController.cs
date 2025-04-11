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
#endregion NameSpaces

namespace SunidhiV2_0.Controllers
{
    public class MyTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult GetMyTaskActivityLog(int syncTaskID)
        {
            List<MyTaskActivityLog> lstActivityLog = new List<MyTaskActivityLog>();
            MyTaskTestDAL objMyTaskTestDAL = new MyTaskTestDAL();
            lstActivityLog = objMyTaskTestDAL.GetMyTaskActivityLog(syncTaskID);
            return Json(new { data = lstActivityLog }, new JsonSerializerSettings());
        }
        public IActionResult GetLoadmoreTasks(int UserID, int workspaceId, int rownNmber, int TypeID, string LastSyncDatetime = "")
        {
          
            // MyTask lstTaskList = new MyTask();
            ListSyncItem lstTaskList = new ListSyncItem();
            MyTaskTestDAL objMyTaskTestDAL = new MyTaskTestDAL();
            lstTaskList = objMyTaskTestDAL.GetLoadmoreTasks(UserID, workspaceId, LastSyncDatetime, rownNmber, TypeID);
            return Json(new { status = "success", data = lstTaskList }, new JsonSerializerSettings());
        }
    }
}