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
using Microsoft.Extensions.FileProviders;

namespace SunidhiV2_0.Controllers
{
    public class MyProjectController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public MyProjectController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        #region CreateProject

        [HttpGet]
        public ActionResult GetNextProjectCode()
        {
            MyProjectDAL objProject = new MyProjectDAL();
            var nextCode = objProject.GetNextProjectCode();
            return Json(new { data = nextCode }, new JsonSerializerSettings());

        }
        [HttpGet]
        public IActionResult GetCountryDetails()
        {
            MyProjectDAL objProject = new MyProjectDAL();
            List<Country> lst = new List<Country>();
            lst = objProject.GetCountryDetails().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetStateDetails(int CountryID)
        {
            MyProjectDAL objProject = new MyProjectDAL();
            List<State> lst = new List<State>();
            lst = objProject.GetStateDetails(CountryID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetDistrictDetails(int StateID)
        {
            MyProjectDAL objProject = new MyProjectDAL();
            List<District> lst = new List<District>();
            lst = objProject.GetDistrictDetails(StateID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetProjectThemeList(int InitiativeID)
        {
            MyProjectDAL objProject = new MyProjectDAL();
            List<ProjectTemplate> lstTheme = new List<ProjectTemplate>();
            lstTheme = objProject.GetProjectThemeList(InitiativeID).ToList();
            return Json(new { data = lstTheme }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetProjectTemplateList(int InitiativeID, int ThemeID, int NoOfTemplates)
        {
            MyProjectDAL objProject = new MyProjectDAL();
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            lstPrjTemp = objProject.GetProjectTemplateList(InitiativeID, ThemeID, NoOfTemplates).ToList();
            return Json(new { data = lstPrjTemp }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult CreateProject(Project objProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            MyProjectDAL objPrjDAL = new MyProjectDAL();
            var retStatus = "";

            retStatus = objPrjDAL.CreateProject(objProject.ProjectName, objProject.Description, objProject.Code, objProject.Budget, objProject.StartDate,
                                            objProject.EndDate, objProject.CountryID, objProject.StateID, objProject.DistrictID, objProject.ProjectTags,
                                            objProject.ProjectTemplateID, objProject.InitiativeId, loggedUserId);

            if (retStatus == "success")
            {
                List<ThemeTemplate> lstProjectclass = new List<ThemeTemplate>();

                string path = _hostingEnvironment.WebRootPath + "\\Upload\\ProjectTemplate_JSON\\json-format-data";
                var JSON = System.IO.File.ReadAllText(path);
                lstProjectclass = JsonConvert.DeserializeObject<List<ThemeTemplate>>(JSON);

                if (lstProjectclass != null && lstProjectclass.Count > 0)
                {
                    var template = lstProjectclass.Where(x => x.ProjectTemplateID == objProject.ProjectTemplateID.ToString()).FirstOrDefault();

                    if(template != null)
                    {
                        ThemeTemplateDAL objThemeTemplateDAL = new ThemeTemplateDAL();
                        objThemeTemplateDAL.InsertTemplatesProjectDetails(template, loggedUserId, objProject.Code);
                    }
                }
            }

            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetInitiative(int WorkspaceID)
        {
            int loggedUserId = 0;
            var ClaimCount = User.Claims.Count();
            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            
            MyProjectDAL objProject = new MyProjectDAL();
            List<Project> lst = new List<Project>();
            lst = objProject.GetInitiative(WorkspaceID,loggedUserId).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        #endregion CreateProject
        #region DeployAndArchive

        [HttpPost]
        public IActionResult GetProjectListByInitID(string stage, int WorkspaceID)
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            int loggedUserId = 0;
            if (WorkspaceID != 0)
            {
                var ClaimCount = User.Claims.Count();
                if(ClaimCount > 0)
                {
                    loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                }
            }
            List<Project> lstProjects = new List<Project>();
            lstProjects = objMyProjectDAL.GetProjectListByInitID(stage, loggedUserId, WorkspaceID).ToList();
            return Json(new { data = lstProjects }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult DeleteProject(string ProjectID, int Type)
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            if (Type == 1)
            {
                List<Project> objProjectList = JsonConvert.DeserializeObject<List<Project>>(ProjectID);
                foreach (Project prjt in objProjectList)
                {
                    objMyProjectDAL.DeleteProject(prjt.ProjectId);
                }
            }
            else
            {
                objMyProjectDAL.DeleteProject(Convert.ToInt32(ProjectID));

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ChangeProjectStatus(string ProjectID, string ProjectStatus)
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();

            objMyProjectDAL.ChangeProjectStatus(Convert.ToInt32(ProjectID), ProjectStatus);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult ChangeState(string ProjectID, bool State)
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);


            objMyProjectDAL.ChangeState(Convert.ToInt32(ProjectID), loggedUserId, State);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpGet]
        public IActionResult GetTheme()
        {
            int loggedUserId = 0;
            var ClaimCount = User.Claims.Count();
            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }

            MyProjectDAL objProject = new MyProjectDAL();
            List<ProjectTheme> lst = new List<ProjectTheme>();
            lst = objProject.GetTheme().ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetProjectTemplateByprjtID(int ProjectID)
        {
            int loggedUserId = 0;
            var ClaimCount = User.Claims.Count();
            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            MyProjectDAL objProject = new MyProjectDAL();
            List<InitiativeProjectTemplate> lst = new List<InitiativeProjectTemplate>();
            lst = objProject.GetProjectTemplateByprjtID(ProjectID).ToList();
            return Json(new { data = lst }, new JsonSerializerSettings());

        }

        public IActionResult InsertProjectTemplate(int ThemeID, string ProjectTemplateName, string Description, int ProjectID)
        {
            string coverOrginalName = string.Empty;
            string coverName = string.Empty;
            var Files = Request.Form.Files;

            if (Files != null && Files.Count > 0)
            {
                foreach (var item in Files)
                {
                    string uploadPath = _hostingEnvironment.WebRootPath + "\\Upload\\TemplateCover\\";
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    coverOrginalName += coverOrginalName != "" ? "," + item.FileName : item.FileName;
                    coverName += coverName != "" ? "," + $@"{Guid.NewGuid()}" + "." + item.FileName.Split('.')[1] : $@"{Guid.NewGuid()}" + "." + item.FileName.Split('.')[1];

                    using (var fileStream = new FileStream((uploadPath + "\\" + coverName), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        fileStream.Close();

                    }
                }
            }
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            //int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            int loggedUserId = 0;
            var ClaimCount = User.Claims.Count();
            if (ClaimCount > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            objMyProjectDAL.InsertProjectTemplate(ThemeID, ProjectTemplateName, Description, loggedUserId, ProjectID, coverName, coverOrginalName);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion DeployAndArchive
        #region ManageProjectDetails
        [HttpPost]
        public IActionResult GetProjectDetailsByID(int ProjectId, int InitiativeId)
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            var project = objMyProjectDAL.GetProjectDetailsByID(ProjectId, InitiativeId);
            return Json(new { data = project }, new JsonSerializerSettings());
        }

        [HttpGet]
        public IActionResult GetCountryStatesDistricts()
        {
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            var countrylist = objMyProjectDAL.GetCountryDetails();
            var statesList = objMyProjectDAL.GetStateList();
            var districtsList = objMyProjectDAL.GetDistrictList();
            return Json(new { data = new { countries = countrylist, states = statesList, districts = districtsList } }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateEditProjectDetails(Project inputObjProject)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            MyProjectDAL objMyProjectDAL = new MyProjectDAL();
            objMyProjectDAL.UpdateEditProjectDetails(inputObjProject, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion ManageProjectDetails
    }
}