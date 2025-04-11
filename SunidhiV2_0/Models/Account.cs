using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    #region ManageProjectAccount
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int Pincode { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public string PAN { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public bool Is80Geligible { get; set; }
    }

    public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
    #endregion ManageProjectAccount
}



