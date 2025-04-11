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
    public class DashboardDAL
    {
        protected IDbConnection _connection;
        public IEnumerable<Project> GetProjectListByWSID(int WorkspaceID)
        {
            List<Project> lstProjects = new List<Project>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT ProjectId,ProjectName FROM  
                             TbProject TP  where TP.IsDeleted=0 and IsCreated=1 and WSID=@WorkspaceID  Order by ProjectId Desc";
                lstProjects = _connection.Query<Project>(strQuery, new { WorkspaceID = WorkspaceID }).ToList();


            }
            return lstProjects;
        }
        public IEnumerable<ActivityCount> GetActivityCount(int ProjectId)
        {
            List<ActivityCount> lstActivity = new List<ActivityCount>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT count(ActivityId) TotalActivityCount,(Select count(ActivityId) from TbProjectActivity where EndDate >now() and TA.IsDeleted=0  and ProjectId=@ProjectId) as PendingActivityCount FROM  
                             TbProjectActivity TA  where TA.IsDeleted=0  and ProjectId=@ProjectId ";
                lstActivity = _connection.Query<ActivityCount>(strQuery, new { ProjectId = ProjectId }).ToList();


            }
            return lstActivity;
        }
        //public int GetProjectProgress(int ProjectId)
        //{
        //    int Progress = 0;
        //    string strQuery = string.Empty;
        //    if(ProjectId>0)
        //    { 
        //    using (_connection = Utils.Database.GetDBConnection())
        //    {

        //        strQuery = @"SELECT IFNULL(Progress,0) as Progress FROM  
        //                     TbProject  where ProjectId=@ProjectId ";
        //         Progress = _connection.Query<int>(strQuery, new { ProjectId = ProjectId }).Single();


        //    }
        //    }
        //    return Progress;
        //}
        public double GetProjectProgress(int ProjectId)
        {
            double ActivityPercentage = 0;
            double OutComePercentage = 0;
            double SurveyPercentage = 0;
            double ProjectPercentage = 0;
            double ProjectPercentageDivision = 0;
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT IFNULL(sum(Progress)/(count(ActivityId)*100)*100,0) FROM TbProjectActivity where ProjectId=@ProjectId and IsDeleted=0";
                ActivityPercentage = _connection.Query<double>(strQuery, new { ProjectId = ProjectId }).Single();

                strQuery = @"SELECT IFNULL(sum(Progress)/(count(pk_OutcomeID)*100)*100,0) FROM TbOutcomes where ProjectId=@ProjectId and IsDeleted=0";
                OutComePercentage = _connection.Query<double>(strQuery, new { ProjectId = ProjectId }).Single();

                strQuery = @"SELECT IFNULL(sum(Progress)/(count(SurveyID)*100)*100,0) FROM TbProjectSurvey where ProjectId=@ProjectId and IsActive=1
                                     and SurveyId not in (select SurveyId 
                                    from (
                                        select SurveyId,ProjectId from TbProjectActivity
                                        union all
                                        select SurveyId,ProjectId from TbOutcomes
                                    ) a where ProjectId=@ProjectId)";
                SurveyPercentage = _connection.Query<double>(strQuery, new { ProjectId = ProjectId }).Single();


                if (ActivityPercentage > 0)
                {
                    ProjectPercentageDivision = 100;
                }
                if (OutComePercentage > 0)
                {
                    ProjectPercentageDivision = ProjectPercentageDivision + 100;
                }
                if (SurveyPercentage > 0)
                {
                    ProjectPercentageDivision = ProjectPercentageDivision + 100;
                }

                if (ProjectPercentageDivision > 0)
                {
                    ProjectPercentage = Math.Round(((ActivityPercentage + OutComePercentage + SurveyPercentage) / ProjectPercentageDivision) * 100);

                }
                //if (ProjectPercentage == 100)
                //{
                //    strQuery = @"Update TbProject Set ProjectStatus = 'Completed' where ProjectId = @ProjectId";
                //    _connection.Execute(strQuery, new
                //    {
                //        ProjectId = ProjectId,
                //        ProjectPercentage = ProjectPercentage,

                //    });
                //}

                return ProjectPercentage;


            }
        }

        public IEnumerable<Project> FillDashboard(int WorkspaceID, int loggedUserId)
        {

            List<Project> lstProjects = new List<Project>();
            List<ProjectList> ProjectList = new List<ProjectList>();
            List<AssignedList> AssignedUser = new List<AssignedList>();
            List<ProjectStatusDetail> lstProjectStatus = new List<ProjectStatusDetail>();
            string strQuery = string.Empty;
            try
            {
                if (loggedUserId != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"select IFNULL(count(distinct aa.ProjectId),0)ProjectCunt,IFNULL(count(distinct c.BeneficiaryTypeID),0)TotalBeneficiary from (SELECT TP.ProjectId FROM TbProject TP left join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 " +
                                   " and (Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) = @wsid and (TP.CreatedBy = @loggedUserId or exists (select p.GroupId from TbUserAssignedToGroup p where p.UserId= @loggedUserId and p.GroupId = TP.GroupId)) And TP.IsCreated = 1) aa " +
                                   " left outer join (select b1.BeneficiaryTypeID,b1.ProjectId from TbProjectBeneficiaryType b1 where IFNULL(b1.IsActive,0) = 1 and IFNULL(b1.IsDeleted,0) = 0) c on c.ProjectId = aa.ProjectId";

                        lstProjects = _connection.Query<Project>(strQuery, new { wsid = WorkspaceID, loggedUserId = loggedUserId }).ToList();
                        if (lstProjects != null)
                        {
                            // Project Progress Data //

                            string strQueryProjectProgress = @"SELECT TP.ProjectId,TP.ProjectName,CASE WHEN IFNULL(TP.Weightage,0) >0 AND IFNULL(TP.OutputProgress,0) > 0 THEN ROUND((TP.OutputProgress/TP.Weightage)*100)  ELSE 0 END Progress FROM TbProject TP left join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 " +
                                                          " and (Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) = @wsid and (TP.CreatedBy =@loggedUserId or exists (select p.GroupId from TbUserAssignedToGroup p where p.UserId=@loggedUserId and p.GroupId = TP.GroupId)) And TP.IsCreated = 1";
                            ProjectList = _connection.Query<ProjectList>(strQueryProjectProgress, new { wsid = WorkspaceID, loggedUserId = loggedUserId }).ToList();
                            lstProjects[0].ProjectList = ProjectList;
                            for (var p = 0; p < ProjectList.Count(); p++)
                            {
                                AssignedUser = GetProjectUserAssign(ProjectList[p].ProjectId, WorkspaceID, loggedUserId).ToList();

                                int CompleteTask = GetProjectUserComplete(ProjectList[p].ProjectId, WorkspaceID, loggedUserId);
                                double ProgressCalL = 0; double CompleteTaskDbl = 0.00; double AssignTaskDbl = 0.00;
                                if (AssignedUser.Count != 0)
                                {
                                    CompleteTaskDbl = CompleteTask;
                                    AssignTaskDbl = AssignedUser[0].AssignedCunt;
                                    ProgressCalL = ((CompleteTaskDbl / AssignTaskDbl) * 100);
                                    if (ProgressCalL > 100)
                                    {
                                        ProgressCalL = 100;
                                    }
                                   // lstProjects[0].ProjectList[p].Progress = Math.Round(ProgressCalL);
                                }
                                else
                                {
                                   // lstProjects[0].ProjectList[p].Progress = 0;
                                }
                            }



                            // Project Summary //

                            int Mtotal = 0;

                            string strQueryProjectSummary = @"select aa.ProjectStatus StatusName,IFNULL(count(aa.ProjectId),0)StatusCunt from (SELECT TP.ProjectStatus,TP.ProjectId FROM TbProject TP left join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 " +
                                                            " and (Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) = @wsid and (TP.CreatedBy =@loggedUserId or exists (select p.GroupId from TbUserAssignedToGroup p where p.UserId=@loggedUserId and p.GroupId = TP.GroupId)) And TP.IsCreated = 1) aa group by aa.ProjectStatus";
                            lstProjectStatus = _connection.Query<ProjectStatusDetail>(strQueryProjectSummary, new { wsid = WorkspaceID, loggedUserId = loggedUserId }).ToList();

                            for (var k = 0; k < lstProjectStatus.Count(); k++)
                            {
                                if (lstProjectStatus[k].StatusName.ToString() == "Archived")
                                {
                                    lstProjects[0].ArchivedCount = lstProjectStatus[k].StatusCunt;
                                    Mtotal += lstProjects[0].ArchivedCount;
                                }
                                if (lstProjectStatus[k].StatusName.ToString() == "Deploy")
                                {
                                    lstProjects[0].DeployCount = lstProjectStatus[k].StatusCunt;
                                    Mtotal += lstProjects[0].DeployCount;
                                }
                                if (lstProjectStatus[k].StatusName.ToString() == "Draft")
                                {
                                    lstProjects[0].DraftCount = lstProjectStatus[k].StatusCunt;
                                    Mtotal += lstProjects[0].DraftCount;
                                }
                                if (lstProjectStatus[k].StatusName.ToString() == "Completed")
                                {
                                    lstProjects[0].CompCount = lstProjectStatus[k].StatusCunt;
                                    Mtotal += lstProjects[0].CompCount;
                                }

                            }
                            lstProjects[0].ProjectSummaryCount = Mtotal;

                            // Task Summary //

                            lstProjects[0].AssignedList = GetMyTaskProjectWiseAssignedCount(WorkspaceID, loggedUserId);

                            List<CompletedList> lstCompletedList = new List<CompletedList>();
                            lstCompletedList = GetMyTaskProjectWiseCompletedCount(WorkspaceID, loggedUserId);

                            //lstProjects[0].CompletedCount = GetProjectUserComplete(0, WorkspaceID, loggedUserId);
                            int CompletedCount = 0;
                            List<ActiveList> lstActivelist = new List<ActiveList>();

                            if (lstProjects[0].AssignedList != null && lstProjects[0].AssignedList.Count > 0)
                            {
                                for (int asscount = 0; asscount < lstProjects[0].AssignedList.Count; asscount++)
                                {
                                    ActiveList objActiveList = new ActiveList();
                                    objActiveList.ActiveCunt = lstProjects[0].AssignedList[asscount].AssignedCunt;
                                    objActiveList.ProjectId = lstProjects[0].AssignedList[asscount].ProjectId;

                                    if (lstCompletedList != null && lstCompletedList.Count > 0)
                                    {
                                        for (int cmpCount = 0; cmpCount < lstCompletedList.Count; cmpCount++)
                                        {
                                            if (lstProjects[0].AssignedList[asscount].ProjectId == lstCompletedList[cmpCount].ProjectId)
                                            {
                                                CompletedCount += lstCompletedList[cmpCount].CompletedCount;
                                                objActiveList.ActiveCunt = (lstProjects[0].AssignedList[asscount].AssignedCunt - lstCompletedList[cmpCount].CompletedCount);
                                            }
                                        }
                                    }

                                    lstActivelist.Add(objActiveList);
                                }
                            }

                            lstProjects[0].CompletedCount = CompletedCount;
                            lstProjects[0].ActiveList = lstActivelist;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstProjects;
        }

        public List<UsersDetails> FillDashboardMyTeam(int ProjectId, int WorkspaceID)
        {

            List<UsersDetails> objUsersList = new List<UsersDetails>();
            List<AssignedList> AssignedUser = new List<AssignedList>();
            List<CompletedList> CompletedList = new List<CompletedList>();
            int CompletedTaskCount = 0;

            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT ifnull(P.GroupId,0)GroupId FROM  TbProject P left join TbGroup G on P.GroupId=G.GroupId where P.ProjectId=@ID";
                    var lstGrp = _connection.Query<int>(strQuery, new { ID = ProjectId }).FirstOrDefault();


                    String query = @"Select Distinct PT.UserID ID, U.Name,U.EmailId , WR.RoleId RoleID, WR.RoleName Role,P.ProjectCount,IFNULL(U.ProfileImage, '') AS ProfileImage,IFNULL(U.Organization,'') AS Organization From TbUser U 
                             Inner join (Select Distinct UserID from TbUserAssignedToGroup Where GroupID = @GroupId and IsDeleted =0) PT On PT.UserID =U.ID 
                             Left Join (Select W.UserID,W.RoleID,R.RoleName , IsDtd , W.IsActive From TbWorkspaceTeam W Inner Join TbRole R On R.RoleId=W.RoleId Where 
                             W.WorkspaceID=@WorkspaceID) WR  On WR.UserID=PT.UserID Left Join (Select Distinct AG.UserID , Count(P.ProjectId) ProjectCount from TbUserAssignedToGroup AG Inner Join TbGroup G 
                             On G.GroupId =AG.GroupId Left Join TbProject P On P.GroupId =G.GroupId Where G.WorkspaceID=@WorkspaceID Group By AG.UserID ) P On P.UserID=U.ID
                              Where U.IsActive=1 and U.IsDeleted=0 and U.IsRegisteredUser =1 and WR.IsDtd =0 and WR.IsActive = 1";

                    objUsersList = _connection.Query<UsersDetails>(query, new { WorkspaceID = WorkspaceID, GroupId = lstGrp }).ToList();


                    for (var k = 0; k < objUsersList.Count(); k++)
                    {
                        AssignedUser = GetMyTaskProjectWiseAssignedCount(WorkspaceID, objUsersList[k].ID); // GetProjectUserAssign(ProjectId, WorkspaceID, objUsersList[k].ID).ToList();
                        if (AssignedUser != null && AssignedUser.Count > 0)
                        {
                            //AssignedUser = AssignedUser.Where(x=> x.ProjectId = ProjectId)
                            AssignedUser = AssignedUser.Where(l => l.ProjectId == ProjectId).ToList();
                            if (AssignedUser != null && AssignedUser.Count > 0)
                            {
                                objUsersList[k].TaskCount = AssignedUser[0].AssignedCunt;
                            }
                            else
                            {
                                objUsersList[k].TaskCount = 0;
                            }
                        }
                        else
                        {
                            objUsersList[k].TaskCount = 0;
                        }

                        CompletedList = GetMyTaskProjectWiseCompletedCount(WorkspaceID, objUsersList[k].ID);

                        if (CompletedList != null && CompletedList.Count > 0)
                        {
                            CompletedList = CompletedList.Where(l => l.ProjectId == ProjectId).ToList();
                            if (CompletedList != null && CompletedList.Count > 0)
                            {
                                CompletedTaskCount = CompletedList.Sum(l => l.CompletedCount);
                            }
                            else
                            {
                                CompletedTaskCount = 0;
                            }
                        }
                        else
                        {
                            CompletedTaskCount = 0;
                        }

                        int CompleteTask = CompletedTaskCount;//GetProjectUserComplete(ProjectId, WorkspaceID, objUsersList[k].ID);
                        double ProgressCal = 0; double CompleteTaskDbl = 0.00; double AssignTaskDbl = 0.00;
                        if (objUsersList[k].TaskCount != 0)
                        {
                            CompleteTaskDbl = CompleteTask;
                            AssignTaskDbl = objUsersList[k].TaskCount;
                            ProgressCal = (CompleteTaskDbl * 100) / AssignTaskDbl;
                            if (ProgressCal > 100)
                            {
                                ProgressCal = 100;
                            }
                            objUsersList[k].UserProgress = Math.Round(ProgressCal);
                        }
                        else
                        {
                            objUsersList[k].UserProgress = 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return objUsersList;
        }


        public List<MyTaskList> FillDashboardMyTask(int WorkspaceID, int loggedUserId)
        {

            List<MyTaskList> MyTaskList = new List<MyTaskList>();
            string strQuery = string.Empty;
            String query = string.Empty;

            try
            {
                if (loggedUserId != 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        query = @"select distinct aa.SurveyName TaskName,aa.ProjectName,aa.CreatedDate AssignedDate,TP.PeriodicityType TaskFrequency,U.Name AssignedByUser,ifnull(BT.Name,'') Beneficiary,aa.TypeID,aa.ProjectId from (Select Distinct T.TypeID,TD.TaskDate,TD.ProjectId,TD.SurveyName,TD.CreatedDate,TD.ProjectName,TD.BeneficiaryTypeID,TD.ReportingFrequency From TaskSyncDB T Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID=  @loggeduserid ) TA On  T.TypeID = TA.TypeID Inner Join (Select Distinct A.BeneficiaryTypeID,A.SurveyName,A.CreatedDate,A.ProjectName,A.ProjectId,A.SurveyID TypeID ,ReportingFrequency,TaskDate,EndDate From ( " +
                                    " Select S.BeneficiaryTypeID,S.SurveyName,S.CreatedDate,P.ProjectName,P.ProjectId,S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate, Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate,Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate From TbProjectSurvey S Inner Join TbProject P On P.ProjectId= S.ProjectId Left Join (Select SurveyID,TrackDate from TbProjectSurveyTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1) A " +
                                    " Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID inner join TbUser U on U.ID = T.CreatedBy Where ifnull(T.IsActive,0) =1 And binary T.Type= 'Survey' AND T.Command ='ADD'  And T.ID >  0   And T.WorkspaceID =  @wsid and    TD.TypeID NOT IN (Select MSK.TypeID FROM TbMyTask MSK JOIN TbProjectSurveyUser OTASN ON MSK.WFLevel = 0 AND MSK.TypeID = OTASN.SurveyID WHERE  OTASN.IsDeleted = 0 AND (MSK.IsActive = 1 or MSK.IsActive = 0) AND  MSK.WFLevelStatus = 'Completed'  AND MSK.Type = 'Survey'	AND MSK.TypeID = TypeID AND (case when TD.ReportingFrequency != 9 then MSK.TaskDate = DATE_FORMAT(TD.TaskDate, '%Y-%m-%d') else TD.TypeID=MSK.TypeID end)) UNION ALL select distinct t.TypeID,t.TaskDate,P.ProjectId,S.SurveyName,S.CreatedDate, " +
                                    " P.ProjectName,S.BeneficiaryTypeID,S.ReportingFrequency from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = S.ProjectId Inner Join  (Select Distinct w1.WorkflowID,w1.WfNoOfLevels From  TbProjectWorkflowDetails w inner join TbProjectWorkflow w1 on w.WorkflowID = w1.WorkflowID where w.IsDeleted =0 and w1.IsDeleted =0 And w.UserID=  @loggeduserid ) TA On  S.WorkflowID = TA.WorkflowID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1 and TA.WfNoOfLevels != t.WFLevel and S.ReportingFrequency != 9 and P.WSID = @wsid and case when (select max(WFLevel) from TbMyTask where   t.TypeID = TypeID AND TaskDate = " +
                                    " t.TaskDate) = TA.WfNoOfLevels then not exists (select SyncTaskID from TbMyTask where t.TypeID = TypeID AND TaskDate = t.TaskDate) else not exists (select SyncTaskID from TbMyTask where t.SyncTaskID = ifnull(DataCollectionSyncDBID,0) and t.TypeID = TypeID and TA.WfNoOfLevels = WFLevel AND TaskDate = t.TaskDate) end UNION ALL select t.TypeID,t.TaskDate,P.ProjectId,S.SurveyName,S.CreatedDate,P.ProjectName,S.BeneficiaryTypeID,S.ReportingFrequency from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = " +
                                    " S.ProjectId Left Join(Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted = 0)   W On W.WorkflowID = S.WorkflowID Where W.WFLevel = (t.WFLevel + 1) And P.WSID And W.AssgUserID = @loggeduserid AND S.ReportingFrequency = 9 AND t.SyncTaskID NOT IN(select ParentSyncDBID from TbMyTask where WFLevel = (t.WFLevel + 1) and WFLevelStatus = 'Completed' and TaskDate = t.TaskDate) AND ifnull(P.IsActive,0) = 1 AND ifnull(S.IsActive,0) = 1 and ifnull(P.IsDeleted,0) = 0  and P.WSID = @wsid ) aa inner join TbPeriodicity TP on TP.PeriodicityId = aa.ReportingFrequency left outer join TbProjectBeneficiaryType BT " +
                                    " on BT.BeneficiaryTypeID = aa.BeneficiaryTypeID inner join TbUser U on U.ID = @loggeduserid";

                        MyTaskList = _connection.Query<MyTaskList>(query, new { wsid = WorkspaceID, loggeduserid = loggedUserId }).ToList();


                        for (var k = 0; k < MyTaskList.Count(); k++)
                        {
                            int SubmittedCunt = 0;

                            string strSubmitted = @"select ifnull(count(bb.SyncDBID),0) Submitted from (SELECT distinct SyncTaskID as SyncDBID,UserID,Type,TypeID,WFLevel,WFLevelStatus, TMT.TaskSubmittedDate as CreatedDate,TMT.TaskDate " +
                                                    ",PER.PeriodicityId,TMT.TaskSubmittedDate FROM TbMyTask TMT inner join TbProjectSurvey TPA on TMT.TypeID = TPA.SurveyID and Type = 'Survey' inner join TbProject P on TPA.ProjectId = P.ProjectId " +
                                                    " inner join TbPeriodicity PER ON PER.PeriodicityId = TPA.ReportingFrequency where P.ProjectId = @ProjectId and ifnull(TMT.DataCollectionSyncDBID,0) = 0 and WFLevelStatus = 'Completed' and NOT EXISTS (select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' " +
                                                    " and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggeduserid and P.WSID = @wsid) bb where bb.TypeID = @TypeId";

                            SubmittedCunt = _connection.Query<int>(strSubmitted, new { wsid = WorkspaceID, loggeduserid = loggedUserId, ProjectId = MyTaskList[k].ProjectId, TypeId = MyTaskList[k].TypeID }).FirstOrDefault();
                            MyTaskList[k].SubmittedCount = SubmittedCunt;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
            }

            return MyTaskList;
        }


        public List<AssignedList> GetProjectUserAssign(int ProjectId, int WorkspaceID, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                List<AssignedList> AssignedUser = new List<AssignedList>();
                string strAssignedUserQuery = "";

                try
                {
                    if (loggedUserId != 0)
                    {
                        if (ProjectId == 0)
                        {
                            strAssignedUserQuery = @"select ifnull(count(aa.TypeID),0) AssignedCunt,aa.ProjectId from (Select Distinct T.TypeID,TD.TaskDate,TD.ProjectId From TaskSyncDB T Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID=  @loggeduserid ) TA On  T.TypeID = TA.TypeID Inner Join (Select Distinct  A.ProjectId,A.SurveyID TypeID ,ReportingFrequency,TaskDate,EndDate From (Select P.ProjectId,S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate,  Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate " +
                                                    ",Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate From TbProjectSurvey S Inner Join TbProject P On P.ProjectId = S.ProjectId Left Join(Select SurveyID, TrackDate from TbProjectSurveyTrackDate Where Trackstatus = 0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted = 0 and P.IsActive = 1 and P.ProjectStatus = 'Deploy' and S.IsActive = 1) A Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID   inner join TbUser U on U.ID = T.CreatedBy Where ifnull(T.IsActive,0) = 1 And binary T.Type = 'Survey' AND T.Command = 'ADD' And T.ID > 0   And T.WorkspaceID = @wsid " +
                                                    " UNION ALL select t.TypeID,t.TaskDate,P.ProjectId from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = S.ProjectId Inner Join  (Select Distinct w1.WorkflowID,w1.WfNoOfLevels,w.WfLevel From  TbProjectWorkflowDetails w inner join TbProjectWorkflow w1 on w.WorkflowID = w1.WorkflowID where w.IsDeleted =0 and w1.IsDeleted =0 And w.UserID= @loggeduserid ) TA On  S.WorkflowID = TA.WorkflowID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1 and (case when (TA.WfLevel = 1 and TA.WfNoOfLevels > TA.WfLevel) then (t.WFLevel = (TA.WfLevel-1)) else (t.WFLevel = TA.WfLevel) end)  AND t.SyncTaskID NOT IN (select ParentSyncDBID from TbMyTask where  WFLevel = (t.WFLevel +1) and WFLevelStatus = 'Completed' and TaskDate  = t.TaskDate " +
                                                    " and TA.WfNoOfLevels = WFLevel)  and P.WSID = @wsid) aa group by aa.ProjectId";
                        }
                        else
                        {
                            strAssignedUserQuery = @"select ifnull(count(aa.TypeID),0) AssignedCunt,aa.ProjectId from (Select Distinct T.TypeID,TD.TaskDate,TD.ProjectId From TaskSyncDB T Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID=  @loggeduserid ) TA On  T.TypeID = TA.TypeID Inner Join (Select Distinct  A.ProjectId,A.SurveyID TypeID ,ReportingFrequency,TaskDate,EndDate From (Select P.ProjectId,S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate,  Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate " +
                                                   ",Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate From TbProjectSurvey S Inner Join TbProject P On P.ProjectId = S.ProjectId Left Join(Select SurveyID, TrackDate from TbProjectSurveyTrackDate Where Trackstatus = 0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted = 0 and P.IsActive = 1 and P.ProjectStatus = 'Deploy' and S.IsActive = 1) A Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID   inner join TbUser U on U.ID = T.CreatedBy Where ifnull(T.IsActive,0) = 1 And binary T.Type = 'Survey' AND T.Command = 'ADD' And T.ID > 0   And T.WorkspaceID = @wsid " +
                                                   " UNION ALL select t.TypeID,t.TaskDate,P.ProjectId from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = S.ProjectId Inner Join  (Select Distinct w1.WorkflowID,w1.WfNoOfLevels,w.WfLevel From  TbProjectWorkflowDetails w inner join TbProjectWorkflow w1 on w.WorkflowID = w1.WorkflowID where w.IsDeleted =0 and w1.IsDeleted =0 And w.UserID= @loggeduserid ) TA On  S.WorkflowID = TA.WorkflowID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1 and (case when (TA.WfLevel = 1 and TA.WfNoOfLevels > TA.WfLevel) then (t.WFLevel = (TA.WfLevel-1)) else (t.WFLevel = TA.WfLevel) end) AND t.SyncTaskID NOT IN (select ParentSyncDBID from TbMyTask where  WFLevel = (t.WFLevel +1) and WFLevelStatus = 'Completed' and TaskDate  = t.TaskDate " +
                                                   " and TA.WfNoOfLevels = WFLevel)  and P.WSID = @wsid) aa where aa.ProjectId = @ProjectId  group by aa.ProjectId";

                        }
                        AssignedUser = _connection.Query<AssignedList>(strAssignedUserQuery, new { wsid = WorkspaceID, loggeduserid = loggedUserId, ProjectId = ProjectId }).ToList();
                    }
                }
                catch (Exception ex)
                {
                }

                return AssignedUser;


            }
        }


        public List<ActiveList> GetProjectUserActive(int WorkspaceID, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                List<ActiveList> ActiveUser = new List<ActiveList>();
                string strActiveUserQuery = "";
                try
                {
                    if (loggedUserId != 0)
                    {
                        strActiveUserQuery = @"select ifnull(count(aa.TypeID),0)ActiveCunt,aa.ProjectId from (Select Distinct T.TypeID,TD.TaskDate,TD.ProjectId,TD.SurveyName,TD.CreatedDate,TD.ProjectName,TD.BeneficiaryTypeID,TD.ReportingFrequency From TaskSyncDB T Inner Join  (Select Distinct SurveyID TypeID , UserID  AssgUserID  From  TbProjectSurveyUser	Where IsDeleted =0 And UserID=  @loggeduserid ) TA On  T.TypeID = TA.TypeID Inner Join (Select Distinct A.BeneficiaryTypeID,A.SurveyName,A.CreatedDate,A.ProjectName,A.ProjectId,A.SurveyID TypeID ,ReportingFrequency,TaskDate,EndDate From ( " +
                                                  " Select S.BeneficiaryTypeID,S.SurveyName,S.CreatedDate,P.ProjectName,P.ProjectId,S.SurveyID ,ReportingFrequency ,Case When (ReportingFrequency =1 OR ReportingFrequency =9 ) then date(now()) Else STrack.TrackDate End AS TaskDate, Case When S.StartDate is null Then P.StartDate Else S.StartDate End As StartDate,Case When S.EndDate is null Then P.EndDate Else S.EndDate End As EndDate From TbProjectSurvey S Inner Join TbProject P On P.ProjectId= S.ProjectId Left Join (Select SurveyID,TrackDate from TbProjectSurveyTrackDate Where Trackstatus =0 And TrackDate <= date(now())) STrack On STrack.SurveyID = S.SurveyID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1) A " +
                                                  " Where TaskDate Between StartDate And EndDate And TaskDate Is not null )  TD On  T.TypeID = TD.TypeID inner join TbUser U on U.ID = T.CreatedBy Where ifnull(T.IsActive,0) =1 And binary T.Type= 'Survey' AND T.Command ='ADD'  And T.ID >  0   And T.WorkspaceID =  @wsid and    TD.TypeID NOT IN (Select MSK.TypeID FROM TbMyTask MSK JOIN TbProjectSurveyUser OTASN ON MSK.WFLevel = 0 AND MSK.TypeID = OTASN.SurveyID WHERE  OTASN.IsDeleted = 0 AND (MSK.IsActive = 1 or MSK.IsActive = 0) AND  MSK.WFLevelStatus = 'Completed'  AND MSK.Type = 'Survey'	AND MSK.TypeID = TypeID AND (case when TD.ReportingFrequency != 9 then MSK.TaskDate = DATE_FORMAT(TD.TaskDate, '%Y-%m-%d') else TD.TypeID=MSK.TypeID end)) UNION ALL select distinct t.TypeID,t.TaskDate,P.ProjectId,S.SurveyName,S.CreatedDate," +
                                                  " P.ProjectName,S.BeneficiaryTypeID,S.ReportingFrequency from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = S.ProjectId Inner Join  (Select Distinct w1.WorkflowID,w1.WfNoOfLevels From  TbProjectWorkflowDetails w inner join TbProjectWorkflow w1 on w.WorkflowID = w1.WorkflowID where w.IsDeleted =0 and w1.IsDeleted =0 And w.UserID=  @loggeduserid ) TA On  S.WorkflowID = TA.WorkflowID where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and S.IsActive = 1 and TA.WfNoOfLevels != t.WFLevel and S.ReportingFrequency != 9 and P.WSID = @wsid and case when (select max(WFLevel) from TbMyTask where   t.TypeID = TypeID AND TaskDate = " +
                                                  " t.TaskDate) = TA.WfNoOfLevels then not exists (select SyncTaskID from TbMyTask where t.TypeID = TypeID AND TaskDate = t.TaskDate) else not exists (select SyncTaskID from TbMyTask where t.SyncTaskID = ifnull(DataCollectionSyncDBID,0) and t.TypeID = TypeID and TA.WfNoOfLevels = WFLevel AND TaskDate = t.TaskDate) end UNION ALL select t.TypeID,t.TaskDate,P.ProjectId,S.SurveyName,S.CreatedDate,P.ProjectName,S.BeneficiaryTypeID,S.ReportingFrequency from TbMyTask t inner join TbProjectSurvey S on t.TypeID = S.SurveyID inner join TbProject P on P.ProjectId = " +
                                                  " S.ProjectId Left Join(Select WorkflowID, WFLevel, UserID AssgUserID From TbProjectWorkflowDetails Where IsDeleted = 0)   W On W.WorkflowID = S.WorkflowID Where W.WFLevel = (t.WFLevel + 1) And P.WSID And W.AssgUserID = @loggedUserId AND S.ReportingFrequency = 9 AND t.SyncTaskID NOT IN(select ParentSyncDBID from TbMyTask where WFLevel = (t.WFLevel + 1) and WFLevelStatus = 'Completed' and TaskDate = t.TaskDate) AND ifnull(P.IsActive,0) = 1 AND ifnull(S.IsActive,0) = 1 and ifnull(P.IsDeleted,0) = 0  and P.WSID = @wsid ) aa group by aa.ProjectId";

                        ActiveUser = _connection.Query<ActiveList>(strActiveUserQuery, new { wsid = WorkspaceID, loggeduserid = loggedUserId }).ToList();
                    }
                }

                catch (Exception ex)
                {
                }
                return ActiveUser;


            }

        }


        public int GetProjectUserComplete(int ProjectId, int WorkspaceID, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                int CompletedUser = 0;
                string strCompletedUserQuery = "";
                try
                {
                    if (loggedUserId != 0)
                    {
                        if (ProjectId == 0)
                        {
                            strCompletedUserQuery = @"select ifnull(count(bb.SyncDBID),0) Completed from (SELECT distinct SyncTaskID as SyncDBID,UserID,Type,TypeID,WFLevel,WFLevelStatus, TMT.TaskSubmittedDate as CreatedDate,TMT.TaskDate " +
                                                           ",PER.PeriodicityId,TMT.TaskSubmittedDate FROM TbMyTask TMT inner join TbProjectSurvey TPA on TMT.TypeID = TPA.SurveyID and Type = 'Survey' inner join TbProject P on TPA.ProjectId = P.ProjectId " +
                                                           " inner join TbPeriodicity PER ON PER.PeriodicityId = TPA.ReportingFrequency where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and TMT.WFLevelStatus = 'Completed' and NOT EXISTS (select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' " +
                                                           " and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggeduserid and P.WSID = @wsid) bb ";

                            CompletedUser = _connection.Query<int>(strCompletedUserQuery, new { wsid = WorkspaceID, loggeduserid = loggedUserId }).FirstOrDefault();

                        }
                        else
                        {
                            strCompletedUserQuery = @"select ifnull(count(bb.SyncDBID),0) Completed from (SELECT distinct SyncTaskID as SyncDBID,UserID,Type,TypeID,WFLevel,WFLevelStatus, TMT.TaskSubmittedDate as CreatedDate,TMT.TaskDate " +
                                                          ",PER.PeriodicityId,TMT.TaskSubmittedDate FROM TbMyTask TMT inner join TbProjectSurvey TPA on TMT.TypeID = TPA.SurveyID and Type = 'Survey' inner join TbProject P on TPA.ProjectId = P.ProjectId " +
                                                          " inner join TbPeriodicity PER ON PER.PeriodicityId = TPA.ReportingFrequency where P.IsDeleted=0 and P.IsActive = 1 and P.ProjectStatus='Deploy' and TMT.WFLevelStatus = 'Completed' and NOT EXISTS (select s.TypeID from TbMyTask s where s.TypeID = TMT.TypeID and s.WFLevelStatus = 'Rejected' " +
                                                          " and TMT.SyncTaskID = s.DataCollectionSyncDBID) and UserID = @loggeduserid and P.WSID = @wsid and P.ProjectId = @Projectid) bb ";

                            CompletedUser = _connection.Query<int>(strCompletedUserQuery, new { wsid = WorkspaceID, loggeduserid = loggedUserId, Projectid = ProjectId }).FirstOrDefault();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return CompletedUser;


            }
        }

        #region GetMyTaskProjectWiseAssignedCount
        /// <summary>
        /// GetMyTaskProjectWiseAssignedCount
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<AssignedList> GetMyTaskProjectWiseAssignedCount(int WorkspaceID, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                List<AssignedList> AssignedUser = new List<AssignedList>();

                if (loggedUserId != 0)
                {
                    AssignedUser = _connection.Query<AssignedList>("sp_GetMyTaskAssignedCount", new { varLoggedUserID = loggedUserId, varWorkspaceID = WorkspaceID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                }

                return AssignedUser;
            }
        }
        #endregion GetMyTaskProjectWiseAssignedCount


        #region GetMyTaskProjectWiseCompletedCount
        /// <summary>
        /// GetMyTaskProjectWiseCompletedCount
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public List<CompletedList> GetMyTaskProjectWiseCompletedCount(int WorkspaceID, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                List<CompletedList> CompletedList = new List<CompletedList>();


                if (loggedUserId != 0)
                {

                    CompletedList = _connection.Query<CompletedList>("sp_GetMyTaskCompletedCount", new { varLoggedUserID = loggedUserId, varWorkspaceID = WorkspaceID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                }

                return CompletedList;
            }
        }
        #endregion GetMyTaskProjectWiseCompletedCount


    }
}
