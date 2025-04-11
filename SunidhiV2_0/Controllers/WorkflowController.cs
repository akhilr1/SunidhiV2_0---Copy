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
    public class WorkflowController : Controller
    {
        #region InitiativeWorkflow
        [HttpPost]
        public IActionResult GetWorkflowsListByIntitiativeID(int InitiativeId, int WorkspaceId)
        {
            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            var lstInitiativeWorkflows = objWorkflowsDAL.GetWorkflowsListByIntitiativeID(InitiativeId, WorkspaceId);
            return Json(new { status = "success", data = lstInitiativeWorkflows }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateWorkflowDetails(Workflow inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.UpdateWorkflowDetails(inputObj.WorkflowID, inputObj.WfName, inputObj.WfDescription, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateWorkflowLevelDetails(UpdateWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            
            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.UpdateWorkflowLevelCount(inputObj.WorkflowID, inputObj.WfNoOfLevels, loggedUserId);
            objWorkflowsDAL.DeleteWorkflowLevel(inputObj.WorkflowID, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objWorkflowsDAL.UpdateWorkflowLevelDetails(inputObj.WorkflowID, wfLevelItem.WfLevel, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult AddWorkflow(AddWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            var wfID = objWorkflowsDAL.AddWorkflow(inputObj, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objWorkflowsDAL.UpdateWorkflowLevelDetails(wfID, wfLevelItem.WfLevel, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //remove/delete workspace
        public ActionResult RemoveWorkflows(string WorkflowIdList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.RemoveWorkflows(loggedUserId, WorkflowIdList);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion InitiativeWorkflow

        #region ProjectWorkflow
        [HttpPost]
        public IActionResult GetWorkflowsListByProjectID(int ProjectId, int WorkspaceId)
        {
            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            var lstProjectWorkflows = objWorkflowsDAL.GetWorkflowsListByProjectID(ProjectId, WorkspaceId);
            return Json(new { status = "success", data = lstProjectWorkflows }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateProjectWorkflowDetails(Workflow inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.UpdateProjectWorkflowDetails(inputObj.WorkflowID, inputObj.WfName, inputObj.WfDescription, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult UpdateProjectWorkflowLevelDetails(UpdateWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.UpdateProjectWorkflowLevelCount(inputObj.WorkflowID, inputObj.WfNoOfLevels, loggedUserId);
            objWorkflowsDAL.DeleteProjectWorkflowLevel(inputObj.WorkflowID, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objWorkflowsDAL.UpdateProjectWorkflowLevelDetails(inputObj.WorkflowID, wfLevelItem.WfLevel, wfLevelItem.workflowlevelname, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult AddProjectWorkflow(AddWorkflowLevelDetails inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            var wfID = objWorkflowsDAL.AddProjectWorkflow(inputObj, loggedUserId);

            List<WorkflowLevelDetails> objWfLevelList = JsonConvert.DeserializeObject<List<WorkflowLevelDetails>>(inputObj.WfLevelDetails);
            foreach (WorkflowLevelDetails wfLevelItem in objWfLevelList)
            {
                if (wfLevelItem.usersList.Count > 0)
                {
                    foreach (User userItem in wfLevelItem.usersList)
                    {
                        objWorkflowsDAL.UpdateProjectWorkflowLevelDetails(wfID, wfLevelItem.WfLevel, wfLevelItem.workflowlevelname, userItem.ID, 0, loggedUserId);
                    }
                }
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //remove/delete workspace
        public ActionResult RemoveProjectWorkflows(string WorkflowIdList)
        {
            string status = "";
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            bool isExists = objWorkflowsDAL.IsProjectWorkflowsExists(loggedUserId, WorkflowIdList);
            if (!isExists)
            {
                objWorkflowsDAL.RemoveProjectWorkflows(loggedUserId, WorkflowIdList);
                status = "success";
            }
            else
            {
                status = "workflowexists";
            }
            return Json(new { status }, new JsonSerializerSettings());
        }

        [HttpPost] //add selected workflow from initiative to project
        public ActionResult AddSelectWorkflow(string WorkflowList, int ProjectId, int WorkspaceID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            objWorkflowsDAL.AddSelectWorkflow(loggedUserId, WorkflowList, ProjectId, WorkspaceID);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetWorkflowsListForFilter(int WorkspaceId)
        {
            WorkflowDAL objWorkflowsDAL = new WorkflowDAL();
            var lstProjectWorkflows = objWorkflowsDAL.GetWorkflowsListForFilter(WorkspaceId);
            return Json(new { status = "success", data = lstProjectWorkflows }, new JsonSerializerSettings());
        }
        #endregion ProjectWorkflow
    }
}
