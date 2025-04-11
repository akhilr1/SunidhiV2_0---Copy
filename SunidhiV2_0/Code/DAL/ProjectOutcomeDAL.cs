using SunidhiV2_0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using SunidhiV2_0.Code.Common;

namespace SunidhiV2_0.Code.DAL
{
    public class ProjectOutcomeDAL
    {
        protected IDbConnection _connection;
        static protected IDbConnection _conn;

        /// <summary>
        /// Function used to fetch all SDG details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDG> FetchAllSDG()
        {
            List<SDG> lstSDG = new List<SDG>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_SDGID,SDG_Name,Status,0 as IsSelected FROM TbSDGMaster Order by pk_SDGID Desc";
                    lstSDG = _connection.Query<SDG>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return lstSDG;
        }

        /// <summary>
        /// Function used to fetch all SDG target details based on SDGId
        /// </summary>
        /// <param name="SDGId"></param>
        /// <returns></returns>
        public IEnumerable<SDGTargets> FetchSDGTargetsBasedOnSDGId(int SDGId)
        {
            List<SDGTargets> lstSDGTargets = new List<SDGTargets>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_SDGTargetID,fk_SDGID,Target_Name,Status FROM TbSDGTargets where fk_SDGID =@SDGId Order by pk_SDGTargetID Desc";
                    lstSDGTargets = _connection.Query<SDGTargets>(strQuery, new { SDGId = SDGId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstSDGTargets;
        }

        /// <summary>
        /// Function used to fetch SDG target indicators details based on SDGTargetId
        /// </summary>
        /// <param name="SDGTargetId"></param>
        /// <returns></returns>
        public IEnumerable<SDGTargetIndicators> FetchSDGTargetIndicatorsBasedOnSDGTargetId(int SDGTargetId)
        {
            List<SDGTargetIndicators> lstSDGTargetIndicators = new List<SDGTargetIndicators>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_Indicaters,fk_SDGTargetID,SDGID,Indicator,Status FROM TbSDGTarget_Indicators where fk_SDGTargetID =@SDGTargetId  Order by pk_Indicaters Desc";
                    lstSDGTargetIndicators = _connection.Query<SDGTargetIndicators>(strQuery, new { SDGTargetId = SDGTargetId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstSDGTargetIndicators;
        }

        /// <summary>
        /// Function used to fetch SD target details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDGTargets> FetchAllSDGTargets()
        {
            List<SDGTargets> lstSDGTargets = new List<SDGTargets>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_SDGTargetID,fk_SDGID,Target_Name,Status FROM TbSDGTargets Order by pk_SDGTargetID Desc";
                    lstSDGTargets = _connection.Query<SDGTargets>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstSDGTargets;
        }

        /// <summary>
        /// Function used to fetch SDG target indicators details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDGTargetIndicators> FetchAllSDGTargetIndicators()
        {
            List<SDGTargetIndicators> lstSDGTargetIndicators = new List<SDGTargetIndicators>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_Indicaters,fk_SDGTargetID,SDGID,Indicator,Status FROM TbSDGTarget_Indicators Order by pk_Indicaters Desc";
                    lstSDGTargetIndicators = _connection.Query<SDGTargetIndicators>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstSDGTargetIndicators;
        }

        /// <summary>
        /// Add new outcome
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int CreateOutcome(ProjectOutcome objOutcome, int loggedUserId)
        {
            string query = string.Empty;
            var Outcomeid = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "insert into TbOutcomes(ProjectId,OutcomeName,StartDate,EndDate,DatacollectionType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget,WorkflowId,Periodicity,BeneficiaryTypeQuestionID,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)values(@ProjectId,@OutcomeName,@StartDate,@EndDate,@DatacollectionType,@SurveyId,@SurveySectionId,@SurveyQuestionId,@SurveyResponseId,@SurveyLogic,@SurveyTarget,@WorkflowId,@Periodicity,@BeneficiaryTypeQuestionID,@IsDeleted,now(),now(),@CreatedBy,@UpdatedBy);SELECT LAST_INSERT_ID();";
                        Outcomeid = _connection.Query<int>(query, new
                        {
                            ProjectId = objOutcome.ProjectID,
                            OutcomeName = objOutcome.OutcomeName,
                            StartDate = objOutcome.StartDate,
                            EndDate = objOutcome.EndDate,
                            DatacollectionType = objOutcome.DatacollectionType,
                            SurveyId = objOutcome.SurveyId,
                            SurveyQuestionId = objOutcome.SurveyQuestionId,
                            SurveySectionId = objOutcome.SurveySectionId,
                            SurveyResponseId = 0,//objOutcome.SurveyResponseId,
                            SurveyLogic = objOutcome.SurveyLogic,
                            SurveyTarget = objOutcome.SurveyTarget,
                            WorkflowId = objOutcome.WorkflowId,
                            Periodicity = objOutcome.Periodicity,
                            BeneficiaryTypeQuestionID = objOutcome.BeneficiaryTypeQuestionID,
                            IsDeleted = 0,
                            CreatedBy = loggedUserId,
                            UpdatedBy = loggedUserId
                        }).Single();

                        if (Outcomeid != 0)
                        {

                            InsertIntoTbOutcome_SDG(objOutcome, Outcomeid);

                            InsertIntoTbOutcome_SDG_Targets(objOutcome, Outcomeid);

                            InsertIntoTbOutcomeAssignUsers(objOutcome, Outcomeid, loggedUserId);

                            fnAssignUserstoOutcome(objOutcome, loggedUserId, Outcomeid, "Insert");

                            // Insert into Sync Db
                            //string Qeury = @"Select TbOutcomes.WorkflowId,TbProjectWorkflow.WfNoOfLevels From TbOutcomes TbOutcomes 
                            //     Left Join TbProjectWorkflow  TbProjectWorkflow On TbProjectWorkflow.WorkflowID = TbOutcomes.WorkflowId
                            //     Where TbOutcomes.IsDeleted = 0 And TbProjectWorkflow.IsDeleted = 0 And TbOutcomes.pk_OutcomeID = @Outcomeid";
                            //dynamic objWF = _connection.Query<object>(Qeury, new { Outcomeid = Outcomeid }).FirstOrDefault();

                            string Qeury = @"Select ifnull(TbProjectWorkflow.WfNoOfLevels,0) WfNoOfLevels From TbProjectWorkflow  TbProjectWorkflow where TbProjectWorkflow.WorkflowID = @WorkflowId And TbProjectWorkflow.IsDeleted = 0";
                            var WorkflowLevel = _connection.Query<int>(Qeury, new { WorkflowId = objOutcome.WorkflowId }).FirstOrDefault();

                            Qeury = @"Select WSID as WorkspaceID From TbProject  Where ProjectId = @ProjectId";
                            var WID = _connection.Query<int>(Qeury, new { ProjectId = objOutcome.ProjectID }).FirstOrDefault();
                            //AddTo SyncDB
                            var strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                            var syncDBId = _connection.Query<int>(strQuery, new
                            {
                                Type = "Outcome",
                                Command = "ADD",
                                TypeIDColumn = "pk_OutcomeID",
                                TypeID = Outcomeid,
                                WorkflowID = objOutcome.WorkflowId,
                                FinanceWorkflowID = 0,
                                WfNoOfLevels = WorkflowLevel,
                                Status = "Pending",
                                WorkspaceID = WID,
                                CreatedBy = loggedUserId
                            }).Single();

                            // Send Notification 

                            //AddToActivityLog(inputObj.ProjectId, actid, "Created a new activity named " + inputObj.ActivityName, loggedUserId);
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = objOutcome.ProjectID;
                            objLog.AuditFrom = "Outcome";
                            objLog.AuditEvent = "ADD";
                            objLog.AuditFromID = Outcomeid;
                            objLog.FromIDColumnName = "pk_OutcomeID";
                            objLog.Message = "Created a new Outcome named " + objOutcome.OutcomeName;
                            objLog.CreatedBy = loggedUserId;
                            ProjectDAL.AddAuditLog(objLog);
                        }


                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return Outcomeid;
        }

        /// <summary>
        /// Function used to update outcome based on OutcomeId
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int UpdateOutcome(ProjectOutcome objOutcome, int loggedUserId)
        {
            string query = string.Empty;
            var Outcomeid = objOutcome.pk_OutcomeID;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "Update  TbOutcomes set OutcomeName = @OutcomeName , StartDate = @StartDate,EndDate =@EndDate,DatacollectionType = @DatacollectionType ,SurveyId = @SurveyId,SurveySectionId = @SurveySectionId,SurveyQuestionId = @SurveyQuestionId,SurveyResponseId = @SurveyResponseId ,SurveyLogic = @SurveyLogic,SurveyTarget = @SurveyTarget,WorkflowId = @WorkflowId ,Periodicity = @Periodicity,BeneficiaryTypeQuestionID = @BeneficiaryTypeQuestionID,IsDeleted = @IsDeleted,CreatedDate = now(),UpdatedDate = now(),CreatedBy = @CreatedBy,UpdatedBy = @UpdatedBy where pk_OutcomeID = @OutcomeId ";
                        _connection.Query<int>(query, new
                        {

                            OutcomeName = objOutcome.OutcomeName,
                            StartDate = objOutcome.StartDate,
                            EndDate = objOutcome.EndDate,
                            DatacollectionType = objOutcome.DatacollectionType,
                            SurveyId = objOutcome.SurveyId,
                            SurveyQuestionId = objOutcome.SurveyQuestionId,
                            SurveySectionId = objOutcome.SurveySectionId,
                            SurveyResponseId = 0,//objOutcome.SurveyResponseId,
                            SurveyLogic = objOutcome.SurveyLogic,
                            SurveyTarget = objOutcome.SurveyTarget,
                            WorkflowId = objOutcome.WorkflowId,
                            Periodicity = objOutcome.Periodicity,
                            BeneficiaryTypeQuestionID = objOutcome.BeneficiaryTypeQuestionID,
                            IsDeleted = 0,
                            CreatedBy = loggedUserId,
                            UpdatedBy = loggedUserId,
                            OutcomeId = objOutcome.pk_OutcomeID
                        });

                        InsertIntoTbOutcome_SDG(objOutcome, Outcomeid);
                        InsertIntoTbOutcome_SDG_Targets(objOutcome, Outcomeid);
                        InsertIntoTbOutcomeAssignUsers(objOutcome, Outcomeid, loggedUserId);
                        fnAssignUserstoOutcome(objOutcome, loggedUserId, Outcomeid, "Update");

                        // Updating Workflow ID to Taskdb
                        string Query = @"Select WfNoOfLevels  From  TbProjectWorkflow  Where IsDeleted = 0 And WorkflowID = @WorkflowID";
                        var WfNoOfLevels = _connection.Query<int>(Query, new { WorkflowID = objOutcome.WorkflowId }).FirstOrDefault();

                        ////Update To SyncDB
                        string strQuery = @"Update TaskSyncDB SET  WorkflowID =@WorkflowID,TotalWFLevel =@TotalWFLevel WHERE TypeID = @TypeID  And Type =@Type ";
                        var syncDBId = _connection.Execute(strQuery, new
                        {
                            Type = "Outcome",
                            TypeID = objOutcome.pk_OutcomeID,
                            WorkflowID = objOutcome.WorkflowId,
                            TotalWFLevel = WfNoOfLevels

                        });

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return Outcomeid;
        }

        /// <summary>
        /// Function used to delete already exists data and insert outcome SDG details based on Outcomeid
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="Outcomeid"></param>
        public void InsertIntoTbOutcome_SDG(ProjectOutcome objOutcome, int Outcomeid)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        // Delete -- Making IsDelete = 1
                        string strDeleteQuery = string.Empty;
                        strDeleteQuery = "Update TbOutcome_SDG set IsDeleted = 1 where OutcomeID = @Outcomeid";
                        _connection.Query<int>(strDeleteQuery, new { Outcomeid = Outcomeid });

                        // Insert into TbOutcome_SDG
                        List<SDG> lstSDGList = JsonConvert.DeserializeObject<List<SDG>>(objOutcome.SDGList);
                        string QuertyStmt = string.Empty;
                        foreach (SDG item in lstSDGList)
                        {

                            QuertyStmt = @"INSERT INTO TbOutcome_SDG(OutcomeID, SDGID, Status, IsDeleted)
                                  VALUES(@OutcomeID, @SDGID, @Status,@IsDeleted); SELECT LAST_INSERT_ID();";
                            var TbOutcome_SDGID = _connection.Query<int>(QuertyStmt, new
                            {
                                OutcomeID = Outcomeid,
                                SDGID = item.pk_SDGID,
                                Status = 0,
                                IsDeleted = 0,
                            }).Single();

                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// Function used to delete already exists data and insert outcome SDG target details based on Outcomeid
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="Outcomeid"></param>
        public void InsertIntoTbOutcome_SDG_Targets(ProjectOutcome objOutcome, int Outcomeid)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        // Delete -- Making IsDelete = 1
                        string strDeleteQuery = "Update TbOutcome_SDG_Targets set IsDeleted = 1 where OutcomeID = @Outcomeid";
                        _connection.Query<int>(strDeleteQuery, new { Outcomeid = Outcomeid });

                        var QuertyStmt = string.Empty;
                        // Insert into TbOutcome_SDG_Targets
                        List<SDGTargets> lstSDGList_Targets = JsonConvert.DeserializeObject<List<SDGTargets>>(objOutcome.SDGTargetsList);
                        string QuertyStmt2 = string.Empty;
                        if (lstSDGList_Targets != null)
                        {
                            foreach (SDGTargets item in lstSDGList_Targets)
                            {
                                QuertyStmt = @"INSERT INTO TbOutcome_SDG_Targets(OutcomeID, SDGID, SDGTargetID,Status, IsDeleted)
                                  VALUES(@OutcomeID, @SDGID, @SDGTargetID,@Status,@IsDeleted); SELECT LAST_INSERT_ID();";
                                var TbOutcome_SDGID = _connection.Query<int>(QuertyStmt, new
                                {
                                    OutcomeID = Outcomeid,
                                    SDGID = item.fk_SDGID,
                                    SDGTargetID = item.pk_SDGTargetID,
                                    Status = 0,
                                    IsDeleted = 0,
                                }).Single();
                            }
                        }
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// Function used to delete already exists data and insert outcome assign users based on Outcomeid
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="Outcomeid"></param>
        /// <param name="loggedUserId"></param>
        public void InsertIntoTbOutcomeAssignUsers(ProjectOutcome objOutcome, int Outcomeid, int loggedUserId)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        if (objOutcome.UserList != null)
                        {
                            // Delete -- Making IsDelete = 1
                            string strDeleteQuery = "Update TbOutcomeAssignUsers set IsDeleted = 1 ,UpdatedDate = now() where OutcomeId = @Outcomeid";
                            _connection.Query<int>(strDeleteQuery, new { Outcomeid = Outcomeid });

                            // insert into  TbOutcomeAssignUsers
                            List<UsersDetails> lstUser = JsonConvert.DeserializeObject<List<UsersDetails>>(objOutcome.UserList);

                            foreach (UsersDetails userItem in lstUser)
                            {
                                //string[] UserData = objOutcome.UserList.Split(',');
                                string QuertyStmt = string.Empty;
                                //if (UserData.Length > 0)
                                //{
                                //for (var Count = 0; Count <= lstUser.Length - 1; Count++)
                                //{
                                QuertyStmt = @"INSERT INTO TbOutcomeAssignUsers(OutcomeId,ProjectId,UserID,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)
                                  VALUES(@OutcomeID, @ProjectId, @UserID,@IsDeleted,now(),now(),@CreatedBy,@UpdatedBy); SELECT LAST_INSERT_ID();";
                                var TbOutcome_SDGID = _connection.Query<int>(QuertyStmt, new
                                {
                                    OutcomeID = Outcomeid,
                                    ProjectId = objOutcome.ProjectID,
                                    UserID = userItem.ID,//UserData[Count],//item.ID,
                                    IsDeleted = 0,
                                    CreatedBy = loggedUserId,
                                    UpdatedBy = loggedUserId,
                                }).Single();
                            }
                            //}
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Function used to create outcome frequency track date
        /// </summary>
        /// <param name="objOutcome"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public bool GenerateReportingFrequencyTrackDate(ProjectOutcome objOutcome, int loggedUserId, int OutcomeId)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    bool isDeleted = DeleteOutcomeTrackDate(OutcomeId);

                    switch (objOutcome.Periodicity)
                    {
                        case (int)CommonActivity.ReportingFrequency.Once:
                            DateTime? reportvalue = CommonActivity.GetDateTimeValue(objOutcome.ReportingOnceDate);
                            lstTrackDates.Add(reportvalue.Value.ToString("yyyy-MM-dd"));
                            break;
                        case (int)CommonActivity.ReportingFrequency.Daily:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, 0, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Weekly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingWeek, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Monthly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonthDay, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Quarterly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonthDateDay, objOutcome.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonthDateDay, objOutcome.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objOutcome.StartDate, objOutcome.EndDate, objOutcome.Periodicity, objOutcome.ReportingMonthDateDay, objOutcome.ReportingMonth);
                            break;
                    }

                    if (lstTrackDates != null && lstTrackDates.Count > 0)
                    {
                        query = "INSERT INTO TbOutcomeTrackDate(TrackDate,OutcomeId,TrackStatus) ";

                        for (int i = 0; i < lstTrackDates.Count; i++)
                        {
                            query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + OutcomeId + ",0" : " SELECT '" + lstTrackDates[i] + "'," + OutcomeId + ",0";
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

        /// <summary>
        /// Function used to delete outcome track date based on OutcomeId
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public bool DeleteOutcomeTrackDate(int OutcomeId)
        {
            try
            {
                string query = string.Empty;

                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "DELETE FROM TbOutcomeTrackDate WHERE OutcomeId=@OutcomeId ";
                    _connection.Execute(strQuery, new { OutcomeId = OutcomeId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        #region AuditLog

        /// <summary>
        /// Add new auditlog data
        /// </summary>
        /// <param name="objPrjAuditLog"></param>
        /// <returns></returns>
        public static string AddAuditLog(ProjectAuditLog objPrjAuditLog)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            try
            {
                using (_conn = Utils.Database.GetDBConnection())
                {
                    try
                    {
                        strQuery = " INSERT INTO TbProjectAuditLog(ProjectID,AuditFrom,AuditEvent,AuditFromID,FromIDColumnName,Message,CreatedDate,CreatedBy) " +
                                   "  VALUES(@ProjectID,@AuditFrom,@AuditEvent,@AuditFromID,@FromIDColumnName,@Message,now(),@loggedUserId);SELECT LAST_INSERT_ID();";
                        id = _conn.Query<int>(strQuery, new
                        {
                            ProjectID = objPrjAuditLog.ProjectID,
                            AuditFrom = objPrjAuditLog.AuditFrom,
                            AuditEvent = objPrjAuditLog.AuditEvent,
                            AuditFromID = objPrjAuditLog.AuditFromID,
                            FromIDColumnName = objPrjAuditLog.FromIDColumnName,
                            Message = objPrjAuditLog.Message,
                            loggedUserId = objPrjAuditLog.CreatedBy
                        }).Single();
                    }

                    catch (Exception ex)
                    {
                        retStatus = "Failed while saving audit log...";
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return retStatus;
        }

        #endregion AuditLog

        /// <summary>
        /// Function used to fetch all outcome details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<ProjectOutcome> FetchAllOutcomeList(int ProjectId)
        {
            List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();
            string strQuery = string.Empty;
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.pk_OutcomeID ActivityId,TbOutcomes.ProjectID, TbOutcomes.OutcomeName ActivityName,TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                            TbOutcomes.DatacollectionType ActivityType, TbOutcomes.DatacollectionType ,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                                 TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                            TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,ifnull(TbOutcomes.UpdatedDate,TbOutcomes.CreatedDate) UpdatedDate,TbPeriodicity.PeriodicityType as ReportingFrequencyName,TbOutcomes.Description,
                            ifnull(TbOutcomes.UnitOfMeasure,'') UnitOfMeasure,ifnull(TbOutcomes.AdditionalSectionId,0) AdditionalSectionId,ifnull(TbOutcomes.AdditionalQuestionId,0) AdditionalQuestionId,ifnull(TbOutcomes.AdditionalQuestionType,false) as AdditionalQuestionType,ifnull(AutoGeneratedActivityId,'') as AutoGeneratedActivityId,IFNULL(SDG_ID,0) AS SDGID  
                            from TbOutcomes TbOutcomes 
                            left join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                            left join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                            left join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                            where TbOutcomes.IsDeleted = 0 and TbOutcomes.ProjectId = @ProjectId
                            order by TbOutcomes.pk_OutcomeID desc";
                    ObjOutcome = _connection.Query<ProjectOutcome>(strQuery, new { ProjectId = ProjectId }).ToList();

                    foreach (ProjectOutcome Item in ObjOutcome)
                    {
                        if (Item.DatacollectionType == "Quantitative")
                        {
                            double result = getData(Item.ActivityId, Item.SurveyLogic, Item.SurveyTarget, Convert.ToInt32(Item.AdditionalSectionId), Item.AdditionalQuestionId, Item.AdditionalQuestionType);
                            Item.ActualValue = result;
                            int totPercentage = (int)Math.Round((result * 100) / Convert.ToDouble(Item.SurveyTarget));
                            Item.Progress = (totPercentage > 100 ? 100 : totPercentage);

                            Item.OutcomeQuantitativeLogic = new List<OutcomeQuantitativeLogic>();
                            Item.OutcomeQuantitativeLogic = GetQuantitativeData(Item.ActivityId);
                        }

                        Item.lstProjectOutcomeMonthwiseData = new List<ProjectOutcomeMonthwiseData>();
                        Item.lstProjectOutcomeMonthwiseData = GetProjectOutcomeMonthwiseData(Item.ActivityId);

                        Item.ActivityUsers = new List<User>();

                        query = @"select distinct t.ID,Name, t.EmailId, t.Phone, t.Password, t.IsActive, t.NoOfLogins, t.FailedLoginCount,t.UserType,
                                    IFNULL(t.ProfileImage, '') as ProfileImage ,Organization from TbUser t inner join 
                                    TbOutcomeAssignUsers u on u.UserID = t.ID and u.IsDeleted <>1 and u.OutcomeId= @actid Order By t.ID Desc";

                        Item.ActivityUsers = _connection.Query<User>(query, new { actid = Item.ActivityId }).ToList();

                        //Get Expense tag List
                        Item.ExpenseTags = new List<Tags>();

                        query = @"select distinct t.TagID,t.TagName from TbProjectExpenseTags t inner join TbOutcomeToExpenseTags e where e.ExpenseTagID = t.TagID
                                  and e.IsDeleted <>1 and e.OutcomeId=@actid Order By t.TagID Desc";

                        Item.ExpenseTags = _connection.Query<Tags>(query, new { actid = Item.ActivityId }).ToList();

                        //Get Activity tag List
                        Item.ActivityTags = new List<Tags>();

                        query = @"select distinct t.TagID,t.TagName,t.TagColor from TbProjectTags t inner join TbOutcomeToTags p where p.TagID = t.TagID and p.IsDeleted <>1 and 
                                     p.OutcomeId=@actid Order By t.TagID Desc";

                        Item.ActivityTags = _connection.Query<Tags>(query, new { actid = Item.ActivityId }).ToList();

                        //Get Periodicity parameters
                        DateTime? RptDate = null;
                        dynamic obj;
                        string strQeury = @"SELECT TrackDate FROM TbOutcomeTrackDate WHERE OutcomeId = @OutcomeId order by ID LIMIT 1";
                        obj = _connection.Query<object>(strQeury, new { OutcomeId = Item.ActivityId }).FirstOrDefault();

                        if (obj != null)
                        {
                            RptDate = obj.TrackDate;
                        }

                        if (RptDate != null)
                        {
                            switch (Item.Periodicity)
                            {
                                case (int)CommonActivity.ReportingFrequency.Once:
                                    Item.PeriodicityOnceDate = RptDate.Value;
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Weekly:
                                    Item.PeriodicityWeek = (int)RptDate.Value.DayOfWeek;
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Monthly:
                                    Item.PeriodicityMonthDay = RptDate.Value.Day;
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Quarterly:
                                    Item.PeriodicityMonthDateDay = RptDate.Value.Day;
                                    Item.PeriodicityMonth = RptDate.Value.Month;
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                    Item.PeriodicityMonthDateDay = RptDate.Value.Day;
                                    Item.PeriodicityMonth = RptDate.Value.Month;
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Yearly:
                                    Item.PeriodicityMonthDateDay = RptDate.Value.Day;
                                    Item.PeriodicityMonth = RptDate.Value.Month;
                                    break;
                            }
                        }
                    }


                    //    // Fetching SGD List
                    //    strQuery = @"select SDGMaster.SDG_Name,SDGMaster.pk_SDGID from TbOutcome_SDG Outcome_SDG inner join TbSDGMaster SDGMaster on Outcome_SDG.SDGID = SDGMaster.pk_SDGID 
                    //  where   Outcome_SDG.IsDeleted=0 and Outcome_SDG.OutcomeId = @OutcomeId Order By Outcome_SDG.ID Desc";
                    //    var SDGs = _connection.Query<SDG>(strQuery, new { OutcomeId = Item.pk_OutcomeID }).ToList();

                    //    Item.SDGList = JsonConvert.SerializeObject(SDGs);

                    //    // Fetching SDG_Targets
                    //    strQuery = @"select SDGTargets.Target_Name,SDGTargets.pk_SDGTargetID,fk_SDGID,SDG_Targets.Status from TbOutcome_SDG_Targets SDG_Targets inner join TbSDGTargets SDGTargets on SDG_Targets.SDGTargetID = SDGTargets.pk_SDGTargetID 
                    //  where   SDG_Targets.IsDeleted=0 and SDG_Targets.OutcomeID = @OutcomeId Order By SDG_Targets.ID Desc";
                    //    var SDG_Targets = _connection.Query<SDGTargets>(strQuery, new { OutcomeId = Item.pk_OutcomeID }).ToList();

                    //    Item.SDGTargetsList = JsonConvert.SerializeObject(SDG_Targets);
                    //}
                }
            }
            catch (Exception ex)
            {
            }
            return ObjOutcome;
        }

        /// <summary>
        /// Function used to fetch all outcome details based on ProjectId,OutcomeId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public ProjectOutcome FetchAllOutcomeListBasedOnOutComeId(int ProjectId, int OutcomeId)
        {
            ProjectOutcome ObjOutcome = new ProjectOutcome();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.ProjectID, TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                    //                TbOutcomes.DatacollectionType,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                    //                TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                    //                TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,TbOutcomes.UpdatedDate, TbPeriodicity.PeriodicityType as ReportingFrequencyName,
                    //                TbProjectSurveySection.SectionName as SectionName, TbLogicType.Type as SurveyLogicName , TbProjectSurveyQuestion.QuestionName as QuestionName
                    //                from TbOutcomes TbOutcomes 
                    //                inner join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                    //                inner join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                    //                inner join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                    //                left join TbProjectSurveySection TbProjectSurveySection on TbProjectSurveySection.SectionID = TbOutcomes.SurveySectionId
                    //                left join TbLogicType TbLogicType on TbLogicType.ID = TbOutcomes.SurveyLogic
                    //                left join TbProjectSurveyQuestion TbProjectSurveyQuestion on TbProjectSurveyQuestion.QuestionID = TbOutcomes.SurveyQuestionId
                    //                where TbOutcomes.pk_OutcomeID = @OutcomeId and TbOutcomes.IsDeleted = 0 and TbOutcomes.ProjectId = @ProjectId and TbProjectWorkflow.IsDeleted = 0
                    //                order by TbOutcomes.pk_OutcomeID desc";
                    strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.ProjectID, TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                                TbOutcomes.DatacollectionType,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                                TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                                TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,TbOutcomes.UpdatedDate, TbPeriodicity.PeriodicityType as ReportingFrequencyName,
                                TbProjectSurveySection.SectionName as SectionName, TbLogicType.Type as SurveyLogicName , TbProjectSurveyQuestion.QuestionName as QuestionName, TbOutcomes.BeneficiaryTypeQuestionID, TbProjectSurvey.BeneficiaryTypeID as BeneficiaryTypeID
                                from TbOutcomes TbOutcomes 
                                left join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                                left join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                                left join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                                left join TbProjectSurveySection TbProjectSurveySection on TbProjectSurveySection.SectionID = TbOutcomes.SurveySectionId
                                left join TbLogicType TbLogicType on TbLogicType.ID = TbOutcomes.SurveyLogic
                                left join TbProjectSurveyQuestion TbProjectSurveyQuestion on TbProjectSurveyQuestion.QuestionID = TbOutcomes.SurveyQuestionId
                                where TbOutcomes.pk_OutcomeID = @OutcomeId and TbOutcomes.IsDeleted = 0 and TbOutcomes.ProjectId = @ProjectId 
                                order by TbOutcomes.pk_OutcomeID desc";
                    ObjOutcome = _connection.Query<ProjectOutcome>(strQuery, new { OutcomeId = OutcomeId, ProjectId = ProjectId }).FirstOrDefault();


                    // Fetching Userlist
                    strQuery = @"select TbUser.Name,TbOutcomeAssignUsers.UserID as ID,IFNULL(TbUser.ProfileImage, '') as ProfileImage from TbOutcomeAssignUsers TbOutcomeAssignUsers inner join TbUser TbUser on TbOutcomeAssignUsers.UserID = TbUser.ID inner join TbOutcomes TbOutcomes on TbOutcomeAssignUsers.OutcomeId = TbOutcomes.pk_OutcomeID
                      where TbOutcomes.ProjectID=@ProjectId and TbOutcomeAssignUsers.IsDeleted=0 and TbOutcomeAssignUsers.OutcomeId = @OutcomeId Order By TbOutcomeAssignUsers.ID Desc";
                    ObjOutcome.lstUserList = _connection.Query<UsersDetails>(strQuery, new { ProjectId = ProjectId, OutcomeId = OutcomeId }).ToList();
                    //var NotifyUsers = _connection.Query<UsersDetails>(strQuery, new
                    //{
                    //    ProjectId = ProjectId,
                    //    OutcomeId = OutcomeId
                    //}).ToList();
                    //ObjOutcome.UserList = JsonConvert.SerializeObject(NotifyUsers);

                    // Fetching SGD List
                    strQuery = @"select SDGMaster.SDG_Name,SDGMaster.pk_SDGID from TbOutcome_SDG Outcome_SDG inner join TbSDGMaster SDGMaster on Outcome_SDG.SDGID = SDGMaster.pk_SDGID 
                      where   Outcome_SDG.IsDeleted=0 and Outcome_SDG.OutcomeId = @OutcomeId Order By Outcome_SDG.ID Desc";
                    var SDGs = _connection.Query<SDG>(strQuery, new { OutcomeId = OutcomeId }).ToList();
                    if (SDGs.Count > 0)
                    {
                        ObjOutcome.SDGList = JsonConvert.SerializeObject(SDGs);
                    }
                    // Fetching SDG_Targets
                    strQuery = @"select SDGTargets.Target_Name,SDGTargets.pk_SDGTargetID,fk_SDGID,SDG_Targets.Status from TbOutcome_SDG_Targets SDG_Targets inner join TbSDGTargets SDGTargets on SDG_Targets.SDGTargetID = SDGTargets.pk_SDGTargetID 
                      where   SDG_Targets.IsDeleted=0 and SDG_Targets.OutcomeID = @OutcomeId Order By SDG_Targets.ID Desc";
                    var SDG_Targets = _connection.Query<SDGTargets>(strQuery, new { OutcomeId = OutcomeId }).ToList();

                    if (SDG_Targets.Count > 0)
                    {
                        ObjOutcome.SDGTargetsList = JsonConvert.SerializeObject(SDG_Targets);
                    }


                    //-- Reporting Frequency
                    DateTime? RptDate = null;

                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbOutcomeTrackDate WHERE OutcomeId = @OutcomeId order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { OutcomeId = OutcomeId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }
                    if (ObjOutcome.Periodicity != 1)
                    {
                        if (RptDate != null)
                        {
                            switch (ObjOutcome.Periodicity)
                            {
                                case (int)CommonActivity.ReportingFrequency.Once:
                                    ObjOutcome.ReportingOnceDate = RptDate.Value.ToString("dd-MM-yyyy");
                                    ObjOutcome.ReportingFrequencyName = "Once";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Weekly:
                                    ObjOutcome.ReportingWeek = (int)RptDate.Value.DayOfWeek;
                                    ObjOutcome.ReportingFrequencyName = "Weekly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Monthly:
                                    ObjOutcome.ReportingMonthDay = RptDate.Value.Day;
                                    ObjOutcome.ReportingFrequencyName = "Monthly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Quarterly:
                                    ObjOutcome.ReportingMonthDateDay = RptDate.Value.Day;
                                    ObjOutcome.ReportingMonth = RptDate.Value.Month;
                                    ObjOutcome.ReportingFrequencyName = "Quarterly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                    ObjOutcome.ReportingMonthDateDay = RptDate.Value.Day;
                                    ObjOutcome.ReportingMonth = RptDate.Value.Month;
                                    ObjOutcome.ReportingFrequencyName = "Half_Yearly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Yearly:
                                    ObjOutcome.ReportingFrequencyName = "Yearly";
                                    ObjOutcome.ReportingMonthDateDay = RptDate.Value.Day;
                                    ObjOutcome.ReportingMonth = RptDate.Value.Month;
                                    break;
                            }
                        }
                    }
                    else
                    { ObjOutcome.ReportingFrequencyName = "Never"; }
                }
            }
            catch (Exception ex)
            {
            }
            return ObjOutcome;
        }

        /// <summary>
        /// Function used to update outcome workflow based on OutcomeId
        /// </summary>
        /// <param name="WorkFlowID"></param>
        /// <param name="ProjectId"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="OutcomeId"></param>
        public void UpdateOutcomeWorkflow(int WorkFlowID, int ProjectId, int loggedUserId, int OutcomeId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbOutcomes set 
                        WorkflowId = @WorkFlowID ,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where pk_OutcomeID = @OutcomeId; ";
                        _connection.Execute(query, new
                        {
                            OutcomeId = OutcomeId,
                            WorkflowID = WorkFlowID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });

                        if (WorkFlowID > 0)
                        {
                            // Updating Workflow ID to Taskdb
                            string Query = @"Select WfNoOfLevels  From  TbProjectWorkflow  Where IsDeleted = 0 And WorkflowID = @WorkflowID";
                            var WfNoOfLevels = _connection.Query<int>(Query, new { WorkflowID = WorkFlowID }).FirstOrDefault();

                            ////Update To SyncDB
                            string strQuery = @"Update TaskSyncDB SET  WorkflowID =@WorkflowID,TotalWFLevel =@TotalWFLevel WHERE TypeID = @TypeID  And Type =@Type ";
                            var syncDBId = _connection.Execute(strQuery, new
                            {
                                Type = "Outcome",
                                TypeID = OutcomeId,
                                WorkflowID = WorkFlowID,
                                TotalWFLevel = WfNoOfLevels

                            });
                        }
                        if (OutcomeId != 0)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectId;
                            objLog.AuditFrom = "Outcome";
                            objLog.AuditEvent = "EDIT";
                            objLog.AuditFromID = OutcomeId;
                            objLog.FromIDColumnName = "pk_OutcomeID";
                            objLog.Message = "Updated Outcome's workflow";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
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

        /// <summary>
        /// Function used to delete already exist data and add new outcome notify users
        /// </summary>
        /// <param name="ObjOutcome"></param>
        /// <param name="loggeduserid"></param>
        /// <param name="OutcomeId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool fnAssignUserstoOutcome(ProjectOutcome ObjOutcome, int loggeduserid, int OutcomeId, string Type)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    if (ObjOutcome.UserList != null && ObjOutcome.UserList != "[]")
                    {
                        DeleteOutcomeAssignedUsers(OutcomeId);

                        List<UsersDetails> lstUser = JsonConvert.DeserializeObject<List<UsersDetails>>(ObjOutcome.UserList);

                        foreach (UsersDetails userItem in lstUser)
                        {
                            //string[] UserData = ObjOutcome.UserList.Split(',');
                            //if (UserData.Length>0)
                            //{
                            //for (var Count = 0; Count <= UserData.Length - 1; Count++)
                            // {
                            InsertOutcomeNotifyUsers(ObjOutcome.ProjectID, OutcomeId, userItem.ID, loggeduserid, Type);
                        }
                        //}

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
        /// Function used to delete outcome assign users based on OutcomeId
        /// </summary>
        /// <param name="OutcomeId"></param>
        public void DeleteOutcomeAssignedUsers(int OutcomeId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbOutcomeAssignUsers set IsDeleted = @IsDeleted, UpdatedDate = now() where OutcomeId =@OutcomeId;";
                    _connection.Execute(query, new { OutcomeId = OutcomeId, IsDeleted = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Add new outcome notify users
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="OutcomeId"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public int InsertOutcomeNotifyUsers(int ProjectId, int OutcomeId, int UserID, int loggedUserId, string Type)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    if (UserID > 0)
                    {
                        using (_connection = Utils.Database.GetDBConnection())
                        {
                            query = @"INSERT INTO TbOutcomeAssignUsers(OutcomeId, ProjectId, UserID, IsDeleted, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
                                  VALUES(@OutcomeID, @ProjectId, @UserID, @IsDeleted, now(), now(), @CreatedBy, @UpdatedBy); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(query, new
                            {
                                ProjectId = ProjectId,
                                OutcomeID = OutcomeId,
                                UserID = UserID,
                                IsDeleted = 0,
                                CreatedBy = loggedUserId,
                                UpdatedBy = loggedUserId

                            }).Single();
                            if (Type == "Insert")
                            {
                                query = "Select Name  From TbUser Where ID =@UserID ";
                                var AssgName = _connection.Query<string>(query, new { UserID = UserID }).Single();
                                query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                                    VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                                id = _connection.Query<int>(query, new
                                {
                                    ProjectID = ProjectId,
                                    TaskType = "Outcome",
                                    TaskTypeID = OutcomeId,
                                    TypeIDColumn = "pk_OutcomeId",
                                    AssignedTo = UserID,
                                    AssignedBy = loggedUserId,
                                    Message = "Assigned the task to " + AssgName,
                                    createdby = loggedUserId
                                }).Single();

                                // Send Notification 
                                try
                                {
                                    // Send Notification 
                                    var title = "";
                                    var sub = "";
                                    query = "Select OutcomeName  From TbKPI Where  pk_OutcomeID =@OutcomeId";
                                    var OutcomeName = _connection.Query<string>(query, new { OutcomeId = OutcomeId }).Single();

                                    title = AssgName + "OUTCOME ASSINGED";
                                    sub = AssgName + "Added an new Outcome :" + OutcomeName + "' on " + DateTime.Now;
                                    Notification objNotification = new Notification();
                                    objNotification.UserId = UserID;
                                    objNotification.Title = title;
                                    objNotification.Subject = sub;
                                    new NotificationDAL().SendUserNotification(objNotification);
                                }

                                catch
                                {

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
            return id;
        }

        /// <summary>
        /// Function used to delete outcome based on OutcomeId,loggedUserId
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteOutcomes(int OutcomeId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();
                        query = @"Update TbOutcomes set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId  where pk_OutcomeID =@OutcomeId;";
                        _connection.Execute(query, new { OutcomeId = OutcomeId, IsDeleted = 1, loggedUserId = loggedUserId });

                        query = "Select OutcomeName,ProjectId as ProjectID  From TbOutcomes Where pk_OutcomeID =@OutcomeId";
                        ObjOutcome = _connection.Query<ProjectOutcome>(query, new { OutcomeId = OutcomeId }).ToList();


                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ObjOutcome[0].ProjectID;
                        objLog.AuditFrom = "Outcome";
                        objLog.AuditEvent = "REMOVE";
                        objLog.AuditFromID = OutcomeId;
                        objLog.FromIDColumnName = "pk_OutcomeID";
                        objLog.Message = "Removed Outcome named '" + ObjOutcome[0].OutcomeName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        AddAuditLog(objLog);
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }


        #region New Outcome Section

        #region AddOutCome
        /// <summary>
        /// AddOutCome
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="activityAttchments"></param>
        /// <returns></returns>
        public int AddOutCome(ProjectOutcome inputObj, int loggedUserId, ActivityAttchments activityAttchments)
        {
            string strQuery = string.Empty;
            var actid = 0;
            bool retStatus = false;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"INSERT INTO TbOutcomes(ProjectId, OutcomeName, StartDate, EndDate, DatacollectionType, SurveyId,  
                                                            SurveyTarget, WorkflowId, Periodicity, IsDeleted, CreatedDate, CreatedBy, Description,IsPaymentLinked,PaymentLinkedBudget,SurveyLogic,UnitOfMeasure,AdditionalQuestionType,AdditionalSectionId,AdditionalQuestionId,AutoGeneratedActivityId,SDG_ID)
                                            VALUES(@projectid,@OutcomeName,@startdate,@enddate,@DatacollectionType,@SurveyId,@SurveyTarget,
                                                   @WorkflowId,@Periodicity,@isdeleted,@createdate,@createby,@Description,@IsPaymentLinked,@PaymentLinkedBudget,@SurveyLogic,@UnitOfMeasure,@AdditionalQuestionType,@AdditionalSectionId,@AdditionalQuestionId,@AutoGeneratedActivityId,@SDGID); 
                                             SELECT LAST_INSERT_ID();";
                        actid = _connection.Query<int>(strQuery, new
                        {
                            projectid = inputObj.ProjectID,
                            OutcomeName = inputObj.OutcomeName,
                            startdate = inputObj.StartDate,
                            enddate = inputObj.EndDate,
                            DatacollectionType = inputObj.DatacollectionType,
                            SurveyId = (inputObj.DatacollectionType == "Quantitative" ? 0 : inputObj.SurveyId),
                            SurveyTarget = inputObj.SurveyTarget,
                            WorkflowId = inputObj.WorkflowId,
                            Periodicity = inputObj.Periodicity,
                            isdeleted = 0,
                            createdate = DateTime.Now,
                            createby = loggedUserId,
                            Description = inputObj.Description,
                            IsPaymentLinked = inputObj.IsPaymentLinked,
                            PaymentLinkedBudget = inputObj.PaymentLinkedBudget,
                            SurveyLogic = (inputObj.SurveyLogic == 0 ? 1 : inputObj.SurveyLogic),
                            UnitOfMeasure = inputObj.UnitOfMeasure,
                            AdditionalSectionId = inputObj.AdditionalSectionId,
                            AdditionalQuestionId = inputObj.AdditionalQuestionId,
                            AdditionalQuestionType = (inputObj.AdditionalQuestionId > 0 ? 1 : 0),
                            AutoGeneratedActivityId = inputObj.AutoGeneratedActivityId,
                            SDGID = inputObj.SDGID
                        }).Single();

                        if (actid > 0)
                        {
                            if (inputObj.OutcomeQuantitativeLogic != null && inputObj.OutcomeQuantitativeLogic.Count > 0)
                            {
                                string QueryOutCollection = @"INSERT INTO TbOutcomeSurveyCollection(OutcomeId,SurveyId,SectionId,QuestionId,BeneficiaryQuestionID,CreatedDate,IsActive) VALUES(@OutcomeId,@SurveyId,@SectionId,@QuestionId,@BeneficiaryQuestionID,now(),1); SELECT LAST_INSERT_ID();";

                                foreach (var itm in inputObj.OutcomeQuantitativeLogic)
                                {
                                    int collId = _connection.Query<int>(QueryOutCollection, new
                                    {
                                        OutcomeId = actid,
                                        SurveyId = itm.SurveyId,
                                        SectionId = itm.SectionId,
                                        QuestionId = itm.QuestionId,
                                        BeneficiaryQuestionID = itm.BeneficiaryTypeQstnID
                                    }).Single();

                                    if (collId > 0 && itm.lstSurveyPrimeLogic != null && itm.lstSurveyPrimeLogic.Count > 0)
                                    {
                                        string QueryOutSurveyLogic = @"INSERT INTO TbOutcomeSurveyLogic(SurveyCollectionID,SurveyLogic,SurveyAnswer,IsActive,CreatedDate,LogicCondition) VALUES(@SurveyCollectionID,@SurveyLogic,@SurveyAnswer,1,now(),@LogicCondition);";

                                        foreach (var itm1 in itm.lstSurveyPrimeLogic)
                                        {
                                            _connection.Execute(QueryOutSurveyLogic, new
                                            {
                                                SurveyCollectionID = collId,
                                                SurveyLogic = itm1.SurveyLogicPrime,
                                                SurveyAnswer = itm1.SurveyLogicPrimeAnswer,
                                                LogicCondition = itm1.SurveyLogicCondition
                                            });
                                        }
                                    }
                                }
                            }

                            // Activity Month wise data insertion - done on 29-01-2023
                            if (inputObj.lstProjectOutcomeMonthwiseData != null && inputObj.lstProjectOutcomeMonthwiseData.Count > 0)
                            {
                                string queryMonth = "insert into TbOutcomeMonthwiseData(OutcomeId,Target,Budget,ActMonth,ActYear,CreatedDate,IsActive) ";

                                int mntcount = 0;

                                foreach (var monthItem in inputObj.lstProjectOutcomeMonthwiseData)
                                {
                                    queryMonth += " select " + actid + "," + monthItem.Target + "," + monthItem.Budget + "," + monthItem.ActMonth + "," + monthItem.ActYear + ",NOW(),1";

                                    mntcount++;

                                    if (mntcount < inputObj.lstProjectOutcomeMonthwiseData.Count)
                                    {
                                        queryMonth += " UNION ALL ";
                                    }
                                }

                                _connection.Execute(queryMonth);
                            }


                            string Qeury = @"Select A.WorkflowId,AW.WfNoOfLevels  From TbOutcomes A 
                                 Left Join TbProjectWorkflow  AW On AW.WorkflowID = A.WorkflowId
                                 Where A.IsDeleted = 0 And AW.IsDeleted = 0 And A.pk_OutcomeID = @actid";
                            dynamic objWF = _connection.Query<object>(Qeury, new { actid = actid }).FirstOrDefault();

                            Qeury = @"Select WSID as WorkspaceID From TbProject  Where ProjectId = @ProjectId";
                            var WID = _connection.Query<int>(Qeury, new { ProjectId = inputObj.ProjectID }).FirstOrDefault();
                            //AddTo SyncDB
                            strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                            var syncDBId = _connection.Query<int>(strQuery, new
                            {
                                Type = "Outcome",
                                Command = "ADD",
                                TypeIDColumn = "pk_OutcomeID",
                                TypeID = actid,
                                WorkflowID = objWF == null ? 0 : objWF.WorkflowId,
                                FinanceWorkflowID = objWF == null ? 0 : objWF.FinanceWorkflowId,
                                WfNoOfLevels = objWF == null ? 0 : objWF.WfNoOfLevels,
                                Status = "Pending",
                                WorkspaceID = WID,
                                CreatedBy = loggedUserId
                            }).Single();

                            // Send Notification 

                            //AddToActivityLog(inputObj.ProjectId, actid, "Created a new activity named " + inputObj.ActivityName, loggedUserId);
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = inputObj.ProjectID;
                            objLog.AuditFrom = "Outcome";
                            objLog.AuditEvent = "ADD";
                            objLog.AuditFromID = actid;
                            objLog.FromIDColumnName = "pk_OutcomeID";
                            objLog.Message = "Created a new Outcome named " + inputObj.OutcomeName;
                            objLog.CreatedBy = loggedUserId;
                            ProjectDAL.AddAuditLog(objLog);

                            retStatus = true;
                            inputObj.ActivityId = actid;

                            //bool addUser = AddUsersToOutcome(actid, activityAttchments.ActivityUsersList, loggedUserId, inputObj.OutcomeName, inputObj.ProjectID);

                            //if (activityAttchments.ActivityTagsList != "null")
                            //{
                            //    bool addTags = AddTagsToOutcome(actid, activityAttchments.ActivityTagsList, loggedUserId);
                            //}
                            //if (activityAttchments.ExpenseTagsList != "null")
                            //{
                            //    bool addExpTags = AddExpenseTagsToOutcome(actid, activityAttchments.ExpenseTagsList, loggedUserId);
                            //}
                            //if (activityAttchments.ExpenseTagsNotifyUsersList != "null")
                            //{
                            //    bool notifyExpenseUser = NotifyExpenseTagsUsers(activityAttchments.ExpenseTagsNotifyUsersList, inputObj.ProjectID);
                            //}
                            //bool addPriodicity = AddPeriodicityToOutcome(loggedUserId, inputObj);

                            // if payment linked activity add a budget head and Item , 
                            if (inputObj.IsPaymentLinked == true)
                            {
                                string BudgetHead = string.Empty;
                                string Query = string.Empty;
                                BudgetHead = "Output";
                                Query = @"SELECT ID FROM TbBudgetPlan where BudgetHead = @BudgetHead and ProjectId = @ProjectId and IsDeleted = 0";
                                var BudgetID = _connection.Query<int>(Query, new { BudgetHead = BudgetHead, ProjectId = inputObj.ProjectID }).FirstOrDefault();

                                if (BudgetID > 0)
                                {
                                    Query = @"INSERT INTO TbBudgetPlanItemDetails(ProjectId, BudgetHeadID, ItemName, Amount, CreatedDate, CreatedBy, IsDeleted) VALUES(@ProjectId,@BudgetHeadID,@ItemName,@Amount,now(),@CreatedBy,0);SELECT LAST_INSERT_ID();";
                                    var Id = _connection.Query<int>(Query, new
                                    {
                                        ProjectId = inputObj.ProjectID,
                                        BudgetHeadID = BudgetID,
                                        ItemName = inputObj.OutcomeName,
                                        Amount = inputObj.PaymentLinkedBudget,
                                        CreatedBy = loggedUserId
                                    }).Single();
                                }
                                else if (BudgetID == 0)
                                {
                                    Query = @"INSERT INTO TbBudgetPlan(ProjectId, BudgetHead, HeadAmount, CreatedDate, CreatedBy, IsDeleted) VALUES(@ProjectId,@BudgetHead,@HeadAmount,now(),@CreatedBy,0);SELECT LAST_INSERT_ID();";
                                    var HeadID = _connection.Query<int>(Query, new
                                    {
                                        ProjectId = inputObj.ProjectID,
                                        BudgetHead = BudgetHead,
                                        HeadAmount = inputObj.PaymentLinkedBudget,
                                        CreatedBy = loggedUserId
                                    }).Single();

                                    Query = @"INSERT INTO TbBudgetPlanItemDetails(ProjectId, BudgetHeadID, ItemName, Amount, CreatedDate, CreatedBy, IsDeleted) VALUES(@ProjectId,@BudgetHeadID,@ItemName,@Amount,now(),@CreatedBy,0);SELECT LAST_INSERT_ID();";
                                    var id = _connection.Query<int>(Query, new
                                    {
                                        ProjectId = inputObj.ProjectID,
                                        BudgetHeadID = HeadID,
                                        ItemName = inputObj.OutcomeName,
                                        Amount = inputObj.PaymentLinkedBudget,
                                        CreatedBy = loggedUserId
                                    }).Single();
                                }

                            }
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return actid;
        }
        #endregion AddOutCome

        #region AddUsersToOutcome
        /// <summary>
        /// AddUsersToOutcome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="userlist"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="outcomeName"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public bool AddUsersToOutcome(int OutcomeId, string userlist, int loggedUserId, string outcomeName, int ProjectId = 0)
        {
            string query = string.Empty;
            var id = 0;
            bool success = DeleteUsersFromOutCome(OutcomeId, loggedUserId);

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    // ProjectActivity actDetails = GetActivityDetailsByID(ActivityId);
                    AddToOutcomeLog(ProjectId, OutcomeId, "Assigned Users to the outcome named " + outcomeName, loggedUserId);

                    List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(userlist);
                    foreach (UsersInfo userItem in objUserList)
                    {
                        query = @"INSERT INTO TbOutcomeAssignUsers(OutcomeId,ProjectId,UserID,IsDeleted,CreatedDate,CreatedBy)
                            VALUES(@OutcomeId,@ProjectId,@UserID,@IsDeleted,@CreatedDate,@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            OutcomeId = OutcomeId,
                            ProjectId = ProjectId,
                            UserID = userItem.ID,
                            IsDeleted = 0,
                            CreatedDate = DateTime.Now,
                            CreatedBy = loggedUserId
                        }).Single();

                        query = "Select Name  From TbUser Where ID =@UserID ";
                        var name = _connection.Query<string>(query, new { UserID = userItem.ID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Outcome";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = OutcomeId;
                        objLog.FromIDColumnName = "ActivityId";
                        objLog.Message = "Outcome assigned to user  " + name;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);


                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy);";
                        _connection.Execute(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "Outcome",
                            TaskTypeID = OutcomeId,
                            TypeIDColumn = "pk_OutcomeID",
                            AssignedTo = userItem.ID,
                            AssignedBy = loggedUserId,
                            Message = "Assigned the task to " + name,
                            createdby = loggedUserId
                        });

                        // Send Notification 
                        try
                        {
                            // Send Notification 
                            var title = "";
                            var sub = "";

                            title = outcomeName + "OUTCOME ASSINGED";
                            sub = outcomeName + "Added an new outcome :" + outcomeName + "' on " + DateTime.Now;
                            Notification objNotification = new Notification();
                            objNotification.UserId = userItem.ID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }
                        catch
                        {

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
        #endregion AddUsersToActivity

        #region DeleteUsersFromOutCome
        /// <summary>
        /// DeleteUsersFromOutCome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteUsersFromOutCome(int OutcomeId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbOutcomeAssignUsers SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid ";
                    strQuery += " Where OutcomeId=@OutcomeId ";
                    _connection.Execute(strQuery, new { OutcomeId = OutcomeId, IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteUsersFromOutCome

        #region GetActivityDetailsByID
        /// <summary>
        /// Function used to fetch activity details based on ActivityId
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public ProjectActivity GetActivityDetailsByID(int ActivityId)
        {
            ProjectActivity objProjectActivityDetails = new ProjectActivity();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select ActivityId,ProjectId,ParentActivityId,ActivityName,Description,StartDate,EndDate,WeightagePercentage,ActivityType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget,IsPaymentLinked,PaymentLinkedBudget,WorkflowId,FinanceWorkflowId
                            ,Periodicity,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,Progress from TbProjectActivity where IsDeleted=@isdeleted and ActivityId=@actid Order By ActivityId Desc";
                    objProjectActivityDetails = _connection.Query<ProjectActivity>(query, new { isdeleted = 0, actid = ActivityId }).FirstOrDefault();

                    //Get Users List
                    objProjectActivityDetails.ActivityUsers = new List<User>();
                    //query = @"select * from TbUser where ID in 
                    //        ( select distinct UserId from TbProjectActivityToUsers where IsDeleted <>1 and ActivityId=@actid )  Order By ID Desc";

                    query = @"select distinct t.ID,t.Name,t.EmailId,t.Phone,t.Password,t.IsActive,t.NoOfLogins,t.FailedLoginCount,t.UserType,IFNULL(t.ProfileImage, '') as ProfileImage,t.Organization from TbUser t inner join TbProjectActivityToUsers u where
                                u.UserId = t.ID and u.IsDeleted <>1 and u.ActivityId=@actid  Order By t.ID Desc";

                    objProjectActivityDetails.ActivityUsers = _connection.Query<User>(query, new { actid = objProjectActivityDetails.ActivityId }).ToList();

                    //Get Expense tag List
                    objProjectActivityDetails.ExpenseTags = new List<Tags>();
                    //query = @"select * from TbProjectExpenseTags where TagID in 
                    //        ( select distinct ExpenseTagID from TbProjectActivityToExpenseTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";

                    query = @"select distinct t.TagID,t.TagName from TbProjectExpenseTags t inner join TbProjectActivityToExpenseTags e where
                            e.ExpenseTagID = t.TagID and e.IsDeleted <>1 and e.ActivityId=@actid Order By t.TagID Desc";

                    objProjectActivityDetails.ExpenseTags = _connection.Query<Tags>(query, new { actid = objProjectActivityDetails.ActivityId }).ToList();

                    //Get Activity tag List
                    objProjectActivityDetails.ActivityTags = new List<Tags>();
                    //query = @"select * from TbProjectTags where TagID in 
                    //        ( select distinct ActivityTagID from TbProjectActivityToTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";

                    query = @"select distinct t.TagID,t.TagName,t.TagColor from TbProjectTags t inner join TbProjectActivityToTags a where
                            a.ActivityTagID = t.TagID and a.IsDeleted <>1 and a.ActivityId=@actid  Order By t.TagID Desc";

                    objProjectActivityDetails.ActivityTags = _connection.Query<Tags>(query, new { actid = objProjectActivityDetails.ActivityId }).ToList();

                    //Get Periodicity parameters
                    DateTime? RptDate = null;
                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbProjectActivityTrackDate WHERE ActivityId = @actid order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { actid = objProjectActivityDetails.ActivityId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }

                    if (RptDate != null)
                    {
                        switch (objProjectActivityDetails.Periodicity)
                        {
                            case (int)CommonActivity.ReportingFrequency.Once:
                                objProjectActivityDetails.PeriodicityOnceDate = RptDate.Value;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Weekly:
                                objProjectActivityDetails.PeriodicityWeek = (int)RptDate.Value.DayOfWeek;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Monthly:
                                objProjectActivityDetails.PeriodicityMonthDay = RptDate.Value.Day;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Quarterly:
                                objProjectActivityDetails.PeriodicityMonthDateDay = RptDate.Value.Day;
                                objProjectActivityDetails.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                objProjectActivityDetails.PeriodicityMonthDateDay = RptDate.Value.Day;
                                objProjectActivityDetails.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Yearly:
                                objProjectActivityDetails.PeriodicityMonthDateDay = RptDate.Value.Day;
                                objProjectActivityDetails.PeriodicityMonth = RptDate.Value.Month;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objProjectActivityDetails;
        }
        #endregion GetActivityDetailsByID

        #region AddToOutcomeLog
        /// <summary>
        /// AddToOutcomeLog
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="OutcomeId"></param>
        /// <param name="message"></param>
        /// <param name="loggedUserId"></param>
        public void AddToOutcomeLog(int ProjectId, int OutcomeId, string message, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"insert into TbOutComeLog (ProjectId, OutcomeId, Message, CreatedDate, CreatedBy)
                                    values(@projid, @OutcomeId, @msg, @createdate, @createby);";
                    _connection.Execute(query, new
                    {
                        projid = ProjectId,
                        OutcomeId = OutcomeId,
                        msg = message,
                        createdate = DateTime.Now,
                        createby = loggedUserId,
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }
        #endregion AddToOutcomeLog

        #region AddTagsToOutcome
        /// <summary>
        /// AddTagsToOutcome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="taglist"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool AddTagsToOutcome(int OutcomeId, string taglist, int loggedUserId)
        {
            bool success = DeleteTagsFromOutcome(OutcomeId, loggedUserId);

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    List<Tags> objTagList = JsonConvert.DeserializeObject<List<Tags>>(taglist);
                    foreach (Tags tagItem in objTagList)
                    {
                        string strQuery = @"INSERT INTO TbOutcomeToTags(OutcomeId,TagID,CreatedDate,CreatedBy,IsDeleted)
                                            VALUES(@OutcomeId,@TagID,now(),@loggedUserId,0);";
                        _connection.Execute(strQuery, new { OutcomeId = OutcomeId, TagID = tagItem.TagID, loggedUserId = loggedUserId });
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion AddTagsToOutcome

        #region DeleteTagsFromOutcome
        /// <summary>
        /// DeleteTagsFromOutcome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteTagsFromOutcome(int OutcomeId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbOutcomeToTags SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid ";
                    strQuery += " Where OutcomeId=@OutcomeId ";
                    _connection.Execute(strQuery, new { OutcomeId = OutcomeId, IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteTagsFromOutcome

        #region AddExpenseTagsToOutcome
        /// <summary>
        /// AddExpenseTagsToOutcome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="exptaglist"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool AddExpenseTagsToOutcome(int OutcomeId, string exptaglist, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            bool success = DeleteExpenseTagsFromOutcome(OutcomeId, loggedUserId);

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    List<Tags> objExpTagList = JsonConvert.DeserializeObject<List<Tags>>(exptaglist);
                    foreach (Tags expTagItem in objExpTagList)
                    {
                        strQuery = @"INSERT INTO TbOutcomeToExpenseTags(OutcomeId,ExpenseTagID,CreatedDate,CreatedBy,IsDeleted)
                                            VALUES(@OutcomeId,@tagid,now(),@userID,0);";
                        _connection.Execute(strQuery, new { OutcomeId = OutcomeId, tagid = expTagItem.TagID, userID = loggedUserId });
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion AddExpenseTagsToOutcome

        #region DeleteExpenseTagsFromOutcome
        /// <summary>
        /// DeleteExpenseTagsFromOutcome
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteExpenseTagsFromOutcome(int OutcomeId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbOutcomeToExpenseTags SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid ";
                    strQuery += " Where OutcomeId=@OutcomeId ";
                    _connection.Execute(strQuery, new { OutcomeId = OutcomeId, IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeleteExpenseTagsFromOutcome

        #region NotifyExpenseTagsUsers
        /// <summary>
        /// Function used to notification send to users through mail
        /// </summary>
        /// <param name="notifyuserlist"></param>
        /// <param name="projid"></param>
        /// <returns></returns>
        public bool NotifyExpenseTagsUsers(string notifyuserlist, int projid)
        {
            string strQuery = string.Empty;

            try
            {
                List<User> objNotifyUserList = JsonConvert.DeserializeObject<List<User>>(notifyuserlist);
                UserDAL objUserDAL = new UserDAL();
                ProjectDAL objProjectDAL = new ProjectDAL();
                string projname = objProjectDAL.GetProjectName(projid);
                foreach (User userItem in objNotifyUserList)
                {
                    LoggedUserInfo userItemDetails = objUserDAL.GetLogUser(userItem.ID);
                    bool bStatus = false;
                    EmailActivity objEmail = new EmailActivity();
                    NotifyExpenseTags obj = new NotifyExpenseTags();
                    obj.UserName = userItemDetails.Name;
                    obj.ProjectName = projname;
                    obj.UserEmailId = userItemDetails.EmailId;
                    bStatus = objEmail.SendMail("Notify Expense Tags", obj);
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion NotifyExpenseTagsUsers

        #region AddPeriodicityToOutcome
        /// <summary>
        /// AddPeriodicityToOutcome
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public bool AddPeriodicityToOutcome(int loggedUserId, ProjectOutcome inputObj)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {

                    bool isDeleted = DeletePeriodicityForOutcome(inputObj.ActivityId.ToString());

                    switch (inputObj.Periodicity)
                    {
                        case (int)CommonActivity.ReportingFrequency.Once:
                            //DateTime? reportvalue = CommonActivity.GetDateTimeValue(inputObj.PeriodicityOnceDate);
                            lstTrackDates.Add(inputObj.PeriodicityOnceDate.ToString("yyyy-MM-dd"));
                            break;
                        case (int)CommonActivity.ReportingFrequency.Daily:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, 0, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Weekly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityWeek, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Monthly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonthDay, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Quarterly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonthDateDay, inputObj.PeriodicityMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonthDateDay, inputObj.PeriodicityMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(inputObj.StartDate, inputObj.EndDate, inputObj.Periodicity, inputObj.PeriodicityMonthDateDay, inputObj.PeriodicityMonth);
                            break;
                    }

                    if (lstTrackDates != null && lstTrackDates.Count > 0)
                    {
                        query = "INSERT INTO TbOutcomeTrackDate(TrackDate,OutcomeId,TrackStatus) ";

                        for (int i = 0; i < lstTrackDates.Count; i++)
                        {
                            query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + inputObj.ActivityId + ",0" : " SELECT '" + lstTrackDates[i] + "'," + inputObj.ActivityId + ",0";
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
        #endregion AddPeriodicityToOutcome

        #region DeletePeriodicityForOutcome
        /// <summary>
        /// DeletePeriodicityForOutcome
        /// </summary>
        /// <param name="outcomeIdsList"></param>
        /// <returns></returns>
        public bool DeletePeriodicityForOutcome(string outcomeIdsList)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "DELETE FROM TbOutcomeTrackDate WHERE FIND_IN_SET(OutcomeId,@outcomeIdsList) ";
                    _connection.Execute(strQuery, new { outcomeIdsList = outcomeIdsList });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        #endregion DeletePeriodicityForOutcome

        #region EditOutcome
        /// <summary>
        /// Function used to edit project activity based on ActivityId
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="activityAttchments"></param>
        /// <returns></returns>
        public int EditOutcome(ProjectOutcome inputObj, int loggedUserId, ActivityAttchments activityAttchments)
        {
            string strQuery = string.Empty;
            int actId = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"UPDATE TbOutcomes SET 
                                OutcomeName=@OutcomeName, Description=@Description, 
                                StartDate=@startdate, EndDate=@enddate,DatacollectionType=@DatacollectionType, 
                                SurveyTarget=@SurveyTarget, IsPaymentLinked=@IsPaymentLinked, PaymentLinkedBudget=@PaymentLinkedBudget, 
                                WorkflowId=@WorkflowId, Periodicity=@Periodicity, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate,SurveyLogic = @SurveyLogic,
                                UnitOfMeasure=@UnitOfMeasure,AdditionalSectionId=@AdditionalSectionId,AdditionalQuestionId=@AdditionalQuestionId,AdditionalQuestionType=@AdditionalQuestionType,SDG_ID=@SDGID  
                            WHERE pk_OutcomeID=@OutcomeID ";
                        _connection.Execute(strQuery, new
                        {
                            OutcomeName = inputObj.OutcomeName,
                            Description = inputObj.Description != "null" ? inputObj.Description : null,
                            StartDate = inputObj.StartDate,
                            EndDate = inputObj.EndDate,
                            DatacollectionType = inputObj.DatacollectionType,
                            SurveyTarget = inputObj.SurveyTarget,
                            IsPaymentLinked = inputObj.IsPaymentLinked,
                            PaymentLinkedBudget = inputObj.PaymentLinkedBudget,
                            WorkflowId = inputObj.WorkflowId,
                            Periodicity = inputObj.Periodicity,
                            UpdatedBy = loggedUserId,
                            UpdatedDate = DateTime.Now,
                            OutcomeID = inputObj.ActivityId,
                            SurveyLogic = (inputObj.SurveyLogic == 0 ? 1 : inputObj.SurveyLogic),
                            UnitOfMeasure = inputObj.UnitOfMeasure,
                            AdditionalSectionId = inputObj.AdditionalSectionId,
                            AdditionalQuestionId = inputObj.AdditionalQuestionId,
                            AdditionalQuestionType = (inputObj.AdditionalQuestionId > 0 ? 1 : 0),
                            SDGID = inputObj.SDGID
                        });

                        actId = inputObj.ActivityId;

                        if (inputObj.OutcomeQuantitativeLogic != null && inputObj.OutcomeQuantitativeLogic.Count > 0)
                        {
                            string strQueryUpdate = @"Update TbOutcomeSurveyCollection SET  IsActive =@IsActive WHERE OutcomeId = @OutcomeId;";
                            _connection.Execute(strQueryUpdate, new
                            {
                                OutcomeId = inputObj.ActivityId,
                                IsActive = 0
                            });


                            string strQueryUpdate1 = @"UPDATE TbOutcomeSurveyLogic T2 INNER JOIN TbOutcomeSurveyCollection T1 ON T1.ID = T2.SurveyCollectionID  SET T2.IsActive = 0 WHERE T1.OutcomeId =  @OutcomeId;";
                            _connection.Execute(strQueryUpdate1, new
                            {
                                OutcomeId = inputObj.ActivityId,
                                IsActive = 0
                            });


                            string QueryOutCollection = @"INSERT INTO TbOutcomeSurveyCollection(OutcomeId,SurveyId,SectionId,QuestionId,BeneficiaryQuestionID,CreatedDate,IsActive) VALUES(@OutcomeId,@SurveyId,@SectionId,@QuestionId,@BeneficiaryQuestionID,now(),1); SELECT LAST_INSERT_ID();";

                            foreach (var itm in inputObj.OutcomeQuantitativeLogic)
                            {
                                int collId = _connection.Query<int>(QueryOutCollection, new
                                {
                                    OutcomeId = inputObj.ActivityId,
                                    SurveyId = itm.SurveyId,
                                    SectionId = itm.SectionId,
                                    QuestionId = itm.QuestionId,
                                    BeneficiaryQuestionID = itm.BeneficiaryTypeQstnID
                                }).Single();

                                if (collId > 0 && itm.lstSurveyPrimeLogic != null && itm.lstSurveyPrimeLogic.Count > 0)
                                {
                                    string QueryOutSurveyLogic = @"INSERT INTO TbOutcomeSurveyLogic(SurveyCollectionID,SurveyLogic,SurveyAnswer,IsActive,CreatedDate,LogicCondition) VALUES(@SurveyCollectionID,@SurveyLogic,@SurveyAnswer,1,now(),@LogicCondition);";

                                    foreach (var itm1 in itm.lstSurveyPrimeLogic)
                                    {
                                        _connection.Execute(QueryOutSurveyLogic, new
                                        {
                                            SurveyCollectionID = collId,
                                            SurveyLogic = itm1.SurveyLogicPrime,
                                            SurveyAnswer = itm1.SurveyLogicPrimeAnswer,
                                            LogicCondition = itm1.SurveyLogicCondition
                                        });
                                    }
                                }
                            }
                        }


                        if (inputObj.lstProjectOutcomeMonthwiseData != null && inputObj.lstProjectOutcomeMonthwiseData.Count > 0)
                        {
                            string strQueryMonthUpdate = @"Update TbOutcomeMonthwiseData SET  IsActive =@IsActive WHERE OutcomeId = @OutcomeId;";
                            _connection.Execute(strQueryMonthUpdate, new
                            {
                                OutcomeId = inputObj.ActivityId,
                                IsActive = 0
                            });

                            string queryMonth = "insert into TbOutcomeMonthwiseData(OutcomeId,Target,Budget,ActMonth,ActYear,CreatedDate,IsActive) ";

                            int mntcount = 0;

                            foreach (var monthItem in inputObj.lstProjectOutcomeMonthwiseData)
                            {
                                queryMonth += " select " + inputObj.ActivityId + "," + monthItem.Target + "," + monthItem.Budget + "," + monthItem.ActMonth + "," + monthItem.ActYear + ",NOW(),1";

                                mntcount++;

                                if (mntcount < inputObj.lstProjectOutcomeMonthwiseData.Count)
                                {
                                    queryMonth += " UNION ALL ";
                                }
                            }

                            _connection.Execute(queryMonth);
                        }

                        string Qeury = @"Select WfNoOfLevels  From  TbProjectWorkflow  Where IsDeleted = 0 And WorkflowID = @WorkflowID";
                        var WfNoOfLevels = _connection.Query<int>(Qeury, new { WorkflowID = inputObj.WorkflowId }).FirstOrDefault();

                        ////Update To SyncDB
                        strQuery = @"Update TaskSyncDB SET  WorkflowID =@WorkflowID,TotalWFLevel =@TotalWFLevel WHERE TypeID = @TypeID  And Type =@Type ";
                        var syncDBId = _connection.Execute(strQuery, new
                        {
                            Type = "Outcome",
                            TypeID = inputObj.ActivityId,
                            WorkflowID = inputObj.WorkflowId,
                            TotalWFLevel = WfNoOfLevels
                        });


                        //AddToActivityLog(inputObj.ProjectId, inputObj.ActivityId, "Updated the activity named " + inputObj.ActivityName, loggedUserId);
                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = inputObj.ProjectID;
                        objLog.AuditFrom = "Outcome";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = inputObj.ActivityId;
                        objLog.FromIDColumnName = "pk_OutcomeID";
                        objLog.Message = "Updated the Outcome named " + inputObj.OutcomeName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                        //bool addUser = AddUsersToOutcome(inputObj.ActivityId, activityAttchments.ActivityUsersList, loggedUserId, inputObj.OutcomeName, inputObj.ProjectID);
                        //if (activityAttchments.ActivityTagsList != "null")
                        //{
                        //    bool addTags = AddTagsToOutcome(inputObj.ActivityId, activityAttchments.ActivityTagsList, loggedUserId);
                        //}
                        //if (activityAttchments.ExpenseTagsList != "null")
                        //{
                        //    bool addExpTags = AddExpenseTagsToOutcome(inputObj.ActivityId, activityAttchments.ExpenseTagsList, loggedUserId);
                        //}
                        //if (activityAttchments.ExpenseTagsNotifyUsersList != "null")
                        //{
                        //    bool notifyExpenseUser = NotifyExpenseTagsUsers(activityAttchments.ExpenseTagsNotifyUsersList, loggedUserId);
                        //}
                        //bool addPriodicity = AddPeriodicityToOutcome(loggedUserId, inputObj);
                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actId;
        }
        #endregion EditActivity

        #region GetLogicType
        /// <summary>
        /// Function used to fetch logic type details 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionTypes> GetLogicType()
        {
            List<QuestionTypes> lstItems = new List<QuestionTypes>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT ID, Type,IsPrimeLogic,IsNumeric,IsNonNumeric From TbLogicType where ifnull(IsActive,0) =1 Order By Type ";
                    lstItems = _connection.Query<QuestionTypes>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }
        #endregion GetLogicType

        #region GetSectionsBySurveyID
        /// <summary>
        /// Function used to get section details based on SurveyID
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectSurveySection> GetSectionsBySurveyID(int SurveyID)
        {
            List<ProjectSurveySection> lstItems = new List<ProjectSurveySection>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct SectionID, SectionName From TbProjectSurveySection Where SurveyID =@SurveyID  Order By SectionName ";
                    lstItems = _connection.Query<ProjectSurveySection>(strQuery, new { SurveyID = SurveyID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }
        #endregion GetSectionsBySurveyID

        #region GetQstnBySectionID
        /// <summary>
        /// Function used to fetch project survey question based on SectionID
        /// </summary>
        /// <param name="SectionID"></param>
        /// <returns></returns>
        public IEnumerable<SurveyQuestion> GetQstnBySectionID(int SectionID)
        {
            List<SurveyQuestion> lstItems = new List<SurveyQuestion>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Distinct Q.QuestionID, Q.QuestionName,QT.Type QuestionType ,QT.Category QuestionCategory From TbProjectSurveyQuestion Q 
                                Left Join TbQuestionType QT On QT.ID=Q.QuestionTypeID  Where Q.IsActive =1  and Q.SectionID =@SectionID and QT.Category in (1,2)  and Q.QuestionTypeID  != @QuestionTypeID  Order By Q.QuestionName"; ;
                    lstItems = _connection.Query<SurveyQuestion>(strQuery, new { SectionID = SectionID, QuestionTypeID = (int)CommonActivity.QuestionType.Note }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }
        #endregion GetQstnBySectionID

        #region GetProjectBeneficiaryType
        /// <summary>
        /// GetProjectBeneficiaryType
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns> List </returns>
        public List<ProjectSurveySection> GetSurveyBeneficiaryType(int surveyId = 0)
        {
            List<ProjectSurveySection> lstBeneficiaryType = new List<ProjectSurveySection>();
            string query = "";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"SELECT t.BeneficiaryTypeID as SectionID,CONCAT((t.Name), ' - ' ,'Beneficiary Regestration form') SectionName,'Beneficiary' as SectionORBeneficiary 
                                 From TbProjectBeneficiaryType t inner join TbProjectSurvey s on t.BeneficiaryTypeID = s.BeneficiaryTypeID
                                  and s.SurveyID= @surveyId Where  IFNULL(t.IsActive,0)= @IsActive AND t.IsDeleted=0
                                   union 
                    SELECT Distinct SectionID, SectionName,'Section' as SectionORBeneficiary From TbProjectSurveySection Where SurveyID = @SurveyID  Order By SectionName ";

                    lstBeneficiaryType = _connection.Query<ProjectSurveySection>(query, new { IsActive = 1, surveyId = surveyId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiaryType;
        }
        #endregion GetProjectBeneficiaryType

        #region GetSurveyLinkedBeneficairyQuestionDetails
        /// <summary>
        /// GetSurveyLinkedBeneficairyQuestionDetails
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public IEnumerable<BeneficiaryTypeQuestions> GetSurveyLinkedBeneficairyQuestionDetails(int SurveyID)
        {
            List<BeneficiaryTypeQuestions> lstBeneficiaryType = new List<BeneficiaryTypeQuestions>();
            string strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Select BTQ.BeneficiaryTypeID,QuestionID, QuestionName,AnsType.AnswerType as AnswerType
                             From TbBeneficiaryTypeQuestion BTQ  inner join TbProjectSurvey s on BTQ.BeneficiaryTypeID = s.BeneficiaryTypeID
                             inner join TbBeneficiaryAnswerType AnsType on  BTQ.AnswerTypeID =  AnsType.AnswerTypeID 
                             Where  BTQ.IsDeleted =0 and s.SurveyID= @SurveyID and AnsType.IsActive = 1 and AnsType.IsDeleted = 0 and BTQ.AnswerTypeID  != @QuestionTypeID and AnsType.Category in (1,2);";
                lstBeneficiaryType = _connection.Query<BeneficiaryTypeQuestions>(strQuery, new { SurveyID = SurveyID, QuestionTypeID = (int)CommonActivity.QuestionType.Note }).ToList();
            }
            return lstBeneficiaryType;
        }
        #endregion GetSurveyLinkedBeneficairyQuestionDetails

        #region GetBeneficiaryQuestionOption
        /// <summary>
        /// GetBeneficiaryQuestionOption
        /// </summary>
        /// <param name="QstnID"></param>
        /// <returns></returns>
        public IEnumerable<QuestionOptionList> GetBeneficiaryQuestionOption(int QstnID)
        {
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select QstnAnsOptionID as OptionID,OptionValue From TbBeneficiaryTypeQstnAnswerOptions Where QuestionID=@QuestionID ";
                    lst = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = QstnID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }
        #endregion GetBeneficiaryQuestionOption

        #region GetSurveyCascadingList
        /// <summary>
        /// GetSurveyCascadingList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MyTaskMasterList> GetSurveyCascadingList()
        {
            List<MyTaskMasterList> lstMasterList = new List<MyTaskMasterList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strMasterQuery = "SELECT CountryID AS ID,CountryName AS Name,'Country' ItemTable,0 AS ParentID,0 AS CensusCode  FROM  TbCountry";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT StateID AS ID,StateName AS Name,'State' ItemTable,CountryID AS ParentID, Census_Code AS CensusCode FROM  TbStates ";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT DistrictID AS ID,DistrictName AS Name,'District' ItemTable, StateID AS ParentID,Census_Code AS CensusCode FROM TbDistrict ";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT SubDistrictID AS ID,SubDistrictName AS Name,'SubDistrict' ItemTable,DistrictID  AS ParentID,Census_Code AS CensusCode FROM TbSubDistrict ";

                    lstMasterList = _connection.Query<MyTaskMasterList>(strMasterQuery).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstMasterList;
        }
        #endregion GetSurveyCascadingList

        #region GetBeneficiaryCascadingList
        /// <summary>
        ///GetBeneficiaryCascadingList
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public IEnumerable<MyTaskSurveyCascadingQuestionOptions> GetCascadingHeirarchyList(int QuestionID, bool isBeneficiary)
        {
            List<MyTaskSurveyCascadingQuestionOptions> lstBeneficiaryCascadingQstnOptions = new List<MyTaskSurveyCascadingQuestionOptions>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    if (!isBeneficiary)
                    {
                        string strTbBeneficiaryCascadingQstnOptions = "SELECT DISTINCT BenCascade.OptionGroupID,BenCascade.OptionValue,BenCascade.HierarchyOrder,BenCascade.TableName,BenCascade.ColumnName,";
                        strTbBeneficiaryCascadingQstnOptions += " BenCascade.PrimaryColumn,BenCascade.ParentItemColumn FROM TbBeneficiaryCascadingQstnOptions BenCascade ";
                        strTbBeneficiaryCascadingQstnOptions += " JOIN TbProjectSurveyQSTOption QSTOPT ON BenCascade.OptionValue = QSTOPT.OptionValue WHERE IFNULL(BenCascade.IsDeleted,0) = 0 AND QSTOPT.QuestionID = @QuestionID";
                        lstBeneficiaryCascadingQstnOptions = _connection.Query<MyTaskSurveyCascadingQuestionOptions>(strTbBeneficiaryCascadingQstnOptions, new { QuestionID = QuestionID }).ToList();
                    }
                    else
                    {
                        string strTbBeneficiaryCascadingQstnOptions = "SELECT DISTINCT BenCascade.OptionGroupID,BenCascade.OptionValue,BenCascade.HierarchyOrder,BenCascade.TableName,BenCascade.ColumnName,";
                        strTbBeneficiaryCascadingQstnOptions += " BenCascade.PrimaryColumn,BenCascade.ParentItemColumn FROM TbBeneficiaryCascadingQstnOptions BenCascade ";
                        strTbBeneficiaryCascadingQstnOptions += " JOIN TbBeneficiaryTypeQstnAnswerOptions QSTOPT ON BenCascade.OptionValue = QSTOPT.OptionValue WHERE IFNULL(BenCascade.IsDeleted,0) = 0 AND QSTOPT.QuestionID = @QuestionID";
                        lstBeneficiaryCascadingQstnOptions = _connection.Query<MyTaskSurveyCascadingQuestionOptions>(strTbBeneficiaryCascadingQstnOptions, new { QuestionID = QuestionID }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstBeneficiaryCascadingQstnOptions;
        }
        #endregion GetSurveyCascadingList

        #region GetQuantitativeData
        /// <summary>
        /// GetQuantitativeData
        /// </summary>
        /// <param name="outcomeID"></param>
        /// <returns></returns>
        public List<OutcomeQuantitativeLogic> GetQuantitativeData(int outcomeID)
        {
            List<OutcomeQuantitativeLogic> lstOutcomeQuantitativeLogic = new List<OutcomeQuantitativeLogic>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = "select tt.ID,tt.OutcomeId,tt.SurveyId,tt.SectionId,tt.QuestionID AS QuestionId,tt.BeneficiaryQuestionID as BeneficiaryTypeQstnID,SRV.SurveyName,SCT.SectionName,QST.QuestionName ";
                query += " FROM TbOutcomeSurveyCollection tt JOIN TbProjectSurvey SRV ON tt.SurveyId= SRV.SurveyID ";
                query += "JOIN TbProjectSurveySection SCT ON tt.SectionId = SCT.SectionID JOIN TbProjectSurveyQuestion QST ON tt.QuestionId = QST.QuestionID ";
                query += "WHERE  tt.OutcomeId = @OutcomeId and ifnull(tt.IsActive,0) = 1 AND tt.QuestionID >0";
                query += " UNION ";
                query += " select tt.ID,tt.OutcomeId,tt.SurveyId,tt.SectionId,tt.QuestionID AS QuestionId,tt.BeneficiaryQuestionID as BeneficiaryTypeQstnID,SRV.SurveyName,CONCAT(BTTYPE.Name,' - Beneficiary Registration Form')  AS SectionName, ";
                query += " BTQST.QuestionName ";
                query += " FROM TbOutcomeSurveyCollection tt JOIN TbProjectSurvey SRV ON tt.SurveyId= SRV.SurveyID ";
                query += " JOIN TbBeneficiaryTypeQuestion BTQST ON BTQST.QuestionID = tt.BeneficiaryQuestionID ";
                query += " JOIN TbProjectBeneficiaryType BTTYPE ON BTTYPE.BeneficiaryTypeID = BTQST.BeneficiaryTypeID";
                query += " WHERE  tt.OutcomeId = @OutcomeId and ifnull(tt.IsActive,0) = 1 AND tt.BeneficiaryQuestionID >0";

                lstOutcomeQuantitativeLogic = _connection.Query<OutcomeQuantitativeLogic>(query, new { OutcomeId = outcomeID }).ToList();

                string qrySurveyLogic = "SELECT SurveyCollectionID,SurveyLogic SurveyLogicPrime,SurveyAnswer SurveyLogicPrimeAnswer,LGType.Type SurveyLogicPrimeType,LG.LogicCondition SurveyLogicCondition FROM TbOutcomeSurveyLogic  LG JOIN ";
                qrySurveyLogic += "TbLogicType LGType ON LGType.ID = LG.SurveyLogic WHERE IFNULL(LG.IsActive,0) = 1 AND  LG.SurveyCollectionID = @SurveyCollectionID";

                if (lstOutcomeQuantitativeLogic != null && lstOutcomeQuantitativeLogic.Count > 0)
                {
                    foreach (var itm in lstOutcomeQuantitativeLogic)
                    {
                        itm.lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                        itm.lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(qrySurveyLogic, new { SurveyCollectionID = itm.ID }).ToList();
                    }
                }
            }

            return lstOutcomeQuantitativeLogic;
        }
        #endregion GetQuantitativeData

        #region getData
        /// <summary>
        /// getData
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="logicType"></param>
        /// <param name="value"></param>
        public double getData(int OutcomeID, int logicType, string SurveyTarget,int additionalSectionId = 0, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();
            List<OutcomeDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();

            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse = new List<OutcomeSurveyResponse>();
            string query = string.Empty;

            double result = 0;

            bool IsSurveyQuestion = true;

            if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                IsSurveyQuestion = objProjectSurveyDAL.IsSurveyQuestion(additionalSectionId, additionalSurveyQuestionId);
            }

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "SELECT ACT.ID,ACT.OutcomeId ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                    query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType,srvLogic.SurveyLogic,srvLogic.SurveyAnswer,ifnull(srvLogic.LogicCondition,0) as LogicCondition ";
                    query += " FROM TbProjectSurvey SRV  LEFT JOIN TbOutcomeSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                    query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                    query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                    query += "left join TbOutcomeSurveyLogic srvLogic ON srvLogic.SurveyCollectionID =  ACT.ID ";
                    query += "where ACT.OutcomeId = @OutcomeId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1 and ifnull(srvLogic.IsActive,0)=1; ";

                    objActivityDataCollectionDetails = _connection.Query<OutcomeDataCollectionDetails>(query, new { OutcomeId = OutcomeID }).ToList();

                    if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                    {
                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();

                        for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
                        {
                            string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                            querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID AND ifnull(LG.IsActive,0) = 1; ";
                            List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();

                            lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();

                            if (objActivityDataCollectionDetails[i].BeneficiaryTypeID <= 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                       // if (lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == itm2.SyncTaskID && x.SubmissionCount == itm2.SubmissionCount).Count() > 0)
                                                        if(itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }

                                                //if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                //{
                                                //    if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                //    {
                                                //        lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    if (lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() > 0)
                                                //    {
                                                //        lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                //    }
                                                //}
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].QuestionId > 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if(itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if(itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    } else
                                                    {
                                                        //if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() > 0)
                                                        //{
                                                        //    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        //}
                                                        if (itm2.ResponseID == itm1.ResponseID && itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }

                                                //    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                //{
                                                //    if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                //    {
                                                //        lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                   
                                                //}
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].BeneficiaryQuestionID > 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                                string query1 = "SELECT DISTINCT BeneficiaryID AS ResponseID,RSP.SurveyID FROM TbBeneficiaryRegistrationDetails reg  JOIN ";
                                query1 += " TbProjectSurveyResponses RSP ON reg.BeneficiaryID = RSP.ResponseID WHERE reg.QuestionID = @QuestionId AND ifnull(reg.IsDeleted,0) = 0 AND  RSP.SurveyID=@SurveyId ";
                                query1 += subqury;

                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyId = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                        //{
                                                        //    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        //}

                                                        if (itm2.ResponseID == itm1.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }

                                                    }
                                                }

                                                //if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                //{
                                                //    if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                //    {
                                                //        lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                //    {
                                                //        lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                //    }
                                                //}
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (logicType == (int)OutcomeLogicType.Average)
                        {
                            result = GetOutcomeQuantitativeAverage(lstProjectOutcomeInitialData, objActivityDataCollectionDetails, SurveyTarget, additionalSurveyQuestionId, additionalQuestionType, IsSurveyQuestion);
                        }
                        else if (logicType == (int)OutcomeLogicType.Cumulative_Sum)
                        {
                            result = GetOutcomeQuantitativeCumulativeSum(lstProjectOutcomeInitialData, objActivityDataCollectionDetails, SurveyTarget, additionalSurveyQuestionId, additionalQuestionType, IsSurveyQuestion);
                        }
                        else if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                        {
                            if (lstProjectOutcomeInitialData != null && lstProjectOutcomeInitialData.Count > 0)
                            {
                                result = lstProjectOutcomeInitialData.Count;
                            }
                        }
                    }

                    //if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                    //{
                    //    var mySKUs = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                    //    string joinedQuestionIds = string.Join(",", mySKUs);

                    //    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                    //    string joinedBeneficiaryTypeID = string.Join(",", mySKUs1);

                    //    var benficiaryIds = objActivityDataCollectionDetails.Where(x => x.BeneficiaryQuestionID > 0).Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                    //    string joinedBeneficiaryQuestionID = string.Join(",", benficiaryIds);

                    //    List<OutcomeSurveyResponse> lstActivitySurveyResponse = new List<OutcomeSurveyResponse>();
                    //    List<OutcomeSurveyResponse> lstActivitySurveyResponse1 = new List<OutcomeSurveyResponse>();
                    //    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse = new List<OutcomeSurveyResponse>();

                    //    if (joinedQuestionIds != null && joinedQuestionIds.Length > 0)
                    //    {
                    //        string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where QuestionID in(" + joinedQuestionIds + ") and ifnull(IsActive,0)=1;";
                    //        lstActivitySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry).ToList();

                    //        lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                    //    }

                    //    if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0)
                    //    {
                    //        string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    //        QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    //        QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                    //        QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID in(" + joinedBeneficiaryQuestionID + ") and ifnull(reg.IsDeleted,0)=0;";
                    //        lstBeneficiarySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTBenQry).ToList();

                    //        lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                    //    }

                    // if (logicType == (int)OutcomeLogicType.Average)
                    //    {
                    //        result = GetQuantitativeAverage(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse, additionalSurveyQuestionId, additionalQuestionType);
                    //    }
                    //    else if (logicType == (int)OutcomeLogicType.Cumulative_Sum)
                    //    {
                    //        result = GetQuantitativeCumulativeSum(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse, additionalSurveyQuestionId, additionalQuestionType);
                    //    }
                    //    else if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                    //    {
                    //        result = GetQuantitativeUniqueResponses(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse);
                    //    }
                    //    else
                    //    {
                    //        double totDays = (objActivityDataCollectionDetails[0].EndDate - objActivityDataCollectionDetails[0].StartDate).TotalDays;
                    //        double totDaysFnished = (DateTime.Now - objActivityDataCollectionDetails[0].StartDate).TotalDays;
                    //        result = (int)((totDaysFnished * 100) / totDays);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

             return result;
        }
        #endregion getData

        #region GetQuantitativeAverage
        /// <summary>
        /// GetQuantitativeAverage
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeAverage(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            double progressPercentage = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();
            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();
            List<OutcomeSurveyResponse> lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = null;
            lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();

                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();


                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse2 = new List<OutcomeSurveyResponse>();

                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                        }
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse4 = new List<OutcomeSurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        QSTQry = "select RES.ResponseID,reg.Answer,reg.QuestionID,RES.SurveyID,RES.SyncTaskID,ifnull(RES.SubmissionCount,1) SubmissionCount ";
                                        QSTQry += "from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID ";
                                        QSTQry += "JOIN TbProjectSurveyResponses RES ON RES.ResponseID = reg.BeneficiaryID WHERE ifnull(ben.IsActive,0)=1  and ";
                                        QSTQry += "ifnull(RES.IsActive,0)=1 and RES.ResponseID=@ResponseID  and RES.SyncTaskID in(" + joinedSyncTaskID + ")  and reg.QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0 && additionalSurveyQuestionId <= 0)
                            {
                                double sumValue = 0;
                                int cnt1 = 0;

                                for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                {
                                    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                }

                                if (sumValue > 0 && cnt1 > 0)
                                {
                                    double average = sumValue / cnt1;

                                    OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                    objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                    objBeneficiaryResponse.Anwer = average;
                                    objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                    lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                }
                            }
                            else if (additionalSurveyQuestionId > 0)
                            {
                                if (i == objActivityDataCollectionDetails.Count - 1 && cnt == lstActivitySurveyResponse1.Count - 1)
                                {
                                    if (lstActivitySurveyResponse5 != null && lstActivitySurveyResponse5.Count > 0)
                                    {
                                        var mySKUs1 = lstActivitySurveyResponse5.Select(l => l.ResponseID).Distinct().ToList();

                                        string joinedResponseID = string.Join(",", mySKUs1);

                                        if (!string.IsNullOrEmpty(joinedResponseID))
                                        {
                                            string[] splt = joinedResponseID.Split(',');

                                            if (splt != null && splt.Length > 0)
                                            {
                                                foreach (var itm10 in splt)
                                                {
                                                    double sumValue = 0;
                                                    int cnt1 = 0;

                                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse6 = new List<OutcomeSurveyResponse>();

                                                    lstActivitySurveyResponse6 = lstActivitySurveyResponse5.Where(x => x.ResponseID == Convert.ToInt32(itm10)).ToList();

                                                    if (lstActivitySurveyResponse6 != null && lstActivitySurveyResponse6.Count > 0)
                                                    {
                                                        for (cnt1 = 0; cnt1 < lstActivitySurveyResponse6.Count; cnt1++)
                                                        {
                                                            sumValue += (lstActivitySurveyResponse6[cnt1].Answer != null && lstActivitySurveyResponse6[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse6[cnt1].Answer) : 0;
                                                        }

                                                        if (sumValue > 0 && cnt1 > 0)
                                                        {
                                                            double average = sumValue / cnt1;

                                                            OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                                            objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse6[0].ResponseID;
                                                            objBeneficiaryResponse.Anwer = average;
                                                            objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse6[0].SurveyID;
                                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT SUM(Answer) AnswerSum,(SELECT COUNT(QuestionID) FROM TbProjectSurveyResponses WHERE QuestionID = resp.QuestionID " + subqury + ") ResponseCount,SurveyID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subqury;
                        query1 += " GROUP BY SurveyID;";

                        objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();
                    }
                    else
                    {
                        List<OutPutQuestionRSP> lstOutPutQuestionRSP = new List<OutPutQuestionRSP>();
                        query1 = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,SurveyID from TbProjectSurveyResponses where  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                        query1 += subqury;

                        lstOutPutQuestionRSP = _connection.Query<OutPutQuestionRSP>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        if (lstOutPutQuestionRSP != null && lstOutPutQuestionRSP.Count > 0)
                        {
                            var mySKUs = lstOutPutQuestionRSP.Select(l => l.SyncTaskID).Distinct().ToList();
                            string joinedSyncTaskID = string.Join(",", mySKUs);

                            if (joinedSyncTaskID != null && joinedSyncTaskID.Length > 0)
                            {
                                objOutPutCount.SurveyID = lstOutPutQuestionRSP[0].SurveyID;

                                List<OutPutQuestionRSP> lstOutPutQuestionRSP1 = new List<OutPutQuestionRSP>();

                                string QSTQry = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount from TbProjectSurveyResponses where SyncTaskID in(" + joinedSyncTaskID + ") and ifnull(IsActive,0)=1 and QuestionID=@QuestionID;";
                                lstOutPutQuestionRSP1 = _connection.Query<OutPutQuestionRSP>(QSTQry, new { QuestionID = additionalSurveyQuestionId }).ToList();

                                if (lstOutPutQuestionRSP1 != null && lstOutPutQuestionRSP1.Count > 0)
                                {
                                    foreach (var itm in lstOutPutQuestionRSP)
                                    {
                                        if (lstOutPutQuestionRSP2.Count == 0)
                                        {
                                            lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                        }
                                        else
                                        {
                                            OutPutQuestionRSP objOutPutQuestionRSP = new OutPutQuestionRSP();
                                            objOutPutQuestionRSP = lstOutPutQuestionRSP2.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();

                                            if (objOutPutQuestionRSP == null)
                                            {
                                                lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (i == objActivityDataCollectionDetails.Count - 1 && additionalSurveyQuestionId > 0)
                    {
                        double answerSum = lstOutPutQuestionRSP2.Sum(a => Convert.ToDouble(a.AnswerString));
                        int responseCount = lstOutPutQuestionRSP2.Count(); // lstOutPutQuestionRSP2.Sum(a => a.SubmissionCount);

                        if (answerSum > 0 && responseCount > 0)
                        {
                            objOutPutCount.AnswerSum = answerSum;
                            objOutPutCount.ResponseCount = responseCount;
                        }
                    }

                    if (objOutPutCount != null && objOutPutCount.ResponseCount > 0)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        double sum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToDouble(x.Anwer));
                        objOutPutCount.AnswerSum = (sum / objOutPutCount.ResponseCount);
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                int toCount = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToInt32(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                        toCount += lstOutPutCount.Where(x => x.SurveyID == Convert.ToInt32(spltsurveyId[cnt6])).Sum(x => x.ResponseCount);
                    }
                }

                result = totsum / toCount;
                progressPercentage = (result * 100) / Convert.ToDouble(SurveyTarget);
            }

            return progressPercentage;
        }
        #endregion GetQuantitativeAverage

        #region GetQuantitativeCumulativeSum
        /// <summary>
        /// GetQuantitativeCumulativeSum
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeCumulativeSum(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();
            List<OutcomeSurveyResponse> lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic -  starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition  from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse2 = new List<OutcomeSurveyResponse>();

                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                        }
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                // lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse4 = new List<OutcomeSurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        QSTQry = "select RES.ResponseID,reg.Answer,reg.QuestionID,RES.SurveyID,RES.SyncTaskID,ifnull(RES.SubmissionCount,1) SubmissionCount ";
                                        QSTQry += "from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID ";
                                        QSTQry += "JOIN TbProjectSurveyResponses RES ON RES.ResponseID = reg.BeneficiaryID WHERE ifnull(ben.IsActive,0)=1  and ";
                                        QSTQry += "ifnull(RES.IsActive,0)=1 and RES.ResponseID=@ResponseID  and RES.SyncTaskID in(" + joinedSyncTaskID + ")  and reg.QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0 && additionalSurveyQuestionId <= 0)
                            {
                                double sumValue = 0;
                                int cnt1 = 0;

                                for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                {
                                    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                }

                                if (sumValue > 0)
                                {
                                    OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                    objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                    objBeneficiaryResponse.Anwer = sumValue;
                                    objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                    lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                }
                            }
                            else if (additionalSurveyQuestionId > 0)
                            {
                                if (i == objActivityDataCollectionDetails.Count - 1 && cnt == lstActivitySurveyResponse1.Count - 1)
                                {
                                    if (lstActivitySurveyResponse5 != null && lstActivitySurveyResponse5.Count > 0)
                                    {
                                        double sumValue = 0;
                                        int cnt1 = 0;

                                        for (cnt1 = 0; cnt1 < lstActivitySurveyResponse5.Count; cnt1++)
                                        {
                                            sumValue += (lstActivitySurveyResponse5[cnt1].Answer != null && lstActivitySurveyResponse5[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse5[cnt1].Answer) : 0;
                                        }

                                        if (sumValue > 0 && cnt1 > 0)
                                        {
                                            OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                            objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse5[0].ResponseID;
                                            objBeneficiaryResponse.Anwer = sumValue;
                                            objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse5[0].SurveyID;
                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                            lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    string subQury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT SUM(Answer) AnswerSum,(SELECT COUNT(QuestionID) FROM TbProjectSurveyResponses WHERE QuestionID = resp.QuestionID " + subQury + ") ResponseCount,SurveyID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subQury;
                        query1 += " GROUP BY SurveyID;";

                        objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();
                    }
                    else
                    {
                        List<OutPutQuestionRSP> lstOutPutQuestionRSP = new List<OutPutQuestionRSP>();
                        query1 = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,SurveyID from TbProjectSurveyResponses where  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                        query1 += subQury;

                        lstOutPutQuestionRSP = _connection.Query<OutPutQuestionRSP>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        if (lstOutPutQuestionRSP != null && lstOutPutQuestionRSP.Count > 0)
                        {
                            var mySKUs = lstOutPutQuestionRSP.Select(l => l.SyncTaskID).Distinct().ToList();
                            string joinedSyncTaskID = string.Join(",", mySKUs);

                            if (joinedSyncTaskID != null && joinedSyncTaskID.Length > 0)
                            {
                                objOutPutCount.SurveyID = lstOutPutQuestionRSP[0].SurveyID;

                                List<OutPutQuestionRSP> lstOutPutQuestionRSP1 = new List<OutPutQuestionRSP>();

                                string QSTQry = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount from TbProjectSurveyResponses where SyncTaskID in(" + joinedSyncTaskID + ") and ifnull(IsActive,0)=1 and QuestionID=@QuestionID;";
                                lstOutPutQuestionRSP1 = _connection.Query<OutPutQuestionRSP>(QSTQry, new { QuestionID = additionalSurveyQuestionId }).ToList();

                                if (lstOutPutQuestionRSP1 != null && lstOutPutQuestionRSP1.Count > 0)
                                {
                                    foreach (var itm in lstOutPutQuestionRSP)
                                    {
                                        if (lstOutPutQuestionRSP2.Count == 0)
                                        {
                                            lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                        }
                                        else
                                        {
                                            OutPutQuestionRSP objOutPutQuestionRSP = new OutPutQuestionRSP();
                                            objOutPutQuestionRSP = lstOutPutQuestionRSP2.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();

                                            if (objOutPutQuestionRSP == null)
                                            {
                                                lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (i == objActivityDataCollectionDetails.Count - 1 && additionalSurveyQuestionId > 0)
                    {
                        double answerSum = lstOutPutQuestionRSP2.Sum(a => Convert.ToDouble(a.AnswerString));

                        if (answerSum > 0)
                        {
                            objOutPutCount.AnswerSum = answerSum;
                        }
                    }

                    if (objOutPutCount != null && objOutPutCount.AnswerSum > 0)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer));
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToDouble(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                    }
                }

                result = (totsum * 100) / Convert.ToDouble(SurveyTarget);
            }

            return result;
        }
        #endregion GetQuantitativeCumulativeSum

        #region GetQuantitativeUniqueResponses
        /// <summary>
        /// GetQuantitativeUniqueResponses
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeUniqueResponses(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();

            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic - starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //  lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                lstActivitySurveyResponse3 = lstActivitySurveyResponse3.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                                OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                objBeneficiaryResponse.Anwer = lstActivitySurveyResponse3.Count;
                                objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    query1 = " SELECT COUNT(Answer) AnswerSum,0 AS ResponseCount,max(SurveyID) SurveyID";
                    query1 += " FROM TbProjectSurveyResponses WHERE QuestionID =  @QuestionId AND ifnull(IsActive,0) = 1  ";
                    query1 += GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    query1 += " GROUP BY QuestionID;";

                    objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();

                    if (objOutPutCount != null)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                //List<ActivitySurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                //for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                //{
                //    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse22 = new List<ActivitySurveyResponse>();
                //    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponse.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();

                //    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse23 = new List<ActivitySurveyResponse>();
                //    lstBeneficiarySurveyResponse23 = lstBeneficiarySurveyResponse22.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                //    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                //    {
                //        BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                //        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                //        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                //        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse23.Count();
                //        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                //    }
                //}

                //if(lstBenActivitySurveyResponse2 != null && lstBenActivitySurveyResponse2.Count >0)
                //{
                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                //if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                //{
                //    OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                //    objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                //    objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                //    objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Count();
                //    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                //}
                // }

                foreach (var itm in lstBeneficiarySurveyResponseAll)
                {
                    OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = itm.ResponseID;
                    objBeneficiaryResponse1.SurveyID = itm.SurveyID;
                    objBeneficiaryResponse1.Anwer = 1;
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToInt32(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                    }
                }

                result = (totsum * 100) / Convert.ToDouble(SurveyTarget);
            }

            return result;
        }
        #endregion GetQuantitativeUniqueResponses

        #region GetNonBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetNonBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetNonBeneficiarySurveyLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.MultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetNonBeneficiarySurveyLogicQuery

        #region GetBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <returns></returns>
        public List<OutcomeSurveyResponse> GetBeneficiarySurveyLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, OutcomeDataCollectionDetails objActivityDataCollectionDetails)
        {
            if (lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)
            {
                foreach (var itm in lstSurveyPrimeLogic)
                {
                    switch (itm.SurveyLogicPrime)
                    {
                        case 3:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() == itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 4:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) > Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 5:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) >= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 6:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) < Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 7:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) <= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 8:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() != itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                    }
                }
            }
            return lstBeneficiarySurveyResponse;
        }
        #endregion GetBeneficiarySurveyLogicQuery

        #region GetNonBeneficiarySurveyLogicQueryNew
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <returns></returns>
        public List<OutcomeSurveyResponse> GetNonBeneficiarySurveyLogicQueryNew(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, OutcomeDataCollectionDetails objActivityDataCollectionDetails)
        {
            foreach (var itm in lstSurveyPrimeLogic)
            {
                switch (itm.SurveyLogicPrime)
                {
                    case 3:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() == itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 4:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) > Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 5:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) >= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 6:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) < Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 7:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) <= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 8:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() != itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                }
            }

            return lstBeneficiarySurveyResponse;
        }
        #endregion GetNonBeneficiarySurveyLogicQueryNew

        #region GetBeneficiarySurveyLogicQuery2
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery2
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetBeneficiarySurveyLogicQuery2(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.BenMultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.CascadingDropdown)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " reg.Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " reg.Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " reg.Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " reg.Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetBeneficiarySurveyLogicQuery2

        #region GetNonBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetNonBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetBeneficiarySurveyResponseLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.MultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.CascadingDropdown)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetNonBeneficiarySurveyLogicQuery

        /// <summary>
        /// Function used to remove periodicity activity based on ActivityId,loggedUserId
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="ActIdList"></param>
        public void RemoveOutcome(int loggedUserId, string OutcomeIdList)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();
                        query = @"Update TbOutcomes set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId  where FIND_IN_SET(pk_OutcomeID,@OutcomeIdList);";
                        _connection.Execute(query, new { OutcomeIdList = OutcomeIdList, IsDeleted = 1, loggedUserId = loggedUserId });

                        query = "Select OutcomeName,ProjectId as ProjectID  From TbOutcomes Where FIND_IN_SET(pk_OutcomeID,@OutcomeIdList);";
                        ObjOutcome = _connection.Query<ProjectOutcome>(query, new { OutcomeIdList = OutcomeIdList }).ToList();


                        foreach (ProjectOutcome actItem in ObjOutcome)
                        {
                            //AddToActivityLog(actItem.ProjectId, actItem.ActivityId, "Removed the activity named " + actItem.ActivityName, loggedUserId);
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = actItem.ProjectID;
                            objLog.AuditFrom = "Outcome";
                            objLog.AuditEvent = "REMOVE";
                            objLog.AuditFromID = actItem.ActivityId;
                            objLog.FromIDColumnName = "pk_OutcomeID";
                            objLog.Message = "Removed the outcome named " + actItem.OutcomeName;
                            objLog.CreatedBy = loggedUserId;
                            ProjectDAL.AddAuditLog(objLog);
                        }

                        //Remove Users from Activity
                        query = @"Update TbOutcomeAssignUsers Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                               Where FIND_IN_SET(OutcomeId,@OutcomeIdList);";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, OutcomeIdList = OutcomeIdList });

                        //Remove Expense Tags from Activity
                        query = @"Update TbOutcomeToExpenseTags Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                               Where FIND_IN_SET(OutcomeId,@OutcomeIdList);";
                        _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, OutcomeIdList = OutcomeIdList });

                        //Remove the Periodicity from activity
                        bool isDeleted = DeletePeriodicityForOutcome(OutcomeIdList);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }


        public bool AddUsersToOutcome(int OutcomeId, string userlist, int loggedUserId, int ProjectId = 0)
        {
            string query = string.Empty;
            var id = 0;
            bool success = DeleteUsersFromOutCome(OutcomeId, loggedUserId);

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    ProjectOutcome ObjOutcome = new ProjectOutcome();
                    query = "Select OutcomeName,ProjectId as ProjectID  From TbOutcomes Where pk_OutcomeID=@OutcomeId;";
                    ObjOutcome = _connection.Query<ProjectOutcome>(query, new { OutcomeId = OutcomeId }).FirstOrDefault();

                    AddToOutcomeLog(ProjectId, OutcomeId, "Assigned Users to the activity named " + ObjOutcome.OutcomeName, loggedUserId);

                    List<UsersInfo> objUserList = JsonConvert.DeserializeObject<List<UsersInfo>>(userlist);
                    foreach (UsersInfo userItem in objUserList)
                    {
                        query = @"INSERT INTO TbOutcomeAssignUsers(OutcomeId,UserID,ProjectId,IsDeleted,CreatedDate,CreatedBy)
                            VALUES(@OutcomeId,@UserID,@ProjectId,@IsDeleted,@CreatedDate,@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            OutcomeId = OutcomeId,
                            UserID = userItem.ID,
                            ProjectId = ProjectId,
                            IsDeleted = 0,
                            CreatedDate = DateTime.Now,
                            CreatedBy = loggedUserId
                        }).Single();

                        query = "Select Name  From TbUser Where ID =@UserID ";
                        var name = _connection.Query<string>(query, new { UserID = userItem.ID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Outcome";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = OutcomeId;
                        objLog.FromIDColumnName = "pk_OutcomeID";
                        objLog.Message = "Outcome assigned to user  " + name;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                        query = "Select Name  From TbUser Where ID =@UserID ";
                        var AssgName = _connection.Query<string>(query, new { UserID = loggedUserId }).Single();

                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "Outcome",
                            TaskTypeID = OutcomeId,
                            TypeIDColumn = "pk_OutcomeID",
                            AssignedTo = userItem.ID,
                            AssignedBy = loggedUserId,
                            Message = "Assigned the task to " + name,
                            createdby = loggedUserId
                        }).Single();

                        // Send Notification 
                        try
                        {
                            // Send Notification 
                            var title = "";
                            var sub = "";
                            query = "SELECT OutcomeName FROM TbOutcomes where pk_OutcomeID = @OutcomeID ";
                            var ActivityName = _connection.Query<string>(query, new { OutcomeID = OutcomeId }).Single();

                            title = AssgName + "Outcome ASSINGED";
                            sub = AssgName + "Added an new Outcome :" + ActivityName + "' on " + DateTime.Now;
                            Notification objNotification = new Notification();
                            objNotification.UserId = userItem.ID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }
                        catch
                        {

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

        #region GetOutcomeSurveyIds
        /// <summary>
        /// GetOutcomeSurveyIds
        /// </summary>
        /// <param name="outcomeId"></param>
        /// <returns></returns>
        public string GetOutcomeSurveyIds(int outcomeId = 0)
        {
            string surveyIds = "";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select GROUP_CONCAT(distinct SurveyId) from TbOutcomeSurveyCollection where OutcomeId = @OutcomeId and ifnull(IsActive,0) = 1 GROUP BY OutcomeId;";
                    surveyIds = _connection.Query<string>(strQuery, new { OutcomeId = outcomeId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return surveyIds;
        }
        #endregion GetOutcomeSurveyIds


        #region GetOutcomePivotPresetList
        /// <summary>
        /// GetOutcomePivotPresetList
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public List<OutcomePivotPresetData> GetOutcomePivotPresetList(int UserID, int OutcomeId)
        {
            List<OutcomePivotPresetData> pivotList = new List<OutcomePivotPresetData>();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "SELECT PresetName,PresetData,IFNULL(SectionId,0) SectionId FROM TbOutcomePivotPresetData WHERE UserID = @UserID AND OutcomeID=@OutcomeId;";
                    pivotList = _connection.Query<OutcomePivotPresetData>(query, new { UserID = UserID, OutcomeId = OutcomeId }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pivotList;
        }
        #endregion GetOutcomePivotPresetList

        #region SaveOutcomePivotPreset
        /// <summary>
        /// SaveOutcomePivotPreset
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PresetName"></param>
        /// <param name="OutcomeId"></param>
        /// <param name="PresetData"></param>
        /// <returns></returns>
        public bool SaveOutcomePivotPreset(int UserID, string PresetName, int OutcomeId, string PresetData, int SectionId = 0)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"INSERT INTO TbOutcomePivotPresetData(UserID,PresetName,OutcomeID,PresetData,CreatedDate,CreatedBy,SectionId) 
                                     VALUES (@UserID,@PresetName,@OutcomeId,@PresetData,now(),@UserID,@SectionId); ";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        OutcomeId = OutcomeId,
                        PresetData = PresetData,
                        SectionId = SectionId
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        #endregion SaveOutcomePivotPreset

        #region RestoreOutcomePivotPreset
        /// <summary>
        /// RestoreOutcomePivotPreset
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PresetName"></param>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public string RestoreOutcomePivotPreset(int UserID, string PresetName, int OutcomeId)
        {
            string presetData = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "SELECT PresetData FROM TbOutcomePivotPresetData WHERE UserID = @UserID AND PresetName = @PresetName AND OutcomeID=@OutcomeID;";
                    presetData = _connection.Query<string>(query, new { UserID = UserID, PresetName = PresetName, OutcomeID = OutcomeId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return presetData;
        }
        #endregion RestoreOutcomePivotPreset

        #region EditOutcomePivotPreset
        /// <summary>
        /// EditOutputPivotPreset
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PresetName"></param>
        /// <param name="OutcomeId"></param>
        /// <param name="PresetData"></param>
        /// <returns></returns>
        public bool EditOutcomePivotPreset(int UserID, string PresetName, int OutcomeId, string PresetData)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"UPDATE TbOutcomePivotPresetData SET  PresetData = @PresetData,UpdatedDate = now(),UpdatedBy = @UserID
                                     WHERE UserID = @UserID AND PresetName = @PresetName AND OutcomeID=@OutcomeId; ";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        OutcomeId = OutcomeId,
                        PresetData = PresetData
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        #endregion OutcomeId

        #region DeleteOutcomePivotPreset
        /// <summary>
        /// DeleteOutcomePivotPreset
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PresetName"></param>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public bool DeleteOutcomePivotPreset(int UserID, string PresetName, int OutcomeId)
        {
            bool status = false;
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"DELETE FROM TbOutcomePivotPresetData WHERE UserID = @UserID AND PresetName = @PresetName AND OutcomeID=@OutcomeId;";

                    _connection.Execute(strQuery, new
                    {
                        UserID = UserID,
                        PresetName = PresetName,
                        OutcomeId = OutcomeId
                    });

                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        #endregion DeleteOutcomePivotPreset


        #region GetOutComeProjectData
        /// <summary>
        ///GetOutComeProjectData
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetOutComeProjectData(int WorkspaceID = 0)
        {
            List<KeyValuePair<int, string>> lstproject = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (WorkspaceID != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"SELECT distinct t.ProjectId,t.ProjectName FROM TbProject t JOIN TbOutcomes ";
                        query += " outcome ON t.ProjectId = outcome.ProjectId WHERE IFNULL(t.IsDeleted,0)=@IsDeleted AND t.IsActive = @IsActive ";
                        query += " AND outcome.IsDeleted = @IsDeleted and t.WSID = @WSID and t.ProjectStatus='Deploy';";

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
        #endregion GetOutComeProjectData   

        #region GetOutComeListByProjectId
        /// <summary>
        /// GetOutComeListByProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetOutComeListByProjectId(int ProjectId = 0, DateTime? mStartDate = null, DateTime? mEndDate = null)
        {
            List<KeyValuePair<int, string>> lstproject = new List<KeyValuePair<int, string>>();
            dynamic obj;
            try
            {
                if (ProjectId != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @" SELECT outcome.pk_OutcomeID,outcome.OutcomeName FROM TbOutcomes outcome WHERE outcome.ProjectId = @ProjectId AND  ifnull(outcome.IsDeleted,0) = @IsDeleted ";

                        if ((mStartDate != null))
                        {
                            query += " AND outcome.StartDate >= @mStartDate";
                        }

                        if ((mEndDate != null))
                        {
                            query += " AND outcome.StartDate <= @mEndDate";
                        }


                        obj = _connection.Query<object>(query, new { IsDeleted = 0, ProjectId = ProjectId, mStartDate = mStartDate, mEndDate = mEndDate }).ToList();

                        foreach (dynamic item in obj)
                        {
                            lstproject.Add(new KeyValuePair<int, string>(item.pk_OutcomeID, item.OutcomeName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstproject;
        }
        #endregion GetOutComeListByProjectId   

        #region GetOutComeListByProjectId
        /// <summary>
        /// GetOutComeListByProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public OutcomeOverview GetOutComeCountByProjectId(int ProjectId = 0)
        {
            OutcomeOverview objOutcomeOverview = new OutcomeOverview();
            string stringifydate = string.Empty;

            try
            {
                if (ProjectId != 0)
                {
                    List<OutcomeOverview> lstOutcomeOverview = new List<OutcomeOverview>();

                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string query = @"select ifnull(OutcomeProgress,0) OutcomeProgress from TbOutcomes where ifnull(IsDeleted,0) = @IsDeleted and ProjectId = @ProjectId;";
                        lstOutcomeOverview = _connection.Query<OutcomeOverview>(query, new { IsDeleted = 0, ProjectId = ProjectId }).ToList();

                        string strQueryexec = @"SELECT  ifnull(DATE_FORMAT(ExecutedDateTime, '%Y-%m-%d %H:%i:%s'),'')  From TbOutputScheduler order by ID desc limit 1";
                        stringifydate = _connection.Query<string>(strQueryexec).FirstOrDefault();
                    }

                    if (stringifydate != null && stringifydate.Length > 0)
                    {
                        objOutcomeOverview.LastExecutedDateTime = stringifydate;
                    }

                    if (lstOutcomeOverview != null && lstOutcomeOverview.Count > 0)
                    {
                        objOutcomeOverview.TotalCount = lstOutcomeOverview.Count();
                        objOutcomeOverview.CompletedCount = lstOutcomeOverview.Where(p => p.OutcomeProgress >= 100).Count();
                        objOutcomeOverview.PendingCount = objOutcomeOverview.TotalCount - objOutcomeOverview.CompletedCount;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objOutcomeOverview;
        }
        #endregion GetOutComeListByProjectId   

        #region GetProjectOutcomeMonthwiseData
        /// <summary>
        /// GetProjectOutcomeMonthwiseData
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public List<ProjectOutcomeMonthwiseData> GetProjectOutcomeMonthwiseData(int OutcomeId)
        {
            List<ProjectOutcomeMonthwiseData> lstProjectOutcomeMonthwiseData = new List<ProjectOutcomeMonthwiseData>();

            using (_connection = Utils.Database.GetDBConnection())
            {
                string query = "SELECT ID,OutcomeId,Target,Budget,ActMonth,ActYear FROM TbOutcomeMonthwiseData WHERE OutcomeId = @OutcomeId AND IFNULL(IsActive,0)=1";

                lstProjectOutcomeMonthwiseData = _connection.Query<ProjectOutcomeMonthwiseData>(query, new { OutcomeId = OutcomeId }).ToList();
            }

            return lstProjectOutcomeMonthwiseData;
        }
        #endregion GetProjectOutcomeMonthwiseData

        #region GetOutcomeActualValue
        /// <summary>
        /// GetOutcomeActualValue
        /// </summary>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public double GetOutcomeActualValue(int OutcomeId)
        {
            double actualValue = 0;

            List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();
            string strQuery = string.Empty;
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.pk_OutcomeID ActivityId,TbOutcomes.ProjectID, TbOutcomes.OutcomeName ActivityName,TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                            TbOutcomes.DatacollectionType ActivityType, TbOutcomes.DatacollectionType ,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                                 TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                            TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,ifnull(TbOutcomes.UpdatedDate,TbOutcomes.CreatedDate) UpdatedDate,TbPeriodicity.PeriodicityType as ReportingFrequencyName,TbOutcomes.Description,
                            ifnull(TbOutcomes.UnitOfMeasure,'') UnitOfMeasure,ifnull(TbOutcomes.AdditionalSectionId,0) AdditionalSectionId,ifnull(TbOutcomes.AdditionalQuestionId,0) AdditionalQuestionId,ifnull(TbOutcomes.AdditionalQuestionType,false) as AdditionalQuestionType,ifnull(AutoGeneratedActivityId,'') as AutoGeneratedActivityId,IFNULL(SDG_ID,0) AS SDGID  
                            from TbOutcomes TbOutcomes 
                            left join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                            left join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                            left join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                            where TbOutcomes.IsDeleted = 0 and TbOutcomes.pk_OutcomeID = @OutcomeId
                            order by TbOutcomes.pk_OutcomeID desc";
                    ObjOutcome = _connection.Query<ProjectOutcome>(strQuery, new { OutcomeId = OutcomeId }).ToList();

                    foreach (ProjectOutcome Item in ObjOutcome)
                    {
                        if (Item.DatacollectionType == "Quantitative")
                        {
                            actualValue = getData(Item.ActivityId, Item.SurveyLogic, Item.SurveyTarget, Convert.ToInt32(Item.AdditionalSectionId), Item.AdditionalQuestionId, Item.AdditionalQuestionType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return actualValue;
        }
        #endregion GetOutcomeActualValue


        #region getDataForActualValue
        /// <summary>
        /// getData
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="logicType"></param>
        /// <param name="value"></param>
        public double getDataForActualValue(int OutcomeID, int logicType, string SurveyTarget, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();
            List<OutcomeDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();

            string query = string.Empty;

            double result = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "SELECT ACT.ID,ACT.OutcomeId ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                    query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType FROM TbProjectSurvey SRV ";
                    query += "LEFT JOIN TbOutcomeSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                    query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                    query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                    query += "where ACT.OutcomeId = @OutcomeId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1; ";

                    objActivityDataCollectionDetails = _connection.Query<OutcomeDataCollectionDetails>(query, new { OutcomeId = OutcomeID }).ToList();

                    if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                    {
                        var mySKUs = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                        string joinedQuestionIds = string.Join(",", mySKUs);

                        var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                        string joinedBeneficiaryTypeID = string.Join(",", mySKUs1);

                        var benficiaryIds = objActivityDataCollectionDetails.Where(x => x.BeneficiaryQuestionID > 0).Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                        string joinedBeneficiaryQuestionID = string.Join(",", benficiaryIds);

                        List<OutcomeSurveyResponse> lstActivitySurveyResponse = new List<OutcomeSurveyResponse>();
                        List<OutcomeSurveyResponse> lstActivitySurveyResponse1 = new List<OutcomeSurveyResponse>();
                        List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse = new List<OutcomeSurveyResponse>();

                        if (joinedQuestionIds != null && joinedQuestionIds.Length > 0)
                        {
                            string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where QuestionID in(" + joinedQuestionIds + ") and ifnull(IsActive,0)=1;";
                            lstActivitySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry).ToList();

                            lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                        }

                        if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0)
                        {
                            string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                            QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                            QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                            QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID in(" + joinedBeneficiaryQuestionID + ") and ifnull(reg.IsDeleted,0)=0;";
                            lstBeneficiarySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTBenQry).ToList();

                            lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                        }

                        if (logicType == (int)OutcomeLogicType.Average)
                        {
                            result = GetQuantitativeAverage_forActualValue(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse, additionalSurveyQuestionId, additionalQuestionType);
                        }
                        else if (logicType == (int)OutcomeLogicType.Cumulative_Sum)
                        {
                            result = GetQuantitativeCumulativeSum_forActualValue(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse, additionalSurveyQuestionId, additionalQuestionType);
                        }
                        else if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                        {
                            result = GetQuantitativeUniqueResponses_forActualValue(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse);
                        }
                        else
                        {
                            double totDays = (objActivityDataCollectionDetails[0].EndDate - objActivityDataCollectionDetails[0].StartDate).TotalDays;
                            double totDaysFnished = (DateTime.Now - objActivityDataCollectionDetails[0].StartDate).TotalDays;
                            result = (int)((totDaysFnished * 100) / totDays);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion getDataForActualValue

        #region GetQuantitativeAverage_forActualValue
        /// <summary>
        /// GetQuantitativeAverage_forActualValue
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeAverage_forActualValue(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            double progressPercentage = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();
            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();
            List<OutcomeSurveyResponse> lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = null;
            lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();

                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();


                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse2 = new List<OutcomeSurveyResponse>();

                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                        }
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse4 = new List<OutcomeSurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        QSTQry = "select RES.ResponseID,reg.Answer,reg.QuestionID,RES.SurveyID,RES.SyncTaskID,ifnull(RES.SubmissionCount,1) SubmissionCount ";
                                        QSTQry += "from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID ";
                                        QSTQry += "JOIN TbProjectSurveyResponses RES ON RES.ResponseID = reg.BeneficiaryID WHERE ifnull(ben.IsActive,0)=1  and ";
                                        QSTQry += "ifnull(RES.IsActive,0)=1 and RES.ResponseID=@ResponseID  and RES.SyncTaskID in(" + joinedSyncTaskID + ")  and reg.QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0 && additionalSurveyQuestionId <= 0)
                            {
                                double sumValue = 0;
                                int cnt1 = 0;

                                for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                {
                                    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                }

                                if (sumValue > 0 && cnt1 > 0)
                                {
                                    double average = sumValue / cnt1;

                                    OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                    objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                    objBeneficiaryResponse.Anwer = average;
                                    objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                    lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                }
                            }
                            else if (additionalSurveyQuestionId > 0)
                            {
                                if (i == objActivityDataCollectionDetails.Count - 1 && cnt == lstActivitySurveyResponse1.Count - 1)
                                {
                                    if (lstActivitySurveyResponse5 != null && lstActivitySurveyResponse5.Count > 0)
                                    {
                                        var mySKUs1 = lstActivitySurveyResponse5.Select(l => l.ResponseID).Distinct().ToList();

                                        string joinedResponseID = string.Join(",", mySKUs1);

                                        if (!string.IsNullOrEmpty(joinedResponseID))
                                        {
                                            string[] splt = joinedResponseID.Split(',');

                                            if (splt != null && splt.Length > 0)
                                            {
                                                foreach (var itm10 in splt)
                                                {
                                                    double sumValue = 0;
                                                    int cnt1 = 0;

                                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse6 = new List<OutcomeSurveyResponse>();

                                                    lstActivitySurveyResponse6 = lstActivitySurveyResponse5.Where(x => x.ResponseID == Convert.ToInt32(itm10)).ToList();

                                                    if (lstActivitySurveyResponse6 != null && lstActivitySurveyResponse6.Count > 0)
                                                    {
                                                        for (cnt1 = 0; cnt1 < lstActivitySurveyResponse6.Count; cnt1++)
                                                        {
                                                            sumValue += (lstActivitySurveyResponse6[cnt1].Answer != null && lstActivitySurveyResponse6[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse6[cnt1].Answer) : 0;
                                                        }

                                                        if (sumValue > 0 && cnt1 > 0)
                                                        {
                                                            double average = sumValue / cnt1;

                                                            OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                                            objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse6[0].ResponseID;
                                                            objBeneficiaryResponse.Anwer = average;
                                                            objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse6[0].SurveyID;
                                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT SUM(Answer) AnswerSum,(SELECT COUNT(QuestionID) FROM TbProjectSurveyResponses WHERE QuestionID = resp.QuestionID " + subqury + ") ResponseCount,SurveyID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subqury;
                        query1 += " GROUP BY SurveyID;";

                        objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();
                    }
                    else
                    {
                        List<OutPutQuestionRSP> lstOutPutQuestionRSP = new List<OutPutQuestionRSP>();
                        query1 = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,SurveyID from TbProjectSurveyResponses where  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                        query1 += subqury;

                        lstOutPutQuestionRSP = _connection.Query<OutPutQuestionRSP>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        if (lstOutPutQuestionRSP != null && lstOutPutQuestionRSP.Count > 0)
                        {
                            var mySKUs = lstOutPutQuestionRSP.Select(l => l.SyncTaskID).Distinct().ToList();
                            string joinedSyncTaskID = string.Join(",", mySKUs);

                            if (joinedSyncTaskID != null && joinedSyncTaskID.Length > 0)
                            {
                                objOutPutCount.SurveyID = lstOutPutQuestionRSP[0].SurveyID;

                                List<OutPutQuestionRSP> lstOutPutQuestionRSP1 = new List<OutPutQuestionRSP>();

                                string QSTQry = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount from TbProjectSurveyResponses where SyncTaskID in(" + joinedSyncTaskID + ") and ifnull(IsActive,0)=1 and QuestionID=@QuestionID;";
                                lstOutPutQuestionRSP1 = _connection.Query<OutPutQuestionRSP>(QSTQry, new { QuestionID = additionalSurveyQuestionId }).ToList();

                                if (lstOutPutQuestionRSP1 != null && lstOutPutQuestionRSP1.Count > 0)
                                {
                                    foreach (var itm in lstOutPutQuestionRSP)
                                    {
                                        if (lstOutPutQuestionRSP2.Count == 0)
                                        {
                                            lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                        }
                                        else
                                        {
                                            OutPutQuestionRSP objOutPutQuestionRSP = new OutPutQuestionRSP();
                                            objOutPutQuestionRSP = lstOutPutQuestionRSP2.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();

                                            if (objOutPutQuestionRSP == null)
                                            {
                                                lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (i == objActivityDataCollectionDetails.Count - 1 && additionalSurveyQuestionId > 0)
                    {
                        double answerSum = lstOutPutQuestionRSP2.Sum(a => Convert.ToDouble(a.AnswerString));
                        int responseCount = lstOutPutQuestionRSP2.Count(); // lstOutPutQuestionRSP2.Sum(a => a.SubmissionCount);

                        if (answerSum > 0 && responseCount > 0)
                        {
                            objOutPutCount.AnswerSum = answerSum;
                            objOutPutCount.ResponseCount = responseCount;
                        }
                    }

                    if (objOutPutCount != null && objOutPutCount.ResponseCount > 0)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        double sum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToDouble(x.Anwer));
                        objOutPutCount.AnswerSum = (sum / objOutPutCount.ResponseCount);
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                int toCount = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToInt32(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                        toCount += lstOutPutCount.Where(x => x.SurveyID == Convert.ToInt32(spltsurveyId[cnt6])).Sum(x => x.ResponseCount);
                    }
                }

                result = totsum / toCount;
                progressPercentage = result;//(result * 100) / Convert.ToDouble(SurveyTarget);
            }

            return progressPercentage;
        }
        #endregion GetQuantitativeAverage_forActualValue

        #region GetQuantitativeCumulativeSum_forActualValue
        /// <summary>
        /// GetQuantitativeCumulativeSum_forActualValue
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeCumulativeSum_forActualValue(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();
            List<OutcomeSurveyResponse> lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic -  starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition  from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse2 = new List<OutcomeSurveyResponse>();

                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                        }
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                // lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse4 = new List<OutcomeSurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        QSTQry = "select RES.ResponseID,reg.Answer,reg.QuestionID,RES.SurveyID,RES.SyncTaskID,ifnull(RES.SubmissionCount,1) SubmissionCount ";
                                        QSTQry += "from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID ";
                                        QSTQry += "JOIN TbProjectSurveyResponses RES ON RES.ResponseID = reg.BeneficiaryID WHERE ifnull(ben.IsActive,0)=1  and ";
                                        QSTQry += "ifnull(RES.IsActive,0)=1 and RES.ResponseID=@ResponseID  and RES.SyncTaskID in(" + joinedSyncTaskID + ")  and reg.QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0 && additionalSurveyQuestionId <= 0)
                            {
                                double sumValue = 0;
                                int cnt1 = 0;

                                for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                {
                                    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                }

                                if (sumValue > 0)
                                {
                                    OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                    objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                    objBeneficiaryResponse.Anwer = sumValue;
                                    objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                    lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                }
                            }
                            else if (additionalSurveyQuestionId > 0)
                            {
                                if (i == objActivityDataCollectionDetails.Count - 1 && cnt == lstActivitySurveyResponse1.Count - 1)
                                {
                                    if (lstActivitySurveyResponse5 != null && lstActivitySurveyResponse5.Count > 0)
                                    {
                                        double sumValue = 0;
                                        int cnt1 = 0;

                                        for (cnt1 = 0; cnt1 < lstActivitySurveyResponse5.Count; cnt1++)
                                        {
                                            sumValue += (lstActivitySurveyResponse5[cnt1].Answer != null && lstActivitySurveyResponse5[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse5[cnt1].Answer) : 0;
                                        }

                                        if (sumValue > 0 && cnt1 > 0)
                                        {
                                            OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                            objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse5[0].ResponseID;
                                            objBeneficiaryResponse.Anwer = sumValue;
                                            objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse5[0].SurveyID;
                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                            lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    string subQury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT SUM(Answer) AnswerSum,(SELECT COUNT(QuestionID) FROM TbProjectSurveyResponses WHERE QuestionID = resp.QuestionID " + subQury + ") ResponseCount,SurveyID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subQury;
                        query1 += " GROUP BY SurveyID;";

                        objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();
                    }
                    else
                    {
                        List<OutPutQuestionRSP> lstOutPutQuestionRSP = new List<OutPutQuestionRSP>();
                        query1 = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,SurveyID from TbProjectSurveyResponses where  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                        query1 += subQury;

                        lstOutPutQuestionRSP = _connection.Query<OutPutQuestionRSP>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        if (lstOutPutQuestionRSP != null && lstOutPutQuestionRSP.Count > 0)
                        {
                            var mySKUs = lstOutPutQuestionRSP.Select(l => l.SyncTaskID).Distinct().ToList();
                            string joinedSyncTaskID = string.Join(",", mySKUs);

                            if (joinedSyncTaskID != null && joinedSyncTaskID.Length > 0)
                            {
                                objOutPutCount.SurveyID = lstOutPutQuestionRSP[0].SurveyID;

                                List<OutPutQuestionRSP> lstOutPutQuestionRSP1 = new List<OutPutQuestionRSP>();

                                string QSTQry = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount from TbProjectSurveyResponses where SyncTaskID in(" + joinedSyncTaskID + ") and ifnull(IsActive,0)=1 and QuestionID=@QuestionID;";
                                lstOutPutQuestionRSP1 = _connection.Query<OutPutQuestionRSP>(QSTQry, new { QuestionID = additionalSurveyQuestionId }).ToList();

                                if (lstOutPutQuestionRSP1 != null && lstOutPutQuestionRSP1.Count > 0)
                                {
                                    foreach (var itm in lstOutPutQuestionRSP)
                                    {
                                        if (lstOutPutQuestionRSP2.Count == 0)
                                        {
                                            lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                        }
                                        else
                                        {
                                            OutPutQuestionRSP objOutPutQuestionRSP = new OutPutQuestionRSP();
                                            objOutPutQuestionRSP = lstOutPutQuestionRSP2.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();

                                            if (objOutPutQuestionRSP == null)
                                            {
                                                lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (i == objActivityDataCollectionDetails.Count - 1 && additionalSurveyQuestionId > 0)
                    {
                        double answerSum = lstOutPutQuestionRSP2.Sum(a => Convert.ToDouble(a.AnswerString));

                        if (answerSum > 0)
                        {
                            objOutPutCount.AnswerSum = answerSum;
                        }
                    }

                    if (objOutPutCount != null && objOutPutCount.AnswerSum > 0)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer));
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToDouble(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                    }
                }

                result = totsum; //(totsum * 100) / Convert.ToDouble(SurveyTarget);
            }

            return result;
        }
        #endregion GetQuantitativeCumulativeSum_forActualValue

        #region GetQuantitativeUniqueResponses_forActualValue
        /// <summary>
        /// GetQuantitativeUniqueResponses_forActualValue
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public double GetQuantitativeUniqueResponses_forActualValue(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();

            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic - starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID and SurveyID=@SurveyID limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyID = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //  lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                lstActivitySurveyResponse3 = lstActivitySurveyResponse3.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                                OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                objBeneficiaryResponse.Anwer = lstActivitySurveyResponse3.Count;
                                objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    query1 = " SELECT COUNT(Answer) AnswerSum,0 AS ResponseCount,max(SurveyID) SurveyID";
                    query1 += " FROM TbProjectSurveyResponses WHERE QuestionID =  @QuestionId AND ifnull(IsActive,0) = 1  ";
                    query1 += GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    query1 += " GROUP BY QuestionID;";

                    objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();

                    if (objOutPutCount != null)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                foreach (var itm in lstBeneficiarySurveyResponseAll)
                {
                    OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = itm.ResponseID;
                    objBeneficiaryResponse1.SurveyID = itm.SurveyID;
                    objBeneficiaryResponse1.Anwer = 1;
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToInt32(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                    }
                }

                result = totsum;//(totsum * 100) / Convert.ToDouble(SurveyTarget);
            }

            return result;
        }
        #endregion GetQuantitativeUniqueResponses_forActualValue

        #region GetOutcomeQuantitativeAverage
        /// <summary>
        /// GetOutcomeQuantitativeAverage
        /// </summary>
        /// <param name="lstProjectOutcomeInitialData"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="additionalSurveyQuestionId"></param>
        /// <param name="additionalQuestionType"></param>
        /// <returns></returns>
        public double GetOutcomeQuantitativeAverage(List<ProjectOutcomeInitialData> lstProjectOutcomeInitialData, List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, string SurveyTarget, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false, bool IsSurveyQuestion = true)
        {
            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse = new List<OutcomeSurveyResponse>();
            string query = string.Empty;

            double result = 0;

            var myBenRes = lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == 0).Select(l => l.ResponseID).Distinct().ToList();
            string joinedBeneficiaryResponseIDs = string.Join(",", myBenRes);

            if (lstProjectOutcomeInitialData != null && lstProjectOutcomeInitialData.Count > 0)
            {
                var myRes = lstProjectOutcomeInitialData.Select(l => l.ResponseID).Distinct().ToList();
                string joinedResponseIDs = string.Join(",", myRes);

                if (joinedResponseIDs == "0")
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);

                    double sum = 0;
                    double count = 0;

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                    string joinedQuestionIds = string.Join(",", mySKUs1);

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        foreach (var itm in lstProjectOutcomeInitialData)
                        {
                            if (lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).Count() > 0)
                            {
                                count++;
                                sum = sum + lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).Select(x => Convert.ToDouble(x.Answer)).First();
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        foreach (var itm in lstProjectOutcomeInitialData)
                        {
                            if (lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).Count() > 0)
                            {
                                count++;
                                sum = sum + lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).Select(x => Convert.ToDouble(x.Answer)).First();
                            }
                        }
                    }

                    if (lstOutcomeSurveyResponse != null && lstOutcomeSurveyResponse.Count > 0)
                    {
                        //  double totalSum = lstOutcomeSurveyResponse.Sum(x => Convert.ToDouble(x.Answer));
                        //double totalItem = lstOutcomeSurveyResponse.Count;

                        result = sum / count;
                        //result = (result * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
                else if (joinedResponseIDs != "0" && joinedBeneficiaryResponseIDs == "0")
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);
                    List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

                    double sum = 0;
                    double count = 0;

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                    string joinedQuestionIds = string.Join(",", mySKUs1);

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (IsSurveyQuestion)
                        {
                            string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        } 
                        else
                        {
                            string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        }

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;
                            double average = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = average });
                                }
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;
                            double average = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }

                    if (lstProjectOutcomeBeneficiaryData != null && lstProjectOutcomeBeneficiaryData.Count > 0)
                    {
                        double totalSum = lstProjectOutcomeBeneficiaryData.Sum(x => Convert.ToDouble(x.TotalSum));
                        double totalItem = lstProjectOutcomeBeneficiaryData.Count;

                        result = totalSum / totalItem;
                        //result = (result * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
                else
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);
                    List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

                    var mySurveyId = objActivityDataCollectionDetails.Select(l => l.SurveyId).Distinct().ToList();
                    string joinedSurveyIds = string.Join(",", mySurveyId);

                    double sum = 0;
                    double count = 0;

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                    string joinedBeneficiaryQuestionIDs = string.Join(",", mySKUs1);

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (IsSurveyQuestion)
                        {
                            string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseIDs + ") and SurveyID in(" + joinedSurveyIds + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId }).ToList();
                        }
                        else
                        {
                            string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        }

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "SELECT reg.BeneficiaryID AS ResponseID,reg.QuestionID,reg.Answer FROM TbBeneficiaryRegistrationDetails reg  WHERE reg.QuestionID  in(" + joinedBeneficiaryQuestionIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;
                            //double average = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }

                    if (lstProjectOutcomeBeneficiaryData != null && lstProjectOutcomeBeneficiaryData.Count > 0)
                    {
                        double totalSum = lstProjectOutcomeBeneficiaryData.Sum(x => Convert.ToDouble(x.TotalSum));
                        double totalItem = lstProjectOutcomeBeneficiaryData.Count;

                        result = totalSum / totalItem;
                        //result = (result * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
            }

            return result;
        }
        #endregion GetOutcomeQuantitativeAverage

        #region GetOutcomeQuantitativeCumulativeSum
        /// <summary>
        /// GetOutcomeQuantitativeCumulativeSum
        /// </summary>
        /// <param name="lstProjectOutcomeInitialData"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="additionalSurveyQuestionId"></param>
        /// <param name="additionalQuestionType"></param>
        /// <returns></returns>
        public double GetOutcomeQuantitativeCumulativeSum(List<ProjectOutcomeInitialData> lstProjectOutcomeInitialData, List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, string SurveyTarget, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false, bool IsSurveyQuestion = true)
        {
            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse = new List<OutcomeSurveyResponse>();
            string query = string.Empty;

            double result = 0;

            var myBenRes = lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == 0).Select(l => l.ResponseID).Distinct().ToList();
            string joinedBeneficiaryResponseIDs = string.Join(",", myBenRes);

            if (lstProjectOutcomeInitialData != null && lstProjectOutcomeInitialData.Count > 0)
            {
                var myRes = lstProjectOutcomeInitialData.Select(l => l.ResponseID).Distinct().ToList();
                string joinedResponseIDs = string.Join(",", myRes);

                if (joinedResponseIDs == "0")
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                    string joinedQuestionIds = string.Join(",", mySKUs1);

                    double sum = 0;

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE   SyncTaskID in(" + joinedSyncTaskIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        foreach (var itm in lstProjectOutcomeInitialData)
                        {
                            if (lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).Count() > 0)
                            {
                                sum = sum + lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).Select(x => Convert.ToDouble(x.Answer)).First();
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        foreach (var itm in lstProjectOutcomeInitialData)
                        {
                            if (lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).Count() > 0)
                            {
                                sum = sum + lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).Select(x => Convert.ToDouble(x.Answer)).First();
                            }
                        }
                    }

                    if (lstOutcomeSurveyResponse != null && lstOutcomeSurveyResponse.Count > 0)
                    {
                        result = sum;//(sum * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
                else if (joinedResponseIDs != "0" && joinedBeneficiaryResponseIDs == "0")
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);
                    List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

                    double sum = 0;
                    double count = 0;

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                    string joinedQuestionIds = string.Join(",", mySKUs1);

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (IsSurveyQuestion)
                        {
                            string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        } 
                        else
                        {
                            string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        }

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;
                            //double average = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }

                    if (lstProjectOutcomeBeneficiaryData != null && lstProjectOutcomeBeneficiaryData.Count > 0)
                    {
                        double totalSum = lstProjectOutcomeBeneficiaryData.Sum(x => Convert.ToDouble(x.TotalSum));
                        //double totalItem = lstProjectOutcomeBeneficiaryData.Count;

                        //result = totalSum / totalItem;
                        result = totalSum;//(totalSum * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
                else
                {
                    var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                    string joinedSyncTaskIDs = string.Join(",", mySKUs);
                    List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

                    var mySurveyId = objActivityDataCollectionDetails.Select(l => l.SurveyId).Distinct().ToList();
                    string joinedSurveyIds = string.Join(",", mySurveyId);

                    double sum = 0;
                    double count = 0;

                    var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                    string joinedBeneficiaryQuestionIDs = string.Join(",", mySKUs1);

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (IsSurveyQuestion)
                        {
                            string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseIDs + ") and SurveyID in(" + joinedSurveyIds + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        } else
                        {
                            string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                            lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                        }

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }
                    else
                    {
                        string QSTQry4 = "SELECT reg.BeneficiaryID AS ResponseID,reg.QuestionID,reg.Answer FROM TbBeneficiaryRegistrationDetails reg  WHERE reg.QuestionID  in(" + joinedBeneficiaryQuestionIDs + ");";
                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                        string[] strSplit = joinedResponseIDs.Split(",");

                        if (strSplit != null && strSplit.Length > 0)
                        {
                            double totSum = 0;
                            //double average = 0;

                            foreach (var itmres in strSplit)
                            {
                                totSum = 0;
                                List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();

                                if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                {
                                    totSum = lstOutcomeSurveyResponse1.Sum(x => Convert.ToDouble(x.Answer));
                                    //average = Math.Round(totSum / lstOutcomeSurveyResponse1.Count, 2);
                                    lstProjectOutcomeBeneficiaryData.Add(new ProjectOutcomeBeneficiaryData { ResponseID = Convert.ToInt32(itmres), TotalSum = totSum });
                                }
                            }
                        }
                    }

                    if (lstProjectOutcomeBeneficiaryData != null && lstProjectOutcomeBeneficiaryData.Count > 0)
                    {
                        double totalSum = lstProjectOutcomeBeneficiaryData.Sum(x => Convert.ToDouble(x.TotalSum));
                        result = totalSum;//(totalSum * 100) / Convert.ToDouble(SurveyTarget);
                    }
                }
            }

            return result;
        }
        #endregion GetOutcomeQuantitativeCumulativeSum


        /// <summary>
        /// Function used to fetch all SDG details
        /// </summary>
        /// <returns></returns>
        public List<SDG> GetSDGListForOutcome()
        {
            List<SDG> lstSDG = new List<SDG>();
            try
            {
                string strQuery = string.Empty;

                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT pk_SDGID AS SDGID,concat(SDG_Goal,': ',SDG_Name) as SDG_Name,Status,0 as IsSelected,SDG_Image as SDGImage FROM TbSDGMaster Order by pk_SDGID asc;";
                    lstSDG = _connection.Query<SDG>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstSDG;
        }

        #endregion New Outcome Section
    }
}
