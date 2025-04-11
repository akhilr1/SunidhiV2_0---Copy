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
    public class WorkspaceController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public WorkspaceController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost] //un-archive workspace
        public ActionResult UnArchiveWorkspace(string WorkspaceIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWorkspaceDAL = new WorkspaceDAL();
            objWorkspaceDAL.UnArchiveWorkspace(loggedUserId, WorkspaceIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //archive workspace
        public ActionResult ArchiveWorkspace(string WorkspaceIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWorkspaceDAL = new WorkspaceDAL();
            objWorkspaceDAL.ArchiveWorkspace(loggedUserId, WorkspaceIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //remove/delete workspace
        public ActionResult RemoveWorkspace(string WorkspaceIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWorkspaceDAL = new WorkspaceDAL();
            objWorkspaceDAL.RemoveWorkspace(loggedUserId, WorkspaceIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpGet] //get workspace for logged userid
        public ActionResult GetMyWorkspaceList()
        {
            int loggeduserid = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWorkspaceDAL = new WorkspaceDAL();
            var workspacelist = objWorkspaceDAL.GetMyWorkspaceList(loggeduserid);

            return Json(new { status="success", data = workspacelist }, new JsonSerializerSettings());
        }

        [HttpGet] //get shared workspace for logged userid
        public ActionResult GetSharedWorkspaceList()
        {
            int loggeduserid = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objSharedWorkspaceDAL = new WorkspaceDAL();
            var sharedworkspacelist = objSharedWorkspaceDAL.GetSharedWorkspaceList(loggeduserid);

            return Json(new { data = sharedworkspacelist }, new JsonSerializerSettings());
        }

        [HttpGet] //get shared workspace for logged userid
        public ActionResult GetArchivedWorkspaceList()
        {
            int loggeduserid = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objArchivedWorkspaceDAL = new WorkspaceDAL();
            var archivedworkspacelist = objArchivedWorkspaceDAL.GetArchivedWorkspaceList(loggeduserid);

            return Json(new { data = archivedworkspacelist }, new JsonSerializerSettings());
        }

        [HttpGet] //get workspace for logged userid
        public ActionResult GetAllWorkspaceList()
        {
            int loggeduserid = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWorkspaceDAL = new WorkspaceDAL();
            var workspacelist = objWorkspaceDAL.GetAllWorkspaceList(loggeduserid);

            return Json(new { data = workspacelist }, new JsonSerializerSettings());
        }

        #region ManageWorkspace

        [HttpPost]
        public IActionResult GetWorkspaceDetails(int WorkspaceID)
        {
            WorkspaceDAL objWs = new WorkspaceDAL();
            //int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            List<Workspace> lstWS = new List<Workspace>();
            lstWS = objWs.GetWorkspaceDetails(WorkspaceID).ToList();
            return Json(new { data = lstWS }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetWSTags(int WorkspaceID)
        {
            WorkspaceDAL objWs = new WorkspaceDAL();
            List<Tags> lstWsTags = new List<Tags>();
            lstWsTags = objWs.GetWSTags(WorkspaceID).ToList();
            return Json(new { data = lstWsTags }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult UpdateWorkspace(int WorkspaceID,string WSName, string WSDescription,string WSTags,string location)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWs = new WorkspaceDAL();

            var retStatus = "";

            var Files = Request.Form.Files;
            var fullimageurl = "";

            if (Files != null && Files.Count > 0)
            {
                string path = _hostingEnvironment.WebRootPath + "//Upload//" + location + "//" + WorkspaceID;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Guid.NewGuid().ToString();

                foreach (var itm in Files)
                {
                    fileName = fileName + "." + itm.FileName.Split(".")[1];
                    fullimageurl = "/" + location + "/" + WorkspaceID + "/" + fileName;

                    using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                    {
                        itm.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
            }

            retStatus = objWs.UpdateWorkspace(WorkspaceID,WSName, WSDescription,  WSTags, loggedUserId, fullimageurl);
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = "success" }, new JsonSerializerSettings());

        }
        public ActionResult SaveWSVisibility(int WorkspaceID, bool WSIsPrivate)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWs = new WorkspaceDAL();

            var retStatus = "";

            retStatus = objWs.SaveWSVisibility(WorkspaceID, WSIsPrivate, loggedUserId);
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = "success" }, new JsonSerializerSettings());

        }
        #endregion ManageWorkspace

        #region CreateWorkspace

        [HttpGet]
        public IActionResult GetWSTagDetails()
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWs = new WorkspaceDAL();
            List<Tags> lstWsTags = new List<Tags>();
            lstWsTags = objWs.GetWSTagDetails(loggedUserId).ToList();
            return Json(new { data = lstWsTags }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult AddNewWsTag(string TagName, string TagColor)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objTag = new WorkspaceDAL();
            var Tagstatus = "";

            Tagstatus = objTag.AddNewWsTag(TagName, TagColor, loggedUserId);
            return Json(new { status= Tagstatus }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateWSTagDetails(int TagID, string TagName, string TagColor)
        {
            var retStatus = "Success";
            TagsDAL objTag = new TagsDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            retStatus = objTag.UpdateWSTagDetails(TagID, TagName, TagColor, loggedUserId);

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult DeleteWSTagDetails(int TagID)
        {
            WorkspaceDAL objTag = new WorkspaceDAL();
            var ReturnVal = false;
            ReturnVal= objTag.DeleteWSTagDetails(TagID);
            return Json(new { status = ReturnVal }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult CreateWorkspace(string WSName, string WSDescription, bool WSIsPrivate, string WSTags,string location)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWs = new WorkspaceDAL();
            string fullFileName = "";

            var Files = Request.Form.Files;
            var fileName = "";

            if (Files != null && Files.Count > 0)
            {
                fileName = Guid.NewGuid().ToString();

                foreach (var itm in Files)
                {
                    fileName = fileName + "." + itm.FileName.Split(".")[1];
                    fullFileName = "/" + location + "/workspaceid/" + fileName;
                }
            }


            var retStatus = "";

            retStatus = objWs.CreateWorkspace(WSName, WSDescription, WSIsPrivate, WSTags, loggedUserId, fullFileName);

            if(retStatus.Contains("success"))
            {
                int workspaceid = Convert.ToInt32(retStatus.Split('-')[1]);

                if(workspaceid >0)
                {
                    if (Files != null && Files.Count > 0)
                    {
                        string path = _hostingEnvironment.WebRootPath + "//Upload//" + location + "//" + workspaceid;

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        foreach (var itm in Files)
                        {
                            using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                            {
                                itm.CopyTo(fileStream);
                                fileStream.Close();
                            }
                        }
                    }
                }
            }

            if (!retStatus.Contains("success"))
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = "success" }, new JsonSerializerSettings());

        }
        #endregion CreateWorkspace

        [HttpGet]
        public ActionResult CreateWorkspaceTemplate()
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            WorkspaceDAL objWs = new WorkspaceDAL();

            var workspacelist = objWs.CreateWorkspaceTemplate(loggedUserId);
            return Json(new { data = workspacelist }, new JsonSerializerSettings());

        }
    }
}