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
    public class VentureDAL
    {
        protected IDbConnection _connection;
        #region SearchVenture
        public IEnumerable<Project> GetProjectListOfVenture (int loggedUserId,int WorkspaceID)
        {
            List<Project> lstProjects = new List<Project>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT distinct P.ProjectId,ProjectName,StartDate,EndDate,Budget,P.IsActive,P.Code,P.InitiativeId,IFNULL(sum(DonorBudgetAmount),0) as FundedAmount,InitiativeName as Initiative,(select sum(TransactionAmount)  from TbLedgerBook LB  Left join  TbAccount
                             on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId) UtilizedFund,IFNULL((select Round((sum(TransactionAmount)*100)/FundedAmount)  
                             from TbLedgerBook LB  Left join  TbAccount on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) FundUtilized FROM  TbProject P 
                             inner join TbInitiative TI on P.InitiativeId=TI.InitiativeId left join TbProjectBudgetAndFundDetails MV on P.ProjectId=MV.ProjectID  where 
                             P.IsDeleted=0 and P.IsActive=1 and GroupId in  (select GroupId from TbUserAssignedToGroup UATG inner join TbWorkspaceTeam W On W.UserID = UATG.UserId Inner join 
                             (SELECT RoleId  FROM TbRoleBasedAccessScreens Where  AccessId =3 And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='Venture'))R
                              On R.RoleId = W.RoleID where W.UserID=@loggedUserId) and @loggedUserId not in (select CreatedBy from TbProjectBudgetAndFundDetails where ProjectId=P.ProjectId and IsDeleted=0 ) 
                              and P.WSID=@WorkspaceID Group By P.ProjectId,ProjectName,StartDate,EndDate,Budget,P.IsActive,Code,P.InitiativeId Order by P.ProjectId Desc";



                lstProjects = _connection.Query<Project>(strQuery, new { loggedUserId = loggedUserId, WorkspaceID = WorkspaceID }).ToList();
            }
            return lstProjects;
        }
        public int InsertCommitFund(int ProjectId, string ProjectType, double Amount , int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"SELECT EXISTS(SELECT ID FROM TbProjectBudgetAndFundDetails WHERE ProjectId=@ProjectId and ProjectType=@ProjectType and IsDeleted = 0);";
                id = _connection.Query<int>(query, new { ProjectId = ProjectId, ProjectType = "Wishlist" }).Single();

                if (id > 0)
                {
                    query = "Update TbProjectBudgetAndFundDetails SET ProjectType = @ProjectType,DonorBudgetAmount=@DonorBudgetAmount,UpdatedDate = @updatedDate,UpdatedBy =@loggedUserId ";
                    query += " Where ProjectId=@ProjectId and ProjectType=@wishType";
                    _connection.Execute(query, new { ProjectId = ProjectId, wishType = "Wishlist",ProjectType= ProjectType, DonorBudgetAmount = Amount, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                    query = "INSERT INTO TbFinanceAuditLog(ProjectId,Message,CreatedBy,CreatedDate)";
                    query += " VALUES(@ProjectId,@Message,@CreatedBy,now());";
                    _connection.Query<int>(query, new
                    {
                        ProjectId = ProjectId,
                        Message = "committed Rs." + Amount,
                        CreatedBy = loggedUserId,

                    });
                    
                }
                else
                { 
                    query = "INSERT INTO TbProjectBudgetAndFundDetails(ProjectId,UserID,UserType,ProjectType,DonorBudgetAmount,CreatedDate,CreatedBy)";
                query += " VALUES(@ProjectId,@CreatedBy,'D',@Type,@DonorBudgetAmount,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                id = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                    Type = ProjectType,
                    DonorBudgetAmount = Amount,
                    CreatedBy = loggedUserId,

                }).Single();
                    query = "INSERT INTO TbFinanceAuditLog(ProjectId,Message,CreatedBy,CreatedDate)";
                    query += " VALUES(@ProjectId,@Message,@CreatedBy,now());";
                    _connection.Query<int>(query, new
                    {
                        ProjectId = ProjectId,
                        Message = "committed a fund of Rs. " + Amount,
                        CreatedBy = loggedUserId,

                    });

                }
                if( id > 0 && ProjectType != "Wishlist")
                {
                    string Qeury = @"Select ifnull(WSID,0) as WorkspaceID  From TbProject  Where ProjectId = @PrjID";
                    var WsID = _connection.Query<int>(Qeury, new { PrjID = ProjectId }).FirstOrDefault();

                    //AddTo SyncDB
                    query = @"SELECT ID FROM TbProjectBudgetAndFundDetails WHERE ProjectId=@ProjectId and ProjectType=@ProjectType and UserID=@UserID and UserType = 'D' and IsDeleted = 0;";
                    int BudgetFundId = _connection.Query<int>(query, new { ProjectId = ProjectId, ProjectType = "Funded Project", UserID = loggedUserId }).FirstOrDefault();
                    
                    string Query = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy,CommitedAmount )
                                     VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status, @WorkspaceID,now(), @CreatedBy,@CommitedFund); SELECT LAST_INSERT_ID();";
                    var syncDBId = _connection.Query<int>(Query, new
                    {
                        Type = "Fund Request",
                        Command = "ADD",
                        TypeIDColumn = "RequestID",
                        TypeID = ProjectId,
                        WorkflowID = 0,
                        FinanceWorkflowID = 0,
                        WfNoOfLevels = 0,
                        Status = "Pending",
                        CreatedBy = loggedUserId,
                        WorkspaceID = WsID,
                        CommitedFund = Amount
                    }).Single();

                    List<userList> lstUsers = new List<userList>();
                    string strQuery = @"select  UserID from  TbProjectBudgetAndFundDetails where ProjectID = @ProjectID and UserType = 'F' and IsDeleted = 0;";
                    lstUsers = _connection.Query<userList>(strQuery, new { ProjectID = ProjectId }).ToList();

                    for (var j = 0; j < lstUsers.Count; j++)
                    {

                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        int Logid = _connection.Query<int>(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "Fund Request",
                            TaskTypeID = BudgetFundId,
                            TypeIDColumn = "REQUESTID",
                            AssignedTo = lstUsers[j].userId,
                            AssignedBy = loggedUserId,
                            Message = "committed a fund of Rs. " + Amount,
                            createdby = loggedUserId
                        }).Single();
                    }
                }

            }
            return id;
        }

        public int UpdateCommitFund(int ProjectId, string ProjectType, double Amount, int loggedUserId)



        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"SELECT EXISTS(SELECT ID FROM TbProjectBudgetAndFundDetails WHERE ProjectId=@ProjectId and ProjectType=@ProjectType and UserID=@UserID);";
                id = _connection.Query<int>(query, new { ProjectId = ProjectId, ProjectType = "Funded Project", UserID = loggedUserId }).Single();

                if (id > 0)
                {
                    query = "Update TbProjectBudgetAndFundDetails SET DonorBudgetAmount=DonorBudgetAmount+@DonorBudgetAmount,UpdatedDate = @updatedDate,UpdatedBy =@UserID ";
                    query += " Where ProjectId=@ProjectId and ProjectType=@ProjectType and UserID=@UserID";
                    _connection.Execute(query, new { ProjectId = ProjectId, ProjectType = ProjectType, DonorBudgetAmount = Amount, updatedDate = DateTime.Now, UserID = loggedUserId });

                    string Qeury = @"Select ifnull(WSID,0) as WorkspaceID From TbProject  Where ProjectId = @PrjID";
                    int WsID = _connection.Query<int>(Qeury, new { PrjID = ProjectId }).FirstOrDefault();
                    //AddTo SyncDB
                    query = @"SELECT ID FROM TbProjectBudgetAndFundDetails WHERE ProjectId=@ProjectId and ProjectType=@ProjectType and UserID=@UserID and UserType = 'D' and IsDeleted = 0;";
                    int BudgetFundId = _connection.Query<int>(query, new { ProjectId = ProjectId, ProjectType = "Funded Project", UserID = loggedUserId }).Single();

                    string Query = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy,CommitedAmount )
                                     VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy,@CommitedFund); SELECT LAST_INSERT_ID();";
                    var syncDBId = _connection.Query<int>(Query, new
                    {
                        Type = "Fund Request",
                        Command = "ADD",
                        TypeIDColumn = "RequestID",
                        TypeID = ProjectId,
                        WorkflowID = 0,
                        FinanceWorkflowID = 0,
                        WfNoOfLevels = 0,
                        Status = "Pending",
                        CreatedBy = loggedUserId,
                        WorkspaceID = WsID,
                        CommitedFund = Amount
                    }).Single();

                    List<userList> lstUsers = new List<userList>();
                    string strQuery = @"select  UserID from  TbProjectBudgetAndFundDetails where ProjectID = @ProjectID and UserType = 'F' and IsDeleted = 0;";
                    lstUsers = _connection.Query<userList>(strQuery, new { ProjectID = ProjectId }).ToList();

                    for (var j = 0; j < lstUsers.Count; j++)
                    {

                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        int Logid = _connection.Query<int>(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "Fund Request",
                            TaskTypeID = BudgetFundId,
                            TypeIDColumn = "REQUESTID",
                            AssignedTo = lstUsers[j].userId,
                            AssignedBy = loggedUserId,
                            Message = "committed a fund of Rs. " + Amount,
                            createdby = loggedUserId
                        }).Single();
                    }
                }
               

            }
            return id;
        }
        public IEnumerable<Project> GetProjectListOfFundedAndWishList(string ProjectType,int loggedUserId,int WorkspaceID)
        {
            List<Project> lstProjects = new List<Project>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT distinct P.ProjectId,ProjectName,StartDate,EndDate,Budget,P.IsActive,P.Code,P.InitiativeId,InitiativeName as Initiative,sum(DonorBudgetAmount) as FundedAmount,
                              IFNULL((select Round((sum(TransactionAmount)*100)/sum(DonorBudgetAmount) )  from TbLedgerBook LB  Left join
                               TbAccount on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) FundUtilized  
                             FROM TbProject P inner join TbInitiative TI on P.InitiativeId=TI.InitiativeId inner join TbProjectBudgetAndFundDetails MV 
                             on P.ProjectId=MV.ProjectID where P.IsDeleted=0  and P.IsActive=1  and  ProjectType=@ProjectType and MV.UserID=@loggedUserId and  MV.UserType='D' and MV.IsDeleted=0 and P.WSID=@WorkspaceID group by   MV.ProjectID,ProjectName,StartDate,EndDate,Budget,P.Code,P.InitiativeId
                             Order by P.ProjectId Desc";

                lstProjects = _connection.Query<Project>(strQuery, new { ProjectType = ProjectType, loggedUserId = loggedUserId, WorkspaceID = WorkspaceID }).ToList();
            }
            return lstProjects;
        }
        public IEnumerable<Account> GetProjectAccounts(int loggedUserId)
        {
            List<Account> lstAccount = new List<Account>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT * from TbAccount where IsDeleted=@isdeleted and IsActive=@isactive and UserId=@loggedUserId  Order by AccountId Desc";
                lstAccount = _connection.Query<Account>(strQuery, new
                {
                    isdeleted = 0,
                    isactive = 1,
                    loggedUserId = loggedUserId,

                }).ToList();
            }
            return lstAccount;
        }
        public IEnumerable<Venture>GetProjectExpenses(int loggedUserId, int WorkspaceId)
        {
            List<Venture> lstAccount = new List<Venture>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT P.ProjectId,ProjectName ,IFNULL(sum(DonorBudgetAmount),0) as FundedAmount,IFNULL((select sum(TransactionAmount)  from TbLedgerBook LB  Left join  TbAccount
                             on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) UtilizedFund,IFNULL((select Round((sum(TransactionAmount) *100)/sum(DonorBudgetAmount),2)  
                             from TbLedgerBook LB  Left join  TbAccount on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) FundUtilized
                             FROM TbProject P inner join TbProjectBudgetAndFundDetails MV on P.ProjectId=MV.ProjectID Where MV.IsDeleted=0  and  ProjectType='Funded Project' and UserType ='D' and MV.UserID=@loggedUserId and P.WSID = @WorkspaceId
                             Group by P.ProjectId,ProjectName  Order by P.ProjectId Desc";
                lstAccount = _connection.Query<Venture>(strQuery, new
                {
                    isdeleted = 0,
                    isactive = 1,
                    loggedUserId = loggedUserId,
                    WorkspaceId = WorkspaceId

                }).ToList();
            }
            return lstAccount;
        }
        public IEnumerable<Venture> GetTotalProjectExpenses(int loggedUserId, int WorkspaceId)
        {
            List<Venture> lstAccount = new List<Venture>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Select IFNULL(Sum(t.UtilizedFund),0)as TotalUtilizedFund,IFNULL(sum(t.FundUtilized),0) as TotalFundUtilized,IFNULL(sum(FundedAmount),0) TotalFundedAmount,
                              IFNULL(sum(FundedAmount) -Sum(t.UtilizedFund),0) as UnutilizedFund from(                             
                             SELECT P.ProjectId,ProjectName ,DonorBudgetAmount as FundedAmount,IFNULL((select sum(TransactionAmount)  from TbLedgerBook LB  Left join  TbAccount
                             on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) UtilizedFund,IFNULL((select Round((sum(TransactionAmount) *100)/DonorBudgetAmount)  
                             from TbLedgerBook LB  Left join  TbAccount on IncomeAccountId=AccountId where UserId=@loggedUserId and LB.ProjectId= P.ProjectId),0) FundUtilized
                             FROM TbProject P inner join TbProjectBudgetAndFundDetails MV on P.ProjectId=MV.ProjectID Where MV.IsDeleted=0  and  ProjectType='Funded Project' and MV.UserID=@loggedUserId and P.WSID = @WorkspaceId)t";
                lstAccount = _connection.Query<Venture>(strQuery, new
                {
                    isdeleted = 0,
                    isactive = 1,
                    loggedUserId = loggedUserId,
                    WorkspaceId = WorkspaceId

                }).ToList();
            }
            return lstAccount;
        }
        public IEnumerable<Project> GetFundedUsersById(int ProjectId)
        {
            List<Project> lstProjects = new List<Project>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                //strQuery = @"SELECT distinct sum(DonorBudgetAmount) as FundedAmount,(Select ID from TbUser where ID=MV.UserID) UserID,(Select Name from TbUser where ID=MV.UserID) as UserName, (Select ifnull(ProfileImage,'') as ProfileImage from TbUser where ID=MV.UserID) ProfileImage  
                //             FROM TbProjectBudgetAndFundDetails MV  where MV.IsDeleted=0  and  ProjectType='Funded Project' and MV.ProjectID=@ProjectId Group by CreatedBy,UserID";
                strQuery = @"SELECT sum(MV.DonorBudgetAmount) as FundedAmount,user.ID AS UserID,user.Name AS UserName,ifnull(user.ProfileImage,'') AS ProfileImage
                                FROM TbProjectBudgetAndFundDetails MV INNER JOIN TbUser user ON user.ID=MV.UserID
                                where MV.IsDeleted=0  and  MV.ProjectType='Funded Project' and MV.ProjectID=@ProjectId Group by UserID,UserName,ProfileImage";

                lstProjects = _connection.Query<Project>(strQuery, new { ProjectId = ProjectId }).ToList();
            }
            return lstProjects;
        }
        public IEnumerable<Venture> GetTotalFundById(int ProjectId)
        {
            List<Venture> lstProjects = new List<Venture>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT  sum(DonorBudgetAmount) as TotalFundedAmount
                                  FROM  TbProjectBudgetAndFundDetails  where IsDeleted=0  and  ProjectType='Funded Project' and UserType='D' and ProjectID=@ProjectId";

                lstProjects = _connection.Query<Venture>(strQuery, new { ProjectId = ProjectId }).ToList();
            }
            return lstProjects;
        }
        public IEnumerable<VentureFund> GetLedgerBookExpenseTagsById(int ProjectId)
        {
            List<VentureFund> lstProjects = new List<VentureFund>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT ExpenseTagID,(select TagName from TbProjectExpenseTags where TagID= ExpenseTagID) TagName,sum(TransactionAmount) as TransactionAmount
                                from TbLedgerBookToExpenseTags PET inner join TbLedgerBook LB 
                                on LB.PaymentId= PET.PaymentId where LB.ProjectId=@ProjectId group by ExpenseTagID order by TagName";

                lstProjects = _connection.Query<VentureFund>(strQuery, new { ProjectId = ProjectId }).ToList();
            }
            return lstProjects;
        }
        #endregion SearchVenture

        #region MyVenture
        public void DeleteVenture(int ProjectId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbProjectBudgetAndFundDetails set IsDeleted = @IsDeleted where ProjectID =@ProjectId; ";
                _connection.Execute(query, new { ProjectId = ProjectId, IsDeleted = 1 });
            }
            return;
        }
        #endregion MyVenture
    }
}
