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


namespace SunidhiV2_0.Controllers
{
    public class ProjectTagsController : Controller
    {
        #region ProjectExpenseTags
        [HttpPost]
        public ActionResult AddNewExpenseTag(string TagName, int ProjectId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            var tagID = objProjectTagsDAL.AddNewExpenseTag(TagName, ProjectId, loggedUserId);

            return Json(new { status = "success", data = tagID }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult EditExpenseTag(int TagID, string TagName)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            objProjectTagsDAL.EditExpenseTag(TagID, TagName, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetAllExpenseTags(int ProjectId)
        {
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            var allExpenseTagList = objProjectTagsDAL.GetAllExpenseTags(ProjectId);
            return Json(new { data = allExpenseTagList }, new JsonSerializerSettings());
        }
        #endregion ProjectExpenseTags

        #region ProjectTags
        [HttpPost]
        public ActionResult CreateNewTag(string TagName, string TagColor, int WorkspaceID,int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            var Tagstatus = "";
            Tagstatus = objProjectTagsDAL.CreateNewTag(TagName, TagColor, WorkspaceID,InitiativeId, loggedUserId);

            return Json(new { status = Tagstatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult UpdateTagDetails(int TagID, string TagName, string TagColor)
        {
            var Tagstatus = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            Tagstatus=objProjectTagsDAL.UpdateTagDetails(TagID, TagName, TagColor, loggedUserId);

            return Json(new { status = Tagstatus }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetAllProjectTags(int WorkspaceID)
        {
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            var allProjectTagList = objProjectTagsDAL.GetAllProjectTags(WorkspaceID);
            return Json(new { data = allProjectTagList }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult DeleteProjectTag(int TagID, int WorkspaceID)
        {
            //int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectTagsDAL objProjectTagsDAL = new ProjectTagsDAL();
            bool Tagstatus = true;
            Tagstatus = objProjectTagsDAL.DeleteProjectTag(TagID, WorkspaceID);

            return Json(new { status = Tagstatus }, new JsonSerializerSettings());
        }
        #endregion ProjectTags
    }
}