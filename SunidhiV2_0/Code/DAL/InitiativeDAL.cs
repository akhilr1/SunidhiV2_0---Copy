using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunidhiV2_0.Models;
using SunidhiV2_0.Controllers;
using System.Globalization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SunidhiV2_0.Code.Utils;
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Transactions;


namespace SunidhiV2_0.Code.DAL
{
    public class InitiativeDAL
    {
        protected IDbConnection _connection;

        #region CreateInitiative

        public string GetNextInitiativeCode()
        {
            int nextID = 0;
            string nextCode = "";
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT Max(InitiativeId) From TbInitiative ";
                nextID = _connection.ExecuteScalar<int>(strQuery);
                nextID++;
                var n = nextID.ToString().Length;
                string Code = n == 5 ? "IN" : (n == 4 ? "IN0" : (n == 3 ? "IN00" : (n == 2 ? "IN000" : (n == 1 ? "IN0000" : "IN00001"))));
                nextCode = Code + nextID.ToString();
            }
            return nextCode;
        }
        public IEnumerable<InitUserList> GetUsersList(int WorkspaceID, int InitID = 0)
        {
            List<InitUserList> lstUsers = new List<InitUserList>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                if (InitID == 0)
                {
                    strQuery = @"SELECT Distinct U.ID UserID, Name ,WT.RoleID  RoleID,RoleName UserRole FROM TbUser U" +
                                " Left JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID " +
                                " Where IsRegisteredUser =1 and IsDeleted =0 And U.IsActive =1 AND WT.WorkspaceID = @workspaceid Order by U.ID Desc ";
                    lstUsers = _connection.Query<InitUserList>(strQuery, new { workspaceid = WorkspaceID }).ToList();
                }
                else
                {
                    strQuery = @"SELECT Distinct U.ID UserID, Name ,WT.RoleID  RoleID,RoleName UserRole FROM TbUser U" +
                     " Left JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID " +
                     " Where IsRegisteredUser =1 and IsDeleted =0 And U.IsActive =1 AND WT.WorkspaceID = @workspaceid " +
                     " AND U.ID Not in (select E.UserID from TbInitiativeToUsers E where E.InitiativeId = @initid and E.IsDeleted = 0 ) Order by U.ID Desc";
                    lstUsers = _connection.Query<InitUserList>(strQuery, new { workspaceid = WorkspaceID, initid = InitID }).ToList();
                }
            }
            return lstUsers;
        }

        public IEnumerable<InitGroupList> GetInitUserGroups(int WorkspaceID, int InitID = 0)
        {
            List<InitGroupList> lstUserGroup = new List<InitGroupList>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                if (InitID == 0)
                {
                    strQuery = @"SELECT G.GroupId ,GroupName ,UG.GroupId,Count(userId) MemberCount from TbGroup G " +
                               " Left join TbUserAssignedToGroup UG On UG.GroupId = G.GroupId Where UG.IsDeleted =0 and G.IsDeleted =0 And IsActive =1 " +
                               " AND WorkspaceID = @workspaceid Group by G.GroupId ,GroupName Order by G.GroupId Desc";
                    lstUserGroup = _connection.Query<InitGroupList>(strQuery, new { workspaceid = WorkspaceID }).ToList();
                }
                else
                {
                    strQuery = @"SELECT G.GroupId ,GroupName ,UG.GroupId,Count(userId) MemberCount from TbGroup G " +
                              " Left join TbUserAssignedToGroup UG On UG.GroupId = G.GroupId Where UG.IsDeleted =0  and G.IsDeleted =0" +
                              " AND WorkspaceID = @workspaceid and G.GroupId Not in (select E.GroupID from TbInitiativeToGroups E where E.InitiativeId = @initid and E.IsDeleted = 0 )  " +
                              " Group by G.GroupId ,GroupName Order by G.GroupId Desc";
                    lstUserGroup = _connection.Query<InitGroupList>(strQuery, new { workspaceid = WorkspaceID, initid = InitID }).ToList();
                }
            }
               
            return lstUserGroup;
        }
        public IEnumerable<ProjectTemplate> GetProjectTheme()
        {
            List<ProjectTemplate> lstTheme = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT ThemeID,ThemeName Theme,0 IsSelected, 0 SelectedTemplCount,IFNULL(ThemeImage,'') AS TemplateImage FROM TbProjectTheme Where IsDeleted=0";
                lstTheme = _connection.Query<ProjectTemplate>(strQuery).ToList();
            }
            return lstTheme;
        }
        public IEnumerable<ProjectTemplate> GetProjectTemplateItems(int ThemeID)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct T.ThemeID ,T.ThemeName Theme ,PT.ProjectTemplateID ProjectTemplateID,"+
                            " ProjectTemplateName TemplateName,Description ,0 IsSelected,IFNULL(PT.TemplateImage,'') AS TemplateImage From TbProjectTemplate PT " +
                            "inner join TbProjectTheme T On T.ThemeID = PT.ThemeID Where PT.IsActive =1 And PT.ThemeID = @themeid";
                lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { themeid = ThemeID }).ToList();
            }
            return lstPrjTemp;
        }
        public IEnumerable<ProjectTemplate> GetProjectTemplateDetails(int TemplateID)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT PT.ProjectTemplateID,ProjectTemplateName TemplateName,Description, UseCase From TbProjectTemplate PT " +
                        " left join TbProjTemplateUsecases TU On TU.ProjectTemplateID = PT.ProjectTemplateID Where PT.IsActive =1 And PT.ProjectTemplateID=@templateid";
                lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { templateid = TemplateID }).ToList();
            }
            return lstPrjTemp;
        }

        public int CreateInitiative(string InitName, string Description, string Code, int WorkspaceID,string InitTags, string UserList, string GroupList, int loggedUserId)
        {

            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_CreateInitiative";
                    id = _connection.ExecuteScalar<int>(strQuery, new
                    {
                        InitName = InitName,
                        vDescription = Description,
                        vCode = Code,
                        vWorkspaceID = WorkspaceID,
                        InitTags = InitTags,
                        UserList = UserList,
                        GroupList = GroupList,
                        loggedUserId = loggedUserId
                    }, commandType: CommandType.StoredProcedure); 
                }

            }

            catch (Exception ex)
            {
                //retMsg = "Error while saving data." + ex.Message;
                id = 0;
                throw ex;
            }
            return id;
        }

        public string AddInitPrjTemplate(int InitiativeId, int ThemeID, int ProjectTemplateID, int userID)
        {
            var id = 0;
            string retMsg = "";
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "SELECT EXISTS(SELECT ID from TbInitiativeToProjectTemplates WHERE InitiativeId= @initid " + 
                               " and ThemeID = @themeid and ProjectTemplateID =@projtempid );";
                    id = _connection.Query<int>(strQuery, new { initid = InitiativeId, themeid = ThemeID, projtempid = ProjectTemplateID }).Single();
                    
                    if (id == 0)
                    {
                        strQuery = "INSERT INTO TbInitiativeToProjectTemplates( InitiativeId,ThemeID,ProjectTemplateID,CreatedDate,CreatedBy)" +
                           " VALUES(@initid,@themeid,@projtempid,now(),@loguserid); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new { initid = InitiativeId, themeid = ThemeID, projtempid = ProjectTemplateID, loguserid = userID }).Single();
                    }                    

                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;
                }
            }            
            return retMsg;         

        }
        public int AddInitWorkFlow(int InitiativeId,int WorkspaceID, string WfName, string WfDescription, int WfNoOfLevels,  int userID)
        {
            var id = 0;
            //string retMsg = "";
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = @"SELECT EXISTS(SELECT WorkflowID from TbInitiativeWorkflow WHERE InitiativeId= @initid 
                                and WorkspaceID = @workspaceid and WfName = @wfname );";
                    id = _connection.Query<int>(strQuery, new { initid = InitiativeId, workspaceid = WorkspaceID, wfname = WfName }).Single();
                    if (id == 0)
                    {
                        strQuery = "INSERT INTO TbInitiativeWorkflow( WfName,WfDescription,WfNoOfLevels,WorkspaceID,InitiativeId,CreatedDate,CreatedBy,IsDeleted)" +
                           " VALUES(@WfName,@WfDescription,@WfNoOfLevels,@WorkspaceID,@InitiativeId,now(),@loggedUserId,0); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new { WfName = WfName, WfDescription= WfDescription, WfNoOfLevels= WfNoOfLevels,
                                    WorkspaceID = WorkspaceID ,InitiativeId = InitiativeId, loggedUserId = userID }).Single();
                    }

                }
                catch (Exception ex)
                {
                    id = 0;
                    //retMsg = "Error while saving data";
                    throw ex;
                }
            }
            return id;

        }
        public string AddInitWorkFlowLevel(int WFID, int WFLevel, int userID, int UserID =0)
        {
            var id = 0;
            string retMsg = "";
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                   strQuery = "INSERT INTO TbInitiativeWorkflowLevelDetails( WorkflowID,WFLevel,UserID,CreatedDate,CreatedBy,IsDeleted)" +
                           " VALUES(@WorkflowID,@WFLevel,@UserID,now(),@loggedUserId,0); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new
                        {
                            WorkflowID = WFID,
                            WFLevel = WFLevel,
                            UserID = UserID,                                                  
                            loggedUserId = userID
                        }).Single();
                 
                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;
                }
            }
            return retMsg;

        }

        #endregion CreateInitiative

        //deploy and archive Initiative functions starts here 
        public IEnumerable<Initiatives> GetInitiativeDetails(int stage, int WorkspaceId)
        {
            List<Initiatives> lstInitiatives = new List<Initiatives>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT * FROM  TbInitiative where IsDeleted=0 and InitiativeStatus=@stg and WorkspaceId = @workspaceid Order by InitiativeId Desc";
                lstInitiatives = _connection.Query<Initiatives>(strQuery,new { stg = stage, workspaceid = WorkspaceId }).ToList();
            }
            return lstInitiatives;
        }

        public void DeleteInitiative(int InitiativeId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiative set IsDeleted = @IsDeleted where InitiativeId =@ID; ";
                _connection.Execute(query, new { ID = InitiativeId, IsDeleted = 1 });
            }
            return;
        }

        public void ChangeInitiativeStatus(int InitiativeId, int InitiativeStatus)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiative set InitiativeStatus = @InitiativeStatus where InitiativeId =@ID; ";
                _connection.Execute(query, new { ID = InitiativeId, InitiativeStatus = InitiativeStatus });
            }
            return;
        }

        public void ChangeStatus(int InitiativeId, int UserId, bool userStatus)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiative set IsActive = @IsActive ,UpdatedDate = @updatedDate,UpdatedBy=@userID where InitiativeId =@ID; ";
                _connection.Execute(query, new { ID = InitiativeId, IsActive = userStatus, updatedDate = DateTime.Now, userID = UserId });
            }
            return;
        }

        public List<Projects> GetProjectsDetails()
        {
            List<Projects> lstUser = new List<Projects>();
            string strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @" SELECT distinct  P.ProjectId ,ProjectName,P.ProjectStatus  as ProjectStatus, '' as Tasks,
                                    Case when IsActive = 1 then 'Active' else 'Inactive' end as State,IsActive 
                                    FROM TbProject P  where P.IsActive =1";
                lstUser = _connection.Query<Projects>(strQuery).ToList();
            }
            return lstUser;
        }
        //deploy and archive Initiative functions Ends here 
        //Manage Initiative functions starts here 
        public Initiatives GetInitiativeDetailsByID(int initID, int wsID)
        {
            Initiatives objInitiatives = new Initiatives();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT * FROM  TbInitiative where WorkspaceId=@workspaceid and InitiativeId=@initid";
                objInitiatives = _connection.Query<Initiatives>(strQuery, new { workspaceid = wsID, initid=initID}).FirstOrDefault();
            }
            return objInitiatives;
        }
        //Manageinitiative Details tags changes start here
        public IEnumerable<Tags> GetInitiativeTags(int InitiativeId)
        {
            List<Tags> InitiativeTags = new List<Tags>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT Distinct IT.TagID, TagName, TagColor From  TbInitiativeToTagList IT 
                           Inner join  TbInitiativeTags TG  On  TG.TagID =  IT.TagID Where InitiativeId= @initid and IT.isDeleted=0;";
                InitiativeTags = _connection.Query<Tags>(strQuery,new { initid = InitiativeId }).ToList();
            }
            return InitiativeTags;
        }

        public string AddNewInitiativeTag(string TagName, string TagColor, int loggedUserId)
        {
            string strQuery = string.Empty;
            var tagID = 0;
            var retMsg = "";

            using (_connection = Utils.Database.GetDBConnection())
            {
                // Saving New Tag 
                strQuery = @"INSERT INTO TbInitiativeToTagList(InitiativeId,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)
                                         VALUES(@InitiativeId,now(),@userID,now(),@userID); SELECT LAST_INSERT_ID();";
                try
                {
                    tagID = _connection.Query<int>(strQuery, new
                    {
                        TagName = TagName,
                        TagColor = TagColor,
                        loggedUserId = loggedUserId
                    }).Single();
                    retMsg = tagID.ToString();

                }
                catch (Exception ex)
                {
                    //retMsg = ex.Message;
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        retMsg = "Duplicate Entry";
                    }
                    else
                    {
                        retMsg = "Error while saving data.";
                        throw ex;
                    }
                }
            }
            return retMsg;
        }

        public void UpdateInitiativeDetails(Initiatives inputObj, int loggeduserid)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Update TbInitiative set InitiativeName=@initiativeName,
                                                     Description=@Description,
                                                     Code=@code,
                                                     UpdatedDate=@updatedDate,
                                                     UpdatedBy=@updatedBy where InitiativeId=@initid";
                _connection.Execute(strQuery, new
                {
                    initiativeName = inputObj.InitiativeName,
                    Description = inputObj.Description,
                    code = inputObj.Code,
                    updatedDate = DateTime.Now,
                    updatedBy = loggeduserid,
                    initid = inputObj.InitiativeId
                });

                //Updating InitiativeTag details Table
                //=== Get Previous Tags
                var retMsg = "";
                var initTags = new List<string>();

                if (inputObj.InitiativeTags == null)
                    inputObj.InitiativeTags = "";
                else
                {
                    string[] strTags = inputObj.InitiativeTags.Split(',');
                    initTags = new List<string>(strTags);
                }
                List<Tags> InitiativeTags = GetInitiativeTags(inputObj.InitiativeId).ToList();
                int newTagID = 0;
                int i = 0;
                if (inputObj.InitiativeTags != "")
                {
                    while (i < initTags.Count)
                    {
                        newTagID = Int32.Parse(initTags[i]);
                        if (InitiativeTags.Count != 0)
                        {
                            foreach (Tags oldTag in InitiativeTags)
                            {
                                if (newTagID == oldTag.TagID)
                                {
                                    InitiativeTags.Remove(oldTag);
                                    initTags.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                        i++;
                    }
                }

                for (i = 0; i < InitiativeTags.Count; i++)
                    retMsg = SaveTagDetails(inputObj.InitiativeId, InitiativeTags[i].TagID, loggeduserid, false);

                for (i = 0; i < initTags.Count; i++)
                    retMsg = SaveTagDetails(inputObj.InitiativeId, Int32.Parse(initTags[i]), loggeduserid, true);
            }
            return;
        }
        public string SaveTagDetails(int InitiativeId, int TagID, int loggedUserId, bool isNew)
        {
            string strQuery = string.Empty;
            var id = 0;
            var retMsg = "";

            using (_connection = Utils.Database.GetDBConnection())
            {
                // Updating WorkspaceTag details Table
                try
                {
                    if (isNew)
                    {
                        strQuery = @"SELECT EXISTS(SELECT ID from TbInitiativeToTagList WHERE InitiativeId=@initid and TagID=@tagid);";
                        id = _connection.Query<int>(strQuery,new { initid= InitiativeId , tagid= TagID }).Single();

                        if (id == 0)
                        {
                            strQuery = "INSERT INTO TbInitiativeToTagList(InitiativeId,TagID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)" +
                                        " VALUES(@InitiativeId,@TagID,now(),@userID,now(),@userID); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { InitiativeId = InitiativeId, TagID = TagID, userID = loggedUserId }).Single();
                        }
                        else
                        {
                            strQuery = "Update  TbInitiativeToTagList Set isDeleted =0  ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId " +
                                " Where InitiativeId =@initid and TagID =@tagid";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, initid = InitiativeId, tagid = TagID });

                        }
                    }
                    else
                    {
                        strQuery = "Update  TbInitiativeToTagList Set isDeleted =1  ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId " +
                            " Where InitiativeId =@initid and TagID =@tagid";
                        _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, initid = InitiativeId, tagid = TagID });
                    }
                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data.";
                    throw ex;
                }
                return retMsg;
            }
        }

        //Manageinitiative Details tags changes end here
        public List<UserGroups> GetGroupsListByIntitiativeID(int InitiativeId)
        {
            List<UserGroups> objUserGroupsList = new List<UserGroups>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"select A.GroupId,A.GroupName,A.Description,
                          CASE WHEN A.IsActive=0 then 'Inactive' ELSE  'Active' end as State,A.IsActive,A.ParentGroupId,
                          (select count(distinct C.UserId) from TbUserAssignedToGroup C where A.GroupId = C.GroupId and C.IsDeleted =0) UsersCount,
                          (select count(distinct D.ProjectId) from TbProject D where A.GroupId=D.GroupId) ProjectsCount,
                          (select count(distinct B.GroupId) from TbGroup B where A.GroupId=B.ParentGroupId) SubGroupsCount
                          FROM TbGroup A where A.IsDeleted=0 and A.GroupId in 
                            (select E.GroupID from TbInitiativeToGroups E where E.InitiativeId=@initid and E.IsDeleted=0) Order by A.GroupId Desc";
                objUserGroupsList = _connection.Query<UserGroups>(query,new { initid= InitiativeId }).ToList();
            }

            return objUserGroupsList;
        }
        public void AddGroupToInitiative(int InitiativeId, int GroupId, int UserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"INSERT INTO TbInitiativeToGroups ( InitiativeId, GroupID, IsDeleted, CreatedDate, CreatedBy) 
			                    VALUES (@initid,@grpid, 0, now(),@loggeduserid)";
                _connection.Execute(strQuery,new { initid= InitiativeId , grpid = GroupId , loggeduserid = UserId });
            }
            return;
        }
        public void DeleteGroupFromInitiative(int InitiativeId, int GroupId, int UserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"UPDATE TbInitiativeToGroups SET IsDeleted=1,UpdatedDate=now(),
                            UpdatedBy=@loggeduserid where GroupID=@grpid and InitiativeId=@initid";
                _connection.Execute(query, new { initid = InitiativeId, grpid = GroupId, loggeduserid = UserId });
            }
            return;
        }

        public List<UsersInfo> GetUsersListByIntitiativeID(int initiativeID, int workspaceID)
        {
            List<UsersInfo> objUserList = new List<UsersInfo>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {  
                query = @"SELECT Distinct IU.UserID ID, Name ,EmailId,Organization userOrganisation,Case when U.IsActive = 1 then 'Active' else 'Inactive' end as State, " +
                            " U.IsActive UserStatus, Case When grpCount > 1 then CONCAT(Cast(grpCount as char),' Groups')  else UG.GroupName end as userGroup  ," +
                            " WT.RoleID userRoleID, RoleName userRole,ProjectCount userProjectCount  , case when(WT.RoleID = 1)  THEN True else  False end  as UserRoleStatus " +
                            " FROM TbInitiativeToUsers IU inner join TbUser U on U.ID=IU.UserID Left Join TbWorkspaceTeam WT On WT.UserID = U.ID  left join TbRole R on R.RoleId = WT.RoleID " +
                            " left join(Select UserId, GROUP_CONCAT(Distinct GroupName) GroupName, Count(Distinct UAG.GroupId) grpCount From TbUserAssignedToGroup UAG  " +
                            " Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID Where IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and WorkspaceId = @wsid group by UserId) UG on UG.UserId = U.ID " +
                            " Left Join (Select UserId , count(distinct P.ProjectId) ProjectCount From TbUserAssignedToGroup UAG Inner join  TbGroup UG  On UG.GroupId = UAG.GroupID " +
                            " left join TbProject P  on P.GroupId = UAG.GroupId Where P.IsActive =1  and  UG.IsActive = 1 and UG.IsDeleted = 0  and UAG.IsDeleted = 0 and UG.WorkspaceId =@wsid  group by UserId) P On P.UserId =U.ID " +
                            " Where IsRegisteredUser = 1 and U.IsDeleted = 0 and IU.IsDeleted=0 AND  WT.WorkspaceID =@wsid and InitiativeId=@initid Order by IU.UserID Desc";
                objUserList = _connection.Query<UsersInfo>(query,new { wsid = workspaceID, initid=initiativeID}).ToList();
            }
            return objUserList;
        }
        public void DeleteUserFromInitiative(int InitiativeId, int UserID, int LoggedUserID)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"UPDATE TbInitiativeToUsers SET IsDeleted=1,UpdatedDate=now(),
                            UpdatedBy=@loggeduserid where UserID=@userid and InitiativeId=@initid";
                _connection.Execute(query, new { loggeduserid = LoggedUserID, userid= UserID, initid = InitiativeId });
            }

            return;
        }

        public void AddUserToInitiative(int InitiativeId, int UserID, int LoggedUserID)        {            var userinitiativeID = 0;            string strQuery = string.Empty;            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = "SELECT EXISTS(SELECT UserID from TbInitiativeToUsers WHERE IsDeleted=1 and InitiativeId=@initid and UserID =@userid);";
                userinitiativeID = _connection.Query<int>(strQuery, new { initid = InitiativeId, userid = UserID }).Single();

                if (userinitiativeID == 0)
                {                    strQuery = @"INSERT INTO TbInitiativeToUsers( InitiativeId, UserID, IsDeleted, CreatedDate, CreatedBy) 			                    VALUES (@initid ,@userid , 0, now(), @loggeduserid )";
                    _connection.Execute(strQuery, new { initid = InitiativeId, userid = UserID, loggeduserid= LoggedUserID });
                }
                else
                {
                    strQuery = @"UPDATE TbInitiativeToUsers SET IsDeleted=0,UpdatedDate=now(),                            UpdatedBy=@loggeduserid where UserID=@userid and InitiativeId=@initid";
                    _connection.Execute(strQuery, new { loggeduserid = LoggedUserID, userid = UserID, initid = InitiativeId });
                }            }            return;        }
        //Manage Initiative functions ends here
    }
}
