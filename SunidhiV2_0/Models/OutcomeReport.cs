using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class OutcomeReport
    {
        public int OutcomeID { get; set; }
        public string OutcomeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SurveyTarget { get; set; }
        public int TargetForTheyear { get; set; }
        public double AchievedTillValue { get; set; }
        public string AchievedTillText { get; set; }
        public double TargetForNextYear { get; set; }
        public int NumberOfMoths { get; set; }
        public List<OutcomeMonthlyReport> lstOutcomeMonthlyReport { get; set; }
        public int SurveyLogic { get; set; }
        public int TotalRecords { get; set; }
        public double TotalAchieved { get; set; }
        public string SDG_Goal { get; set; }
        public string SDG_Goal_Image { get; set; }
    }

    public class OutcomeMonthlyReport
    {
        public double Planned { get; set; }
        public double Achieved { get; set; }
        public string Month_Year { get; set; }
    }
}
