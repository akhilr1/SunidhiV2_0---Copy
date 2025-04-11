using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Finance
    {
        public int ProjectId { get; set; }
        public int BudgetHeadID { get; set; }
        public string BudgetHeadName { get; set; }
        public string HeadAmount { get; set; }
        public string BudgetDetails { get; set; }
        public string ProjectExpenseTags { get; set; }
        public List<BudgetDetails> BudgetItemList { get; set; }
        public List<ProjectExpenseTags> ExpenseTags { get; set; }

    }
    public class BudgetDetails
    {
        public int ProjectId { get; set; }
        public int BudgetHeadID { get; set; }
        public int BudgetItemID { get; set; }
        public string ItemName { get; set; }
        public string Amount { get; set; }

    }

    public class BudgetPlan
    {
        public string js { get; set; }
 
    }
  

}
