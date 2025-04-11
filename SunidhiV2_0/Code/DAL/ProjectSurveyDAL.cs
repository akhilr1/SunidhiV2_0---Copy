#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using SunidhiV2_0.Code.Utils;
using SunidhiV2_0.Code.Common;
using Dapper;
using Newtonsoft.Json;
using static SunidhiV2_0.Models.SurveyResponse;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
#endregion Namespace

/// <summary>
/// ProjectSurveyDAL
/// </summary>
namespace SunidhiV2_0.Code.DAL
{
    public class ProjectSurveyDAL
    {
        protected IDbConnection _connection;

        #region SurveyLanding

        #region GetProjectSurveyList
        /// <summary>
        /// Function used to fetch project survey details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns>ProjectSurvey  List - json</returns>
        public List<ProjectSurvey> GetProjectSurveyList(int ProjectId = 0)
        {
            List<ProjectSurvey> lstProjectSurvey = new List<ProjectSurvey>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //string query = @"SELECT SRV.SurveyID,SRV.SurveyName,SRV.SurveyDescription, Date(SRV.StartDate) StartDate,Date(SRV.EndDate) EndDate,WRKFLW.WfName AS WorkFlowName,
                    //                BNF.`Name` AS BeneficiaryTypeName,	ifnull(ResponseCount,0) ResponseCount,DATE_FORMAT(SRV.CreatedDate,'%d/%m/%Y') CreatedDate, 
                    //                DATE_FORMAT((case when SRV.UpdatedDate is null then SRV.CreatedDate else SRV.UpdatedDate end) , '%d/%m/%Y') as UpdatedDate, ifnull(usr.asndUser,'') AssignedUsers,SRV.WorkflowID
                    //                FROM TbProjectSurvey SRV LEFT JOIN TbProjectWorkflow WRKFLW ON SRV.WorkflowID = WRKFLW.WorkflowID LEFT JOIN TbProjectBeneficiaryType BNF ON SRV.BeneficiaryTypeID = BNF.BeneficiaryTypeID 
                    //                LEFT JOIN ( SELECT GROUP_CONCAT(ifnull(usr.Name,'')) asndUser ,SurveyID   FROM TbUser usr JOIN   TbProjectSurveyUser  SRVUSR ON usr.ID = SRVUSR.UserID WHERE ifnull(SRVUSR.IsDeleted,0) = 0   GROUP BY SRVUSR.SurveyID ) usr ON usr.SurveyID = SRV.SurveyID  
                    //                Left Join (Select SurveyID, Count(distinct SyncTaskID)  ResponseCount From TbProjectSurveyResponses Group by SurveyID) SResponses On SResponses.SurveyID=SRV.SurveyID  
                    //                WHERE SRV.ProjectId = @ProjectId AND IFNULL(SRV.IsActive,0)=@IsActive Order By SRV.SurveyID desc";
                    string query = @"SELECT SRV.SurveyID,SRV.SurveyName,SRV.SurveyDescription, Date(SRV.StartDate) StartDate,Date(SRV.EndDate) EndDate,WRKFLW.WfName AS WorkFlowName,
                                    BNF.`Name` AS BeneficiaryTypeName,IFNULL(SResponses.ResponseCount,0)  ResponseCount,DATE_FORMAT(SRV.CreatedDate,'%d/%m/%Y') CreatedDate, 
                                    DATE_FORMAT(ifnull(SRV.UpdatedDate,SRV.CreatedDate) , '%d/%m/%Y') as UpdatedDate, ifnull(usr.asndUser,'') AssignedUsers,SRV.WorkflowID, ifnull(usr.ProfileImage,'') as ProfileImage
                                    FROM TbProjectSurvey SRV LEFT JOIN TbProjectWorkflow WRKFLW ON SRV.WorkflowID = WRKFLW.WorkflowID LEFT JOIN TbProjectBeneficiaryType BNF ON SRV.BeneficiaryTypeID = BNF.BeneficiaryTypeID 
                                    LEFT JOIN ( SELECT GROUP_CONCAT(ifnull(usr.Name,'')) asndUser ,SRVUSR.SurveyID, GROUP_CONCAT(usr.ProfileImage) as ProfileImage   FROM TbUser usr JOIN   TbProjectSurveyUser  SRVUSR ON usr.ID = SRVUSR.UserID WHERE ifnull(SRVUSR.IsDeleted,0) = 0   GROUP BY SRVUSR.SurveyID ) usr ON usr.SurveyID = SRV.SurveyID  
                                    Left Join 
                                   (select r.TaskCount  as ResponseCount, r.SurveyID from (Select  sum( TSR1.ResponseID) as ResponseID , TSR1.SurveyID, (
                                select count(RSP.QuestionID) TaskCount from  TbProjectSurveyResponses RSP JOIN  (SELECT QST.QuestionID
                                FROM TbProjectSurveyQuestion QST LEFT JOIN TbProjectSurveySectionLogic LG ON QST.SectionID = LG.SectionID
                                WHERE QST.SurveyID = TSR1.SurveyID  AND (LG.IsActive = 1 OR LG.IsActive IS NULL) AND (QST.IsActive = 1 OR QST.IsActive IS NULL) LIMIT 1
                                ) TT ON RSP.QuestionID = TT.QuestionID  AND (RSP.IsActive = 1 OR RSP.IsActive IS NULL) )  TaskCount
                                From TbProjectSurveyResponses TSR1 where TSR1.IsActive = 1 Group by TSR1.SurveyID ) r) SResponses On SResponses.SurveyID=SRV.SurveyID  
                                    WHERE SRV.ProjectId = @ProjectId AND IFNULL(SRV.IsActive,0)=@IsActive Order By SRV.SurveyID desc";


                    lstProjectSurvey = _connection.Query<ProjectSurvey>(query, new
                    {
                        ProjectId = ProjectId,
                        IsActive = 1
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstProjectSurvey;
        }
        #endregion GetProjectSurveyList

        #region GetProjectSurveyBySurveyId
        /// <summary>
        /// Function used to fetch survey details based on SurveyID
        /// </summary>
        /// <param name="SurveyID">SurveyID</param>
        /// <returns> ProjectSurvey object </returns>
        public ProjectSurvey GetProjectSurveyBySurveyId(int SurveyID = 0)
        {
            ProjectSurvey objProjectSurvey = new ProjectSurvey();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"SELECT SurveyID,SurveyName,ifnull(SurveyDescription, '') as SurveyDescription, DATE_FORMAT(StartDate,'%d-%m-%Y') StartDate,DATE_FORMAT(EndDate,'%d-%m-%Y') EndDate,PS.WorkflowID,ReportingFrequency,TotalWFLevel,BeneficiaryTypeID,
                     WorkspaceID,(Case When ReportingFrequency =1 then 'Never'	When ReportingFrequency =2 then 'Once'  When ReportingFrequency =3 then 'Daily'
							When ReportingFrequency =4 then 'Weekly'  When ReportingFrequency =5 then 'Monthly'	When ReportingFrequency =6 then 'Quarterly'
							When ReportingFrequency =7 then 'Half_Yearly'	When ReportingFrequency =8 then 'Yearly' When ReportingFrequency =9 then 'Anytime' End ) ReportingFrequencyName FROM TbProjectSurvey PS inner join  TaskSyncDB on SurveyID=TypeID WHERE SurveyID = @SurveyID AND IFNULL(PS.IsActive,0)=@IsActive";

                    objProjectSurvey = _connection.Query<ProjectSurvey>(query, new
                    {
                        SurveyID = SurveyID,
                        IsActive = 1
                    }).FirstOrDefault();

                    objProjectSurvey.AssignedUsers = GetSurveyAssignedUsers(SurveyID);
                    objProjectSurvey.AssignedTags = GetSurveyAssignedTags(SurveyID);

                    DateTime? RptDate = null;

                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbProjectSurveyTrackDate WHERE SurveyID = @SurveyID order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { SurveyID = objProjectSurvey.SurveyID }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }

                    if (RptDate != null)
                    {
                        switch (objProjectSurvey.ReportingFrequency)
                        {
                            case (int)CommonActivity.ReportingFrequency.Once:
                                objProjectSurvey.ReportingOnceDate = RptDate.Value.ToString("dd-MM-yyyy");
                                break;
                            case (int)CommonActivity.ReportingFrequency.Weekly:
                                objProjectSurvey.ReportingWeek = (int)RptDate.Value.DayOfWeek;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Monthly:
                                objProjectSurvey.ReportingMonthDay = RptDate.Value.Day;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Quarterly:
                                objProjectSurvey.ReportingMonthDateDay = RptDate.Value.Day;
                                objProjectSurvey.ReportingMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                objProjectSurvey.ReportingMonthDateDay = RptDate.Value.Day;
                                objProjectSurvey.ReportingMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Yearly:
                                objProjectSurvey.ReportingMonthDateDay = RptDate.Value.Day;
                                objProjectSurvey.ReportingMonth = RptDate.Value.Month;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objProjectSurvey;
        }

        #endregion GetProjectSurveyBySurveyId

        #region InsertProjectSurvey
        /// <summary>
        /// Function used to add new survey
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool InsertProjectSurvey(ProjectSurvey objProjectSurvey, int loggeduserid, DateTime? StartDate, DateTime? EndDate, bool notify = false)
        {
            var SurveyId = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string strQuery = @"INSERT INTO TbProjectSurvey(SurveyName,SurveyDescription,ProjectId,WorkflowID,";
                        strQuery += "ReportingFrequency,BeneficiaryTypeID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,StartDate,EndDate)";
                        strQuery += "VALUES (@SurveyName,@SurveyDescription,@ProjectId,@WorkflowID,@ReportingFrequency,@BeneficiaryTypeID,";
                        strQuery += "@IsActive,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy,@StartDate,@EndDate); SELECT LAST_INSERT_ID();";

                        SurveyId = _connection.Query<int>(strQuery, new
                        {
                            SurveyName = objProjectSurvey.SurveyName,
                            SurveyDescription = objProjectSurvey.SurveyDescription != "null" ? objProjectSurvey.SurveyDescription : null,
                            ProjectId = objProjectSurvey.ProjectId,
                            WorkflowID = objProjectSurvey.WorkflowID,
                            ReportingFrequency = objProjectSurvey.ReportingFrequency,
                            BeneficiaryTypeID = objProjectSurvey.BeneficiaryTypeID,
                            StartDate = objProjectSurvey.StartDate != "null" ? objProjectSurvey.StartDate : null,
                            EndDate = objProjectSurvey.EndDate != "null" ? objProjectSurvey.EndDate : null,
                            IsActive = 1,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            CreatedBy = loggeduserid,
                            UpdatedBy = loggeduserid
                        }).Single();
                        string Qeury = @"Select A.WorkflowId, AW.WfNoOfLevels  From TbProjectSurvey A 
                                Left Join TbProjectWorkflow  AW On AW.WorkflowID = A.WorkflowId
                                Where A.IsActive = 1 And AW.IsDeleted = 0 And A.SurveyID = @SurveyId";
                        dynamic objWF = _connection.Query<object>(Qeury, new { SurveyId = SurveyId }).FirstOrDefault();

                        Qeury = @"Select WSID as WorkspaceID From TbProject  Where ProjectId = @ProjectId";
                        var WID = _connection.Query<int>(Qeury, new { ProjectId = objProjectSurvey.ProjectId }).FirstOrDefault();

                        //AddTo SyncDB
                        strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                        var syncDBId = _connection.Query<int>(strQuery, new
                        {
                            Type = "Survey",
                            Command = "ADD",
                            TypeIDColumn = "SurveyID",
                            TypeID = SurveyId,
                            WorkflowID = objWF == null ? 0 : objWF.WorkflowId,
                            WfNoOfLevels = objWF == null ? 0 : objWF.WfNoOfLevels,
                            Status = "Pending",
                            WorkspaceID = WID,
                            CreatedBy = loggeduserid

                        }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = objProjectSurvey.ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = SurveyId;
                        objLog.FromIDColumnName = "SurveyId";
                        objLog.Message = "Added new Survey named " + objProjectSurvey.SurveyName;
                        objLog.CreatedBy = loggeduserid;
                        ProjectDAL.AddAuditLog(objLog);

                        if (SurveyId > 0)
                        {
                            objProjectSurvey.SurveyID = SurveyId;
                            bool isUserAssigned = AssignUserstoSurvey(objProjectSurvey, loggeduserid, syncDBId);
                            bool isTagAssigned = AssignTagstoSurvey(objProjectSurvey, loggeduserid);
                            bool isTrackdategenerated = GenerateReportingFrequencyTrackDate(objProjectSurvey, loggeduserid, StartDate, EndDate);
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion InsertProjectSurvey

        #region SaveProjectSurveyTag
        /// <summary>
        /// Function used to insert / update project survey tag details
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <param name="TagID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveProjectSurveyTag(int SurveyId, int TagID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            var retMsg = "success";

            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        strQuery = @"SELECT EXISTS(SELECT ID FROM TbProjectSurveyTag WHERE SurveyID=@SurveyID and TagID=@tagid);";
                        id = _connection.Query<int>(strQuery, new { SurveyID = SurveyId, tagid = TagID }).Single();

                        if (id > 0)
                        {
                            strQuery = "Update TbProjectSurveyTag SET IsDeleted = @IsDeleted,UpdatedDate = @updatedDate,UpdatedBy =@loggedUserId ";
                            strQuery += " Where SurveyID =@SurveyID and TagID =@tagid";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, SurveyID = SurveyId, tagid = TagID, IsDeleted = 0 });
                        }
                        else
                        {
                            strQuery = "INSERT INTO TbProjectSurveyTag(SurveyID,TagID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted)";
                            strQuery += " VALUES(@SurveyID,@TagID,@CreatedDate,@loggedUserId,@CreatedDate,@loggedUserId,@IsDeleted); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { SurveyID = SurveyId, TagID = TagID, loggedUserId = loggedUserId, CreatedDate = DateTime.Now, IsDeleted = 0 }).Single();
                        }
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error";
                        throw ex;
                    }
                    _Tran.Complete();
                }
                return retMsg;
            }
        }
        #endregion SaveProjectSurveyTag

        #region SaveProjectSurveyUser
        /// <summary>
        /// Function used to insert / update project survey user
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <param name="UserId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveProjectSurveyUser(int SurveyId, int UserId, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            var retMsg = "success";

            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        strQuery = @"SELECT EXISTS(SELECT ID FROM TbProjectSurveyUser WHERE SurveyID=@SurveyID and UserID=@UserID);";
                        id = _connection.Query<int>(strQuery, new { SurveyID = SurveyId, UserID = UserId }).Single();

                        if (id > 0)
                        {
                            strQuery = "Update TbProjectSurveyUser SET IsDeleted = @IsDeleted,UpdatedDate = @updatedDate,UpdatedBy =@loggedUserId ";
                            strQuery += " Where SurveyID =@SurveyID and UserID =@UserID";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, SurveyID = SurveyId, UserID = UserId, IsDeleted = 0 });
                        }
                        else
                        {
                            strQuery = "INSERT INTO TbProjectSurveyUser(SurveyID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted)";
                            strQuery += " VALUES(@SurveyID,@UserID,@CreatedDate,@loggedUserId,@CreatedDate,@loggedUserId,@IsDeleted); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { SurveyID = SurveyId, UserID = UserId, loggedUserId = loggedUserId, CreatedDate = DateTime.Now, IsDeleted = 0 }).Single();
                        }
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error";
                        throw ex;
                    }
                    _Tran.Complete();
                }
                return retMsg;
            }
        }
        #endregion SaveSurveyTagDetails

        #region IsSurveyNameExists
        /// <summary>
        /// Function used to check survey name already exists / not based on ProjectId,SurveyID
        /// </summary>
        /// <param name="ProjectId">ProjectId</param>
        /// <param name="SurveyName">SurveyName</param>
        /// <param name="SurveyID">SurveyID</param>
        /// <returns> bool - true /false </returns>
        public bool IsSurveyNameExists(int ProjectId, string SurveyName, int SurveyID)
        {
            bool isExists = false;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "SELECT COUNT(SurveyID) FROM TbProjectSurvey WHERE ProjectId = @ProjectId";
                    strQuery += " AND LOWER(TRIM(SurveyName))= @SurveyName and IsActive=@IsActive and ifnull(SurveyID,0) != @SurveyID";
                    int cnt = _connection.Query<int>(strQuery, new
                    {
                        ProjectId = ProjectId,
                        SurveyName = SurveyName.ToLower(),
                        IsActive = 1,
                        SurveyID = SurveyID
                    }).Single();

                    if (cnt > 0)
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
        #endregion IsSurveyNameExists

        #region UpdateProjectSurvey
        /// <summary>
        /// Function used to update project survey details based on SurveyID
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <param name="loggeduserid"></param>
        /// <returns>bool - true / false </returns>
        public bool UpdateProjectSurvey(ProjectSurvey objProjectSurvey, int loggeduserid, DateTime? StartDate, DateTime? EndDate, bool notify = false)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string strQuery = @"Update TbProjectSurvey SET SurveyName=@SurveyName,SurveyDescription=@SurveyDescription,WorkflowID=@WorkflowID," +
                                 "ReportingFrequency =@ReportingFrequency,BeneficiaryTypeID =@BeneficiaryTypeID,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy," +
                                 " StartDate =@StartDate ,EndDate =@EndDate  WHERE SurveyID = @SurveyID";

                        var rr = _connection.Execute(strQuery, new
                        {
                            SurveyName = objProjectSurvey.SurveyName,
                            SurveyDescription = objProjectSurvey.SurveyDescription,
                            WorkflowID = objProjectSurvey.WorkflowID,
                            ReportingFrequency = objProjectSurvey.ReportingFrequency,
                            BeneficiaryTypeID = objProjectSurvey.BeneficiaryTypeID,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = loggeduserid,
                            SurveyID = objProjectSurvey.SurveyID,
                            StartDate = StartDate,
                            EndDate = EndDate

                        });
                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = objProjectSurvey.ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = objProjectSurvey.SurveyID;
                        objLog.FromIDColumnName = "SurveyID";
                        objLog.Message = "Updated Survey details named " + objProjectSurvey.SurveyName;
                        objLog.CreatedBy = loggeduserid;
                        ProjectDAL.AddAuditLog(objLog);

                        bool isUserAssigned = AssignUserstoSurvey(objProjectSurvey, loggeduserid, 0);
                        bool isTagAssigned = AssignTagstoSurvey(objProjectSurvey, loggeduserid);
                        bool isTrackdategenerated = GenerateReportingFrequencyTrackDate(objProjectSurvey, loggeduserid, StartDate, EndDate);

                        string Qeury = @"Select WfNoOfLevels  From  TbProjectWorkflow  Where IsDeleted = 0 And WorkflowID = @WorkflowID";
                        var WfNoOfLevels = _connection.Query<int>(Qeury, new { WorkflowID = objProjectSurvey.WorkflowID }).FirstOrDefault();

                        ////Update To SyncDB
                        strQuery = @"Update TaskSyncDB SET  WorkflowID =@WorkflowID,TotalWFLevel =@TotalWFLevel WHERE TypeID = @TypeID  And Type =@Type ";
                        var syncDBId = _connection.Execute(strQuery, new
                        {
                            Type = "Survey",
                            TypeID = objProjectSurvey.SurveyID,
                            WorkflowID = objProjectSurvey.WorkflowID,
                            TotalWFLevel = WfNoOfLevels

                        });

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion SaveProjectSurvey

        #region DeleteProjectSurvey
        /// <summary>
        /// Function used to delete project survey based on SurveyId
        /// </summary>
        /// <param name="SurveyId">SurveyId</param>
        /// <param name="loggeduserid">loggeduserid</param>
        /// <returns>bool - true/false </returns>
        public bool DeleteProjectSurvey(int SurveyId, int loggeduserid)
        {
            bool isDeleted = false;
            ProjectSurvey objProjectSurvey = new ProjectSurvey();

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string strQuery = "Update TbProjectSurvey SET IsActive = @IsActive,UpdatedDate=@UpdatedDate,";
                        strQuery += "UpdatedBy=@UpdatedBy WHERE SurveyID =@SurveyID;";

                        var rr = _connection.Execute(strQuery, new
                        {
                            SurveyID = SurveyId,
                            UpdatedBy = loggeduserid,
                            UpdatedDate = DateTime.Now,
                            IsActive = 0
                        });

                        isDeleted = (rr == 1 ? true : false);
                        strQuery = @"Select ProjectId,SurveyName From TbProjectSurvey Where SurveyID =@SurveyId";
                        objProjectSurvey = _connection.Query<ProjectSurvey>(strQuery, new
                        {
                            SurveyId = SurveyId
                        }).FirstOrDefault();


                        //strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        //var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = SurveyId }).Single();
                        //strQuery = "Select SurveyName  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        //var name = _connection.Query<string>(strQuery, new { SurveyId = SurveyId }).Single();


                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = objProjectSurvey.ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "REMOVE";
                        objLog.AuditFromID = SurveyId;
                        objLog.FromIDColumnName = "SurveyID";
                        objLog.Message = "Removed Survey named " + objProjectSurvey.SurveyName;
                        objLog.CreatedBy = loggeduserid;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return isDeleted;
        }
        #endregion DeleteProjectSurvey      

        #region 
        /// <summary>
        /// Function used to check section exists / not based on SurveyId, SectionName, SectionId
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <param name="SectionName"></param>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        public bool IsSectionNameExists(int SurveyId, string SectionName, int SectionId)
        {
            bool isExists = false;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "SELECT COUNT(SectionID) FROM TbProjectSurveySection WHERE SurveyID = @SurveyID" +
                                    " AND LOWER(TRIM(SectionName))= @SectionName AND IsActive=@IsActive AND IFNULL(SectionID,0) = @SectionID";
                    int cnt = _connection.Query<int>(strQuery, new
                    {
                        SurveyID = SurveyId,
                        SectionName = SectionName.ToLower(),
                        IsActive = 1,
                        SectionID = SectionId
                    }).Single();

                    if (cnt > 0)
                        isExists = true;
                }
            }
            catch (Exception ex)
            {
            }
            return isExists;
        }

        #region ChangeSurveySectionOrder
        /// <summary>
        /// Function used to change survey section order based on SectionId
        /// </summary>
        /// <param name="SectionId">SectionId</param>
        /// <param name="SurveyId">SurveyId</param>
        /// <param name="key">key - up/ down </param>
        /// <param name="loggedUserId">loggedUserId</param>
        /// <returns> bool -  true / false </returns>
        public bool ChangeSurveySectionOrder(int SectionId, string key, int loggedUserId)
        {
            bool status = false;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    ProjectSurveySection objProjectSurveySection = new ProjectSurveySection();

                    string query = @"SELECT SectionID,SectionOrder,SurveyID FROM TbProjectSurveySection ";
                    query += " WHERE SectionID = @SectionId AND IFNULL(IsActive,0)=@IsActive";
                    objProjectSurveySection = _connection.Query<ProjectSurveySection>(query, new
                    {
                        SectionID = SectionId,
                        IsActive = 1
                    }).FirstOrDefault();

                    if (key == "down")
                    {
                        ProjectSurveySection objProjectSurveySection1 = new ProjectSurveySection();

                        query = @"SELECT SectionID,SectionOrder FROM TbProjectSurveySection  WHERE SurveyID = @SurveyID ";
                        query += " AND IFNULL(IsActive,0)=@IsActive AND SectionOrder >@SectionOrder ORDER BY SectionOrder;";
                        objProjectSurveySection1 = _connection.Query<ProjectSurveySection>(query, new
                        {
                            SurveyID = objProjectSurveySection.SurveyID,
                            IsActive = 1,
                            SectionOrder = objProjectSurveySection.SectionOrder
                        }).FirstOrDefault();

                        if (objProjectSurveySection1 != null)
                        {
                            //update Current Section Order
                            string strQuery = @"Update TbProjectSurveySection SET SectionOrder=@SectionOrder,UpdatedDate=@UpdatedDate,";
                            strQuery += "UpdatedBy=@UpdatedBy WHERE SectionID = @SectionId AND IFNULL(IsActive,0)=@IsActive;";
                            int currentOrder = _connection.Execute(strQuery, new
                            {
                                SectionId = SectionId,
                                SectionOrder = objProjectSurveySection1.SectionOrder,
                                IsActive = 1,
                                UpdatedBy = loggedUserId,
                                UpdatedDate = DateTime.Now
                            });

                            //update upcomig Section Order
                            strQuery = @"Update TbProjectSurveySection SET SectionOrder=@SectionOrder,UpdatedDate=@UpdatedDate,";
                            strQuery += "UpdatedBy=@UpdatedBy WHERE SectionID = @SectionId AND IFNULL(IsActive,0)=@IsActive;";
                            int upcomingOrder = _connection.Execute(strQuery, new
                            {
                                SectionId = objProjectSurveySection1.SectionID,
                                SectionOrder = objProjectSurveySection.SectionOrder,
                                IsActive = 1,
                                UpdatedBy = loggedUserId,
                                UpdatedDate = DateTime.Now
                            });

                            if (currentOrder > 0 && upcomingOrder > 0)
                            {
                                status = true;
                            }
                        }
                    }
                    else if (key == "up")
                    {
                        ProjectSurveySection objProjectSurveySection1 = new ProjectSurveySection();

                        query = @"SELECT SectionID,SectionOrder FROM TbProjectSurveySection  WHERE SurveyID = @SurveyID ";
                        query += " AND IFNULL(IsActive,0)=@IsActive AND SectionOrder < @SectionOrder ORDER BY SectionOrder DESC;";
                        objProjectSurveySection1 = _connection.Query<ProjectSurveySection>(query, new
                        {
                            SurveyID = objProjectSurveySection.SurveyID,
                            IsActive = 1,
                            SectionOrder = objProjectSurveySection.SectionOrder
                        }).FirstOrDefault();

                        if (objProjectSurveySection1 != null)
                        {
                            //update Current Section Order
                            string strQuery = @"Update TbProjectSurveySection SET SectionOrder=@SectionOrder,UpdatedDate=@UpdatedDate,";
                            strQuery += "UpdatedBy=@UpdatedBy WHERE SectionID = @SectionId AND IFNULL(IsActive,0)=@IsActive;";
                            int currentOrder = _connection.Execute(strQuery, new
                            {
                                SectionId = SectionId,
                                SectionOrder = objProjectSurveySection1.SectionOrder,
                                IsActive = 1,
                                UpdatedBy = loggedUserId,
                                UpdatedDate = DateTime.Now
                            });

                            //update previous Section Order
                            strQuery = @"Update TbProjectSurveySection SET SectionOrder=@SectionOrder,UpdatedDate=@UpdatedDate,";
                            strQuery += "UpdatedBy=@UpdatedBy  WHERE SectionID = @SectionId AND IFNULL(IsActive,0)=@IsActive;";
                            int previousOrder = _connection.Execute(strQuery, new
                            {
                                SectionId = objProjectSurveySection1.SectionID,
                                SectionOrder = objProjectSurveySection.SectionOrder,
                                IsActive = 1,
                                UpdatedBy = loggedUserId,
                                UpdatedDate = DateTime.Now
                            });

                            if (currentOrder > 0 && previousOrder > 0)
                            {
                                status = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        #endregion ChangeSurveySectionOrder

        #region GetSurveyHierarchyGroup
        /// <summary>
        /// Function used to fetch all survey hierarchy group details 
        /// </summary>
        /// <returns> Survey Hierarchy group list - json</returns>
        public List<KeyValuePair<int, string>> GetSurveyHierarchyGroup()
        {
            List<KeyValuePair<int, string>> lstHierarchyGroup = new List<KeyValuePair<int, string>>();
            dynamic obj;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"SELECT GroupId,GroupName FROM TbSurveyHierarchyGroup WHERE IFNULL(IsActive,0)=@IsActive";
                    obj = _connection.Query<object>(query, new { IsActive = 1 }).ToList();

                    foreach (dynamic item in obj)
                    {
                        lstHierarchyGroup.Add(new KeyValuePair<int, string>(item.GroupId, item.GroupName));
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstHierarchyGroup;
        }
        #endregion GetSurveyHierarchyGroup

        #region GetHierarchyDrpValues
        /// <summary>
        /// Function used to all hierarchyDrp values based on type
        /// </summary>
        /// <param name="mainDrpValue">mainDrpValue</param>
        /// <param name="type">type</param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetHierarchyDrpValues(string mainDrpValue, string type)
        {
            List<KeyValuePair<int, string>> values = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (type == "State")
                    {
                        obj = _connection.Query<object>("SELECT StateID,StateName FROM TbStates ORDER BY StateName ").ToList();
                        foreach (dynamic item in obj)
                        {
                            values.Add(new KeyValuePair<int, string>(item.StateID, item.StateName));
                        }
                        return values;
                    }
                    else if (type == "District")
                    {
                        string query = "SELECT DistrictID,DistrictName FROM TbDistrict WHERE StateID=@stateid ORDER BY DistrictName ";
                        obj = _connection.Query<object>(query, new { stateid = mainDrpValue }).ToList();
                        foreach (dynamic item in obj)
                        {
                            values.Add(new KeyValuePair<int, string>(item.DistrictID, item.DistrictName));
                        }
                        return values;
                    }
                    else if (type == "Subdistrict")
                    {
                        string query = "SELECT SubDistrictID,SubDistrictName FROM TbSubDistrict WHERE DistrictID=@DistrictID ORDER BY SubDistrictName ";
                        obj = _connection.Query<object>(query, new { DistrictID = mainDrpValue }).ToList();
                        foreach (dynamic item in obj)
                        {
                            values.Add(new KeyValuePair<int, string>(item.SubDistrictID, item.SubDistrictName));
                        }
                        return values;
                    }
                    else if (type == "GP")
                    {
                        string query = "SELECT BlockCode,BlockName FROM TbBlock WHERE SubdistrictCode=@SubDistrictID ";
                        obj = _connection.Query<object>(query, new { SubDistrictID = mainDrpValue }).ToList();
                        foreach (dynamic item in obj)
                        {
                            values.Add(new KeyValuePair<int, string>(item.BlockCode, item.BlockName));
                        }
                        return values;
                    }
                    else if (type == "village")
                    {
                        string query = "SELECT VillageID,VillageName FROM TbVillage WHERE BlockId=@BlockId ";
                        obj = _connection.Query<object>(query, new { BlockId = mainDrpValue }).ToList();
                        foreach (dynamic item in obj)
                        {
                            values.Add(new KeyValuePair<int, string>(item.VillageID, item.VillageName));
                        }
                        return values;
                    }
                    else { return values; }
                }
            }
            catch (Exception ex)
            {
                return values;
            }
        }
        #endregion GetHierarchyDrpValues

        #region GetHeirarchyGroupItems
        /// <summary>
        /// Function used to fetch hierarchy group items based on GroupId,Order
        /// </summary>
        /// <param name="GroupId">GroupId</param>
        /// <param name="Order">Order</param>
        /// <param name="ParentId">ParentId</param>
        /// <returns>KeyValuePair</returns>
        public List<KeyValuePair<int, string>> GetHierarchyGroupItems(int GroupId, int Order, int ParentId = 0)
        {
            List<KeyValuePair<int, string>> lstWorkFlow = new List<KeyValuePair<int, string>>();
            MasterHierarchy objMasterHierarchy = new MasterHierarchy();
            dynamic obj;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"SELECT TableName,PrimaryKeyId,FieldName,ParentColumnId FROM TbSurveyMasterHierarchy WHERE  ";
                    query += "GroupId =@GroupId AND HierarchyOrder = @HierarchyOrder";
                    objMasterHierarchy = _connection.Query<MasterHierarchy>(query,
                        new
                        {
                            GroupId = GroupId,
                            HierarchyOrder = Order
                        }).FirstOrDefault();

                    if (objMasterHierarchy != null)
                    {
                        string stringQry = "SELECT " + objMasterHierarchy.PrimaryKeyId + " AS ItemId," + objMasterHierarchy.FieldName;
                        stringQry += " AS ItemName FROM " + objMasterHierarchy.TableName + " WHERE IFNULL(" + (objMasterHierarchy.ParentColumnId == null ? "0" : objMasterHierarchy.ParentColumnId) + ",0)=@ParentId";

                        obj = _connection.Query<object>(stringQry, new { ParentId = ParentId }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstWorkFlow.Add(new KeyValuePair<int, string>(item.ItemId, item.ItemName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return lstWorkFlow;
        }
        #endregion GetQuestionType
        #endregion

        #region GetProjectBeneficiaryType
        /// <summary>
        /// GetProjectBeneficiaryType
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns> List </returns>
        public List<BeneficiaryType> GetProjectBeneficiaryType(int projectId = 0, int surveyId = 0)
        {
            List<BeneficiaryType> lstBeneficiaryType = new List<BeneficiaryType>();
            string query = "";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //    string query = @"SELECT BFT.BeneficiaryTypeID as ID,BFT.Name,DATE_FORMAT(BFT.CreatedDate,'%d/%m/%Y') CreatedDate,";
                    //    query += "case when ifnull(SRV.SurveyID,0) > 0 then 1 else 0 end as IsSelected  FROM TbProjectBeneficiaryType BFT  ";
                    //    query += " LEFT JOIN TbProjectSurvey SRV ON BFT.BeneficiaryTypeID = SRV.BeneficiaryTypeID ";
                    //    query += " WHERE BFT.ProjectID =@ProjectID AND IFNULL(BFT.IsActive,0)=@IsActive AND ";
                    //    query += " (ifnull(SRV.SurveyID,0) = 0 ||  ifnull(SRV.SurveyID,0) = @surveyId)";

                    if (surveyId == 0)
                        query = @"SELECT BeneficiaryTypeID as ID,Name,DATE_FORMAT(CreatedDate,'%d/%m/%Y') CreatedDate, 0 IsSelected  
                            FROM TbProjectBeneficiaryType WHERE ProjectID =@ProjectID AND IFNULL(IsActive,0)=@IsActive AND IsDeleted=0 ";
                    else
                        //query = @"SELECT BeneficiaryTypeID as ID,Name,DATE_FORMAT(CreatedDate,'%d/%m/%Y') CreatedDate ,1 IsSelected
                        //    From TbProjectBeneficiaryType Where ProjectID =@ProjectID AND IFNULL( IsActive,0)=@IsActive AND IsDeleted=0 
                        //    And BeneficiaryTypeID= (Select BeneficiaryTypeID FRom TbProjectSurvey Where SurveyID= @surveyId)
                        //    Union Select BeneficiaryTypeID as ID,Name,DATE_FORMAT(CreatedDate,'%d/%m/%Y') CreatedDate ,0 IsSelected
                        //    From TbProjectBeneficiaryType Where ProjectID =@ProjectID AND IFNULL( IsActive,0)=@IsActive AND IsDeleted=0 
                        //    And BeneficiaryTypeID not in  (Select BeneficiaryTypeID FRom TbProjectSurvey Where SurveyID=@surveyId) ";

                        query = @"SELECT t.BeneficiaryTypeID as ID,t.Name,DATE_FORMAT(t.CreatedDate,'%d/%m/%Y') CreatedDate ,1 IsSelected
                                 From TbProjectBeneficiaryType t inner join TbProjectSurvey s on t.BeneficiaryTypeID = s.BeneficiaryTypeID
                                  and s.SurveyID= @surveyId Where t.ProjectID = @ProjectID AND IFNULL(t.IsActive,0)= @IsActive AND t.IsDeleted=0  
                                 Union Select b.BeneficiaryTypeID as ID,b.Name,DATE_FORMAT(b.CreatedDate,'%d/%m/%Y') CreatedDate ,0 IsSelected
                                 From TbProjectBeneficiaryType b Where b.ProjectID = @ProjectID AND IFNULL(b.IsActive,0)= @IsActive AND b.IsDeleted=0 
                                 and not exists (Select v.BeneficiaryTypeID FRom TbProjectSurvey v Where v.SurveyID=@surveyId and 
                                b.BeneficiaryTypeID=v.BeneficiaryTypeID)";

                    lstBeneficiaryType = _connection.Query<BeneficiaryType>(query, new { ProjectID = projectId, IsActive = 1, surveyId = surveyId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiaryType;
        }
        #endregion GetProjectBeneficiaryType

        #region GetProjectData
        /// <summary>
        /// Function used to fetch project data based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetProjectData(int WorkspaceID = 0)
        {
            List<KeyValuePair<int, string>> lstproject = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (WorkspaceID != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT t.ProjectId,t.ProjectName FROM TbProject t ";
                        query += " WHERE IFNULL(t.IsDeleted,0)=@IsDeleted AND t.IsActive = @IsActive and t.WSID = @WSID and t.ProjectStatus='Deploy';";
                        obj = _connection.Query<object>(query, new { IsDeleted = 0, IsActive = 1, WSID = WorkspaceID }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstproject.Add(new KeyValuePair<int, string>(item.ProjectId, item.ProjectName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstproject;
        }
        #endregion GetProjectData        

        #region GetSurveyData
        /// <summary>
        /// Function used to fetch survey data based on ProjectId,mStartDate,mEndDate
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="mStartDate"></param>
        /// <param name="mEndDate"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetSurveyData(int ProjectId = 0, DateTime? mStartDate = null, DateTime? mEndDate = null)
        {
            List<KeyValuePair<int, string>> lstsurvey = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (ProjectId != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT t.SurveyID,t.SurveyName from TbProjectSurvey t  WHERE t.ProjectId = @ProjectId";
                        if ((mStartDate != null) && (mEndDate != null))
                        {
                            query += " AND t.StartDate between @mStartDate and @mEndDate";
                        }
                        query += " AND IFNULL(t.IsActive,0)=@IsActive;";
                        obj = _connection.Query<object>(query, new { ProjectId = ProjectId, IsActive = 1, mStartDate = mStartDate, mEndDate = mEndDate }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstsurvey.Add(new KeyValuePair<int, string>(item.SurveyID, item.SurveyName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstsurvey;
        }
        #endregion GetSurveyData  

        #region GetQuestionData
        /// <summary>        
        /// <returns> List </returns>
        public List<KeyValuePair<int, string>> GetQuestionData(int SurveyID = 0)
        {
            List<KeyValuePair<int, string>> lstquestion = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (SurveyID != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT t.QuestionID,t.QuestionName from TbProjectSurveyQuestion t ";
                        query += " WHERE t.SurveyID = @SurveyID AND IFNULL(t.IsActive,0)=@IsActive;";
                        obj = _connection.Query<object>(query, new { SurveyID = SurveyID, IsActive = 1 }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstquestion.Add(new KeyValuePair<int, string>(item.QuestionID, item.QuestionName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstquestion;
        }
        #endregion GetQuestionData  

        #region GetSurveyAuditLog
        /// <summary>
        /// Function used to fetch survey audit log details based SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public IEnumerable<TaskAuditLog> GetSurveyAuditLog(int SurveyID = 0)
        {
            List<TaskAuditLog> lstSurveyLog = new List<TaskAuditLog>();
            try
            {
                if (SurveyID != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT A.CreatedBy,A.Name,A.Message , A.CreatedDate, A.Type , A.TypeID, A.TaskUpdateComment , A.AttachedDocumentName, A.ProfileImage ,DATE_FORMAT(A.CreatedDate, '%Y-%m-%d %T') CreatedDateStr
		                        FROM (
		                                SELECT	TAL.CreatedBy,user.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType `Type`,
				                                TAL.TaskTypeID TypeID,IFNULL(TAL.Command,'') AS TaskUpdateComment,
				                                IFNULL(TAL.Document, """") AttachedDocumentName,
				                                IFNULL(user.ProfileImage,'') AS ProfileImage
		                                FROM	TbTaskAuditLog TAL
		                                INNER JOIN	TbUser user
			                                ON	user.ID =TAL.AssignedBy
		                                WHERE	TAL.TaskType = 'Survey'	and TAL.TaskTypeID = @SurveyID 
                                 ) A  ORDER BY A.CreatedDate asc";
                        lstSurveyLog = _connection.Query<TaskAuditLog>(query, new { SurveyID = SurveyID }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyLog;
        }
        #endregion GetSurveyAuditLog  

        #region GetBeneficiaryData
        /// <summary>
        /// Function used to fetch beneficiary details based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetBeneficiaryData(int SurveyID = 0)
        {
            List<KeyValuePair<int, string>> lstbeneficiary = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (SurveyID != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        var BeneficiaryTypeID = 0;
                        string strQuery = @"SELECT BeneficiaryTypeID FROM TbProjectSurvey WHERE SurveyID=@SurveyID;";
                        BeneficiaryTypeID = _connection.Query<int>(strQuery, new { SurveyID = SurveyID }).Single();

                        string query = @"SELECT t.BeneficiaryID, t.BeneficiaryName FROM TbProjectBeneficiary t ";
                        query += " WHERE t.BeneficiaryTypeID = @BeneficiaryTypeID AND IFNULL(t.IsActive,0)=@IsActive and IFNULL(t.IsDeleted,0)=@IsDeleted;";
                        obj = _connection.Query<object>(query, new { BeneficiaryTypeID = BeneficiaryTypeID, IsActive = 1, IsDeleted = 0 }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstbeneficiary.Add(new KeyValuePair<int, string>(item.BeneficiaryID, item.BeneficiaryName));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstbeneficiary;
        }
        #endregion GetBeneficiaryData  

        #region GetProjectWorkFlow
        /// <summary>
        /// Function used to fetch project workflow details based on projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetProjectWorkFlow(int projectId = 0)
        {
            List<KeyValuePair<int, string>> lstWorkFlow = new List<KeyValuePair<int, string>>();
            dynamic obj;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"SELECT WorkflowID,WfName FROM TbProjectWorkflow ";
                    query += " WHERE IFNULL(IsDeleted,0)=@IsDeleted AND ProjectId = @ProjectId;";
                    obj = _connection.Query<object>(query, new { ProjectId = projectId, IsDeleted = 0 }).ToList();

                    foreach (dynamic item in obj)
                    {
                        lstWorkFlow.Add(new KeyValuePair<int, string>(item.WorkflowID, item.WfName));
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return lstWorkFlow;
        }
        #endregion GetProjectBeneficiaryType        

        #region DeleteProjectSurveyUser
        /// <summary>
        /// Function used to delete project survey user based on SurveyID,loggedUserId
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteProjectSurveyUser(int SurveyID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbProjectSurveyUser SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId ";
                    strQuery += " Where SurveyID =@SurveyID ";
                    _connection.Execute(strQuery, new { SurveyID = SurveyID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteProjectSurveyUser

        #region AssignUserstoSurvey
        /// <summary>
        /// Function used to save project survey users
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool AssignUserstoSurvey(ProjectSurvey objProjectSurvey, int loggeduserid, int syncDBId, bool IsAssignUser = false)
        {
            string userIds = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    if (!string.IsNullOrEmpty(objProjectSurvey.AssignedUsers))
                    {
                        string strUserObject = GetSurveyAssignedUsers(objProjectSurvey.SurveyID);
                        bool success = DeleteProjectSurveyUser(objProjectSurvey.SurveyID, loggeduserid);
                        string query = string.Empty;
                        var id = 0;
                        List<UsersDetails> lstNotifyUser = JsonConvert.DeserializeObject<List<UsersDetails>>(objProjectSurvey.AssignedUsers);
                        List<UsersDetails> lstNotifyUser1 = JsonConvert.DeserializeObject<List<UsersDetails>>(strUserObject);
                        List<UsersDetails> lstNotifyUser2 = new List<UsersDetails>();

                        foreach (UsersDetails userItem in lstNotifyUser)
                        {
                            SaveProjectSurveyUser(objProjectSurvey.SurveyID, userItem.ID, loggeduserid);

                            if (IsAssignUser)
                            {
                                lstNotifyUser2 = lstNotifyUser1.Where(x => x.ID == userItem.ID).ToList();

                                if (lstNotifyUser2 == null || lstNotifyUser2.Count == 0)
                                {
                                    if (userIds.Length > 0)
                                    {
                                        userIds += "," + userItem.ID;
                                    }
                                    else
                                    {
                                        userIds = userItem.ID.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (userIds.Length > 0)
                                {
                                    userIds += "," + userItem.ID;
                                }
                                else
                                {
                                    userIds = userItem.ID.ToString();
                                }
                            }

                            //if (objProjectSurvey.Notify)
                            //{

                            //}

                            query = "Select Name  From TbUser Where ID =@UserID ";
                            var AssgName = _connection.Query<string>(query, new { UserID = userItem.ID }).Single();
                            query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy,TaskSyncDbID)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy,@TaskSyncDbID); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(query, new
                            {
                                ProjectID = objProjectSurvey.ProjectId,
                                TaskType = "Survey",
                                TaskTypeID = objProjectSurvey.SurveyID,
                                TypeIDColumn = "SurveyId",
                                AssignedTo = userItem.ID,
                                AssignedBy = loggeduserid,
                                Message = "Assigned the task to " + AssgName,
                                createdby = loggeduserid,
                                TaskSyncDbID = syncDBId,
                            }).Single();
                        }
                    }
                    _Tran.Complete();
                }

                EmailDAL emailDAL = new EmailDAL();
                EmailActivity objEmailActivity = new EmailActivity();
                SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
                objSurveyEmailTemplate = emailDAL.GetTemplateDetailsForTaskAsssigned(objProjectSurvey.SurveyID, loggeduserid, userIds);

                bool bStatus = objEmailActivity.SendMail("TaskAssigned", objSurveyEmailTemplate);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion AssignUserstoSurvey

        #region GetSurveyAssignedUsers
        /// <summary>
        /// Function used to fetch survey assigned users based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public string GetSurveyAssignedUsers(int SurveyID = 0)
        {
            string AssignedUsers = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select usr.Name,srvusr.UserID as ID , IFNULL(usr.ProfileImage,'') as ProfileImage from TbProjectSurveyUser srvusr inner join TbUser usr on srvusr.UserID=usr.ID ";
                    query += "where srvusr.SurveyID=@ID and srvusr.IsDeleted=0  Order By srvusr.ID Desc";

                    var asndUsers = _connection.Query<UsersDetails>(query, new
                    {
                        ID = SurveyID
                    }).ToList();

                    AssignedUsers = JsonConvert.SerializeObject(asndUsers);
                }
            }
            catch (Exception ex)
            {
            }
            return AssignedUsers;
        }
        #endregion GetSurveyAssignedUsers

        #region UpdateWorkflow
        /// <summary>
        /// Function used to update workflow based on SurveyID
        /// </summary>
        /// <param name="WorkFlowID"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public bool UpdateWorkflow(int WorkFlowID, int loggedUserId, int SurveyID)
        {
            string query = string.Empty;
            bool success = false;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbProjectSurvey SET  WorkflowID = @WorkflowID";
                        query += " ,UpdatedDate = @updatedDate, UpdatedBy=@userID where SurveyID = @SurveyID; ";
                        var result = _connection.Execute(query, new
                        {
                            SurveyID = SurveyID,
                            WorkflowID = WorkFlowID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });

                        string Qeury = @"Select WfNoOfLevels  From  TbProjectWorkflow  Where IsDeleted = 0 And WorkflowID = @WorkflowID";
                        var WfNoOfLevels = _connection.Query<int>(Qeury, new { WorkflowID = WorkFlowID }).FirstOrDefault();

                        ////Update To SyncDB
                        string strQuery = @"Update TaskSyncDB SET  WorkflowID =@WorkflowID,TotalWFLevel =@TotalWFLevel WHERE TypeID = @TypeID  And Type =@Type ";
                        var syncDBId = _connection.Execute(strQuery, new
                        {
                            Type = "Survey",
                            TypeID = SurveyID,
                            WorkflowID = WorkFlowID,
                            TotalWFLevel = WfNoOfLevels

                        });


                        if (result == 1)
                        {
                            success = true;
                        }

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        #endregion UpdateWorkflow

        #region  GetWorkFlowIdByName
        /// <summary>
        /// Function used to fetch workflowID based on ProjectId,WorkflowName
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="WorkflowName"></param>
        /// <returns></returns>
        public int GetWorkFlowIdByName(int ProjectId, string WorkflowName)
        {
            int workflowId = 0;
            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "SELECT WorkflowID FROM TbProjectWorkflow WHERE LOWER(TRIM(WfName)) = @WfName AND ProjectId = @ProjectId AND IFNULL(IsDeleted,0)=@IsDeleted;";
                    workflowId = _connection.Query<int>(query, new { WfName = WorkflowName.ToLower(), ProjectId = ProjectId, IsDeleted = 0 }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return workflowId;
        }
        #endregion GetWorkFlowIdByName

        #region  GetWorkFlowIdByName
        /// <summary>
        /// Function used to fetch BeneficiaryTypeID based on ProjectId,BeneficiaryType
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="BeneficiaryType"></param>
        /// <returns></returns>
        public int GetBeneficiaryTypeIdByName(int ProjectId, string BeneficiaryType)
        {
            int BeneficiaryTypeID = 0;
            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "SELECT BeneficiaryTypeID FROM TbProjectBeneficiaryType WHERE LOWER(TRIM(`Name`)) = @BeneficiaryTypeName AND ProjectID = @ProjectId AND IFNULL(IsDeleted,0)=@IsDeleted;";
                    BeneficiaryTypeID = _connection.Query<int>(query, new { BeneficiaryTypeName = BeneficiaryType.ToLower(), ProjectId = ProjectId, IsDeleted = 0 }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return BeneficiaryTypeID;
        }
        #endregion GetBeneficiaryTypeIdByName

        #region UpdateSurveyBeneficiaryType
        /// <summary>
        /// Function used to update beneficiary type based on SurveyID
        /// </summary>
        /// <param name="benficiaryTypeId"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public bool UpdateSurveyBeneficiaryType(int benficiaryTypeId, int loggedUserId, int SurveyID)
        {
            string query = string.Empty;
            bool success = false;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProjectSurvey SET  BeneficiaryTypeID = @BeneficiaryTypeID";
                    query += " ,UpdatedDate = @updatedDate, UpdatedBy=@userID where SurveyID = @SurveyID; ";
                    var result = _connection.Execute(query, new
                    {
                        SurveyID = SurveyID,
                        BeneficiaryTypeID = benficiaryTypeId,
                        updatedDate = DateTime.Now,
                        userID = loggedUserId
                    });

                    if (result == 1)
                    {
                        success = true;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        #endregion UpdateSurveyBeneficiaryType

        #region AssignTagstoSurvey
        /// <summary>
        /// Function used to save project survey tag
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool AssignTagstoSurvey(ProjectSurvey objProjectSurvey, int loggeduserid)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    if (!string.IsNullOrEmpty(objProjectSurvey.AssignedTags))
                    {
                        bool success = DeleteProjectSurveyTag(objProjectSurvey.SurveyID, loggeduserid);

                        List<Tags> lstNotifyUser = JsonConvert.DeserializeObject<List<Tags>>(objProjectSurvey.AssignedTags);
                        foreach (Tags tagItem in lstNotifyUser)
                        {
                            SaveProjectSurveyTag(objProjectSurvey.SurveyID, tagItem.TagID, loggeduserid);
                        }
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion AssignTagstoSurvey

        #region GetSurveyAssignedTags
        /// <summary>
        /// Function used to get project survey tags based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public string GetSurveyAssignedTags(int SurveyID = 0)
        {
            string AssignedTags = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select tgs.TagName,srvusr.TagID,tgs.TagColor from TbProjectSurveyTag srvusr inner join TbProjectTags tgs on srvusr.TagID=tgs.TagID ";
                    query += "where srvusr.SurveyID=@ID and srvusr.IsDeleted=0  Order By srvusr.ID Desc";

                    var asndTags = _connection.Query<Tags>(query, new
                    {
                        ID = SurveyID
                    }).ToList();

                    AssignedTags = JsonConvert.SerializeObject(asndTags);
                }
            }
            catch (Exception ex)
            {
            }
            return AssignedTags;
        }
        #endregion GetSurveyAssignedTags

        #region DeleteProjectSurveyTag
        /// <summary>
        /// Function used to delete project survey tags based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteProjectSurveyTag(int SurveyID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbProjectSurveyTag SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId ";
                    strQuery += " Where SurveyID =@SurveyID ";
                    _connection.Execute(strQuery, new { SurveyID = SurveyID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteProjectSurveyTag

        #region GenerateReportingFrequencyTrackDate
        /// <summary>
        /// GenerateReportingFrequencyTrackDate
        /// </summary>
        /// <param name="objProjectSurvey"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool GenerateReportingFrequencyTrackDate(ProjectSurvey objProjectSurvey, int loggedUserId, DateTime? StartDate, DateTime? EndDate)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    bool isDeleted = DeleteProjectSurveyTrackDate(objProjectSurvey.SurveyID);

                    switch (objProjectSurvey.ReportingFrequency)
                    {
                        case (int)CommonActivity.ReportingFrequency.Once:
                            DateTime? reportvalue = CommonActivity.GetDateTimeValue(objProjectSurvey.ReportingOnceDate);
                            lstTrackDates.Add(reportvalue.Value.ToString("yyyy-MM-dd"));
                            break;
                        case (int)CommonActivity.ReportingFrequency.Daily:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, 0, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Weekly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingWeek, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Monthly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingMonthDay, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Quarterly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingMonthDateDay, objProjectSurvey.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingMonthDateDay, objProjectSurvey.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(StartDate, EndDate, objProjectSurvey.ReportingFrequency, objProjectSurvey.ReportingMonthDateDay, objProjectSurvey.ReportingMonth);
                            break;
                    }

                    if (lstTrackDates != null && lstTrackDates.Count > 0)
                    {
                        query = "INSERT INTO TbProjectSurveyTrackDate(TrackDate,SurveyID,TrackStatus) ";

                        for (int i = 0; i < lstTrackDates.Count; i++)
                        {
                            query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + objProjectSurvey.SurveyID + ",0" : " SELECT '" + lstTrackDates[i] + "'," + objProjectSurvey.SurveyID + ",0";
                        }

                        if (query.Length > 0)
                        {
                            using (_connection = Utils.Database.GetDBConnection())
                            {
                                var result = _connection.Execute(query);

                                if (result > 0)
                                {
                                    success = true;
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

            return success;
        }
        #endregion GenerateReportingFrequencyTrackDate

        #region DeleteProjectSurveyTrackDate
        /// <summary>
        /// Function used to delete project survey track date based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public bool DeleteProjectSurveyTrackDate(int SurveyID)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "DELETE FROM TbProjectSurveyTrackDate WHERE SurveyID=@SurveyID ";
                    _connection.Execute(strQuery, new { SurveyID = SurveyID });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteProjectSurveyTrackDate

        #region GetProjectDate
        /// <summary>
        /// Function used to fetch project startdate & enddate based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public dynamic GetProjectDate(int ProjectId)
        {
            dynamic obj;

            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQeury = @"SELECT StartDate,EndDate FROM TbProject WHERE ProjectId = @ProjectId";
                obj = _connection.Query<object>(strQeury, new { ProjectId = ProjectId }).FirstOrDefault();
            }
            return obj;

        }
        #endregion GetProjectDate

        /// <summary>
        /// Function used to fetch survey date based on SurvId
        /// </summary>
        /// <param name="SurvId"></param>
        /// <returns></returns>
        public dynamic GetSurveyDate(int SurvId)
        {
            dynamic obj;
            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQeury = @"SELECT date_format(StartDate,'%Y-%m-%d')StartDate,date_format(EndDate,'%Y-%m-%d')EndDate,StartDate StartDateD,EndDate EndDateD FROM TbProjectSurvey WHERE SurveyID = @SurveyID and IsActive =@IsActive";
                obj = _connection.Query<object>(strQeury, new { SurveyID = SurvId, IsActive = 1 }).FirstOrDefault();
            }
            return obj;

        }

        /// <summary>
        /// Function used to save DuplicateProjectSurvey based on NewSurveyID,OldSurveyID
        /// </summary>
        /// <param name="NewSurveyID"></param>
        /// <param name="OldSurveyID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool SaveDuplicateProjectSurvey(int NewSurveyID, int OldSurveyID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            var resultStatus = true;
            ProjectSurvey objProjectSurvey = new ProjectSurvey();

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //strQuery = "sp_CopySurveySectionQuestions";
                        //id = _connection.ExecuteScalar<int>(strQuery, new { NewSurveyID = NewSurveyID, OldSurveyID = OldSurveyID, loggedUserId = loggedUserId }, commandType: CommandType.StoredProcedure);

                        //strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        //var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = NewSurveyID }).Single();
                        //strQuery = "Select SurveyName  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        //var name = _connection.Query<string>(strQuery, new { SurveyId = NewSurveyID }).Single();

                        // inserting sections
                        string strSectionQuery = " INSERT INTO TbProjectSurveySection(SectionName,SectionDescription,SectionOrder,SurveyID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy) ";
                        strSectionQuery += " SELECT SectionName,SectionDescription,SectionOrder,@NewSurveyId,IsActive,NOW(),null,@loggedUserId,null from TbProjectSurveySection where SurveyID = @oldSurveyId ORDER BY SectionID ASC;";
                        _connection.Query<int>(strSectionQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                oldSurveyId = OldSurveyID,
                                loggedUserId = loggedUserId
                            });

                        // inserting Question
                        string strQuestionQuery = "INSERT INTO TbProjectSurveyQuestion(QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,SurveyID,SectionID,CreatedBy,CreatedDate,IsConditionQuestion,IsRepeatSection,RepeatSectionId,IsUnique,IsSearchable,MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables,HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,IsOtherOptionSelected) ";
                        strQuestionQuery += " select QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,@NewSurveyId,SectionID,@loggedUserId,now(),IsConditionQuestion,IsRepeatSection,";
                        strQuestionQuery += "RepeatSectionId,IsUnique,IsSearchable,MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,";
                        strQuestionQuery += "FormulaExpression,FormulaOperation,FormulaFieldVariables,HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,IsOtherOptionSelected";
                        strQuestionQuery += "  FROM TbProjectSurveyQuestion   where  SurveyID = @oldSurveyId order by QuestionID ASC;";

                        _connection.Query<int>(strQuestionQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                oldSurveyId = OldSurveyID,
                                loggedUserId = loggedUserId
                            });

                        // inserting Question option
                        string strQuestionOptionQuery = " INSERT INTO TbProjectSurveyQSTOption(OptionValue,QuestionID,CreatedBy,CreatedDate)";
                        strQuestionOptionQuery += " SELECT OptionValue,QuestionID,@NewSurveyId,NOW() FROM TbProjectSurveyQSTOption WHERE  QuestionID IN ";
                        strQuestionOptionQuery += " (SELECT QuestionID FROM TbProjectSurveyQuestion WHERE SurveyID = @oldSurveyId) AND length(IFNULL(OptionValue,'')) >0;";

                        _connection.Query<int>(strQuestionOptionQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                oldSurveyId = OldSurveyID,
                                loggedUserId = loggedUserId
                            });

                        // inserting SurveySectionLogic
                        string strSurveySectionLogicQuery = " INSERT INTO TbProjectSurveySectionLogic(SurveyID,SectionID,LogicType,QuestionID,ConditionID,Answer,LogicOperator,CreatedDate,CreatedBy,IsActive)";
                        strSurveySectionLogicQuery += " SELECT @NewSurveyId,SectionID,LogicType,QuestionID,ConditionID,Answer,LogicOperator,NOW(),@NewSurveyId,IsActive FROM TbProjectSurveySectionLogic WHERE SurveyID = @oldSurveyId;";
                        _connection.Query<int>(strSurveySectionLogicQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                oldSurveyId = OldSurveyID,
                                loggedUserId = loggedUserId
                            });


                        // to get old var and new var 
                        List<SurveyQuestionVariable> lstSurveyQuestionVariable = new List<SurveyQuestionVariable>();
                        string strQstVariable = @"select concat('var_',QuestionID) as NewVariable ,FieldVar as OldVariable from TbProjectSurveyQuestion where SurveyID =@SurveyId";
                        lstSurveyQuestionVariable = _connection.Query<SurveyQuestionVariable>(strQstVariable, new
                        {
                            SurveyId = NewSurveyID
                        }).ToList();


                        // update fieldvar
                        string strfieldvarQuery = "UPDATE TbProjectSurveyQuestion SET FieldVar  = CONCAT('var_',QuestionID) WHERE SurveyID = @NewSurveyId;";
                        _connection.Query<int>(strfieldvarQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID
                            });

                        // to get IsFormulaQuestion  List 
                        List<SurveyQuestion> lstSurveyFormulaQuestion = new List<SurveyQuestion>();
                        string strFormulaQST = @"select QuestionID,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables from TbProjectSurveyQuestion where IsFormulaQuestion = 1 and SurveyID = @SurveyId and ifnull(IsActive,0) = 1";
                        lstSurveyFormulaQuestion = _connection.Query<SurveyQuestion>(strFormulaQST, new
                        {
                            SurveyId = NewSurveyID
                        }).ToList();

                        // update FormulaExpression,FormulaFieldVariables
                        if (lstSurveyFormulaQuestion != null && lstSurveyFormulaQuestion.Count > 0 &&
                            lstSurveyQuestionVariable != null && lstSurveyQuestionVariable.Count > 0)
                        {
                            foreach (var item in lstSurveyFormulaQuestion)
                            {
                                string FormulaExpression = item.FormulaExpression;
                                string FormulaFieldVariables = item.FormulaFieldVariables;

                                if (FormulaFieldVariables != null && FormulaFieldVariables.Length > 0)
                                {
                                    string[] spltFormulaFieldVariables = FormulaFieldVariables.Split(",");

                                    foreach (var frmitm in spltFormulaFieldVariables)
                                    {
                                        List<SurveyQuestionVariable> lstSurveyQuestionVariable1 = new List<SurveyQuestionVariable>();

                                        lstSurveyQuestionVariable1 = lstSurveyQuestionVariable.Where(x => x.OldVariable == frmitm).ToList();

                                        if (lstSurveyQuestionVariable1 != null && lstSurveyQuestionVariable1.Count > 0)
                                        {
                                            FormulaExpression = FormulaExpression.Replace(lstSurveyQuestionVariable1[0].OldVariable, lstSurveyQuestionVariable1[0].NewVariable);
                                            FormulaFieldVariables = FormulaFieldVariables.Replace(lstSurveyQuestionVariable1[0].OldVariable, lstSurveyQuestionVariable1[0].NewVariable);
                                        }
                                    }
                                }

                                string strUpdateFormulaExp = "update TbProjectSurveyQuestion set FormulaExpression = @FormulaExpression, FormulaFieldVariables = @FormulaFieldVariables where  SurveyID = @NewSurveyId and  QuestionID = @QuestionID";
                                _connection.Query<int>(strUpdateFormulaExp,
                                    new
                                    {
                                        NewSurveyId = NewSurveyID,
                                        QuestionID = item.QuestionID,
                                        FormulaExpression = FormulaExpression,
                                        FormulaFieldVariables = FormulaFieldVariables
                                    });
                            }
                        }

                        // insert TbProjectSurveyQSTCondition
                        string strQSTConditionrQuery = "Insert Into TbProjectSurveyQSTCondition (QuestionID,ValidationCondition, ConditionValue, MinValue, `MaxValue`,CreatedBy,CreatedDate)";
                        strQSTConditionrQuery += " SELECT QuestionID,ValidationCondition, ConditionValue, MinValue, `MaxValue`,@NewSurveyId,NOW() FROM TbProjectSurveyQSTCondition WHERE QuestionID ";
                        strQSTConditionrQuery += " IN (SELECT QuestionID FROM TbProjectSurveyQuestion WHERE SurveyID = @oldSurveyId);";

                        _connection.Query<int>(strQSTConditionrQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                loggedUserId = loggedUserId,
                                oldSurveyId = OldSurveyID,
                            });

                        // insert SurveyCloneHistory
                        string strSurveyCloneHistoryQuery = "Insert Into TbProjectSurveyCloneHistory(OldSurveyID,NewSurveyID,CreatedBy,CreatedDate)";
                        strSurveyCloneHistoryQuery += "   Select @oldSurveyId,@NewSurveyId,@loggedUserId,now(); ";

                        _connection.Query<int>(strSurveyCloneHistoryQuery,
                            new
                            {
                                NewSurveyId = NewSurveyID,
                                loggedUserId = loggedUserId,
                                oldSurveyId = OldSurveyID,
                            });


                        strQuery = @"Select ProjectId,SurveyName From TbProjectSurvey Where SurveyID =@SurveyId";
                        objProjectSurvey = _connection.Query<ProjectSurvey>(strQuery, new
                        {
                            SurveyId = NewSurveyID
                        }).FirstOrDefault();


                        strQuery = "Select SurveyName  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        var nameOld = _connection.Query<string>(strQuery, new { SurveyId = OldSurveyID }).Single();


                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = objProjectSurvey.ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = NewSurveyID;
                        objLog.FromIDColumnName = "SurveyID";
                        objLog.Message = "Added new Survey named " + objProjectSurvey.SurveyName + " duplicate of  '" + nameOld + "'";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                resultStatus = false;
                throw ex;
            }

            return resultStatus;
        }

        #endregion SurveyLanding

        #region SurveySection

        #region Get

        /// <summary>
        /// Function used to fetch project workspace theme based on ProjectID,WorkspaceID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<InitUserList> GetProjectTeam(int ProjectID, int WorkspaceID)
        {
            List<InitUserList> lstUsers = new List<InitUserList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select Distinct PT.UserID , U.Name,ifnull(U.ProfileImage,'') as ProfileImage,U.EmailId , WR.RoleId RoleID, WR.RoleName UserRole From TbUser U Inner join " +
                                " (Select Distinct t.UserID from TbUserAssignedToGroup t Where exists (Select p.GroupID from TbProject p Where p.ProjectId= @ProjectID and t.GroupID = p.GroupID)) PT On PT.UserID =U.ID " +
                                " Inner Join (Select UserID,W.RoleID,RoleName From TbWorkspaceTeam W Inner Join TbRole R On R.RoleId=W.RoleId Where W.WorkspaceID=@WorkspaceID) WR  On WR.UserID=PT.UserID ";

                    lstUsers = _connection.Query<InitUserList>(strQuery, new { ProjectID = ProjectID, WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to fetch all question types 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionTypes> GetAllQuestionTypes()
        {
            List<QuestionTypes> lst = new List<QuestionTypes>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select ID,Type,Category QuestionCategory,IsCalcavailable,IsValavailable From TbQuestionType Order By Type";
                    lst = _connection.Query<QuestionTypes>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        /// <summary>
        /// Function used to fetch all cascading question options
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CascadingQuestionOptions> GetCascadingQuestionOptions()
        {
            List<CascadingQuestionOptions> lst = new List<CascadingQuestionOptions>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select ID,OptionValue  From TbSurveyCascadingQuestionOptions Where IsActive =1 ";
                    lst = _connection.Query<CascadingQuestionOptions>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        /// <summary>
        /// Function used to fetch all cascading question suboptions based on ParentOptionID
        /// </summary>
        /// <param name="ParentOptionID"></param>
        /// <returns></returns>
        public IEnumerable<CascadingQuestionOptions> GetCascadingQuestionSubOptions(int ParentOptionID)
        {
            List<CascadingQuestionOptions> lst = new List<CascadingQuestionOptions>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select B.HierarchyOrder,B.OptionValue,B.TableName, B.ColumnName, B.PrimaryColumn, B.ParentItemColumn 
                            From TbSurveyCascadingQuestionOptions A Left Join TbSurveyCascadingQuestionOptions B On A.OptionGroupID =B.OptionGroupID
                            Where B.IsActive =1 And B.IsDeleted =0 And A.ID =@ParentOptionID And A.HierarchyOrder <=B.HierarchyOrder  Order By B.HierarchyOrder";
                    lst = _connection.Query<CascadingQuestionOptions>(strQuery, new { ParentOptionID = ParentOptionID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }
        #region Section

        #region SectionLogic

        /// <summary>
        /// Function used to fetch project logic conditions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogicConditions> GetAllLogicConditions()
        {
            List<LogicConditions> lst = new List<LogicConditions>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select ID ConditionID,`Condition`,ConditionValue From TbProjectLogicConditions ";
                    lst = _connection.Query<LogicConditions>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        /// <summary>
        /// Function used to fetch survey question's answers based on QstnID
        /// </summary>
        /// <param name="QstnID"></param>
        /// <returns></returns>
        public IEnumerable<QuestionOptionList> GetLogicAnswers(int QstnID)
        {
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select OptionID,OptionValue From TbProjectSurveyQSTOption Where QuestionID=@QuestionID ";
                    lst = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = QstnID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        /// <summary>
        /// Function used to fetch section Skip & Repeat logic datas
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="SectionID"></param>
        /// <param name="LogicType"></param>
        /// <returns></returns>
        public IEnumerable<SurveySectionLogic> GetAllQstnsForSectionLogic(int SurveyID, int SectionID, string LogicType)
        {
            List<SurveySectionLogic> lst = new List<SurveySectionLogic>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (LogicType == "Skip Logic")
                        strQuery = @"Select Q.QuestionID,Q.QuestionName,Q.QuestionTypeID,QT.Type QuestionType, QT.Category QuestionCategory From TbProjectSurveyQuestion Q 
                                Left Join TbQuestionType QT On QT.ID=Q.QuestionTypeID  Where  Q.IsActive =1 and Q.SurveyID =@SurveyID and QT.Category != 3 and  Q.QuestionTypeID not in (8,13) And  Q.SectionID!= @SectionID";
                    else if (LogicType == "Repeat Logic")
                        strQuery = @"Select Q.QuestionID,Q.QuestionName,Q.QuestionTypeID,QT.Type QuestionType, QT.Category QuestionCategory From TbProjectSurveyQuestion Q 
                                Left Join TbQuestionType QT On QT.ID=Q.QuestionTypeID  Where Q.QuestionTypeID =5 And Q.IsActive =1 and Q.SurveyID =@SurveyID and Q.SectionID!= @SectionID";

                    lst = _connection.Query<SurveySectionLogic>(strQuery, new { SurveyID = SurveyID, SectionID = SectionID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;

        }

        /// <summary>
        /// Function used to fetch section logic datas
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="SectionID"></param>
        /// <returns></returns>
        public IEnumerable<SurveySectionLogic> GetSectionsLogic(int SurveyID, int SectionID)
        {
            List<SurveySectionLogic> lst = new List<SurveySectionLogic>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select L.SectionLogicID , L.LogicType, L.QuestionID, L.ConditionID, L.Answer,L.LogicOperator From TbProjectSurveySectionLogic L 
                            Inner Join TbProjectSurveyQuestion Q On Q.QuestionID =L.QuestionID Left Join  TbProjectLogicConditions LC On LC.ID =L.ConditionID
                            Where L.IsActive =1 and L.SurveyID =@SurveyID and L.SectionID= @SectionID and L.IsActive = 1";
                    lst = _connection.Query<SurveySectionLogic>(strQuery, new { SurveyID = SurveyID, SectionID = SectionID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;

        }

        #endregion

        /// <summary>
        /// Function used to fetch all section based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectSurveySection> GetAllSectionsBySurveyID(int SurveyID)
        {
            List<ProjectSurveySection> lstSurveySec = new List<ProjectSurveySection>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    List<SurveyQuestion> lstSurveyQuestion = new List<SurveyQuestion>();
                    List<QuestionOptionList> lstQuestionOptionList = new List<QuestionOptionList>();
                    List<QuestionCondition> lstQuestionCondition = new List<QuestionCondition>();

                    strQuery = @"SELECT SectionID, SectionName, ifnull(SectionDescription, '') as SectionDescription, SectionOrder FROM TbProjectSurveySection Where SurveyID = @SurveyID 
                            AND IFNULL(IsActive,0)=@IsActive Order By SectionOrder";
                    lstSurveySec = _connection.Query<ProjectSurveySection>(strQuery, new { SurveyID = SurveyID, IsActive = 1 }).ToList();

                    //// TO GET SURVEYQUESTION FOR SURVEYID
                    strQuery = @"SELECT SQ.SurveyID,SQ.SectionID ,SQ.QuestionID,SQ.QuestionName,SQ.QuestionTypeID,Q.Type QuestionType,Q.Category QuestionCategory," +
                                 " Q.IsCalcavailable,Q.IsValavailable,SQ.QuestionOrder,SQ.QuestionHint,SQ.DefaultValue,SQ.IsUnique,SQ.IsSearchable,SQ.IsMandatory," +
                                 " SQ.IsFormulaQuestion,SQ.FormulaOperation,SQ.FormulaExpression,SQ.FormulaFieldVariables,SQ.RegEx,SQ.ErrorText,SQ.FieldVar,SQ.UOM,ifnull(SQ.IsOtherOptionSelected,0) IsOtherOptionSelected" +
                                 " FROM TbProjectSurveyQuestion SQ Inner Join TbQuestionType Q On Q.ID =SQ.QuestionTypeID" +
                                 " WHERE IFNULL(SQ.IsActive,0)= 1 And SQ.SurveyID = @SurveyID Order By SQ.QuestionOrder ";
                    lstSurveyQuestion = _connection.Query<SurveyQuestion>(strQuery, new { SurveyID = SurveyID }).ToList();

                    //// TO GET SURVEYQUESTION OPTION FOR SURVEYID
                    strQuery = "SELECT opt.QuestionID, opt.OptionID,opt.OptionValue FROM TbProjectSurveyQSTOption opt JOIN TbProjectSurveyQuestion qst ON opt.QuestionID =  qst.QuestionID ";
                    strQuery += "WHERE qst.SurveyID = @SurveyID AND opt.OptionValue != CASE WHEN ifnull(qst.IsOtherOptionSelected,0) = 1 THEN 'Others' ELSE '' END; ";
                    lstQuestionOptionList = _connection.Query<QuestionOptionList>(strQuery, new { SurveyID = SurveyID }).ToList();

                    //// TO GET SURVEYQUESTION CONDITION FOR SURVEYID
                    strQuery = @"SELECT QSTCON.QuestionID, QSTCON.ValidationCondition,QSTCON.MinValue,QSTCON.`MaxValue`,QSTCON.ConditionValue,QSTCON.LogicOperator ";
                    strQuery += " From TbProjectSurveyQSTCondition QSTCON JOIN TbProjectSurveyQuestion QST ON  QST.QuestionID = QSTCON.QuestionID  WHERE IFNULL(QST.IsActive,0)= 1 AND QST.SurveyID  = @SurveyID; ";
                    lstQuestionCondition = _connection.Query<QuestionCondition>(strQuery, new { SurveyID = SurveyID }).ToList();


                    foreach (ProjectSurveySection secItem in lstSurveySec)
                    {
                        secItem.SurveySecQuestions = new List<SurveyQuestion>();
                        secItem.SurveySecQuestions = lstSurveyQuestion.Where(x => x.SectionID == secItem.SectionID).ToList();

                        foreach (SurveyQuestion qstnItem in secItem.SurveySecQuestions)
                        {
                            qstnItem.lstQuestionOptions = new List<QuestionOptionList>();
                            qstnItem.lstQuestionOptions = lstQuestionOptionList.Where(x => x.QuestionID == qstnItem.QuestionID).ToList();

                            qstnItem.lstQuestionCondition = new List<QuestionCondition>();
                            qstnItem.lstQuestionCondition = lstQuestionCondition.Where(x => x.QuestionID == qstnItem.QuestionID).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveySec;
        }

        #endregion

        #region Question

        /// <summary>
        /// Function used to fetch all question details based on QuestionID
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public IEnumerable<SurveyQuestion> GetQuestionDetails(int QuestionID)
        {
            List<SurveyQuestion> lstSurveyQstns = new List<SurveyQuestion>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select SQ.SurveyID,SQ.SectionID,SQ.QuestionID,SQ.QuestionName,SQ.QuestionTypeID,Q.Type QuestionType,Q.Category QuestionCategory,
                        SQ.QuestionOrder,SQ.QuestionHint,SQ.DefaultValue,SQ.IsUnique,SQ.IsSearchable,SQ.IsMandatory,SQ.FormulaOperation,SQ.FormulaExpression,
                        SQ.IsFormulaQuestion,SQ.FormulaFieldVariables,SQ.RegEx,SQ.ErrorText,SQ.FieldVar,IFNULL(SQ.IsOtherOptionSelected,0) IsOtherOptionSelected From TbProjectSurveyQuestion SQ Inner Join TbQuestionType Q On Q.ID =SQ.QuestionTypeID Where SQ.QuestionID= @QuestionID Order by SQ.QuestionOrder";
                    lstSurveyQstns = _connection.Query<SurveyQuestion>(strQuery, new { QuestionID = QuestionID }).ToList();

                    lstSurveyQstns[0].lstQuestionOptions = new List<QuestionOptionList>();
                    strQuery = "SELECT opt.OptionID,opt.OptionValue FROM TbProjectSurveyQSTOption opt JOIN TbProjectSurveyQuestion qst ON opt.QuestionID =  qst.QuestionID ";
                    strQuery += "WHERE qst.QuestionID = @QuestionID AND opt.OptionValue != CASE WHEN ifnull(qst.IsOtherOptionSelected,0) = 1 THEN 'Others' ELSE '' END; ";
                    lstSurveyQstns[0].lstQuestionOptions = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = QuestionID }).ToList();

                    lstSurveyQstns[0].lstQuestionCondition = new List<QuestionCondition>();
                    strQuery = @"Select ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator From TbProjectSurveyQSTCondition Where QuestionID = @QuestionID";
                    lstSurveyQstns[0].lstQuestionCondition = _connection.Query<QuestionCondition>(strQuery, new { QuestionID = QuestionID }).ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQstns;
        }
        #endregion

        #region Response

        /// <summary>
        /// Function used to question response based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IEnumerable<SurveyResponse> GetAllQstnsResponsesBySurveyID(int SurveyID, int offset = 0)
        {
            List<SurveyResponse> lstSurveyQstns = new List<SurveyResponse>();
            string strQuery = string.Empty;
            string[] ds;
            List<string> ResponseID;// = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select Distinct SQ.QuestionID, SQ.QuestionName, QT.Type as QuestionType,SQ.QuestionOrder,SQ.SectionID,SQ.SectionName,SQ.SectionDescription from
                        (select PSQ.QuestionID, PSQ.QuestionName, PSQ.QuestionOrder, PSQ.QuestionTypeID, PSS.SectionID, PSS.SectionName,PSS.SectionDescription from TbProjectSurveyQuestion PSQ inner join TbProjectSurveySection PSS on
                        PSQ.SectionID= PSS.SectionID WHERE IFNULL(PSQ.IsActive,0)= 1
                         And PSQ.SurveyID = @SurveyID order by PSQ.SectionID,PSQ.QuestionOrder
                        ) SQ inner join TbQuestionType QT on QT.ID = SQ.QuestionTypeID order by SQ.QuestionOrder";


                    lstSurveyQstns = _connection.Query<SurveyResponse>(strQuery, new { SurveyID = SurveyID, offset = offset }).ToList();
                    foreach (SurveyResponse qstnItem in lstSurveyQstns)
                    {
                        qstnItem.QuestionResponses = new List<SurveyResponses>();
                        strQuery = @"Select distinct ID,ResponseID, Answer,Date(Case When UpdatedDate IS Null  Then CreatedDate Else  UpdatedDate End)  UpdatedDate 
                                From TbProjectSurveyResponses Where IFNULL(IsActive,0)= 1 And QuestionID = @QuestionID";
                        qstnItem.QuestionResponses = _connection.Query<SurveyResponses>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        qstnItem.QuestionOptionList = new List<QuestionOptionList>();
                        strQuery = @"SELECT QuestionID, OptionID,OptionValue FROM TbProjectSurveyQSTOption WHERE QuestionID =@QuestionID ";
                        qstnItem.QuestionOptionList = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        string query = "SELECT IFNULL(ResponseID,0) as ResponseID, IFNULL(Answer,'') as Answer FROM TbProjectSurveyResponses WHERE QuestionID =@QuestionID and SectionID=@SectionID";
                        var strResponse = _connection.Query<AnswerList>(query, new { QuestionID = qstnItem.QuestionID, SectionID = qstnItem.SectionID }).ToList();

                        qstnItem.AnswerList = new List<AnswerList>();
                        strQuery = @"SELECT IFNULL(Answer,'') FROM TbProjectSurveyResponses WHERE QuestionID =@QuestionID";
                        var strDelivery = _connection.Query<string>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();


                        IEnumerable<string> strings = strDelivery;

                        Dictionary<string, int> result = strings.SelectMany(s => s.Split(','))
                                                               .GroupBy(s => s.Trim())
                                                               .ToDictionary(g => g.Key, g => g.Count());



                        var temp = GetObject(result, qstnItem.QuestionID);
                        for (int i = 0; i <= strResponse.Count - 1; i++)
                        {
                            if (strResponse[i].Answer.Contains(","))
                            {
                                string[] splitData = strResponse[i].Answer.Split(',');
                                for (int j = 0; j <= splitData.Length - 1; j++)
                                {
                                    if (temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).ResponseID > 0)
                                    {

                                        int Count = temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).count;
                                        string Answer = temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).Answer;
                                        int QuestionID = temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).QuestionID;
                                        temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).count = 1;
                                        temp.Add(new AnswerList { count = 1, Answer = Answer, QuestionID = QuestionID, ResponseID = strResponse[i].ResponseID });
                                    }
                                    else
                                    {
                                        temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).ResponseID = strResponse[i].ResponseID;
                                    }
                                }
                            }
                            else
                            {
                                temp.Find(x => x.Answer.Trim() == strResponse[i].Answer.Trim()).ResponseID = strResponse[i].ResponseID;
                            }

                        }
                        qstnItem.AnswerList = temp;


                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQstns;
        }

        /// <summary>
        /// Function used to fetch survey report details based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="offset"></param>
        /// <param name="questionID"></param>
        /// <returns></returns>
        public IEnumerable<SurveyResponse> GetAllResponsesBySurveyReport(int SurveyID, int offset = 0, int questionID = 0)
        {
            List<SurveyResponse> lstSurveyQstns = new List<SurveyResponse>();
            string strQuery = string.Empty;
            string[] ds;
            List<string> ResponseID;// = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select Distinct SQ.QuestionID, SQ.QuestionName, QT.Type as QuestionType,SQ.QuestionOrder,SQ.SectionID,SQ.SectionName,SQ.SectionDescription from
                        (select PSQ.QuestionID, PSQ.QuestionName, PSQ.QuestionOrder, PSQ.QuestionTypeID, PSS.SectionID, PSS.SectionName,PSS.SectionDescription from TbProjectSurveyQuestion PSQ inner join TbProjectSurveySection PSS on
                        PSQ.SectionID= PSS.SectionID WHERE IFNULL(PSQ.IsActive,0)= 1
                         And PSQ.SurveyID = @SurveyID And PSQ.QuestionID = @QuestionID order by PSQ.SectionID,PSQ.QuestionOrder
                        ) SQ inner join TbQuestionType QT on QT.ID = SQ.QuestionTypeID";


                    lstSurveyQstns = _connection.Query<SurveyResponse>(strQuery, new { SurveyID = SurveyID, offset = offset, QuestionID = questionID }).ToList();
                    foreach (SurveyResponse qstnItem in lstSurveyQstns)
                    {
                        qstnItem.QuestionResponses = new List<SurveyResponses>();
                        strQuery = @"Select distinct ID,ResponseID, Answer,Date(Case When UpdatedDate IS Null  Then CreatedDate Else  UpdatedDate End)  UpdatedDate 
                                From TbProjectSurveyResponses Where IFNULL(IsActive,0)= 1 And QuestionID = @QuestionID";
                        qstnItem.QuestionResponses = _connection.Query<SurveyResponses>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        qstnItem.QuestionOptionList = new List<QuestionOptionList>();
                        strQuery = @"SELECT QuestionID, OptionID,OptionValue FROM TbProjectSurveyQSTOption WHERE QuestionID =@QuestionID ";
                        qstnItem.QuestionOptionList = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        string query = "SELECT IFNULL(ResponseID,0) as ResponseID, IFNULL(Answer,'') as Answer FROM TbProjectSurveyResponses WHERE QuestionID =@QuestionID and SectionID=@SectionID";
                        var strResponse = _connection.Query<AnswerList>(query, new { QuestionID = qstnItem.QuestionID, SectionID = qstnItem.SectionID }).ToList();

                        qstnItem.AnswerList = new List<AnswerList>();
                        strQuery = @"SELECT IFNULL(Answer,'') FROM TbProjectSurveyResponses WHERE QuestionID =@QuestionID";
                        var strDelivery = _connection.Query<string>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        IEnumerable<string> strings = strDelivery;

                        Dictionary<string, int> result = strings.SelectMany(s => s.Split(','))
                                                               .GroupBy(s => s.Trim())
                                                               .ToDictionary(g => g.Key, g => g.Count());



                        var temp = GetObject(result, qstnItem.QuestionID);
                        for (int i = 0; i <= strResponse.Count - 1; i++)
                        {
                            if (strResponse[i].Answer.Contains(","))
                            {
                                string[] splitData = strResponse[i].Answer.Split(',');
                                for (int j = 0; j <= splitData.Length - 1; j++)
                                {
                                    if (temp.Find(x => x.Answer == splitData[j]).ResponseID > 0)
                                    {

                                        int Count = temp.Find(x => x.Answer == splitData[j]).count;
                                        string Answer = temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).Answer;
                                        int QuestionID = temp.Find(x => x.Answer == splitData[j]).QuestionID;
                                        temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).count = 1;
                                        temp.Add(new AnswerList { count = 1, Answer = Answer, QuestionID = QuestionID, ResponseID = strResponse[i].ResponseID });
                                    }
                                    else
                                    {
                                        temp.Find(x => x.Answer.Trim() == splitData[j].Trim()).ResponseID = strResponse[i].ResponseID;
                                    }
                                }
                            }
                            else
                            {
                                temp.Find(x => x.Answer.Trim() == strResponse[i].Answer.Trim()).ResponseID = strResponse[i].ResponseID;
                            }

                        }
                        qstnItem.AnswerList = temp;


                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQstns;
        }

        /// <summary>
        /// Fetch answer list details based on QuestionID
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        List<AnswerList> GetObject(Dictionary<string, int> dict, int QuestionID)
        {
            List<AnswerList> lstAnswerList = new List<AnswerList>();

            foreach (var kv in dict)
            {
                lstAnswerList.Add(new AnswerList { count = kv.Value, Answer = kv.Key, QuestionID = QuestionID, ResponseID = 0 });
            }
            return lstAnswerList;
        }

        /// <summary>
        /// Function used to fetch all question response details based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public IEnumerable<SurveyResponse> GetAllQstnsIndResponsesBySurveyID(int SurveyID)
        {
            List<SurveyResponse> lstSurveyQstnsRes = new List<SurveyResponse>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct Q.QuestionID,Q.QuestionName,Q.SectionID,PSS.SectionName,PSS.SectionDescription ,Q.QuestionOrder
                             FROM  TbProjectSurveyQuestion Q  inner join TbProjectSurveySection PSS on Q.SectionID=PSS.SectionID 
                             WHERE IFNULL(Q.IsActive,0)= 1 And Q.SurveyID = @SurveyID Order by Q.QuestionOrder ASC";
                    lstSurveyQstnsRes = _connection.Query<SurveyResponse>(strQuery, new { SurveyID = SurveyID }).ToList();
                    foreach (SurveyResponse respItem in lstSurveyQstnsRes)
                    {
                        respItem.QuestionResponses = new List<SurveyResponses>();

                        strQuery = @"Select Distinct R.ID,R.SurveyID,R.SectionID,R.ResponseID,R.SyncTaskID,Q.QuestionID ,Q.QuestionName,R.Answer,QT.Type as QuestionType ,Date(Case When R.UpdatedDate IS Null Then R.CreatedDate Else  R.UpdatedDate End)  UpdatedDate From TbProjectSurveyQuestion Q 
                                Left Join TbProjectSurveyResponses R On R.QuestionID =Q.QuestionID  And R.SurveyID=Q.SurveyID
                                inner join TbQuestionType QT on QT.ID =Q.QuestionTypeID
                                Where IFNULL(R.IsActive,0)= 1 And Q.SurveyID=@SurveyID And R.QuestionID =@QuestionID Order By Q.QuestionID ";
                        respItem.QuestionResponses = _connection.Query<SurveyResponses>(strQuery, new { SurveyID = SurveyID, QuestionID = respItem.QuestionID }).ToList();

                        respItem.QuestionOptionList = new List<QuestionOptionList>();
                        strQuery = @"SELECT QuestionID, OptionID,OptionValue FROM TbProjectSurveyQSTOption WHERE QuestionID =@QuestionID ";
                        respItem.QuestionOptionList = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = respItem.QuestionID }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQstnsRes;
        }
        public SurveyResponseDetails GetResponsesBySurveyID(int SurveyID)
        {
            List<SurveyResponse> lstSurveyQstnsRes = new List<SurveyResponse>();
            List<BeneficiaryQuestions> lstBeneficiaryQuestions = new List<BeneficiaryQuestions>();
            List<SurveyQuestions> lstSurveyQuestions = new List<SurveyQuestions>();
            SurveyResponseDetails lstSurveyResponse = new SurveyResponseDetails();
            string strQuery = string.Empty;
            string joinedResponseID = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strloadMyTask = @"SELECT  T.SyncTaskID ,";
                    strloadMyTask += "T.TaskSubmittedDate SubmittedDateTime,IFNULL(U.Name,'') SubmittedBy,T.TypeID AS SurveyID,ifnull(sur.BeneficiaryTypeID,0) BeneficiaryTypeID ";
                    strloadMyTask += ",ifnull(sur.WorkflowID,0) WorkflowID,ifnull(T.WFLevel,0) WFLevel FROM TbMyTask T JOIN TbUser U ON T.CreatedBy = U.ID JOIN TbProjectSurvey sur ON sur.SurveyID = T.TypeID ";
                    strloadMyTask += " WHERE T.TypeID = @SurveyID and T.IsActive = 1 and T.IsDeleted = 0;";

                    List<MyTaskSummary> lstMyTask = new List<MyTaskSummary>();
                    lstMyTask = _connection.Query<MyTaskSummary>(strloadMyTask, new { SurveyID = SurveyID }).ToList();
                    lstSurveyResponse.MyTaskSummary = lstMyTask;


                    //START Survey Question
                    string strSurveyQuestion = "SELECT QuestionID,QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,SurveyID,SectionID,CreatedBy,UpdatedBy,CreatedDate,";
                    strSurveyQuestion += "UpdatedDate,IsConditionQuestion,IsRepeatSection,RepeatSectionId,IsUnique,IsSearchable,MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,";
                    strSurveyQuestion += "TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables,";
                    strSurveyQuestion += "HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,ifnull(IsOtherOptionSelected,0) IsOtherOptionSelected FROM TbProjectSurveyQuestion WHERE ";
                    strSurveyQuestion += " SurveyID =@SurveyID and IFNULL(IsActive,0) = 1;";

                    List<MyTaskSurveyQuestion> lstMyTaskSurveyQuestion = new List<MyTaskSurveyQuestion>();
                    lstMyTaskSurveyQuestion = _connection.Query<MyTaskSurveyQuestion>(strSurveyQuestion, new { SurveyID = SurveyID }).ToList();
                    lstSurveyResponse.lstMyTaskSurveyQuestion = lstMyTaskSurveyQuestion;


                    //STARTS  My task Question Type
                    string strQuestionType = "select ID,`Type`,Category from TbQuestionType";
                    List<MyTaskQuestionType> lstQuestionType = new List<MyTaskQuestionType>();
                    lstQuestionType = _connection.Query<MyTaskQuestionType>(strQuestionType).ToList();

                    lstSurveyResponse.lstMyTaskQuestionType = lstQuestionType;
                    //ENDS  Master List

                    //STARTS Project Survey QST Option
                    string strSurveyQSTOption = "SELECT QSTOption.OptionID,QSTOption.OptionValue,QSTOption.QuestionID,false as IsOptSelected,QST.SurveyID  FROM TbProjectSurveyQSTOption QSTOption JOIN TbProjectSurveyQuestion QST ON QST.QuestionID = QSTOption.QuestionID WHERE";
                    strSurveyQSTOption += " QST.SurveyID = @SurveyID";

                  
                        List<MyTaskProjectSurveyQSTOption> lstMyTaskProjectSurveyQSTOption = new List<MyTaskProjectSurveyQSTOption>();
                        lstMyTaskProjectSurveyQSTOption = _connection.Query<MyTaskProjectSurveyQSTOption>(strSurveyQSTOption, new { SurveyID = SurveyID }).ToList();
                    lstSurveyResponse.lstMyTaskProjectSurveyQSTOption = lstMyTaskProjectSurveyQSTOption;


                    //START Survey Section Logic

                    string strSurveySkipLogic = @"SELECT SectionLogicID,SurveyID,SectionID,LogicType,QuestionID,ConditionID,Answer,LogicOperator FROM TbProjectSurveySectionLogic WHERE IFNULL(IsActive,0)=1 and SurveyID= @SurveyID";
                    List<MyTaskSurveySectionLogic> lstMyTaskSurveySectionLogic = new List<MyTaskSurveySectionLogic>();
                    lstMyTaskSurveySectionLogic = _connection.Query<MyTaskSurveySectionLogic>(strSurveySkipLogic, new { SurveyID = SurveyID }).ToList();

                    lstSurveyResponse.lstMyTaskSurveySectionLogic = lstMyTaskSurveySectionLogic;
                    //ENDS Survey Section Logic

                    string strSurQuery = @"SELECT srvRES.ID,srvRES.ResponseID BeneficiaryId,srvQST.QuestionID,srvQST.QuestionName,REPLACE(ifnull(srvRES.Answer,''), '\n', '') Answer,ifnull(srvQST.IsConditionQuestion,0) IsConditionQuestion ";
                    strSurQuery += ",srvQST.SectionID,srvRES.SyncTaskID FROM TbProjectSurveyQuestion srvQST JOIN TbProjectSurveyResponses srvRES ON srvQST.QuestionID  = srvRES.QuestionID WHERE srvRES.SurveyID = @SurveyID and ";
                    strSurQuery += "IFNULL(srvRES.IsActive,0) =1 order by srvRES.ID asc;";
                    lstSurveyQuestions = _connection.Query<SurveyQuestions>(strSurQuery, new { SurveyID = SurveyID }).ToList();

                    if (lstSurveyQuestions != null && lstSurveyQuestions.Count() > 0)
                    {
                        lstSurveyResponse.MyTaskResponseDetails = new List<MyTaskResponseDetails>();
                        var mySKUs = lstSurveyQuestions.Select(l => l.BeneficiaryId).Distinct().ToList();
                        joinedResponseID = string.Join(",", mySKUs);
                    }

                    //STARTS TbProject BeneficiaryType
                    string strBeneficiaryType = "SELECT  BType.BeneficiaryTypeID,BType.`Name`,BType.Description,BType.ProjectID,SRV.SurveyID FROM TbProjectBeneficiaryType BType  ";
                    strBeneficiaryType += " JOIN TbProjectSurvey SRV ON SRV.BeneficiaryTypeID = BType.BeneficiaryTypeID";
                    strBeneficiaryType += " JOIN TbProject prj ON prj.ProjectId = BType.ProjectID";
                    strBeneficiaryType += " WHERE  IFNULL(BType.IsActive,0) =1  and IFNULL(BType.IsDeleted,0) = 0 AND SRV.SurveyID = @SurveyID";
                    //and IFNULL(SRV.IsActive,0) =1 and IFNULL(prj.IsActive,0) =1 
                    List<TbMyTaskProjectBeneficiaryType> lstTbMyTaskProjectBeneficiaryType = new List<TbMyTaskProjectBeneficiaryType>();
                    lstTbMyTaskProjectBeneficiaryType = _connection.Query<TbMyTaskProjectBeneficiaryType>(strBeneficiaryType, new { SurveyID = SurveyID }).ToList();

                    lstSurveyResponse.lstTbMyTaskProjectBeneficiaryType = lstTbMyTaskProjectBeneficiaryType;
                    //ENDS TbProject BeneficiaryType
                   
                    var MyTypeIds = lstTbMyTaskProjectBeneficiaryType.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                    string joinedBeneficiaryTypeID = string.Join(",", MyTypeIds);

                    if (joinedResponseID != null && joinedResponseID.Length > 0)
                    {

                        //STARTS  Beneficiary Type Question
                        string strBeneficiaryTypeQuestion = "SELECT QuestionID,QuestionName,AnswerTypeID,BeneficiaryTypeID,IsUnique,IsSearchable,IsMandatoryResponse,QuestionHint,IsValidationQuestion,RegexExpression,ifnull(QuestionOrder,0) QuestionOrder FROM TbBeneficiaryTypeQuestion WHERE IFNULL(IsActive,0)=1 AND IFNULL(IsDeleted,0)=0";
                        strBeneficiaryTypeQuestion += " and BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ")";

                        if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                        {
                            List<MyTaskBeneficiaryTypeQuestion> lstBeneficiaryTypeQuestion = new List<MyTaskBeneficiaryTypeQuestion>();
                            lstBeneficiaryTypeQuestion = _connection.Query<MyTaskBeneficiaryTypeQuestion>(strBeneficiaryTypeQuestion).ToList();

                            lstSurveyResponse.lstMyTaskBeneficiaryTypeQuestion = lstBeneficiaryTypeQuestion;
                        }
                        //ENDS Beneficiary Type Question


                        //STARTS Beneficiary Survey QST Option
                        string strBeneficiaryQSTOption = "SELECT QSTOption.QstnAnsOptionID,QSTOption.BeneficiaryTypeID,QSTOption.QuestionID,QSTOption.AnswerTypeID,QSTOption.OptionValue  FROM TbBeneficiaryTypeQstnAnswerOptions QSTOption JOIN TbBeneficiaryTypeQuestion QST ON QST.QuestionID = QSTOption.QuestionID WHERE";
                        strBeneficiaryQSTOption += " QST.BeneficiaryTypeID IN (" + joinedBeneficiaryTypeID + ")";

                        if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                        {
                            List<MyTaskBeneficiarySurveyQSTOption> lstMyTaskBeneficiarySurveyQSTOption = new List<MyTaskBeneficiarySurveyQSTOption>();
                            lstMyTaskBeneficiarySurveyQSTOption = _connection.Query<MyTaskBeneficiarySurveyQSTOption>(strBeneficiaryQSTOption).ToList();

                            lstSurveyResponse.lstMyTaskBeneficiarySurveyQSTOption = lstMyTaskBeneficiarySurveyQSTOption;
                        }
                        //ENDS Beneficiary Survey QST Option

                        //STARTS  Beneficiary TbBeneficiaryQSTCondition

                        //var MyBenQuestion = lstBeneficiaryTypeQuestion.Select(l => l.QuestionID).ToList();
                        //string joinedMyBenQuestion = string.Join(",", MyBenQuestion);

                        string strBeneficiaryQSTCondition = "SELECT qstCondition.ID,qstCondition.QuestionID,qstCondition.ValidationCondition,qstCondition.ConditionValue,qstCondition.MinValue,qstCondition.`MaxValue`,qstCondition.RegEx,qstCondition.ErrorText,qstCondition.LogicOperator,bnfQST.BeneficiaryTypeID FROM TbBeneficiaryQSTCondition";
                        strBeneficiaryQSTCondition += " qstCondition JOIN TbBeneficiaryTypeQuestion bnfQST ON qstCondition.QuestionID = bnfQST.QuestionID";
                        strBeneficiaryQSTCondition += " WHERE bnfQST.BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ")";

                        if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                        {
                            List<MyTaskProjectSurveyQSTCondition> lstBenMyTaskProjectSurveyQSTCondition = new List<MyTaskProjectSurveyQSTCondition>();
                            lstBenMyTaskProjectSurveyQSTCondition = _connection.Query<MyTaskProjectSurveyQSTCondition>(strBeneficiaryQSTCondition).ToList();
                            lstSurveyResponse.lstMyTaskProjectBenSurveyQSTCondition = lstBenMyTaskProjectSurveyQSTCondition;
                        }
                        //ENDS Beneficiary TbBeneficiaryQSTCondition



                        //STARTS  Beneficiary Beneficiary Registration Details
                        string strBeneficiaryRegistrationDetails = "select benreg.BeneficiaryRegID,benreg.BeneficiaryID,benreg.QuestionID,benreg.AnswerTypeID,benreg.Answer,ben.BeneficiaryTypeID FROM TbBeneficiaryRegistrationDetails benreg JOIN TbProjectBeneficiary ben ON benreg.BeneficiaryID = ben.BeneficiaryID WHERE IFNULL(benreg.IsDeleted,0) = 0";
                        strBeneficiaryRegistrationDetails += " AND ben.BeneficiaryTypeID IN(" + joinedBeneficiaryTypeID + ") ";
                        strBeneficiaryRegistrationDetails += " AND benreg.BeneficiaryID IN(" + joinedResponseID + ") ";

                        if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                        {
                            List<TbMyTaskBeneficiaryRegistrationDetails> lstBeneficiaryRegistrationDetails = new List<TbMyTaskBeneficiaryRegistrationDetails>();
                            lstBeneficiaryRegistrationDetails = _connection.Query<TbMyTaskBeneficiaryRegistrationDetails>(strBeneficiaryRegistrationDetails).ToList();
                            lstSurveyResponse.lstTbMyTaskBeneficiaryRegistrationDetails = lstBeneficiaryRegistrationDetails;
                        }
                        //ENDS Beneficiary Beneficiary Registration Details


                        //STARTS  Beneficiary Cascading QstnOptions
                        string strTbBeneficiaryCascadingQstnOptions = "SELECT OptionGroupID,OptionValue,HierarchyOrder,TableName,ColumnName,PrimaryColumn,ParentItemColumn FROM TbBeneficiaryCascadingQstnOptions WHERE IFNULL(IsDeleted,0) = 0";
                        List<MyTaskSurveyCascadingQuestionOptions> lstBeneficiaryCascadingQstnOptions = new List<MyTaskSurveyCascadingQuestionOptions>();
                        lstBeneficiaryCascadingQstnOptions = _connection.Query<MyTaskSurveyCascadingQuestionOptions>(strTbBeneficiaryCascadingQstnOptions).ToList();

                        lstSurveyResponse.lstMyTaskBeneficiarySurveyCascadingQuestionOptions = lstBeneficiaryCascadingQstnOptions;
                        //ENDS Beneficiary Cascading QstnOptions

                        if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                        {
                            string strBenQuery = @"SELECT BenReg.BeneficiaryID BeneficiaryId,BenQST.QuestionID,BenQST.QuestionName,REPLACE(ifnull(BenReg.Answer,''), '\n', '') Answer FROM TbBeneficiaryTypeQuestion BenQST  ";
                            strBenQuery += "JOIN TbBeneficiaryRegistrationDetails BenReg ON BenQST.QuestionID  = BenReg.QuestionID ";
                            strBenQuery += "WHERE  BenReg.BeneficiaryID in(" + joinedResponseID + ") order by BeneficiaryID asc;";
                            lstBeneficiaryQuestions = _connection.Query<BeneficiaryQuestions>(strBenQuery).ToList();
                        }

                        List<SurveyQuestions> lstResponses = new List<SurveyQuestions>();
                        string strResponses = @"SELECT SyncTaskID from TbProjectSurveyResponses srvRES  WHERE srvRES.SurveyID = @SurveyID and IFNULL(srvRES.IsActive,0) =1  order by srvRES.ID asc;";
                        lstResponses = _connection.Query<SurveyQuestions>(strResponses, new { SurveyID = SurveyID }).ToList();




                        if (lstResponses != null && lstResponses.Count() > 0)
                        {
                           
                            var mySKUs = lstResponses.Select(l => l.SyncTaskID).Distinct().ToList();
                            joinedResponseID = string.Join(",", mySKUs);
                        }


                        if (lstResponses != null && lstResponses.Count() > 0)
                        {
                            var uniqueSyncTaskID = lstResponses.Select(p => p.SyncTaskID).Distinct().ToList();
           
                            
                            lstSurveyResponse.MyTaskResponseDetails = new List<MyTaskResponseDetails>();

                            foreach (var itm in uniqueSyncTaskID)
                            {
                               
                                MyTaskResponseDetails objResponseDetails = new MyTaskResponseDetails();
                                objResponseDetails.SyncTaskID = itm;

                                List<BeneficiaryQuestions> lstFilteredBeneficiary = new List<BeneficiaryQuestions>();
                                List<SurveyQuestions> lstFilteredSurveyQuestions = new List<SurveyQuestions>();
                                List<SurveyQuestions> lstFilteredSurveyQuestionsfilter = new List<SurveyQuestions>();

                               // lstFilteredBeneficiary = lstBeneficiaryQuestions.Where(x => x.BeneficiaryId == itm).ToList();
                                lstFilteredSurveyQuestions = lstSurveyQuestions.Where(x => x.SyncTaskID == itm).ToList();
                               // objResponseDetails.BeneficiaryQuestions = lstFilteredBeneficiary;

                                if (lstMyTaskSurveySectionLogic != null && lstMyTaskSurveySectionLogic.Count() > 0)
                                {
                                    for (var k = 0; k < lstMyTaskSurveySectionLogic.Count(); k++)
                                    {
                                        List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                        lstSurveyQuestions1 = lstFilteredSurveyQuestions.Where(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID).ToList();

                                        if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                        {
                                            lstFilteredSurveyQuestions.RemoveAll(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID);

                                            SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                            objlstSurveyQuestions1 = lstFilteredSurveyQuestions.Where(x => x.QuestionId == lstMyTaskSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                            int index = lstFilteredSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                            for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                            {
                                                lstFilteredSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                            }
                                        }
                                    }
                                }
                                objResponseDetails.SurveyQuestions = lstFilteredSurveyQuestions;
                                lstSurveyResponse.MyTaskResponseDetails.Add(objResponseDetails);


                            }
                        }
                        else
                        {
                            MyTaskResponseDetails objResponseDetails = new MyTaskResponseDetails();
                            objResponseDetails.SurveyQuestions = new List<SurveyQuestions>();
                            lstSurveyResponse.MyTaskResponseDetails = new List<MyTaskResponseDetails>();

                            if (lstMyTaskSurveySectionLogic != null && lstMyTaskSurveySectionLogic.Count() > 0)
                            {
                                for (var k = 0; k < lstMyTaskSurveySectionLogic.Count(); k++)
                                {
                                    List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                    lstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID).ToList();

                                    if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                    {
                                        lstSurveyQuestions.RemoveAll(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID);

                                        SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                        objlstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.QuestionId == lstMyTaskSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                        int index = lstSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                        for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                        {
                                            lstSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                        }
                                    }
                                }
                            }

                            objResponseDetails.SurveyQuestions = lstSurveyQuestions;
                            lstSurveyResponse.MyTaskResponseDetails.Add(objResponseDetails);
                        }
                    }
                    else
                    {
                        MyTaskResponseDetails objResponseDetails = new MyTaskResponseDetails();
                        objResponseDetails.SurveyQuestions = new List<SurveyQuestions>();
                        lstSurveyResponse.MyTaskResponseDetails= new List<MyTaskResponseDetails>();

                        if (lstMyTaskSurveySectionLogic != null && lstMyTaskSurveySectionLogic.Count() > 0)
                        {
                            for (var k = 0; k < lstMyTaskSurveySectionLogic.Count(); k++)
                            {
                                List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                lstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID).ToList();

                                if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                {
                                    lstSurveyQuestions.RemoveAll(x => x.SectionID == lstMyTaskSurveySectionLogic[k].SectionID);

                                    SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                    objlstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.QuestionId == lstMyTaskSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                    int index = lstSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                    for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                    {
                                        lstSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                    }
                                }
                            }
                        }

                        objResponseDetails.SurveyQuestions = lstSurveyQuestions;
                        lstSurveyResponse.MyTaskResponseDetails.Add(objResponseDetails);
                    }
                }
                
            }
            catch (Exception ex)
            {
            }
            return lstSurveyResponse;
        }

        #endregion Response

        #endregion

        #region Create
        /// <summary>
        /// Function used to add project survey section
        /// </summary>
        /// <param name="objSurveySection"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool AddProjectSurveySection(ProjectSurveySection objSurveySection, int loggeduserid)
        {
            var SectionId = 0;
            string strQuery = "";
            var lastSceOrder = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = "Select ifnull(max(ifnull(SectionOrder, 0)),0)   From TbProjectSurveySection Where SurveyID = @SID ";
                        lastSceOrder = _connection.Query<int>(strQuery, new { SID = objSurveySection.SurveyID }).Single();

                        strQuery = @"INSERT INTO TbProjectSurveySection(SectionName,SectionDescription,SectionOrder ,SurveyID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)" +
                                    "VALUES (@SectionName,@SectionDescription,@SectionOrder,@SurveyID,@IsActive ,@CreatedDate,@CreatedDate,@CreatedBy,@CreatedBy); SELECT LAST_INSERT_ID();";

                        SectionId = _connection.Query<int>(strQuery, new
                        {
                            SectionName = objSurveySection.SectionName,
                            SectionDescription = objSurveySection.SectionDescription,
                            SectionOrder = lastSceOrder + 1,
                            SurveyID = objSurveySection.SurveyID,
                            IsActive = 1,
                            CreatedDate = DateTime.Now,
                            CreatedBy = loggeduserid
                        }).Single();

                        strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = objSurveySection.SurveyID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = SectionId;
                        objLog.FromIDColumnName = "SectionID";
                        objLog.Message = "Added new Survey Section named " + objSurveySection.SectionName + "'";
                        objLog.CreatedBy = loggeduserid;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            if (SectionId > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function used to update project survey section
        /// </summary>
        /// <param name="objSurveySection"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool UpdateProjectSurveySection(ProjectSurveySection objSurveySection, int loggeduserid)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string strQuery = @"Update TbProjectSurveySection SET SectionName=@SectionName,SectionDescription=@SectionDescription,";
                        strQuery += "UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy WHERE SectionID = @SectionID;";

                        var rr = _connection.Execute(strQuery, new
                        {
                            SectionName = objSurveySection.SectionName,
                            SectionDescription = objSurveySection.SectionDescription,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = loggeduserid,
                            SectionID = objSurveySection.SectionID
                        });

                        strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                        var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = objSurveySection.SurveyID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = objSurveySection.SectionID;
                        objLog.FromIDColumnName = "SectionID";
                        objLog.Message = "Updated Survey Section  details named " + objSurveySection.SectionName + "'.";
                        objLog.CreatedBy = loggeduserid;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        /// <summary>
        /// Function used to update section order list
        /// </summary>
        /// <param name="strSectionOrderList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateSectionOrder(string strSectionOrderList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    List<ProjectSurveySection> lstSectionOrder = JsonConvert.DeserializeObject<List<ProjectSurveySection>>(strSectionOrderList);

                    if (lstSectionOrder != null)
                    {
                        foreach (ProjectSurveySection Item in lstSectionOrder)
                        {
                            strQuery = @"UPDATE TbProjectSurveySection SET SectionOrder =@SectionOrder, UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where SectionID =@SectionID";
                            _connection.Execute(strQuery, new
                            {
                                SectionID = Item.SectionID,
                                SectionOrder = Item.SectionOrder,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while updating data";
                throw ex;
            }

            return retMsg;

        }

        /// <summary>
        /// Function used to add survey section question list
        /// </summary>
        /// <param name="SecQuestionList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddSurveySectionQstn(string SecQuestionList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_AddSurveySectionQuestions";
                    id = _connection.ExecuteScalar<int>(strQuery, new { SecQuestionList = SecQuestionList, loggedUserId = loggedUserId }, commandType: CommandType.StoredProcedure);
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

        /// <summary>
        /// Function used to update project survey questions based on QuestionID
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="QuestionName"></param>
        /// <param name="QstnTypeID"></param>
        /// <param name="lstOptions"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="SurveyID"></param>
        /// <param name="SectionID"></param>
        /// <returns></returns>
        public string UpdateQuestion(int QuestionID, string QuestionName, int QstnTypeID, string lstOptions, int loggedUserId, int SurveyID, int SectionID, bool IsOtherOptionSelected)
        {
            string strQuery = string.Empty;
            var retMsg = "success";

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"SELECT EXISTS( SELECT QuestionID  FROM TbProjectSurveyQuestion WHERE SurveyID=@SurveyID and SectionID=@SectionID And QuestionName=@QuestionName and QuestionID != @QuestionID)";
                        var id = _connection.Query<int>(strQuery, new { SurveyID = SurveyID, SectionID = SectionID, QuestionID = QuestionID, QuestionName = QuestionName }).Single();
                        if (id == 0)
                        {
                            strQuery = @"UPDATE TbProjectSurveyQuestion SET QuestionName =@QuestionName,QuestionTypeID =@QuestionTypeID,
                                     UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId,IsOtherOptionSelected =@IsOtherOptionSelected where QuestionID =@QuestionID";
                            _connection.Execute(strQuery, new
                            {
                                QuestionID = QuestionID,
                                QuestionName = QuestionName,
                                QuestionTypeID = QstnTypeID,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId,
                                IsOtherOptionSelected = IsOtherOptionSelected
                            });

                            strQuery = @"Delete from TbProjectSurveyQSTOption where QuestionID =@QuestionID";
                            _connection.Execute(strQuery, new { @QuestionID = QuestionID });

                            List<QuestionOptionList> lstQstnOpt = JsonConvert.DeserializeObject<List<QuestionOptionList>>(lstOptions);
                            if (lstQstnOpt != null)
                            {
                                foreach (QuestionOptionList Item in lstQstnOpt)
                                {
                                    strQuery = @"INSERT INTO TbProjectSurveyQSTOption(QuestionID,OptionValue,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate) 
                                VALUES (@QuestionID,@OptionValue,@loggedUserId,@loggedUserId,now(),now());";

                                    _connection.Execute(strQuery, new
                                    {
                                        @QuestionID = QuestionID,
                                        OptionValue = Item.OptionValue,
                                        loggedUserId = loggedUserId
                                    });
                                }

                                if (IsOtherOptionSelected)
                                {
                                    strQuery = @"INSERT INTO TbProjectSurveyQSTOption(QuestionID,OptionValue,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate) 
                                VALUES (@QuestionID,@OptionValue,@loggedUserId,@loggedUserId,now(),now());";

                                    _connection.Execute(strQuery, new
                                    {
                                        @QuestionID = QuestionID,
                                        OptionValue = "Others",
                                        loggedUserId = loggedUserId
                                    });
                                }
                            }

                        }
                        else
                        {
                            retMsg = "QuestionName already exists";
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while deleting data";
                throw ex;
            }

            return retMsg;

        }

        /// <summary>
        /// Function used to update question settings based on QuestionID
        /// </summary>
        /// <param name="objQstn"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateQuestionSettings(SurveyQuestion objQstn, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"UPDATE TbProjectSurveyQuestion SET QuestionHint =@QuestionHint,IsMandatory =@IsMandatory,DefaultValue =@DefaultValue, " +
                                        " IsSearchable =@IsSearchable,IsUnique =@IsUnique,RegEx =@RegEx,ErrorText =@ErrorText, UOM= @UOM, " +
                                        //"ValidationCondition =@ValidationCondition , MinValue =@MinValue ,`MaxValue` =@MaxValue ," +
                                        " FormulaExpression=@FormulaExpression, IsFormulaQuestion =@IsFormulaQuestion ," +
                                        " FormulaOperation =@FormulaOperation, FormulaFieldVariables =@FormulaFieldVariables, IsValidationQuestion =@IsValidationQuestion , " +
                                        " UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where QuestionID =@QuestionID";
                        _connection.Execute(strQuery, new
                        {
                            @QuestionID = objQstn.QuestionID,
                            @QuestionHint = objQstn.QuestionHint != null ? objQstn.QuestionHint : "",
                            @IsMandatory = objQstn.IsMandatory,
                            @DefaultValue = objQstn.DefaultValue != null ? objQstn.DefaultValue : "",
                            @IsSearchable = objQstn.IsSearchable,
                            @IsUnique = objQstn.IsUnique,
                            @RegEx = objQstn.RegEx != null ? objQstn.RegEx : "",
                            @ErrorText = objQstn.ErrorText != null ? objQstn.ErrorText : "",
                            FormulaExpression = objQstn.FormulaExpression != null ? objQstn.FormulaExpression : "",
                            IsFormulaQuestion = objQstn.IsFormulaQuestion,
                            FormulaOperation = objQstn.FormulaOperation != null ? objQstn.FormulaOperation : "",
                            FormulaFieldVariables = objQstn.FormulaFieldVariables != null ? objQstn.FormulaFieldVariables : "",
                            IsValidationQuestion = objQstn.IsValidationQuestion,
                            UOM = objQstn.UOM,
                            updatedDate = DateTime.Now,
                            loggedUserId = loggedUserId
                        });

                        strQuery = @"Delete from TbProjectSurveyQSTCondition where QuestionID =@QuestionID";
                        _connection.Execute(strQuery, new { @QuestionID = objQstn.QuestionID });

                        List<QuestionCondition> lstQstnCondition = JsonConvert.DeserializeObject<List<QuestionCondition>>(objQstn.ValidationConditions);

                        if (lstQstnCondition != null)
                        {
                            foreach (QuestionCondition Item in lstQstnCondition)
                            {
                                strQuery = @"INSERT INTO TbProjectSurveyQSTCondition(QuestionID,ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator,CreatedDate,CreatedBy) 
                                VALUES (@QuestionID,@ValidationCondition,@MinValue,@MaxValue,@ConditionValue,@LogicOperator,now(),@loggedUserId);";

                                _connection.Execute(strQuery, new
                                {
                                    @QuestionID = objQstn.QuestionID,
                                    @ValidationCondition = Item.ValidationCondition,
                                    @MinValue = Item.MinValue,
                                    @MaxValue = Item.MaxValue,
                                    @ConditionValue = Item.ConditionValue,
                                    @LogicOperator = Item.LogicOperator,
                                    loggedUserId = loggedUserId
                                });
                            }
                        }


                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while deleting data";
                throw ex;
            }

            return retMsg;

        }

        /// <summary>
        /// Function used to update project survey question order based on QuestionID
        /// </summary>
        /// <param name="strQstnOrderList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateQuestionOrder(string strQstnOrderList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    List<SurveyQuestion> lstQstnOrder = JsonConvert.DeserializeObject<List<SurveyQuestion>>(strQstnOrderList);

                    if (lstQstnOrder != null)
                    {
                        foreach (SurveyQuestion Item in lstQstnOrder)
                        {
                            strQuery = @"UPDATE TbProjectSurveyQuestion SET QuestionOrder =@QuestionOrder, UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where QuestionID =@QuestionID";
                            _connection.Execute(strQuery, new
                            {
                                @QuestionID = Item.QuestionID,
                                @QuestionOrder = Item.QuestionOrder,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while updating data";
                throw ex;
            }

            return retMsg;

        }

        /// <summary>
        /// Function used to update section login based on SurveyID,SectionID,LogicType
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="SectionID"></param>
        /// <param name="LogicType"></param>
        /// <param name="SectionLogicID"></param>
        /// <param name="QuestionID"></param>
        /// <param name="ConditionID"></param>
        /// <param name="Answer"></param>
        /// <param name="LogicOperator"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateSectionLogic(int SurveyID, int SectionID, string LogicType, int SectionLogicID, int QuestionID, int ConditionID, string Answer, int LogicOperator, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "";
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        //strQuery = @"SELECT EXISTS( SELECT SectionLogicID  FROM TbProjectSurveySectionLogic WHERE SurveyID=@SurveyID and SectionID=@SectionID And QuestionID =@QuestionID);";
                        //var id = _connection.Query<int>(strQuery, new { SurveyID = SurveyID, SectionID = SectionID, QuestionID = QuestionID }).Single();
                        //if (id == 0)
                        //    SectionLogicID = 0;
                        //else
                        //{
                        //    strQuery = @"SELECT SectionLogicID  FROM TbProjectSurveySectionLogic WHERE SurveyID=@SurveyID and SectionID=@SectionID And QuestionID =@QuestionID;";
                        //    id = _connection.Query<int>(strQuery, new { SurveyID = SurveyID, SectionID = SectionID, QuestionID = QuestionID }).Single();
                        //    SectionLogicID = SectionLogicID == 0 ? id : SectionLogicID;
                        //}
                        string Logic = string.Empty;
                        int IsConditionQuestion = 0;
                        int IsRepeatSection = 0;
                        if (LogicType == "Skip Logic")
                        {
                            IsConditionQuestion = 1;
                            IsRepeatSection = 0;
                            // Make all repeat logic IsActive= 0in that section
                            Logic = "Repeat Logic";
                        }
                        else if (LogicType == "Repeat Logic")
                        {
                            IsConditionQuestion = 0;
                            IsRepeatSection = 1;
                            // Make all Skip logic IsActive= 0 in that section
                            Logic = "Skip Logic";
                        }

                        strQuery = @"UPDATE TbProjectSurveySectionLogic SET IsActive =0 , UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where SurveyID = @SurveyID and SectionID = @SectionID and LogicType =@LogicType";
                        _connection.Execute(strQuery, new
                        {
                            SurveyID = SurveyID,
                            SectionID = SectionID,
                            updatedDate = DateTime.Now,
                            LogicType = Logic,
                            loggedUserId = loggedUserId
                        });

                        if (SectionLogicID == 0)
                        {

                            strQuery = @"INSERT INTO TbProjectSurveySectionLogic(SectionID,SurveyID,LogicType,QuestionID,ConditionID,Answer,LogicOperator,
                                     IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy) VALUES (@SectionID,@SurveyID,@LogicType,@QuestionID,@ConditionID,
                                    @Answer,@LogicOperator,1 ,now(),now(),@loggedUserId,@loggedUserId); SELECT LAST_INSERT_ID();";

                            _connection.Execute(strQuery, new
                            {
                                SectionID = SectionID,
                                SurveyID = SurveyID,
                                LogicType = LogicType,
                                QuestionID = QuestionID,
                                ConditionID = ConditionID,
                                Answer = Answer,
                                LogicOperator = LogicOperator,
                                loggedUserId = loggedUserId
                            });
                        }
                        else
                        {
                            strQuery = @"UPDATE TbProjectSurveySectionLogic SET LogicType =@LogicType,QuestionID =@QuestionID,ConditionID =@ConditionID, " +
                                       " Answer =@Answer,LogicOperator =@LogicOperator,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where SectionLogicID =@SectionLogicID";
                            _connection.Execute(strQuery, new
                            {
                                SectionLogicID = SectionLogicID,
                                LogicType = LogicType,
                                QuestionID = QuestionID,
                                ConditionID = ConditionID,
                                Answer = Answer,
                                LogicOperator = LogicOperator,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId
                            });

                        }
                        //// IsConditionQuestion
                        //strQuery = @"UPDATE TbProjectSurveyQuestion SET IsConditionQuestion =@IsConditionQuestion , IsRepeatSection =@IsRepeatSection, UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId Where QuestionID =@QuestionID ";
                        //_connection.Execute(strQuery, new  { QuestionID = QuestionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId , IsConditionQuestion = IsConditionQuestion, IsRepeatSection = IsRepeatSection });



                        // IsConditionQuestion
                        strQuery = @"UPDATE TbProjectSurveyQuestion SET IsConditionQuestion =1 ,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId Where QuestionID =@QuestionID ";
                        _connection.Execute(strQuery, new { QuestionID = QuestionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                        // IsRepeatSection
                        if (@LogicType == "Repeat Logic")
                        {
                            strQuery = @"UPDATE TbProjectSurveyQuestion SET IsRepeatSection =1 ,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId Where SectionID =@SectionID ";
                            _connection.Execute(strQuery, new { SectionID = SectionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                        }


                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while deleting data";
                throw ex;
            }

            return retMsg;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Function used to delete survey section based on SectionID
        /// </summary>
        /// <param name="SectionID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveSurveySection(int SectionID, int loggedUserId)
        {
            var retMsg = "success";
            ProjectSurvey objProjectSurvey = new ProjectSurvey();

            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        string query = @"UPDATE TbProjectSurveySection SET IsActive =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                        " where SectionID =@SectionID";
                        _connection.Execute(query, new { SectionID = SectionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                        query = @"UPDATE TbProjectSurveySectionLogic SET IsActive =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                       " where SectionID =@SectionID";
                        _connection.Execute(query, new { SectionID = SectionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                        query = @"UPDATE TbProjectSurveyQuestion SET IsActive =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                        " where SectionID =@SectionID";
                        _connection.Execute(query, new { SectionID = SectionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                        //query = "Select distinct t.ProjectId  From TbProjectSurvey t inner join TbProjectSurveySection s on t.SurveyID = s.SurveyID and s.SectionID =@SectionID";
                        //var ProjectId = _connection.Query<int>(query, new { SectionID = SectionID }).Single();

                        //query = "Select SectionName  From TbProjectSurveySection Where SectionID =@SectionID ";
                        //var name = _connection.Query<string>(query, new { SectionID = SectionID }).Single();

                        query = @"Select distinct t.ProjectId,s.SectionName  From TbProjectSurvey t inner join TbProjectSurveySection s on t.SurveyID = s.SurveyID and s.SectionID =@SectionID";

                        objProjectSurvey = _connection.Query<ProjectSurvey>(query, new
                        {
                            SectionID = SectionID
                        }).FirstOrDefault();


                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = objProjectSurvey.ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = SectionID;
                        objLog.FromIDColumnName = "SectionID";
                        objLog.Message = "Removed Survey Section  details named " + objProjectSurvey.SectionName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while deleting data";
                        throw ex;
                    }
                    _Tran.Complete();
                }

            }
            return retMsg;
        }

        /// <summary>
        /// Function used to delete survey section question based on SectionID,QuestionID
        /// </summary>
        /// <param name="SectionID"></param>
        /// <param name="QuestionID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveSurveySectionQuestion(int SectionID, int QuestionID, int loggedUserId)
        {
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbProjectSurveyQuestion SET IsActive =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                    " where SectionID =@SectionID And QuestionID =@QuestionID";
                    _connection.Execute(query, new { SectionID = SectionID, QuestionID = QuestionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
                catch (Exception ex)
                {
                    retMsg = "Error while deleting data";
                    throw ex;
                }

            }
            return retMsg;
        }

        /// <summary>
        /// Function used to delete section logic items based on SectionLogicID
        /// </summary>
        /// <param name="SectionLogicID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string DeleteSectionLogicItem(int SectionLogicID, int loggedUserId)
        {
            var retMsg = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbProjectSurveySectionLogic SET IsActive =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                    " where SectionLogicID =@SectionLogicID";
                    _connection.Execute(query, new { SectionLogicID = SectionLogicID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                    query = @"SELECT EXISTS(SELECT SectionLogicID FROM TbProjectSurveySectionLogic where IsActive = 1 and
                    QuestionID = (SELECT QuestionID FROM TbProjectSurveySectionLogic WHERE SectionLogicID= @SectionLogicID));";
                    id = _connection.Query<int>(query, new { SectionLogicID = SectionLogicID }).Single();

                    if (id == 0)
                    {
                        query = @"UPDATE TbProjectSurveyQuestion SET IsConditionQuestion =0,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId 
                                     where QuestionID = (SELECT QuestionID FROM TbProjectSurveySectionLogic WHERE SectionLogicID = @SectionLogicID)";
                        _connection.Execute(query, new { SectionLogicID = SectionLogicID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                    }

                }
                catch (Exception ex)
                {
                    retMsg = "Error while deleting data";
                    throw ex;
                }

            }
            return retMsg;
        }
        #endregion

        /// <summary>
        /// Function used to fetch project detail data based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectSurvey GetDetailProjectData(int ProjectId)
        {
            ProjectSurvey ObjProjectSur = new ProjectSurvey();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    var SurveyTotResponseObj = 0;
                    var SurveyTotCompleteObj = 0;
                    var SurveyTotResponseCountObj = 0;
                    var SurveyTotCompleteCountObj = 0;

                    if (ProjectId != 0)
                    {
                        strQuery = @"select IFNULL(count(aa.ID),0) as SurveyTotResponse from (Select distinct t.ID,t.ResponseID, t.Answer,t.SurveyID From TbProjectSurveyResponses t inner join TbProjectSurvey s on s.ProjectId = @ProjectId and IFNULL(s.IsActive,0)= 1 and t.SurveyID = s.SurveyID Where IFNULL(t.IsActive,0)= 1) aa";
                        ObjProjectSur = _connection.Query<ProjectSurvey>(strQuery, new { ProjectId = ProjectId }).FirstOrDefault();
                        SurveyTotResponseObj = ObjProjectSur.SurveyTotResponse;

                        strQuery = @"select IFNULL(count(t.SurveyID),0) as SurveyTotComplete from TbProjectSurvey t where t.ProjectId = @ProjectId and IFNULL(t.IsActive,0)= 1 and t.Progress = 100;";
                        ObjProjectSur = _connection.Query<ProjectSurvey>(strQuery, new { ProjectId = ProjectId }).FirstOrDefault();
                        SurveyTotCompleteObj = ObjProjectSur.SurveyTotComplete;

                        strQuery = @"select ifnull(sum(subCount), 0) SurveyTotResponseCount from (
                                    Select t.SyncTaskID, max(ifnull(t.SubmissionCount, 1)) subCount
                                    From TbProjectSurveyResponses t inner join TbProjectSurvey s on s.ProjectId = @ProjectId 
                                    and IFNULL(s.IsActive,0)= 1 and t.SurveyID = s.SurveyID Where IFNULL(t.IsActive,0)= 1
                                    group by t.SyncTaskID) aa";
                        ObjProjectSur = _connection.Query<ProjectSurvey>(strQuery, new { ProjectId = ProjectId }).FirstOrDefault();
                        SurveyTotResponseCountObj = ObjProjectSur.SurveyTotResponseCount;

                        strQuery = @"SELECT TypeID as SurveyID, ms.SyncTaskID from 
                                    TbMyTask ms inner join TbProjectSurvey s on ms.TypeID = s.SurveyID
                                    where s.ProjectId = @ProjectId  
                                    group by TypeID";

                        List<TotalCompletedCount> lstSurvey = _connection.Query<TotalCompletedCount>(strQuery, new { ProjectId = ProjectId }).ToList();

                        foreach (var items in lstSurvey)
                        {
                            var wfLevel = _connection.Query<int>("select WfNoOfLevels from TbProjectSurvey s inner join TbProjectWorkflow  w on s.WorkflowID = w.WorkflowID where s.SurveyID = @surveyId; ", new { surveyId = items.SurveyID }).FirstOrDefault();


                            if (wfLevel > 0)
                            {
                                /* strQuery = @"Select max(ifnull(t.SubmissionCount, 1)) submissionCount
                                             from TbProjectSurveyResponses t inner join 
                                             TbMyTask ms on t.SyncTaskID = ms.DataCollectionSyncDBID 
                                              inner join TbProjectSurvey ps on t.SurveyID = ps.SurveyID and IFNULL(ps.IsActive,0)= 1
                                             where ms.WFLevel =  @wfLevel
                                             and  ms.WFLevelStatus = 'Completed'
                                             and ps.SurveyID = @SurveyID
                                             group by t.SyncTaskID";*/

                                strQuery = @"Select max(ifnull(t.SubmissionCount, 1)) submissionCount, t.ResponseID
                                            from TbProjectSurveyResponses t inner join 
                                            TbMyTask ms on t.SyncTaskID = ms.DataCollectionSyncDBID 
                                             inner join TbProjectSurvey ps on ps.SurveyID = ms.TypeID and IFNULL(ps.IsActive,0)= 1
                                            where ms.WFLevel =  @wfLevel
                                            and  ms.WFLevelStatus = 'Completed'
                                            and ps.SurveyID = @SurveyID
                                            group by t.SyncTaskID";

                            }
                            else if (wfLevel == 0)
                            {
                                strQuery = @"Select max(ifnull(t.SubmissionCount, 1)) submissionCount, t.ResponseID
                                             from TbProjectSurveyResponses t inner join 
                                                    TbProjectSurvey ps on t.SurveyID = ps.SurveyID and IFNULL(ps.IsActive,0)= 1
                                                    inner join TbMyTask ms on ps.SurveyID = ms.TypeID 
                                                    where ms.WFLevel = 0
                                                    and ms.WFLevelStatus = 'Completed'
                                                    and ps.SurveyID =  @SurveyID group by t.SyncTaskID";

                            }

                            var submissionCount = _connection.Query<TotalCompletedResposeCount>(strQuery, new { SurveyID = items.SurveyID, wfLevel = wfLevel }).ToList();
                            int submissionCounttot = Convert.ToInt32(submissionCount.Sum(item => item.submissionCount));
                            SurveyTotCompleteCountObj = SurveyTotCompleteCountObj + submissionCounttot;

                        }


                        ObjProjectSur.SurveyTotResponse = SurveyTotResponseObj;
                        ObjProjectSur.SurveyTotComplete = SurveyTotCompleteObj;
                        ObjProjectSur.SurveyTotResponseCount = SurveyTotResponseCountObj;
                        ObjProjectSur.SurveyTotCompleteCount = SurveyTotCompleteCountObj;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ObjProjectSur;
        }

        /// <summary>
        /// Function used to calculate survey progress based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public double GetSurveyProgress(int ProjectId)
        {

            double SurveyPercentage = 0;
            double ProjectPercentage = 0;
            double ProjectPercentageDivision = 0;
            string strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT IFNULL(sum(Progress)/(count(SurveyID)*100)*100,0) FROM TbProjectSurvey where ProjectId=@ProjectId and IsActive=1
                                     and SurveyId not in (select SurveyId 
                                    from (
                                        select SurveyId,ProjectId from TbProjectActivity
                                        union all
                                        select SurveyId,ProjectId from TbOutcomes
                                    ) a where ProjectId=@ProjectId)";
                SurveyPercentage = _connection.Query<double>(strQuery, new { ProjectId = ProjectId }).Single();



                if (SurveyPercentage > 0)
                {
                    ProjectPercentageDivision = ProjectPercentageDivision + 100;
                }

                if (ProjectPercentageDivision > 0)
                {
                    ProjectPercentage = Math.Round(((SurveyPercentage) / ProjectPercentageDivision) * 100);

                }

                return ProjectPercentage;


            }
        }

        /// <summary>
        /// Function used to fetch all survey question details based on SurveyID,questionID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <param name="questionID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectSurvey> FetchAllSurveyQuestDetail(int SurveyID, int questionID)
        {
            // ProjectSurvey ObjProjectSurQuest = new ProjectSurvey();
            List<ProjectSurvey> ObjProjectSurQuest = new List<ProjectSurvey>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    var SurveyQuestionTotResponseObj = 0;
                    var SurveyPeriodicityObj = "";
                    var SurveyBeneficiaryObj = "";
                    var QuestionCreateDateObj = "";
                    var QuestionUpdateDateObj = "";
                    var ProgressObj = 0;
                    var EnumNameObj = "";
                    var EnumImageObj = "";
                    if (SurveyID != 0)
                    {
                        if (questionID != 0)
                        {
                            strQuery = @"select IFNULL(count(a.ID),0) as SurveyQuestionTotResponse from (Select distinct ID, ResponseID, Answer From TbProjectSurveyResponses Where IFNULL(IsActive, 0) = 1 And QuestionID = @QuestionID) a";
                            ObjProjectSurQuest = _connection.Query<ProjectSurvey>(strQuery, new { QuestionID = questionID }).ToList();
                            SurveyQuestionTotResponseObj = ObjProjectSurQuest[0].SurveyQuestionTotResponse;
                            if (ObjProjectSurQuest[0].SurveyQuestionTotResponse != 0)
                            {
                                strQuery = @"Select distinct ID,ResponseID, Answer,DATE_FORMAT(CreatedDate,'%d/%m/%Y') as QuestionCreateDate,DATE_FORMAT(Date(Case When UpdatedDate IS Null  Then CreatedDate Else  UpdatedDate End),'%d/%m/%Y') as QuestionUpdateDate From TbProjectSurveyResponses Where IFNULL(IsActive, 0) = 1 And QuestionID = @QuestionID";
                                ObjProjectSurQuest = _connection.Query<ProjectSurvey>(strQuery, new { QuestionID = questionID }).ToList();
                                if (ObjProjectSurQuest.Count > 0)
                                {
                                    QuestionCreateDateObj = ObjProjectSurQuest[0].QuestionCreateDate;
                                    QuestionUpdateDateObj = ObjProjectSurQuest[0].QuestionUpdateDate;
                                }
                            }
                        }

                        strQuery = @"select IFNULL(p.PeriodicityType,' ') as SurveyPeriodicity,IFNULL(b.Name,' ') as SurveyBeneficiary,t.Progress as SurveyProgress from TbProjectSurvey t left outer join TbPeriodicity p on p.PeriodicityId = t.ReportingFrequency left outer join TbProjectBeneficiaryType b on b.BeneficiaryTypeID = t.BeneficiaryTypeID where t.SurveyID = @SurveyID and IFNULL(t.IsActive,0)= 1";
                        ObjProjectSurQuest = _connection.Query<ProjectSurvey>(strQuery, new { SurveyID = SurveyID }).ToList();
                        if (ObjProjectSurQuest.Count > 0)
                        {
                            SurveyPeriodicityObj = ObjProjectSurQuest[0].SurveyPeriodicity;
                            SurveyBeneficiaryObj = ObjProjectSurQuest[0].SurveyBeneficiary;
                            ProgressObj = ObjProjectSurQuest[0].SurveyProgress;
                        }


                        var strQueryC = @"select IFNULL(count(user.Name),0) as cunt from TaskSyncDB TAL INNER JOIN TbUser user ON user.ID =IFNULL(TAL.DataCollectedBy,0) WHERE TAL.Status = 'DataCollected' and TAL.TypeID = @SurveyID and IFNULL(TAL.IsActive,0)= 1 and IFNULL(TAL.DataCollectedBy,0) != 0";
                        int SyncCunt = _connection.Query<int>(strQueryC, new { SurveyID = SurveyID }).Single();
                        if (SyncCunt != 0)
                        {
                            strQuery = @"select user.Name as EnumName,IFNULL(user.ProfileImage,'') AS EnumImage from TaskSyncDB TAL INNER JOIN TbUser user ON user.ID =IFNULL(TAL.DataCollectedBy,0) WHERE TAL.Status = 'DataCollected' and TAL.TypeID = @SurveyID and IFNULL(TAL.IsActive,0)= 1 and IFNULL(TAL.DataCollectedBy,0) != 0";
                            ObjProjectSurQuest = _connection.Query<ProjectSurvey>(strQuery, new { SurveyID = SurveyID }).ToList();
                            if (ObjProjectSurQuest.Count > 0)
                            {
                                EnumNameObj = ObjProjectSurQuest[0].EnumName;
                                EnumImageObj = ObjProjectSurQuest[0].EnumImage;
                            }
                        }


                        ObjProjectSurQuest[0].SurveyQuestionTotResponse = SurveyQuestionTotResponseObj;
                        ObjProjectSurQuest[0].SurveyPeriodicity = SurveyPeriodicityObj;
                        ObjProjectSurQuest[0].SurveyBeneficiary = SurveyBeneficiaryObj;
                        ObjProjectSurQuest[0].QuestionCreateDate = QuestionCreateDateObj;
                        ObjProjectSurQuest[0].QuestionUpdateDate = QuestionUpdateDateObj;
                        ObjProjectSurQuest[0].SurveyProgress = ProgressObj;
                        ObjProjectSurQuest[0].EnumName = EnumNameObj;
                        ObjProjectSurQuest[0].EnumImage = EnumImageObj;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ObjProjectSurQuest;
        }

        /// <summary>
        /// Function used to check survey exists
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="lstSurveyID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool IsProjectSurveyExists(int loggedUserId, string lstSurveyID, int Type) // To Check existance of survey in  Outcome and Output Table
        {
            bool isExists = false;
            int id = 0;
            string SurveyList = lstSurveyID;

            try
            {
                if (Type == 1)
                {
                    SurveyList = "";
                    List<ProjectSurvey> objSurveyList = JsonConvert.DeserializeObject<List<ProjectSurvey>>(lstSurveyID);
                    foreach (ProjectSurvey objSurvey in objSurveyList)
                    {
                        SurveyList += objSurvey.SurveyID + ",";
                    }
                    SurveyList = SurveyList.TrimEnd(',');
                }
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select count(a.id) as cunt from (select t.ActivityId as id from TbProjectActivity t where FIND_IN_SET(t.SurveyID,@lstSurveyID)  and t.IsDeleted = 0 union all select t.pk_OutcomeID as id from TbOutcomes t where FIND_IN_SET(t.SurveyID,@lstSurveyID) and t.IsDeleted = 0) a;";
                    id = _connection.Query<int>(strQuery, new { lstSurveyID = SurveyList }).Single();

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

        #endregion SurveySection

        public bool SavePivotPreset(int UserID, string PresetName, int SurveyID, string PresetData, int SectionId = 0)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"INSERT INTO TbPivotPresetData(UserID,PresetName,SurveyID,PresetData,CreatedDate,CreatedBy,SectionId) 
                                     VALUES (@UserID,@PresetName,@SurveyID,@PresetData,now(),@UserID,@SectionId); ";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        SurveyID = SurveyID,
                        PresetData = PresetData,
                        SectionId = SectionId
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }

            return status;
        }

        public bool EditPivotPreset(int UserID, string PresetName, int SurveyID, string PresetData)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"UPDATE TbPivotPresetData SET  PresetData = @PresetData,UpdatedDate = now(),UpdatedBy = @UserID
                                     WHERE UserID = @UserID AND PresetName = @PresetName AND SurveyID=@SurveyID; ";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        SurveyID = SurveyID,
                        PresetData = PresetData
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }

            return status;
        }

        public bool DeletePivotPreset(int UserID, string PresetName, int SurveyID)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"DELETE FROM TbPivotPresetData WHERE UserID = @UserID AND PresetName = @PresetName AND SurveyID=@SurveyID;";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        SurveyID = SurveyID
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }

            return status;
        }

        public string RestorePivotPreset(int UserID, string PresetName, int SurveyID)
        {
            string presetData = string.Empty;
            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "SELECT PresetData FROM TbPivotPresetData WHERE  PresetName = @PresetName AND SurveyID=@SurveyID;";
                    presetData = _connection.Query<string>(query, new {  PresetName = PresetName, SurveyID = SurveyID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return presetData;
        }

        //public List<string> GetPivotPresetList(int UserID, int SurveyID)
        //{
        //    List<string> pivotList = new List<string>();
        //    try
        //    {
        //        using (_connection = Database.GetDBConnection())
        //        {
        //            string query = "SELECT PresetName FROM TbPivotPresetData WHERE UserID = @UserID AND SurveyID=@SurveyID;";
        //            pivotList = _connection.Query<string>(query, new { UserID = UserID, SurveyID = SurveyID }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return pivotList;
        //}

        public List<PivotPresetData> GetPivotPresetList(int UserID, int SurveyID)
        {
            List<PivotPresetData> pivotList = new List<PivotPresetData>();
            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "SELECT PresetName,PresetData,IFNULL(SectionId,0) SectionId FROM TbPivotPresetData WHERE SurveyID=@SurveyID;";
                    pivotList = _connection.Query<PivotPresetData>(query, new { UserID = UserID, SurveyID = SurveyID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return pivotList;
        }

        public bool SaveShareLinkStatus(int SurveyID, int UserID, string ShareLinkStatus)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (ShareLinkStatus == "true")
                    {

                        int? cnt = _connection.Query<int?>(@"SELECT COUNT(UserID) FROM TbProjectSurveyUser WHERE UserID = @UserID AND SurveyID =@SurveyID;", new { SurveyID = SurveyID, UserID = UserID }).FirstOrDefault();

                        if (cnt == 0)
                        {
                            strQuery = @"INSERT INTO TbProjectSurveyUser(SurveyID,UserID,IsDeleted,CreatedDate,CreatedBy) 
                                     VALUES (@SurveyID,@UserID,0,now(),@UserID ); ";
                        }

                        //strQuery = @"INSERT IGNORE INTO TbProjectSurveyUser(SurveyID,UserID,IsDeleted,CreatedDate,CreatedBy) 
                        //             VALUES (@SurveyID,@UserID,0,now(),@UserID ) ON DUPLICATE KEY UPDATE SurveyID = @SurveyID, UserID =@UserID ; ";
                    }
                    else
                    {
                        strQuery = @"DELETE FROM TbProjectSurveyUser WHERE UserID = @UserID AND SurveyID =@SurveyID ; ";

                    }

                    _connection.Execute(strQuery, new
                    {
                        SurveyID = SurveyID,
                        UserID = UserID
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }

            return status;
        }


        #region UpdateQuestionIdSectionIdForDuplicatedSurvey
        /// <summary>
        /// UpdateQuestionIdSectionIdForDuplicatedSurvey
        /// </summary>
        /// <param name="NewSurveyID"></param>
        /// <param name="OldSurveyID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool UpdateQuestionIdSectionIdForDuplicatedSurvey(int NewSurveyID, int OldSurveyID, int loggedUserId)
        {
            string strQuery = string.Empty;
            ProjectSurvey objProjectSurvey = new ProjectSurvey();
            bool resultStatus = true;
            string strOldSectionIds = string.Empty;
            string strNewSectionIds = string.Empty;
            string strOldQstIds = string.Empty;
            string strNewQstIds = string.Empty;
            try
            {
                //    using (var _Tran = new System.Transactions.TransactionScope())
                //    {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    // select old sectionids
                    strOldSectionIds = _connection.Query<string>("SELECT GROUP_CONCAT(SectionID) FROM TbProjectSurveySection WHERE SurveyID = @oldSurveyId ORDER BY SectionID asc;",
                       new { oldSurveyId = OldSurveyID }).Single();

                    // select new sectionids
                    strNewSectionIds = _connection.Query<string>("SELECT GROUP_CONCAT(SectionID) FROM TbProjectSurveySection WHERE SurveyID = @NewSurveyId ORDER BY SectionID asc;",
                       new { NewSurveyId = NewSurveyID }).Single();

                    // select old Questionids
                    strOldQstIds = _connection.Query<string>("SELECT GROUP_CONCAT(QuestionID) FROM TbProjectSurveyQuestion WHERE SurveyID = @oldSurveyId ORDER BY QuestionID asc;",
                       new { oldSurveyId = OldSurveyID }).Single();

                    // select new Questionids
                    strNewQstIds = _connection.Query<string>("SELECT GROUP_CONCAT(QuestionID) FROM TbProjectSurveyQuestion WHERE SurveyID = @NewSurveyId ORDER BY QuestionID asc;",
                       new { NewSurveyId = NewSurveyID }).FirstOrDefault();
                }

                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (!string.IsNullOrEmpty(strOldSectionIds) && !string.IsNullOrEmpty(strOldQstIds))
                    {
                        string[] spltOldSectionIds = strOldSectionIds.Split(',');
                        string[] spltNewSectionIds = strNewSectionIds.Split(',');

                        for (int i = 0; i < spltOldSectionIds.Length; i++)
                        {
                            _connection.Query<int>("UPDATE TbProjectSurveyQuestion SET SectionID  = @spltNewSectionIds WHERE SurveyID = @NewSurveyID AND SectionID  = @oldSectionId;", new { spltNewSectionIds = spltNewSectionIds[i], NewSurveyID = NewSurveyID, oldSectionId = spltOldSectionIds[i] });
                            _connection.Query<int>("UPDATE TbProjectSurveySectionLogic SET SectionID = @spltNewSectionIds WHERE SurveyID = @NewSurveyID AND SectionID  = @oldSectionId;", new { spltNewSectionIds = spltNewSectionIds[i], NewSurveyID = NewSurveyID, oldSectionId = spltOldSectionIds[i] });
                        }
                    }

                    if (!string.IsNullOrEmpty(strOldQstIds) && !string.IsNullOrEmpty(strNewQstIds))
                    {
                        string[] spltOldQstIds = strOldQstIds.Split(',');
                        string[] spltNewQstIds = strNewQstIds.Split(',');

                        for (int i = 0; i < spltOldQstIds.Length; i++)
                        {
                            _connection.Query<int>("UPDATE TbProjectSurveySectionLogic SET QuestionID = @NewQstIds WHERE SurveyID = @NewSurveyID AND QuestionID  = @OldQstIds;", new { NewQstIds = spltNewQstIds[i], NewSurveyID = NewSurveyID, OldQstIds = spltOldQstIds[i] });
                            _connection.Query<int>("UPDATE TbProjectSurveyQSTOption SET QuestionID = @NewQstIds WHERE QuestionID  = @OldQstIds AND CreatedBy = @NewSurveyID;", new { NewQstIds = spltNewQstIds[i], OldQstIds = spltOldQstIds[i], NewSurveyID = NewSurveyID });
                            _connection.Query<int>("UPDATE TbProjectSurveyQSTCondition SET QuestionID = @NewQstIds WHERE QuestionID  = @OldQstIds AND CreatedBy = @NewSurveyID;", new { NewQstIds = spltNewQstIds[i], OldQstIds = spltOldQstIds[i], NewSurveyID = NewSurveyID });
                        }
                    }

                    _connection.Query<int>("UPDATE TbProjectSurveyQSTCondition SET CreatedBy = @NewCreatedBy WHERE CreatedBy  = @SurveyId;", new { NewCreatedBy = loggedUserId, SurveyId = NewSurveyID });
                    _connection.Query<int>("UPDATE TbProjectSurveySectionLogic SET CreatedBy = @NewCreatedBy WHERE CreatedBy  = @SurveyId;", new { NewCreatedBy = loggedUserId, SurveyId = NewSurveyID });
                    _connection.Query<int>("UPDATE TbProjectSurveyQSTOption SET CreatedBy = @NewCreatedBy WHERE CreatedBy  = @SurveyId;", new { NewCreatedBy = loggedUserId, SurveyId = NewSurveyID });
                }
                //    _Tran.Complete();
                //}
            }
            catch (Exception ex)
            {
                resultStatus = false;
                throw ex;
            }

            return resultStatus;
        }
        #endregion UpdateQuestionIdSectionIdForDuplicatedSurvey

        #region GetSurveySectionLogicSectionId
        /// <summary>
        /// GetSurveySectionLogicSectionId
        /// </summary>
        /// <param name="SurveyID">SurveyID</param>
        /// <returns></returns>
        public List<SurveyQuestion> GetSurveySectionLogicSectionId(int SurveyID)
        {
            List<SurveyQuestion> lstSurveyQuestion = new List<SurveyQuestion>();

            try
            {
                string qry = "SELECT distinct QST.QuestionID,QST.SectionID from TbProjectSurveyQuestion QST  ";
                qry += "JOIN (SELECT distinct SectionID  FROM TbProjectSurveySectionLogic WHERE SurveyID = @SurveyId AND IFNULL(IsActive,0) =1)";
                qry += " ST ON QST.SectionID = ST.SectionID WHERE  QST.SurveyID = @SurveyId AND ifnull(QST.IsActive,0) =1";

                using (_connection = Utils.Database.GetDBConnection())
                {
                    lstSurveyQuestion = _connection.Query<SurveyQuestion>(qry,
                       new { SurveyId = SurveyID }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstSurveyQuestion;
        }
        #endregion GetSurveySectionLogicSectionId


        #region GetSurveyRepeatSection
        /// <summary>
        /// GetSurveyRepeatSection
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetSurveyRepeatSection(string SurveyId = "")
        {
            List<KeyValuePair<int, string>> lstproject = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (SurveyId.Length > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT sect.SectionID,sect.SectionName FROM TbProjectSurveySection sect  ";
                        query += " JOIN TbProjectSurveySectionLogic logic ON sect.SectionID = logic.SectionID  ";
                        query += " WHERE sect.SurveyID in(@SurveyID) AND IFNULL(sect.IsActive,0) =@IsActive AND IFNULL(logic.IsActive,0) =@IsActive AND ifnull(logic.LogicType,'') = 'Repeat Logic';";

                        obj = _connection.Query<object>(query, new { IsActive = 1, SurveyID = SurveyId }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstproject.Add(new KeyValuePair<int, string>(item.SectionID, item.SectionName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstproject;
        }
        #endregion GetOutComeProjectData   

        #region IsSurveyQuestion
        /// <summary>
        /// IsSurveyQuestion
        /// </summary>
        /// <param name="SectionId"></param>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public bool IsSurveyQuestion(int SectionId, int QuestionId)
        {
            int QuestionID = 0;
            bool Status = false;

            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "select QuestionID FROM  TbProjectSurveyQuestion WHERE QuestionID = @QuestionID AND  SectionID = @SectionID";
                    QuestionID = _connection.Query<int>(query, new { QuestionID = QuestionId, SectionID = SectionId }).FirstOrDefault();
                    
                    if (QuestionID > 0)
                    {
                        Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Status;
        }
        #endregion IsSurveyQuestion
    }
}
