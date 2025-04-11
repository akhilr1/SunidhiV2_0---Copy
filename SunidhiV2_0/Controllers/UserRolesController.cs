using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunidhiV2_0.Code.Common;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Models;

namespace SunidhiV2_0.Controllers
{
    public class UserRolesController : Controller
    {
        [HttpPost]
        public IActionResult GetUserRolesLists(int WorkspaceID)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var userRoleList = objUserRoleDAL.GetUserRolesList(WorkspaceID);

            return Json(new { status = "success", data = userRoleList }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetRoleBasedScreenList(int WorkspaceID)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var userRoleList = objUserRoleDAL.GetRoleBasedScreenList(WorkspaceID, loggedUserId);

            return Json(new { status = "success", data = userRoleList }, new JsonSerializerSettings());

        }
        [HttpGet]
        public IActionResult GetScreenList()
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var ScreenList = objUserRoleDAL.GetAllScreenList();

            return Json(new { status = "success", data = ScreenList }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult AddRole(UserRoles objUserRoles)
        {
            UserRoleDAL objUserRolesDAL = new UserRoleDAL();
            var intRoleId = objUserRoles.RoleId;
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            if (intRoleId == 0)
            {
                intRoleId = objUserRolesDAL.AddRole(objUserRoles.RoleName , objUserRoles.Description, objUserRoles.WorkspaceID, loggedUserId);
            }
            var intScreenId = 0;

            List<ScreenList> objScreenList = JsonConvert.DeserializeObject<List<ScreenList>>(objUserRoles.ScreenList);
            foreach (ScreenList userItem in objScreenList)
            {
                intScreenId = objUserRolesDAL.AddRoleScreens(intRoleId, userItem.Screenid, userItem.AccessId, loggedUserId);
            }

            return Json(new { status = "success", data = intRoleId }, new JsonSerializerSettings());
        }
        
        [HttpGet]
        public IActionResult GetAllAceesType()
        {
            UserRoleDAL objAccessType = new UserRoleDAL();
            var ScreenList = objAccessType.GetAllAceesType();

            return Json(new { status = "success", data = ScreenList }, new JsonSerializerSettings());
        }

        [HttpPost] //get role details by RoleId
        public ActionResult GetRoleDetails(UserRoles inputObj)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var roleDetails = objUserRoleDAL.GetRoleDetails(inputObj.RoleId);

            return Json(new { status = "success", data = roleDetails }, new JsonSerializerSettings());
        }

        [HttpPost] //get users list by RoleId
        public ActionResult GetUsersByRole(UserRoles inputObj)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var userByRoleList = objUserRoleDAL.GetUsersByRole(inputObj.RoleId);

            return Json(new { status = "success", data = userByRoleList }, new JsonSerializerSettings());
        }

        [HttpPost] //get screen list by RoleId
        public ActionResult GetScreensByRole(UserRoles inputObj)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var screensByRoleList = objUserRoleDAL.GetScreensByRole(inputObj.RoleId);

            return Json(new { status = "success", data = screensByRoleList }, new JsonSerializerSettings());
        }

        [HttpPost] //soft delete 
        public ActionResult DeleteUserRole(Role objRole)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var notify = objRole.Notify;

            if (objRole.RoleId > 0)
            {
                objUserRoleDAL.DeleteUserRole(objRole.RoleId, loggedUserId);
            }
            if (objRole.UsersList.Length  > 0)
            {
                List<RoleBasedUserList_New> objUserlist = JsonConvert.DeserializeObject<List<RoleBasedUserList_New>>(objRole.UsersList);
                foreach (RoleBasedUserList_New userItem in objUserlist)
                {
                    objUserRoleDAL.UpdateUserRole(userItem.ID, userItem.RoleId.RoleId, objRole.WorkspaceID);
                    // Sending Mail
                    if (notify)
                    {
                        EmailActivity objEmail = new EmailActivity();
                        UserRoleList objUsrRole = new UserRoleList();
                        objUsrRole.userId = userItem.ID;
                        objUsrRole.userName = userItem.Name;
                        objUsrRole.userRole = userItem.RoleId.RoleName;
                        objUsrRole.userRoleID = userItem.RoleId.RoleId.ToString();
                        objUsrRole.EmailId = userItem.EmailID;

                        bool bStatus = objEmail.SendMail("AssignRole", objUsrRole, loggedUserId);
                    }
                }
            }
                         
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost] //get users list for that Workspace ID
        public ActionResult GetUsersToAssignRole(int WorkspaceID)
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var userList = objUserRoleDAL.GetUsersToAssignRole(WorkspaceID);

            return Json(new { status = "success", data = userList }, new JsonSerializerSettings());
        }

        [HttpPost] //get users screen that does not have role
        public ActionResult GetUserWithoutscreens(int RoleId )
        {
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            var userByRoleList = objUserRoleDAL.GetUserWithoutscreens(RoleId);

            return Json(new { status = "success", data = userByRoleList }, new JsonSerializerSettings());
        }

        public IActionResult AssignUsersToRole(AssignUsersToRole objUserRoles)
        {
            UserRoleDAL objUserRolesDAL = new UserRoleDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            List<UserList> objUsersList = JsonConvert.DeserializeObject<List<UserList>>(objUserRoles.UserList);
            var notify = objUserRoles.notify;
            var RoleName = objUserRoles.RoleName;
            var RoleId = objUserRoles.RoleId;
            foreach (UserList userItem in objUsersList)
            {
               objUserRolesDAL.AssignUsersToRole(RoleId, userItem.ID, objUserRoles.WorkspaceID);

                // Sending Mail
                if (notify)
                {
                    EmailActivity objEmail = new EmailActivity();
                    UserRoleList objUsrRole = new  UserRoleList();
                    objUsrRole.userId = userItem.ID;
                    objUsrRole.userName = userItem.Name;
                    objUsrRole.userRole = RoleName;
                    objUsrRole.userRoleID = RoleId.ToString();
                    objUsrRole.EmailId = userItem.EmailID;

                    bool bStatus = objEmail.SendMail("AssignRole", objUsrRole, loggedUserId);
                }
            }

            return Json(new { data = "success" }, new JsonSerializerSettings());
        }

        //public IActionResult GetUserRoleDetailsBasedOnRoleId(UserRoles inputObj)
        //{
        //    UserRoleDAL objUserRoleDAL = new UserRoleDAL();
        //    var screensByRoleList = objUserRoleDAL.GetUserRoleDetailsBasedOnRoleId(inputObj.RoleId);

        //    return Json(new { status = "success", data = screensByRoleList }, new JsonSerializerSettings());
        //}

        public IActionResult UpdateRole(UserRoles inputObj)
        {
            UserRoleDAL objUserRolesDAL = new UserRoleDAL();
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (inputObj.RoleId > 0)
            {
                objUserRolesDAL.UpdateRole(inputObj.RoleId, inputObj.RoleName, inputObj.Description, loggedUserId);
            }

            return Json(new { data = "success" }, new JsonSerializerSettings());
        }
               
        public ActionResult GetRoleBasedUsersList(int RoleId, int WorkspaceID)
        {
            UserRoleDAL objRoleBaesedRoleDAL = new UserRoleDAL();
            var userByRoleList = objRoleBaesedRoleDAL.GetRoleBasedUsersList(RoleId, WorkspaceID);

            return Json(new { status = "success", data = userByRoleList }, new JsonSerializerSettings());
        }

        public ActionResult DeleteAssignedUser( int userID, string userName, string userEmail, int NewRoleId, string NewRoleName, int WorkspaceID, bool notify)
        {
            try
            {
                int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                UserRoleDAL objUserRoleDAL = new UserRoleDAL();
                objUserRoleDAL.UpdateUserRole(userID, NewRoleId, WorkspaceID);

                // Sending Mail
                if (notify)
                {
                    EmailActivity objEmail = new EmailActivity();
                    UserRoleList objUsrRole = new UserRoleList();
                    objUsrRole.userId = userID;
                    objUsrRole.userName = userName;
                    objUsrRole.userRole = NewRoleName;
                    objUsrRole.userRoleID = NewRoleId.ToString();
                    objUsrRole.EmailId = userEmail;

                    bool bStatus = objEmail.SendMail("AssignRole", objUsrRole, loggedUserId);
                }
                
                return Json(new { data = "success" }, new JsonSerializerSettings());
            }
            catch
            {
                return Json(new { data = "failed" }, new JsonSerializerSettings());
            }
        }

        [HttpPost] //soft delete 
        public ActionResult DeleteScreenFromRole(int ScreenId, int RoleId)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserRoleDAL objUserRoleDAL = new UserRoleDAL();
            objUserRoleDAL.DeleteScreenFromRole(RoleId, ScreenId, loggedUserId);

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

    }
}
