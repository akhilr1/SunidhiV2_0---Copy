using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SunidhiV2_0.Models
{
    public class OutputReport
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public double NumberOfUnits { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public int ParentActivityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfMoths { get; set; }
        public double TotalExpenditure { get; set; }
        public List<OutputReport> lstOutputReport { get; set; }
        public List<OutputMonthlyReport> lstOutputMonthlyReport { get; set; }
        public int SurveyLogic { get; set; }
        public int TotalRecords { get; set; }
        public double TotalPhysicalAchieved { get; set; }
        public int PhysicalSurveyLogic { get; set; }
    }

    public class OutputMonthlyReport
    {
        public double Units { get; set; }
        public double Budget { get; set; }
        public double Expenditure { get; set; }
        public string Month_Year { get; set; }
        public double PhysicalAchieved { get; set; }
    }
}
