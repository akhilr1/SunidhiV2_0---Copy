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
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace SunidhiV2_0.Code.DAL
{
    public class AccountDAL
    {
        protected IDbConnection _connection;

        #region ManageProjectAccount
        public IEnumerable<Account> GetProjectAccountsByID(int projID)
        {
            List<Account> lstAccount = new List<Account>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT * from TbAccount where IsDeleted=@isdeleted and IsActive=@isactive and ProjectId=@projid Order by AccountId Desc";
                lstAccount = _connection.Query<Account>(strQuery, new
                {
                    isdeleted = 0,
                    isactive = 1,
                    projid = projID
                }).ToList();
            }
            return lstAccount;
        }
        public IEnumerable<string> GetBankList()
        {
            List<string> lstBankName = new List<string>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select distinct Bank from tbIFSCMaster Order by Bank";
                lstBankName = _connection.Query<string>(strQuery).ToList();
            }
            return lstBankName;
        }
        public IEnumerable<string> GetBranchList(string BankName)
        {
            List<string> lstBranch = new List<string>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select distinct BranchName from tbIFSCMaster where Bank=@bank Order by BranchName";
                lstBranch = _connection.Query<string>(strQuery, new { bank = BankName }).ToList();
            }
            return lstBranch;
        }
        public string GetIFSC(string BankName, string BranchName)
        {
            string ifsc = string.Empty;
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select IFSCCode from tbIFSCMaster where Bank=@bank and BranchName=@branch";
                ifsc = _connection.Query<string>(strQuery, new { bank = BankName, branch = BranchName }).FirstOrDefault();
            }
            return ifsc;
        }
        public IEnumerable<string> GetAccountTypeList()
        {
            List<string> lstBankName = new List<string>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select AccountTypeName from TbAccountType";
                lstBankName = _connection.Query<string>(strQuery).ToList();
            }
            return lstBankName;
        }
        public int AddAccount(Account inputObj, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"INSERT INTO TbAccount(AccountName, Description, ContactName, ContactPhone, ContactEmail, ContactAddress, 
                                                   Pincode, BankIFSCCode, BankName, BankBranch, BankAccountType, BankAccountNumber, PAN, 
                                                   ProjectId, UserId, IsActive, IsDeleted, CreatedDate, CreatedBy, Is80Geligible)
                                            VALUES(@accname,@descrip,@contactname,@contactphone,@contactemail,@contactaddress,
                                                   @pincode,@ifsc,@bank,@branch,@acctype,@accnum,@pan,
                                                   @projectid,@userid,@isactive,@isdeleted,@createdate,@createby,@is80g); 
                                             SELECT LAST_INSERT_ID();";
                id = _connection.Query<int>(strQuery, new
                {
                    accname = inputObj.AccountName,
                    descrip = inputObj.Description,
                    contactname = inputObj.ContactName,
                    contactphone = inputObj.ContactPhone,
                    contactemail = inputObj.ContactEmail,
                    contactaddress = inputObj.ContactAddress,
                    pincode = inputObj.Pincode,
                    ifsc = inputObj.BankIFSCCode,
                    bank = inputObj.BankName,
                    branch = inputObj.BankBranch,
                    acctype = inputObj.BankAccountType,
                    accnum = inputObj.BankAccountNumber,
                    pan = inputObj.PAN,
                    projectid = inputObj.ProjectId,
                    userid = inputObj.UserId,
                    isactive = 1,
                    isdeleted = 0,
                    createdate = DateTime.Now,
                    createby = loggedUserId,
                    is80g = inputObj.Is80Geligible
                }).Single();

                AddAuditLog(inputObj.ProjectId, "ADD", id, "AccountId", "Added Account " + inputObj.AccountName, loggedUserId);


            }
            return id;
        }
        public void EditAccount(Account inputObj, int loggedUserId)
        {
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"update TbAccount set AccountName=@accname, 
                                                  Description=@descrip, 
                                                  ContactName=@contactname, 
                                                  ContactPhone=@contactphone, 
                                                  ContactEmail=@contactemail, 
                                                  ContactAddress=@contactaddress, 
                                                  Pincode=@pincode, 
                                                  BankIFSCCode=@ifsc, 
                                                  BankName=@bank, 
                                                  BankBranch=@branch,  
                                                  BankAccountType=@acctype,  
                                                  BankAccountNumber=@accnum,  
                                                  PAN=@pan, 
                                                  UpdatedDate=@updatedate,  
                                                  UpdatedBy=@updateby,  
                                                  Is80Geligible=@is80g,
                                                  UserId=@userid
                            where AccountId=@acctid";
                _connection.Execute(strQuery, new
                {
                    accname = inputObj.AccountName,
                    descrip = inputObj.Description != "null" ? inputObj.Description : null,
                    contactname = inputObj.ContactName != "null" ? inputObj.ContactName : null,
                    contactphone = inputObj.ContactPhone,
                    contactemail = inputObj.ContactEmail != "null" ? inputObj.ContactEmail : null,
                    contactaddress = inputObj.ContactAddress != "null" ? inputObj.ContactAddress : null,
                    pincode = inputObj.Pincode,
                    ifsc = inputObj.BankIFSCCode,
                    bank = inputObj.BankName,
                    branch = inputObj.BankBranch,
                    acctype = inputObj.BankAccountType,
                    accnum = inputObj.BankAccountNumber,
                    pan = inputObj.PAN,
                    updatedate = DateTime.Now,
                    updateby = loggedUserId,
                    is80g = inputObj.Is80Geligible,
                    acctid = inputObj.AccountId,
                    userid= inputObj.UserId
                });
                AddAuditLog(inputObj.ProjectId, "EDIT", inputObj.AccountId, "AccountId", "Updated the activity named " + inputObj.AccountName, loggedUserId);

            }
            return;
        }
        public void RemoveAccount(int loggedUserId, string AccIdList)
        {
            string query = string.Empty;
            List<Account> lstAccount = new List<Account>();

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbAccount Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggeduserid 
                              Where FIND_IN_SET(AccountId,@accidlist)";
                _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggeduserid = loggedUserId, accidlist = AccIdList });


                query = "Select ProjectId,AccountId,AccountName  From TbAccount Where FIND_IN_SET(AccountId,@accidlist)";
                lstAccount = _connection.Query<Account>(query, new { accidlist = AccIdList }).ToList();

                foreach (Account Item in lstAccount)
              {
                ProjectAuditLog objLog = new ProjectAuditLog();
                objLog.ProjectID = Item.ProjectId;
                objLog.AuditEvent = "REMOVE";
                objLog.AuditFromID = Item.AccountId;
                objLog.FromIDColumnName = "AccountId";
                objLog.Message = "Removed Account " + Item.AccountName;
                objLog.CreatedBy = loggedUserId;
                ProjectDAL.AddAuditLog(objLog);
            }

            }
            return;
        }
        #endregion ManageProjectAccount
        #region AuditLog
    
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
