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

namespace SunidhiV2_0.Code.DAL
{
    public class CreateProjectDAL
    {
        protected IDbConnection _connection;
        static protected IDbConnection _conn;

        #region CreateProject

      
      

        public IEnumerable<ProjectTemplate> GetProjectThemeList(int InitiativeID)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct IP.ThemeID ,T.ThemeName Theme,Count(case when ProjectTemplateID =0 then null else ProjectTemplateID end) NoOfTemplates ,0 IsSelected " +
                            " From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID " +
                            " Where IP.IsDeleted =0 And IP.InitiativeId = @InitiativeID  Group By IP.ThemeID ,T.ThemeName";
                lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { InitiativeID = InitiativeID }).ToList();
            }
            return lstPrjTemp;
        }
        public IEnumerable<ProjectTemplate> GetProjectTemplateList(int InitiativeID, int ThemeID, int NoOfTemplates)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                if (NoOfTemplates != 0)
                    strQuery = @"SELECT Distinct IP.ThemeID ,PT.ProjectTemplateID ProjectTemplateID,ProjectTemplateName TemplateName,Description ,0 IsSelected" +
                                " From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID  " +
                                " left join  TbProjectTemplate PT On IP.ProjectTemplateID= PT.ProjectTemplateID Where IP.ThemeID =@ThemeID  And IP.InitiativeId =@InitiativeID" ;
                else
                    strQuery = @"SELECT Distinct T.ThemeID ,T.ThemeName Theme ,PT.ProjectTemplateID ProjectTemplateID," +
                                " ProjectTemplateName TemplateName,Description ,0 IsSelected From TbProjectTemplate PT " +
                                "inner join TbProjectTheme T On T.ThemeID = PT.ThemeID Where PT.IsActive =1 And PT.ThemeID =@ThemeID ";
                lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { ThemeID = ThemeID ,InitiativeID = InitiativeID }).ToList();
            }
            return lstPrjTemp;
        }

        public int CreateProject(string ProjectName, string ProjectDescription, DateTime StartDate, DateTime EndDate,
                    string ProjectTags,  int InitiativeID, int loggedUserId, int WorkspaceId)
        {
            string strQuery = string.Empty;            
         
            var id = 0;

           // var ProjectCode = GenerateProjectCode();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_CreateProjectNew";
                    id = _connection.ExecuteScalar<int>(strQuery, new
                    {
                        INProjectName = ProjectName,
                        INProjectDescription = ProjectDescription,
                        INStartDate = StartDate,
                        INEndDate = EndDate,
                        INProjectTags = ProjectTags,
                        INInitiativeID = 1,                        
                        loggedUserId = loggedUserId,
                        WorkspaceId = WorkspaceId,
                    }, commandType: CommandType.StoredProcedure);
                }

            }

            catch (Exception ex)
            {
              
                throw ex;
            }
            return id;
        }

        public void UpdateEditProjectDetails(Project inputProject, int loguser)
        {
            string query = string.Empty;
            //DateTime? startDate = inputProject.StartDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.StartDate) : null;
            //DateTime? endDate = inputProject.EndDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.EndDate) : null;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbProject set
                                                ProjectName = @projectName ,
                                                Description = @description ,
                                                Code = @code ,
                                                Budget = @budget ,
                                                StartDate = @startdate ,
                                                EndDate = @enddate ,
                                                CountryID = @countryID ,
                                                StateID = @stateID ,
                                                DistrictID = @districtID ,
                                                UpdatedDate = @updatedDate,
                                                UpdatedBy=@userID where ProjectID =@projectID; ";
                _connection.Execute(query, new
                {
                    projectID = inputProject.ProjectId,
                    projectName = inputProject.ProjectName,
                    description = inputProject.Description,
                    code = inputProject.Code,
                    budget = inputProject.Budget,
                    startdate = inputProject.StartDate,
                    enddate = inputProject.EndDate,
                    countryID = inputProject.CountryID,
                    stateID = inputProject.StateID,
                    districtID = inputProject.DistrictID,
                    updatedDate = DateTime.Now,
                    userID = loguser
                });
                //first remove the tag list for this project ID
                query = @"Update TbProjectToTagList set IsDeleted=1,UpdatedDate = @updatedDate,
UpdatedBy=@userID where ProjectID =@projectID; ";
                _connection.Execute(query, new
                {
                    projectID = inputProject.ProjectId,
                    updatedDate = DateTime.Now,
                    userID = loguser
                });

                List<Tags> objTagsList = JsonConvert.DeserializeObject<List<Tags>>(inputProject.ProjectTags);
                foreach (Tags tagItem in objTagsList)
                {
                    query = @"insert TbProjectToTagList (ProjectId, TagID, CreatedDate, CreatedBy, IsDeleted)
values (@projectid,@tagid,@createddate,@createdby,@isdeleted)";
                    _connection.Execute(query, new
                    {
                        projectid = inputProject.ProjectId,
                        tagid = tagItem.TagID,
                        createddate = DateTime.Now,
                        createdby = loguser,
                        isdeleted = 0
                    });
                }
            }
            ProjectAuditLog objLog = new ProjectAuditLog();
            objLog.ProjectID = inputProject.ProjectId;
            objLog.AuditFrom = "Project";
            objLog.AuditEvent = "EDIT";
            objLog.AuditFromID = inputProject.ProjectId;
            objLog.FromIDColumnName = "ProjectId";
            objLog.Message = "Updated Project details";
            objLog.CreatedBy = loguser;
            AddAuditLog(objLog);
            return;
        }
        public static string AddAuditLog(ProjectAuditLog objPrjAuditLog)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;

            using (_conn = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = " INSERT INTO TbProjectAuditLog(ProjectID,AuditFrom,AuditEvent,AuditFromID,FromIDColumnName,Message,CreatedDate,CreatedBy) " +
                    " VALUES(@ProjectID,@AuditFrom,@AuditEvent,@AuditFromID,@FromIDColumnName,@Message,now(),@loggedUserId);SELECT LAST_INSERT_ID();";
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
            return retStatus;
        }
        #endregion CreateProject

        public int OutcomesExists(int ProjectId)
        {
            string query = string.Empty;
            var prjid = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"SELECT ProjectId FROM TbOutcomes WHERE ProjectId=@ProjectId;";
                prjid = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                }).FirstOrDefault();

            }
            return prjid;
        }
        public int InsertIntoOutcomes(ProjectOutcome objOutcome, int loggedUserId, int ProjectId, int SurveyId, string PrjtStartDate, string PrjtEndDate)
        {
            
            string query = string.Empty;
            var Outcomeid = 0;

            int prjid = OutcomesExists(ProjectId);
            if (prjid == 0)
            {

                //string inputFormat = "dd-MM-yyyy";
                //string outputFormat = "yyyy-MM-dd";
                //var dtStartDate = DateTime.ParseExact(objOutcome.strStartDate, inputFormat, CultureInfo.InvariantCulture);
                //var dtEndDate = DateTime.ParseExact(objOutcome.strEndDate, inputFormat, CultureInfo.InvariantCulture);
                //string strStartDate = dtStartDate.ToString(outputFormat);
                //string strEndDate = dtEndDate.ToString(outputFormat);

                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @" insert into TbOutcomes(ProjectId,OutcomeName,StartDate,EndDate,DatacollectionType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,SurveyLogic,SurveyTarget,WorkflowId,Periodicity,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)values(@ProjectId,@OutcomeName, DATE_FORMAT(@StartDate, "" %Y-%m-%d""),DATE_FORMAT(@EndDate, "" %Y-%m-%d""),@DatacollectionType,@SurveyId," +
                        "@SurveySectionId,@SurveyQuestionId,@SurveyResponseId,@SurveyLogic,@SurveyTarget," +
                        "@WorkflowId,@Periodicity,@IsDeleted,now(),now(),@CreatedBy,@UpdatedBy);SELECT LAST_INSERT_ID();";
                    Outcomeid = _connection.Query<int>(query, new
                    {
                        ProjectId = ProjectId,
                        OutcomeName = objOutcome.OutcomeName,
                        // StartDate = Convert.ToDateTime(objOutcome.strStartDate),
                        // EndDate = Convert.ToDateTime(objOutcome.strEndDate),
                        StartDate = Convert.ToDateTime(PrjtStartDate),
                        EndDate = Convert.ToDateTime(PrjtEndDate),
                        DatacollectionType = objOutcome.DatacollectionType,
                        SurveyId = SurveyId,
                        SurveyQuestionId = objOutcome.SurveyQuestionId,
                        SurveySectionId = objOutcome.SurveySectionId,
                        SurveyResponseId = 0,//objOutcome.SurveyResponseId,
                        SurveyLogic = objOutcome.SurveyLogic,
                        SurveyTarget = objOutcome.SurveyTarget,
                        WorkflowId = objOutcome.WorkflowId,
                        Periodicity = objOutcome.Periodicity,
                        IsDeleted = 0,
                        CreatedBy = loggedUserId,
                        UpdatedBy = loggedUserId
                    }).Single();

                    if (Outcomeid != 0)
                    {

                        InsertIntoTbOutcome_SDG(objOutcome, Outcomeid);

                        ProjectOutcomeDAL objOutcomeDAL = new ProjectOutcomeDAL();
                        objOutcomeDAL.GenerateReportingFrequencyTrackDate(objOutcome, loggedUserId, Outcomeid);

                        // Insert into Sync Db
                        string Qeury = @"Select TbOutcomes.WorkflowId,WfNoOfLevels From TbOutcomes TbOutcomes 
                                 Left Join TbProjectWorkflow  TbProjectWorkflow On TbProjectWorkflow.WorkflowID = TbOutcomes.WorkflowId
                                 Where TbOutcomes.IsDeleted = 0 And TbProjectWorkflow.IsDeleted = 0 And TbOutcomes.pk_OutcomeID = @Outcomeid";
                        dynamic objWF = _connection.Query<object>(Qeury, new { Outcomeid = Outcomeid }).FirstOrDefault();

                        Qeury = @"Select WSID as WorkspaceID  From TbProject Where ProjectId = @ProjectId";
                        //Qeury = @"Select WorkspaceID From TbProject  A Inner Join TbInitiative B On B.InitiativeId =A.InitiativeId Where ProjectId = @ProjectId";
                        var WID = _connection.Query<int>(Qeury, new { ProjectId = ProjectId }).FirstOrDefault();
                        //AddTo SyncDB
                        var strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                        var syncDBId = _connection.Query<int>(strQuery, new
                        {
                            Type = "Outcome",
                            Command = "ADD",
                            TypeIDColumn = "pk_OutcomeID",
                            TypeID = Outcomeid,
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
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Outcome";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = Outcomeid;
                        objLog.FromIDColumnName = "pk_OutcomeID";
                        objLog.Message = "Created a new Outcome named " + objOutcome.OutcomeName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }


                }
            }
            return Outcomeid;
        }
        public void InsertIntoTbOutcome_SDG(ProjectOutcome objOutcome, int Outcomeid)
        {
            using (_connection = Utils.Database.GetDBConnection())
            {
                // Delete -- Making IsDelete = 1
                string strDeleteQuery = string.Empty;
                strDeleteQuery = "Update TbOutcome_SDG set IsDeleted = 1 where OutcomeID = @Outcomeid";
                _connection.Query<int>(strDeleteQuery, new { Outcomeid = Outcomeid });

                // Insert into TbOutcome_SDG
                //List<SDG> lstSDGList = JsonConvert.DeserializeObject<List<SDG>>(objOutcome.SDGList);
                string QuertyStmt = string.Empty;
                foreach (SDG item in objOutcome.lstSDGList)
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

                    InsertIntoTbOutcome_SDG_Targets(item, Outcomeid);
                }
            }

        }
        public void InsertIntoTbOutcome_SDG_Targets(SDG SDGTargets, int Outcomeid)
        {

            using (_connection = Utils.Database.GetDBConnection())
            {
                // Delete -- Making IsDelete = 1
                string strDeleteQuery = "Update TbOutcome_SDG_Targets set IsDeleted = 1 where OutcomeID = @Outcomeid";
                _connection.Query<int>(strDeleteQuery, new { Outcomeid = Outcomeid });

                var QuertyStmt = string.Empty;
                // Insert into TbOutcome_SDG_Targets
                //List<SDGTargets> lstSDGList_Targets = JsonConvert.DeserializeObject<List<SDGTargets>>(objOutcome.SDGTargetsList);
                string QuertyStmt2 = string.Empty;
                foreach (SDGTargets item in SDGTargets.Target)
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

        public int OutputExists(int ProjectId)
        {
            string query = string.Empty;
            var prjid = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"SELECT ProjectId FROM TbProjectActivity WHERE ProjectId=@ProjectId;";
                prjid = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                }).FirstOrDefault();

            }
            return prjid;
        }

        public bool InsertIntoOutput(ProjectActivity inputObj, int loggedUserId, int ProjectId, int SurveyId, string PrjtStartDate, string PrjtEndDate)
        {
            string strQuery = string.Empty;
            var actid = 0;
            bool retStatus = false;

            int prjid = OutputExists(ProjectId);
            if (prjid == 0)
            {

                //string inputFormat = "dd-MM-yyyy";
                //string outputFormat = "yyyy-MM-dd";
                //var dtStartDate = DateTime.ParseExact(inputObj.strStartDate, inputFormat, CultureInfo.InvariantCulture);
                //var dtEndDate = DateTime.ParseExact(inputObj.strEndDate, inputFormat, CultureInfo.InvariantCulture);
                //string strStartDate = dtStartDate.ToString(outputFormat);
                //string strEndDate = dtEndDate.ToString(outputFormat);

                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"INSERT INTO TbProjectActivity(ProjectId, ParentActivityId, ActivityName, Description, StartDate, EndDate, 
                                                            WeightagePercentage, ActivityType, SurveyId, SurveySectionId, 
                                                            SurveyQuestionId, SurveyResponseId, SurveyLogic, SurveyTarget, 
                                                            IsPaymentLinked, PaymentLinkedBudget, WorkflowId, Periodicity, 
                                                            IsDeleted, CreatedDate, CreatedBy, UpdatedDate )
                                            VALUES(@projectid,@parentactid,@actname,@desc,DATE_FORMAT(@startdate, "" %Y-%m-%d""),DATE_FORMAT(@enddate, "" %Y-%m-%d""),
                                                   @weight,@acttype,@surveyid,@sectionid,
                                                   @quesid,@respid,@logic,@target,
                                                   @ispaylink,@linkbudg,@wfid,@period,
                                                   @isdeleted,@createdate,@createby,@upddate); 
                                             SELECT LAST_INSERT_ID();";
                    actid = _connection.Query<int>(strQuery, new
                    {
                        projectid = ProjectId,
                        parentactid = 0,
                        actname = inputObj.OutputName,
                        desc = inputObj.Description,
                        // startdate = Convert.ToDateTime(inputObj.strStartDate),
                        // enddate = Convert.ToDateTime(inputObj.strStartDate),
                        startdate = Convert.ToDateTime(PrjtStartDate),
                        enddate = Convert.ToDateTime(PrjtEndDate),
                        weight = 0,
                        acttype = inputObj.Type,
                        surveyid = SurveyId,
                        sectionid = 0,
                        quesid = 0,
                        respid = 0,
                        logic = 0,
                        target = 0,
                        ispaylink = 0,
                        linkbudg = 0,
                        wfid = 0,
                        period = inputObj.Periodicity,
                        isdeleted = 0,
                        createdate = DateTime.Now,
                        createby = loggedUserId,
                        upddate = DateTime.Now
                    }).Single();

                    if (actid > 0)
                    {


                        string Qeury = @"Select A.WorkflowId, FinanceWorkflowId,WfNoOfLevels   From TbProjectActivity A 
                                 Left Join TbProjectWorkflow  AW On AW.WorkflowID = A.WorkflowId
                                 Where A.IsDeleted = 0 And AW.IsDeleted = 0 And ActivityId = @actid";
                        dynamic objWF = _connection.Query<object>(Qeury, new { actid = actid }).FirstOrDefault();
                        //Qeury = @"Select WorkspaceID From TbProject  A Inner Join TbInitiative B On B.InitiativeId =A.InitiativeId Where ProjectId = @ProjectId";
                        Qeury = @"Select WSID as WorkspaceID  From TbProject Where ProjectId = @ProjectId";
                        var WID = _connection.Query<int>(Qeury, new { ProjectId = ProjectId }).FirstOrDefault();
                        //AddTo SyncDB
                        strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                        var syncDBId = _connection.Query<int>(strQuery, new
                        {
                            Type = "Activity",
                            Command = "ADD",
                            TypeIDColumn = "ActivityId",
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
                        objLog.ProjectID = inputObj.ProjectId;
                        objLog.AuditFrom = "Activity";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = actid;
                        objLog.FromIDColumnName = "ActivityId";
                        objLog.Message = "Created a new output named " + inputObj.OutputName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                        retStatus = true;
                        //inputObj.ActivityId = actid;
                        bool addPriodicity = AddPeriodicityToActivity(loggedUserId, inputObj, actid);
                    }
                }
            }

            return retStatus;
        }
        public bool AddPeriodicityToActivity(int loggedUserId, ProjectActivity inputObj, int Outputid)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;

            bool isDeleted = DeletePeriodicityForActivity(Outputid.ToString());

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
                query = "INSERT INTO TbProjectActivityTrackDate(TrackDate,ActivityId,TrackStatus) ";

                for (int i = 0; i < lstTrackDates.Count; i++)
                {
                    query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + Outputid + ",0" : " SELECT '" + lstTrackDates[i] + "'," + Outputid + ",0";
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

            return success;
        }

        public bool DeletePeriodicityForActivity(string ActIdList)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQuery = "DELETE FROM TbProjectActivityTrackDate WHERE FIND_IN_SET(ActivityId,@actidlist) ";
                _connection.Execute(strQuery, new { actidlist = ActIdList });
            }
            return true;
        }

        public int SurveyExists(int ProjectId)
        {
            string query = string.Empty;
            var prjid = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"SELECT ProjectId FROM TbProjectSurvey WHERE ProjectId=@ProjectId;";
                prjid = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                }).FirstOrDefault();

            }
            return prjid;
        }

        public int InsertIntoSurvey(ProjectSurvey objSurvey, int loggedUserId, int ProjectId,string  PrjtStartDate, string PrjtEndDate)
        {
            var SurveyId = 0;
            string query = string.Empty;

            int prjid = SurveyExists(ProjectId);
            if (prjid == 0)
            {

                //string inputFormat = "dd-MM-yyyy";
                //string outputFormat = "yyyy-MM-dd";
                //var dtStartDate = DateTime.ParseExact(objSurvey.StartDate, inputFormat, CultureInfo.InvariantCulture);
                //var dtEndDate = DateTime.ParseExact(objSurvey.EndDate, inputFormat, CultureInfo.InvariantCulture);
                //string strStartDate = dtStartDate.ToString(outputFormat);
                //string strEndDate = dtEndDate.ToString(outputFormat);


                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"INSERT INTO TbProjectSurvey(SurveyName,SurveyDescription,ProjectId,WorkflowID,ReportingFrequency,BeneficiaryTypeID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,StartDate,EndDate) VALUES (@SurveyName,@SurveyDescription,@ProjectId,@WorkflowID,@ReportingFrequency,@BeneficiaryTypeID,@IsActive,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy,DATE_FORMAT(@StartDate, "" %Y-%m-%d""),DATE_FORMAT(@EndDate, "" %Y-%m-%d"")); SELECT LAST_INSERT_ID();";

                    SurveyId = _connection.Query<int>(strQuery, new
                    {
                        SurveyName = objSurvey.SurveyName,
                        SurveyDescription = objSurvey.SurveyDescription,
                        ProjectId = ProjectId,
                        WorkflowID = objSurvey.WorkflowID,
                        ReportingFrequency = objSurvey.ReportingFrequency,
                        BeneficiaryTypeID = objSurvey.BeneficiaryTypeID,
                        // StartDate = Convert.ToDateTime(objSurvey.StartDate),
                        // EndDate = Convert.ToDateTime(objSurvey.EndDate),
                        StartDate = Convert.ToDateTime(PrjtStartDate),
                        EndDate = Convert.ToDateTime(PrjtEndDate),
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUserId,
                        UpdatedBy = loggedUserId
                    }).FirstOrDefault();

                    if (SurveyId > 0)
                    {
                        string Qeury = @"Select A.WorkflowId, WfNoOfLevels  From TbProjectSurvey A 
                                Left Join TbProjectWorkflow  AW On AW.WorkflowID = A.WorkflowId
                                Where A.IsActive = 1 And AW.IsDeleted = 0 And SurveyID = @SurveyId";
                        dynamic objWF = _connection.Query<object>(Qeury, new { SurveyId = SurveyId }).FirstOrDefault();

                        //Qeury = @"Select WorkspaceID From TbProject  A Inner Join TbInitiative B On B.InitiativeId =A.InitiativeId Where ProjectId = @ProjectId";
                        Qeury = @"Select WSID as WorkspaceID  From TbProject Where ProjectId = @ProjectId";
                        var WID = _connection.Query<int>(Qeury, new { ProjectId = ProjectId }).FirstOrDefault();

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
                            CreatedBy = loggedUserId

                        }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "Survey";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = SurveyId;
                        objLog.FromIDColumnName = "SurveyId";
                        objLog.Message = "Added new Survey named " + objSurvey.SurveyName;
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                    }

                    AddProjectSurveySection(objSurvey, loggedUserId, SurveyId);
                }
            }
            return SurveyId;
        }
        public void AddProjectSurveySection(ProjectSurvey objSurvey, int loggeduserid, int SurveyID)
        {
            var SectionId = 0;
            string strQuery = "";
            var lastSceOrder = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = "Select ifnull(max(ifnull(SectionOrder, 0)),0)   From TbProjectSurveySection Where SurveyID = @SID ";
                lastSceOrder = _connection.Query<int>(strQuery, new { SID = SurveyID }).Single();

                foreach (ProjectSurveySection item in objSurvey.SectionList)
                {
                    strQuery = @"INSERT INTO TbProjectSurveySection(SectionName,SectionDescription,SectionOrder ,SurveyID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)" +
                            "VALUES (@SectionName,@SectionDescription,@SectionOrder,@SurveyID,@IsActive ,@CreatedDate,@CreatedDate,@CreatedBy,@CreatedBy); SELECT LAST_INSERT_ID();";

                    SectionId = _connection.Query<int>(strQuery, new
                    {
                        SectionName = item.SectionName,
                        SectionDescription = item.SectionDescription,
                        SectionOrder = lastSceOrder + 1,
                        SurveyID = SurveyID,
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        CreatedBy = loggeduserid
                    }).Single();

                    AddProjectSurveySectionQuestions(item, loggeduserid, SurveyID, SectionId);


                    strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                    var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = SurveyID }).Single();

                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "Survey";
                    objLog.AuditEvent = "ADD";
                    objLog.AuditFromID = SectionId;
                    objLog.FromIDColumnName = "SectionID";
                    objLog.Message = "Added new Survey Section named " + item.SectionName + "'";
                    objLog.CreatedBy = loggeduserid;
                    ProjectDAL.AddAuditLog(objLog);
                }
            }
        }
        public void AddProjectSurveySectionQuestions(ProjectSurveySection objSurveySection, int loggeduserid, int SurveyID, int SectionId)
        {
            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQuery = string.Empty;
                var QuestionId = 0;
                foreach (SurveyQuestion item in objSurveySection.SurveySecQuestions)
                {
                    strQuery = @"INSERT INTO TbProjectSurveyQuestion(SurveyID, SectionID, QuestionName, QuestionTypeID, QuestionHint,DefaultValue,IsUnique,IsSearchable,IsMandatory,IsFormulaQuestion,FormulaOperation,FormulaExpression,FormulaFieldVariables,RegEx,ErrorText,IsValidationQuestion,UOM,CreatedDate, CreatedBy, IsActive,QuestionOrder) VALUES (@SurveyID, @SectionID, @QuestionName, @QuestionTypeID, @QuestionHint,@DefaultValue,@IsUnique,@IsSearchable,@IsMandatory,@IsFormulaQuestion,@FormulaOperation,@FormulaExpression,@FormulaFieldVariables,@RegEx,@ErrorText,@IsValidationQuestion,@UOM,now(), @CreatedBy, 1,@QuestionOrder); SELECT LAST_INSERT_ID();";

                    QuestionId = _connection.Query<int>(strQuery, new
                    {
                        SurveyID = SurveyID,
                        SectionID = SectionId,
                        QuestionName = item.QuestionName,
                        QuestionTypeID = item.QuestionTypeID,
                        QuestionHint = "",
                        DefaultValue = "",
                        IsUnique = 1,
                        IsSearchable = 1,
                        IsMandatory = 1,
                        IsFormulaQuestion = 0,
                        FormulaOperation = "",
                        FormulaExpression= "",
                        FormulaFieldVariables = "",
                        RegEx = "",
                        ErrorText = "",
                        IsValidationQuestion = 0,
                        UOM = "",
                        CreatedBy = loggeduserid,
                        QuestionOrder =item.QuestionOrder,
                    }).Single();

                    if (item.QuestionTypeID == 7)
                    {
                        AddProjectSurveySectionQuestionsOptions(item,QuestionId, loggeduserid);
                    }
                    
                    strQuery = "Select ProjectId  From TbProjectSurvey Where SurveyID =@SurveyId ";
                    var ProjectId = _connection.Query<int>(strQuery, new { SurveyId = SurveyID }).Single();

                    //ProjectAuditLog objLog = new ProjectAuditLog();
                    //objLog.ProjectID = ProjectId;
                    //objLog.AuditFrom = "Survey";
                    //objLog.AuditEvent = "ADD";
                    //objLog.AuditFromID = SectionId;
                    //objLog.FromIDColumnName = "SectionID";
                    //objLog.Message = "Added new Survey Section named " + item.SectionName + "'";
                    //objLog.CreatedBy = loggeduserid;
                    //ProjectDAL.AddAuditLog(objLog);
                }
            }
        }

        public void AddProjectSurveySectionQuestionsOptions(SurveyQuestion objSurveyQuest, int QuestId, int loggeduserid)
        {
            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQuery = string.Empty;
                var QuestionOptionId = 0;
                foreach (QuestionOptionList item in objSurveyQuest.lstQuestionOptions)
                {
                    strQuery = @"INSERT INTO TbProjectSurveyQSTOption(QuestionID, OptionValue, CreatedDate, CreatedBy)VALUES (@QuestionIDVar,@OptionVal,now(), @CreatedBy); SELECT LAST_INSERT_ID();";

                    QuestionOptionId = _connection.Query<int>(strQuery, new
                    {
                       OptionVal = item.OptionValue,
                       QuestionIDVar = QuestId,
                       CreatedBy = loggeduserid,
                    }).Single();

                }
            }
        }

        public string  CompleteProjectCreation(int ProjectId, int UserId, double Budget)
        {
            string query = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbProject set Budget = @Budget ,UpdatedDate = @updatedDate,UpdatedBy=@userID, IsCreated = 1 where ProjectId =@ID; ";
                _connection.Execute(query, new { ID = ProjectId, Budget = Budget, updatedDate = DateTime.Now, userID = UserId });
            }
            return "Success";
        }
        public IEnumerable<UsersDetails> GetUsersListByID(int WorkspaceID)
        {
            List<UsersDetails> lstUsers = new List<UsersDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {


                strQuery = @"SELECT Distinct U.ID, Name ,WT.RoleID  RoleID,RoleName Role,IFNULL(U.ProfileImage, '') AS ProfileImage,U.EmailId,U.IsRegisteredUser,U.IsDeleted  FROM TbUser U Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID Where IsRegisteredUser = 1 and U.IsDeleted = 0 And U.IsActive =1 AND WT.WorkspaceID =@WorkspaceID and WT.IsDtd = 0 and WT.IsActive = 1  Order by U.ID Desc ";

                lstUsers = _connection.Query<UsersDetails>(strQuery, new { WorkspaceID = WorkspaceID}).ToList();
            }
            return lstUsers;
        }
        public void DeleteBudgetPlanHead(int ProjectId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {


                strQuery = @"DELETE from TbBudgetPlan where ProjectId = @ProjectId ";

                _connection.Execute(strQuery, new
                {
                    ProjectId = ProjectId
                 });

                strQuery = @"DELETE from TbBudgetPlanItemDetails where ProjectId = @ProjectId ";

                _connection.Execute(strQuery, new
                {
                    ProjectId = ProjectId
                });
            }
        }

        public void DeletePaymentTerms(int ProjectId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {


                strQuery = @"DELETE from TbPaymentTerms where ProjectId = @ProjectId ";

                _connection.Execute(strQuery, new
                {
                    ProjectId = ProjectId
                });

                strQuery = @"DELETE from TbPaymentTermNotifyUsers where ProjectId = @ProjectId ";

                _connection.Execute(strQuery, new
                {
                    ProjectId = ProjectId
                });
            }
        }

        public void DeleteDonorsBudget(int ProjectId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {


                strQuery = @"DELETE from TbProjectBudgetAndFundDetails where ProjectId = @ProjectId AND UserType = 'D' ";

                _connection.Execute(strQuery, new
                {
                    ProjectId = ProjectId
                });

            }
        }
    }
}
