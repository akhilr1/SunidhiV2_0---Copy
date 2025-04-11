using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Venture
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string UtilizedFund { get; set; }
        public string FundUtilized { get; set; }
        public string FundedAmount { get; set; }
        public string TotalUtilizedFund { get; set; }
        public string TotalFundUtilized { get; set; }
        public string TotalFundedAmount { get; set; }
        public string UnutilizedFund { get; set; }
       
    }
    public class VentureFund
    {
        public int ProjectId { get; set; }
        public int ExpenseTagID { get; set; }
        public string TagName { get; set; }
        public string TransactionAmount { get; set; }
   

    }
}
