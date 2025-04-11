using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class LedgerBook
    { 
        public int PaymentId { get; set; }
        public string PaymentActivityName { get; set; }
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public int IncomeAccountId { get; set; }
        public int ExpenseAccountId { get; set; }
        public string TransactionDocName { get; set; }
        public string TransactionStatus { get; set; }
        public bool IsActivityLinked { get; set; }
        public int ActivityId { get; set; }
        public int ProjectId { get; set; }
        public List<ProjectExpenseTags> ExpenseTags { get; set; }
        public int BudgetHeadID { get; set; }
        public int BudgetItemID { get; set; }
        public string BudgetHeadName { get; set; }
        public string IncomeAccount { get; set; }
        public string ExpenseAccount { get; set; }
        public string TransactionType { get; set; }
    }

    public class ReuestFund
    {
        public int ProjectId { get; set; }
        public int PaymentID { get; set; }
        public int DonorUser { get; set; }
        public double RequestAmount { get; set; }
        public string NotifyUserList { get; set; }
        public string ProjectName { get; set; }
        public string Comment { get; set; }
        public string ReqDocument { get; set; }
    }

    public class RequestFundNotify
    {
        public string UserName { get; set; }
        public string DonorUser { get; set; }
        public string FundAmount { get; set; }
        public string ProjectName { get; set; }
        public string UserEmailId { get; set; }
    }

    public class PaymnetFile
    {
       
        public string PaymentActivityName { get; set; }
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public string  IncomeAccount { get; set; }
        public string ExpenseAccount { get; set; }
        public string TransactionDocName { get; set; }
        public string TransactionStatus { get; set; }
        public bool IsActivityLinked { get; set; }
        public string ProjectName { get; set; }
        public string BudgetHeadName { get; set; }
    }
}



