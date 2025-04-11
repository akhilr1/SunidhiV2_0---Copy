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
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace SunidhiV2_0.Controllers
{
    public class UserAccountController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UserAccountController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public async Task<ActionResult> GetShareLinkStatus(int SurveyID)
        {
            bool status = false;
            string statusmsg = string.Empty;
            int UserID = Convert.ToInt32(Startup.ShareLinkUserID);
            UserDAL objUserDAL = new UserDAL();
            var isExists = objUserDAL.GetShareLinkStatus(UserID, SurveyID);
            if (!isExists)
            {
                statusmsg = "invalid";
            }
            return Json(new { status = statusmsg });
        }

        public async Task<ActionResult> GetWorkflowLevelOneUsers(int SurveyID)
        {
            string strWorkflowLevelOneUsers = string.Empty;
            UserDAL objUserDAL = new UserDAL();
            strWorkflowLevelOneUsers = objUserDAL.GetWorkflowLevelOneUsers(SurveyID);
            return Json(new { workflowleveloneusers = strWorkflowLevelOneUsers });
        }

        public async Task<List<string>> DecryptShareLinkURL(string EncryptedSurveyID, string EncryptedWorkspaceID)
        {
            EncryptDecrypt objEncryptDecrypt = new EncryptDecrypt();
            string strEncryptionKey = objEncryptDecrypt.Encryptionkey();
            List<string> decryptedids = new List<string>();
            string surveyid = !string.IsNullOrEmpty(EncryptedSurveyID) ? objEncryptDecrypt.DecryptData(EncryptedSurveyID, strEncryptionKey) : null;
            decryptedids.Add(surveyid);
            string workspaceid = !string.IsNullOrEmpty(EncryptedWorkspaceID) ? objEncryptDecrypt.DecryptData(EncryptedWorkspaceID, strEncryptionKey) : null;
            decryptedids.Add(workspaceid);
            return decryptedids;
        }

        [AllowAnonymous]
        public async Task<ActionResult> ShareLinkLogin()
        {
            string ID = Startup.ShareLinkUserID;
            string statusmsg = string.Empty;

            UserDAL objUserDAL = new UserDAL();


            //
            string EmailId = "sharelinkemail@test.com";

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, EmailId),
                            new Claim("ID", ID.ToString()),
                            new Claim("LoggedUserName", "sharelinktest")
                        };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

            statusmsg = "valid";


            return Json(new { status = statusmsg });
        }

        [HttpPost]
        public IActionResult GetShareLinkTaskFromServer(int SurveyID, int WorkspaceID)
        {
            UserDAL objUser = new UserDAL();
            ListSyncItem objListSyncItem = new ListSyncItem();
            int UserID = Convert.ToInt32(Startup.ShareLinkUserID);
            objListSyncItem.lstMyTaskList = objUser.GetShareLinkTaskFromServer(SurveyID, WorkspaceID, UserID);
            return Json(new { status = "success", data = new { MyTaskList = objListSyncItem.lstMyTaskList } }, new JsonSerializerSettings());
        }

        #region Login_Sec

        #region Login
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="objMobileUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(MobileUser objMobileUser)
        {
            HttpContext.Session.SetString("imworks-session", "Test"); // this is to take effect the session cookie in startup.cs
            string statusmsg = string.Empty;
            bool isLoggedIn = false;
            int userId = 0;
            string username = string.Empty;
            int UserType = 0;
            string userEmail = string.Empty;

            ////// To be commented when the front end value for both username and password is encoded
            objMobileUser.username = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(objMobileUser.username));
            //objMobileUser.password = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(objMobileUser.password));
            //// To be commented when the front end value for both username and password is encoded

            string orginalPassword = string.Empty;
            string originalUsername = string.Empty;

            byte[] usernameData = Convert.FromBase64String(objMobileUser.username);
            originalUsername = System.Text.Encoding.UTF8.GetString(usernameData);
            objMobileUser.username = originalUsername;

            byte[] passwordData = Convert.FromBase64String(objMobileUser.password);
            orginalPassword = System.Text.Encoding.UTF8.GetString(passwordData);
            objMobileUser.password = orginalPassword;

            UserDAL objUserDL = new UserDAL();

            if (!string.IsNullOrEmpty(objMobileUser.username) && !string.IsNullOrEmpty(objMobileUser.password))
            {
                string action = string.Empty;

                var encryptedPassword = Code.Utils.CryptoUtils.EncryptPassword(objMobileUser.password);
                var userList = objUserDL.GetUserDetails(objMobileUser.username, encryptedPassword);

                if (userList != null)
                {
                    if (!userList.IsActive)
                    {
                        statusmsg = "deactivated";
                    }
                    else
                    {
                        objUserDL.UpdateLoginDetails(userList.EmailId, false, userList.ID);
                        isLoggedIn = true;
                        userId = userList.ID;
                        username = userList.Name;
                        UserType = userList.UserType;
                        userEmail = userList.EmailId;

                        /*Notification Token saving Started*/
                        if (!string.IsNullOrEmpty(objMobileUser.notificationtoken))
                        {
                            Notification objNotification = new Notification();
                            objNotification.UserId = userId;
                            objNotification.Token = objMobileUser.notificationtoken;
                            ManageNotificationToken(objNotification);
                        }
                        /*Notification Token saving Ended*/

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userList.EmailId),
                            new Claim("ID", userList.ID.ToString()),
                            new Claim("LoggedUserName", username)
                        };

                        var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                        statusmsg = "valid";
                       // EmailActivity objEmail = new EmailActivity();
                       // objEmail.SendMail("ImpactWorks - Login Request", objMobileUser);
                    }
                }
                else
                {
                    objUserDL.UpdateLoginDetails(objMobileUser.username, true, 0);
                    statusmsg = "invalid";
                }
            }
            else
            {
                objUserDL.UpdateLoginDetails(objMobileUser.username, true, 0);
                statusmsg = "invalid";
            }

            return Json(new { status = isLoggedIn, statusmsg = statusmsg, userId = userId, username = username, userType = UserType, userEmail = userEmail });
        }
        #endregion Login

        #region IsAuthenticated
        /// <summary>
        /// IsAuthenticated
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimlist = User.Claims.ToList();
                if (claimlist.Count > 0)
                {
                    return Int32.Parse(claimlist[1].Value);
                    //return Json(new { status = true , statusmsg = "Valid", userId = Int32.Parse(claimlist[1].Value), username = claimlist[2].Value, userEmail = claimlist[0].Value });
                }
            }
            return 0;
        }
        #endregion IsAuthenticated

        #region Logout
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task<string> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "true";
        }
        #endregion Logout

        #endregion Login_Sec

        #region GetUserList_Sec

        #region GetAllUserList
        UserDAL objUser = new UserDAL();
        [HttpPost]
        public IActionResult GetAllUserDetails(int WorkspaceId)
        {
            List<UsersInfo> lstUser = new List<UsersInfo>();
            lstUser = objUser.GetAllUsers(WorkspaceId).ToList();
            return Json(new { data = lstUser }, new JsonSerializerSettings());

        }

        //[HttpPost]
        //public IActionResult GetAllUsersList(int WorkspaceId)
        //{
        //    List<UsersInfo> lstUser = new List<UsersInfo>();
        //    lstUser = objUser.GetAllUsersList(WorkspaceId).ToList();
        //    return Json(new { data = lstUser }, new JsonSerializerSettings());

        //}

        /// <param name="userId"></param>       
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUserIndividualDetails(int userId = 0, int WorkspaceID = 0)
        {
            List<UsersInfo> lstUser = new List<UsersInfo>();
            lstUser = objUser.GetUserIndividualDetails(userId, WorkspaceID).ToList();
            return Json(new { data = lstUser }, new JsonSerializerSettings());

        }
        #endregion GetAllUserList

        #region GetUserRoleDetails
        /// <param name="userId"></param>
        [HttpPost]
        public IActionResult GetUserRoleDetails(int WorkspaceId) //(int userId)
        {
            List<UsersRoleInfo> lstUser = new List<UsersRoleInfo>();
            lstUser = objUser.GetUserRoleDetails(WorkspaceId).ToList();
            return Json(new { data = lstUser }, new JsonSerializerSettings());

        }
        #endregion GetUserRoleDetails


        #region GetUserScreenDetails
        /// <param name="RoleID"></param>
        [HttpPost]
        public IActionResult GetUserScreenDetails(int RoleID = 0)
        {
            List<UsersScreenInfo> lstUser = new List<UsersScreenInfo>();
            lstUser = objUser.GetUserScreenDetails(RoleID).ToList();
            return Json(new { data = lstUser }, new JsonSerializerSettings());

        }

        #endregion GetUserScreenDetails



        #region GetUserGroupDetails
        [HttpPost]
        /// <param name="userId"></param> 
        public IActionResult GetUserGroupList(int userId, int WorkspaceID)
        {
            UserDAL objUsergroupDAL = new UserDAL();
            var userGroupList = objUsergroupDAL.GetUserGroupLists(userId, WorkspaceID);

            return Json(new { data = userGroupList }, new JsonSerializerSettings());

        }
        #endregion GetUserGroupDetails
        #region GetAllUserProjectsDetails
        [HttpPost]
        /// <param name="userId"></param> 
        public IActionResult GetAllUserProjectsDetails(int userId, int WorkspaceID)
        {
            UserDAL objUsergroupDAL = new UserDAL();
            var userGroupList = objUsergroupDAL.GetAllUserProjectsDetails(userId, WorkspaceID);

            return Json(new { data = userGroupList }, new JsonSerializerSettings());

        }
        #endregion GetAllUserProjectsDetails

        #endregion GetUserList_Sec

        #region DeleteUser
        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="userId"></param>
        /// /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUser(int userId, string uName, string uEmail, bool ChkNotified, int WorkspaceID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var retStatus = "";
            bool bStatus = false;
            UserDAL objUser = new UserDAL();
            retStatus = objUser.DeleteUser(userId, loggedUserId, WorkspaceID);
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());

            if (ChkNotified)
            {

                EmailActivity objEmail = new EmailActivity();
                UsersInfo objUsr = new UsersInfo();

                objUsr.Name = uName;
                objUsr.EmailId = uEmail;
                bStatus = objEmail.SendMail("DeleteUser", objUsr, loggedUserId);
                if (!bStatus)
                    return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());

        }

        #endregion DeleteUser
        [HttpPost]
        public ActionResult ChangeActiveStatus(int userId, bool userStatus, string uName, string uEmail, bool ChkNotified, int WorkspaceID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var retStatus = "";
            bool bStatus = false;
            UserDAL objUser = new UserDAL();
            retStatus = objUser.ChangeActiveStatus(userId, loggedUserId, userStatus, WorkspaceID);
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            if (ChkNotified)
            {

                EmailActivity objEmail = new EmailActivity();
                UsersInfo objUsr = new UsersInfo();

                objUsr.Name = uName;
                objUsr.UserStatus = userStatus;
                objUsr.EmailId = uEmail;
                bStatus = objEmail.SendMail("ChangeUserStatus", objUsr, loggedUserId);
                if (!bStatus)
                    return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #region AssignUserRole
        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRoleID"></param>
        /// <param name="userAccessID"></param>
        /// /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult AssignUserRole(bool isNotified, string userRoleListInput, int WorkspaceID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            var retStatus = "";
            bool bStatus = false;

            UserDAL objUser = new UserDAL();
            List<UserRoleList> objUserRoleList = JsonConvert.DeserializeObject<List<UserRoleList>>(userRoleListInput);

            foreach (UserRoleList userRoleItem in objUserRoleList)
            {
                retStatus = objUser.AssignUserRole(userRoleItem.userId, loggedUserId, Int32.Parse(userRoleItem.userRoleID), WorkspaceID);
                if (retStatus != "")
                    return Json(new { status = retStatus }, new JsonSerializerSettings());
                if (isNotified)
                {
                    EmailActivity objEmail = new EmailActivity();
                    UserRoleList objUsrRole = (UserRoleList)userRoleItem;

                    bStatus = objEmail.SendMail("AssignRole", objUsrRole, loggedUserId);
                    if (!bStatus)
                        return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                }

            }
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion AssignUserRole

        #region AssignUserToGroup     
        [HttpPost]
        public ActionResult AssignUserToGroup(bool isNotified, string userGroupList)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            string userGroups = "";
            bool bStatus = false;
            UserDAL objUser = new UserDAL();

            List<userGroupList> objUserGroupList = JsonConvert.DeserializeObject<List<userGroupList>>(userGroupList);
            var retStatus = "";

            int groupId = 0;

            foreach (userGroupList userItem in objUserGroupList)
            {
                foreach (UGroupList groupItem in userItem.uGroupList)
                {
                    //Setting atleast one group id (as all work groups belongs to the same workspace seleced) to get the workspace information at later point while sending the mail.

                    if (groupId == 0)
                    {
                        groupId = groupItem.uGroupId;
                    }

                    userGroups = userGroups == "" ? groupItem.uGroupName : userGroups + " , " + groupItem.uGroupName;
                    retStatus = objUser.AddUserToGroup(userItem.userId, groupItem.uGroupId, loggedUserId);

                     if (retStatus != "")
                        return Json(new { status = retStatus }, new JsonSerializerSettings());
                }

                //if (isNotified)
                //{
                //    //Send Notification                  

                //    EmailActivity objEmail = new EmailActivity();
                //    GroupNotify objUsrGrp = new GroupNotify();
                //    objUsrGrp.UserName = userItem.userName;
                //    objUsrGrp.GroupName = userGroups;
                //    objUsrGrp.UserEmailId = userItem.EmailId;
                //    bStatus = objEmail.SendMail("AssignGroup", objUsrGrp, loggedUserId);
                //    if (!bStatus)
                //        return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                //}
            }

            //Sending mails after all the users has been assigned to a group
            if (isNotified)
            {
                int workSpaceId = objUser.GetWorkspaceIdByGroupId(groupId);                

                foreach (userGroupList userItem in objUserGroupList)
                {
                    string userName = objUser.GetUserName(userItem.EmailId);

                    bStatus = objUser.SendEmailtoAssignedUsersFromWorkspace(userItem.EmailId, userName, loggedUserId, workSpaceId, 0);

                    if (!bStatus)
                        return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                }
            }

            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion AssignUserToGroup

        #region Invite_Users

        [HttpPost]
        public ActionResult InviteUser(string userInviteList, int WorkspaceId, string inviteFrom = "", int InitiativeId = 0, int ProjectID = 0)
        {

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            bool bStatus = false;
            UserDAL objUser = new UserDAL();
            var retStatus = "";
            List<InviteUsersList> lstInviteUser = JsonConvert.DeserializeObject<List<InviteUsersList>>(userInviteList);
            string decryptPassword = string.Empty;
            string password = string.Empty;

            foreach (InviteUsersList userItem in lstInviteUser)
            {
                password = Code.Utils.CryptoUtils.RandomPasswordGenerator(8, false);

                retStatus = objUser.AddInviteUser(userItem.EmailId, userItem.UserRole.userRoleID, loggedUserId, WorkspaceId, inviteFrom, InitiativeId, ProjectID, password);
                
                if (retStatus != "")
                    return Json(new { status = retStatus }, new JsonSerializerSettings());

                var objWorkspaceInfo = objUser.GetWSInfoOfInvitedUser(userItem.EmailId); 

                if (ProjectID != 0)
                {
                    if (objWorkspaceInfo != null && objWorkspaceInfo.WorkspaceID != 0 && objWorkspaceInfo.WorkspaceID != WorkspaceId)
                    {
                        if (objWorkspaceInfo.Name == null)
                        {
                            string[] names = !string.IsNullOrEmpty(userItem.EmailId) ? userItem.EmailId.Split("@") : null;

                            if (names != null && names.Length > 0)
                            {
                                objWorkspaceInfo.Name = names[0];
                            }                             
                        }
                        bStatus = objUser.SendEmailtoAssignedUsersFromWorkspace(userItem.EmailId, objWorkspaceInfo.Name, loggedUserId, WorkspaceId, ProjectID);
                    }
                    else
                        bStatus = objUser.InviteUserFromWorkspaceOrProject(userItem.EmailId, userItem.UserRole.userRole, 
                            User.Identity.Name, loggedUserId, password, inviteFrom, InitiativeId, WorkspaceId, ProjectID);
                }
                else if (WorkspaceId != 0)
                {
                    if (objWorkspaceInfo != null && objWorkspaceInfo.WorkspaceID != 0 && objWorkspaceInfo.WorkspaceID != WorkspaceId)
                    {
                        if (objWorkspaceInfo.Name == null)
                        {
                            string[] names = !string.IsNullOrEmpty(userItem.EmailId) ? userItem.EmailId.Split("@") : null;

                            if (names != null && names.Length > 0)
                            {
                                objWorkspaceInfo.Name = names[0];
                            }
                        }
                        bStatus = objUser.SendEmailtoAssignedUsersFromWorkspace(userItem.EmailId, objWorkspaceInfo.Name, loggedUserId, WorkspaceId, 0);
                    }
                    else
                        bStatus = objUser.InviteUserFromWorkspaceOrProject(userItem.EmailId, userItem.UserRole.userRole, User.Identity.Name, loggedUserId, password, inviteFrom, InitiativeId, WorkspaceId, 0);
                }

                if (!bStatus)
                    return Json(new { status = retStatus }, new JsonSerializerSettings());
            }
            return Json(new { status = "success" }, new JsonSerializerSettings());

        }
        #endregion Invite_Users

        [HttpGet]
        public ActionResult GetLogUser()
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserDAL objUserDAL = new UserDAL();
            var userdetails = objUserDAL.GetLogUser(loggedUserId);

            return Json(new { data = userdetails }, new JsonSerializerSettings());
        }
        [HttpPost]
        public ActionResult UpdateLogUser(LoggedUserInfo inputObj)
        {
            UserDAL objUserDAL = new UserDAL();

            if (inputObj.ID > 0)
            {

                var Files = Request.Form.Files;

                if (Files != null && Files.Count > 0)
                {
                    string path = _hostingEnvironment.WebRootPath + "//Upload//" + inputObj.location + "//" + inputObj.ID;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = Guid.NewGuid().ToString();

                    foreach (var itm in Files)
                    {
                        fileName = fileName + "." + itm.FileName.Split(".")[1];
                        inputObj.ProfileImageName = "/" + inputObj.location + "/" + inputObj.ID + "/" + fileName;

                        using (var fileStream = new FileStream((path + "//" + fileName), FileMode.Create))
                        {
                            itm.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }
                }

                objUserDAL.UpdateLogUser(inputObj);
            }

            return Json(new { data = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword objpassword)
        {
            string statusmsg = string.Empty;
            UserDAL objUserDAL = new UserDAL();

            if (!string.IsNullOrEmpty(objpassword.oldpassword) && !string.IsNullOrEmpty(objpassword.newpassword))
            {
                var encryptedOldPassword = Code.Utils.CryptoUtils.EncryptPassword(objpassword.oldpassword);
                var encryptedNewPassword = Code.Utils.CryptoUtils.EncryptPassword(objpassword.newpassword);

                var userList = objUserDAL.OldPasswordMatch(objpassword.Id, encryptedOldPassword);

                if (userList == null)
                {
                    statusmsg = "The value entered for the Old Password field is not correct";
                    return Json(new { status = "failed", statusmessage = statusmsg }, new JsonSerializerSettings());
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(objpassword.newpassword, "(?=.*[0-9])(?=.*[[a-zA-Z])(?=.*[!@#$%&*])[a-zA-Z0-9!@#$%&*]{8,14}$") == false)
                {
                    statusmsg = "New Password must be between 8 and 14 characters and Must be a combination of letters,numbers and special characters";
                    return Json(new { status = "failed", statusmessage = statusmsg }, new JsonSerializerSettings());
                }
                else if (objUserDAL.IsPasswordUsedBefore(encryptedNewPassword, objpassword.Id))
                {
                    statusmsg = "You have entered this Password before";
                    return Json(new { status = "failed", statusmessage = statusmsg }, new JsonSerializerSettings());
                }
                else
                {
                    objUserDAL.ChangePassword(encryptedNewPassword, userList.ID);
                    return Json(new { status = "success" }, new JsonSerializerSettings());
                }
            }
            else
            {
                statusmsg = "You have not entered old/new password";
                return Json(new { status = "failed", statusmessage = statusmsg }, new JsonSerializerSettings());
            }
        }

        // function used to fetech User Access Page list
        [HttpPost]
        public ActionResult FetchUserAccessPagesList(int WorkspaceID)
        {
            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            UserDAL objUserDAL = new UserDAL();
            var Pagelist = objUserDAL.FetchUserAccessPagesList(WorkspaceID, loggedUserId);

            return Json(new { data = Pagelist }, new JsonSerializerSettings());
        }
        #region ManageNotificationToken
        /// <summary>
        /// ManageNotificationToken
        /// </summary>
        /// <param name="objNotification">objNotification</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageNotificationToken(Notification objNotification)
        {
            int numberofToken = Convert.ToInt32(Startup.NumberOfUserToken);
            UserDAL objUserDAL = new UserDAL();
            objUserDAL.ManageUserToken(objNotification.UserId, objNotification.Token, numberofToken);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
        #endregion ManageNotificationToken

        #region Signup
        /// <summary>
        /// New user signup 
        /// </summary>
        /// <param name="objUser">objUser</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Signup(SignupInfo objSignup)
        {
            bool status = false;
            bool mailstatus = false;
            string mailerror = string.Empty;
            UserDAL objUserDAL = new UserDAL();

            if (!string.IsNullOrEmpty(objSignup.EmailId) && !string.IsNullOrEmpty(objSignup.Password))
            {
                byte[] data = Convert.FromBase64String(objSignup.Password);
                string decodedPassword = System.Text.Encoding.UTF8.GetString(data);
                var encryptedPassword = Code.Utils.CryptoUtils.EncryptPassword(decodedPassword);
                //var encryptedPassword = Code.Utils.CryptoUtils.EncryptPassword(objSignup.Password);
                string OTP = CommonActivity.GenerateOTP();

                status = objUserDAL.Signup(objSignup.Name, objSignup.EmailId, encryptedPassword, OTP);

                if (status)
                {
                    try
                    {
                        EmailActivity objEmail = new EmailActivity();
                        objSignup.OTP = OTP;
                        
                        mailstatus = objEmail.SendMail("SignupNewUser", objSignup);
                        if (!mailstatus)
                        {
                            mailerror = "Failed to send otp email";
                            //return Json(new { status = "Fail to send notification mail" }, new JsonSerializerSettings());
                        }
                         objEmail.SendMail("ImpactWorks - New Signup", objSignup);
                    }
                    catch (Exception ex)
                    {
                        mailerror = "Failed to send otp email";
                        status = false;
                    }

                }

            }

            return Json(new { status = status, mailstatus = mailstatus, mailerror = mailerror }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult VerifySignupOTP(SignupInfo objSignup)
        {
            //bool status = false;
            string verifyStatus = string.Empty;
            UserDAL objUserDAL = new UserDAL();
            //List<SignupInfo> lstVerifyOTP = new List<SignupInfo>();
            if (!string.IsNullOrEmpty(objSignup.EmailId) && !string.IsNullOrEmpty(objSignup.OTP))
            {
                verifyStatus = objUserDAL.VerifySignupOTP(objSignup.EmailId, objSignup.OTP);
                //lstVerifyOTP = objUserDAL.VerifySignupOTP(objSignup.EmailId, objSignup.OTP);
                //if (lstVerifyOTP != null)
                //{
                //    if (lstVerifyOTP.First().UserID > 0)
                //    {
                //        if ((DateTime.Now - Convert.ToDateTime(lstVerifyOTP.First().CreatedDate)).TotalMinutes > 30)
                //        {
                //            verifyError = "OTP Timedout";
                //        }
                //        else
                //        {
                //            status = true;
                //        }
                //    }
                //}
            }

            return Json(new { status = verifyStatus }, new JsonSerializerSettings());
        }
        #endregion Signup   

        #region ForgotPassword
        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword(string emailId)
        {
            string statusMsg = string.Empty;
            bool status = false;

            UserDAL objUserDAL = new UserDAL();

            string response = objUserDAL.ForgotPassword(emailId);

            if (response == "success")
            {
                status = true;
                statusMsg = "Mail successfully send to the email id " + emailId
                        + ". Please access the link in the mail to reset your password.";
            }
            else
            {
                if (response == "failure")
                {
                    status = false;
                    statusMsg = "Failed to sent the mail.";
                }
                else
                {
                    status = false;
                    statusMsg = response;
                }
            }

            return Json(new { status, statusMsg }, new JsonSerializerSettings());
        }
        #endregion ForgotPassword


        #region ResetPassword
        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordInfo objforgotPasswordInfo)
        {
            string statusMsg = string.Empty;
            bool status = false;
            UserDAL objUserDAL = new UserDAL();

            var encodedEMail = objforgotPasswordInfo != null ? objforgotPasswordInfo.key1 : "";
            //string emailId = Encoding.UTF8.GetString(Convert.FromBase64String(encodedEMail));

            var encodedPassword = objforgotPasswordInfo != null ? objforgotPasswordInfo.Password : "";
            string passsword = string.Empty;

            //To be commented 
            //encodedPassword = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(encodedPassword));

            byte[] data = Convert.FromBase64String(encodedPassword);
            passsword = System.Text.Encoding.UTF8.GetString(data);
            var encryptedPassword = Code.Utils.CryptoUtils.EncryptPassword(passsword);

            if (System.Text.RegularExpressions.Regex.IsMatch(passsword, "(?=.*[0-9])(?=.*[[a-zA-Z])(?=.*[!@#$%&*])[a-zA-Z0-9!@#$%&*]{8,14}$") == false)
            {
                statusMsg = "New Password must be between 8 and 14 characters and Must be a combination of letters,numbers and special characters";
                return Json(new { status = "failed", statusmessage = statusMsg }, new JsonSerializerSettings());
            }            

            statusMsg = objUserDAL.UpdatePassword(encodedEMail, encryptedPassword);

            return Json(new {  status = (statusMsg == "success") ? true : false, statusMsg= statusMsg }, new JsonSerializerSettings());
        }
        #endregion ResetPassword
    }
}