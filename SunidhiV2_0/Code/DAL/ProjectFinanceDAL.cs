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
    public class ProjectFinanceDAL
    {
        protected IDbConnection _connection;

        #region Finance_BudgetAndFunds

        public IEnumerable<ProjectBudgetAndFundsDetails> GetProjectBudgetDetails(int ProjectID)
        {
            List<ProjectBudgetAndFundsDetails> lstBudgetInfo = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"Select Distinct P.ProjectID, P.GroupId, Budget,PB.ID BudgetFundID,PB.UserID, PB.UserType, U.Name UserName, U.EmailId,IFNULL(U.ProfileImage, '') AS ProfileImage, DonorBudgetAmount, 
                            PB.AccountId, AccountName, BudgetHeadAmount ,NoOfBudgetHead From TbProject P Left Join TbProjectBudgetAndFundDetails PB On PB.ProjectID = P.ProjectId 
                            and PB.IsDeleted =0 Left Join TbAccount A On A.AccountId= PB.AccountId Left join TbUser U On U.ID =PB.UserID  
                            Where P.IsActive =1 and P.IsDeleted =0 And P.ProjectId =@ProjectID ";
                lstBudgetInfo = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { ProjectID = ProjectID }).ToList();
            }
            return lstBudgetInfo;
        }
        public IEnumerable<ProjectBudgetAndFundsDetails> GetProjectUsers(int WorkspaceID,int GroupID, string Type)
        {
            List<ProjectBudgetAndFundsDetails> lstUsers = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                if (Type == "Finance")
                    strQuery = @"Select Distinct TG.UserID ,Name UserName,EmailId,'F' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,IFNULL(ProfileImage, '') as ProfileImage  
                                from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID  Inner  Join TbWorkspaceTeam W On W.UserID = U.ID 
                                Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='My Finance'))R On R.RoleId = W.RoleID 
                                Where TG.IsDeleted=0 And  GroupId =@GroupID And W.WorkspaceID = @WorkspaceID and W.IsDtd = 0 and W.IsActive = 1";
                else if (Type == "Donor")
                    strQuery = @"Select Distinct TG.UserID ,Name UserName,EmailId,'D' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,IFNULL(ProfileImage, '') as ProfileImage  from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID " +
                                " Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 " +
                                " And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='Venture'))R On R.RoleId = W.RoleID Where TG.IsDeleted=0 And  GroupId =@GroupID And W.WorkspaceID = @WorkspaceID and W.IsDtd = 0 and W.IsActive = 1";
                lstUsers = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { GroupID = GroupID, WorkspaceID = WorkspaceID }).ToList();
            }
            return lstUsers;
        }
        public IEnumerable<ProjectBudgetAndFundsDetails> GetDonorAccountDetails(int ProjectID, int UserID)
        {
            List<ProjectBudgetAndFundsDetails> lstBudgetInfo = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Select AccountId,AccountName From TbAccount Where ProjectId =@ProjectID And UserId =@UserID";
                lstBudgetInfo = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { ProjectID = ProjectID, UserID = UserID }).ToList();
            }
            return lstBudgetInfo;
        }

        public string SaveProjectBudgetsDetails(int ProjectID, int UserID, string UserType, int loggedUserId, double Amount = 0, int AccountID = 0)
        {            
            var userName = "";
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    if (UserID > 0)
                    {
                        strQuery = "INSERT INTO TbProjectBudgetAndFundDetails(ProjectID,UserID, UserType,CreatedDate, CreatedBy, IsDeleted,DonorBudgetAmount,AccountId)" +
                                        " VALUES(@ProjectID,@UserID,@UserType,now(),@loggedUserId,0,@Amount,@AccountID);SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new
                        {
                            ProjectID = ProjectID,
                            UserID = UserID,
                            UserType = UserType,
                            loggedUserId = loggedUserId,
                            Amount = Amount,
                            AccountID = AccountID
                        }).Single();

                        strQuery = "Select Name  From TbUser Where ID =@UserID ";
                        userName = _connection.Query<string>(strQuery, new { UserID = UserID }).Single();

                        if (UserType == "F")
                            AddAuditLog(ProjectID, "ADD", id, "ID", "Added Budget FinanceUser -" + userName, loggedUserId);
                        else if (UserType == "D")
                            AddAuditLog(ProjectID, "ADD", id, "ID", "Added Budget Donor - " + userName, loggedUserId);
                    }
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project Budget users...";
                    throw ex;
                }
            }
            return retStatus;
        }
        public string RemoveProjectBudgetUser(int ProjectID, int UserID, int loggedUserId,string userType, string userName)
        {           
            string strQuery = string.Empty;
            var retStatus = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails Set IsDeleted =1 ,UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID And UserID = @UserID AND UserType=@UserType;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        UserID = UserID,
                        loggedUserId = loggedUserId,
                        UserType = userType

                    });
                    //strQuery = "Select Name  From TbUser Where ID =@UserID ";
                    //var userName = _connection.Query<string>(strQuery, new { UserID = UserID }).Single();

                    AddAuditLog(ProjectID,  "REMOVE", UserID, "ID", "Removed Budget User - " + userName, loggedUserId);
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project Budget users...";
                    throw ex;
                }
            }
            return retStatus;
        }

        public string UpdateProjectBudget(int ProjectID, double TotalBudgetAmount, int loggedUserId)
        {            
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProject Set Budget =@TotalBudgetAmount ,UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID;";
                    _connection.Execute(strQuery, new { ProjectID = ProjectID, TotalBudgetAmount = TotalBudgetAmount, loggedUserId = loggedUserId });
                    AddAuditLog(ProjectID, "UPDATE", ProjectID, "ProjectId", "Updated Budget Amount - " + TotalBudgetAmount, loggedUserId);
                }
                catch (Exception ex)
                {
                    retStatus = "Failed while saving project Budget ...";
                    throw ex;
                }
            }
            return retStatus;
        }
        public string UpdateDonorAmontAndAccount(int ProjectID, int UserID, double BudgetAmount, int AccountID, int loggedUserId)
        {            
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails  Set DonorBudgetAmount =@BudgetAmount , AccountId =@AccountID, " +
                                " UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID And UserID= @UserID;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        loggedUserId = loggedUserId,
                        UserID = UserID,
                        BudgetAmount = BudgetAmount,
                        AccountID = AccountID
                    });

                    strQuery = "Select Name  From TbUser Where ID =@UserID ";
                    var userName = _connection.Query<string>(strQuery, new { UserID = UserID }).Single();                  
                    var Msg = "Updated Donor (" + userName + ") Budget Amount & Account details";
                    AddAuditLog(ProjectID, "UPDATE", UserID, "ID", Msg, loggedUserId);
                }
                catch (Exception ex)
                {
                    retStatus = "Failed while saving  project budget donor details...";
                    throw ex;
                }
            }
            return retStatus;
        }

        #endregion Finance_BudgetAndFunds

        #region BudgetPlan

        public IEnumerable<ProjectBudgetPlan> GetProjectBudgetPlanDetails(int ProjectID)
        {
            List<ProjectBudgetPlan> lstBudgetPlan = new List<ProjectBudgetPlan>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                //strQuery = @"Select ID BudgetHeadID ,BudgetHead , BudgetPlanAmount From  TbBudgetPlan A Inner Join (Select BudgetHeadID,Sum(Amount) BudgetPlanAmount  
                //            From TbBudgetPlanItemDetails  Where ProjectId=@ProjectID Group By BudgetHeadID) B On A.ID=B.BudgetHeadID 
                //            Where  ProjectId =@ProjectID ";

                strQuery = @"Select ID BudgetHeadID, BudgetHead, HeadAmount From TbBudgetPlan Where IsDeleted =0 and ProjectId = @ProjectID ";
                lstBudgetPlan = _connection.Query<ProjectBudgetPlan>(strQuery, new { ProjectID = ProjectID }).ToList();

                foreach (ProjectBudgetPlan budItem in lstBudgetPlan)
                {
                    
                    budItem.BudgetPlanSubList = new List<ProjectBudgetPlanDetails>();
                    strQuery = @"SELECT ID , ProjectID, BudgetHeadID,ItemName,  Amount  FROM TbBudgetPlanItemDetails Where IsDeleted =0 and  BudgetHeadID = @BudgetHeadID Order By BudgetHeadID ";
                    budItem.BudgetPlanSubList = _connection.Query<ProjectBudgetPlanDetails>(strQuery, new { BudgetHeadID = budItem.BudgetHeadID }).ToList();
                }
                   
            }
            return lstBudgetPlan;
        }
        
        public int AddProjectBudgetPlan(int ProjectID, int NoOfBudgetPlan, string BudgetPlanList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_AddProjectBudgetPlan";
                    id = _connection.ExecuteScalar<int>(strQuery, new
                    {
                        PrjID = ProjectID,
                        NoOfBudgetPlan = NoOfBudgetPlan,
                        BudgetPlanList = BudgetPlanList,                       
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
        public string UpdateProjectBudgetPlan(int ProjectID, int BudgetHeadID, string BudgetHead,  int loggedUserId)
        {
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbBudgetPlan SET BudgetHead =@BudgetHead,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                    " where ProjectId =@ProjectID and ID =@BudgetHeadID";
                    _connection.Execute(query, new
                    {
                        ProjectID = ProjectID,
                        BudgetHeadID = BudgetHeadID,
                        BudgetHead = BudgetHead,
                        updatedDate = DateTime.Now,
                        loggedUserId = loggedUserId
                    });
                   AddAuditLog(ProjectID, "EDIT", BudgetHeadID, "ID", "Updated Budget Head Name as " + BudgetHead, loggedUserId);
                }
                catch (Exception ex)
                {
                    retMsg = "Error while deleting data";
                    throw ex;
                }

            }
            return retMsg;
        }
        public string UpdateProjectBudgetPlanSubList(int ProjectID, int BudgetHeadID, int BudgetHeadItemID, string BudgetHeadItemName,double ItemAmount, int loggedUserId)
        {
            string strQuery = "";
            var retMsg = "success";
            double lastAmt = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    var cmd = _connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.Connection = _connection;

                    MySqlParameter pProjectID = new MySqlParameter("@ProjectID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadID = new MySqlParameter("@BudgetHeadID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadItemID = new MySqlParameter("@BudgetHeadItemID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadItemName = new MySqlParameter("@BudgetHeadItemName", MySqlDbType.String);
                    MySqlParameter pItemAmount = new MySqlParameter("@ItemAmount", MySqlDbType.Float);
                    MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                    MySqlParameter plastAmt = new MySqlParameter("@lastAmt", MySqlDbType.Float);

                    pProjectID.Value = ProjectID;
                    pBudgetHeadID.Value = BudgetHeadID;
                    pBudgetHeadItemID.Value = BudgetHeadItemID;
                    pBudgetHeadItemName.Value = BudgetHeadItemName;
                    pItemAmount.Value = ItemAmount;
                    ploggedUserId.Value = loggedUserId;

                    cmd.Parameters.Add(pProjectID);
                    cmd.Parameters.Add(pBudgetHeadID);
                    cmd.Parameters.Add(pBudgetHeadItemID);
                    cmd.Parameters.Add(pBudgetHeadItemName);
                    cmd.Parameters.Add(pItemAmount);
                    cmd.Parameters.Add(ploggedUserId);

                    try
                    {

                        strQuery = "Select ifnull(Amount, 0)   From TbBudgetPlanItemDetails Where ID =@BudgetHeadItemID And ProjectId =@ProjectID and BudgetHeadID =@BudgetHeadID";
                        lastAmt = _connection.Query<int>(strQuery, new { ProjectID = ProjectID, BudgetHeadID = BudgetHeadID, BudgetHeadItemID = BudgetHeadItemID }).Single();

                        plastAmt.Value = lastAmt;
                        cmd.Parameters.Add(plastAmt);
                        strQuery = @"UPDATE TbBudgetPlanItemDetails SET ItemName =@BudgetHeadItemName,Amount = @ItemAmount ,UpdatedDate = now(),
                                            UpdatedBy=@loggedUserId where ID =@BudgetHeadItemID And ProjectId =@ProjectID and BudgetHeadID =@BudgetHeadID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        strQuery = @"UPDATE TbBudgetPlan SET HeadAmount =(HeadAmount - @lastAmt) + @ItemAmount,UpdatedDate = now(),UpdatedBy=@loggedUserId " +
                                      " where ProjectId =@ProjectID and ID =@BudgetHeadID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();
                        
                        strQuery = @"Update TbProject set BudgetHeadAmount = (BudgetHeadAmount - @lastAmt )+ @ItemAmount,UpdatedDate = now() ,UpdatedBy=@loggedUserId where ProjectId =@ProjectID ";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        retMsg = "Error while updating data.";
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (MySqlException ex)
                        {
                            if (transaction.Connection != null)
                            {
                                retMsg = "An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.";
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
               
            }
            AddAuditLog(ProjectID, "EDIT", BudgetHeadItemID, "ID", "Updated Budget SubHead '"+ BudgetHeadItemName  + "'and Amount " + ItemAmount, loggedUserId);
            return retMsg;
        }
        public string UpdateNoOfBudgetHead(int ProjectID, int NoOfBudgetHead, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Update TbProject set NoOfBudgetHead = @NoOfBudgetHead ,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where ProjectId =@ProjectID; ";
                _connection.Execute(strQuery, new { ProjectID = ProjectID, NoOfBudgetHead = NoOfBudgetHead, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
            }           
            return retMsg;
        }

        public string RemoveBudgetPlanHead(int ProjectID, int BudgetHeadID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";
            double lastAmt = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    var cmd = _connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.Connection = _connection;

                    MySqlParameter pProjectID = new MySqlParameter("@ProjectID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadID = new MySqlParameter("@BudgetHeadID", MySqlDbType.Int32);
                    MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                    MySqlParameter plastAmt = new MySqlParameter("@lastAmt", MySqlDbType.Float);

                    pProjectID.Value = ProjectID;
                    pBudgetHeadID.Value = BudgetHeadID;
                    ploggedUserId.Value = loggedUserId;
                    cmd.Parameters.Add(pProjectID);
                    cmd.Parameters.Add(pBudgetHeadID);
                    cmd.Parameters.Add(ploggedUserId);

                    try
                    {
                        strQuery = @"Update TbBudgetPlan Set IsDeleted=1, UpdatedDate =now() ,UpdatedBy=@loggedUserId Where ID=@BudgetHeadID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        strQuery = @"Update TbBudgetPlanItemDetails Set IsDeleted=1, UpdatedDate=now() ,UpdatedBy=@loggedUserId Where BudgetHeadID=@BudgetHeadID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();
                        
                        strQuery = "Select ifnull(HeadAmount, 0)   From TbBudgetPlan Where ID =@BudgetHeadID And ProjectId =@ProjectID ";
                        lastAmt = _connection.Query<int>(strQuery, new { ProjectID = ProjectID, BudgetHeadID = BudgetHeadID }).Single();

                        plastAmt.Value = lastAmt;
                        cmd.Parameters.Add(plastAmt);

                        strQuery = @"Update TbProject set NoOfBudgetHead = NoOfBudgetHead -1 ,BudgetHeadAmount = BudgetHeadAmount - @lastAmt ,UpdatedDate = now() ,UpdatedBy=@loggedUserId where ProjectId =@ProjectID ";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();
                        
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        retMsg = "Error while updating data.";
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (MySqlException ex)
                        {
                            if (transaction.Connection != null)
                            {
                                retMsg = "An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.";
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
            }
            strQuery = "Select BudgetHead  From TbBudgetPlan Where ID =@BudgetHeadID ";
            var name = _connection.Query<string>(strQuery, new { BudgetHeadID = BudgetHeadID }).Single();
            AddAuditLog(ProjectID, "REMOVE", BudgetHeadID, "ID", "Removed Budget Head -'" + name + "'", loggedUserId);

            return retMsg;
        }       
        public string RemoveBudgetPlanSubList(int ProjectID, int BudgetHeadID, int BudgetHeadItemID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";
            double lastAmt = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                _connection.Open();
                using (var transaction = _connection.BeginTransaction())
                {
                    var cmd = _connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.Connection = _connection;

                    MySqlParameter pProjectID = new MySqlParameter("@ProjectID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadID = new MySqlParameter("@BudgetHeadID", MySqlDbType.Int32);
                    MySqlParameter pBudgetHeadItemID = new MySqlParameter("@BudgetHeadItemID", MySqlDbType.Int32);
                    MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                    MySqlParameter plastAmt = new MySqlParameter("@lastAmt", MySqlDbType.Float);

                    pProjectID.Value = ProjectID;
                    pBudgetHeadID.Value = BudgetHeadID;
                    pBudgetHeadItemID.Value = BudgetHeadItemID;
                    ploggedUserId.Value = loggedUserId;
                    cmd.Parameters.Add(pProjectID);
                    cmd.Parameters.Add(pBudgetHeadID);
                    cmd.Parameters.Add(pBudgetHeadItemID);
                    cmd.Parameters.Add(ploggedUserId);

                    try
                    {
                        strQuery = @"Update TbBudgetPlanItemDetails Set IsDeleted=1, UpdatedDate=now(),UpdatedBy=@loggedUserId Where ID=@BudgetHeadItemID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        strQuery = "Select ifnull(Amount, 0)   From TbBudgetPlanItemDetails Where ID =@BudgetHeadItemID And ProjectId =@ProjectID ";
                        lastAmt = _connection.Query<int>(strQuery, new { ProjectID = ProjectID, BudgetHeadItemID = BudgetHeadItemID }).Single();

                        plastAmt.Value = lastAmt;
                        cmd.Parameters.Add(plastAmt);

                        strQuery = @"Update TbBudgetPlan Set HeadAmount = HeadAmount - @lastAmt, UpdatedDate=now() ,UpdatedBy=@loggedUserId Where ID=@BudgetHeadID";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        strQuery = @"Update TbProject set BudgetHeadAmount = BudgetHeadAmount - @lastAmt ,UpdatedDate = now() ,UpdatedBy=@loggedUserId where ProjectId =@ProjectID ";
                        cmd.CommandText = strQuery;
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        retMsg = "Error while updating data.";
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (MySqlException ex)
                        {
                            if (transaction.Connection != null)
                            {
                                retMsg = "An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.";
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
            }
            strQuery = "Select ItemName  From TbBudgetPlanItemDetails Where ID =@BudgetHeadItemID ";
            var name = _connection.Query<string>(strQuery, new { BudgetHeadItemID = BudgetHeadItemID }).Single();
            AddAuditLog(ProjectID, "REMOVE", BudgetHeadItemID, "ID", "Removed Budget SubHead  -'" + name + "'", loggedUserId);
            return retMsg;
        }

        #endregion

        #region PaymentTerm

        public int SavepaymentTerms(int ProjectId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = "INSERT INTO TbPaymentTerms(ProjectId,PaymentTermName,Logic,paymentdate,Target,Amount,CreatedDate,CreatedBy)";
                query += " VALUES(@ProjectId,@PaymentTermName,@Logic,@paymentdate,@Target,@Amount,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                id = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                    PaymentTermName = PaymentTermName,
                    Logic = Logic,
                    paymentdate = paymentdate,
                    Target = Target,
                    Amount = Amount,
                    CreatedBy = loggedUserId,

                }).Single();
                query = @"Update TbProject set PaymentTermAmount = PaymentTermAmount+@PaymentTermAmount ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectId =@ID; ";
                _connection.Execute(query, new { ID = ProjectId, PaymentTermAmount = Amount, updatedDate = DateTime.Now, userID = loggedUserId });

                AddAuditLog(ProjectId, "ADD", id, "ID", "Added new PaymentTerm '" + PaymentTermName + "'", loggedUserId);

            }
            
            return id;
        }
        public int InsertpaymentTermsNotifyUsers(int ProjectId, int PaymentId, int UserID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = "INSERT INTO TbPaymentTermNotifyUsers(PaymentId,ProjectId,UserID,CreatedDate,CreatedBy)";
                query += " VALUES(@PaymentId,@ProjectId,@UserID,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                id = _connection.Query<int>(query, new
                {
                    ProjectId = ProjectId,
                    PaymentId = PaymentId,
                    UserID = UserID,
                    CreatedBy = loggedUserId,

                }).Single();
            }
            return id;
        }
        public void UpdatePaymentTermInProject(int ProjectId, int NoOfPaymentTerms, int UserId)
        {
            string query = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbProject set NoOfPaymentTerms = NoOfPaymentTerms+@NoOfPaymentTerms ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectId =@ID; ";
                _connection.Execute(query, new { ID = ProjectId, NoOfPaymentTerms = NoOfPaymentTerms, updatedDate = DateTime.Now, userID = UserId });
            }
            return;
        }
        public IEnumerable<PaymentTerms> GetPaymentTermsByprjID(int ProjectId)
        {

            List<PaymentTerms> objPaymentTerms = new List<PaymentTerms>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT P.ProjectId,PT.ID PaymentId,NoOfPaymentTerms,PaymentTermName,Logic,paymentdate,Target,Amount,PaymentStatus FROM  TbProject P 
                             inner join TbPaymentTerms PT on P.ProjectId=PT.ProjectId where P.ProjectId=@ID and PT.IsDeleted=0; ";
                objPaymentTerms = _connection.Query<PaymentTerms>(strQuery, new { ID = ProjectId }).ToList();
            }
            return objPaymentTerms;
        }
        public void DeletePaymentTerm(int PaymentId,int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbPaymentTerms set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId where ID =@ID; ";
                _connection.Execute(query, new { ID = PaymentId, IsDeleted = 1, loggedUserId = loggedUserId });

                var strQuery = "Select PaymentTermName  From TbPaymentTerms Where ID =@PaymentId ";
                var name = _connection.Query<string>(strQuery, new { PaymentId = PaymentId }).Single();
                strQuery = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                var ProjectId = _connection.Query<int>(strQuery, new { PaymentId = PaymentId }).Single();

                AddAuditLog(ProjectId, "REMOVE", PaymentId, "ID", "Removed PaymentTerm '" + name + "'", loggedUserId);
            }          
            return;
        }
        public void UpdatePaymnetStatus(int PaymentId, string PaymentStatus, int loggedUserId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbPaymentTerms set PaymentStatus = @PaymentStatus , UpdatedDate=now(),UpdatedBy=@loggedUserId where ID =@ID; ";
                _connection.Execute(query, new { ID = PaymentId, PaymentStatus = PaymentStatus, loggedUserId = loggedUserId });

                var strQuery = "Select PaymentTermName  From TbPaymentTerms Where ID =@PaymentId ";
                var name = _connection.Query<string>(strQuery, new { PaymentId = PaymentId }).Single();
                strQuery = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                var ProjectId = _connection.Query<int>(strQuery, new { PaymentId = PaymentId }).Single();

                AddAuditLog(ProjectId, "EDIT", PaymentId, "ID", "Updated Payment Status '" + PaymentStatus + "'", loggedUserId);
            }
            return;
        }
        public PaymentTerms GetPaymentTermsByPaymentId(int PaymentId)
        {

            PaymentTerms objPaymentTerms = new PaymentTerms();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT ProjectId,ID PaymentId,PaymentTermName,Logic,paymentdate,Target,Amount,PaymentStatus FROM  TbPaymentTerms where ID=@ID and IsDeleted=0  ";
                objPaymentTerms = _connection.Query<PaymentTerms>(strQuery, new { ID = PaymentId }).FirstOrDefault();


                strQuery = @"select Name UserName,UserID,IFNULL(ProfileImage, '') as ProfileImage from TbPaymentTermNotifyUsers PTNU inner join TbUser U on PTNU.UserID=U.ID where PaymentId=@ID and PTNU.IsDeleted=0 Order By PTNU.ID Desc";
                var NotifyUsers = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new
                {
                    ID = PaymentId
                }).ToList();
                objPaymentTerms.users = JsonConvert.SerializeObject(NotifyUsers);

            }
            return objPaymentTerms;
        }

        public void UpdatepaymentTerms(int PaymentId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, int loggedUserId)
        {
            string query = string.Empty;


            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbPaymentTerms set PaymentTermName = @PaymentTermName,Logic=@Logic,paymentdate=@paymentdate,Target=@Target,Amount=@Amount, UpdatedDate = @updatedDate,
                        UpdatedBy=@UpdatedBy where ID =@PaymentId; ";

                _connection.Query<int>(query, new
                {
                    PaymentId = PaymentId,
                    PaymentTermName = PaymentTermName,
                    Logic = Logic,
                    paymentdate = paymentdate,
                    Target = Target,
                    Amount = Amount,
                    UpdatedBy = loggedUserId,
                    UpdatedDate = DateTime.Now

                });
                query = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                var ProjectId = _connection.Query<int>(query, new { PaymentId = PaymentId }).Single();

                AddAuditLog(ProjectId, "EDIT", PaymentId, "ID", "Updated Payment Term details ", loggedUserId);
            }
            return;
        }
        
        public void DeletePaymentTermNotifyUsers(int PaymentId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbPaymentTermNotifyUsers set IsDeleted = @IsDeleted where PaymentId =@ID; ";
                _connection.Execute(query, new { ID = PaymentId, IsDeleted = 1 });
            }
            return;
        }
        public int SavePaymentRequest(ReuestFund inputObj, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"INSERT INTO TbProjectRequestFund(ProjectId,PaymentID,DonorUser,RequestAmount,Comment,ReqDocument,IsDeleted,CreatedDate,CreatedBy)
                               VALUES(@projid,@PaymentID,@donor,@reqamt,@Comment,@ReqDocument,@isdeleted,now(),@userID); SELECT LAST_INSERT_ID();";
                id = _connection.Query<int>(strQuery, new { projid = inputObj.ProjectId,
                    PaymentID= inputObj.PaymentID, donor = inputObj.DonorUser, reqamt = inputObj.RequestAmount,
                    Comment = inputObj.Comment,  ReqDocument = inputObj.ReqDocument,  isdeleted = 0, userID = loggedUserId }).Single();

                strQuery = "Select Name  From TbUser Where ID =@UserID ";
                var userName = _connection.Query<string>(strQuery, new { UserID = inputObj.DonorUser }).FirstOrDefault();
                AddAuditLog(inputObj.ProjectId, "ADD", id, "ID", "Added new Request Payment to Donor '"+ userName + "'", loggedUserId);

                if (id > 0)
                {
                    string Qeury = @"Select WSID as WorkspaceID  From TbProject Where ProjectId = @ProjectId";
                    var WsID = _connection.Query<int>(Qeury, new { ProjectId = inputObj.ProjectId }).FirstOrDefault();
                    //AddTo SyncDB
                    string Query = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                     VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                    var syncDBId = _connection.Query<int>(Query, new
                    {
                        Type = "Payment Request",
                        Command = "ADD",
                        TypeIDColumn = "RequestID",
                        TypeID = id,
                        WorkflowID = 0,
                        FinanceWorkflowID = 0,
                        WfNoOfLevels = 0,
                        Status = "Pending",
                        WorkspaceID = WsID,
                        CreatedBy = loggedUserId
                    }).Single();

                    // Log insert
                    var query = "Select PaymentTermName  From TbPaymentTerms Where ID =@PaymentID ";
                    var PaymentTermName = _connection.Query<string>(query, new { PaymentID = inputObj.PaymentID }).FirstOrDefault();
                    query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy,Command,Document)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy,@Comment,@ReqDocument); SELECT LAST_INSERT_ID();";
                    int Logid = _connection.Query<int>(query, new
                    {
                        ProjectID = inputObj.ProjectId,
                        TaskType = "Payment Request",
                        TaskTypeID = id,
                        TypeIDColumn = "REQUESTID",
                        AssignedTo = inputObj.DonorUser,
                        AssignedBy = loggedUserId,
                        Comment=inputObj.Comment,
                        ReqDocument=inputObj.ReqDocument,
                        Message =  "Requested Rs. " + inputObj.RequestAmount + " for " + PaymentTermName ,
                        createdby = loggedUserId
                    }).Single();
                    try
                    {
                        // Send Notification 
                        var title = "";
                        var sub = "";
                        strQuery = "Select Name  From TbUser Where  ID =@logUserID ";
                        var name = _connection.Query<string>(strQuery, new { logUserID = loggedUserId }).Single();

                        title = name + " Requested for Paymnet";
                        sub = name + " Requested for Payment  " + inputObj.RequestAmount + "' on " + DateTime.Now;
                        Notification objNotification = new Notification();
                        objNotification.UserId = inputObj.DonorUser;
                        objNotification.Title = title;
                        objNotification.Subject = sub;
                        new NotificationDAL().SendUserNotification(objNotification);
                    }
                    catch
                    {

                    }
                }
                return id;
            }
        }

        #endregion PaymentTerm

        #region FundUtilisation
        public IEnumerable<ProjectBudgetPlan> GetFundPaymentDeatils(int ProjectID)
        {
            List<ProjectBudgetPlan> lstBudget = new List<ProjectBudgetPlan>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                
                strQuery = @"Select LB.ProjectId  ProjectID, BudgetHeadID, BudgetHead, Sum(TransactionAmount) HeadAmount From TbLedgerBook LB Left Join TbBudgetPlan BP On BP.ID = LB.BudgetHeadID 
                                    Where LB.IsDeleted =0 and LB.ProjectId = @ProjectID Group By BudgetHeadID,BudgetHead";
                lstBudget = _connection.Query<ProjectBudgetPlan>(strQuery, new { ProjectID = ProjectID }).ToList();

                foreach (ProjectBudgetPlan budItem in lstBudget)
                {

                    budItem.FundPaymentList = new List<ProjectPaymentList>();                 
                    strQuery = @"SELECT LB.ProjectId ,PaymentId, PaymentActivityName Title, TransactionId TransactionID, TransactionDate, TransactionAmount, TransactionDocName, BudgetHeadID, BudgetHead  
                                FROM TbLedgerBook LB Left Join TbBudgetPlan BP On BP.ID = LB.BudgetHeadID Where LB.IsDeleted =0 and  BudgetHeadID = @BudgetHeadID  Order By BudgetHeadID ";
                    budItem.FundPaymentList = _connection.Query<ProjectPaymentList>(strQuery, new { BudgetHeadID = budItem.BudgetHeadID }).ToList();
                }

            }
            return lstBudget;
        }
        #endregion
        #region AuditLog
        public IEnumerable<ActivityLog> GetAuditLog(int ProjectID)
        {
            List<ActivityLog> lstUsers = new List<ActivityLog>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"Select  FAL.CreatedBy UserId ,Name ,Message ,FAL.CreatedDate,IFNULL(ProfileImage, '') as ProfileImage 
                                from TbProjectAuditLog FAL Inner Join TbUser U  On U.ID =FAL.CreatedBy 
                                Where ProjectID =@ProjectID And AuditFrom='Finance'  Order By FAL.CreatedDate desc";

                lstUsers = _connection.Query<ActivityLog>(strQuery, new { ProjectID = ProjectID }).ToList();
            }
            return lstUsers;
        }
        public void AddAuditLog(int ProjectID,string AuditEvent, int AuditFromID, string FromIDColumnName, string Message,int loggedUserId)
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
