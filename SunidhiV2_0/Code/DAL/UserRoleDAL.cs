using Dapper;
using SunidhiV2_0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SunidhiV2_0.Code.DAL
{
    public class UserRoleDAL
    {
        protected IDbConnection _connection;

        /// <summary>
        /// Used to fetch User Roles
        /// </summary>
        /// <param name="WsID"></param>
        /// <returns></returns>
        public List<UserRoles> GetUserRolesList(int WsID)
        {
            List<UserRoles> objUserRolesList = new List<UserRoles>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"SELECT A.RoleId,A.RoleName,DATE_FORMAT((case when A.UpdatedDate is null then A.CreatedDate else A.UpdatedDate end) , "" %d/%m/%Y"") as UpdatedDate, ifnull(A.Description, '') as Description, 
                    //     (SELECT COUNT(B.id) FROM TbRoleBasedAccessScreens B where B.IsDeleted=0 and B.RoleId= A.RoleId) as NoOfScreens
                    //   FROM TbRole A where A.IsActive=1 and A.WorkspaceID=@wsid Order by A.RoleId desc";

                    query = @"SELECT A.RoleId,A.RoleName,DATE_FORMAT((case when A.UpdatedDate is null then A.CreatedDate else A.UpdatedDate end) , "" %d/%m/%Y"") as UpdatedDate
                            ,ifnull(A.Description, '') as Description, ifnull(COUNT(B.id),0) NoOfScreens FROM TbRole A left outer join TbRoleBasedAccessScreens B on B.IsDeleted=0 and B.RoleId= A.RoleId  
                             where A.IsActive=1 and A.WorkspaceID=@wsid group by A.RoleId,A.RoleName,ifnull(A.Description, ''),
                             DATE_FORMAT((case when A.UpdatedDate is null then A.CreatedDate else A.UpdatedDate end) , "" %d/%m/%Y"")
                             Order by A.RoleId desc";
                    objUserRolesList = _connection.Query<UserRoles>(query, new { wsid = WsID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return objUserRolesList;
        }

        /// <summary>
        /// Used to fetch Screen 
        /// </summary>
        /// <returns></returns>
        public List<Screens> GetAllScreenList()
        {
            List<Screens> objUserRolesList = new List<Screens>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"SELECT ScreenId,ScreenName,ScreenPath, Case When DefaultScreen =1 then 3 Else 1 End as AccessId ,DefaultScreen FROM TbScreen Order by ScreenId";
                    objUserRolesList = _connection.Query<Screens>(query).ToList();
                }

            }
            catch (Exception ex)
            {
            }
            // in query AccessId is Taken as 1 for default value
            return objUserRolesList;
        }

        /// <summary>
        /// Get RoleBased Screen List -> function used to fetch screens based on WsID,loggedUserId
        /// </summary>
        /// <param name="WsID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<RoleBasedScreen> GetRoleBasedScreenList(int WsID, int loggedUserId)
        {
            List<RoleBasedScreen> objScreen = new List<RoleBasedScreen>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"Select W.RoleID,RoleName,ScreenId,S.AccessId,ScreenName, AccessName From TbWorkspaceTeam W 
                    //    Inner Join TbRole R On R.RoleId = W.RoleID 
                    //    Left Join  (Select RS.ScreenId, RS.RoleId,RS.AccessId,ScreenName, AccessName  From TbRoleBasedAccessScreens RS 
                    //             Inner Join TbScreen S On S.ScreenId = RS.ScreenId Inner Join  TbAccessType A On A.AccessId = RS.AccessId where RS.IsDeleted=0) S On S.RoleId = R.RoleId
                    //    Where W.UserID =@UserID And W.WorkspaceID =@WorkspaceID ;";

                    query = @"Select W.RoleID,R.RoleName,S.ScreenId,S.AccessId,S.ScreenName, S.AccessName From TbWorkspaceTeam W 
                             Inner Join TbRole R On R.RoleId = W.RoleID Left Join  (Select RS.ScreenId, RS.RoleId,RS.AccessId,S.ScreenName, A.AccessName  From TbRoleBasedAccessScreens RS 
                             Inner Join TbScreen S On S.ScreenId = RS.ScreenId Inner Join  TbAccessType A On A.AccessId = RS.AccessId where RS.IsDeleted=0) S On S.RoleId = R.RoleId
                             Where W.UserID =@UserID And W.WorkspaceID =@WorkspaceID;";

                    objScreen = _connection.Query<RoleBasedScreen>(query, new { UserID = loggedUserId, WorkspaceID = WsID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objScreen;
        }

        /// <summary>
        /// Add New Role
        /// </summary>
        /// <param name="rolename"></param>
        /// <param name="roledescription"></param>
        /// <param name="wsid"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddRole(string rolename , string roledescription, int wsid, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"insert into TbRole(RoleName, Description,CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, UserType, IsActive, WorkspaceID)
                            values(@RoleName, @Description,@CreatedBy,  SYSDATE(),@UpdatedBy,  SYSDATE(),  1, @IsActive, @WorkspaceID); SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(query, new
                    {
                        RoleName = rolename,
                        Description = roledescription,
                        CreatedBy = loggedUserId,
                        UpdatedBy = loggedUserId,
                        IsActive = 1,
                        WorkspaceID = wsid
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Add Role Screen
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="ScreenId"></param>
        /// <param name="AccessId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddRoleScreens(int RoleId, int ScreenId, int AccessId, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    query = @"insert into TbRoleBasedAccessScreens
                            (RoleId,ScreenId,AccessId,CreatedBy,CreatedDate)
                        values(@RoleId,@ScreenId,@AccessId,@CreatedBy,SYSDATE()); SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(query, new
                    {
                        RoleId = RoleId,
                        ScreenId = ScreenId,
                        CreatedBy = loggedUserId,
                        AccessId = AccessId
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Get All AccessType 
        /// </summary>
        /// <returns></returns>         
        public List<AccessTypes> GetAllAceesType()
        {
            List<AccessTypes> objAccessType = new List<AccessTypes>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"SELECT AccessId,AccessName FROM TbAccessType Order By AccessId";
                    objAccessType = _connection.Query<AccessTypes>(query).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objAccessType;

        }

        /// <summary>
        /// Used to fetch Role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public UserRoles GetRoleDetails(int roleId)
        {
            UserRoles objUserRoles = new UserRoles();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"SELECT RoleId,RoleName,UpdatedDate,ifnull(Description, '') as Description,WorkspaceID  
                            FROM TbRole where RoleId=@roleid Order by RoleId Desc";
                    objUserRoles = _connection.Query<UserRoles>(query, new { roleid = roleId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return objUserRoles;
        }

        /// <summary>
        /// Get Rolewise User -> function used to fetch roles based on roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<UsersByRole> GetUsersByRole(int roleId)
        {
            List<UsersByRole> objUsersByRoleList = new List<UsersByRole>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.ID,A.Name, ";
                    //query += "(select count(B.ProjectId) from TbProject B) ProjectsCount ";
                    //query += "FROM TbUser A where A.RoleId=@roleid Order by  A.ID Desc";

                    query = @"select A.ID,A.Name,sum(case when ifnull(P.ProjectId,0) then 0 else 1 end) ProjectsCount from TbUserAssignedToGroup t 
                            inner join TbUser A on t.UserId = A.ID and A.IsActive = 1 and A.IsDeleted = 0 and A.RoleId= @roleid
                            left outer join TbProject P on P.GroupId = t.GroupId group by A.ID,A.Name order by A.ID Desc";

                    objUsersByRoleList = _connection.Query<UsersByRole>(query, new { roleid = roleId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUsersByRoleList;
        }

        /// <summary>
        /// Get Rolewise Screen -> function used to fetch screen based on roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Screens> GetScreensByRole(int roleId)
        {
            List<Screens> objScreensByRoleList = new List<Screens>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select A.ScreenId,A.ScreenName,A.ScreenPath,B.AccessId,C.AccessName from TbScreen A left join
                            TbRoleBasedAccessScreens B on B.ScreenId = A.ScreenId left join 
                            TbAccessType C on C.AccessId = B.AccessId where B.IsDeleted=0 and B.RoleId=@roleid Order by  A.ScreenId Desc";
                    objScreensByRoleList = _connection.Query<Screens>(query, new { roleid = roleId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objScreensByRoleList;
        }

        /// <summary>
        /// Delete User Role -> function used to delete role based on roleId,loggedUserId
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteUserRole(int RoleId, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"update TbRole set IsActive = 0 , UpdatedDate =now(), UpdatedBy=@loggeduserid where RoleId = @RoleId";
                    _connection.Execute(query, new { RoleId = RoleId, loggeduserid = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Get User to Assign Role -> function used to assign role for user based on WsID
        /// </summary>
        /// <param name="WsID"></param>
        /// <returns></returns>
        public List<UsersByRole> GetUsersToAssignRole(int WsID)
        {
            List<UsersByRole> objUsers = new List<UsersByRole>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.ID,A.Name,A.EmailID, 
                    //           (select count(*) from TbProject B) ProjectsCount 
                    //            FROM TbUser A where A.IsRegisteredUser=1 and A.IsActive=1 and A.IsDeleted=0 and A.ID in (select B.UserId from TbWorkspaceTeam B where B.WorkspaceId=@wsid and RoleID!=1)";
                    //query = @"select A.ID,A.Name,A.EmailID, ProjectsCount,ifnull(A.ProfileImage,'') as ProfileImage FROM TbUser A left join(Select UserId, count(distinct P.ProjectId) ProjectsCount From TbUserAssignedToGroup UAG Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID  left join TbProject P  on P.GroupId = UAG.GroupId Where P.IsActive = 1  and  UG.IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and UG.WorkspaceId = @WsID  group by UserId) P on P.UserId = A.ID where A.IsRegisteredUser = 1 and A.IsActive = 1 and A.IsDeleted = 0 and A.ID in (select B.UserId from TbWorkspaceTeam B where B.WorkspaceId = @WsID and RoleID!= 1)";

                    //query = @"select A.ID,A.Name,A.EmailID, P.ProjectsCount,ifnull(A.ProfileImage,'') as ProfileImage FROM TbUser A 
                    //         left join(Select UAG.UserId, count(distinct P.ProjectId) ProjectsCount From TbUserAssignedToGroup UAG 
                    //         Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID left join TbProject P  on P.GroupId = UAG.GroupId 
                    //         Where P.IsActive = 1  and  UG.IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and UG.WorkspaceId = @wsid  group by UAG.UserId) P on P.UserId = A.ID where A.IsRegisteredUser = 1 
                    //         and A.IsActive = 1 and A.IsDeleted = 0 and exists (select B.UserId from TbWorkspaceTeam B where B.WorkspaceId = @wsid and B.RoleID!= 1 and B.UserId = A.ID)";

                    query = @"select A.ID,A.Name,A.EmailID, P.ProjectsCount,ifnull(A.ProfileImage,'') as ProfileImage FROM TbUser A 
                             inner join TbWorkspaceTeam B on B.WorkspaceId = @wsid and B.RoleID!= 1 and B.UserId = A.ID  
                             left join(Select UAG.UserId, count(distinct P.ProjectId) ProjectsCount From TbUserAssignedToGroup UAG 
                             Inner join TbGroup UG  On UG.GroupId = UAG.GroupID left join TbProject P  on P.GroupId = UAG.GroupId 
                             Where P.IsActive = 1  and  UG.IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and UG.WorkspaceId = @wsid  group by UAG.UserId) P on P.UserId = A.ID 
                             where A.IsRegisteredUser = 1 and A.IsActive = 1 and A.IsDeleted = 0 and B.IsDtd = 0 and B.IsActive = 1";

                    objUsers = _connection.Query<UsersByRole>(query, new { wsid = WsID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUsers;
        }

        /// <summary>
        /// Get User without Screen -> function used to User without using scrren based on roleId
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public List<UsersByscreens> GetUserWithoutscreens(int RoleId)
        {
            List<UsersByscreens> objUsersWithoutScreens = new List<UsersByscreens>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                   // query = @"select A.ScreenId,A.ScreenName, A.ScreenPath, 1 as AccessId 
                   //     FROM TbScreen A where A.ScreenId not in 
                   //     (select ScreenId from TbRoleBasedAccessScreens where IsDeleted=0 and RoleId=@roleid)";

                    query = @"select A.ScreenId,A.ScreenName, A.ScreenPath, 1 as AccessId 
                             FROM TbScreen A where not exists (select s.ScreenId from TbRoleBasedAccessScreens s where s.IsDeleted=0 and s.RoleId=@roleid 
                             and s.ScreenId = A.ScreenId)";

                    // query = @"select A.ScreenId,A.ScreenName, A.ScreenPath, 1 as AccessId 
                    //FROM TbScreen A left outer join TbRoleBasedAccessScreens s  on s.ScreenId = A.ScreenId and s.RoleId=@roleid and s.IsDeleted=0 where s.ScreenId IS NULL";

                    objUsersWithoutScreens = _connection.Query<UsersByscreens>(query, new { roleid = RoleId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUsersWithoutScreens;
        }

        /// <summary>
        /// Assign Users to Role -> function used to assUser using role based on roleId,UserId,WsID
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserId"></param>
        /// <param name="WsID"></param>
        public void AssignUsersToRole(int RoleId, int UserId, int WsID)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbWorkspaceTeam set RoleID= @RoleID  where UserID = @userid and WorkspaceID=@wsid";
                    _connection.Execute(query, new { RoleID = RoleId, wsid = WsID, userid = UserId });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }


        //public List<UserRoleDetails> GetUserRoleDetailsBasedOnRoleId(int RoleId)
        //{
        //    List<UserRoleDetails> objUserRoleDetails = new List<UserRoleDetails>();
        //    string query = string.Empty;

        //    try
        //    {
        //        using (_connection = Utils.Database.GetDBConnection())
        //        {
        //            query = @"SELECT RoleId,RoleName,Description FROM TbRole where RoleId=@roleid Order By RoleId Desc";
        //            objUserRoleDetails = _connection.Query<UserRoleDetails>(query, new { roleid = RoleId }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return objUserRoleDetails;
        //}

        /// <summary>
        /// Update Role -> function used to update role based on roleId
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="rolename"></param>
        /// <param name="roledescription"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateRole(int roleid, string rolename, string roledescription,int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"update TbRole set 
                        RoleName=@RoleName,Description=@Description,UpdatedDate=now(),UpdatedBy=@UpdatedBy where RoleId=@RoleId;";
                    _connection.Execute(query, new
                    {
                        RoleId = roleid,
                        RoleName = rolename,
                        UpdatedBy = loggedUserId,
                        Description = roledescription,
                    });

                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Get RoleBased User List -> function used to fetch role with user based on roleId,WsID
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="WsID"></param>
        /// <returns></returns>
        public List<UsersByRole> GetRoleBasedUsersList(int RoleId, int WsID)
        {
            List<UsersByRole> objRoleBasedUsers = new List<UsersByRole>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.ID,A.Name,A.EmailID,case when A.IsActive=1 then 'Active' else 'Inactive' end as Status, 0 as Tasks,
                    //        @roleid as RoleId, (select count(*) from TbProject B) ProjectsCount,IFNULL(A.ProfileImage,'') as ProfileImage FROM TbUser A where 
                    //        A.ID in (select B.UserId from TbWorkspaceTeam B where B.WorkspaceId=@wsid and RoleID=@roleid and A.IsActive =1) Order By A.ID Desc";

                    //query = @"select A.ID,A.Name,A.EmailID,case when A.IsActive=1 then 'Active' else 'Inactive' end as Status, 0 as Tasks,
                    //        @roleid as RoleId,sum(case when ifnull(P.ProjectId,0) then 0 else 1 end) ProjectsCount,IFNULL(A.ProfileImage,'') as ProfileImage from TbUserAssignedToGroup t 
                    //        inner join TbUser A on t.UserId = A.ID and A.IsActive = 1 and A.IsDeleted = 0 and 
                    //        exists (select B.UserId from TbWorkspaceTeam B where B.WorkspaceId=@wsid and RoleID=@roleid and A.IsActive =1 and B.UserId = A.ID) 
                    //        left outer join TbProject P on P.GroupId = t.GroupId group by A.ID,A.Name,A.EmailID
                    //         Order By A.ID Desc;";

                    query = @"select A.ID,A.Name,A.EmailID,case when A.IsActive=1 then 'Active' else 'Inactive' end as Status, 0 as Tasks,
                            @roleid as RoleId,sum(case when ifnull(P.ProjectId,0) then 0 else 1 end) ProjectsCount,IFNULL(A.ProfileImage,'') as ProfileImage from TbUserAssignedToGroup t 
                            inner join TbUser A on t.UserId = A.ID and A.IsActive = 1 and A.IsDeleted = 0 
                            inner join TbWorkspaceTeam B on B.WorkspaceId=@wsid and B.RoleID=@roleid and A.IsActive =1 and B.UserId = A.ID
                            left outer join TbProject P on P.GroupId = t.GroupId group by A.ID,A.Name,A.EmailID
                             Order By A.ID Desc;";

                    objRoleBasedUsers = _connection.Query<UsersByRole>(query, new { roleid = RoleId, wsid = WsID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objRoleBasedUsers;
        }

        /// <summary>
        /// Update User Role -> function used to update roleid,userid based on WsID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="AssignedRoleId"></param>
        /// <param name="WsID"></param>
        public void UpdateUserRole(int UserID, int AssignedRoleId, int WsID)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbWorkspaceTeam set RoleID=@RoleId where UserID=@userid and WorkspaceID=@wsid";
                    _connection.Execute(query, new { RoleId = AssignedRoleId, wsid = WsID, userid = UserID });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Delete Screen (Roleid Basis)--(function used to delete screen based on screenid,roleid)
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="screenid"></param>
        /// <param name="loggeduserid"></param>
        public void DeleteScreenFromRole(int roleid, int screenid, int loggeduserid)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"update TbRoleBasedAccessScreens set IsDeleted=1, UpdatedDate=@updateddate, UpdatedBy=@updatedby where ScreenId=@screenId and RoleId=@roleId";
                    _connection.Execute(query, new { roleId = roleid, screenId = screenid, updateddate = DateTime.Now, updatedby = loggeduserid });

                }
            }
            catch (Exception ex)
            {
            }
            return;
        }
    }
}
