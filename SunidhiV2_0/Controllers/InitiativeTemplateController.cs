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
    public class InitiativeTemplateController : Controller
    {
        [HttpPost]
        public IActionResult GetProjectThemeByIntitiativeID(int InitiativeId)
        {
            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            var lstInitiativeProjectTheme = objProjectDAL.GetProjectThemeByIntitiativeID(InitiativeId);
            return Json(new { status = "success", data = lstInitiativeProjectTheme }, new JsonSerializerSettings());
        }

        [HttpPost] //remove or delete ProjectTheme and its Project Templates from Initiative
        public ActionResult RemoveProjectThemeFromInitiative(string ThemeIDList, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            objProjectDAL.RemoveProjectThemeFromInitiative(loggedUserId, ThemeIDList, InitiativeId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetAllProjectTheme()
        {
            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            var lstAllProjectTheme = objProjectDAL.GetAllProjectTheme();
            return Json(new { status = "success", data = lstAllProjectTheme }, new JsonSerializerSettings());
        }

        [HttpPost] //add ProjectTheme only to Initiative
        public ActionResult AddProjectThemeToInitiative(string ThemeIDList, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            objProjectDAL.AddProjectThemeToInitiative(loggedUserId, ThemeIDList, InitiativeId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //remove or delete ProjectTheme and its Project Templates from Initiative
        public ActionResult RemoveProjectTemplateFromInitiative(string TemplateIDList, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            objProjectDAL.RemoveProjectTemplateFromInitiative(loggedUserId, TemplateIDList, InitiativeId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetAllProjectTemplateByThemeID(int ThemeID)
        {
            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            var lstAllProjectTemplate = objProjectDAL.GetAllProjectTemplateByThemeID(ThemeID);
            return Json(new { status = "success", data = lstAllProjectTemplate }, new JsonSerializerSettings());
        }

        [HttpPost] //add ProjectTemplates to Initiative
        public ActionResult AddProjectTemplateToInitiative(string TemplateList, int InitiativeId, int ThemeID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            InitiativeTemplateDAL objProjectDAL = new InitiativeTemplateDAL();
            objProjectDAL.AddProjectTemplateToInitiative(loggedUserId, TemplateList, InitiativeId, ThemeID);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
    }
}
