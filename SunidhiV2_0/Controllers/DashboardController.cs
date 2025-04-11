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
    public class DashboardController : Controller
    {
        [HttpPost]
        public IActionResult GetProjectListByWSID(int WorkspaceID)
        {
            DashboardDAL objDashboard = new DashboardDAL();
            List<Project> lst = new List<Project>();
            lst = objDashboard.GetProjectListByWSID(WorkspaceID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetActivityCount(int ProjectId)
        {
            DashboardDAL objDashboard = new DashboardDAL();
            List<ActivityCount> lst = new List<ActivityCount>();
            lst = objDashboard.GetActivityCount(ProjectId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult FillDashboard(int WorkspaceID)        
        {
            DashboardDAL objDashboard = new DashboardDAL();
            int loggedUserId = 0;
            try
            { 
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            catch (Exception ex)
            {
                loggedUserId = 0;
            }

            List<Project> lstProjects = new List<Project>();
            lstProjects = objDashboard.FillDashboard(WorkspaceID, loggedUserId).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult FillDashboardMyTeam(int ProjectId,int WorkspaceID)
        {
            DashboardDAL objDashboard = new DashboardDAL();
           
            List<UsersDetails> objUsersList = new List<UsersDetails>();
            objUsersList = objDashboard.FillDashboardMyTeam(ProjectId,WorkspaceID).ToList();
            return Json(new { data = objUsersList }, new JsonSerializerSettings());

        }


        [HttpPost]
        public IActionResult FillDashboardMyTask(int WorkspaceID)
        {
            DashboardDAL objDashboard = new DashboardDAL();
            List<MyTaskList> MyTaskList = new List<MyTaskList>();
            int loggedUserId = 0;
            try
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            catch (Exception ex)
            {
                loggedUserId = 0;
            }
            MyTaskList = objDashboard.FillDashboardMyTask(WorkspaceID, loggedUserId).ToList();
            return Json(new { data = MyTaskList }, new JsonSerializerSettings());

        }


        //[HttpPost]
        //public ActionResult GetProjectProgress(int ProjectId)
        //{
        //    DashboardDAL objDashboard = new DashboardDAL();
        //    int Progress = 0;
        //    Progress = objDashboard.GetProjectProgress(ProjectId);
        //    return Json(new { data = Progress }, new JsonSerializerSettings());

        //}

        [HttpPost]
        public ActionResult GetProjectProgress(int ProjectId)
        {
            DashboardDAL objDashboard = new DashboardDAL();
            double Progress = 0;
            Progress = objDashboard.GetProjectProgress(ProjectId);
            return Json(new { data = Progress }, new JsonSerializerSettings());

        }
    }
}
