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
using SunidhiV2_0.Code.Common.Authentication;
using System.Net.NetworkInformation;

namespace SunidhiV2_0.Controllers
{
    public class UserGroupsController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UserGroupsController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost] //get all groups list based on WorkspaceID
        public ActionResult GetUserGroupsList(int WorkspaceId)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var usergroupslist = objUserGroupsDAL.GetUserGroupsList(WorkspaceId);

            return Json(new { status = "success", data = usergroupslist }, new JsonSerializerSettings());
        }

        [HttpPost] //get sub groups list for a groupId
        public ActionResult GetSubGroupsList(Group inputObj)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var subgroupslist = objUserGroupsDAL.GetSubGroupsList(inputObj.GroupId);

            return Json(new { status = "success", data = subgroupslist }, new JsonSerializerSettings());
        }

        [HttpPost] //get assigned user list for a groupId
        public ActionResult GetAssignedUsersList(int GroupId, int WorkspaceID)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var assigneduserslist = objUserGroupsDAL.GetAssignedUsersList(GroupId, WorkspaceID);

            return Json(new { status = "success", data = assigneduserslist }, new JsonSerializerSettings());
        }

        [HttpPost] //add a new parent group with or without userslist
        public ActionResult AddUserGroup(AddGroup inputObj)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var userDAL = new UserDAL();
            var idGroup = inputObj.GroupId;
            var notify = inputObj.Notify;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (idGroup == 0)
            {
                var Files = Request.Form.Files;

                if (Files != null && Files.Count > 0)
                {
                    string path = _hostingEnvironment.WebRootPath + "//Upload//" + inputObj.location + "//" + inputObj.WorkspaceId;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = Guid.NewGuid().ToString();

                    foreach (var itm in Files)
                    {
                        fileName = fileName + "." + itm.FileName.Split(".")[1];
                        inputObj.UserGroupImage = "/" + inputObj.location + "/" + inputObj.WorkspaceId + "/" + fileName;

                        using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                        {
                            itm.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }
                }

                idGroup = objUserGroupsDAL.AddGroup(inputObj.GroupName, inputObj.Description, 0, loggedUserId, inputObj.WorkspaceId, inputObj.UserGroupImage);
            }

            var idUser = 0;

            List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.UserList);
            foreach (UsersInfo userItem in objUserList)
            {
                idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, idGroup, loggedUserId);

                //Removing the notification flag as sending the mail has become mandatory for this action

                bool bStatus = false;
                EmailActivity objEmail = new EmailActivity();
                string userName = userDAL.GetUserName(userItem.EmailId);
                bStatus = userDAL.SendEmailtoAssignedUsersFromWorkspace(userItem.EmailId, userName, loggedUserId, inputObj.WorkspaceId, inputObj.ProjectId != null ? inputObj.ProjectId.Value : 0);
            }

            return Json(new { status = "success", data = idGroup }, new JsonSerializerSettings());
        }

        [HttpPost] //add a new sub group with or without userslist
        public ActionResult AddUserSubGroup(AddGroup inputObj)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var idGroup = inputObj.GroupId;
            var notify = inputObj.Notify;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (idGroup == 0)
            {
                var Files = Request.Form.Files;

                if (Files != null && Files.Count > 0)
                {
                    string path = _hostingEnvironment.WebRootPath + "//Upload//" + inputObj.location + "//" + inputObj.WorkspaceId;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = Guid.NewGuid().ToString();

                    foreach (var itm in Files)
                    {
                        fileName = fileName + "." + itm.FileName.Split(".")[1];
                        inputObj.UserGroupImage = "/" + inputObj.location + "/" + inputObj.WorkspaceId + "/" + fileName;

                        using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                        {
                            itm.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }
                }

                idGroup = objUserGroupsDAL.AddGroup(inputObj.GroupName, inputObj.Description, inputObj.ParentGroupId, loggedUserId, inputObj.WorkspaceId, inputObj.UserGroupImage);
            }
            var idUser = 0;

            List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.UserList);
            foreach (UsersInfo userItem in objUserList)
            {
                idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, idGroup, loggedUserId);
                if (notify)
                {
                    bool bStatus = false;

                    EmailActivity objEmail = new EmailActivity();

                    //Get ClientName by userID- logged userID
                    //string ClientName = "TestClient"; //GetClientNameById(userID);
                    GroupNotify obj = new GroupNotify();
                    obj.UserName = userItem.Name;
                    obj.GroupName = inputObj.GroupName;
                    obj.UserEmailId = userItem.EmailId;
                    bStatus = objEmail.SendMail("Group Add Users", obj);
                }
            }
            return Json(new { status = "success", data = idGroup }, new JsonSerializerSettings());
        }

        [HttpPost] //soft delete of a group(parent only currently)
        public ActionResult DeleteUserGroup(Group inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            objUserGroupsDAL.DeleteGroup(inputObj.GroupId, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //change the state IsActive
        public ActionResult ChangeGroupState(UserGroups inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            objUserGroupsDAL.ChangeGroupState(inputObj.GroupId, inputObj.IsActive, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //get group details by GroupId
        public ActionResult GetGroupDetails(Group inputObj)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var groupDetails = objUserGroupsDAL.GetGroupDetails(inputObj.GroupId);

            return Json(new { status = "success", data = groupDetails }, new JsonSerializerSettings());
        }

        [HttpPost] //soft delete of a group(parent only currently)
        public ActionResult UpdateUserGroup(AddGroup inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            var Files = Request.Form.Files;

            if (Files != null && Files.Count > 0)
            {
                string path = _hostingEnvironment.WebRootPath + "//Upload//" + inputObj.location + "//" + inputObj.WorkspaceId;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Guid.NewGuid().ToString();

                foreach (var itm in Files)
                {
                    fileName = fileName + "." + itm.FileName.Split(".")[1];
                    inputObj.UserGroupImage = "/" + inputObj.location + "/" + inputObj.WorkspaceId + "/" + fileName;

                    using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                    {
                        itm.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
            }

            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            objUserGroupsDAL.UpdateGroup(inputObj.GroupId, inputObj.GroupName, inputObj.Description, loggedUserId, inputObj.UserGroupImage);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //add bulk users to bulk groups
        public ActionResult BulkAddUsersToGroups(Bulk inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var userDal = new UserDAL();
            List<UserGroups> objGroupList = JsonConvert.DeserializeObject<List<UserGroups>>(inputObj.GroupList);
            var notify = inputObj.Notify;
            int groupId = 0;
            bool bStatus = false;

            foreach (UserGroups groupItem in objGroupList)
            {
                var idUser = 0;

                if (groupId == 0)
                {
                    groupId = groupItem.GroupId;
                }

                List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.UserList);
                foreach (UsersInfo userItem in objUserList)
                {
                    idUser = objUserGroupsDAL.AddUserToGroup(userItem.ID, groupItem.GroupId, loggedUserId);
                    //Commenting the existing code for adding the users to the group

                    //if (notify)
                    //{
                    //    bool bStatus = false;

                    //    EmailActivity objEmail = new EmailActivity();

                    //    //Get ClientName by userID- logged userID
                    //    //string ClientName = "TestClient"; //GetClientNameById(userID);
                    //    GroupNotify obj = new GroupNotify();
                    //    obj.UserName = userItem.Name;
                    //    obj.GroupName = groupItem.GroupName;
                    //    obj.UserEmailId = userItem.EmailId;
                    //    bStatus = objEmail.SendMail("Group Add Users", obj);
                    //}
                }
            }

            List<UsersInfo> objUser = JsonConvert.DeserializeObject<List<UsersInfo>>(inputObj.UserList);
            int workspaceId = userDal.GetWorkspaceIdByGroupId(groupId);

            foreach (UsersInfo userItem in objUser)
            {
                string userName = userDal.GetUserName(userItem.EmailId);

                bStatus = userDal.SendEmailtoAssignedUsersFromWorkspace(userItem.EmailId, userName, loggedUserId, workspaceId, 0);

                if (!bStatus)
                    return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult DeleteAssignedUser(AssignUser inputObj)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            objUserGroupsDAL.DeleteAssignedUser(inputObj.GroupId, inputObj.UserId, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //get projects list for a groupId
        public ActionResult GetProjectsList(Group inputObj)
        {
            UserGroupsDAL objUserGroupsDAL = new UserGroupsDAL();
            var projectslist = objUserGroupsDAL.GetProjectsList(inputObj.GroupId);

            return Json(new { status = "success", data = projectslist }, new JsonSerializerSettings());
        }
    }
}