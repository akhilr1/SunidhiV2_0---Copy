using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using SunidhiV2_0.Code.Common;
using Dapper;

namespace SunidhiV2_0.Code.DAL
{
    public class FinanceDAL
    {
        protected IDbConnection _connection;
        #region SearchFinance
        public IEnumerable<Project> GetProjectListOfFinance(int loggedUserId,int WorkspaceID)
        {
            List<Project> lstProjects = new List<Project>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT distinct P.ProjectId,ProjectName,StartDate,EndDate,Budget,P.IsActive,P.Code,P.InitiativeId,ProjectStatus,IFNULL(sum(DonorBudgetAmount),0) as FundedAmount, InitiativeName as Initiative,(select sum(TransactionAmount)  from TbLedgerBook LB  Left join  TbAccount
                                on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId) UtilizedFund,IFNULL((select Round((sum(TransactionAmount)*100)/FundedAmount)  
                                from TbLedgerBook LB  Left join  TbAccount on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) FundUtilized FROM  TbProject P
                                inner join TbInitiative TI on P.InitiativeId=TI.InitiativeId left join TbProjectBudgetAndFundDetails MV on P.ProjectId=MV.ProjectId  where 
                                P.IsDeleted=0 and P.IsActive=1 and GroupId in  (select GroupId from TbUserAssignedToGroup UATG inner join TbWorkspaceTeam W On W.UserID = UATG.UserId Inner join 
                                (SELECT RoleId  FROM TbRoleBasedAccessScreens Where  AccessId =3 And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='My Finance'))R
                                On R.RoleId = W.RoleID where W.UserID=@loggedUserId) and P.WSID=@WorkspaceID 
                                Group By P.ProjectId,ProjectName,StartDate,EndDate,Budget,P.IsActive,P.Code,P.InitiativeId Order by P.ProjectId Desc";


                lstProjects = _connection.Query<Project>(strQuery, new { loggedUserId = loggedUserId, WorkspaceID= WorkspaceID }).ToList();
            }
            return lstProjects;
        }
        #endregion SearchFinance
        #region BudgetFund
        public string UpdateDonarFund(int ProjectID,int BudgetFundID, int UserID, double BudgetAmount, int AccountID, int loggedUserId, string UserName, string AccountName)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails  Set DonorBudgetAmount =@BudgetAmount , AccountId =@AccountID, " +
                                " UpdatedDate =now(),UpdatedBy=@loggedUserId,UserID= @UserID  where ProjectID =@ProjectID And ID=@BudgetFundID ;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        loggedUserId = loggedUserId,
                        UserID = UserID,
                        BudgetAmount = BudgetAmount,
                        AccountID = AccountID,
                        BudgetFundID= BudgetFundID
                    });

                    AddAuditLog(ProjectID, "EDIT", BudgetFundID, "ID", "Updated Funded Donor " + UserName + " Amount " + BudgetAmount + " Account Name " + AccountName, loggedUserId);

                }
                catch (Exception ex)
                {
                    retStatus = "Failed while updating budget donor details...";
                    throw ex;
                }
            }
            return retStatus;
        }
        public string RemoveBudgetUser(int ProjectID, int BudgetFundID,string DonorName,  int loggedUserId)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails Set IsDeleted =1 ,UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID And ID=@BudgetFundID ;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        loggedUserId = loggedUserId,
                        BudgetFundID = BudgetFundID
                    });

                    AddAuditLog(ProjectID, "REMOVE", BudgetFundID, "ID", "Removed Funded Donor " + DonorName, loggedUserId);

                }
                catch (Exception ex)
                {
                    retStatus = "Failed while deleting budget donor details...";
                    throw ex;
                }
            }
            return retStatus;
        }

        #endregion BudgetFund
        #region BudgetPlan


        public int UpdateBudgetPlan(int ProjectId,int BudgetHeadID, String BudgetHead, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbBudgetPlan Set BudgetHead=@BudgetHead, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where ID=@BudgetHeadID";
                _connection.Execute(query, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, BudgetHead= BudgetHead, BudgetHeadID = BudgetHeadID });

                AddAuditLog(ProjectId, "EDIT", BudgetHeadID, "ID", "Updated Budget Head Name as " + BudgetHead, loggedUserId);

            }
            return id;
        }
        public int UpdateBudgetPlanitem( int BudgetItemID, String ItemName,string Amount, int ProjectId, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbBudgetPlanItemDetails Set ItemName=@ItemName,Amount=@Amount, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where ID=@BudgetItemID";
                _connection.Execute(query, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, ItemName = ItemName, Amount= Amount, BudgetItemID = BudgetItemID });

                AddAuditLog(ProjectId, "EDIT", BudgetItemID, "ID", "Updated Budget SubHead '" + ItemName + "'and Amount " + Amount, loggedUserId);

            }
            return id;
        }
  
   
        public int RemoveBudgetPlan(int BudgetHeadID,string BudgetPlanHeader, int ProjectId, int loggedUserId)
        {
            string strQuery = string.Empty;
            string Query = string.Empty;
            var paymentId = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                Query = @"SELECT PaymentId FROM TbLedgerBook where BudgetHeadID =@BudgetItemID";
                 paymentId = _connection.Query<int>(Query, new {BudgetItemID = BudgetHeadID }).FirstOrDefault();

                if (paymentId == 0)
                {
                    strQuery = @"Update TbBudgetPlan Set IsDeleted=1, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where ID=@BudgetHeadID";
                    _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, BudgetHeadID = BudgetHeadID });

                    AddAuditLog(ProjectId, "REMOVE", BudgetHeadID, "ID", "Removed Budget Head -'" + BudgetPlanHeader + "'", loggedUserId);
                }
                return paymentId;
            }
        }
        public int RemoveBudgetPlanItems(int BudgetItemID,string ItemName,int ProjectId, int loggedUserId)
        {
            string strQuery = string.Empty;
            string Query = string.Empty;
            var paymentId = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                Query = @"SELECT PaymentId FROM TbLedgerBook where BudgetItemID =@BudgetItemID";
                paymentId = _connection.Query<int>(Query, new { BudgetItemID = BudgetItemID }).FirstOrDefault();
                if (paymentId == 0)
                {

                    strQuery = @"Update TbBudgetPlanItemDetails Set IsDeleted=1, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where ID=@BudgetItemID";
                    _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, BudgetItemID = BudgetItemID });

                    AddAuditLog(ProjectId, "REMOVE", BudgetItemID, "ID", "Removed Budget SubHead  -'" + ItemName + "'", loggedUserId);
                }
                return paymentId;
            }
        }
        public IEnumerable<BudgetPlan> FetchBudgetPlan(int ProjectId)
        {
            List<BudgetPlan> lstProjects = new List<BudgetPlan>();
            string strQuery = string.Empty;
            var retStatus = "";

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_FetchBudgetPlan";
                    lstProjects = _connection.Query<BudgetPlan>(strQuery, new
                    {
                        ProjectId = ProjectId
                    }, commandType: CommandType.StoredProcedure).ToList();
                }

            }

            catch (Exception ex)
            {
                retStatus = "Failed while saving new project...";
                throw ex;
            }
            return lstProjects;
        }
 
        #endregion BudgetPlan
        #region FinanceActivity

        public List<ProjectActivity> GetFinanceActivityList(int projectID)
        {
            List<ProjectActivity> objProjectActivityList = new List<ProjectActivity>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                //Get all parent activities here
                //query = @"select ActivityId,ProjectId,ParentActivityId,ActivityName,Description,StartDate,EndDate,WeightagePercentage,ActivityType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,
                //SurveyLogic,SurveyTarget,IsPaymentLinked,PaymentLinkedBudget,Periodicity,FinanceWorkflowId as WorkflowId,(select WfName from TbFinanceWorkflow where WorkflowID = FinanceWorkflowId) Workflow
                //  from TbProjectActivity where IsDeleted = @isdeleted and ProjectId = @projid and ParentActivityId = @parentactid and IsPaymentLinked = 1 Order By ActivityId Desc";

                query = @"select ActivityId,ProjectId,ParentActivityId,ActivityName,Description,StartDate,EndDate,WeightagePercentage,ActivityType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,
                            SurveyLogic,SurveyTarget,IsPaymentLinked,PaymentLinkedBudget,Periodicity,FinanceWorkflowId as WorkflowId,(select WfName from TbFinanceWorkflow where WorkflowID=FinanceWorkflowId) Workflow
                            from TbProjectActivity t where t.IsDeleted=@isdeleted and t.ProjectId=@projid and t.IsPaymentLinked=1 and t.ParentActivityId not in (select ActivityId from TbProjectActivity where IsDeleted=@isdeleted and ProjectId=@projid and IsPaymentLinked=1) Order By t.ActivityId Desc";

                objProjectActivityList = _connection.Query<ProjectActivity>(query, new { isdeleted = 0, projid = projectID, parentactid = 0 }).ToList();

                foreach (ProjectActivity actItem in objProjectActivityList)
                {
                    //Get all SubActivities of this parent to one sub-level
                    actItem.children = GetSubFinanceActivityListByID(actItem.ActivityId);

                    //Get Users List
                    actItem.ActivityUsers = new List<User>();
                    query = @"select * from TbUser where ID in 
                            ( select distinct UserId from TbProjectActivityToUsers where IsDeleted <>1 and ActivityId=@actid )  Order By ID Desc";
                    actItem.ActivityUsers = _connection.Query<User>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Expense tag List
                    actItem.ExpenseTags = new List<Tags>();
                    query = @"select * from TbProjectExpenseTags where TagID in 
                            ( select distinct ExpenseTagID from TbProjectActivityToExpenseTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";
                    actItem.ExpenseTags = _connection.Query<Tags>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Activity tag List
                    actItem.ActivityTags = new List<Tags>();
                    query = @"select * from TbProjectTags where TagID in 
                            ( select distinct ActivityTagID from TbProjectActivityToTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";
                    actItem.ActivityTags = _connection.Query<Tags>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Periodicity parameters
                    DateTime? RptDate = null;
                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbProjectActivityTrackDate WHERE ActivityId = @actid order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { actid = actItem.ActivityId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }

                    if (RptDate != null)
                    {
                        switch (actItem.Periodicity)
                        {
                            case (int)CommonActivity.ReportingFrequency.Once:
                                actItem.PeriodicityOnceDate = RptDate.Value;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Weekly:
                                actItem.PeriodicityWeek = (int)RptDate.Value.DayOfWeek;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Monthly:
                                actItem.PeriodicityMonthDay = RptDate.Value.Day;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Quarterly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Yearly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                        }
                    }
                }
            }





            return objProjectActivityList;
        }

        public List<ProjectActivity> GetSubFinanceActivityListByID(int activityID)
        {
            List<ProjectActivity> objSubProjectActivityList = new List<ProjectActivity>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"select ActivityId,ProjectId,ParentActivityId,ActivityName,Description,StartDate,EndDate,WeightagePercentage,ActivityType,SurveyId,SurveySectionId,SurveyQuestionId,SurveyResponseId,
                            SurveyLogic,SurveyTarget,IsPaymentLinked,PaymentLinkedBudget,Periodicity,FinanceWorkflowId as WorkflowId,(select WfName from TbFinanceWorkflow where WorkflowID=FinanceWorkflowId) Workflow from TbProjectActivity 
                            where IsDeleted=@isdeleted and ParentActivityId=@parentactid and IsPaymentLinked=1 Order By ActivityId Desc";
                objSubProjectActivityList = _connection.Query<ProjectActivity>(query, new { isdeleted = 0, parentactid = activityID }).ToList();

                foreach (ProjectActivity actItem in objSubProjectActivityList)
                {
                    //Get Users List
                    actItem.ActivityUsers = new List<User>();
                    query = @"select * from TbUser where ID in 
                            ( select distinct UserId from TbProjectActivityToUsers where IsDeleted <>1 and ActivityId=@actid )  Order By ID Desc";
                    actItem.ActivityUsers = _connection.Query<User>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Expense tag List
                    actItem.ExpenseTags = new List<Tags>();
                    query = @"select * from TbProjectExpenseTags where TagID in 
                            ( select distinct ExpenseTagID from TbProjectActivityToExpenseTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";
                    actItem.ExpenseTags = _connection.Query<Tags>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Activity tag List
                    actItem.ActivityTags = new List<Tags>();
                    query = @"select * from TbProjectTags where TagID in 
                            ( select distinct ActivityTagID from TbProjectActivityToTags where IsDeleted <>1 and ActivityId=@actid )  Order By TagID Desc";
                    actItem.ActivityTags = _connection.Query<Tags>(query, new { actid = actItem.ActivityId }).ToList();

                    //Get Periodicity parameters
                    DateTime? RptDate = null;
                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbProjectActivityTrackDate WHERE ActivityId = @actid order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { actid = actItem.ActivityId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }

                    if (RptDate != null)
                    {
                        switch (actItem.Periodicity)
                        {
                            case (int)CommonActivity.ReportingFrequency.Once:
                                actItem.PeriodicityOnceDate = RptDate.Value;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Weekly:
                                actItem.PeriodicityWeek = (int)RptDate.Value.DayOfWeek;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Monthly:
                                actItem.PeriodicityMonthDay = RptDate.Value.Day;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Quarterly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                            case (int)CommonActivity.ReportingFrequency.Yearly:
                                actItem.PeriodicityMonthDateDay = RptDate.Value.Day;
                                actItem.PeriodicityMonth = RptDate.Value.Month;
                                break;
                        }
                    }
                }
            }

            return objSubProjectActivityList;
        }


        public void UpdateFinanceWFToActivity(int ProjectID, int ActivityId,string ActivityName,int FinanceWorkflowId, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQuery = "Update  TbProjectActivity SET FinanceWorkflowId =@FinanceWorkflowId ,UpdatedDate = @updatedDate ,UpdatedBy =@loggeduserid ";
                strQuery += " Where ActivityId=@actid ";
                _connection.Execute(strQuery, new { actid = ActivityId, FinanceWorkflowId = FinanceWorkflowId, updatedDate = DateTime.Now, loggeduserid = loggedUserId });

                string Query = "Update  TaskSyncDB SET FinanceWorkflowID =@FinanceWorkflowId  ";
                 Query += " Where TypeID=@actid and Type='Activity' ";
                _connection.Execute(Query, new { actid = ActivityId, FinanceWorkflowId = FinanceWorkflowId });

                AddAuditLog(ProjectID, "EDIT", ActivityId, "ActivityId", "Updated  workflow assigned to Activity  " + ActivityName, loggedUserId);

            }
            return ;
        }
        public IEnumerable<Finance> GetBudgetHeadDetails(int ProjectId)
        {
            List<Finance> lstItems = new List<Finance>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct ID as BudgetHeadID, BudgetHead as BudgetHeadName From TbBudgetPlan where ProjectId=@ProjectId and IsDeleted = 0  Order By BudgetHead ";
                lstItems = _connection.Query<Finance>(strQuery, new { ProjectId = ProjectId}).ToList();
            }
            return lstItems;
        }
        public IEnumerable<BudgetDetails> GetBudgetItemDetails(int BudgetHeadID)
        {
            List<BudgetDetails> lstItems = new List<BudgetDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct ID as BudgetItemID, ItemName From TbBudgetPlanItemDetails Where BudgetHeadID =@BudgetHeadID and IsDeleted =0 Order By ItemName ";
                lstItems = _connection.Query<BudgetDetails>(strQuery, new { BudgetHeadID = BudgetHeadID }).ToList();
            }
            return lstItems;
        }
        public IEnumerable<Finance> GetPaymentBudgetHead(int ProjectId)
        {
            List<Finance> lstItems = new List<Finance>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct BudgetHead as BudgetHeadName, sum(TransactionAmount) as HeadAmount From TbLedgerBook LB inner join TbBudgetPlan BP 
                              on LB.BudgetHeadID=BP.ID Where LB.ProjectId =@ProjectId and LB.IsDeleted= 0 group by BudgetHead";
                lstItems = _connection.Query<Finance>(strQuery, new { ProjectId = ProjectId }).ToList();
            }
            return lstItems;
        }
        public List<LedgerBook> GetPaymentListOfActivicty(int projectID, int ActivityId)
        {
            List<LedgerBook> objPaymentList = new List<LedgerBook>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
              
                    query = @"select * from TbLedgerBook where IsDeleted=0 and ProjectId=@projid and ActivityId=@ActivityId Order By PaymentId Desc";
                    objPaymentList = _connection.Query<LedgerBook>(query, new { projid = projectID, ActivityId= ActivityId }).ToList();

                foreach (LedgerBook payItem in objPaymentList)
                {
                    payItem.ExpenseTags = new List<ProjectExpenseTags>();
                    query = @"select TagID , TagName from TbProjectExpenseTags where TagID in 
                            ( select ExpenseTagID from TbLedgerBookToExpenseTags where IsDeleted <>1 and PaymentId=@payid)  Order By TagID Desc";
                    payItem.ExpenseTags = _connection.Query<ProjectExpenseTags>(query, new { payid = payItem.PaymentId }).ToList();
                }
            }

            return objPaymentList;
        }

        #endregion FinanceActivity
        #region FinanceWorkflow

        public List<Workflow> GetWorkflowsListToactivity(int ProjectId, int WorkspaceID)
        {
            List<Workflow> objWorkflowsList = new List<Workflow>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Select WorkflowID ,WfName ,WfDescription ,WfNoOfLevels  From TbFinanceWorkflow 
                            Where WorkspaceID =@wsid and  ProjectId=@ProjectId and IsDeleted=0 ";
                objWorkflowsList = _connection.Query<Workflow>(query, new { ProjectId = ProjectId, wsid = WorkspaceID }).ToList();

                foreach (Workflow wfItem in objWorkflowsList)
                {
                    wfItem.WfLevelDetails = new List<WorkflowLevelDetails>();
                    for (int eachLevel = 1; eachLevel <= wfItem.WfNoOfLevels; eachLevel++)
                    {
                        var tempitem = new WorkflowLevelDetails();
                        tempitem.WfLevel = eachLevel;
                        query = @"select ID, Name, EmailId, IFNULL(ProfileImage, '') as ProfileImage from TbUser where ID in 
                                    (select UserID from TbFinancetWorkflowDetails where WorkflowID=@workflowid and IsDeleted=0 and WfLevel=@eachlevel)";
                        tempitem.usersList = _connection.Query<User>(query, new { workflowid = wfItem.WorkflowID, eachlevel = eachLevel }).ToList();

                        wfItem.WfLevelDetails.Add(tempitem);
                    }
                }
            }
            return objWorkflowsList;
        }
        public int AddFinanceWorkflow(AddWorkflowLevelDetails inputObj, int loggedUserId)
        {
            string query = string.Empty;
            var wfID = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"INSERT INTO TbFinanceWorkflow (WfName,WfDescription,WfNoOfLevels,CreatedDate,CreatedBy,IsDeleted,ProjectId,WorkspaceID)
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

                AddAuditLog(inputObj.ProjectId, "ADD", wfID, "WorkflowID", "Created a new finance workflow  " + inputObj.WfName, loggedUserId);
            }
            return wfID;
        }
        public void UpdateFinanceWorkflowDetails(int wfID, string wfName, string wfDesc, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"UPDATE TbFinanceWorkflow SET WfName=@WfName,WfDescription=@WfDescription,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                _connection.Execute(query, new { WfName = wfName, WfDescription = wfDesc, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
                query = "Select ProjectId  From TbFinanceWorkflow Where WorkflowID=@wfId ";
                var ProjectId = _connection.Query<int>(query, new { wfId = wfID }).Single();

                ProjectAuditLog objLog = new ProjectAuditLog();
                objLog.ProjectID = ProjectId;
                objLog.AuditEvent = "EDIT";
                objLog.AuditFromID = wfID;
                objLog.FromIDColumnName = "WorkflowID";
                objLog.Message = "Updated workflow details  named " + wfName;
                objLog.CreatedBy = loggedUserId;
                ProjectDAL.AddAuditLog(objLog);
            }
            return;
        }
        public void UpdateFinanceWorkflowLevelDetails(int wfID, int wfLevel, int userID, int groupID, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = "INSERT INTO TbFinancetWorkflowDetails( WorkflowID,WFLevel,UserID,CreatedDate,CreatedBy,IsDeleted)" +
                            " VALUES(@WorkflowID,@WFLevel,@UserID,now(),@logUserId,0); SELECT LAST_INSERT_ID();";
                _connection.Query(query, new
                {
                    WorkflowID = wfID,
                    WFLevel = wfLevel,
                    UserID = userID,
                    logUserId = loggedUserId
                }).Single();


            }
            return;
        }
        public void UpdateFinanceWorkflowLevelCount(int wfID, int wfNoOfLevels, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"UPDATE TbFinanceWorkflow SET WfNoOfLevels=@WfNoOfLevels,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                _connection.Execute(query, new { WfNoOfLevels = wfNoOfLevels, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
            }
            return;
        }
        public void RemoveFinanceWorkflows(int loggedUserId, string wfIdList)
        {
            string query = string.Empty;
            List<Workflow> objWorkflowsList = new List<Workflow>();

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbFinanceWorkflow Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, wfidlist = wfIdList });

                query = @"Update TbFinancetWorkflowDetails Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggedUserId 
                              Where FIND_IN_SET(WorkflowID,@wfidlist)";
                _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId, wfidlist = wfIdList });


                query = "Select ProjectId,WorkflowID,WfName  From TbFinanceWorkflow Where FIND_IN_SET(WorkflowID,@wfidlist)";
                objWorkflowsList = _connection.Query<Workflow>(query, new { wfidlist = wfIdList }).ToList();

                foreach (Workflow Item in objWorkflowsList)
                {
                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = Item.ProjectId;
                    objLog.AuditEvent = "REMOVE";
                    objLog.AuditFromID = Item.WorkflowID;
                    objLog.FromIDColumnName = "WorkflowID";
                    objLog.Message = "Removed Finance Workflow " + Item.WfName;
                    objLog.CreatedBy = loggedUserId;
                    ProjectDAL.AddAuditLog(objLog);
                }
            }
            return;
        }
        public void DeleteFinanceWorkflowLevel(int wfID, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"UPDATE TbFinancetWorkflowDetails SET IsDeleted=@on,UpdatedDate=@updatedDate,UpdatedBy=@userID where WorkflowID=@wfId";
                _connection.Execute(query, new { on = 1, updatedDate = DateTime.Now, userID = loggedUserId, wfId = wfID });
            }
            return;
        }
        #endregion FinanceWorkflow
        #region AuditLog
        public IEnumerable<ActivityLog> GetAuditLog(int ProjectId)
        {
            List<ActivityLog> lstUsers = new List<ActivityLog>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"Select  FAL.CreatedBy UserId ,Name ,Message ,FAL.CreatedDate,IFNULL(ProfileImage, '') as ProfileImage
                                from TbProjectAuditLog FAL Inner Join TbUser U  On U.ID =FAL.CreatedBy 
                                Where ProjectId =@ProjectId ORDER BY FAL.CreatedDate DESC";

                lstUsers = _connection.Query<ActivityLog>(strQuery, new { ProjectId = ProjectId}).ToList();
            }
            return lstUsers;
        }
        public void AddAuditLog(int ProjectID, string AuditEvent, int AuditFromID, string FromIDColumnName, string Message, int loggedUserId)
        {
            ProjectAuditLog objLog = new ProjectAuditLog();
            objLog.ProjectID = ProjectID;
            objLog.AuditFrom = "Finance";
            objLog.AuditEvent = AuditEvent;
            objLog.AuditFromID = AuditFromID;
            objLog.FromIDColumnName = FromIDColumnName;
            objLog.Message = Message;
            objLog.CreatedBy = loggedUserId;
            ProjectDAL.AddAuditLog(objLog);
        }
        #endregion AuditLog
    }
}
