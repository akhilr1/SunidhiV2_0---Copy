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
    public class InitiativeController : Controller
    {
        #region CreateInitiative


        [HttpGet]
        public ActionResult GetNextInitiativeCode()
        {
            InitiativeDAL objInit = new InitiativeDAL();
            var nextCode = objInit.GetNextInitiativeCode();
            return Json(new { data = nextCode }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetInitUsersDetails(int WorkspaceID, int InitiativeId)
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<InitUserList> lstUsers = new List<InitUserList>();
            lstUsers = objInit.GetUsersList(WorkspaceID, InitiativeId).ToList();
            return Json(new { data = lstUsers }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetInitUserGroups(int WorkspaceID, int InitiativeId)
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<InitGroupList> lstUserGroup = new List<InitGroupList>();
            lstUserGroup = objInit.GetInitUserGroups(WorkspaceID, InitiativeId).ToList();
            return Json(new { data = lstUserGroup }, new JsonSerializerSettings());

        }

        [HttpGet]
        public IActionResult GetProjectTheme()
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<ProjectTemplate> lstTheme = new List<ProjectTemplate>();
            lstTheme = objInit.GetProjectTheme().ToList();
            return Json(new { data = lstTheme }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetProjectTemplateItems(int ThemeID)
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            lstPrjTemp = objInit.GetProjectTemplateItems(ThemeID).ToList();
            return Json(new { data = lstPrjTemp }, new JsonSerializerSettings());

        }
        
        [HttpPost]
        public IActionResult GetProjectTemplateDetails(int TemplateID)
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            lstPrjTemp = objInit.GetProjectTemplateDetails(TemplateID).ToList();
            return Json(new { data = lstPrjTemp }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetTagDetails()
        {
            TagsDAL objTag = new TagsDAL();
            List<Tags> lstTags = new List<Tags>();
            lstTags = objTag.GetTagDetails().ToList();
            return Json(new { data = lstTags }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetInitTagDetails(int WorkspaceId)
        {
            TagsDAL objTag = new TagsDAL();
            List<Tags> lstTags = new List<Tags>();
            lstTags = objTag.GetInitTagDetails(WorkspaceId).ToList();
            return Json(new { data = lstTags }, new JsonSerializerSettings());

        }

        [HttpPost]
        public ActionResult AddNewTag(string TagName, string TagColor,int WorkspaceId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            TagsDAL objTag = new TagsDAL();

            var Tagstatus = "";

            Tagstatus = objTag.AddInitNewWsTag(TagName, TagColor, loggedUserId, WorkspaceId);
            return Json(new { status = Tagstatus }, new JsonSerializerSettings());
        }
        public IActionResult UpdateInitTagDetails(int TagID, string TagName, string TagColor)
        {
            TagsDAL objTag = new TagsDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            objTag.UpdateInitTagDetails(TagID, TagName, TagColor, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult CreateInitiative(Initiatives objInitiatiive)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInit = new InitiativeDAL();
            UserDAL objUser = new UserDAL();
            
            var nextInitID = 0;
            var nextWFID = 0;
            bool bStatus = false;
            var retStatus = "";

            nextInitID = objInit.CreateInitiative(objInitiatiive.InitiativeName, objInitiatiive.Description, objInitiatiive.Code,
                objInitiatiive.WorkspaceID, objInitiatiive.InitiativeTags, objInitiatiive.InitUserList, objInitiatiive.InitGroupList, loggedUserId);
            if (nextInitID > 0)
            {
                //============= Update Send Invite List====================                       
                if (objInitiatiive.InviteUserList != null)
                {
                    List<InviteUsersList> lstInviteUser = JsonConvert.DeserializeObject<List<InviteUsersList>>(objInitiatiive.InviteUserList);

                    foreach (InviteUsersList userItem in lstInviteUser)
                    {
                        retStatus = objUser.AddInviteUser(userItem.EmailId, userItem.UserRole.userRoleID, loggedUserId, objInitiatiive.WorkspaceID, "Initiative", nextInitID);
                        if (retStatus != "")
                            return Json(new { status = retStatus }, new JsonSerializerSettings());
                        else
                        {
                            bStatus = objUser.InviteUser(userItem.EmailId, userItem.UserRole.userRole, loggedUserId, "Initiative", nextInitID);
                            if (!bStatus)
                                return Json(new { status = retStatus }, new JsonSerializerSettings());
                        }
                    }
                }


                //=============Update Initiative to Project Template List==============
                if (objInitiatiive.InitProjectTemplate != null)
                {
                    List<ProjectTemplate> objPrjTempList = JsonConvert.DeserializeObject<List<ProjectTemplate>>(objInitiatiive.InitProjectTemplate);
                    foreach (ProjectTemplate prjItem in objPrjTempList)
                        retStatus = objInit.AddInitPrjTemplate(nextInitID, prjItem.ThemeID, prjItem.ProjectTemplateID, loggedUserId);
                }
                //=============Update Initiative to Work Flow List
                if (objInitiatiive.InitWorkflowList != null)
                {
                    List<InitWorkflowList> objWFList = JsonConvert.DeserializeObject<List<InitWorkflowList>>(objInitiatiive.InitWorkflowList);
                    foreach (InitWorkflowList wfItem in objWFList)
                    {
                        nextWFID = objInit.AddInitWorkFlow(nextInitID, objInitiatiive.WorkspaceID, wfItem.WfName, wfItem.WfDescription, wfItem.WfNoOfLevels, loggedUserId);
                        foreach (WfLevelItem wfLevelItem in wfItem.WfLevelItems)
                        {
                            foreach (InitUserList userItem in wfLevelItem.InitUserList)
                            {
                                retStatus = objInit.AddInitWorkFlowLevel(nextWFID, wfLevelItem.WfLevel, loggedUserId, userItem.UserID);
                                if (retStatus != "")
                                    return Json(new { status = retStatus }, new JsonSerializerSettings());
                            }
                        }
                    }
                }
            }
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = nextInitID >0 ? "success" : "falied" }, new JsonSerializerSettings());

        }
        #endregion CreateInitiative

        //deploy and archive Initiative functions starts here 

        [HttpPost]
        public IActionResult GetInitiativeDetails(int stage, int WorkspaceId)
        {
            InitiativeDAL objInit = new InitiativeDAL();
            List<Initiatives> lstInitiatives = new List<Initiatives>();
            lstInitiatives = objInit.GetInitiativeDetails(stage, WorkspaceId).ToList();
            return Json(new { status = "success" ,data = lstInitiatives }, new JsonSerializerSettings());

        }



        public IActionResult DeleteInitiative(string InitiativeId, int Type)
        {
            InitiativeDAL objInit = new InitiativeDAL();


            if (Type == 1)
            {

                List<Initiatives> objInitiativeList = JsonConvert.DeserializeObject<List<Initiatives>>(InitiativeId);
                foreach (Initiatives userItem in objInitiativeList)
                {
                    objInit.DeleteInitiative(userItem.InitiativeId);

                }
            }
            else
            {
                objInit.DeleteInitiative(Convert.ToInt32(InitiativeId));

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        public ActionResult ChangeInitiativeStatus(string InitiativeId, int Type, int InitiativeStatus)
        {


            InitiativeDAL objInit = new InitiativeDAL();

            if (Type == 1)
            {

                List<Initiatives> objInitiativeList = JsonConvert.DeserializeObject<List<Initiatives>>(InitiativeId);
                foreach (Initiatives userItem in objInitiativeList)
                {
                    objInit.ChangeInitiativeStatus(userItem.InitiativeId, InitiativeStatus);

                }
            }
            else
            {

                objInit.ChangeInitiativeStatus(Convert.ToInt32(InitiativeId), InitiativeStatus);
            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        public ActionResult ChangeStatus(string InitiativeId, bool userStatus)
        {


            InitiativeDAL objInit = new InitiativeDAL();

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);


            objInit.ChangeStatus(Convert.ToInt32(InitiativeId), loggedUserId, userStatus);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        public ActionResult GetProjectsDetails()
        {
            InitiativeDAL objInit = new InitiativeDAL();
            var ProjectsDetails = objInit.GetProjectsDetails();

            return Json(new { status = "success" , data = ProjectsDetails }, new JsonSerializerSettings());

        }
        //deploy and archive Initiative functions ends here 


        //Manage Initiative functions starts here 
        [HttpPost]
        public IActionResult GetInitiativeDetailsByID(int InitiativeId, int WorkspaceID)
        {
            InitiativeDAL objInitDAL = new InitiativeDAL();
            var objInitiative = objInitDAL.GetInitiativeDetailsByID(InitiativeId, WorkspaceID);
            return Json(new { status = "success" ,data = objInitiative }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult UpdateInitiativeDetails(Initiatives inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();
            objInitDAL.UpdateInitiativeDetails(inputObj,loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetGroupsListByIntitiativeID(int InitiativeId)
        {
            InitiativeDAL objInitDAL = new InitiativeDAL();
            var lstInitiativeGroups = objInitDAL.GetGroupsListByIntitiativeID(InitiativeId);
            return Json(new { status = "success" , data = lstInitiativeGroups }, new JsonSerializerSettings());
        }

        [HttpPost] //add a new parent group with or without userslist and insert into initiative
                   //public ActionResult AddGroupToInitiative(AddGroupToInitiative inputObj)
                   //{
                   //    /* 3 steps in this --> 1 - Add the group to TbGroup
                   //     *                     2 - Add Users to TbUsersAssignedToGroup
                   //     *                     3 - Add GroupId to TbInitiativeToGroups 
                   //     Also step1 can be skipped when adding users */

        //    UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
        //    var idGroup = inputObj.GroupId;
        //    var notify = inputObj.Notify;
        //    int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

        //    if (idGroup == 0)
        //    {
        //        //step 1
        //        idGroup = objUserGroupsDAL.AddGroup(inputObj.GroupName, inputObj.Description, 0, loggedUserId, inputObj.WorkspaceId);
        //    }
        //    var idUser = 0;

        //    //step 2
        //    List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.UserList);
        //    foreach (UsersInfo userItem in objUserList)
        //    {
        //        idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, idGroup);
        //        if (notify)
        //        {
        //            bool bStatus = false;

        //            EmailActivity objEmail = new EmailActivity();

        //            //Get ClientName by userID- logged userID
        //            //string ClientName = "TestClient"; //GetClientNameById(userID);
        //            GroupNotify obj = new GroupNotify();
        //            obj.UserName = userItem.Name;
        //            obj.GroupName = inputObj.GroupName;
        //            obj.UserEmailId = userItem.EmailId;
        //            bStatus = objEmail.SendMail("Group Add Users", obj);
        //        }
        //    }

        //    //step 3
        //    InitiativeDAL objInitDAL = new InitiativeDAL();
        //    objInitDAL.AddGroupToInitiative(inputObj.InitiativeId, idGroup, loggedUserId);
        //    return Json(new { status = "success" }, new JsonSerializerSettings());
        //}
        public ActionResult AddGroupToInitiative(string GroupList, int InitiativeId)
        {


            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);


            InitiativeDAL objInitDAL = new InitiativeDAL();
            List<UserGroupList> objUserList = JsonConvert.DeserializeObject<List<UserGroupList>>(GroupList);
            foreach (UserGroupList userItem in objUserList)
            {
                objInitDAL.AddGroupToInitiative(InitiativeId, userItem.GroupId, loggedUserId);

            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //soft delete of a group(parent only currently)
        public ActionResult DeleteGroupFromInitiative(int GroupId, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();
            objInitDAL.DeleteGroupFromInitiative(InitiativeId, GroupId, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetUsersListByIntitiativeID(int InitiativeId, int WorkspaceId)
        {
            InitiativeDAL objInitDAL = new InitiativeDAL();
            var lstInitiativeUsers = objInitDAL.GetUsersListByIntitiativeID(InitiativeId, WorkspaceId);
            return Json(new { status = "success" , data = lstInitiativeUsers }, new JsonSerializerSettings());
        }

        [HttpPost] //soft delete of a group(parent only currently)
        public ActionResult DeleteUserFromInitiative(int UserId, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();
            objInitDAL.DeleteUserFromInitiative(InitiativeId, UserId, loggedUserId);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult AddUserInitiative(string userInviteList, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            InitiativeDAL objInitDAL = new InitiativeDAL();

            List<InitUserList> objUserList = JsonConvert.DeserializeObject<List<InitUserList>>(userInviteList);
            foreach (InitUserList userItem in objUserList)
            {
                objInitDAL.AddUserToInitiative(InitiativeId, userItem.UserID, loggedUserId);

            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        ////Manageinitiative Details tags changes Starts here
        [HttpPost]
        public ActionResult AddNewInitiativeTag(string TagName, string TagColor)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();

            var retStatus = "";

            retStatus = objInitDAL.AddNewInitiativeTag(TagName, TagColor, loggedUserId);
            // return Json(new { status = retStatus }, new JsonSerializerSettings());
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetInitiativeTags(int InitiativeId)
        {
            InitiativeDAL objInitDAL = new InitiativeDAL();
            List<Tags> InitiativeTags = new List<Tags>();
            InitiativeTags = objInitDAL.GetInitiativeTags(InitiativeId).ToList();
            return Json(new { status = "success" ,data = InitiativeTags }, new JsonSerializerSettings());

        }
        //bulk remove user
        public ActionResult DeleteBulkUserFromInitiative(string UserId, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();

            List<UsersInfo> objuserList = JsonConvert.DeserializeObject<List<UsersInfo>>(UserId);
            foreach (UsersInfo userItem in objuserList)
            {
                objInitDAL.DeleteUserFromInitiative(InitiativeId, userItem.ID, loggedUserId);

            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        [HttpPost] ////bulk remove group
        public ActionResult DeleteBulkGroupFromInitiative(string GroupId, int InitiativeId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            InitiativeDAL objInitDAL = new InitiativeDAL();

            List<UserGroupList> objgroupList = JsonConvert.DeserializeObject<List<UserGroupList>>(GroupId);
            foreach (UserGroupList groupItem in objgroupList)
            {
                objInitDAL.DeleteGroupFromInitiative(InitiativeId, groupItem.GroupId, loggedUserId);

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        //Manage Initiative functions ends here
    }
}
