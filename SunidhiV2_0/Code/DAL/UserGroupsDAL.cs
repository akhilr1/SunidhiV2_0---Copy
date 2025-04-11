using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
namespace SunidhiV2_0.Code.DAL
{
    public class UserGroupsDAL
    {
        protected IDbConnection _connection;

        /// <summary>
        /// User Group Details (WorkspaceID Basis)
        /// </summary>
        /// <param name="WsID"></param>
        /// <returns></returns>
        public List<UserGroups> GetUserGroupsList(int WsID)
        {
            List<UserGroups> objUserGroupsList = new List<UserGroups>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.GroupId,A.GroupName,ifnull(A.Description, '') as Description,
                    //      CASE WHEN A.IsActive=0 then 'Inactive' ELSE  'Active' end as State,A.IsActive,A.ParentGroupId,
                    //      (select count(distinct C.UserId) from TbUserAssignedToGroup C where C.IsDeleted=0 and A.GroupId = C.GroupId) UsersCount,
                    //      (select count(distinct D.ProjectId) from TbProject D where A.GroupId=D.GroupId) ProjectsCount,
                    //      (select count(distinct B.GroupId) from TbGroup B where A.GroupId=B.ParentGroupId) SubGroupsCount,ifnull(A.UserGroupImage,'') UserGroupImage
                    //      FROM TbGroup A where A.IsDeleted=0 and A.WorkspaceID=@wsid Order by A.GroupId Desc";


                    query = @"select A.GroupId,A.GroupName,ifnull(A.Description, '') as Description,CASE WHEN A.IsActive=0 then 'Inactive' ELSE  'Active' end as State,A.IsActive,A.ParentGroupId
                            ,ifnull(count(distinct C.UserId),0) UsersCount,ifnull(count(distinct D.ProjectId),0) ProjectsCount, ifnull(count(distinct B.GroupId),0) SubGroupsCount,ifnull(A.UserGroupImage,'') UserGroupImage           
                             FROM TbGroup A left outer join TbUserAssignedToGroup C on C.IsDeleted=0 and A.GroupId = C.GroupId left outer join TbProject D on A.GroupId=D.GroupId 
                              left outer join TbGroup B on A.GroupId=B.ParentGroupId where A.IsDeleted=0 and A.WorkspaceID=@wsid group by A.GroupId,A.GroupName,ifnull(A.Description, ''),
                             CASE WHEN A.IsActive=0 then 'Inactive' ELSE  'Active' end,A.IsActive,A.ParentGroupId Order by A.GroupId Desc";

                    objUserGroupsList = _connection.Query<UserGroups>(query, new { wsid = WsID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUserGroupsList;
        }
        
        /// <summary>
        /// User Details
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="wsId"></param>
        /// <returns></returns>
        public List<UsersList> GetAssignedUsersList(int groupId, int wsId)
        {
            List<UsersList> objUsersList = new List<UsersList>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"Select distinct A.ID,A.Name,C.RoleName UserRole, 0 as IsSelected ,ifnull(A.ProfileImage,'') as ProfileImage from TbUser A
                    //        inner join TbWorkspaceTeam B On A.ID = B.UserID
                    //        inner join TbRole C On C.RoleID = B.RoleID
                    //        Where B.WorkspaceID=@wsid 
                    //    and exists (Select distinct UserID From TbUserAssignedToGroup where IsDeleted = 0 and GroupId = @groupid and UserID = A.ID) AND A.IsActive = 1 and A.IsDeleted = 0 and A.IsRegisteredUser = 1 Order by A.ID Desc";                    

                    query = @"Select distinct A.ID,A.Name,C.RoleName UserRole, 0 as IsSelected ,ifnull(A.ProfileImage,'') as ProfileImage from TbUser A
                             inner join TbWorkspaceTeam B On A.ID = B.UserID inner join TbRole C On C.RoleID = B.RoleID
                             inner join TbUserAssignedToGroup G on G.IsDeleted = 0 and G.GroupId = @groupid and G.UserID = A.ID                  
                             Where B.WorkspaceID=@wsid AND A.IsActive = 1 and A.IsDeleted = 0 and A.IsRegisteredUser = 1 and B.IsDtd = 0 and B.IsActive = 1 Order by A.ID Desc;";

                    objUsersList = _connection.Query<UsersList>(query, new { groupid = groupId, wsid = wsId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return objUsersList;
        }

        /// <summary>
        /// Add New Group
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="parentid"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="wsid"></param>
        /// <param name="UserGroupImage"></param>
        /// <returns></returns>
        public int AddGroup(String name, String description, int parentid, int loggedUserId, int wsid,string UserGroupImage=null)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "INSERT INTO TbGroup(GroupName,Description,CreatedDate,CreatedBy,IsActive,ParentGroupId,WorkspaceID,UserGroupImage)";
                    query += " VALUES(@GroupName,@Description,now(),@CreatedBy,@IsActive,@ParentGroupId,@WorkspaceID,@UserGroupImage); SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(query, new
                    {
                        GroupName = name,
                        Description = description,
                        CreatedBy = loggedUserId,
                        IsActive = 1,
                        ParentGroupId = parentid,
                        WorkspaceID = wsid,
                        UserGroupImage = UserGroupImage
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// User Assigned to Group
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddUserToGroup(int userID, int groupID, int loggedUserId)
        {
            string query = string.Empty;
            var  InitiativeID = 0;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        id = GetUserAssignedToGroup(userID, groupID);
                        if (id == 0)
                        {
                            query = @"INSERT INTO TbUserAssignedToGroup(GroupId,UserId,CreatedDate,CreatedBy)
                            VALUES(@GroupId,@UserId,now(),@loggeduserid); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(query, new
                            {
                                GroupId = groupID,
                                UserId = userID,
                                loggeduserid = loggedUserId
                            }).Single();
                        }

                        // this Query is commented because of , in some condition the query return more than one data, that will cause an error 
                        // In our current system we dont have Initiative at now 
                        // In the  where condition groupId is not enough Project Id also needed 
                        // query = "SELECT ifnull(InitiativeId,0) InitiativeId from TbProject where Groupid =@groupID";
                        // So this section is not needed now
                        //InitiativeID = _connection.Query<int>(query, new { groupID = groupID }).SingleOrDefault();

                        //if (InitiativeID > 0)
                        //{
                       //     InitiativeDAL objInitDAL = new InitiativeDAL();
                       //     objInitDAL.AddUserToInitiative(InitiativeID, userID, loggedUserId);
                       // }


                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Get ID from UserAssignedGroup
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int GetUserAssignedToGroup(int userId, int groupId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select ID FROM TbUserAssignedToGroup where IsDeleted=0 and UserId = @uId and GroupId = @gId";
                    id = _connection.Query<int>(query, new { uId = userId, gId = groupId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Delete Group ie (Update IsDeleted field equal to 1)
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        public void DeleteGroup(int groupid,int userid)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbGroup SET IsDeleted =@IsDeleted,UpdatedDate = @updatedDate,UpdatedBy=@userID where GroupId =@ID";
                    _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, userID = userid, ID = groupid });

                }
            }
            catch (Exception ex)
            {
            }

            return;
        }

        /// <summary>
        /// Change Group State
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="activestate"></param>
        /// <param name="userid"></param>
        public void ChangeGroupState(int groupid, bool activestate, int userid)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbGroup SET IsActive=@IsActive,UpdatedDate = @updatedDate,UpdatedBy=@userID where GroupId =@ID";
                    _connection.Execute(query, new { IsActive = activestate, updatedDate = DateTime.Now, userID = userid, ID = groupid });

                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Get Group Details (GroupID Basis)
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public UserGroups GetGroupDetails(int groupId)
        {
            UserGroups objUserGroups = new UserGroups();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId, ";
                    //query += "(select count(C.GroupId) from TbUserAssignedToGroup C where C.IsDeleted=0 and A.GroupId = C.GroupId) UsersCount,";
                    //query += "(select count(B.ParentGroupId) from TbGroup B where A.GroupId=B.ParentGroupId) SubGroupsCount,ifnull(A.UserGroupImage,'') UserGroupImage ";
                    //query += "FROM TbGroup A where A.GroupId=@groupid Order by A.GroupId Desc ";

                    query = @"select A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId,ifnull(count(C.GroupId),0) UsersCount,                     
                            ifnull(count(B.ParentGroupId),0) SubGroupsCount,ifnull(A.UserGroupImage,'') UserGroupImage
                             FROM TbGroup A left outer join TbUserAssignedToGroup C on C.IsDeleted=0 and A.GroupId = C.GroupId
                             left outer join TbGroup B on A.GroupId=B.ParentGroupId where A.GroupId=@groupid group by A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId
                             Order by A.GroupId Desc";

                    objUserGroups = _connection.Query<UserGroups>(query, new { groupid = groupId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return objUserGroups;
        }

        /// <summary>
        /// Get SubGroup List
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<UserGroups> GetSubGroupsList(int groupId)
        {
            List<UserGroups> objSubGroupsList = new List<UserGroups>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"select A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId, ";
                    //query += "(select count(C.GroupId) from TbUserAssignedToGroup C where C.IsDeleted=0 and A.GroupId = C.GroupId) UsersCount,ifnull(A.UserGroupImage,'') UserGroupImage ";
                    //query += "FROM TbGroup A where A.IsDeleted = 0 and A.ParentGroupId=@groupid Order by A.GroupId Desc ";

                    query = @"select A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId,ifnull(count(C.GroupId),0) UsersCount,ifnull(A.UserGroupImage,'') UserGroupImage 
                             FROM TbGroup A left outer join TbUserAssignedToGroup C on C.IsDeleted=0 and A.GroupId = C.GroupId
                             where A.IsDeleted = 0 and A.ParentGroupId=@groupid group by A.GroupId,A.GroupName,A.Description,A.IsActive,A.ParentGroupId 
                             Order by A.GroupId Desc";

                    objSubGroupsList = _connection.Query<UserGroups>(query, new { groupid = groupId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objSubGroupsList;
        }

        /// <summary>
        /// Update Group Details
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="grouppname"></param>
        /// <param name="groupdescriptionint"></param>
        /// <param name="userid"></param>
        /// <param name="UserGroupImage"></param>
        public void UpdateGroup(int groupid, String grouppname, String groupdescriptionint, int userid,string UserGroupImage = null)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbGroup SET GroupName =@GroupName,Description =@Description,UpdatedDate = @updatedDate,UpdatedBy=@userID ";

                    if (!string.IsNullOrEmpty(UserGroupImage))
                    {
                        query += ",UserGroupImage =@UserGroupImage";
                    }
                    query += " where GroupId =@ID";

                    _connection.Execute(query, new { GroupName = grouppname, Description = groupdescriptionint, updatedDate = DateTime.Now, userID = userid, ID = groupid, UserGroupImage = UserGroupImage });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Delete Assigned User ie (Update IsDeleted field equal to 1)
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        /// <param name="loggeduserid"></param>
        public void DeleteAssignedUser(int groupid, int userid, int loggeduserid)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"update TbUserAssignedToGroup set IsDeleted=1, UpdatedDate=@updateddate, UpdatedBy=@updatedby where GroupId=@groupId and UserId=@userId";
                    _connection.Execute(query, new { userId = userid, groupId = groupid, updateddate = DateTime.Now, updatedby = loggeduserid });

                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<Projects> GetProjectsList(int groupId)
        {
            List<Projects> objProjectsList = new List<Projects>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select ProjectId,ProjectName,ProjectStatus  Status FROM TbProject A where GroupId=@grpid";
                    objProjectsList = _connection.Query<Projects>(query, new { grpid = groupId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objProjectsList;
        }

    }
}
