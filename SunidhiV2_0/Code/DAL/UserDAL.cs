using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using SunidhiV2_0.Code.Common;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using Org.BouncyCastle.Bcpg.OpenPgp;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.DAL
{
    public class UserDAL
    {
        protected IDbConnection _connection;

        #region GetDetails

        /// <summary>
        /// Function used to fetch user details based on EmailId,password
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserDetails(string EmailId, string password)
        {
            var user = new User();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select ID,Name,EmailId,Phone,IsActive,NoOfLogins,FailedLoginCount,UserType  " +
                                    " FROM TbUser WHERE EmailId = @EmailId and Password = @Password and IsActive = 1 and IsDeleted = 0";
                    user = _connection.Query<User>(query, new
                    {
                        EmailId = EmailId,
                        Password = password
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return user;
        }

        /// <summary>
        /// Function used to fetch all users based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<UsersInfo> GetAllUsers(int WorkspaceID)
        {
            List<UsersInfo> lstUser = new List<UsersInfo>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct U.ID , U.Name ,U.EmailId,U.Organization userOrganisation,Case when WT.IsActive = 1 then 'Active' else 'Inactive' end as State,
                             WT.IsActive UserStatus, Case When UG.grpCount > 1 then CONCAT(Cast(UG.grpCount as char),' Groups')  else UG.GroupName end as userGroup  ,
                             WT.RoleID userRoleID, R.RoleName userRole,P.ProjectCount userProjectCount  , case when(WT.RoleID = 1)  THEN True else  False end  as UserRoleStatus,ifnull(U.ProfileImage,'') ProfileImage,
                             U.IsActive,U.IsDeleted,U.IsRegisteredUser, WT.RoleID userRoleID, R.RoleName userRole 
                             FROM TbUser U Left Join TbWorkspaceTeam WT On WT.UserID = U.ID  left join TbRole R on R.RoleId = WT.RoleID  
                              left join(Select UAG.UserId, GROUP_CONCAT(Distinct UG.GroupName) GroupName, Count(Distinct UAG.GroupId) grpCount From TbUserAssignedToGroup UAG  
                              Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID Where UG.IsActive = 1 and UAG.IsDeleted = 0 and UG.IsDeleted = 0 and UG.WorkspaceId =@WorkspaceID group by UAG.UserId) UG on UG.UserId = U.ID 
                              Left Join (Select UAG.UserId , count(distinct P.ProjectId) ProjectCount From TbUserAssignedToGroup UAG Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID 
                              left join TbProject P  on P.GroupId = UAG.GroupId Where P.IsActive =1  and  UG.IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and UG.WorkspaceId =@WorkspaceID  group by UAG.UserId) P On P.UserId =U.ID 
                              Where U.IsRegisteredUser = 1 and U.IsDeleted = 0 AND WT.IsDtd=0 AND  WT.WorkspaceID =@WorkspaceID Order by U.ID Desc;";
                    lstUser = _connection.Query<UsersInfo>(strQuery, new { WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }

        /// <summary>
        /// Function used to fetch all users list based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        //public IEnumerable<UsersInfo> GetAllUsersList(int WorkspaceID)
        //{
        //    List<UsersInfo> lstUser = new List<UsersInfo>();
        //    string strQuery = string.Empty;
        //    try
        //    {
        //        using (_connection = Utils.Database.GetDBConnection())
        //        {
        //            strQuery = @"SELECT Distinct U.ID , U.Name ,U.EmailId,U.IsActive,U.IsDeleted,U.IsRegisteredUser, WT.RoleID userRoleID, R.RoleName userRole " +
        //                       " FROM TbUser U Left Join TbWorkspaceTeam WT On WT.UserID = U.ID  left join TbRole R on R.RoleId = WT.RoleID  " +
        //                       " Where WT.WorkspaceID =@WorkspaceID and WT.IsDtd = 0 and WT.IsActive = 1";

        //            lstUser = _connection.Query<UsersInfo>(strQuery, new { WorkspaceID = WorkspaceID }).ToList();


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return lstUser;
        //}    

        /// <summary>
        /// Function used to user individual details based on userID,WorkspaceID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<UsersInfo> GetUserIndividualDetails(int userID, int WorkspaceID)
        {
            List<UsersInfo> lstUser = new List<UsersInfo>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT U.ID , U.Name,U.EmailId,U.Organization userOrganisation, R.RoleName userRole , " +
                                "case when(WT.RoleID = 1)  THEN True else  False end  as UserRoleStatus,ifnull(U.ProfileImage,'') ProfileImage  FROM TbUser U " +
                                  " Left Join TbWorkspaceTeam WT On WT.UserID = U.ID left join TbRole R on R.RoleId = WT.RoleID " +
                                  " where U.ID =@userID  and  WT.WorkspaceID =@WorkspaceID";

                    lstUser = _connection.Query<UsersInfo>(strQuery, new { userID = userID, WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }

        #region GetUserGroupDetails

        /// <summary>
        /// Function used to fetch user group list based on userID,WorkspaceID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<UserGroupList> GetUserGroupLists(int userID, int WorkspaceID)
        {
            List<UserGroupList> lstUser = new List<UserGroupList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select a.GroupId, a.GroupName,Case When a.prjCount > 1 then CONCAT(Cast(a.prjCount as char),' Projects')  else a.ProjectName end as ProjectName,a.UserGroupImage from (                                
                                SELECT Distinct UG.GroupId, UG.GroupName GroupName
                                , UG.UserGroupImage,GROUP_CONCAT(Distinct tb.ProjectName) ProjectName,Count(Distinct tb.ProjectName ) prjCount
                                  FROM TbUser U Left Join TbWorkspaceTeam WT On WT.UserID = U.ID left join(SELECT UserId, UAG.GroupId, GroupName, IFNULL(UG.UserGroupImage,'') as UserGroupImage From TbUserAssignedToGroup UAG   
                                 Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID Where IsActive = 1 and UAG.IsDeleted = 0 and UG.IsDeleted = 0 and WorkspaceID =@WorkspaceID) 
                                 UG on UG.UserId = U.ID left join TbProject tb on tb.IsActive = 1 and tb.GroupId = UG.GroupId
                                 where U.ID = @userID and WT.WorkspaceID = @WorkspaceID Group by UG.GroupId Order by UG.GroupId Desc) a;";

                    lstUser = _connection.Query<UserGroupList>(strQuery, new { userID = userID, WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }
        #endregion GetUserGroupDetails

        #region GetAllUserProjectsDetails

        /// <summary>
        /// Function used to fetch user project details based on userID,WorkspaceID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<UserGroupList> GetAllUserProjectsDetails(int userID, int WorkspaceID)
        {
            List<UserGroupList> lstUser = new List<UserGroupList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @" SELECT distinct  U.ID ,P.ProjectId ,P.ProjectName,P.ProjectStatus  as ProjectStatus, '' as Tasks ,'' Workflow " +
                                " FROM TbUser U left join  TbUserAssignedToGroup AssUG on AssUG.UserId = U.ID and AssUG.IsDeleted =0 " +
                                " Left join TbGroup UG On UG.GroupID =AssUG.GroupID  and UG.IsDeleted =0  " +
                                " left join TbProject P  on P.GroupId =AssUG.GroupId where P.IsActive =1 and U.ID = @userID and UG.WorkspaceID = @WorkspaceID Order by P.ProjectId Desc;";
                    lstUser = _connection.Query<UserGroupList>(strQuery, new { userID = userID, WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }
        #endregion GetAllUserProjectsDetails

        #region GetUserRoleDetails

        /// <summary>
        /// Function used to fetch user role details based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<UsersRoleInfo> GetUserRoleDetails(int WorkspaceID)
        {
            List<UsersRoleInfo> lstUser = new List<UsersRoleInfo>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct R.RoleId userRoleID,R.RoleName userRole " +
                             "FROM  TbRole R Where R.IsActive =1  and R.WorkspaceID =@WorkspaceID Order by R.RoleId Desc;";
                    lstUser = _connection.Query<UsersRoleInfo>(strQuery, new { WorkspaceID = WorkspaceID }).ToList();


                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }
        #endregion GetUserRoleDetails

        #region GetUserScreenDetails

        /// <summary>
        /// Function used to fetch user screen details based on RoleID
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public IEnumerable<UsersScreenInfo> GetUserScreenDetails(int RoleID)
        {
            List<UsersScreenInfo> lstUser = new List<UsersScreenInfo>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select Distinct RA.ScreenId userScreenID,S.ScreenName userScreenName, RA.AccessId , A.AccessName  userAccessType " +
                                " from  TbScreen S " +
                                " inner join TbRoleBasedAccessScreens RA on RA.ScreenId = S.ScreenId  and RA.IsDeleted =0 " +
                                 " inner join TbAccessType A on A.AccessId =RA.AccessId where RA.RoleId =@RoleID Order by RA.ScreenId Desc;";
                    lstUser = _connection.Query<UsersScreenInfo>(strQuery, new { RoleID = RoleID }).ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return lstUser;
        }

        #endregion GetUserScreenDetails

        /// <summary>
        /// Function used to fetch client name based on userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetClientNameById(int userID)
        {
            string userClientName = "";
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct t.ClientName FROM  TbClients t inner join TbUser u on t.Client_ID = u.Client_ID and 
                            u.ID =@userID and t.IsActive =true;";
                    userClientName = _connection.Query<String>(strQuery, new { userID = userID }).ToString();

                }
            }
            catch (Exception ex)
            {
            }
            return userClientName;
        }

        #endregion GetDetails

        #region DeleteUser
        /// <summary>
        /// Function used to delete workspace team based on Id,WorkspaceID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userStatus"></param>
        /// 
        public string DeleteUser(int Id, int userId, int WorkspaceID)
        {
            var retMsg = "";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbWorkspaceTeam SET IsDtd =@IsDeleted,UpdatedDate = @updatedDate,UpdatedBy=@userID where UserID =@ID and WorkspaceID=@WorkspaceID";
                    //_connection.Execute(query, new { IsActive = (userStatus == false ? true : false), updatedDate = DateTime.Now, userID = userId, ID = Id });
                    _connection.Execute(query, new { IsDeleted = true, updatedDate = DateTime.Now, userID = userId, ID = Id, WorkspaceID = WorkspaceID });

                    //Update  Project Tasks Status 
                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;

                }
                return retMsg;

                //User objUser = GetUserById(userId);
                //Code.Utils.Audit.Log(userId, "User " + objUser != null ? objUser.Name : "" + (isactive == false ? "activated" : "Deactivated"));

            }
        }

        /// <summary>
        /// Function used to change status active / not based on Id,WorkspaceID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <param name="userStatus"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public string ChangeActiveStatus(int Id, int userId, bool userStatus, int WorkspaceID)
        {
            var retMsg = "";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbWorkspaceTeam SET IsActive =@IsActive,UpdatedDate = @updatedDate,UpdatedBy=@userID where UserID =@ID and WorkspaceID=@WorkspaceID ";
                    _connection.Execute(query, new { IsActive = userStatus, updatedDate = DateTime.Now, userID = userId, ID = Id, WorkspaceID = WorkspaceID });

                    //Update  Project Tasks Status 
                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;
                }

            }
            return retMsg;
        }

        #endregion DeleteUser

        #region AssignUserRole
        /// <summary>
        /// Function used to update role in workspace team based on userId,userRoleID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRoleID"></param>        
        /// /// <returns></returns>
        public string AssignUserRole(int Id, int userId, int userRoleID, int WorkspaceID)
        {
            var retMsg = "";
            using (_connection = Utils.Database.GetDBConnection())
            {
                //string query = @"UPDATE TbUser SET RoleId =@RoleId,UpdatedDate = @updatedDate,UpdatedBy=@userID where ID =@ID";
                try
                {
                    //_connection.Execute(query, new { RoleId = userRoleID, updatedDate = DateTime.Now, userID = userId, ID = Id });

                    string query = @"UPDATE TbWorkspaceTeam SET RoleId =@RoleId,UpdatedDate = @updatedDate,UpdatedBy=@userID " +
                                    " where UserID =@ID and WorkspaceID =@WorkspaceID ";
                    _connection.Execute(query, new { RoleId = userRoleID, updatedDate = DateTime.Now, userID = userId, ID = Id, WorkspaceID = WorkspaceID });
                }

                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;

                }
                return retMsg;
                //User objUser = GetUserById(userId);
                //Code.Utils.Audit.Log(userId, "User " + objUser != null ? objUser.Name : "" + (isactive == false ? "activated" : "Deactivated"));
            }
        }
        #endregion AssignUserRole      

        /// <summary>
        /// Function used to add user assign group 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string AddUserToGroup(int userID, int groupID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            var InitiativeID = 0;
            var retMsg = "";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "SELECT EXISTS(SELECT ID from TbUserAssignedToGroup WHERE IsDeleted=0 and GroupId= @groupID and UserId =@userID );";
                    id = _connection.Query<int>(strQuery, new { groupID = groupID, userID = userID }).Single();

                    if (id == 0)
                    {
                        strQuery = "INSERT INTO TbUserAssignedToGroup(GroupId,UserId,CreatedDate,IsDeleted)" +
                                    " VALUES(@GroupId,@UserId,now(),0); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new { GroupId = groupID, UserId = userID }).Single();
                    }
                    else
                        retMsg = "The user is already assigned to same group";
                }

                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;

                }
                //strQuery = "SELECT InitiativeId from TbProject where Groupid =@groupID";
                //InitiativeID = _connection.Query<int>(strQuery, new { groupID = groupID }).Single();

                //if (InitiativeID > 0)
                //{
                //    InitiativeDAL objInitDAL = new InitiativeDAL();
                //    objInitDAL.AddUserToInitiative(InitiativeID, userID, loggedUserId);
                //}

            }
            return retMsg;
        }

        /// <summary>
        /// Function used to update login details based on email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="IsFailed"></param>
        /// <param name="Id"></param>
        public void UpdateLoginDetails(string email, bool IsFailed, int Id = 0)
        {
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (IsFailed)
                    {
                        _connection.Execute("UPDATE TbUser SET FailedLoginCount =FailedLoginCount+1,UpdatedDate= @updatedDate WHERE EmailId =@EmailId;", new { updatedDate = DateTime.Now, EmailId = email });
                    }
                    else
                    {
                        _connection.Execute("UPDATE TbUser SET NoOfLogins = NoOfLogins+1,UpdatedDate=@updatedDate WHERE ID = @ID;", new { updatedDate = DateTime.Now, ID = Id });
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Function used to invite user
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="userRole"></param>
        /// <param name="userID"></param>
        /// <param name="inviteFrom"></param>
        /// <param name="InitiativeId"></param>
        /// <returns></returns>
        public bool InviteUser(string EmailId, string userRole, int userID, string inviteFrom, int InitiativeId)
        {

            bool bStatus = false;
            string Action = string.Empty;
            EmailActivity objEmail = new EmailActivity();

            try
            {
                //Get ClientName by userID- logged userID
                string ClientName = "TestClient"; //GetClientNameById(userID);

                //bStatus = objEmail.SendInviteMail("Invite User", EmailId, userRole, ClientName, userID);

                UsersInfo objUsr = new UsersInfo();

                objUsr.EmailId = EmailId;
                objUsr.userRole = userRole;
                objUsr.ClientName = ClientName;
                Action = (inviteFrom == "" ? "Invite User" : (inviteFrom + " " + "Invite User"));
                bStatus = objEmail.SendMail(Action, objUsr, userID);
            }
            catch (Exception ex)
            {
            }
            return bStatus;

        }

        /// <summary>
        /// Function used to invite user
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="userRole"></param>
        /// <param name="userID"></param>
        /// <param name="inviteFrom"></param>
        /// <param name="InitiativeId"></param>
        /// <returns></returns>
        public bool InviteUserFromWorkspaceOrProject(string EmailId, string userRole, string loggedUserName, int loggedUserId, string password, string inviteFrom, int InitiativeId, int workspaceId = 0, int projectId = 0)
        {
            bool bStatus = false;
            string emailAction = string.Empty;
            EmailActivity objEmail = new EmailActivity();
            var inviteUserInfo = new InviteUserInfo();
            var inviteUserdetails = new InviteUserInfo();
            string inviterRole = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (projectId != 0)
                    {
                        emailAction = "InviteUserToProjectWithPassword";
                        //For getting the Invitee Users details to project like Password emailId role, workspace name, project name etc ,
                        string inviteeUserDetails = @"select wsname WorkspaceName, project.ProjectName from TbWorkspace ws " +
                            " inner join TbProject project on ws.WorkspaceID = project.WSID where WorkspaceID = @WorkspaceID and project.ProjectId = @ProjectId ";

                        inviteUserInfo = _connection.Query<InviteUserInfo>(inviteeUserDetails, new
                        {
                            WorkspaceID = workspaceId,
                            ProjectId = projectId
                        }).FirstOrDefault();

                        if (inviteUserInfo != null)
                        {
                            inviteUserInfo.InviteeUserEmail = EmailId;
                            inviteUserInfo.InviteeUserRole = userRole;
                            inviteUserInfo.InviteeUserPassword = password;

                            //For getting the user role info about the logged in user,
                            string query = @"select roles.RoleName InviterRole, usr.Name InviterName, usr.EmailId InviterEmail from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                                " inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID inner join TbRole roles on wsteam.RoleId = roles.RoleId " +
                                " where usr.Id = @Id and wsteam.WorkspaceID = @WorkspaceID";

                            inviteUserdetails = _connection.Query<InviteUserInfo>(query, new
                            {
                                Id = loggedUserId,
                                WorkspaceID = workspaceId
                            }).FirstOrDefault();

                            if (inviterRole != null)
                            {
                                inviteUserInfo.InviterRole = inviteUserdetails.InviterRole;
                                inviteUserInfo.InviterName = inviteUserdetails.InviterName;
                                inviteUserInfo.InviterEmail = inviteUserdetails.InviterEmail;
                            }
                        }
                    }
                    else
                    {
                        emailAction = "InviteUserToWorkspaceWithPassword";
                        //For getting the Invitee Users details to workspace like Password emailId role, workspace name etc ,
                        string inviteeUserDetails = @"select wsname WorkspaceName from TbWorkspace where WorkspaceID = @WorkspaceID";


                        inviteUserInfo = _connection.Query<InviteUserInfo>(inviteeUserDetails, new
                        {
                            WorkspaceID = workspaceId
                        }).FirstOrDefault();

                        if (inviteUserInfo != null)
                        {
                            inviteUserInfo.InviteeUserEmail = EmailId;
                            inviteUserInfo.InviteeUserRole = userRole;
                            inviteUserInfo.InviteeUserPassword = password;

                            //For getting the user role info about the logged in user,
                            string query = @"select roles.RoleName InviterRole, usr.Name InviterName, usr.EmailId InviterEmail from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                                " inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID inner join TbRole roles on wsteam.RoleId = roles.RoleId " +
                                " where usr.Id = @Id and wsteam.WorkspaceID = @WorkspaceID";

                            inviteUserdetails = _connection.Query<InviteUserInfo>(query, new
                            {
                                Id = loggedUserId,
                                WorkspaceID = workspaceId
                            }).FirstOrDefault();

                            if (inviterRole != null)
                            {
                                inviteUserInfo.InviterRole = inviteUserdetails.InviterRole;
                                inviteUserInfo.InviterName = inviteUserdetails.InviterName;
                                inviteUserInfo.InviterEmail = inviteUserdetails.InviterEmail;
                            }
                        }
                    }


                }

                bStatus = objEmail.SendMail(emailAction, inviteUserInfo, loggedUserId);

            }
            catch (Exception ex)
            {
            }
            return bStatus;

        }

        /// <summary>
        /// Function used to add invite user 
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="RoleID"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="WorkspaceID"></param>
        /// <param name="inviteFrom"></param>
        /// <param name="InitiativeId"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string AddInviteUser(string EmailId, int RoleID, int loggedUserId, int WorkspaceID, string inviteFrom, int InitiativeId, int ProjectID = 0, string password = "")
        {
            string strQuery = string.Empty; var retMsg = "";
            var id = 0; var UserId = 0; var idWSwsTeamId = 0; var isDeleted = 0;
            UsersDetails objProjectUser = new UsersDetails();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        var cmd = _connection.CreateCommand();
                        cmd.Transaction = transaction;
                        cmd.Connection = _connection;

                        string[] names = !string.IsNullOrEmpty(EmailId) ? EmailId.Split("@") : null;
                        string userName = string.Empty;

                        if (names != null && names.Length > 0)
                        {
                            userName = names[0];
                        }

                        MySqlParameter pEmail = new MySqlParameter("@EmailId", MySqlDbType.String);
                        MySqlParameter puserName = new MySqlParameter("@userName", MySqlDbType.String);
                        MySqlParameter pRoleID = new MySqlParameter("@RoleID", MySqlDbType.Int32);
                        MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                        MySqlParameter pWorkspaceID = new MySqlParameter("@WorkspaceID", MySqlDbType.Int32);
                        MySqlParameter pInitiativeId = new MySqlParameter("@InitiativeId", MySqlDbType.Int32);
                        MySqlParameter pProjectID = new MySqlParameter("@ProjectID", MySqlDbType.Int32);
                        MySqlParameter pUserId = new MySqlParameter("@UserId", MySqlDbType.Int32);

                        var encryptedPassword = Code.Utils.CryptoUtils.EncryptPassword(password);
                        MySqlParameter pEncryptedPassword = new MySqlParameter("@EncryptedPassword", MySqlDbType.String);

                        pEmail.Value = EmailId;
                        pRoleID.Value = RoleID;
                        ploggedUserId.Value = loggedUserId;
                        pWorkspaceID.Value = WorkspaceID;
                        pInitiativeId.Value = InitiativeId;
                        pProjectID.Value = ProjectID;
                        pEncryptedPassword.Value = encryptedPassword;
                        puserName.Value = userName;

                        cmd.Parameters.Add(pEmail);
                        cmd.Parameters.Add(pRoleID);
                        cmd.Parameters.Add(ploggedUserId);
                        cmd.Parameters.Add(pWorkspaceID);
                        cmd.Parameters.Add(pInitiativeId);
                        cmd.Parameters.Add(pProjectID);
                        cmd.Parameters.Add(pEncryptedPassword);
                        cmd.Parameters.Add(puserName);

                        try
                        {
                            // TbUser -Save 

                            //strQuery = "SELECT EXISTS(SELECT ID from TbUser WHERE EmailId= @EmailId);";
                            //id = _connection.Query<int>(strQuery, new { EmailId = EmailId }).Single();
                            strQuery = "SELECT ID,Isdeleted from TbUser WHERE EmailId= @EmailId;";
                            objProjectUser = _connection.Query<UsersDetails>(strQuery, new
                            {
                                EmailId = EmailId
                            }).FirstOrDefault();
                            if (objProjectUser != null)
                            {
                                id = objProjectUser.ID;
                            }

                            if (id == 0)
                            {
                                //byte[] data = Convert.FromBase64String(objSignup.Password);
                                //string decodedPassword = System.Text.Encoding.UTF8.GetString(data);



                                strQuery = "INSERT INTO TbUser( EmailId, Name, FirstName, IsRegisteredUser, CreatedDate,UpdatedDate,CreatedBy,UpdatedBy, Password, IsActive, RoleID)" +
                                           " VALUES(@EmailId, @userName, @userName, 1,now(),now(),@loggedUserId,@loggedUserId, @EncryptedPassword, 1, @RoleID); SELECT LAST_INSERT_ID();";
                                cmd.CommandText = strQuery;
                                UserId = Convert.ToInt32(cmd.ExecuteScalar()); //cmd.ExecuteScalar(strQuery, new { EmailId = EmailId, loggedUserId = loggedUserId }, commandType: CommandType.Text); 
                                pUserId.Value = UserId;
                                cmd.Parameters.Add(pUserId);
                            }
                            if (id != 0)
                            {
                                UserId = objProjectUser.ID;
                                pUserId.Value = UserId;
                                cmd.Parameters.Add(pUserId);
                                //strQuery = "SELECT Isdeleted from TbUser WHERE EmailId= @EmailId;";
                                //isDeleted = _connection.Query<int>(strQuery, new { EmailId = EmailId }).Single();
                                isDeleted = objProjectUser.IsDeleted;
                                if (isDeleted == 1)
                                {

                                    strQuery = "UPDATE TbUser SET Isdeleted = 0 , UpdatedDate =now()  ,UpdatedBy =@loggedUserId  Where  ID= @UserId";
                                    cmd.CommandText = strQuery;
                                    cmd.ExecuteNonQuery();

                                }
                            }
                            //pUserId.Value = UserId;
                            //cmd.Parameters.Add(pUserId);

                            // TbWorkspaceTeam -Save

                            strQuery = "SELECT EXISTS(SELECT ID from TbWorkspaceTeam WHERE UserID =@UserId and WorkspaceID =@WorkspaceID );";
                            id = _connection.Query<int>(strQuery, new { UserId = UserId, WorkspaceID = WorkspaceID }).Single();
                            if (id == 0)
                            {
                                strQuery = "INSERT INTO TbWorkspaceTeam( WorkspaceID,UserID,RoleID, CreatedDate,CreatedBy)" +
                                       " VALUES(@WorkspaceID, @UserId, @RoleID,now(),@loggedUserId); SELECT LAST_INSERT_ID();";
                                cmd.CommandText = strQuery;
                                idWSwsTeamId = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            //else
                            //    retMsg = "Already Exists in same workspace...";
                            if (id != 0)
                            {
                                //strQuery = "SELECT RoleID from TbWorkspaceTeam WHERE UserID =@UserId  and WorkspaceID =@WorkspaceID ";
                                //var roleID = _connection.Query<int>(strQuery, new { UserId = UserId, WorkspaceID = WorkspaceID }).Single();
                                //if (roleID != RoleID)
                                //{
                                //    strQuery = "UPDATE TbWorkspaceTeam SET RoleID =@RoleID, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserId  and WorkspaceID =@WorkspaceID ";

                                //    cmd.CommandText = strQuery;
                                //    cmd.ExecuteNonQuery();

                                //}

                                strQuery = "UPDATE TbWorkspaceTeam SET RoleID =@RoleID, UpdatedDate =now()  ,UpdatedBy =@loggedUserId , IsActive =1 , IsDtd = 0 Where  UserID =@UserId  and WorkspaceID =@WorkspaceID ";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();


                            }

                            // TbInitiativeToUsers --Save
                            if (inviteFrom == "Initiative" || inviteFrom == "Project")
                            {
                                strQuery = "SELECT EXISTS(SELECT UserID from TbInitiativeToUsers WHERE InitiativeId=@InitiativeId and UserID =@UserId );";
                                id = _connection.Query<int>(strQuery, new { UserId = UserId, InitiativeId = InitiativeId }).Single();
                                if (id == 0)
                                {
                                    strQuery = "INSERT INTO TbInitiativeToUsers( InitiativeId,UserID,CreatedDate,CreatedBy)" +
                                       " VALUES(@InitiativeId,@UserId ,now(),@loggedUserId ); SELECT LAST_INSERT_ID();";

                                    cmd.CommandText = strQuery;
                                    id = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                                else
                                    retMsg = "Already Exists in same initiative...";
                            }
                            // Users To ProjectTeam--Save 
                            if (inviteFrom == "Project")
                            {
                                strQuery = "SELECT GroupId from TbProject WHERE ProjectID=@ProjectID ";
                                id = _connection.Query<int>(strQuery, new { ProjectID = ProjectID }).Single();
                                retMsg = AddUserToGroup(@UserId, id, loggedUserId);
                            }

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("EmailId_UNIQUE"))
                                retMsg = "Duplicate Email Entry";
                            else
                            {
                                retMsg = "Error while saving data.";
                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (MySqlException ex)
                                {
                                    if (transaction.Connection != null)
                                    {
                                        retMsg = "An exception of type " + ex.GetType() +
                                        " was encountered while attempting to roll back the transaction.";
                                    }
                                    throw ex;
                                }
                                throw e;
                            }
                        }
                        finally
                        {
                            _connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        /// <summary>
        /// Function used to fetch login user based on userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public LoggedUserInfo GetLogUser(int userID)
        {
            LoggedUserInfo userdetails = new LoggedUserInfo();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT ID ,Name, FirstName,coalesce(LastName, '') as LastName , EmailId, Phone, ifnull(Organization,'') Organization,ifnull(ProfileImage,'') ProfileImageName FROM TbUser where ID =@userID ";
                    userdetails = _connection.Query<LoggedUserInfo>(strQuery, new { userID = userID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return userdetails;
        }

        /// <summary>
        /// Fumction used to update login user
        /// </summary>
        /// <param name="inputObj"></param>
        public void UpdateLogUser(LoggedUserInfo inputObj)
        {
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "UPDATE TbUser SET Name = CONCAT(@FirstName, ' ',coalesce(@LastName, '')),FirstName= @FirstName, LastName= @LastName,Phone=@Phone,Organization=@Organization,UpdatedDate=@updatedDate";

                    if (!string.IsNullOrEmpty(inputObj.ProfileImageName))
                    {
                        query += ",ProfileImage=@upProfileImage";
                    }

                    query += " WHERE ID = @ID;";

                    _connection.Execute(query,
                        new { updatedDate = DateTime.Now, ID = inputObj.ID, FirstName = inputObj.FirstName, LastName = inputObj.LastName, Phone = inputObj.Phone, Organization = inputObj.Organization, upProfileImage = inputObj.ProfileImageName });

                }
            }
            catch (Exception ex)
            {
            }
        }

        #region GetWorkspaceIdByGroupId
        /// <summary>
        ///GetWorkspaceIdByGroupId
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetWorkspaceIdByGroupId(int groupId)
        {
            int workspaceId = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select WorkspaceID from TbGroup where GroupId = @GroupId ";
                    workspaceId = _connection.Query<int>(strQuery, new { GroupId = groupId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return workspaceId;
        }
        #endregion GetWorkspaceIdByGroupId

        /// <summary>
        /// Function used to check password
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User OldPasswordMatch(int Id, string password)
        {
            var user = new User();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select ID,Name,EmailId,Phone,IsActive,NoOfLogins,FailedLoginCount,UserType,password FROM TbUser WHERE ID = @Id and Password = @Password";
                    user = _connection.Query<User>(query, new
                    {
                        ID = Id,
                        Password = password
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return user;
        }

        /// <summary>
        /// Function used to check old password is used / not
        /// </summary>
        /// <param name="newpassword"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsPasswordUsedBefore(string newpassword, int userId)
        {
            bool isExists = false;
            try
            {
                if (userId > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        int? cnt = _connection.Query<int?>(@"SELECT COUNT(UserID) FROM TbPasswordHistory WHERE UserID=@userId and Password = @password;", new { userId = userId, password = newpassword }).FirstOrDefault();

                        if (cnt > 0)
                        {
                            isExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isExists;
        }

        /// <summary>
        /// Function used to change passsword
        /// </summary>
        /// <param name="newpassword"></param>
        /// <param name="Id"></param>
        public void ChangePassword(string newpassword, int Id)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //  _connection.Execute("UPDATE TbUser SET password = @password,UpdatedDate=@updatedDate WHERE ID = @ID;", new { updatedDate = DateTime.Now, ID = Id, password = newpassword });

                        _connection.Execute("UPDATE TbUser SET password = @password,UpdatedDate=@updatedDate WHERE ID = @ID;INSERT INTO TbPasswordHistory(Password,UserID,CreatedDate)  VALUES(@Password,@userId,@createdDate);",
                            new { updatedDate = DateTime.Now, ID = Id, Password = newpassword, userId = Id, createdDate = DateTime.Now });
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Function used to fetch user access page list based on WorkspaceID,LoggedUserID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="LoggedUserID"></param>
        /// <returns></returns>
        public List<UserPageList> FetchUserAccessPagesList(int WorkspaceID, int LoggedUserID)
        {
            List<UserPageList> Pagelist = new List<UserPageList>();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"Select W.UserID, SP.PageName  From TbWorkspaceTeam W inner join TbRoleBasedAccessScreens RS on W.RoleID =RS.RoleId inner join TbScreenPageList SP on RS.ScreenId = SP.ScreenID where W.UserID = @UserID and W.WorkspaceID = @WorkspaceID";
                    Pagelist = _connection.Query<UserPageList>(query, new
                    {
                        WorkspaceID = WorkspaceID,
                        UserID = LoggedUserID
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {
            }

            return Pagelist;
        }
        #region ManageUserToken

        /// <summary>
        /// Function used to manage user token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="numberOfToken"></param>
        public void ManageUserToken(int userId, string token, int numberOfToken)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        int? isexist = _connection.Query<int?>(@"SELECT ID FROM TbNotificationToken WHERE UserId=@userId AND IFNULL(Token,'')= @token;", new { userId = userId, token = token }).FirstOrDefault();

                        if (isexist != null && isexist.Value > 0)
                        {
                            _connection.Execute("UPDATE TbNotificationToken SET CreatedDate = @CreatedDate WHERE ID = @ID;", new { ID = isexist.Value, CreatedDate = DateTime.Now });
                        }
                        else
                        {
                            List<int> lsttoken = _connection.Query<int>(@"SELECT ID FROM TbNotificationToken WHERE UserId=@userId Order by ID ASC", new { userId = userId }).ToList();

                            if (lsttoken != null && lsttoken.Count >= numberOfToken)
                            {
                                _connection.Execute("DELETE FROM TbNotificationToken WHERE ID = @ID;", new { ID = lsttoken[0] });
                            }

                            _connection.Execute("INSERT INTO TbNotificationToken(UserId,Token,CreatedDate) VALUES (@UserId,@Token,@CreatedDate);",
                              new
                              {
                                  UserId = userId,
                                  Token = token,
                                  CreatedDate = DateTime.Now
                              });
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion ManageUserToken

        public List<MyTask> GetShareLinkTaskFromServer(int SurveyID, int WorkspaceID, int UserID)
        {
            ListSyncItem objListSyncItem = new ListSyncItem();
            List<MyTask> lstMyTask_Survey = new List<MyTask>();
            string strQuery = string.Empty;
            strQuery = "sp_GetMyTaskList_ShareLink";
            using (_connection = Utils.Database.GetDBConnection())
            {
                //lstMyTask_Survey = _connection.Query<MyTask>(strQuery, new { loggedUserID = userId, SyncDbID = SyncServerID, varWorkspaceID = workspaceId }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                lstMyTask_Survey = _connection.Query<MyTask>(strQuery, new { loggedUserID = UserID, SyncDbID = 0, varWorkspaceID = WorkspaceID, varSurveyID = SurveyID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
            }
            objListSyncItem.lstMyTaskList = lstMyTask_Survey;
            return objListSyncItem.lstMyTaskList;
        }

        public bool GetShareLinkStatus(int UserID, int SurveyID)
        {
            bool isExists = false;
            try
            {
                if (UserID > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //int? cnt = _connection.Query<int?>(@"SELECT COUNT(UserID) FROM TbProjectSurveyUser WHERE UserID = @UserID AND SurveyID =@SurveyID;", new { SurveyID = SurveyID, UserID = UserID }).FirstOrDefault();
                        int? cnt = _connection.Query<int?>(@"SELECT COUNT(surveyuser.UserID)  FROM TbProjectSurveyUser surveyuser INNER JOIN TbProjectSurvey survey ON surveyuser.SurveyID = survey.SurveyID 
                                                            INNER JOIN TbProject project ON project.ProjectId = survey.ProjectId INNER JOIN TbWorkspace workspace ON project.WSID = workspace.WorkspaceID 
                                                            WHERE	workspace.IsDeleted = 0 AND	project.IsActive =1 AND	project.IsDeleted = 0 AND	project.EndDate >= CURDATE()  AND project.ProjectStatus ='Deploy'  
                                                            AND survey.IsActive =1 AND survey.EndDate >= CURDATE() 
                                                            AND surveyuser.UserID = @UserID AND surveyuser.SurveyID =@SurveyID;", new { SurveyID = SurveyID, UserID = UserID }).FirstOrDefault();

                        if (cnt > 0)
                        {
                            isExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isExists;
        }

        public string GetWorkflowLevelOneUsers(int SurveyID)
        {
            string strWorkflowLevelOneUsers = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strWorkflowLevelOneUsers = _connection.Query<string>(@"SELECT GROUP_CONCAT(users.Name) AS UsersNames FROM TbProjectSurvey survey INNER JOIN TbProjectWorkflow WF ON survey.WorkflowID = WF.WorkflowID 
                                                            INNER JOIN TbProjectWorkflowDetails WFLevel ON WFLevel.WorkflowID = WF.WorkflowID INNER JOIN TbUser users ON users.ID = WFLevel.UserID 
                                                            WHERE survey.SurveyID = @SurveyID AND WF.IsDeleted=0 AND WFLevel.IsDeleted = 0 AND WFLevel.WfLevel = 1
                                                            AND users.IsActive = 1 AND users.IsDeleted=0;", new { SurveyID = SurveyID }).FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
            }
            return strWorkflowLevelOneUsers;
        }

        #region Signup
        /// <summary>
        /// Function used to sign up new user
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userStatus"></param>
        /// 
        public bool Signup(string Name, string EmailId, string Password, string OTP)
        {
            bool status = false;
            string strQuery = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        int userid = 0;
                        strQuery = @" select ID " +
                                        " FROM TbUser WHERE EmailId = @EmailId ";
                        userid = _connection.Query<int>(strQuery, new
                        {
                            EmailId = EmailId
                        }).FirstOrDefault();

                        if (userid == 0)
                        {

                            strQuery = " INSERT INTO TbUser(Name,FirstName,EmailId,Password,CreatedDate,UpdatedDate)" +
                                            " VALUES(@Name,@Name,@EmailId,@Password,now(),now()); SELECT LAST_INSERT_ID(); "; //SELECT LAST_INSERT_ID();
                                                                                                                                                  //_connection.Execute(strQuery, new { FirstName = FirstName, LastName = LastName, EmailId = EmailId, Password = Password });

                            int newUserID = _connection.Query<int>(strQuery, new
                            {
                                Name = Name,
                                FirstName = Name,
                                //LastName = LastName,
                                EmailId = EmailId,
                                Password = Password
                            }).FirstOrDefault();

                            strQuery = " INSERT INTO TbSignupUserOTP(UserID,EmailId,OTP,CreatedDate)" +
                                            " VALUES(@NewUserID,@EmailId,@OTP,now()); ";
                            _connection.Execute(strQuery, new
                            {
                                NewUserID = newUserID,
                                EmailId = EmailId,
                                OTP = OTP
                            });

                        }
                        else
                        {
                            //resend
                            strQuery = "UPDATE TbUser SET UpdatedDate =now()  Where  EmailId= @EmailId; " +
                                " UPDATE TbSignupUserOTP SET IsExpired = 1, UpdatedDate =now()  Where  EmailId= @EmailId ;";
                            _connection.Execute(strQuery, new { EmailId = EmailId });

                            strQuery = " INSERT INTO TbSignupUserOTP(UserID,EmailId,OTP,CreatedDate)" +
                                            " VALUES(@NewUserID,@EmailId,@OTP,now()); ";
                            _connection.Execute(strQuery, new
                            {
                                NewUserID = userid,
                                EmailId = EmailId,
                                OTP = OTP
                            });
                        }

                        status = true;

                    }
                    _Tran.Complete();

                }

            }
            catch (Exception ex)
            {
                status = false;
                throw ex;

            }

            return status;
        }

        /// <summary>
        /// Function used to sign up new user
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userStatus"></param>
        /// 
        public string VerifySignupOTP(string EmailId, string OTP)
        {
            string statusMessage = string.Empty;
            string strQuery = string.Empty;
            List<SignupInfo> lstVerifyOTP = new List<SignupInfo>();
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        strQuery = @" select UserID,CreatedDate " +
                                        " FROM TbSignupUserOTP WHERE EmailId = @EmailId AND OTP = @OTP AND IsExpired = 0; "; // AND IsVerified = 0
                        lstVerifyOTP = _connection.Query<SignupInfo>(strQuery, new
                        {
                            EmailId = EmailId,
                            OTP = OTP
                        }).ToList();


                        if (lstVerifyOTP != null && lstVerifyOTP.Count > 0)
                        {
                            if (lstVerifyOTP.First().UserID > 0)
                            {
                                if ((DateTime.Now - Convert.ToDateTime(lstVerifyOTP.First().CreatedDate)).TotalMinutes < 5)
                                {
                                    strQuery = "UPDATE TbUser SET IsActive = 1, IsRegisteredUser =1, UpdatedDate =now()  Where  EmailId= @EmailId;"; // AND ID = 
                                    _connection.Execute(strQuery, new { EmailId = EmailId });
                                    //strQuery = "UPDATE TbSignupUserOTP SET IsVerified = 1, UpdatedDate =now()  Where  EmailId= @EmailId"; // AND ID = 
                                    //_connection.Execute(strQuery, new { EmailId = EmailId });
                                    statusMessage = "success";
                                }
                                else
                                {
                                    strQuery = "UPDATE TbSignupUserOTP SET IsExpired = 1, UpdatedDate =now()  Where  EmailId= @EmailId AND OTP = @OTP;"; // AND ID = 
                                    _connection.Execute(strQuery, new { EmailId = EmailId, OTP = OTP });
                                    statusMessage = "OTP_Timedout";
                                }
                            }
                        }
                        else
                        {
                            statusMessage = "invalid_OTP";
                        }

                    }
                    _Tran.Complete();

                }

            }
            catch (Exception ex)
            {
                statusMessage = "error";
                throw ex;

            }

            return statusMessage;
        }
        #endregion Signup

        #region SendEmailtoAssignedUsersFromWorkspace
        /// <summary>
        /// SendEmailtoAssignedUsersFromWorkspace
        /// </summary>
        /// <param name="objUserGroupList"></param>
        /// <param name="groupId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool SendEmailtoAssignedUsersFromWorkspace(string EmailId, string userName, int loggedUserId, int workspaceId, int projectId = 0)
        {
            bool bStatus = false;
            string emailAction = string.Empty;
            EmailActivity objEmail = new EmailActivity();
            var inviteUserInfo = new InviteUserInfo();
            var inviteUserdetails = new InviteUserInfo();
            string inviterRole = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (projectId != 0)
                    {
                        emailAction = "InviteUserToProjectWithOutPassword";

                        string inviteeUserDetails = @"select wsname WorkspaceName, project.ProjectName from TbWorkspace ws " +
                            " inner join TbProject project on ws.WorkspaceID = project.WSID where WorkspaceID = @WorkspaceID and project.ProjectId = @ProjectId ";

                        inviteUserInfo = _connection.Query<InviteUserInfo>(inviteeUserDetails, new
                        {
                            WorkspaceID = workspaceId,
                            ProjectId = projectId
                        }).FirstOrDefault();

                        if (inviteUserInfo != null)
                        {
                            inviteUserInfo.InviteeUserEmail = EmailId;
                            inviteUserInfo.InviteeUserName = userName;

                            string query = @"select roles.RoleName InviterRole, usr.Name InviterName, usr.EmailId InviterEmail from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                                " inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID inner join TbRole roles on wsteam.RoleId = roles.RoleId " +
                                " where usr.Id = @Id and wsteam.WorkspaceID = @WorkspaceID";

                            inviteUserdetails = _connection.Query<InviteUserInfo>(query, new
                            {
                                Id = loggedUserId,
                                WorkspaceID = workspaceId
                            }).FirstOrDefault();

                            if (inviterRole != null)
                            {
                                inviteUserInfo.InviterRole = inviteUserdetails.InviterRole;
                                inviteUserInfo.InviterName = inviteUserdetails.InviterName;
                                inviteUserInfo.InviterEmail = inviteUserdetails.InviterEmail;
                            }
                        }
                    }
                    else
                    {
                        emailAction = "InviteUserToWorkspaceWithOutPassword";

                        string inviteeUserDetails = @"select wsname WorkspaceName from TbWorkspace where WorkspaceID = @WorkspaceID";

                        inviteUserInfo = _connection.Query<InviteUserInfo>(inviteeUserDetails, new
                        {
                            WorkspaceID = workspaceId
                        }).FirstOrDefault();

                        if (inviteUserInfo != null)
                        {
                            inviteUserInfo.InviteeUserEmail = EmailId;
                            inviteUserInfo.InviteeUserName = userName;

                            string query = @"select roles.RoleName InviterRole, usr.Name InviterName, usr.EmailId InviterEmail from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                                " inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID inner join TbRole roles on wsteam.RoleId = roles.RoleId " +
                                " where usr.Id = @Id and wsteam.WorkspaceID = @WorkspaceID";

                            inviteUserdetails = _connection.Query<InviteUserInfo>(query, new
                            {
                                Id = loggedUserId,
                                WorkspaceID = workspaceId
                            }).FirstOrDefault();

                            if (inviterRole != null)
                            {
                                inviteUserInfo.InviterRole = inviteUserdetails.InviterRole;
                                inviteUserInfo.InviterName = inviteUserdetails.InviterName;
                                inviteUserInfo.InviterEmail = inviteUserdetails.InviterEmail;

                            }
                        }
                    }
                }

                bStatus = objEmail.SendMail(emailAction, inviteUserInfo, loggedUserId);

            }
            catch (Exception ex)
            {
            }
            return bStatus;
        }
        #endregion SendEmailtoAssignedUsersFromWorkspace

        #region GetUserName
        /// <summary>
        /// GetUserName
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetUserName(string emailId)
        {
            string userName = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select Name FROM TbUser WHERE EmailId = @EmailId and IsActive = 1";

                    userName = _connection.Query<string>(query, new
                    {
                        EmailId = emailId
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return userName;
        }

        #endregion GetUserName

        #region GetWSInfoOfInvitedUser
        /// <summary>
        ///GetWSInfoOfInvitedUser
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public WorkspaceInfo GetWSInfoOfInvitedUser(string emailId)
        {
            var objWorkspaceInfo = new WorkspaceInfo();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select ws.WorkspaceID, usr.Name, usr.EmailId from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                        " inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID where usr.EmailId = @EmailId; ";
                    objWorkspaceInfo = _connection.Query<WorkspaceInfo>(strQuery, new { EmailId = emailId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                objWorkspaceInfo = null;
            }
            return objWorkspaceInfo;
        }
        #endregion GetWSInfoOfInvitedUser   

        #region ForgotPassword
        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public string ForgotPassword(string EmailId)
        {
            string statusMsg = string.Empty;
            string strQuery = string.Empty;
            bool bStatus = false;
            string emailAction = string.Empty;
            EmailActivity objEmail = new EmailActivity();

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @" select usr.ID, usr.Name, usr.FirstName, usr.LastName, usr.EmailId, roles.RoleName Role, usr.IsActive, usr.IsDeleted " +
                                    " from TbWorkspaceTeam wsteam inner join TbUser usr on wsteam.UserID = usr.ID " +
                                    " left join TbRole roles on usr.RoleId = roles.RoleId inner join TbWorkspace ws on wsteam.WorkspaceID = ws.WorkspaceID " +
                                    " where usr.EmailId = @EmailId ";

                        var forgotPasswordInfo = _connection.Query<ForgotPasswordInfo>(strQuery, new
                        {
                            EmailId = EmailId
                        }).FirstOrDefault();

                        if (forgotPasswordInfo == null)
                        {
                            statusMsg = "No such user in the system";
                            return statusMsg;
                        }

                        if (forgotPasswordInfo != null && forgotPasswordInfo.IsActive != null && !forgotPasswordInfo.IsActive.Value)
                        {
                            statusMsg = "Inactive User";
                            return statusMsg;
                        }

                        if (forgotPasswordInfo != null && forgotPasswordInfo.IsDeleted != null && forgotPasswordInfo.IsDeleted.Value)
                        {
                            statusMsg = "Deleted User";
                            return statusMsg;
                        }

                        var encodedEmailId = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(EmailId));
                        forgotPasswordInfo.EncodedEmailId = encodedEmailId;

                        int reqId = _connection.Query<int>("INSERT INTO TbEmailAndPasswordRequests(UserID,Key1,CreatedDate)" +
                                   " VALUES(@UserID,@Key1,now()); SELECT LAST_INSERT_ID();", new { UserID = forgotPasswordInfo.ID, Key1 = encodedEmailId }).Single();
                        bStatus = objEmail.SendMail("ForgotPassword", forgotPasswordInfo, 0);

                        if (bStatus)
                        {
                            statusMsg = "success";
                        }
                        else
                            statusMsg = "failure";
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                statusMsg = "failure";
            }

            return statusMsg;
        }

        #endregion ForgotPassword

        #region UpdatePassword
        /// <summary>
        /// UpdatePassword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="encodedEmailId"></param>
        public string UpdatePassword(string encodedEmailId, string password)
        {
            string strQuery = string.Empty;
            string statusMsg = string.Empty;
            bool status = false;


            string emailId = Encoding.UTF8.GetString(Convert.FromBase64String(encodedEmailId));

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @" select usr.ID, usr.FirstName, usr.LastName, usr.EmailId, usr.IsActive, usr.IsDeleted " +
                                    " from TbUser usr where usr.EmailId = @EmailId ";

                        var forgotPasswordInfo = _connection.Query<ForgotPasswordInfo>(strQuery, new
                        {
                            EmailId = emailId
                        }).FirstOrDefault();

                        if (forgotPasswordInfo == null)
                        {
                            statusMsg = "User Does not exists";
                            return statusMsg;
                        }

                        if (forgotPasswordInfo != null && forgotPasswordInfo.ID == null)
                        {
                            statusMsg = "Invalid User";
                            return statusMsg;
                        }

                        if (forgotPasswordInfo != null && forgotPasswordInfo.IsActive != null && !forgotPasswordInfo.IsActive.Value)
                        {
                            statusMsg = "Inactive User";
                            return statusMsg;
                        }

                        if (forgotPasswordInfo != null && forgotPasswordInfo.IsDeleted != null && forgotPasswordInfo.IsDeleted.Value)
                        {
                            statusMsg = "Deleted User";
                            return statusMsg;
                        }

                        if (IsPasswordUsedBefore(password, forgotPasswordInfo.ID.Value))
                        {
                            statusMsg = "You have entered this Password before";
                            return statusMsg;
                        }

                        //Check whether the request from the user is valid or not
                        string reqExists = @"SELECT CreatedDate, ID RequestId FROM TbEmailAndPasswordRequests where UserID = @UserID and Key1 = @Key1 ;";

                        var requestedInfo = _connection.Query<ForgotPasswordInfo>(reqExists, new
                        {
                            UserID = forgotPasswordInfo.ID,
                            Key1 = encodedEmailId
                        }).FirstOrDefault();

                        DateTime now = DateTime.Now;

                        if (requestedInfo == null)
                        {
                            statusMsg = "Invalid Request";
                            return statusMsg;
                        }
                        else if (requestedInfo.CreatedDate < now.AddHours(-1) && requestedInfo.CreatedDate <= now)
                        {
                            statusMsg = "The Link is valid only for 1 hours and has expired. Please initiate the forgot password request once again.";
                            _connection.Execute("DELETE FROM TbEmailAndPasswordRequests where ID = @ID ;",
                                new { ID = requestedInfo.RequestId });
                        }

                        if (statusMsg == "")
                        {
                            _connection.Execute("UPDATE TbUser SET Password = @Password, UpdatedDate = @updatedDate WHERE ID = @userId; INSERT INTO TbPasswordHistory(Password,UserID,CreatedDate)  VALUES(@Password,@userId,@createdDate);",
                                new { updatedDate = DateTime.Now, Password = password, userId = forgotPasswordInfo.ID, createdDate = DateTime.Now });
                            _connection.Execute("DELETE FROM TbEmailAndPasswordRequests where ID = @ID ;",
                                new { ID = requestedInfo.RequestId });

                            statusMsg = "success";
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                statusMsg = "failure";
            }

            return statusMsg;
        }
        #endregion UpdatePassword
    }
}
