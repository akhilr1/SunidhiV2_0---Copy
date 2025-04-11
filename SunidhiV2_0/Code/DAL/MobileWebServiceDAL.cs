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
using SunidhiV2_0.Code.Common;
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;


namespace SunidhiV2_0.Code.DAL
{
    public class MobileWebServiceDAL
    {
        protected IDbConnection _connection;

        public IEnumerable<MyTask> SyncFromServer(int UserID, int SyncServerID, int WorkspaceID)
        {
            List<MyTask> lstMyTask_Survey = new List<MyTask>();
            List<MyTask> lstMyTask1 = new List<MyTask>();
            List<MyTask> lstMyTask = new List<MyTask>();
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_GetMyTaskList_Survey_NEW";
                    lstMyTask_Survey = _connection.Query<MyTask>(strQuery, new { loggedUserID = UserID, SyncDbID = SyncServerID, varWorkspaceID = WorkspaceID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                    lstMyTask.AddRange(lstMyTask_Survey);

                    strQuery = "sp_GetMyTaskList";
                    lstMyTask1 = _connection.Query<MyTask>(strQuery, new { loggedUserID = UserID, varSyncDbID = SyncServerID, varWorkspaceID = WorkspaceID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();

                    lstMyTask.AddRange(lstMyTask1);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstMyTask;

        }

        public IEnumerable<CompletedTask> GetMyCompletedTaskList(int UserID, int WorkspaceID, int SyncServerID, string LastSyncDatetime)
        {

            List<CompletedTask> lstMyCompletedTask = new List<CompletedTask>();
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //strQuery = @"SELECT distinct Type,WFLevelStatus, ActivityName TaskName,Description,TPA.CreatedDate,(select ProjectName from TbProject where ProjectId=TPA.ProjectId) ProjectName FROM TbMyTask  TMT
                    //           inner join TbProjectActivity TPA on TMT.TypeID = TPA.ActivityId and Type = 'Activity' where WFLevelStatus = 'Completed' and UserID=@loggedUserID";
                    // --- Commented On - 18-09-2020
                    //strQuery = @"SELECT distinct SyncTaskID,UserID,Type,TypeID,WFLevel,WFLevelStatus, ActivityName TaskName, Description, TPA.CreatedDate,(select ProjectName from TbProject where ProjectId = TPA.ProjectId) ProjectName FROM TbMyTask TMT
                    //             inner join TbProjectActivity TPA on TMT.TypeID = TPA.ActivityId and Type = 'Activity' where WFLevelStatus = 'Completed' and UserID =  @loggedUserID
                    //             Union 
                    //             SELECT distinct SyncTaskID,UserID,Type,TypeID,WFLevel,WFLevelStatus, SurveyName TaskName, SurveyDescription as Description, TPA.CreatedDate,(select ProjectName from TbProject where ProjectId = TPA.ProjectId) ProjectName FROM TbMyTask TMT
                    //             inner join TbProjectSurvey TPA on TMT.TypeID = TPA.SurveyID and Type = 'Survey' where WFLevelStatus = 'Completed' and UserID =  @loggedUserID";
                    // --- Comments Ends here --- 18-09-2020

                    //SELECT distinct SyncTaskID as SyncDBID,UserID,Type,TypeID,WFLevel,WFLevelStatus, ActivityName TaskName, TPA.Description, TMT.TaskSubmittedDate as CreatedDate,P.ProjectName,TMT.TaskDate,PER.PeriodicityType Periodicity, PER.PeriodicityId,PS.BeneficiaryTypeID,TPA.WorkflowID, TMT.TaskSubmittedDate, TMT.TaskUpdateComment,U.Name as TaskSubmittedBy FROM TbMyTask TMT
                    //             inner join TbProjectActivity TPA on TMT.TypeID = TPA.ActivityId and Type = 'Activity'
                    //            inner join TbProject P on TPA.ProjectId = P.ProjectId
                    //            inner join TbPeriodicity PER ON PER.PeriodicityId = TPA.Periodicity
                    //            inner join TbProjectSurvey PS on PS.SurveyID = TPA.SurveyID
                    //            left join TbProjectActivityTrackDate PATD on PATD.ActivityId = TPA.ActivityId and PATD.TrackDate = TMT.TaskDate
                    //            inner join TbUser U on U.ID = TMT.UserID
                    //            where WFLevelStatus = 'Completed' and NOT EXISTS(select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggedUserID and P.WSID = @WorkspaceID AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE 1 = 2 END
                    //        Union


                    //   SELECT distinct SyncTaskID as SyncDBID, UserID, Type, TypeID, WFLevel, WFLevelStatus, KPIName TaskName, KPI.Description as Description, TMT.TaskSubmittedDate as CreatedDate,
                    //    (select ProjectName from TbProject where ProjectId = KPI.ProjectID) ProjectName,TMT.TaskDate,PER.PeriodicityType Periodicity, PER.PeriodicityId,PS.BeneficiaryTypeID,PS.WorkflowID,TMT.TaskSubmittedDate, TMT.TaskUpdateComment,U.Name as TaskSubmittedBy
                    //    FROM TbMyTask TMT
                    //   inner join TbKPI KPI on TMT.TypeID = KPI.ID and Type = 'KPI'
                    //   inner join TbPeriodicity PER ON PER.PeriodicityId = KPI.KPIPeriodicity
                    //   inner join TbProjectSurvey PS on PS.SurveyID = KPI.SurveyID
                    //   left join TbKPITrackDate PSTD on PSTD.KPIID = KPI.ID and PSTD.TrackDate = TMT.TaskDate
                    //   inner join TbUser U on U.ID = TMT.UserID
                    //   where WFLevelStatus = 'Completed' and NOT EXISTS(select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggedUserID AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE 1 = 2 END
                    //Union
                    //    SELECT distinct SyncTaskID as SyncDBID, UserID, Type, TypeID, WFLevel, WFLevelStatus, OutcomeName TaskName, '' as Description, 
                    //   TMT.TaskSubmittedDate as CreatedDate,P.ProjectName,TMT.TaskDate,PER.PeriodicityType Periodicity, PER.PeriodicityId,PS.BeneficiaryTypeID,PS.WorkflowID,TMT.TaskSubmittedDate, TMT.TaskUpdateComment,U.Name as TaskSubmittedBy
                    //   FROM TbMyTask TMT
                    //   inner join TbOutcomes Outcome on TMT.TypeID = Outcome.pk_OutcomeID and Type = 'Outcome'
                    //   inner join TbProject P on Outcome.ProjectId = P.ProjectId
                    //   inner join TbPeriodicity PER ON PER.PeriodicityId = Outcome.Periodicity
                    //   inner join TbProjectSurvey PS on PS.SurveyID = Outcome.SurveyID
                    //   left join TbOutcomeTrackDate PSTD on PSTD.OutcomeId = Outcome.pk_OutcomeID and PSTD.TrackDate = TMT.TaskDate
                    //   inner join TbUser U on U.ID = TMT.UserID
                    //   where WFLevelStatus = 'Completed' and NOT EXISTS(select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggedUserID and P.WSID = @WorkspaceID
                    //   AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE 1 = 2 END
                    // Union
                    //   SELECT distinct SyncTaskID as SyncDBID, UserID, Type, TypeID, WFLevel, WFLevelStatus, RiskName TaskName, Risk.Description as Description, TMT.TaskSubmittedDate as CreatedDate,P.ProjectName,TMT.TaskDate,PER.PeriodicityType Periodicity, PER.PeriodicityId,0 as BeneficiaryTypeID,Risk.WorkflowID,TMT.TaskSubmittedDate, TMT.TaskUpdateComment,U.Name as TaskSubmittedBy
                    //   FROM TbMyTask TMT inner join TbRisk Risk on TMT.TypeID = Risk.ID and Type = 'RISK'
                    //   inner join TbProject P on Risk.ProjectId = P.ProjectId
                    //   inner join TbPeriodicity PER ON PER.PeriodicityId = Risk.RiskPeriodicity
                    //   left join TbKPITrackDate PSTD on PSTD.KPIID = Risk.ID and PSTD.TrackDate = TMT.TaskDate
                    //   inner join TbUser U on U.ID = TMT.UserID
                    //   where WFLevelStatus = 'Completed' and NOT EXISTS(select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggedUserID and P.WSID = @WorkspaceID AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE 1 = 2 END";

                    strQuery = @"SELECT distinct SyncTaskID as SyncDBID,UserID,Type,TypeID,WFLevel,WFLevelStatus, SurveyName TaskName, SurveyDescription as Description, TMT.TaskSubmittedDate as CreatedDate,P.ProjectName,TMT.TaskDate,PER.PeriodicityType Periodicity,PER.PeriodicityId,TPA.BeneficiaryTypeID,TPA.WorkflowID,TMT.TaskSubmittedDate, TMT.TaskUpdateComment,U.Name as TaskSubmittedBy FROM TbMyTask TMT
                                inner join TbProjectSurvey TPA on TMT.TypeID = TPA.SurveyID and Type = 'Survey' 
                                inner join TbProject P on TPA.ProjectId = P.ProjectId
                                inner join TbPeriodicity PER ON PER.PeriodicityId = TPA.ReportingFrequency
                                left join TbProjectSurveyTrackDate PSTD on PSTD.SurveyId = TPA.SurveyID and PSTD.TrackDate = TMT.TaskDate
                                inner join TbUser U on U.ID = TMT.UserID
                                where WFLevelStatus = 'Completed' and NOT EXISTS (select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID =  @loggedUserID and P.WSID = @WorkspaceID and TMT.IsActive=1 AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1=1 ELSE 1=2 END";

                    lstMyCompletedTask = _connection.Query<CompletedTask>(strQuery, new { loggedUserID = UserID, WorkspaceID = WorkspaceID, SyncServerID = SyncServerID, LastSyncDate = LastSyncDatetime }, commandTimeout: 86400).ToList();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return lstMyCompletedTask;

        }
        public IEnumerable<TaskAuditLog> GetTaskAuditLog(int UserID, string LastSyncDatetime)
        {
            if (string.IsNullOrEmpty(LastSyncDatetime))
            {
                LastSyncDatetime = "";
            }
            List<TaskAuditLog> lstUsers = new List<TaskAuditLog>();
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //UNION

                    //                SELECT  TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
                    //                        TAL.TaskTypeID TypeID, IFNULL(TAL.Command, '') AS TaskUpdateComment,
                    //                          IFNULL(TAL.Document, """") AttachedDocumentName,
                    //                IFNULL(USR1.ProfileImage, '') AS ProfileImage,
                    //                             IFNULL(Doc.UniqueDocumentName, '') as UniqueDocumentName,
                    //                            IFNULL(Doc.DocumentName, '') as DocumentName,
                    //                            IFNULL(Doc.DocumentTypeIcon, '') as DocumentTypeIcon,
                    //                            IFNULL(TAL.TaskSyncDbID, 0) as TaskSyncDbID

                    //                    FROM TbUser user INNER JOIN TbOutcomeAssignUsers outuser
                    //                    on user.ID = outuser.UserID   INNER JOIN TbTaskAuditLog TAL

                    //                    on TAL.TaskTypeID = outuser.OutcomeId LEFT JOIN TbUser USR1

                    //                    ON TAL.CreatedBy = USR1.ID left join TbDocument Doc on TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
                    //                    where user.ID = @AssignedTo AND IFNULL(outuser.IsDeleted,0) = 0

                    //                    and TAL.TaskType = 'Outcome'  AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE TAL.CreatedDate > STR_TO_DATE(@LastSyncDate, '%Y-%m-%d %H:%i:%s') END


                    //    UNION

                    //select  TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
                    //        TAL.TaskTypeID TypeID, IFNULL(TAL.Command, '') AS TaskUpdateComment,
                    //          IFNULL(TAL.Document, """") AttachedDocumentName,
                    //                      IFNULL(USR1.ProfileImage, '') AS ProfileImage,
                    //                             IFNULL(Doc.UniqueDocumentName, '') as UniqueDocumentName,
                    //                            IFNULL(Doc.DocumentName, '') as DocumentName,
                    //                            IFNULL(Doc.DocumentTypeIcon, '') as DocumentTypeIcon,
                    //                            IFNULL(TAL.TaskSyncDbID, 0) as TaskSyncDbID
                    //                     from TbUser usr JOIN TbProjectWorkflowDetails wrkflw  on wrkflw.UserID = usr.ID
                    //                     JOIN TbOutcomes prjOutcome ON prjOutcome.WorkflowId = wrkflw.WorkflowID
                    //                     JOIN TbTaskAuditLog TAL on TAL.TaskTypeID = prjOutcome.pk_OutcomeID
                    //                     LEFT JOIN TbUser USR1  ON TAL.CreatedBy = USR1.ID
                    //                     left join TbDocument Doc on TAL.TaskSyncDbID = Doc.TaskSyncID   and Doc.TaskSyncID > 0
                    //                     WHERE usr.ID = @AssignedTo  and TAL.TaskType = 'Outcome'
                    //                     AND IFNULL(wrkflw.IsDeleted,0) = 0

                    //                     AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE TAL.CreatedDate > STR_TO_DATE(@LastSyncDate, '%Y-%m-%d %H:%i:%s') END


                    //UNION

                    //SELECT  TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
                    //            TAL.TaskTypeID TypeID, IFNULL(TAL.Command, '') AS TaskUpdateComment,
                    //              IFNULL(TAL.Document, """") AttachedDocumentName,
                    //                IFNULL(USR1.ProfileImage, '') AS ProfileImage,
                    //                             IFNULL(Doc.UniqueDocumentName, '') as UniqueDocumentName,
                    //                            IFNULL(Doc.DocumentName, '') as DocumentName,
                    //                            IFNULL(Doc.DocumentTypeIcon, '') as DocumentTypeIcon,
                    //                            IFNULL(TAL.TaskSyncDbID, 0) as TaskSyncDbID

                    //                    FROM TbUser user INNER JOIN TbProjectActivityToUsers actuser
                    //                    on user.ID = actuser.UserId INNER JOIN TbTaskAuditLog TAL

                    //                    on TAL.TaskTypeID = actuser.ActivityId LEFT JOIN TbUser USR1

                    //                    ON TAL.CreatedBy = USR1.ID left join TbDocument Doc on TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
                    //                    where user.ID = @AssignedTo AND IFNULL(actuser.IsDeleted,0) = 0

                    //                    and TAL.TaskType = 'Activity'  AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE TAL.CreatedDate > STR_TO_DATE(@LastSyncDate, '%Y-%m-%d %H:%i:%s') END


                    //UNION

                    //select  TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
                    //        TAL.TaskTypeID TypeID, IFNULL(TAL.Command, '') AS TaskUpdateComment,
                    //          IFNULL(TAL.Document, """") AttachedDocumentName,
                    //                  IFNULL(USR1.ProfileImage, '') AS ProfileImage,
                    //                         IFNULL(Doc.UniqueDocumentName, '') as UniqueDocumentName,
                    //                        IFNULL(Doc.DocumentName, '') as DocumentName,
                    //                        IFNULL(Doc.DocumentTypeIcon, '') as DocumentTypeIcon,
                    //                        IFNULL(TAL.TaskSyncDbID, 0) as TaskSyncDbID
                    //                 from TbUser usr JOIN TbProjectWorkflowDetails wrkflw  on wrkflw.UserID = usr.ID
                    //                 JOIN TbProjectActivity prjAct ON prjAct.WorkflowId = wrkflw.WorkflowID
                    //                 JOIN TbTaskAuditLog TAL on TAL.TaskTypeID = prjAct.ActivityId
                    //                 LEFT JOIN TbUser USR1  ON TAL.CreatedBy = USR1.ID
                    //                 left join TbDocument Doc on TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
                    //                 WHERE usr.ID = @AssignedTo  and TAL.TaskType = 'Activity'
                    //                 AND IFNULL(wrkflw.IsDeleted,0) = 0

                    //                 AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE TAL.CreatedDate > STR_TO_DATE(@LastSyncDate, '%Y-%m-%d %H:%i:%s') END

                    //UNION

                    //SELECT  TAL.CreatedBy ,user.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
                    //        TAL.TaskTypeID TypeID, IFNULL(TAL.Command, '') AS TaskUpdateComment, IFNULL(TAL.Document, """") AttachedDocumentName,
                    //                IFNULL(user.ProfileImage, '') AS ProfileImage,
                    //                             IFNULL(Doc.UniqueDocumentName, '') as UniqueDocumentName,
                    //                            IFNULL(Doc.DocumentName, '') as DocumentName,
                    //                            IFNULL(Doc.DocumentTypeIcon, '') as DocumentTypeIcon,
                    //                            IFNULL(TAL.TaskSyncDbID, 0) as TaskSyncDbID

                    //                    FROM TbTaskAuditLog TAL
                    //                 INNER JOIN TbUser user
                    //                    ON  user.ID = TAL.CreatedBy
                    //                    left join TbDocument Doc on TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0

                    //                    INNER JOIN(SELECT DISTINCT TBPRJ.ProjectId
                    //                                    FROM TbProject TBPRJ

                    //                                    INNER JOIN TbInitiative TBINT  ON TBPRJ.InitiativeId = TBINT.InitiativeId

                    //                                    INNER JOIN TbInitiativeWorkflow FLW ON FLW.InitiativeId = TBINT.InitiativeId

                    //                                    INNER JOIN TbInitiativeWorkflowLevelDetails FLWDETAIL ON FLWDETAIL.WorkflowID = FLW.WorkflowID

                    //                                    WHERE UserID = @AssignedTo
                    //                                ) PRJ ON TAL.TaskTypeID = PRJ.ProjectId

                    //                    WHERE TAL.TaskType = 'Project'
                    //                    AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1 = 1 ELSE TAL.CreatedDate > STR_TO_DATE(@LastSyncDate, '%Y-%m-%d %H:%i:%s') END


                    strQuery = @"SELECT CreatedBy,Name,Message , CreatedDate, Type , TypeID, TaskUpdateComment , AttachedDocumentName, ProfileImage ,DATE_FORMAT(CreatedDate, '%Y-%m-%d %T') CreatedDateStr,UniqueDocumentName,DocumentName,DocumentTypeIcon,TaskSyncDbID
		                        FROM (
		                                SELECT	TAL.CreatedBy,user.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType `Type`,
				                                TAL.TaskTypeID TypeID,IFNULL(TAL.Command,'') AS TaskUpdateComment,
				                                IFNULL(TAL.Document, """") AttachedDocumentName,
				                                IFNULL(user.ProfileImage,'') AS ProfileImage,
                                                IFNULL(Doc.UniqueDocumentName,'') as UniqueDocumentName,
                                                IFNULL(Doc.DocumentName,'') as DocumentName,
                                                IFNULL(Doc.DocumentTypeIcon,'') as DocumentTypeIcon, 
                                                IFNULL(TAL.TaskSyncDbID,0) as TaskSyncDbID      
		                                FROM	TbTaskAuditLog TAL
		                                INNER JOIN	TbUser user
			                                ON	user.ID =TAL.AssignedBy
                                        left join TbDocument Doc on     TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
		                                WHERE	TAL.AssignedTo = @AssignedTo
			                                AND	TAL.TaskType != 'Project'
			                                AND TAL.TaskType != 'Outcome'
                                            AND TAL.TaskType != 'Survey'
                                            AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1=1 ELSE  TAL.CreatedDate > STR_TO_DATE(@LastSyncDate,'%Y-%m-%d %H:%i:%s') END

		                             UNION
		
		                                SELECT	TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
				                                TAL.TaskTypeID TypeID,IFNULL(TAL.Command,'') AS TaskUpdateComment,
				                                IFNULL(TAL.Document, """") AttachedDocumentName,
				                                IFNULL(USR1.ProfileImage,'') AS ProfileImage,
                                                IFNULL(Doc.UniqueDocumentName,'') as UniqueDocumentName,
                                                IFNULL(Doc.DocumentName,'') as DocumentName,
                                                IFNULL(Doc.DocumentTypeIcon,'') as DocumentTypeIcon,
                                                TAL.TaskSyncDbID
		                                FROM TbUser user INNER JOIN TbProjectSurveyUser seruser 
			                            on user.ID = seruser.UserID   INNER JOIN TbTaskAuditLog TAL 
			                            on TAL.TaskTypeID = seruser.SurveyID LEFT JOIN TbUser USR1 
			                            ON TAL.CreatedBy = USR1.ID left join TbDocument Doc on     TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
                                        where  user.ID = @AssignedTo AND  IFNULL(seruser.IsDeleted,0) = 0
		                                and TAL.TaskType = 'Survey'  AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1=1 ELSE  TAL.CreatedDate > STR_TO_DATE(@LastSyncDate,'%Y-%m-%d %H:%i:%s') END
                                        
                                        UNION

                                    select 	TAL.CreatedBy,USR1.Name,TAL.Message,TAL.CreatedDate,TAL.TaskType Type,
		                                    TAL.TaskTypeID TypeID,IFNULL(TAL.Command,'') AS TaskUpdateComment,
		                                    IFNULL(TAL.Document, """") AttachedDocumentName,
		                                    IFNULL(USR1.ProfileImage,'') AS ProfileImage,
                                            IFNULL(Doc.UniqueDocumentName,'') as UniqueDocumentName,
                                            IFNULL(Doc.DocumentName,'') as DocumentName,
                                            IFNULL(Doc.DocumentTypeIcon,'') as DocumentTypeIcon,
                                            TAL.TaskSyncDbID 
                                     from TbUser usr JOIN TbProjectWorkflowDetails wrkflw on wrkflw.UserID = usr.ID
                                     JOIN TbProjectSurvey prjSurvey ON prjSurvey.WorkflowID = wrkflw.WorkflowID
                                     JOIN TbTaskAuditLog TAL on TAL.TaskTypeID = prjSurvey.SurveyID 
                                     LEFT JOIN TbUser USR1 ON TAL.CreatedBy = USR1.ID
                                     left join TbDocument Doc on     TAL.TaskSyncDbID = Doc.TaskSyncID and Doc.TaskSyncID > 0
                                     WHERE  usr.ID =  @AssignedTo  and TAL.TaskType = 'Survey' AND  IFNULL(wrkflw.IsDeleted,0) = 0 
		                             AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1=1 ELSE  TAL.CreatedDate > STR_TO_DATE(@LastSyncDate,'%Y-%m-%d %H:%i:%s') END
                                  ) A  ORDER BY A.CreatedDate asc;";

                    lstUsers = _connection.Query<TaskAuditLog>(strQuery, new { AssignedTo = UserID, LastSyncDate = LastSyncDatetime }, commandTimeout: 86400).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstUsers;
        }
        public int SyncToServer_Survey(int WFLevel, bool IsBeneficiaryLinked, string objSynData, string Periodicity, int loggedUserId, string clientGuid)
        {
            string strQuery = string.Empty;
            //var id = 1;
            var IsSubmitted = 0;
            var syncDBId = 0;
            int varSurveyId = 0;
            string varWorkflowLevel = "";

            List<EmailSurveyList> lstEmailSurveyList = new List<EmailSurveyList>();

            using (_connection = Utils.Database.GetDBConnection())
            {
                _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    var cmd = _connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.Connection = _connection;

                    MySqlParameter pWFLevel = new MySqlParameter("@WFLevel", MySqlDbType.Int32);
                    MySqlParameter pClientSynData = new MySqlParameter("@ClientData", MySqlDbType.String);
                    MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                    MySqlParameter psyncDBId = new MySqlParameter("@syncDBId", MySqlDbType.Int32);

                    MySqlParameter pType = new MySqlParameter("@Type", MySqlDbType.String);
                    MySqlParameter pCommand = new MySqlParameter("@Command", MySqlDbType.String);
                    MySqlParameter pTypeIDColumn = new MySqlParameter("@TypeIDColumn", MySqlDbType.String);
                    MySqlParameter pTypeID = new MySqlParameter("@TypeID", MySqlDbType.Int32);
                    MySqlParameter pStatus = new MySqlParameter("@Status", MySqlDbType.String);
                    MySqlParameter pWorkflowID = new MySqlParameter("@WorkflowID", MySqlDbType.Int32);
                    MySqlParameter pTotalWFLevel = new MySqlParameter("@TotalWFLevel", MySqlDbType.Int32);
                    MySqlParameter pDataCollectedBy = new MySqlParameter("@DataCollectedBy", MySqlDbType.Int32);
                    MySqlParameter pIsActive = new MySqlParameter("@IsActive", MySqlDbType.Int32);

                    MySqlParameter pWFLevelStatus = new MySqlParameter("@WFLevelStatus", MySqlDbType.String);
                    MySqlParameter pTaskUpdateComment = new MySqlParameter("@TaskUpdateComment", MySqlDbType.String);
                    MySqlParameter pAttachedDocumentName = new MySqlParameter("@AttachedDocumentName", MySqlDbType.String);
                    MySqlParameter pTaskDate = new MySqlParameter("@TaskDate", MySqlDbType.Date);
                    MySqlParameter pTaskSubmittedDate = new MySqlParameter("@TaskSubmittedDate", MySqlDbType.DateTime);

                    MySqlParameter pResponseID = new MySqlParameter("@ResponseID", MySqlDbType.Int32);
                    MySqlParameter pAnswer = new MySqlParameter("@Answer", MySqlDbType.String);
                    MySqlParameter pQuestionID = new MySqlParameter("@QuestionID", MySqlDbType.Int32);
                    MySqlParameter pSurveyID = new MySqlParameter("@SurveyID", MySqlDbType.Int32);
                    MySqlParameter pSectionID = new MySqlParameter("@SectionID", MySqlDbType.Int32);
                    MySqlParameter pResponseSyncID = new MySqlParameter("@ResponseSyncID", MySqlDbType.Int32);
                    MySqlParameter pSubmittedDataStatus = new MySqlParameter("@SubmittedDataStatus", MySqlDbType.String);
                    MySqlParameter pWorkspaceID = new MySqlParameter("@WorkspaceID", MySqlDbType.Int32);
                    MySqlParameter parentSyncDBID = new MySqlParameter("@parentSyncDBID", MySqlDbType.Int32);
                    MySqlParameter datacollSyncDBID = new MySqlParameter("@datacollSyncDBID", MySqlDbType.Int32);
                    MySqlParameter ID = new MySqlParameter("@ID", MySqlDbType.Int32);
                    MySqlParameter ClientGuid = new MySqlParameter("@ClientGuid", MySqlDbType.String);
                    MySqlParameter pSubmissionCount = new MySqlParameter("@SubmissionCount", MySqlDbType.Int32);

                    pWFLevel.Value = WFLevel;
                    pType.Value = "Survey";
                    pCommand.Value = "TaskResponse";
                    pClientSynData.Value = objSynData;
                    ploggedUserId.Value = loggedUserId;
                    ClientGuid.Value = clientGuid;

                    cmd.Parameters.Add(pWFLevel);
                    cmd.Parameters.Add(pType);
                    cmd.Parameters.Add(pCommand);
                    cmd.Parameters.Add(pClientSynData);
                    cmd.Parameters.Add(ploggedUserId);
                    //cmd.Parameters.Add(ploggedUserId);
                    cmd.Parameters.Add(ClientGuid);

                    try
                    {
                        List<MyTask> lstTaskData = JsonConvert.DeserializeObject<List<MyTask>>(objSynData);
                        if (lstTaskData != null)
                        {
                            for (var i = 0; i < lstTaskData.Count; i++)
                            {
                                var Item = lstTaskData[i];

                                varSurveyId = Item.TypeID;
                                varWorkflowLevel = Item.Status;

                                pTypeIDColumn.Value = Item.TypeIDColumn;
                                pTypeID.Value = Item.TypeID;
                                pStatus.Value = Item.Status;
                                pWorkflowID.Value = Item.WorkflowID;
                                pTotalWFLevel.Value = Item.TotalWFLevel;
                                pDataCollectedBy.Value = Item.DataCollectedBy == -1 ? loggedUserId : Item.DataCollectedBy;
                                pWFLevelStatus.Value = Item.WFLevelStatus;
                                pIsActive.Value = Item.WFLevelStatus == "Rejected" ? 0 : 1;
                                pTaskUpdateComment.Value = Item.TaskUpdateComment;
                                pAttachedDocumentName.Value = Item.AttachedDocumentName;
                                pTaskDate.Value = Item.TaskDate;
                                pTaskSubmittedDate.Value = Item.TaskSubmittedDate;
                                pResponseSyncID.Value = Item.ResponseSyncID;
                                pWorkspaceID.Value = Item.WorkspaceID;
                                parentSyncDBID.Value = Item.SyncDBID;
                                if (WFLevel == 0)
                                {
                                    datacollSyncDBID.Value = 0;
                                }
                                else
                                {
                                    datacollSyncDBID.Value = GetDataCollectedSyncDBID(Item.SyncDBID);
                                }

                                cmd.Parameters.Add(pTypeIDColumn);
                                cmd.Parameters.Add(pTypeID);
                                cmd.Parameters.Add(pStatus);
                                cmd.Parameters.Add(pWorkflowID);
                                cmd.Parameters.Add(pTotalWFLevel);
                                cmd.Parameters.Add(pDataCollectedBy);
                                cmd.Parameters.Add(pWFLevelStatus);
                                cmd.Parameters.Add(pIsActive);
                                cmd.Parameters.Add(pTaskUpdateComment);
                                cmd.Parameters.Add(pAttachedDocumentName);
                                cmd.Parameters.Add(pTaskDate);
                                cmd.Parameters.Add(pTaskSubmittedDate);
                                cmd.Parameters.Add(pResponseSyncID);
                                cmd.Parameters.Add(pWorkspaceID);
                                cmd.Parameters.Add(parentSyncDBID);
                                cmd.Parameters.Add(datacollSyncDBID);
                                cmd.Parameters.Add(ID);

                                strQuery = @"SELECT EXISTS(SELECT T.ID from TaskSyncDB T Inner Join TbMyTask MT On T.ID = MT.SyncTaskID WHERE T.IsActive =1 And IsDeleted=0 And " +
                                            " MT.IsActive =1 And Status = @TaskStatus And T.DataCollectedBy = @DataCollector And MT.Type = @TaskType And MT.TypeID = @TypeId  " +
                                            " And MT.TaskDate = @TaskDate And UserID =@UserID And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed');";
                                var IsExists = _connection.Query<int>(strQuery, new
                                {
                                    CurrWFLevel = WFLevel,
                                    DataCollector = Item.DataCollectedBy == -1 ? loggedUserId : Item.DataCollectedBy,
                                    TaskStatus = Item.Status,
                                    TaskType = Item.Type,
                                    TypeId = Item.TypeID,
                                    TaskDate = Item.TaskDate,
                                    UserID = loggedUserId,
                                }).Single();
                                if (IsExists == 0 || (IsExists == 1 && Periodicity == "Anytime"))
                                {
                                    if (WFLevel == 0)
                                    {
                                        strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive =1 And IsDeleted=0 And
                                                         T.IsActive =1 And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And MT.TaskDate = @TaskDate
                                                         And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectSurveyUser Where IsDeleted =0 And SurveyID = @TypeId) );";
                                        IsSubmitted = _connection.Query<int>(strQuery, new
                                        {
                                            CurrWFLevel = Item.WFLevel,
                                            WFID = Item.WorkflowID,
                                            TaskType = Item.Type,
                                            TypeId = Item.TypeID,
                                            TaskDate = Item.TaskDate
                                        }).Single();
                                    }

                                    if (WFLevel > 0)
                                    {
                                        if (Periodicity == "Anytime")
                                        {
                                            strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive =1 And IsDeleted=0 And
                                                         T.IsActive =1 And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate
                                                         And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectWorkflowDetails Where IsDeleted =0 And WorkflowID = @WFID)
                                                         AND MT.ParentSyncDBID = @ParentSyncDBID );";
                                        }
                                        else
                                        {
                                            strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive =1 And IsDeleted=0 And
                                                         T.IsActive =1 And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate
                                                         And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectWorkflowDetails Where IsDeleted =0 And WorkflowID = @WFID) );";
                                        }

                                        IsSubmitted = _connection.Query<int>(strQuery, new
                                        {
                                            CurrWFLevel = Item.WFLevel,
                                            DataCollector = Item.DataCollectedBy,
                                            WFID = Item.WorkflowID,
                                            TaskType = Item.Type,
                                            TypeId = Item.TypeID,
                                            TaskDate = Item.TaskDate,
                                            ParentSyncDBID = Item.SyncDBID
                                        }).Single();
                                    }
                                    pSubmittedDataStatus.Value = (IsSubmitted == 1 && WFLevel == 0) ? "Saved" : "Submitted";
                                    cmd.Parameters.Add(pSubmittedDataStatus);

                                    if (IsSubmitted == 0 || (IsSubmitted == 1 && WFLevel == 0))
                                    {

                                        strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,Status, WorkflowID,TotalWFLevel, DataCollectedBy,ClientData,CreatedDate, CreatedBy ,IsActive,SubmittedDataStatus,WorkspaceID,ClientGuid)
                                                    VALUES(@Type,@Command, @TypeIDColumn, @TypeID, @Status, @WorkflowID, @TotalWFLevel, @DataCollectedBy,@ClientData,@TaskSubmittedDate, @loggedUserId,@IsActive,@SubmittedDataStatus,@WorkspaceID,@ClientGuid); SELECT LAST_INSERT_ID();";

                                        cmd.CommandText = strQuery;
                                        syncDBId = Convert.ToInt32(cmd.ExecuteScalar());
                                        psyncDBId.Value = syncDBId;
                                        cmd.Parameters.Add(psyncDBId);

                                        strQuery = @"INSERT INTO TbMyTask(SyncTaskID,UserID,Type, TypeID, WFLevel, WFLevelStatus,TaskDate,TaskSubmittedDate, TaskUpdateComment, AttachedDocumentName, IsActive, IsDeleted, CreatedDate, CreatedBy,ParentSyncDBID,DataCollectionSyncDBID)
                                             VALUES(@syncDBId,@loggedUserId,@Type,@TypeID,@WFLevel,@WFLevelStatus,@TaskDate,@TaskSubmittedDate,@TaskUpdateComment,@AttachedDocumentName,@IsActive,0,now(),@loggedUserId,@parentSyncDBID,@datacollSyncDBID); SELECT LAST_INSERT_ID();";
                                        cmd.CommandText = strQuery;
                                        var taskId = Convert.ToInt32(cmd.ExecuteScalar());

                                        if (Item.WFLevelStatus == "Rejected")
                                        {
                                            //strQuery = "UPDATE TaskSyncDB SET IsActive =0 Where Type=@Type And TypeID =@TypeID And DataCollectedBy =@DataCollectedBy And Command =@Command ";

                                            //cmd.CommandText = strQuery;
                                            //cmd.ExecuteNonQuery();
                                            //strQuery = @"UPDATE TbMyTask SET IsActive =0 ,UpdatedDate  = now() ,  UpdatedBy = @loggedUserId Where Type=@Type And TypeID =@TypeID And SyncTaskID In 
                                            //        ( Select ID FRom TaskSyncDB Where  Type=@Type And TypeID =@TypeID And DataCollectedBy =@DataCollectedBy And Command =@Command And IsActive =0)";

                                            //cmd.CommandText = strQuery;
                                            //cmd.ExecuteNonQuery();

                                            strQuery = "UPDATE TbProjectSurveyResponses SET IsActive =0 ,UpdatedDate  = now() ,  UpdatedBy = @loggedUserId Where SyncTaskID =@ResponseSyncID";

                                            cmd.CommandText = strQuery;
                                            cmd.ExecuteNonQuery();
                                            // Changing Status in TbMyTask
                                            List<int> lstSyncTaskID = new List<int>();
                                            strQuery = "Select SyncTaskID from TbMyTask where DataCollectionSyncDBID = @SyncDBID";
                                            lstSyncTaskID = _connection.Query<int>(strQuery, new
                                            {
                                                SyncDBID = datacollSyncDBID.Value
                                            }).ToList();
                                            lstSyncTaskID.Add(Convert.ToInt32(datacollSyncDBID.Value));

                                            for (var Count = 0; Count <= lstSyncTaskID.Count - 1; Count++)
                                            {
                                                ID.Value = lstSyncTaskID[Count];
                                                strQuery = @"Update TbMyTask Set IsActive = 0 where SyncTaskID = @ID";
                                                cmd.CommandText = strQuery;
                                                cmd.ExecuteNonQuery();

                                                strQuery = @"UPDATE TaskSyncDB SET IsActive =0 Where ID = @ID";
                                                cmd.CommandText = strQuery;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        // TaskProgress(Item.Type, Item.TypeID, Periodicity, Item.WFLevel, Item.TotalWFLevel, Item.FinanceWorkflowId, Item.WorkspaceID, Item.SurveyAnswers);
                                        SendTaskNotification(loggedUserId, Item.WorkflowID, WFLevel, Item.Type, Item.TypeID, Item.WFLevelStatus, Item.TypeIDColumn, 0);

                                        var emailTemplateName = string.Empty;
                                        var userRole = string.Empty;

                                        if(Item.Status != null)
                                        {
                                            if (Item.Status.ToLower() == "datacollected")
                                            {
                                                emailTemplateName = "TaskSubmission";
                                                //userRole = "Data Collector";
                                            }
                                            else if (Item.Status.ToLower() == "dataapproved")
                                            {
                                                emailTemplateName = "TaskApproved";
                                                //userRole = "Data Approver";
                                            }
                                            else if (Item.Status.ToLower() == "rejected")
                                            {
                                                emailTemplateName = "TaskRejection";
                                                //userRole = "Data Approver";
                                            }
                                        }

                                        EmailSurveyList objEmailSurveyList = new EmailSurveyList();
                                        objEmailSurveyList.SurveyID = Item.TypeID;
                                        objEmailSurveyList.EmailTemplateName = emailTemplateName;
                                        lstEmailSurveyList.Add(objEmailSurveyList);

                                        if (WFLevel == 0)
                                        {
                                            int[] benefID = new int[1];
                                            if (IsBeneficiaryLinked)
                                            {
                                                ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
                                                List<ProjectBeneficiary> lstBeneficiary = JsonConvert.DeserializeObject<List<ProjectBeneficiary>>(Item.NewBeneficiaries);
                                                benefID = new int[lstBeneficiary.Count];
                                                if (lstBeneficiary != null)
                                                {

                                                    for (var j = 0; j < lstBeneficiary.Count; j++)
                                                    {
                                                        //strQuery = @"SELECT EXISTS(SELECT BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryName =@BeneficiaryName and IsDeleted=0 );";
                                                        ////strQuery = @"SELECT IFNULL(BeneficiaryID,0) as BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryName =@BeneficiaryName and IsDeleted=0";
                                                        //var Id = _connection.Query<int>(strQuery, new
                                                        //{
                                                        //    BeneficiaryName = lstBeneficiary[j].BeneficiaryName,
                                                        //    BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID
                                                        //}).Single();
                                                        //if (Id == 0)
                                                        //{
                                                        //    benefID[j] = objPrjDAL.CreateBeneficiary(lstBeneficiary[j].BeneficiaryName, lstBeneficiary[j].BeneficiaryTypeID, lstBeneficiary[j].BeneficiaryQstnAnswers, loggedUserId);
                                                        //}
                                                        //else
                                                        //{
                                                        //    strQuery = @"SELECT IFNULL(BeneficiaryID,0) as BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryName =@BeneficiaryName and IsDeleted=0";
                                                        //    var BefId = _connection.Query<int>(strQuery, new
                                                        //    {
                                                        //        BeneficiaryName = lstBeneficiary[j].BeneficiaryName,
                                                        //        BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID
                                                        //    }).Single();
                                                        //    benefID[j] = BefId; //Id means benefciaryID
                                                        //}

                                                        strQuery = @"SELECT IFNULL(BeneficiaryID,0) as BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryID =@BeneficiaryID and IsDeleted=0";
                                                        var Id = _connection.Query<int>(strQuery, new
                                                        {
                                                            BeneficiaryID = lstBeneficiary[j].BeneficiaryID,
                                                            BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID
                                                        }).FirstOrDefault();
                                                        if (Id > 0)
                                                        {
                                                            benefID[j] = Id; //Id means benefciaryID
                                                        }
                                                        else
                                                        {

                                                            var Query = "Select BeneficiaryID from TbProjectBeneficiary  WHERE BeneficiaryTypeID=@BeneficiaryTypeID And TokenValue =@TokenValue and IsDeleted=0";
                                                            var BenefID = _connection.Query<int>(Query, new
                                                            {
                                                                BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID,
                                                                TokenValue = lstBeneficiary[j].TokenValue,
                                                            }).FirstOrDefault();

                                                            if (BenefID > 0)
                                                            {
                                                                benefID[j] = BenefID;
                                                            }
                                                            else
                                                            {
                                                                benefID[j] = objPrjDAL.CreateBeneficiary(lstBeneficiary[j].BeneficiaryName, lstBeneficiary[j].BeneficiaryTypeID, lstBeneficiary[j].BeneficiaryQstnAnswers, loggedUserId, lstBeneficiary[j].TokenValue);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            List<SurveyResponses> lstSurveyAnswers = JsonConvert.DeserializeObject<List<SurveyResponses>>(Item.SurveyAnswers);

                                            if (lstSurveyAnswers != null)
                                            {
                                                List<SurveyResponseGPSLocation> lstSurveyResponseGPSLocation = new List<SurveyResponseGPSLocation>();

                                                for (var k = 0; k < lstSurveyAnswers.Count; k++)
                                                {
                                                    if (lstSurveyAnswers[k].QuestionType == "GPS Capture")
                                                    {
                                                        SurveyResponseGPSLocation objSurveyResponseGPSLocation = new SurveyResponseGPSLocation();
                                                        objSurveyResponseGPSLocation.SurveyID = lstSurveyAnswers[k].SurveyID;
                                                        objSurveyResponseGPSLocation.SubmissionCount = (lstSurveyAnswers[k].BeneficiaryInd == 0 ? 1 : lstSurveyAnswers[k].BeneficiaryInd);
                                                        objSurveyResponseGPSLocation.GPS = lstSurveyAnswers[k].Answer;
                                                        objSurveyResponseGPSLocation.SyncTaskID = syncDBId;//lstSurveyAnswers[k].ResponseID <= -1 ? benefID[((lstSurveyAnswers[k].ResponseID * -1) - 1)] : lstSurveyAnswers[k].ResponseID;
                                                        lstSurveyResponseGPSLocation.Add(objSurveyResponseGPSLocation);
                                                    }

                                                    pResponseID.Value = lstSurveyAnswers[k].ResponseID <= -1 ? benefID[((lstSurveyAnswers[k].ResponseID * -1) - 1)] : lstSurveyAnswers[k].ResponseID;
                                                    pAnswer.Value = lstSurveyAnswers[k].Answer;
                                                    pQuestionID.Value = lstSurveyAnswers[k].QuestionID;
                                                    pSurveyID.Value = lstSurveyAnswers[k].SurveyID;
                                                    pSectionID.Value = lstSurveyAnswers[k].SectionID;
                                                    pSubmissionCount.Value = lstSurveyAnswers[k].BeneficiaryInd == 0 ? 1 : lstSurveyAnswers[k].BeneficiaryInd;
                                                    pTaskSubmittedDate.Value = lstSurveyAnswers[k].TaskSubmittedDate;

                                                    cmd.Parameters.Add(pResponseID);
                                                    cmd.Parameters.Add(pAnswer);
                                                    cmd.Parameters.Add(pQuestionID);
                                                    cmd.Parameters.Add(pSurveyID);
                                                    cmd.Parameters.Add(pSectionID);
                                                    cmd.Parameters.Add(pSubmissionCount);

                                                    strQuery = @"INSERT INTO TbProjectSurveyResponses(SyncTaskID,ResponseID,Answer,QuestionID,SurveyID,SectionID,CreatedDate,CreatedBy,IsActive,SubmissionCount)
                                                                 VALUES(@syncDBId,@ResponseID,@Answer,@QuestionID,@SurveyID,@SectionID,@TaskSubmittedDate,@loggedUserId,1,@SubmissionCount);";
                                                    cmd.CommandText = strQuery;
                                                    cmd.ExecuteScalar();

                                                    cmd.Parameters.Remove(pResponseID);
                                                    cmd.Parameters.Remove(pAnswer);
                                                    cmd.Parameters.Remove(pQuestionID);
                                                    cmd.Parameters.Remove(pSurveyID);
                                                    cmd.Parameters.Remove(pSectionID);
                                                    cmd.Parameters.Remove(pSubmissionCount);
                                                }

                                                if (lstSurveyResponseGPSLocation != null && lstSurveyResponseGPSLocation.Count > 0)
                                                {
                                                    InsertResposeGPSLocation(lstSurveyResponseGPSLocation);
                                                }

                                              
                                            }
                                        }
                                    }
                                    TaskProgress(Item.Type, Item.TypeID, Periodicity, Item.WFLevel, Item.TotalWFLevel, Item.FinanceWorkflowId, Item.WorkspaceID, Item.SurveyAnswers);
                                    InsertIntoAuditLog(Item.WorkflowID, Item.TypeID, Item.TypeIDColumn, Item.Type, Item.FinanceWorkflowId, WFLevel, loggedUserId, Item.TaskUpdateComment, Item.AttachedDocumentName, Item.WFLevelStatus, syncDBId);

                                }

                                cmd.Parameters.Remove(pType);
                                cmd.Parameters.Remove(pCommand);
                                cmd.Parameters.Remove(pTypeIDColumn);
                                cmd.Parameters.Remove(pTypeID);
                                cmd.Parameters.Remove(pStatus);
                                cmd.Parameters.Remove(pWorkflowID);
                                cmd.Parameters.Remove(pTotalWFLevel);
                                cmd.Parameters.Remove(pDataCollectedBy);
                                cmd.Parameters.Remove(pWFLevelStatus);
                                cmd.Parameters.Remove(pTaskUpdateComment);
                                cmd.Parameters.Remove(pAttachedDocumentName);
                                cmd.Parameters.Remove(pTaskDate);
                                cmd.Parameters.Remove(pTaskSubmittedDate);
                                cmd.Parameters.Remove(pTaskSubmittedDate);
                                cmd.Parameters.Remove(pResponseSyncID);

                            }
                        }
                        transaction.Commit();

                    }
                    catch (Exception e)
                    {
                        lstEmailSurveyList = null;

                        try
                        {
                            transaction.Rollback();
                        }
                        catch (MySqlException ex)
                        {
                            if (transaction.Connection != null)
                            {
                                // id = 0;
                            }
                            throw ex;
                        }
                        throw e;

                    }
                    finally
                    {
                        _connection.Close();
                    }
                }

                if(lstEmailSurveyList != null && lstEmailSurveyList.Count >0)
                {
                    foreach(var itm in lstEmailSurveyList)
                    {
                        try
                        {
                            EmailDAL emailDAL = new EmailDAL();
                            EmailActivity objEmailActivity = new EmailActivity();
                            SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
                            objSurveyEmailTemplate = emailDAL.GetTemplateDetails(itm.SurveyID, loggedUserId);
                           
                            if(objSurveyEmailTemplate != null &&
                                objSurveyEmailTemplate.SubmittedtoEmail != null &&
                                objSurveyEmailTemplate.SubmittedtoEmail.Length >0)
                            {
                                bool mailStatus = objEmailActivity.SendMail(itm.EmailTemplateName, objSurveyEmailTemplate, loggedUserId);
                            }
                        } 
                        catch(Exception ex)
                        {
                        }
                    }
                }

            }
            return syncDBId;

        }

        public int SyncToServer(string Type, int WFLevel, bool IsBeneficiaryLinked, string objSynData, string Periodicity, int loggedUserId)
        {
            string strQuery = string.Empty;
            //var id = 0;
            var isApproved = 0;
            var syncDBId = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    List<MyTask> lstTaskData = JsonConvert.DeserializeObject<List<MyTask>>(objSynData);

                    if (lstTaskData != null)
                    {

                        for (var i = 0; i < lstTaskData.Count; i++)
                        {
                            var Item = lstTaskData[i];

                            strQuery = @"SELECT EXISTS(SELECT T.ID from TaskSyncDB T Inner Join TbMyTask MT On T.ID = MT.SyncTaskID WHERE IsDeleted=0 And " +
                                        " Status = @TaskStatus And T.DataCollectedBy = @DataCollector And MT.Type = @TaskType And MT.TypeID = @TypeId  " +
                                        " And MT.TaskDate = @TaskDate And UserID =@UserID And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' AND T.IsActive = 1 AND MT.IsActive = 1);";
                            var IsSubmitted = _connection.Query<int>(strQuery, new
                            {
                                CurrWFLevel = Item.WFLevel,
                                DataCollector = Item.DataCollectedBy == -1 ? loggedUserId : Item.DataCollectedBy,
                                TaskStatus = Item.Status,
                                TaskType = Item.Type,
                                TypeId = Item.TypeID,
                                TaskDate = Item.TaskDate,
                                UserID = loggedUserId,
                            }).Single();

                            if (IsSubmitted == 0 || (IsSubmitted == 1 && Periodicity == "Anytime") || (IsSubmitted == 1 && Periodicity == "Issue"))
                            {
                                if (WFLevel > 0)
                                {
                                    //strQuery = "SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE IsDeleted=0 And " +
                                    //                " WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate" +
                                    //                " And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectWorkflowDetails Where IsDeleted =0 And WorkflowID = @WFID) );";
                                    //isApproved = _connection.Query<int>(strQuery, new { CurrWFLevel = Item.WFLevel, DataCollector = Item.DataCollectedBy, WFID = Item.WorkflowID, TaskType = Item.Type, TypeId = Item.TypeID, TaskDate = Item.TaskDate }).Single();

                                    strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive = 1 And IsDeleted = 0 And T.IsActive = 1 And WFLevel = @CurrWFLevel  And WFLevelStatus = 'Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY(SELECT UserID From TbProjectWorkflowDetails Where IsDeleted = 0 And WorkflowID = @WFID));";

                                    isApproved = _connection.Query<int>(strQuery, new
                                    {
                                        CurrWFLevel = Item.WFLevel,
                                        DataCollector = Item.DataCollectedBy,
                                        WFID = Item.WorkflowID,
                                        TaskType = Item.Type,
                                        TypeId = Item.TypeID,
                                        TaskDate = Item.TaskDate
                                    }).Single();

                                }

                                if (isApproved == 0 && Item.Command != "ADD")
                                {
                                    int ParentSyncDBID = 0;
                                    int datacollSyncDBID = 0;
                                    int datacollSyncDBID1 = 0;

                                    if (Item.Type == "Issue")
                                    {
                                        ParentSyncDBID = Item.SyncDBID;
                                    }
                                    var IsActive = 1;
                                    if (Item.WFLevelStatus == "Rejected")
                                    {
                                        IsActive = 0;
                                    }
                                    strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,Status, WorkflowID,FinanceWorkflowID,TotalWFLevel, DataCollectedBy,ClientData,CreatedDate, CreatedBy,ParentSyncDBID,IsActive ,WorkspaceID)
                                                VALUES(@Type, @Command, @TypeIDColumn, @TypeID, @Status, @WorkflowID,@FinanceWorkflowID, @TotalWFLevel, @DataCollectedBy,@ClientData,now(), @CreatedBy,@ParentSyncDBID,@IsActive,@WorkspaceID); SELECT LAST_INSERT_ID();";
                                    syncDBId = _connection.Query<int>(strQuery, new
                                    {
                                        Type = Item.Type,
                                        Command = "TaskResponse",
                                        TypeIDColumn = Item.TypeIDColumn,
                                        TypeID = Item.TypeID,
                                        Status = Item.Status,
                                        WorkflowID = Item.WorkflowID,
                                        FinanceWorkflowID = Item.FinanceWorkflowId,
                                        TotalWFLevel = Item.TotalWFLevel,
                                        DataCollectedBy = Item.DataCollectedBy == -1 ? loggedUserId : Item.DataCollectedBy,
                                        ClientData = objSynData,
                                        CreatedBy = loggedUserId,
                                        ParentSyncDBID = ParentSyncDBID,
                                        IsActive = IsActive,
                                        WorkspaceID = Item.WorkspaceID
                                    }).Single();

                                    //strQuery = "sp_UpdateMyTaskList";
                                    //id = _connection.ExecuteScalar<int>(strQuery, new { TaskResponseList = objSynData.ClientData, loggedUserId = loggedUserId }, commandType: CommandType.StoredProcedure);

                                    var PaymentRejectWFLevel = Item.WFLevel;
                                    //var IsActive = 1;
                                    if (Item.WFLevelStatus == "Rejected" && Item.Type == "Payment")
                                    {
                                        PaymentRejectWFLevel = 0;
                                    }

                                    if (WFLevel == 0)
                                    {
                                        datacollSyncDBID1 = 0;
                                    }
                                    else
                                    {
                                        datacollSyncDBID1 = GetDataCollectedSyncDBID(Item.SyncDBID);
                                    }

                                    strQuery = @"INSERT INTO TbMyTask(SyncTaskID,UserID,Type, TypeID, WFLevel, WFLevelStatus,TaskDate,TaskSubmittedDate, TaskUpdateComment, AttachedDocumentName, IsDeleted, CreatedDate, CreatedBy, RiskReportingValue,IsActive,ParentSyncDBID,DataCollectionSyncDBID)
                                            VALUES(@syncDBId,@UserID,@Type,@TypeID,@WFLevel,@WFLevelStatus,@TaskDate,@TaskSubmittedDate,@TaskUpdateComment,@AttachedDocumentName,0,now(),@CreatedBy,@RiskReportingValue, @IsActive,@parentSyncDBID,@datacollSyncDBID); SELECT LAST_INSERT_ID();";
                                    var taskId = _connection.Query<int>(strQuery, new
                                    {
                                        syncDBId = syncDBId,
                                        UserID = loggedUserId,
                                        Type = Item.Type,
                                        TypeID = Item.TypeID,
                                        WFLevel = PaymentRejectWFLevel, //Item.WFLevel, //
                                        WFLevelStatus = Item.WFLevelStatus,
                                        TaskUpdateComment = Item.TaskUpdateComment,
                                        AttachedDocumentName = Item.AttachedDocumentName,
                                        CreatedBy = loggedUserId,
                                        TaskDate = Item.TaskDate,
                                        TaskSubmittedDate = Item.TaskSubmittedDate,
                                        RiskReportingValue = Item.ReportingValue,
                                        IsActive = IsActive,
                                        parentSyncDBID = Item.SyncDBID,
                                        datacollSyncDBID = datacollSyncDBID1
                                    }).Single();

                                    //TaskProgress(Item.Type, Item.TypeID, Periodicity, Item.WFLevel, Item.TotalWFLevel, Item.FinanceWorkflowId, Item.WorkspaceID, Item.SurveyAnswers);
                                    SendTaskNotification(loggedUserId, Item.WorkflowID, WFLevel, Item.Type, Item.TypeID, Item.WFLevelStatus, Item.TypeIDColumn, ParentSyncDBID);

                                    // Updating Last Updated value for Risk
                                    UpdateRiskLastUpdateValue(Item.WFLevel, Item.TotalWFLevel, Item.ReportingValue, Item.Type, Item.TypeID, loggedUserId);

                                    //Reject Task
                                    if (Item.Type == "Issue" && Item.WFLevelStatus == "Rejected")
                                    {
                                        RejectIssueTask(Item.Type, Item.TypeID, ParentSyncDBID, loggedUserId);
                                    }
                                    else
                                    {
                                        RejectTask(Item.Type, Item.TypeID, loggedUserId, Item.DataCollectedBy, Item.WFLevelStatus, Item.TaskDate);
                                    }

                                    // Updating Requested Paymnet Status to Approve or reject
                                    UpdateRequestPaymnet(Item.TypeID, Item.Status, Item.Type);

                                    // Updatting donor commited fund as Approve or reject
                                    UpdateFundApprovalOrReject(Item.TypeID, Item.WFLevelStatus, Item.Type, Item.REQUESTID, Item.SyncDBID, Item.CommittedFund);


                                    if (WFLevel == 0)
                                    {
                                        //var benefID = 0;
                                        int[] benefID = new int[1];

                                        if (IsBeneficiaryLinked)
                                        {
                                            ProjectBeneficiaryDAL objPrjDAL = new ProjectBeneficiaryDAL();
                                            List<ProjectBeneficiary> lstBeneficiary = JsonConvert.DeserializeObject<List<ProjectBeneficiary>>(Item.NewBeneficiaries);
                                            benefID = new int[lstBeneficiary.Count];
                                            if (lstBeneficiary != null)
                                            {

                                                for (var j = 0; j < lstBeneficiary.Count; j++)
                                                {
                                                    strQuery = @"SELECT EXISTS(SELECT BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryName =@BeneficiaryName and IsDeleted=0 );";
                                                    var Id = _connection.Query<int>(strQuery, new
                                                    {
                                                        BeneficiaryName = lstBeneficiary[j].BeneficiaryName,
                                                        BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID
                                                    }).Single();
                                                    if (Id == 0)
                                                    {
                                                        benefID[j] = objPrjDAL.CreateBeneficiary(lstBeneficiary[j].BeneficiaryName, lstBeneficiary[j].BeneficiaryTypeID, lstBeneficiary[j].BeneficiaryQstnAnswers, loggedUserId, lstBeneficiary[j].TokenValue);
                                                    }
                                                    else
                                                    {
                                                        strQuery = @"SELECT IFNULL(BeneficiaryID,0) as BeneficiaryID from TbProjectBeneficiary WHERE BeneficiaryTypeID=@BeneficiaryTypeID And BeneficiaryName =@BeneficiaryName and IsDeleted=0";
                                                        var BefId = _connection.Query<int>(strQuery, new
                                                        {
                                                            BeneficiaryName = lstBeneficiary[j].BeneficiaryName,
                                                            BeneficiaryTypeID = lstBeneficiary[j].BeneficiaryTypeID
                                                        }).Single();
                                                        benefID[j] = BefId; //Id means benefciaryID
                                                    }

                                                }
                                            }
                                        }
                                        List<SurveyResponses> lstSurveyAnswers = JsonConvert.DeserializeObject<List<SurveyResponses>>(Item.SurveyAnswers);

                                        if (lstSurveyAnswers != null)
                                        {
                                            //foreach (SurveyResponses ansItem in lstSurveyAnswers)
                                            for (var k = 0; k < lstSurveyAnswers.Count; k++)
                                            {

                                                strQuery = @"INSERT INTO TbProjectSurveyResponses(SyncTaskID,ResponseID,Answer,QuestionID,SurveyID,SectionID,CreatedDate,CreatedBy,IsActive,SubmissionCount)
                                                VALUES(@SyncTaskID,@ResponseID,@Answer,@QuestionID,@SurveyID,@SectionID,now(),@CreatedBy,1,@SubmissionCount);";
                                                _connection.Execute(strQuery, new
                                                {
                                                    SyncTaskID = syncDBId,
                                                    ResponseID = lstSurveyAnswers[k].ResponseID <= -1 ? benefID[((lstSurveyAnswers[k].ResponseID * -1) - 1)] : lstSurveyAnswers[k].ResponseID,
                                                    Answer = lstSurveyAnswers[k].Answer,
                                                    QuestionID = lstSurveyAnswers[k].QuestionID,
                                                    SurveyID = lstSurveyAnswers[k].SurveyID,
                                                    SectionID = lstSurveyAnswers[k].SectionID,
                                                    CreatedBy = loggedUserId,
                                                    SubmissionCount = lstSurveyAnswers[k].SubmissionCount
                                                });
                                            }
                                        }
                                    }



                                }
                                if (Item.Type == "Issue" && Item.Command == "ADD")
                                {
                                    ReportAnIssue(Item.Type, Item.TypeID, loggedUserId, Item.TypeIDColumn, Item.TaskUpdateComment, Item.AttachedDocumentName, Item.TaskDate, Item.WorkspaceID);
                                    SendTaskNotification(loggedUserId, Item.WorkflowID, WFLevel, Item.Type, Item.TypeID, Item.WFLevelStatus, Item.TypeIDColumn, 0);
                                }
                                TaskProgress(Item.Type, Item.TypeID, Periodicity, Item.WFLevel, Item.TotalWFLevel, Item.FinanceWorkflowId, Item.WorkspaceID, Item.SurveyAnswers);
                                InsertIntoAuditLog(Item.WorkflowID, Item.TypeID, Item.TypeIDColumn, Item.Type, Item.FinanceWorkflowId, WFLevel, loggedUserId, Item.TaskUpdateComment, Item.AttachedDocumentName, Item.WFLevelStatus, syncDBId);

                            }

                        }
                    }
                }

                //id = 1;

            }

            catch (Exception ex)
            {
                //retMsg = "Error while saving data." + ex.Message;
                //id = 0;
                throw ex;
            }
            return syncDBId;

        }
        public void TaskProgress(string Type, int TypeID, String Periodicity, int WFLevel, int TotalWFLevel, int FinanceWorkflowId, int WorkspaceID, string SurveyAnswers)
        {

            Double Progress = 0;
            Double NoOfDays = 0;
            Double taskcount = 0;
            Double ActivityPercentag = 0;
            Double Weightage = 0;
            int ProjectId = 0;
            int SyncTaskIdVar = 0;
            int ParentActivityIdVar = 0;
            var LogID = 0;
            string strQeury = string.Empty;
            string strQeuryN = string.Empty;

            try
            {
                if (WFLevel == TotalWFLevel)
                {
                    strQeuryN = @"SELECT SyncTaskID FROM TbMyTask where TypeID = @TypeID and Type= @Type and IsActive=1";
                    SyncTaskIdVar = _connection.Query<int>(strQeuryN, new
                    {
                        Type = Type,
                        TypeID = TypeID,
                    }).FirstOrDefault();

                    strQeury = @"SELECT count(distinct TaskDate) FROM TbMyTask where TypeID = @TypeID and Type= @Type and IsActive=1 and WFLevel=@TotalWFLevel";
                    taskcount = _connection.Query<int>(strQeury, new
                    {
                        Type = Type,
                        TypeID = TypeID,
                        TotalWFLevel = TotalWFLevel,
                    }).Single();

                    if (Type == "Activity")
                    {
                        List<ProjectActivity> objProjectActivityList = new List<ProjectActivity>();
                        List<TaskProgressQuestions> objTaskProgress = new List<TaskProgressQuestions>();
                        string query = string.Empty;


                        using (_connection = Utils.Database.GetDBConnection())
                        {

                            query = @"SELECT ActivityType,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget,ParentActivityId,WeightagePercentage
                         ,BeneficiaryTypeQuestionID
                        from TbProjectActivity ProjectActivity  where ProjectActivity.IsDeleted = @isdeleted and ActivityId = @TypeID";
                            objProjectActivityList = _connection.Query<ProjectActivity>(query, new { isdeleted = 0, TypeID = TypeID }).ToList();
                            int n;
                            bool isNumeric = int.TryParse(objProjectActivityList[0].SurveyTarget, out n);

                            if (objProjectActivityList[0].ActivityType == "Quantitative" && isNumeric == true)
                            {
                                double TaskAnswer = 0;
                                var QuestionId = 0;
                                var surveyOrBeneficiary = "Survey";

                                double SurveyTarget = Convert.ToDouble(objProjectActivityList[0].SurveyTarget);
                                if (objProjectActivityList[0].SurveyQuestionId != 0)
                                {
                                    QuestionId = objProjectActivityList[0].SurveyQuestionId;

                                    surveyOrBeneficiary = "Survey";
                                }
                                else
                                {
                                    QuestionId = objProjectActivityList[0].BeneficiaryTypeQuestionID;
                                    surveyOrBeneficiary = "Beneficiary";

                                }


                                if (objProjectActivityList[0].SurveyLogic == 1)//Count of Unique Reponses
                                {
                                    var QuestionSum = "";
                                    if (surveyOrBeneficiary == "Survey")
                                    {
                                        strQeury = @"SELECT count(Answer) FROM TbProjectSurveyResponses WHERE QuestionId = @QuestionId and SyncTaskID = @SyncTaskIdVar";
                                        QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, SyncTaskIdVar = SyncTaskIdVar }).Single();
                                    }
                                    else
                                    {
                                        strQeury = @"SELECT count(Answer) FROM TbBeneficiaryRegistrationDetails WHERE QuestionID = @QuestionId and IsDeleted = @isdeleted";
                                        QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, isdeleted = 0 }).Single();
                                    }

                                    if ((Convert.ToDouble(QuestionSum)) > 0 && (Convert.ToDouble(SurveyTarget)) > 0)
                                    {

                                        Progress = ((Convert.ToDouble(QuestionSum)) / (Convert.ToDouble(SurveyTarget))) * 100;
                                    }
                                }
                                else if (objProjectActivityList[0].SurveyLogic == 2)//Cumulative  Sum
                                {
                                    var QuestionSum = "";

                                    if (surveyOrBeneficiary == "Survey")
                                    {
                                        strQeury = @"SELECT sum(Answer) FROM TbProjectSurveyResponses WHERE QuestionId = @QuestionId and SyncTaskID = @SyncTaskIdVar";
                                        QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, SyncTaskIdVar = SyncTaskIdVar }).Single();
                                    }
                                    else
                                    {
                                        strQeury = @"SELECT sum(Answer) FROM TbBeneficiaryRegistrationDetails WHERE QuestionID = @QuestionId and IsDeleted = @isdeleted";
                                        QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, isdeleted = 0 }).Single();
                                    }


                                    if ((Convert.ToDouble(QuestionSum)) > 0 && (Convert.ToDouble(SurveyTarget)) > 0)
                                    {

                                        Progress = ((Convert.ToDouble(QuestionSum)) / (Convert.ToDouble(SurveyTarget))) * 100;
                                    }
                                }
                                else
                                {
                                    if (objProjectActivityList[0].SurveyQuestionId != 0)
                                    {

                                        List<TaskProgress> lstSurveyAnswers = JsonConvert.DeserializeObject<List<TaskProgress>>(SurveyAnswers);

                                        foreach (TaskProgress inputObj in lstSurveyAnswers)
                                        {
                                            if (inputObj != null && inputObj.Questions != null)
                                            {
                                                foreach (TaskProgressQuestions Ques in inputObj.Questions)
                                                {
                                                    if (QuestionId == Ques.QuestionID)
                                                    {
                                                        int num;
                                                        bool Numeric = int.TryParse(Ques.Answers, out num);
                                                        if (Numeric == true)
                                                        {
                                                            TaskAnswer += Convert.ToDouble(Ques.Answers);
                                                            SurveyTarget += SurveyTarget;
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {


                                        query = @"SELECT QuestionID,Answer Answers
                                          from TbBeneficiaryRegistrationDetails  where IsDeleted = @isdeleted and QuestionID = @QuestionID";
                                        objTaskProgress = _connection.Query<TaskProgressQuestions>(query, new { isdeleted = 0, QuestionID = QuestionId }).ToList();
                                        double dcTarget = 0;
                                        foreach (TaskProgressQuestions Ques in objTaskProgress)
                                        {
                                            if (QuestionId == Ques.QuestionID)
                                            {
                                                int num;
                                                bool Numeric = int.TryParse(Ques.Answers, out num);
                                                if (Numeric == true)
                                                {
                                                    TaskAnswer += Convert.ToDouble(Ques.Answers);
                                                    dcTarget += Convert.ToDouble(SurveyTarget);
                                                }

                                            }
                                        }
                                        SurveyTarget = dcTarget;
                                    }

                                    if ((Convert.ToDouble(TaskAnswer)) > 0 && Convert.ToDouble(SurveyTarget) > 0)
                                    {
                                        Progress = Math.Round(((Convert.ToDouble(TaskAnswer)) / Convert.ToDouble(SurveyTarget) * 100));
                                    }
                                }
                            }

                            else
                            {
                                strQeury = @"SELECT count(distinct TrackDate) FROM TbProjectActivityTrackDate where ActivityId = @TypeID";
                                NoOfDays = _connection.Query<int>(strQeury, new
                                {
                                    TypeID = TypeID,
                                }).Single();

                                if (Periodicity == "Anytime")
                                {
                                    strQeury = @"SELECT EndDate FROM TbProjectActivity WHERE ActivityId = @actid";
                                    DateTime EndDate = _connection.Query<DateTime>(strQeury, new { actid = TypeID }).Single();
                                    if (EndDate < DateTime.Now)
                                    {
                                        Progress = 100;
                                    }
                                }
                                else
                                {
                                    if (taskcount > 0 && NoOfDays > 0)
                                    {
                                        Progress = Math.Round((taskcount / NoOfDays) * 100);
                                    }
                                }

                            }
                            strQeury = @"SELECT ProjectId FROM TbProjectActivity WHERE ActivityId = @TypeID";
                            ProjectId = _connection.Query<int>(strQeury, new { TypeID = TypeID }).Single();

                            if (Progress > 100)
                            {
                                Progress = 100;
                            }

                            var strQuery = @"Update TbProjectActivity Set Progress = @Progress where ActivityId = @TypeID";
                            _connection.Execute(strQuery, new
                            {
                                TypeID = TypeID,
                                Progress = Progress,
                            });

                            ParentActivityIdVar = objProjectActivityList[0].ParentActivityId;
                            Weightage = objProjectActivityList[0].WeightagePercentage;
                            List<ProjectActivity> objProjectChildActivityList = new List<ProjectActivity>();
                            do
                            {
                                if (ParentActivityIdVar > 0)
                                {
                                    // Progress Calculation 
                                    // Progress = (weightage * TaskProgress)/(weightage + TaskProgress)
                                    query = @"SELECT ActivityType,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget,ParentActivityId,WeightagePercentage,Progress
                         
                        from TbProjectActivity ProjectActivity  where ProjectActivity.IsDeleted = @isdeleted and ParentActivityId = @TypeID";
                                    objProjectChildActivityList = _connection.Query<ProjectActivity>(query, new { isdeleted = 0, TypeID = ParentActivityIdVar }).ToList();
                                    var denometer = 0.0;
                                    var divider = 0.0;
                                    if (objProjectChildActivityList.Count > 0)
                                    {
                                        for (var Cnt = 0; Cnt <= objProjectChildActivityList.Count - 1; Cnt++)
                                        {
                                            denometer += (objProjectChildActivityList[Cnt].WeightagePercentage * objProjectChildActivityList[Cnt].Progress);
                                            divider += (objProjectChildActivityList[Cnt].WeightagePercentage);

                                        }
                                        ActivityPercentag = (denometer) / (divider);
                                    }

                                    var strQueryT = @"Update TbProjectActivity Set Progress = @ActivityPercentag where ActivityId = @ParentActivityIdVar";
                                    _connection.Execute(strQueryT, new
                                    {
                                        ParentActivityIdVar = ParentActivityIdVar,
                                        ActivityPercentag = ActivityPercentag,
                                    });

                                    var strQueryL = @"SELECT ParentActivityId FROM TbProjectActivity where ActivityId = @TypeID";
                                    ParentActivityIdVar = _connection.Query<int>(strQueryL, new { TypeID = ParentActivityIdVar }).Single();
                                }
                            }
                            while (ParentActivityIdVar > 0);


                            //var strQueryL = @"SELECT ParentActivityId FROM TbProjectActivity where ActivityId = @TypeID";
                            // ParentActivityIdVar = _connection.Query<int>(strQueryL, new { TypeID = TypeID }).Single();
                            //if (ParentActivityIdVar > 0)
                            //{
                            //    var strQueryP = @"SELECT IFNULL(sum(Progress)/(count(ActivityId)*100)*100,0) FROM TbProjectActivity where ParentActivityId = @ParentActivityIdVar";
                            //    ActivityPercentag = _connection.Query<double>(strQueryP, new { ParentActivityIdVar = ParentActivityIdVar }).Single();

                            //    var strQueryT = @"Update TbProjectActivity Set Progress = @ActivityPercentag where ActivityId = @ParentActivityIdVar";
                            //    _connection.Execute(strQueryT, new
                            //    {
                            //        ParentActivityIdVar = ParentActivityIdVar,
                            //        ActivityPercentag = ActivityPercentag,
                            //    });

                            //}



                        }
                    }

                    if (Type == "Outcome")
                    {

                        List<ProjectOutcome> objProjectOutcome = new List<ProjectOutcome>();
                        string query = string.Empty;

                        using (_connection = Utils.Database.GetDBConnection())
                        {

                            query = @"SELECT DatacollectionType,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget
                         
                        from TbOutcomes where IsDeleted = @isdeleted and pk_OutcomeID = @TypeID";
                            objProjectOutcome = _connection.Query<ProjectOutcome>(query, new { isdeleted = 0, TypeID = TypeID }).ToList();
                            int n;
                            bool isNumeric = int.TryParse(objProjectOutcome[0].SurveyTarget, out n);

                            if (objProjectOutcome[0].DatacollectionType == "Quantitative" && isNumeric == true)
                            {
                                var TaskAnswer = "";
                                var QuestionId = objProjectOutcome[0].SurveyQuestionId;
                                var SurveyTarget = objProjectOutcome[0].SurveyTarget;
                                List<TaskProgress> lstSurveyAnswers = JsonConvert.DeserializeObject<List<TaskProgress>>(SurveyAnswers);

                                foreach (TaskProgress inputObj in lstSurveyAnswers)
                                {
                                    foreach (TaskProgressQuestions Ques in inputObj.Questions)
                                    {
                                        if (QuestionId == Ques.QuestionID)
                                        {
                                            TaskAnswer = Ques.Answers;
                                            break;
                                        }
                                    }
                                }
                                if (objProjectOutcome[0].SurveyLogic == 1)//Count of Unique Reponses
                                {


                                    strQeury = @"SELECT count(Answer) FROM TbProjectSurveyResponses WHERE QuestionId = @QuestionId and SyncTaskID = @SyncTaskIdVar";
                                    var QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, SyncTaskIdVar = SyncTaskIdVar }).Single();

                                    if ((Convert.ToDouble(QuestionSum)) > 0 && (Convert.ToDouble(SurveyTarget)) > 0)
                                    {

                                        Progress = ((Convert.ToDouble(QuestionSum)) / (Convert.ToDouble(SurveyTarget))) * 100;
                                    }
                                }
                                else if (objProjectOutcome[0].SurveyLogic == 2)//Cumulative  Sum
                                {


                                    strQeury = @"SELECT sum(Answer) FROM TbProjectSurveyResponses WHERE QuestionId = @QuestionId and SyncTaskID = @SyncTaskIdVar";
                                    var QuestionSum = _connection.Query<string>(strQeury, new { QuestionId = QuestionId, SyncTaskIdVar = SyncTaskIdVar }).Single();

                                    if ((Convert.ToDouble(QuestionSum)) > 0 && (Convert.ToDouble(SurveyTarget)) > 0)
                                    {

                                        Progress = ((Convert.ToDouble(QuestionSum)) / (Convert.ToDouble(SurveyTarget))) * 100;
                                    }
                                }
                                else
                                {

                                    if ((Convert.ToDouble(TaskAnswer)) > 0 && Convert.ToDouble(objProjectOutcome[0].SurveyTarget) > 0)
                                    {
                                        Progress = Math.Round(((Convert.ToDouble(TaskAnswer)) / Convert.ToDouble(objProjectOutcome[0].SurveyTarget) * 100));
                                    }
                                }
                            }

                            else
                            {
                                strQeury = @"SELECT count(distinct TrackDate) FROM TbOutcomeTrackDate where OutcomeId = @TypeID";
                                NoOfDays = _connection.Query<int>(strQeury, new
                                {
                                    TypeID = TypeID,
                                }).Single();

                                if (Periodicity == "Anytime")
                                {
                                    strQeury = @"SELECT EndDate FROM TbOutcomes WHERE pk_OutcomeID = @OutcomeID";
                                    DateTime EndDate = _connection.Query<DateTime>(strQeury, new { OutcomeID = TypeID }).Single();
                                    if (EndDate < DateTime.Now)
                                    {
                                        Progress = 100;
                                    }
                                }
                                else
                                {
                                    if (taskcount > 0 && NoOfDays > 0)
                                    {
                                        Progress = Math.Round((taskcount / NoOfDays) * 100);
                                    }
                                }

                            }
                            strQeury = @"SELECT ProjectId FROM TbOutcomes WHERE pk_OutcomeID = @TypeID";
                            ProjectId = _connection.Query<int>(strQeury, new { TypeID = TypeID }).Single();

                            if (Progress > 100)
                            {
                                Progress = 100;
                            }

                            var strQuery = @"Update TbOutcomes Set Progress = @Progress where pk_OutcomeID = @TypeID";
                            _connection.Execute(strQuery, new
                            {
                                TypeID = TypeID,
                                Progress = Progress,
                            });





                        }

                    }
                    if (Type == "Survey")
                    {

                        strQeury = @"SELECT ProjectId FROM TbProjectSurvey WHERE SurveyID = @TypeID";
                        ProjectId = _connection.Query<int>(strQeury, new { TypeID = TypeID }).Single();

                        strQeury = @"SELECT EXISTS(select SurveyId 
                                    from (
                                        select SurveyId,ProjectId from TbProjectActivity
                                        union all
                                        select SurveyId,ProjectId from TbOutcomes
                                    ) a
                                    where SurveyId=@TypeID and ProjectId=@ProjectId);";
                        LogID = _connection.Query<int>(strQeury, new
                        {
                            ProjectId = ProjectId,
                            TypeID = TypeID,
                        }).Single();
                        if (LogID == 0)
                        {
                            strQeury = @"SELECT count(distinct TrackDate) FROM TbProjectSurveyTrackDate where SurveyID = @TypeID";
                            NoOfDays = _connection.Query<int>(strQeury, new
                            {

                                TypeID = TypeID,
                            }).Single();

                            if (Periodicity == "Anytime")
                            {
                                strQeury = @"SELECT EndDate FROM TbProjectSurvey WHERE SurveyID = @SurveyID";
                                DateTime EndDate = _connection.Query<DateTime>(strQeury, new { SurveyID = TypeID }).Single();
                                if (EndDate < DateTime.Now)
                                {
                                    Progress = 100;
                                }
                            }
                            else
                            {
                                if (taskcount > 0 && NoOfDays > 0)
                                {
                                    Progress = Math.Round((taskcount / NoOfDays) * 100);
                                }
                            }
                            var strQuery = @"Update TbProjectSurvey Set Progress = @Progress where SurveyID = @TypeID";
                            _connection.Execute(strQuery, new
                            {
                                TypeID = TypeID,
                                Progress = Progress,

                            });
                        }
                    }
                    //GetProjectPercentage(ProjectId, LogID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendTaskNotification(int logUserID, int WorkflowID, int WFLevel, string Type, int TypeID, string WFLevelStatus, string TypeIdColumn, int ParentSyncDBID)
        {
            string strQuery = string.Empty;
            dynamic obj;
            var title = "";
            var sub = "";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (Type != "Payment Request" && Type != "Fund Request" && Type != "Issue" && Type != "Payment")
                    {
                        strQuery = @"SELECT UserID From TbProjectWorkflowDetails Where WorkflowID = @WorkflowID And WfLevel = @WFLevel and IsDeleted =0";
                        obj = _connection.Query<object>(strQuery, new { WorkflowID = WorkflowID, WFLevel = WFLevel + 1 }).ToList();

                        strQuery = "Select Name  From TbUser Where  ID =@logUserID ";
                        var name = _connection.Query<string>(strQuery, new { logUserID = logUserID }).Single();
                        switch (Type)
                        {
                            case "Activity":
                                strQuery = "Select ActivityName TaskName From TbProjectActivity Where  ActivityId =@TypeID ";
                                break;
                            case "Survey":
                                strQuery = "Select SurveyName TaskName From TbProjectSurvey Where  SurveyID =@TypeID ";
                                break;
                            case "KPI":
                                strQuery = "Select KPIName TaskName From TbKPI Where  ID =@TypeID ";
                                break;
                            case "Outcome":
                                strQuery = "Select OutcomeName TaskName From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                                break;
                            case "RISK":
                                strQuery = "Select RiskName TaskName From TbRisk Where  ID =@TypeID ";
                                break;
                            case "Project":
                                strQuery = "Select ProjectName TaskName From TbProject Where  ProjectId =@TypeID ";
                                break;
                            case "Issue":
                                if (TypeIdColumn == "ActivityId")
                                {
                                    strQuery = "Select ActivityName TaskName From TbProjectActivity Where  ActivityId =@TypeID ";
                                }
                                else if (TypeIdColumn == "KPIID")
                                {
                                    strQuery = "Select KPIName TaskName From TbKPI Where  ID =@TypeID ";
                                }
                                else if (TypeIdColumn == "pk_OutcomeID")
                                {
                                    strQuery = "Select OutcomeName TaskName From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                                }
                                else if (TypeIdColumn == "RISKID")
                                {
                                    strQuery = "Select RiskName TaskName From TbRisk Where  ID  =@TypeID ";
                                }
                                break;
                        }
                        var taskname = _connection.Query<string>(strQuery, new { TypeID = TypeID }).Single();


                        if (WFLevel == 0)
                        {
                            title = name + " data collected.";
                            sub = name + " data collected  for the " + Type + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        if (WFLevel > 0 && WFLevelStatus != "Rejected")
                        {
                            title = name + " data approved.";
                            sub = name + " data approved  for the " + Type + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        if (WFLevel > 0 && WFLevelStatus == "Rejected")
                        {
                            title = name + " data rejected.";
                            sub = name + " data rejected  for the " + Type + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        foreach (dynamic item in obj)
                        {
                            Notification objNotification = new Notification();
                            objNotification.UserId = item.UserID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }

                    }
                    else if (Type == "Issue")
                    {
                        string taskQuery = string.Empty;
                        string TypeofTask = string.Empty;
                        string qry = "SELECT usr.ID UserID FROM TbUser usr JOIN TbWorkspace wrk on wrk.CreatedBy = usr.ID ";
                        qry += " JOIN TbInitiative tbint ON tbint.WorkspaceID = wrk.WorkspaceID ";
                        qry += " JOIN TbProject prj ON prj.InitiativeId = tbint.InitiativeId ";

                        switch (TypeIdColumn)
                        {
                            case "ActivityId":
                                qry += "JOIN TbProjectActivity ACT on ACT.ProjectId = prj.ProjectId where ACT.ActivityId =@TypeId;";
                                taskQuery = "Select ActivityName TaskName From TbProjectActivity Where  ActivityId =@TypeID ";
                                TypeofTask = "Activity";
                                break;
                            case "SurveyID":
                                qry += "JOIN TbProjectSurvey srv on srv.ProjectId = prj.ProjectId where srv.SurveyID =@TypeId;";
                                taskQuery = "Select SurveyName TaskName From TbProjectSurvey Where  SurveyID =@TypeID ";
                                TypeofTask = "Survey";
                                break;
                            case "KPIID":
                                qry += "JOIN TbKPI KPI on KPI.ProjectID = prj.ProjectId where KPI.ID =@TypeId;";
                                taskQuery = "Select KPIName TaskName From TbKPI Where  ID =@TypeID ";
                                TypeofTask = "KPI";
                                break;
                            case "pk_OutcomeID":
                                qry += "JOIN TbOutcomes Outcome on Outcome.ProjectId = prj.ProjectId where Outcome.pk_OutcomeID =@TypeId;";
                                taskQuery = "Select OutcomeName TaskName From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                                TypeofTask = "Outcome";
                                break;
                            case "RISKID":
                                qry += "JOIN TbRisk RSK on RSK.ProjectID = prj.ProjectId where RSK.ID =@TypeId;";
                                taskQuery = "Select RiskName TaskName From TbRisk Where  ID =@TypeID ";
                                TypeofTask = "RISK";
                                break;
                        }

                        obj = _connection.Query<object>(qry, new { TypeId = TypeID }).ToList();

                        strQuery = "Select Name  From TbUser Where  ID =@logUserID ";
                        var name = _connection.Query<string>(strQuery, new { logUserID = logUserID }).Single();
                        var taskname = _connection.Query<string>(taskQuery, new { TypeID = TypeID }).Single();

                        if (ParentSyncDBID > 0)
                        {
                            obj = _connection.Query<object>("SELECT UserID FROM TbMyTask WHERE SyncTaskID =@SyncTaskID", new { SyncTaskID = ParentSyncDBID }).ToList();
                        }

                        if (WFLevel == 1 && WFLevelStatus == "Completed")
                        {
                            title = name + " Reviewed";
                            sub = name + " Reviewed issue for the " + TypeofTask + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        else if (WFLevel == 0 && WFLevelStatus == "Completed")
                        {
                            title = name + " Issue reported";
                            sub = name + " Issue reported for the " + TypeofTask + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        else if (WFLevelStatus == "Rejected")
                        {
                            title = name + " Issue Rejected";
                            sub = name + " Issue Rejected  for the " + TypeofTask + " named '" + taskname + "' on " + DateTime.Now;
                        }

                        foreach (dynamic item in obj)
                        {
                            Notification objNotification = new Notification();
                            objNotification.UserId = item.UserID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void UpdateRiskLastUpdateValue(int CurrentWFLevel, int TotalWFLevel, int ReportingValue, string Type, int TypeID, int loggedUserId)
        {
            if ((CurrentWFLevel == TotalWFLevel) && (Type == "RISK"))
            {
                var strQuery = @"Update TbRisk Set LastUpdatedValue = @ReportingValue, UpdatedDate  = now() ,  UpdatedBy = @UpdatedBy where ID = @TypeID";
                _connection.Execute(strQuery, new
                {
                    ReportingValue = ReportingValue,
                    TypeID = TypeID,
                    UpdatedBy = loggedUserId,

                });
            }
        }
        public void InsertIntoAuditLog(int WorkflowID, int TypeID, string TypeIDColumn, string Type, int FinanceWorkflowId, int WFLevel, int loggedUserId, string TaskUpdateComment, string AttachedDocumentName, string WFLevelStatus, int TaskSyncDbID)
        {
            string strQuery = string.Empty;
            var AssignedTo = 0;
            var LogID = 0;
            var Message = "";
            if (Type != "Payment Request" && Type != "Fund Request" && Type != "Issue")
            {
                List<userList> lstUsers = new List<userList>();
                strQuery = @"Select UserID  From TbProjectWorkflowDetails Where WorkflowID =@WorkflowID and IsDeleted = 0;";
                lstUsers = _connection.Query<userList>(strQuery, new { WorkflowID = WorkflowID }).ToList();

                for (var j = 0; j < lstUsers.Count; j++)
                {
                    strQuery = @"SELECT EXISTS(SELECT LogID from TbTaskAuditLog WHERE AssignedTo=@AssignedTo and TaskTypeID=@TypeID and TypeIDColumn=@TypeIDColumn );";
                    LogID = _connection.Query<int>(strQuery, new
                    {
                        AssignedTo = lstUsers[j].userId,
                        TypeIDColumn = TypeIDColumn,
                        TypeID = TypeID,
                    }).Single();


                    if (LogID == 0)
                    {
                        AssignedTo = lstUsers[j].userId;
                        break;

                    }
                }

                if (AssignedTo == 0)
                {
                    AssignedTo = loggedUserId;
                }

                if (FinanceWorkflowId > 0 && Type == "Payment")
                {
                    if (WFLevelStatus != "Rejected")
                    {
                        Message = "Payment Approved";
                    }
                    else
                    {
                        Message = "Payment Rejected";
                    }
                }
                else if (WFLevel == 0)
                {
                    Message = " data collected.";

                }
                else if (WFLevel > 0)
                {
                    if (WFLevelStatus != "Rejected")
                    {
                        Message = " data approved.";
                    }
                    else { Message = " data rejected."; }

                }
            }
            else if (Type == "Payment Request")
            {
                Message = "Requested Advance Amount Approved ";
                AssignedTo = loggedUserId;
            }
            else if (Type == "Fund Request")
            {
                Message = "Funded Amount Approved ";
                AssignedTo = loggedUserId;
            }
            else if (Type == "Issue")
            {
                if (WFLevel == 1 && WFLevelStatus == "Completed")
                {
                    Message = "Reviewed an issue against the task";
                }
                else if (WFLevel == 0 && WFLevelStatus == "Completed")
                {
                    Message = "Raised an issue against the task";
                }
                else
                {
                    Message = "Task Rejected";
                }

                switch (TypeIDColumn)
                {
                    case "ActivityId":
                        strQuery = "Select ProjectId as ProjectID  From TbProjectActivity Where  ActivityId =@TypeID ";
                        break;
                    case "KPIID":
                        strQuery = "Select ProjectID as ProjectID  From TbKPI Where  ID =@TypeID ";
                        break;
                    case "pk_OutcomeID":
                        strQuery = "Select ProjectId as ProjectID  From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                        break;
                    case "RISKID":
                        strQuery = "Select ProjectID as ProjectID  From TbRisk Where  ID  =@TypeID ";
                        break;
                    case "SurveyID":
                        strQuery = "Select ProjectId as ProjectID From TbProjectSurvey Where  SurveyID  =@TypeID ";
                        break;
                }
                var ProjectID = _connection.Query<int>(strQuery, new { TypeID = TypeID }).Single();

                strQuery = "select TbWorkspace.CreatedBy From TbWorkspace TbWorkspace inner join TbInitiative TbInitiative on TbInitiative.WorkspaceID = TbWorkspace.WorkspaceID inner join TbProject TbProject on TbProject.InitiativeId = TbInitiative.InitiativeId where TbProject.ProjectId = @ProjectID";
                AssignedTo = _connection.Query<int>(strQuery, new { ProjectID = ProjectID }).Single();
            }
            strQuery = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType, TaskTypeID, TypeIDColumn, AssignedTo,AssignedBy, Message,CreatedDate, CreatedBy,Command,Document,TaskSyncDbID )
                                  VALUES(0,@Type,@TypeID,  @TypeIDColumn, @AssignedTo, @CreatedBy, @Message,now(), @CreatedBy,@TaskUpdateComment,@AttachedDocumentName, @TaskSyncDbID); SELECT LAST_INSERT_ID();";
            var ID = _connection.Query<int>(strQuery, new
            {
                Type = Type,
                TypeIDColumn = TypeIDColumn,
                TypeID = TypeID,
                AssignedTo = AssignedTo,
                Message = Message, //WFLevel == 0 ? "Data Collected " : "Data approved  ",
                CreatedBy = loggedUserId,
                TaskUpdateComment = TaskUpdateComment,
                AttachedDocumentName = AttachedDocumentName,
                TaskSyncDbID = TaskSyncDbID
            }).Single();

        }

        public void RejectTask(string Type, int TypeID, int loggedUserId, int SubmittedUser, string WFLevelStatus, DateTime Taskdate)
        {
            if (WFLevelStatus == "Rejected") //&& SubmittedUser > 0)
            {
                string Query = string.Empty;

                Query = @"Update TbMyTask set IsActive = 0 where TbMyTask.Type = @Type and TbMyTask.TypeID = @TypeID  and WFLevelStatus = 'Completed'";
                _connection.Query<int>(Query, new
                {
                    Type = Type,
                    TypeID = TypeID,
                    // SubmittedUser = SubmittedUser,
                    // Taskdate = Taskdate
                });



                Query = @"Update TaskSyncDB set IsActive = 0 where Type = @Type and TypeID = @TypeID  and Status != 'Pending'";
                _connection.Query<int>(Query, new
                {
                    Type = Type,
                    TypeID = TypeID,
                    // SubmittedUser = SubmittedUser,
                    // Taskdate = Taskdate
                });
            }
        }
        // Payment term . Paymnet request rejection
        public void UpdateRequestPaymnet(int TypeID, string WFLevelStatus, string Type)
        {
            if (Type == "Payment Request")
            {
                string Query = string.Empty;

                Query = @"update TbProjectRequestFund Set PaymentStatus = @PaymentStatus where ID = @TypeID";
                _connection.Query<int>(Query, new
                {
                    PaymentStatus = WFLevelStatus,
                    TypeID = TypeID,
                });
            }
        }

        // Donnor commit fund rejection or approval updation

        public void UpdateFundApprovalOrReject(int TypeID, string WFLevelStatus, string Type, int RequestID, int SyncDBID, double CommittedFund)
        {
            try
            {
                if (Type == "Fund Request")
                {
                    // Update SyncTask db
                    string Query = string.Empty;
                    Query = @"update TaskSyncDB Set IsActive = 0 , Status = @WFLevelStatus where ID = @SyncDBID";
                    _connection.Query<int>(Query, new
                    {
                        SyncDBID = SyncDBID,
                        WFLevelStatus = WFLevelStatus
                    });

                    // Update TbProjectBudgetAndFundDetails 
                    // if Status is rejected substract the amount added
                    if (WFLevelStatus == "Rejected")
                    {
                        Query = @"update TbProjectBudgetAndFundDetails Set DonorBudgetAmount =  (DonorBudgetAmount - @CommittedFund) where ID = @RequestID and ProjectID = @TypeID";
                        _connection.Query<int>(Query, new
                        {
                            TypeID = TypeID,
                            CommittedFund = CommittedFund,
                            RequestID = RequestID
                        });
                    }

                }
            }
            catch
            {

            }
        }
        public void ReportAnIssue(string Type, int TypeID, int loggedUserId, string TypeIDColumn, string TaskUpdateComment, string AttachedDocumentName, DateTime TaskDate, int WorkspaceID)
        {
            var strQuery = string.Empty;

            strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,Status, WorkflowID,FinanceWorkflowID,TotalWFLevel, DataCollectedBy,CreatedDate, CreatedBy,WorkspaceID )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID, @Status, @WorkflowID,@FinanceWorkflowID, @TotalWFLevel, @DataCollectedBy,now(), @CreatedBy,@WorkspaceID); SELECT LAST_INSERT_ID();";
            var syncDBId = _connection.Query<int>(strQuery, new
            {
                Type = Type,
                Command = "ADD",
                TypeIDColumn = TypeIDColumn,
                TypeID = TypeID,
                Status = "Pending",
                WorkflowID = 0,
                FinanceWorkflowID = 0,
                TotalWFLevel = 0,
                DataCollectedBy = loggedUserId,
                ClientData = "",
                CreatedBy = loggedUserId,
                WorkspaceID = WorkspaceID
            }).Single();

            //strQuery = "sp_UpdateMyTaskList";
            //id = _connection.ExecuteScalar<int>(strQuery, new { TaskResponseList = objSynData.ClientData, loggedUserId = loggedUserId }, commandType: CommandType.StoredProcedure);

            strQuery = @"INSERT INTO TbMyTask(SyncTaskID,UserID,Type, TypeID, WFLevel, WFLevelStatus,TaskDate,TaskSubmittedDate, TaskUpdateComment, AttachedDocumentName, IsActive, IsDeleted, CreatedDate, CreatedBy, RiskReportingValue)
                                VALUES(@syncDBId,@UserID,@Type,@TypeID,@WFLevel,@WFLevelStatus,@TaskDate,@TaskSubmittedDate,@TaskUpdateComment,@AttachedDocumentName,1,0,now(),@CreatedBy,@RiskReportingValue); SELECT LAST_INSERT_ID();";
            var taskId = _connection.Query<int>(strQuery, new
            {
                syncDBId = syncDBId,
                UserID = loggedUserId,
                Type = Type,
                TypeID = TypeID,
                WFLevel = 0,//Item.WFLevel,
                WFLevelStatus = "Completed",
                TaskUpdateComment = TaskUpdateComment,
                AttachedDocumentName = AttachedDocumentName,
                CreatedBy = loggedUserId,
                TaskDate = TaskDate,
                TaskSubmittedDate = TaskDate,
                RiskReportingValue = 0,

            }).Single();





        }

        public void RejectIssueTask(string Type, int TypeID, int syncDBId, int loggedUserId)
        {
            string Query = string.Empty;

            Query = @"Update TbMyTask set IsActive = 0,UpdatedDate=@UpdatedDate WHERE TbMyTask.SyncTaskID = @SyncTaskID;";
            _connection.Query<int>(Query, new
            {
                SyncTaskID = syncDBId,
                UpdatedDate = DateTime.Now,
                UpdatedBy = loggedUserId
            });

            Query = @"Update TaskSyncDB set IsActive = 0 where ID = @SyncTaskID OR IFNULL(ParentSyncDBID,0)= @SyncTaskID;";
            _connection.Query<int>(Query, new
            {
                SyncTaskID = syncDBId
            });
        }

        #region GetUpdatedTaskList
        /// <summary>
        /// GetUpdatedTaskList
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SyncServerID"></param>
        /// <param name="LastSyncDatetime"></param>
        /// <returns></returns>
        public IEnumerable<UpdatedTaskList> GetUpdatedTaskList(int UserID, int SyncServerID, string LastSyncDatetime, int WorkspaceID)
        {
            List<UpdatedTaskList> lstUpdatedTaskList = new List<UpdatedTaskList>();
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_GetMyTask_Completed_List";
                    lstUpdatedTaskList = _connection.Query<UpdatedTaskList>(strQuery, new { loggedUserID = UserID, varSyncDbID = SyncServerID, varLastSyncDatetime = LastSyncDatetime, varWorkspaceID = WorkspaceID }, commandType: CommandType.StoredProcedure, commandTimeout: 86400).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstUpdatedTaskList;
        }
        #endregion GetUpdatedTaskList

        #region GetDataCollectedSyncDBID
        /// <summary>
        /// GetDataCollectedSyncDBID
        /// </summary>
        /// <param name="CurrentSyncDBID"></param>
        /// <returns></returns>
        public int GetDataCollectedSyncDBID(int CurrentSyncDBID)
        {
            int currentvalue = CurrentSyncDBID;

            try
            {
                string strQeury = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQeury = @"SELECT IFNULL(DataCollectionSyncDBID,0) FROM TbMyTask WHERE  SyncTaskID = @CurrentSyncDBID and ifnull(IsActive,0)=1 and ifnull(IsDeleted,0)=0 limit 1;";

                    int syncdbid = _connection.Query<int>(strQeury, new { CurrentSyncDBID = CurrentSyncDBID }).SingleOrDefault();

                    if (syncdbid > 0)
                    {
                        CurrentSyncDBID = syncdbid;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CurrentSyncDBID;
        }
        #endregion GetDataCollectedSyncDBID

        #region IsClientGuidExists
        /// <summary>
        /// IsClientGuidExists
        /// </summary>
        /// <param name="ClientGuid"></param>
        /// <returns></returns>
        public bool IsClientGuidExists(string ClientGuid)
        {
            bool isExists = false;
            try
            {
                if (ClientGuid != null && ClientGuid.Length > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        int? cnt = _connection.Query<int?>(@"SELECT COUNT(ID) FROM TaskSyncDB WHERE ClientGuid=@ClientGuid", new { ClientGuid = ClientGuid }).FirstOrDefault();

                        if (cnt > 0)
                        {
                            isExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExists;
        }
        #endregion IsClientGuidExists

        #region GetSyncDBIDByClientGuid
        /// <summary>
        /// GetSyncDBIDByClientGuid
        /// </summary>
        /// <param name="ClientGuid"></param>
        /// <returns></returns>
        public int GetSyncDBIDByClientGuid(string ClientGuid)
        {
            int syncDbID = 0;
            try
            {
                if (ClientGuid != null && ClientGuid.Length > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        int? cnt = _connection.Query<int?>(@"SELECT ID FROM TaskSyncDB WHERE ClientGuid=@ClientGuid", new { ClientGuid = ClientGuid }).FirstOrDefault();

                        if (cnt > 0)
                        {
                            syncDbID = cnt.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return syncDbID;
        }
        #endregion GetSyncDBIDByClientGuid

        #region InsertResposeGPSLocation
        /// <summary>
        /// lstSurveyResponseGPSLocation
        /// </summary>
        /// <param name="lstSurveyResponseGPSLocation"></param>
        public void InsertResposeGPSLocation(List<SurveyResponseGPSLocation> lstSurveyResponseGPSLocation)
        {
            string StateName = "";
            string DistrictName = "";

            var strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                foreach (var item in lstSurveyResponseGPSLocation)
                {
                    StateName = "";
                    DistrictName = "";

                    if (CommonActivity.IsValidlatitudeAndlongitude(item.GPS))
                    {
                        string geoLocation = CommonActivity.ReverseGeoCoding(item.GPS);
                        if (!string.IsNullOrEmpty(geoLocation))
                        {
                            foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (myString.Length > 0)
                                {
                                    if (myString.ToString().Contains("district"))
                                    {
                                        var indexOfColon = myString.ToString().IndexOf(":");
                                        var distname = myString.ToString().Substring(indexOfColon + 1);
                                        DistrictName = distname.Trim();
                                    }

                                    if (myString.ToString().Contains("state"))
                                    {
                                        var indexOfColon = myString.ToString().IndexOf(":");
                                        var statename = myString.ToString().Substring(indexOfColon + 1);
                                        StateName = statename.Trim();
                                    }
                                }
                            }
                        }
                    }


                    strQuery = @"INSERT INTO TbProjectSurveyResponseLocation(SurveyID,SyncTaskID,State,District,SubmissionCount,CreatedDate,GPS) VALUES(@SurveyID,@SyncTaskID,@State,@District,@SubmissionCount,NOW(),@GPS); ";
                    _connection.Execute(strQuery, new
                    {
                        SurveyID = item.SurveyID,
                        SyncTaskID = item.SyncTaskID,
                        State = StateName,
                        District = DistrictName,
                        SubmissionCount = item.SubmissionCount,
                        GPS = item.GPS
                    });
                }
            }
        }
        #endregion InsertResposeGPSLocation
    }
}
