using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
using Newtonsoft.Json;


namespace SunidhiV2_0.Code.DAL
{
    public class WorkflowDAL
    {
        protected IDbConnection _connection;

        #region InitiativeWorkflow

        /// <summary>
        /// Get Initiative Workflow List -> -> function used to fetch initiative workflow details based on InitiaveId,WorkspaceID
        /// </summary>
        /// <param name="InitiaveId"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<Workflow> GetWorkflowsListByIntitiativeID(int InitiaveId, int WorkspaceID)
        {
            List<Workflow> objWorkflowsList = new List<Workflow>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Select WorkflowID ,IFNULL(WfName,'') as WfName  ,IFNULL(WfDescription,'') as WfDescription ,WfNoOfLevels  From TbInitiativeWorkflow 
                            Where WorkspaceID =@wsid and  InitiativeId=@initid and IsDeleted=0 ";
                        objWorkflowsList = _connection.Query<Workflow>(query, new { initid = InitiaveId, wsid = WorkspaceID }).ToList();

                        foreach (Workflow wfItem in objWorkflowsList)
                        {
                            wfItem.WfLevelDetails = new List<WorkflowLevelDetails>();
                            for (int eachLevel = 1; eachLevel <= wfItem.WfNoOfLevels; eachLevel++)
                            {
                                var tempitem = new WorkflowLevelDetails();
                                tempitem.WfLevel = eachLevel;
                                //query = @"select ID, Name, EmailId from TbUser where ID in 
                                //    (select UserID from TbInitiativeWorkflowLevelDetails where WorkflowID=@workflowid and IsDeleted=0 and WfLevel=@eachlevel)";

                                //query = @"select T.ID, T.Name, T.EmailId from TbUser T where exists (select W.UserID from TbInitiativeWorkflowLevelDetails W 
                                //            where W.WorkflowID=@workflowid and W.IsDeleted=0 and W.WfLevel=@eachlevel and T.ID = W.UserID)";

                                query = @"select T.ID, T.Name, T.EmailId from TbUser T inner join TbInitiativeWorkflowLevelDetails W 
                                            on W.WorkflowID=@workflowid and W.IsDeleted=0 and W.WfLevel=@eachlevel and T.ID = W.UserID";

                                tempitem.usersList = _connection.Query<User>(query, new { workflowid = wfItem.WorkflowID, eachlevel = eachLevel }).ToList();

                                wfItem.WfLevelDetails.Add(tempitem);
                            }
                        }
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return objWorkflowsList;
        }

        /// <summary>
        /// Update WorkFlow -> function used to update workflow details based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfName"></param>
        /// <param name="wfDesc"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateWorkflowDetails(int wfID, string wfName, string wfDesc, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbInitiativeWorkflow SET WfName=@WfName,WfDescription=@WfDescription,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                    _connection.Execute(query, new { WfName = wfName, WfDescription = wfDesc, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Update WorkFlow Level -> function used to update workflow level based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfNoOfLevels"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateWorkflowLevelCount(int wfID, int wfNoOfLevels, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbInitiativeWorkflow SET WfNoOfLevels=@WfNoOfLevels,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                    _connection.Execute(query, new { WfNoOfLevels = wfNoOfLevels, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Delete WorkFlow -> function used to delete workflow based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteWorkflowLevel(int wfID, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbInitiativeWorkflowLevelDetails SET IsDeleted=@on,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                    _connection.Execute(query, new { on = 1, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Add New WorkFlow Level
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfLevel"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateWorkflowLevelDetails(int wfID, int wfLevel, int userID, int groupID, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "INSERT INTO TbInitiativeWorkflowLevelDetails( WorkflowID,WFLevel,UserID,CreatedDate,CreatedBy,IsDeleted)" +
                                " VALUES(@WorkflowID,@WFLevel,@UserID,now(),@logUserId,0); SELECT LAST_INSERT_ID();";
                    _connection.Query(query, new
                    {
                        WorkflowID = wfID,
                        WFLevel = wfLevel,
                        UserID = userID,
                        logUserId = loggedUserId
                    }).Single();

                }
            }
            catch (Exception ex)
            {
            }
            return ;
        }

        /// <summary>
        /// Add WorkFlow
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddWorkflow(AddWorkflowLevelDetails inputObj, int loggedUserId)
        {
            string query = string.Empty;
            var wfID = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"INSERT INTO TbInitiativeWorkflow (WfName,WfDescription,WfNoOfLevels,CreatedDate,CreatedBy,IsDeleted,InitiativeId,WorkspaceID)
                            VALUES(@wfname,@wfdescription,@wfnooflevels,now(),@logUserId,0,@initid,@wsid); SELECT LAST_INSERT_ID();";
                    wfID = _connection.Query<int>(query, new
                    {
                        wfname = inputObj.WfName,
                        wfdescription = inputObj.WfDescription,
                        wfnooflevels = inputObj.WfNoOfLevels,
                        initid = inputObj.InitiativeId,
                        wsid = inputObj.WorkspaceID,
                        logUserId = loggedUserId
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return wfID;
        }

        /// <summary>
        /// Remove WorkFlow -> function used to delete workflow & workflow levels
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="wfIdList"></param>
        public void RemoveWorkflows(int loggedUserId, string wfIdList)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbInitiativeWorkflow Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wfidlist = wfIdList });

                        query = @"Update TbInitiativeWorkflowLevelDetails Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggedUserId 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId, wfidlist = wfIdList });
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion InitiativeWorkflow

        #region ProjectWorkflow

        /// <summary>
        /// Get Project Workflow List -> -> function used to fetch project workflow details based on ProjectId,WorkspaceID
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<Workflow> GetWorkflowsListByProjectID(int ProjectId, int WorkspaceID)
        {
            List<Workflow> objWorkflowsList = new List<Workflow>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Select WorkflowID ,IFNULL(WfName,'') as WfName ,IFNULL(WfDescription,'') as WfDescription ,WfNoOfLevels  From TbProjectWorkflow 
                            Where WorkspaceID =@wsid and  ProjectId=@projid and IsDeleted=0 ";
                        objWorkflowsList = _connection.Query<Workflow>(query, new { projid = ProjectId, wsid = WorkspaceID }).ToList();

                        foreach (Workflow wfItem in objWorkflowsList)
                        {
                            wfItem.WfLevelDetails = new List<WorkflowLevelDetails>();
                            for (int eachLevel = 1; eachLevel <= wfItem.WfNoOfLevels; eachLevel++)
                            {
                                var tempitem = new WorkflowLevelDetails();
                                tempitem.WfLevel = eachLevel;
                                //query = @"select ID, Name, EmailId, IFNULL(ProfileImage, '') as ProfileImage from TbUser where ID in 
                                //    (select UserID from TbProjectWorkflowDetails where WorkflowID=@workflowid and IsDeleted=0 and WfLevel=@eachlevel)";

                                //query = @"select t.ID, t.Name, t.EmailId, IFNULL(t.ProfileImage, '') as ProfileImage from TbUser t where exists (select P.UserID 
                                //            from TbProjectWorkflowDetails P where P.WorkflowID=@workflowid and P.IsDeleted=0 and P.WfLevel=@eachlevel and t.ID = P.UserID)";

                                query = @"select t.ID, t.Name, t.EmailId, IFNULL(t.ProfileImage, '') as ProfileImage, P.WorkflowLevelName AS workflowlevelname from TbUser t 
                                            inner join TbProjectWorkflowDetails P on P.WorkflowID=@workflowid and P.IsDeleted=0 and P.WfLevel=@eachlevel and t.ID = P.UserID";

                                tempitem.usersList = _connection.Query<User>(query, new { workflowid = wfItem.WorkflowID, eachlevel = eachLevel }).ToList();
                                if (tempitem.usersList.Count > 0)
                                {
                                    tempitem.workflowlevelname = tempitem.usersList[0].workflowlevelname;
                                }
                                wfItem.WfLevelDetails.Add(tempitem);
                            }
                        }
                    }

                    _Tran.Complete();
                }
                }
            catch (Exception ex)
            {
            }
            return objWorkflowsList;
        }

        /// <summary>
        /// Update Project WorkFlow -> function used to update project workflow details based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfName"></param>
        /// <param name="wfDesc"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateProjectWorkflowDetails(int wfID, string wfName, string wfDesc, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"UPDATE TbProjectWorkflow SET WfName=@WfName,WfDescription=@WfDescription,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                        _connection.Execute(query, new { WfName = wfName, WfDescription = wfDesc, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                        query = "Select ProjectId  From TbProjectWorkflow Where WorkflowID=@wfId ";
                        var ProjectId = _connection.Query<int>(query, new { wfId = wfID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Workflow";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = wfID;
                        objLog.FromIDColumnName = "WorkflowID";
                        objLog.Message = "Updated workflow details  named " + wfName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
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
        /// Update Project WorkFlow Level -> function used to update project workflow level based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfNoOfLevels"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateProjectWorkflowLevelCount(int wfID, int wfNoOfLevels, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"UPDATE TbProjectWorkflow SET WfNoOfLevels=@WfNoOfLevels,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                        _connection.Execute(query, new { WfNoOfLevels = wfNoOfLevels, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                        query = @"Update TaskSyncDB  Set TotalWFLevel = @WfNoOfLevels  Where Status != 'Completed' And WorkflowID = @wfId";
                        _connection.Execute(query, new { WfNoOfLevels = wfNoOfLevels, wfId = wfID });
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
        /// Delete Project WorkFlow -> function used to delete workflow based on WsID
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteProjectWorkflowLevel(int wfID, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"UPDATE TbProjectWorkflowDetails SET IsDeleted=@on,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                    _connection.Execute(query, new { on = 1, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Add Project WorkFlow Level
        /// </summary>
        /// <param name="wfID"></param>
        /// <param name="wfLevel"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateProjectWorkflowLevelDetails(int wfID, int wfLevel,string workflowlevelname, int userID, int groupID, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "INSERT INTO TbProjectWorkflowDetails( WorkflowID,WFLevel,WorkflowLevelName,UserID,CreatedDate,CreatedBy,IsDeleted)" +
                                " VALUES(@WorkflowID,@WFLevel,@WorkflowLevelName,@UserID,now(),@logUserId,0); SELECT LAST_INSERT_ID();";
                    _connection.Query(query, new
                    {
                        WorkflowID = wfID,
                        WFLevel = wfLevel,
                        WorkflowLevelName = workflowlevelname,
                        UserID = userID,
                        logUserId = loggedUserId
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Add Project WorkFlow
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddProjectWorkflow(AddWorkflowLevelDetails inputObj, int loggedUserId)
        {
            string query = string.Empty;
            var wfID = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"INSERT INTO TbProjectWorkflow (WfName,WfDescription,WfNoOfLevels,CreatedDate,CreatedBy,IsDeleted,ProjectId,WorkspaceID)
                            VALUES(@WfName,@WfDescription,@WfNoOfLevels,now(),@logUserId,0,@ProjectId,@WorkspaceID); SELECT LAST_INSERT_ID();";
                        wfID = _connection.Query<int>(query, new
                        {
                            WfName = inputObj.WfName,
                            WfDescription = inputObj.WfDescription,
                            WfNoOfLevels = inputObj.WfNoOfLevels,
                            ProjectId = inputObj.ProjectId,
                            WorkspaceID = inputObj.WorkspaceID,
                            logUserId = loggedUserId
                        }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = inputObj.ProjectId;
                        objLog.AuditFrom = "Workflow";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = wfID;
                        objLog.FromIDColumnName = "WorkflowID";
                        objLog.Message = "Added new Workflow named " + inputObj.WfName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return wfID;
        }

        /// <summary>
        /// Project WorkFlow Exists -> function used to check workflow exists in (Survey,Risk,Output,Outcome)
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="wfIdList"></param>
        /// <returns></returns>
        public bool IsProjectWorkflowsExists(int loggedUserId, string wfIdList)
        {
            bool isExists = false;
            int id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select count(a.id) as cunt from (select t.SurveyID as id from TbProjectSurvey t where FIND_IN_SET(t.WorkflowID,@wfidlist) union all select t.ID as id from TbRisk t where FIND_IN_SET(t.WorkflowID,@wfidlist) union all select t.ActivityId as id from TbProjectActivity t where FIND_IN_SET(t.WorkflowID,@wfidlist) union all select t.pk_OutcomeID as id from TbOutcomes t where FIND_IN_SET(t.WorkflowID,@wfidlist)) a;";
                    id = _connection.Query<int>(strQuery, new { wfidlist = wfIdList }).Single();

                    if (id > 0)
                    {
                        isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isExists;
        }

        /// <summary>
        /// Remove Project WorkFlow -> function used to delete project workflow & project workflow level
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="wfIdList"></param>
        public void RemoveProjectWorkflows(int loggedUserId, string wfIdList)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbProjectWorkflow Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wfidlist = wfIdList });

                        query = @"Update TbProjectWorkflowDetails Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggedUserId 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId, wfidlist = wfIdList });

                        //List<Workflow> lstWF = JsonConvert.DeserializeObject<List<Workflow>>(wfIdList);
                        //query = "Select ProjectId  From TbProjectWorkflow Where FIND_IN_SET(WorkflowID,@wfidlist)";
                        //var ProjectId = _connection.Query<int>(query, new { wfidlist = wfIdList).Single();

                        //foreach (Workflow Item in lstWF)
                        //{
                        //    ProjectAuditLog objLog = new ProjectAuditLog();
                        //    objLog.ProjectID = ProjectId;
                        //    objLog.AuditFrom = "Workflow";
                        //    objLog.AuditEvent = "REMOVE";
                        //    objLog.AuditFromID = Item.WorkflowID;
                        //    objLog.FromIDColumnName = "WorkflowID";
                        //    objLog.Message = "Added new Workflow named " + inputObj.WfName;
                        //    objLog.CreatedBy = loggedUserId;
                        //    ProjectDAL.AddAuditLog(objLog);
                        //}
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
        /// Add Project WorkFlow & Add Project WorkFlow Level 
        /// </summary>
        /// <param name="loggeduserid"></param>
        /// <param name="WorkflowList"></param>
        /// <param name="ProjectId"></param>
        /// <param name="WorkspaceID"></param>
        public void AddSelectWorkflow(int loggeduserid, string WorkflowList, int ProjectId, int WorkspaceID)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        List<Workflow> objWorkflowList = JsonConvert.DeserializeObject<List<Workflow>>(WorkflowList);
                        foreach (Workflow workflowItem in objWorkflowList)
                        {
                            query = @"INSERT INTO TbProjectWorkflow (WfName,WfDescription,WfNoOfLevels,CreatedDate,CreatedBy,IsDeleted,ProjectId,WorkspaceID)
                            VALUES(@wfname,@wfdescription,@wflevels,@createddate,@loguserid,0,@projid,@wsid); SELECT LAST_INSERT_ID();";
                            int wfID = _connection.Query<int>(query, new
                            {
                                wfname = workflowItem.WfName,
                                wfdescription = workflowItem.WfDescription,
                                wflevels = workflowItem.WfNoOfLevels,
                                createddate = DateTime.Now,
                                projid = ProjectId,
                                wsid = WorkspaceID,
                                loguserid = loggeduserid
                            }).Single();

                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectId;
                            objLog.AuditFrom = "Workflow";
                            objLog.AuditEvent = "ADD";
                            objLog.AuditFromID = wfID;
                            objLog.FromIDColumnName = "WorkflowID";
                            objLog.Message = "Added new Workflow named " + workflowItem.WfName;
                            objLog.CreatedBy = loggeduserid;

                            ProjectDAL.AddAuditLog(objLog);
                            foreach (WorkflowLevelDetails wfLevelItem in workflowItem.WfLevelDetails)
                            {
                                if (wfLevelItem.usersList.Count > 0)
                                {
                                    foreach (User userItem in wfLevelItem.usersList)
                                    {
                                        UpdateProjectWorkflowLevelDetails(wfID, wfLevelItem.WfLevel, wfLevelItem.workflowlevelname, userItem.ID, 0, loggeduserid);
                                    }
                                }
                            }
                        }
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        public List<Workflow> GetWorkflowsListForFilter(int WorkspaceID)
        {
            List<Workflow> objWorkflowsList = new List<Workflow>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Select WorkflowID ,IFNULL(WfName,'') as WfName ,IFNULL(WfDescription,'') as WfDescription ,WfNoOfLevels  From TbProjectWorkflow 
                            Where WorkspaceID =@wsid and IsDeleted=0 ";
                        objWorkflowsList = _connection.Query<Workflow>(query, new { wsid = WorkspaceID }).ToList();
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return objWorkflowsList;
        }
        #endregion ProjectWorkflow
    }
}
