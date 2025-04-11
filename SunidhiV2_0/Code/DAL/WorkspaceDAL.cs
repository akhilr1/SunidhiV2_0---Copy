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

namespace SunidhiV2_0.Code.DAL
{
    public class WorkspaceDAL
    {
        protected IDbConnection _connection;

        /// <summary>
        /// UnArchieve workspace -> function used to undo temporary deleted workspace
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="WsIdList"></param>
        public void UnArchiveWorkspace(int loggedUserId, string WsIdList)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbWorkspace Set IsArchived=@IsArchived, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkspaceID,@wsidlist)";
                    _connection.Execute(query, new { IsArchived = 0, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsidlist = WsIdList });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Archieve workspace -> function used to temporary delete workspace
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="WsIdList"></param>
        public void ArchiveWorkspace(int loggedUserId, string WsIdList)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbWorkspace Set IsArchived=@IsArchived, UpdatedDate=@updatedDate,UpdatedBy=@loggedserid 
                              Where FIND_IN_SET(WorkspaceID,@wsidlist)";
                    _connection.Execute(query, new { IsArchived = 1, updatedDate = DateTime.Now, loggedserid = loggedUserId, wsidlist = WsIdList });
                }
            }
            catch (Exception ex)
            {
            }

            return;
        }

        /// <summary>
        /// Delete workspace -> function used to delete workspace & workspace tag details
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="WsIdList"></param>
        public void RemoveWorkspace(int loggedUserId, string WsIdList)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbWorkspace Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkspaceID,@wsidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsidlist = WsIdList });


                        query = @"Update TbWorkspaceTagDetails Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkspaceID,@wsidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsidlist = WsIdList });
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

            return;
        }

        /// <summary>
        /// Used to fetch worksapce details based on loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<Workspace> GetMyWorkspaceList(int loggedUserId)
        {
            List<Workspace> objWorkspaceList = new List<Workspace>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate,CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage  from TbWorkspace A 
                        //  where A.IsArchived=0 and A.IsDeleted=0 and A.WorkspaceID in (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //  @loggeduserid and B.CreatedBy=@loggeduserid )  Order By A.WorkspaceID Desc";

                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate,A.CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage  from TbWorkspace A 
                        //         where A.IsArchived=0 and A.IsDeleted=0 and exists (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //         @loggeduserid and B.CreatedBy=@loggeduserid and A.WorkspaceID = B.WorkspaceID)  Order By A.WorkspaceID Desc";

                        query = @"select distinct A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate,A.CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage  from TbWorkspace A
                                    inner join TbWorkspaceTeam B on B.UserID= @loggeduserid and B.CreatedBy=@loggeduserid and A.WorkspaceID = B.WorkspaceID and B.IsActive=1 and B.IsDtd=0 and A.IsArchived=0 and A.IsDeleted=0 Order By A.WorkspaceID Desc";

                        objWorkspaceList = _connection.Query<Workspace>(query, new { loggeduserid = loggedUserId }).ToList();

                        foreach (Workspace wsItem in objWorkspaceList)
                        {
                            wsItem.WSTags = new List<Tags>();
                            //query = @"select WSTagID TagID , TagName, TagColor from TbWorkspaceTags where WSTagID in 
                            //( select WSTagID from TbWorkspaceTagDetails where IsDeleted <>1 and WorkspaceID=@wsid)  Order By TagID Desc";

                            //query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t where exists (select w.WSTagID from TbWorkspaceTagDetails w where w.IsDeleted <>1 and w.WorkspaceID=@wsid
                            //            and w.WSTagID = t.WSTagID)  Order By t.WSTagID Desc";

                            query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t 
                                        inner join TbWorkspaceTagDetails w on w.IsDeleted <>1 and w.WorkspaceID=@wsid and w.WSTagID = t.WSTagID  Order By t.WSTagID Desc";

                            wsItem.WSTags = _connection.Query<Tags>(query, new { wsid = wsItem.WorkspaceID }).ToList();

                            wsItem.WSUsers = new List<User>();
                            //query = @"select ID, Name, EmailId, Phone,IsActive,IFNULL(ProfileImage,'') as  ProfileImage from TbUser where ID in 
                            //( select UserID from TbWorkspaceTeam where WorkspaceID=@wsid  and IsActive = 1) Order By ID Desc";

                            //query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive,IFNULL(t.ProfileImage,'') as  ProfileImage from TbUser t where 
                            //            exists (select w.UserID from TbWorkspaceTeam w where w.WorkspaceID=@wsid and w.UserID = t.ID) and t.IsActive = 1 Order By t.ID Desc";

                            query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive,IFNULL(t.ProfileImage,'') as  ProfileImage from TbUser t  
                                        inner join TbWorkspaceTeam w on w.WorkspaceID=@wsid and w.UserID = t.ID where t.IsActive = 1 and w.IsActive=1 and w.IsDtd=0 Order By t.ID Desc";

                            wsItem.WSUsers = _connection.Query<User>(query, new { wsid = wsItem.WorkspaceID }).ToList();
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return objWorkspaceList;
        }

        /// <summary>
        /// Used to fetch shared worksapce details based on loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<Workspace> GetSharedWorkspaceList(int loggedUserId)
        {
            List<Workspace> objSharedWorkspaceList = new List<Workspace>();

            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A 
                        //  where A.IsArchived=0 and A.IsDeleted=0 and A.WorkspaceID in (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //  @loggeduserid and B.CreatedBy!=@loggeduserid ) Order By  A.WorkspaceID Desc";

                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , A.CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A 
                        //         where A.IsArchived=0 and A.IsDeleted=0 and exists (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //         @loggeduserid and B.CreatedBy!=@loggeduserid and B.WorkspaceID = A.WorkspaceID) Order By  A.WorkspaceID Desc";

                        query = @"select distinct A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , A.CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A
                                 inner join TbWorkspaceTeam B on B.UserID= @loggeduserid and B.CreatedBy!=@loggeduserid and B.WorkspaceID = A.WorkspaceID
                                 where A.IsArchived=0 and A.IsDeleted=0 and B.IsActive=1 and B.IsDtd=0 Order By  A.WorkspaceID Desc ";

                        objSharedWorkspaceList = _connection.Query<Workspace>(query, new { loggeduserid = loggedUserId }).ToList();

                        foreach (Workspace wsItem in objSharedWorkspaceList)
                        {
                            wsItem.WSTags = new List<Tags>();
                            //query = @"select WSTagID TagID , TagName, TagColor from TbWorkspaceTags where WSTagID in 
                            //( select WSTagID from TbWorkspaceTagDetails where IsDeleted <>1 and  WorkspaceID=@wsid)  Order By TagID Desc";

                            //query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t where exists (select d.WSTagID from TbWorkspaceTagDetails d where d.IsDeleted <>1 and  d.WorkspaceID=@wsid
                            //           and t.WSTagID = d.WSTagID)  Order By TagID Desc";

                            query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t 
                                    inner join TbWorkspaceTagDetails d on d.IsDeleted <>1 and  d.WorkspaceID=@wsid and t.WSTagID = d.WSTagID Order By t.WSTagID Desc";

                            wsItem.WSTags = _connection.Query<Tags>(query, new { wsid = wsItem.WorkspaceID }).ToList();

                            wsItem.WSUsers = new List<User>();
                            //query = @"select ID, Name, EmailId, Phone,IsActive,ProfileImage from TbUser where ID in 
                            //( select UserID from TbWorkspaceTeam where WorkspaceID=@wsid and IsActive = 1) Order By ID Desc";

                            //query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive,t.ProfileImage from TbUser t where 
                            //           exists (select w.UserID from TbWorkspaceTeam w where w.WorkspaceID=@wsid and w.UserID = t.ID) and t.IsActive = 1 Order By t.ID Desc";

                            query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive,t.ProfileImage from TbUser t 
                                    inner join TbWorkspaceTeam w on w.WorkspaceID=@wsid and w.UserID = t.ID and t.IsActive = 1 and w.IsActive=1 and w.IsDtd=0 Order By t.ID Desc";

                            wsItem.WSUsers = _connection.Query<User>(query, new { wsid = wsItem.WorkspaceID }).ToList();
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

            return objSharedWorkspaceList;
        }

        /// <summary>
        /// Archieve workspace details -> function used to temporary deleted workspace details based on loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<Workspace> GetArchivedWorkspaceList(int loggedUserId)
        {
            List<Workspace> objArchivedWorkspaceList = new List<Workspace>();

            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A 
                        //  where A.IsArchived=1 and A.IsDeleted=0 and A.WorkspaceID in (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //  @loggeduserid and B.CreatedBy=@loggeduserid ) Order By A.WorkspaceID Desc";

                        //query = @"select A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A 
                        //         where A.IsArchived=1 and A.IsDeleted=0 and exists (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //         @loggeduserid and B.CreatedBy=@loggeduserid and B.WorkspaceID = A.WorkspaceID) Order By A.WorkspaceID Desc";

                        query = @"select distinct A.WorkspaceID,A.WSname,A.WSDescription,A.WSIsPrivate , A.CreatedBy,ifnull(A.WorkSpaceImage,'') WorkSpaceImage from TbWorkspace A 
                                    inner join TbWorkspaceTeam B on B.UserID= @loggeduserid and B.CreatedBy=@loggeduserid and B.WorkspaceID = A.WorkspaceID
                                     where A.IsArchived=1 and A.IsDeleted=0 Order By A.WorkspaceID Desc";

                        objArchivedWorkspaceList = _connection.Query<Workspace>(query, new { loggeduserid = loggedUserId }).ToList();

                        foreach (Workspace wsItem in objArchivedWorkspaceList)
                        {
                            wsItem.WSTags = new List<Tags>();
                            //query = @"select WSTagID TagID , TagName, TagColor from TbWorkspaceTags where WSTagID in 
                            //( select WSTagID from TbWorkspaceTagDetails where IsDeleted <>1 and WorkspaceID=@wsid) Order By TagID Desc";

                            //query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t where WSTagID in 
                            //( select WSTagID from TbWorkspaceTagDetails where IsDeleted <>1 and WorkspaceID=@wsid) Order By TagID Desc";

                            //query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t where exists 
                            //        (select w.WSTagID from TbWorkspaceTagDetails w where w.IsDeleted <>1 and w.WorkspaceID=@wsid and w.WSTagID = t.WSTagID) Order By t.WSTagID Desc";

                            query = @"select t.WSTagID TagID , t.TagName, t.TagColor from TbWorkspaceTags t inner join TbWorkspaceTagDetails w 
                                        where w.IsDeleted <>1 and w.WorkspaceID=@wsid and w.WSTagID = t.WSTagID  Order By t.WSTagID Desc";

                            wsItem.WSTags = _connection.Query<Tags>(query, new { wsid = wsItem.WorkspaceID }).ToList();

                            wsItem.WSUsers = new List<User>();
                            //query = @"select ID, Name, EmailId, Phone,IsActive from TbUser where IsActive=1 and IsRegisteredUser=1 and ID in 
                            //( select UserID from TbWorkspaceTeam where WorkspaceID=@wsid) Order By ID Desc";

                            //query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive from TbUser t where t.IsActive=1 and t.IsRegisteredUser=1 and 
                            //        exists (select w.UserID from TbWorkspaceTeam w where w.WorkspaceID=@wsid and w.UserID = t.ID) Order By t.ID Desc";

                            query = @"select t.ID, t.Name, t.EmailId, t.Phone,t.IsActive from TbUser t inner join TbWorkspaceTeam w on w.WorkspaceID=@wsid and w.UserID = t.ID 
                                        where t.IsActive=1 and t.IsRegisteredUser=1 Order By t.ID Desc";

                            wsItem.WSUsers = _connection.Query<User>(query, new { wsid = wsItem.WorkspaceID }).ToList();
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

            return objArchivedWorkspaceList;
        }

        /// <summary>
        /// Used to fetch all workspace details based on loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<Workspace> GetAllWorkspaceList(int loggedUserId)
        {
            List<Workspace> objWorkspaceList = new List<Workspace>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"SELECT WorkspaceID,WSName,WsDescription,WSIsPrivate FROM TbWorkspace where 
                    //        IsDeleted=0 and WorkspaceID in (select distinct WorkspaceID from TbWorkspaceTeam where UserID=@loggeduserid) Order By WorkspaceID Desc";

                    //query = @"SELECT t.WorkspaceID,t.WSName,t.WsDescription,t.WSIsPrivate FROM TbWorkspace t where t.IsDeleted=0 and 
                    //            exists (select distinct w.WorkspaceID from TbWorkspaceTeam w where w.UserID=@loggeduserid and w.WorkspaceID = t.WorkspaceID) Order By t.WorkspaceID Desc";

                    query = @"SELECT distinct t.WorkspaceID,t.WSName,t.WsDescription,t.WSIsPrivate FROM TbWorkspace t 
                                inner join TbWorkspaceTeam w on w.UserID=@loggeduserid and w.WorkspaceID = t.WorkspaceID where t.IsDeleted=0 Order By t.WorkspaceID Desc ";

                    objWorkspaceList = _connection.Query<Workspace>(query, new { loggeduserid = loggedUserId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return objWorkspaceList;
        }

        #region ManageWorkspace

        /// <summary>
        /// Used to fetch all workspace details based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<Workspace> GetWorkspaceDetails(int WorkspaceID)
        {
            List<Workspace> lstWS = new List<Workspace>();

            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct WorkspaceID,WSname,WSDescription,WSIsPrivate,ifnull(WorkSpaceImage,'') WorkSpaceImage from TbWorkspace " +
                                " Where  WorkspaceID =@wsid";
                    lstWS = _connection.Query<Workspace>(strQuery, new { wsid = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return lstWS;
        }

        /// <summary>
        /// Used to fetch all workspace tag details based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<Tags> GetWSTags(int WorkspaceID)
        {
            List<Tags> lstWSTags = new List<Tags>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct WT.WSTagID TagID, TagName, TagColor From  TbWorkspaceTagDetails WT 
                           Inner join  TbWorkspaceTags TG  On  TG.WSTagID =  WT.WSTagID Where WorkspaceID=@wsid and WT.isDeleted<>1 Order By WT.WSTagID Desc";
                    lstWSTags = _connection.Query<Tags>(strQuery, new { wsid = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return lstWSTags;
        }

        /// <summary>
        /// Update update workspace -> function used to update workspace & workspace tag details based on wsid
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="WSName"></param>
        /// <param name="WSDescription"></param>
        /// <param name="WSTags"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="fullimageurl"></param>
        /// <returns></returns>
        public string UpdateWorkspace(int WorkspaceID, string WSName, string WSDescription, string WSTags, int loggedUserId, string fullimageurl)
        {
            string strQuery = string.Empty;
            var retMsg = "";
            var wsTags = new List<string>();

            if (WSTags == null)
                WSTags = "";
            else
            {
                string[] strWsTags = WSTags.Split(',');
                wsTags = new List<string>(strWsTags);
            }
            using (var _Tran = new System.Transactions.TransactionScope())
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    try
                    {
                        //// Update To Workspace Table
                        strQuery = @"Update  TbWorkspace Set WSName =@WSName   ,WSDescription =@WSDescription ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid,WorkSpaceImage=@WorkSpaceImage 
                                    Where WorkspaceID =@wsid";
                        _connection.Execute(strQuery, new { WSName = WSName, WSDescription = WSDescription, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsid = WorkspaceID, WorkSpaceImage = fullimageurl });

                        // Updating WorkspaceTag details Table
                        //===Get Previous Tags
                        List<Tags> lstWSTags = GetWSTags(WorkspaceID).ToList();
                        int newTagID = 0;
                        int i = 0;
                        if (WSTags != "")
                        {
                            while (i < wsTags.Count)
                            {
                                newTagID = Int32.Parse(wsTags[i]);
                                if (lstWSTags.Count != 0)
                                {
                                    foreach (Tags oldTag in lstWSTags)
                                    {
                                        if (newTagID == oldTag.TagID)
                                        {
                                            lstWSTags.Remove(oldTag);
                                            wsTags.RemoveAt(i);
                                            i--;
                                            break;
                                        }
                                    }
                                }
                                i++;
                            }
                        }

                        for (i = 0; i < lstWSTags.Count; i++)
                            retMsg = SaveTagDetails(WorkspaceID, lstWSTags[i].TagID, loggedUserId, false);

                        for (i = 0; i < wsTags.Count; i++)
                            retMsg = SaveTagDetails(WorkspaceID, Int32.Parse(wsTags[i]), loggedUserId, true);
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while saving data.";
                        throw ex;
                    }
                }
                _Tran.Complete();
            }
           
            return retMsg;
        }

        /// <summary>
        /// Save Tag Details -> function used to save tag details
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="WSTagID"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public string SaveTagDetails(int WorkspaceID, int WSTagID, int loggedUserId, bool isNew)
        {
            string strQuery = string.Empty;
            var id = 0;
            var retMsg = "";
            using (var _Tran = new System.Transactions.TransactionScope())
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    // Updating WorkspaceTag details Table
                    try
                    {
                        if (isNew)
                        {
                            strQuery = @"SELECT EXISTS(SELECT ID from TbWorkspaceTagDetails WHERE WorkspaceID= @wsid and WSTagID =@wstagid);";
                            id = _connection.Query<int>(strQuery, new { wsid = WorkspaceID, wstagid = WSTagID }).Single();

                            if (id == 0)
                            {
                                strQuery = @"INSERT INTO TbWorkspaceTagDetails(WorkspaceID,WSTagID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)
                                            VALUES(@wsid,@wstagid,now(),@userID,now(),@userID); SELECT LAST_INSERT_ID();";
                                id = _connection.Query<int>(strQuery, new { wsid = WorkspaceID, wstagid = WSTagID, userID = loggedUserId }).Single();
                            }
                            else
                            {
                                strQuery = @"Update  TbWorkspaceTagDetails Set isDeleted =0  ,UpdatedDate = @updatedDate ,UpdatedBy=@loggeduserid 
                                             Where WorkspaceID=@wsid and WSTagID=@wstagid";
                                _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsid = WorkspaceID, wstagid = WSTagID });
                            }
                        }
                        else
                        {
                            strQuery = @"Update  TbWorkspaceTagDetails Set isDeleted =1  ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid 
                                            Where WorkspaceID=@wsid and WSTagID=@wstagid";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsid = WorkspaceID, wstagid = WSTagID });

                        }

                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while saving data.";
                        throw ex;

                    }
                    
                }

                _Tran.Complete();
            }
            return retMsg;

        }

        /// <summary>
        /// Used to update workspace visibility based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="WSIsPrivate"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveWSVisibility(int WorkspaceID, bool WSIsPrivate, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "";
            
            using (_connection = Utils.Database.GetDBConnection())
                {
                    try
                    {
                        strQuery = "Update  TbWorkspace Set WSIsPrivate =@wsprivate  ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid " +
                                 " Where WorkspaceID =@wsid ;";
                        _connection.Execute(strQuery, new { wsprivate = WSIsPrivate, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wsid = WorkspaceID });
                    }

                    catch (Exception ex)
                    {
                        retMsg = "Error while saving data.";
                        throw ex;

                    }
                    return retMsg;
                }
            
        }

        /// <summary>
        /// Used to delete workspace tag details based on TagID
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public bool DeleteWSTagDetails(int TagID)
        {
            string query = string.Empty;
            var id = 0;
            var returnVal = false;
            if (TagID > 0)
            {
                try
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"select Exists(select ID from TbWorkspaceTagDetails where WSTagID = @TagID and isDeleted = 0)";
                        id = _connection.Query<int>(query, new
                        {
                            TagID = TagID,
                        }).Single();

                        if (id == 0)
                        {
                            query = @"Delete from TbWorkspaceTags where WSTagID = @TagID";
                            _connection.Execute(query, new
                            {
                                TagID = TagID,
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                if (id > 0)
                {
                    returnVal = false;
                }
                else { returnVal = true; }
            }
            return returnVal;
        }

        #endregion ManageWorkspace

        #region Createworkspace 

        /// <summary>
        /// Used to fetch workspace tag details based on loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public IEnumerable<Tags> GetWSTagDetails(int loggedUserId)
        {
            List<Tags> lstWSTags = new List<Tags>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct WSTagID TagID ,TagName, TagColor FROM  TbWorkspaceTags where CreatedBy=@CreatedBy order by TagID desc";
                    lstWSTags = _connection.Query<Tags>(strQuery, new { CreatedBy = loggedUserId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return lstWSTags;
        }

        /// <summary>
        /// Add workspace tag details
        /// </summary>
        /// <param name="TagName"></param>
        /// <param name="TagColor"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string AddNewWsTag(string TagName, string TagColor, int loggedUserId)
        {
            string strQuery = string.Empty;
            var tagID = 0;
            var retMsg = "success";
            
           using (_connection = Utils.Database.GetDBConnection())
                {
                    // Saving New Tag 
                    strQuery = "INSERT INTO TbWorkspaceTags( TagName,TagColor,CreatedDate,CreatedBy)" +
                                " VALUES(@TagName,@TagColor,now(),@loggedUserId); SELECT LAST_INSERT_ID();";
                    try
                    {
                        tagID = _connection.Query<int>(strQuery, new
                        {
                            TagName = TagName,
                            TagColor = TagColor,
                            loggedUserId = loggedUserId
                        }).Single();

                    }
                    catch (Exception ex)
                    {
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

        /// <summary>
        /// Add workspace
        /// </summary>
        /// <param name="WSName"></param>
        /// <param name="WSDescription"></param>
        /// <param name="WSIsPrivate"></param>
        /// <param name="WSTags"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public string CreateWorkspace(string WSName, string WSDescription, bool WSIsPrivate, string WSTags, int loggedUserId,string fullFileName)
        {
            string strQuery = string.Empty;
            var retMsg = "";
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_CreateWorkspace";
                    id = _connection.ExecuteScalar<int>(strQuery, new
                    {
                        vWSName = WSName,
                        vWSDescription = WSDescription,
                        vWSIsPrivate = WSIsPrivate,
                        WSTags = WSTags,
                        loggedUserId = loggedUserId,
                        WorkSpaceImage = fullFileName
                    }, commandType: CommandType.StoredProcedure);

                    retMsg = "success-" + id.ToString();
                }
            }

            catch (Exception ex)
            {
                retMsg = "Error while saving data." + ex.Message;
                throw ex;
            }
            return retMsg;
        }

        #endregion Createworkspace

        /// <summary>
        /// Add workspace template & workspace team & workspace tag details
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<Workspace> CreateWorkspaceTemplate(int loggedUserId)
        {
            string strQuery = string.Empty;
            var idWS = 0;
            var tagID = 0;
            var grpid = 0;

            using (var _Tran = new System.Transactions.TransactionScope())
            {
                try
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //strQuery = @"select count(A.WorkspaceID) from TbWorkspace A where A.WorkspaceID in 
                        //    (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID=
                        //    @loggeduserid and B.CreatedBy=@loggeduserid )";

                        //strQuery = @"select count(A.WorkspaceID) from TbWorkspace A where exists 
                        //            (select distinct B.WorkspaceID from TbWorkspaceTeam B where B.UserID= @loggeduserid and B.CreatedBy=@loggeduserid
                        //             and B.WorkspaceID = A.WorkspaceID)";

                        strQuery = @"select count(distinct A.WorkspaceID) from TbWorkspace A 
                                    inner join TbWorkspaceTeam B on B.UserID= @loggeduserid and B.CreatedBy=@loggeduserid and B.WorkspaceID = A.WorkspaceID";

                        var count = _connection.Query<int>(strQuery, new { loggeduserid = loggedUserId }).Single();
                        if (count == 0)
                        {
                            //// Saving To Workspace Table
                            //strQuery = @"select RoleID from TbWorkspaceTeam where UserID=@loggeduserid LIMIT 1";
                            //var roleid = _connection.Query<int>(strQuery, new { loggeduserid = loggedUserId }).Single();

                            strQuery = "INSERT INTO TbWorkspace( WSName,WSDescription,WSIsPrivate,CreatedDate,CreatedBy)" +
                                        " VALUES(@WSName,@WSDescription,@WSIsPrivate,now(),@loggeduserid); SELECT LAST_INSERT_ID();";
                            idWS = _connection.Query<int>(strQuery, new
                            {
                                WSName = "Workspace Template",
                                WSDescription = "Description",
                                WSIsPrivate = 1,
                                loggeduserid = loggedUserId
                            }).Single();

                            // Saving To WorkspaceTeam Table
                            strQuery = @"INSERT INTO TbWorkspaceTeam( WorkspaceID,UserID,RoleID,CreatedDate,CreatedBy) 
                               VALUES(@WorkspaceID,@UserID,@RoleID,now(),@loggeduserid); SELECT LAST_INSERT_ID();";

                            _connection.Query<int>(strQuery, new { WorkspaceID = idWS, UserID = loggedUserId, RoleID = 1, loggeduserid = loggedUserId }).Single();

                            strQuery = @"SELECT TagID FROM  TbTags LIMIT 1";
                            tagID = _connection.Query<int>(strQuery).Single();
                            strQuery = "INSERT INTO TbWorkspaceTagDetails(WorkspaceID,WSTagID,CreatedDate,CreatedBy)" +
                                        " VALUES(@WorkspaceID,@WSTagID,now(),@userID); SELECT LAST_INSERT_ID();";
                            _connection.Query<int>(strQuery, new
                            {
                                WorkspaceID = idWS,
                                WSTagID = tagID,
                                userID = loggedUserId
                            }).Single();
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                _Tran.Complete();
            }
            return GetMyWorkspaceList(loggedUserId);
        }

    }
}
