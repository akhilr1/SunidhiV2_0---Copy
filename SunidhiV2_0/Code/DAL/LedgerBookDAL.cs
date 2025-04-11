using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.DAL
{
    public class LedgerBookDAL
    {
        protected IDbConnection _connection;

        #region ManageProjectLedgerBook
        /// <summary>
        /// Function used to fetch payment details based on projectID,acctID
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="acctID"></param>
        /// <returns></returns>
        public List<LedgerBook> GetPaymentList(int projectID, int acctID)
        {
            List<LedgerBook> objPaymentList = new List<LedgerBook>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (acctID == 0)
                    {

                        query = @"select LB.PaymentId,LB.PaymentActivityName,LB.TransactionId,LB.TransactionDate,LB.TransactionAmount,LB.IncomeAccountId,LB.ExpenseAccountId,LB.TransactionType,
                              LB.TransactionDocName,LB.TransactionStatus,LB.IsActivityLinked,LB.ActivityId,LB.ProjectId,LB.BudgetHeadID,LB.BudgetItemID,
                             plan.BudgetHead AS BudgetHeadName,Acc.AccountName as IncomeAccount,Acc1.AccountName as ExpenseAccount from TbLedgerBook LB 
                                inner join TbAccount Acc on Acc.AccountId = LB.IncomeAccountId inner join TbAccount Acc1 on Acc1.AccountId = LB.ExpenseAccountId 
                                INNER JOIN TbBudgetPlan plan ON plan.ID=LB.BudgetHeadID where LB.IsDeleted = 0  and LB.ProjectId = @projid Order By LB.PaymentId Desc;";
                        objPaymentList = _connection.Query<LedgerBook>(query, new { projid = projectID }).ToList();
                    }
                    else
                    {

                        query = @"select LB.PaymentId,LB.PaymentActivityName,LB.TransactionId,LB.TransactionDate,LB.TransactionAmount,LB.IncomeAccountId,LB.ExpenseAccountId,LB.TransactionType,
                              LB.TransactionDocName,LB.TransactionStatus,LB.IsActivityLinked,LB.ActivityId,LB.ProjectId,LB.BudgetHeadID,LB.BudgetItemID,
                             plan.BudgetHead AS BudgetHeadName,Acc.AccountName as IncomeAccount,Acc1.AccountName as ExpenseAccount from TbLedgerBook LB 
                                inner join TbAccount Acc on Acc.AccountId = LB.IncomeAccountId inner join TbAccount Acc1 on Acc1.AccountId = LB.ExpenseAccountId 
                                INNER JOIN TbBudgetPlan plan ON plan.ID=LB.BudgetHeadID where LB.IsDeleted = 0  and LB.ProjectId = @projid and (LB.IncomeAccountId=@accid or LB.ExpenseAccountId=@accid) Order By LB.PaymentId Desc;";
                        objPaymentList = _connection.Query<LedgerBook>(query, new { projid = projectID, accid = acctID }).ToList();
                    }

                    foreach (LedgerBook payItem in objPaymentList)
                    {
                        payItem.ExpenseTags = new List<ProjectExpenseTags>();
                        query = @"select projtags.TagID , projtags.TagName from TbProjectExpenseTags projtags INNER JOIN TbLedgerBookToExpenseTags ledgertags 
                            ON projtags.TagID = ledgertags.ExpenseTagID WHERE ledgertags.IsDeleted = 0 AND ledgertags.PaymentId = @payid  Order By projtags.TagID Desc";
                        payItem.ExpenseTags = _connection.Query<ProjectExpenseTags>(query, new { payid = payItem.PaymentId }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objPaymentList;
        }

        /// <summary>
        /// Function used to fetch activity details based on projectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<ProjectActivity> GetActivityList(int projectID)
        {
            List<ProjectActivity> objActivityList = new List<ProjectActivity>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select ActivityId,ActivityName from TbProjectActivity where IsDeleted=0 and ProjectId=@projid and IsPaymentLinked=1 Order By ActivityId Desc";
                    objActivityList = _connection.Query<ProjectActivity>(query, new { projid = projectID }).ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return objActivityList;
        }

        /// <summary>
        /// Function used to fetch project account details based on projID,accType
        /// </summary>
        /// <param name="projID"></param>
        /// <param name="accType"></param>
        /// <returns></returns>
        public IEnumerable<Account> GetProjectAccountsByType(int projID,string accType)
        {
            List<Account> lstAccount = new List<Account>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT AccountId,AccountName from TbAccount where IsDeleted=@isdeleted and IsActive=@isactive and ProjectId=@projid and BankAccountType=@acctype Order by AccountId Desc";
                    lstAccount = _connection.Query<Account>(strQuery, new
                    {
                        isdeleted = 0,
                        isactive = 1,
                        projid = projID,
                        acctype = accType
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstAccount;
        }
        
        /// <summary>
        /// Function used to add payment
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="exptaglist"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddPayment(LedgerBook inputObj, string exptaglist, int loggedUserId)
        {
            string strQuery = string.Empty;
            var idPayment = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"INSERT INTO TbLedgerBook(ProjectId, PaymentActivityName, TransactionId, TransactionDate, 
                                                      TransactionAmount, IncomeAccountId, ExpenseAccountId, 
                                                      TransactionDocName, TransactionStatus,ActivityId,IsActivityLinked,BudgetHeadID,BudgetItemID, IsDeleted, 
                                                      CreatedDate, CreatedBy,TransactionType )
                                            VALUES(@projectid,@payname,@transid,@transdate,@transamt,@inacct,
                                                   @exacct,@transdoc,@transstatus,@ActivityId,@IsActivityLinked,@BudgetHeadID,@BudgetItemID,@isdeleted,
                                                   @createdate,@createby,@transactiontype); 
                                             SELECT LAST_INSERT_ID();";
                        idPayment = _connection.Query<int>(strQuery, new
                        {
                            projectid = inputObj.ProjectId,
                            payname = inputObj.PaymentActivityName,
                            transid = inputObj.TransactionId,
                            transdate = inputObj.TransactionDate,
                            transamt = inputObj.TransactionAmount,
                            inacct = inputObj.IncomeAccountId,
                            exacct = inputObj.ExpenseAccountId,
                            transdoc = inputObj.TransactionDocName,
                            transstatus = inputObj.TransactionStatus,
                            ActivityId = inputObj.ActivityId,
                            IsActivityLinked = inputObj.IsActivityLinked,
                            BudgetHeadID = inputObj.BudgetHeadID,
                            BudgetItemID = inputObj.BudgetItemID,
                            isdeleted = 0,
                            createdate = DateTime.Now,
                            createby = loggedUserId,
                            transactiontype = inputObj.TransactionType

                        }).Single();

                        if (idPayment > 0)
                        {
                            List<ProjectExpenseTags> objExpenseTagList = JsonConvert.DeserializeObject<List<ProjectExpenseTags>>(exptaglist);
                            foreach (ProjectExpenseTags exptagitem in objExpenseTagList)
                            {
                                AddPaymentTags(exptagitem.TagID, idPayment, loggedUserId);
                            }
                        }

                        AddAuditLog(inputObj.ProjectId, "ADD", inputObj.TransactionId, "TransactionId", "Added Payment " + inputObj.PaymentActivityName + " Amount " + inputObj.TransactionAmount, loggedUserId);

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return idPayment;
        }

        /// <summary>
        /// Function used to add payment tags
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="idPayment"></param>
        /// <param name="loggedUserId"></param>
        public void AddPaymentTags(int tagId, int idPayment, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                { 
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT EXISTS(SELECT ID from TbLedgerBookToExpenseTags WHERE PaymentId=@payid and ExpenseTagID=@tagid);";
                    id = _connection.Query<int>(strQuery, new { payid = idPayment, tagid = tagId }).Single();

                    if (id == 0)
                    {
                        strQuery = @"INSERT INTO TbLedgerBookToExpenseTags(PaymentId,ExpenseTagID,CreatedDate,CreatedBy)
                                            VALUES(@payid,@tagid,now(),@userID); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(strQuery, new { payid = idPayment, tagid = tagId, userID = loggedUserId }).Single();
                    }
                    else
                    {
                        strQuery = @"Update TbLedgerBookToExpenseTags Set isDeleted=0, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where PaymentId=@payid and ExpenseTagID=@tagid";
                        _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, payid = idPayment, tagid = tagId });
                    }
                    return;
                }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Function used to delete payment based on loggedUserId,PayIdList
        /// </summary>
        /// <param name="loggedUserId"></param>
        /// <param name="PayIdList"></param>
        public void RemovePayment(int loggedUserId, string PayIdList)
        {
            string query = string.Empty;
            List<LedgerBook> objPaymentList = new List<LedgerBook>();
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbLedgerBook Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(PaymentId,@payidlist); Update TbLedgerBookToExpenseTags Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                               Where FIND_IN_SET(PaymentId,@payidlist);Select ProjectId,PaymentId,PaymentActivityName  From TbLedgerBook Where FIND_IN_SET(PaymentId,@payidlist);";
                        //_connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, payidlist = PayIdList });

                        //query = @"Update TbLedgerBookToExpenseTags Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                        //       Where FIND_IN_SET(PaymentId,@payidlist)";
                        //_connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, payidlist = PayIdList });

                        //query = "Select ProjectId,PaymentId,PaymentActivityName  From TbLedgerBook Where FIND_IN_SET(PaymentId,@payidlist)";


                        objPaymentList = _connection.Query<LedgerBook>(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, payidlist = PayIdList }).ToList();

                        foreach (LedgerBook Item in objPaymentList)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = Item.ProjectId;
                            objLog.AuditEvent = "REMOVE";
                            objLog.AuditFromID = Item.PaymentId;
                            objLog.FromIDColumnName = "PaymentId";
                            objLog.Message = "Remove Payment " + Item.PaymentActivityName;
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
            return;
        }

        /// <summary>
        /// Function used to edit payment based on payid
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="exptaglist"></param>
        /// <param name="loggedUserId"></param>
        public void EditPayment(LedgerBook inputObj, string exptaglist, int loggedUserId)
        {
            string strQuery = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = @"UPDATE TbLedgerBook 
                                SET PaymentActivityName=@payname, 
                                    TransactionId=@transid, 
                                    TransactionDate=@transdate,
                                    TransactionAmount=@transamt, 
                                    IncomeAccountId=@inacct, 
                                    ExpenseAccountId=@exacct,
                                    TransactionDocName=@transdoc, 
                                    TransactionStatus=@transstatus,
                                    UpdatedDate=@upddate, UpdatedBy=@updby 
                            WHERE PaymentId=@payid";
                        _connection.Execute(strQuery, new
                        {
                            payname = inputObj.PaymentActivityName,
                            transid = inputObj.TransactionId,
                            transdate = inputObj.TransactionDate,
                            transamt = inputObj.TransactionAmount,
                            inacct = inputObj.IncomeAccountId,
                            exacct = inputObj.ExpenseAccountId,
                            transdoc = inputObj.TransactionDocName,
                            transstatus = inputObj.TransactionStatus,
                            upddate = DateTime.Now,
                            updby = loggedUserId,
                            payid = inputObj.PaymentId,

                        });

                        //Before adding the tags, we will delete the entries in TbLedgerBookToExpenseTags first and then insert new ones
                        RemovePaymentTags(inputObj.PaymentId, loggedUserId);
                        List<ProjectExpenseTags> objExpenseTagList = JsonConvert.DeserializeObject<List<ProjectExpenseTags>>(exptaglist);
                        foreach (ProjectExpenseTags exptagitem in objExpenseTagList)
                        {
                            AddPaymentTags(exptagitem.TagID, inputObj.PaymentId, loggedUserId);
                        }
                        AddAuditLog(inputObj.ProjectId, "EDIT", inputObj.TransactionId, "TransactionId", "Updated Payment " + inputObj.PaymentActivityName + " Amount " + inputObj.TransactionAmount, loggedUserId);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return ;
        }

        /// <summary>
        /// Function used to delete payment tags based on payid
        /// </summary>
        /// <param name="idPayment"></param>
        /// <param name="loggedUserId"></param>
        public void RemovePaymentTags(int idPayment, int loggedUserId)
        {
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Update TbLedgerBookToExpenseTags Set isDeleted=1, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                                             Where PaymentId=@payid";
                    _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggeduserid = loggedUserId, payid = idPayment });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to add request fund
        /// </summary>
        /// <param name="inputObj"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddRequestFund(ReuestFund inputObj, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
                       
                using (_connection = Utils.Database.GetDBConnection())
                {
                    try
                    {
                    using (var _Tran = new System.Transactions.TransactionScope())
                        {
                            strQuery = @"INSERT INTO TbProjectRequestFund(ProjectId,DonorUser,RequestAmount,IsDeleted,CreatedDate,CreatedBy)
                                   VALUES(@projid,@donor,@reqamt,@isdeleted,now(),@userID); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { projid = inputObj.ProjectId, donor = inputObj.DonorUser, reqamt = inputObj.RequestAmount, isdeleted = 0, userID = loggedUserId }).Single();

                            AddAuditLog(inputObj.ProjectId, "ADD", id, "RequestFundId", "Requested Fund from " + inputObj.DonorUser + " Requested Amount " + inputObj.RequestAmount, loggedUserId);
                        _Tran.Complete();
                    }
                    }
                catch (Exception ex)
                {
                }
                return id;
                }
             
           
        }

        /// <summary>
        /// Function used to 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="acctID"></param>
        /// <returns></returns>
        public List<PaymnetFile> ExportPaymentFile(int projectID, int acctID)
        {
            List<PaymnetFile> objPaymentList = new List<PaymnetFile>();
            string query = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (acctID == 0)
                    {
                        query = @"select LB.PaymentActivityName,LB.TransactionId,LB.TransactionDate,LB.TransactionAmount,
                             LB.TransactionDocName,LB.TransactionStatus,P.ProjectName,
                             ifnull(Bp.BudgetHead,'') as BudgetHeadName, Acc.AccountName as IncomeAccount,Acc1.AccountName as ExpenseAccount
                             from TbLedgerBook LB inner join TbProject P on P.ProjectId = LB.ProjectId
                             inner join TbAccount Acc on Acc.AccountId = LB.IncomeAccountId
                             inner join TbAccount Acc1 on Acc1.AccountId = LB.ExpenseAccountId
                             left join TbBudgetPlan Bp on Bp.ID=LB.BudgetHeadID 
                             where LB.IsDeleted=0 and P.IsCreated = 1 and P.IsDeleted = 0 and LB.ProjectId=@projid Order By LB.PaymentId Desc;";
                        objPaymentList = _connection.Query<PaymnetFile>(query, new { projid = projectID }).ToList();
                    }
                    else
                    {
                        query = @"select LB.PaymentActivityName,LB.TransactionId,LB.TransactionDate,LB.TransactionAmount,
                             LB.TransactionDocName,LB.TransactionStatus,P.ProjectName,
                             ifnull(Bp.BudgetHead,'') as BudgetHeadName, Acc.AccountName as IncomeAccount,Acc1.AccountName as ExpenseAccount
                             from TbLedgerBook LB inner join TbProject P on P.ProjectId = LB.ProjectId
                             inner join TbAccount Acc on Acc.AccountId = LB.IncomeAccountId
                             inner join TbAccount Acc1 on Acc1.AccountId = LB.ExpenseAccountId
                             left join TbBudgetPlan Bp on Bp.ID=LB.BudgetHeadID 
                             where LB.IsDeleted=0 and P.IsCreated = 1 and P.IsDeleted = 0 and LB.ProjectId=@projid  and (LB.IncomeAccountId=@accid or LB.ExpenseAccountId=@accid)Order By LB.PaymentId Desc;";
                        objPaymentList = _connection.Query<PaymnetFile>(query, new { projid = projectID, accid = acctID }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objPaymentList;
        }

        #endregion ManageProjectLedgerBook
        #region AuditLog

        /// <summary>
        /// Function used to add auditlog
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="AuditEvent"></param>
        /// <param name="AuditFromID"></param>
        /// <param name="FromIDColumnName"></param>
        /// <param name="Message"></param>
        /// <param name="loggedUserId"></param>
        public void AddAuditLog(int ProjectID, string AuditEvent, int AuditFromID, string FromIDColumnName, string Message, int loggedUserId)
        {
            try
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
            catch (Exception ex)
            {
            }
        }
        #endregion AuditLog
    }
}
