using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using SunidhiV2_0.Code.Utils;
using SunidhiV2_0.Code.Common;
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace SunidhiV2_0.Code.DAL
{
    public class OutputReportDAL
    {

        protected IDbConnection _connection;

        #region GetOutputReport
        /// <summary>
        /// GetOutputReport
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<OutputReport> GetOutputReport(int ProjectID, int PageNumber = 0)
        {
            List<OutputReport> lstOutputReport = new List<OutputReport>();
            List<OutputReport> lstActualOutputReport = new List<OutputReport>();
            List<ProjectActivityMonthwiseData> lstProjectActivityMonthwiseData = new List<ProjectActivityMonthwiseData>();

            PageNumber = PageNumber * 10;

            try
            {
                string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] MonthData = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

                List<ProjectActivity> lstProjectActivity = new List<ProjectActivity>();

                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select ActivityId,ActivityName,StartDate,EndDate,ParentActivityId,ifnull(PaymentLinkedBudget,0) TotalCost,";
                    strQuery += "ifnull(SurveyTarget,0) NumberOfUnits,ifnull(FinancialSurveyLogic,0) as SurveyLogic,ifnull(SurveyLogic,0) as PhysicalSurveyLogic,(select count(*) from  TbProjectActivity where ProjectId = @ProjectID AND ifnull(IsDeleted,0)=0) as TotalRecords ";
                    strQuery += ",ifnull(UnitCost,0) as UnitCost from TbProjectActivity where ProjectId = @ProjectID AND ifnull(IsDeleted,0)=0  ORDER BY ActivityId DESC;"; //  LIMIT @PageNumber,10 and ActivityId in (1354,1355,1356)  

                    lstOutputReport = _connection.Query<OutputReport>(strQuery, new { ProjectID = ProjectID }).ToList(); //, PageNumber = PageNumber 

                    string strMonthwiseData = "SELECT ActMonth.ID,ActMonth.ActivityId,ActMonth.Target,ActMonth.Budget,ActMonth.ActMonth,ActMonth.ActYear FROM TbProjectActivityMonthwiseData ActMonth JOIN TbProjectActivity prjAct ";
                    strMonthwiseData += "  ON ActMonth.ActivityId = prjAct.ActivityId WHERE prjAct.ProjectId = @ProjectId AND IFNULL(ActMonth.IsActive,0) =1 AND IFNULL(IsDeleted,0)=0;";

                    lstProjectActivityMonthwiseData = _connection.Query<ProjectActivityMonthwiseData>(strMonthwiseData, new { ProjectId = ProjectID }).ToList(); //, PageNumber = PageNumber 
                }

                if (lstOutputReport != null && lstOutputReport.Count > 0)
                {
                    var SelActivityId = lstOutputReport.Select(l => l.ActivityId).Distinct().ToList();
                    string joinedActivityIds = string.Join(",", SelActivityId);
                    lstProjectActivity = getData(ProjectID, joinedActivityIds);

                    List<OutputReport> lstParentOutputReport = new List<OutputReport>();
                    lstParentOutputReport = lstOutputReport.Where(x => x.ParentActivityId == 0).ToList();

                    if (lstParentOutputReport != null && lstParentOutputReport.Count > 0)
                    {
                        for (int first = 0; first < lstParentOutputReport.Count; first++)
                        {
                            lstParentOutputReport[first].ActivityCode = alpha[first] + (first + 1);

                            List<OutputReport> lstFirstChildOutputReport = new List<OutputReport>();

                            lstFirstChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstParentOutputReport[first].ActivityId).ToList();

                            if (lstFirstChildOutputReport != null && lstFirstChildOutputReport.Count > 0)
                            {
                                for (int second = 0; second < lstFirstChildOutputReport.Count; second++)
                                {
                                    lstFirstChildOutputReport[second].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1);

                                    List<OutputReport> lstSecondChildOutputReport = new List<OutputReport>();

                                    lstSecondChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstFirstChildOutputReport[second].ActivityId).ToList();

                                    if (lstSecondChildOutputReport != null && lstSecondChildOutputReport.Count > 0)
                                    {
                                        for (int third = 0; third < lstSecondChildOutputReport.Count; third++)
                                        {
                                            lstSecondChildOutputReport[third].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1);

                                            List<OutputReport> lstThirdChildOutputReport = new List<OutputReport>();

                                            lstThirdChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstSecondChildOutputReport[third].ActivityId).ToList();

                                            if (lstThirdChildOutputReport != null && lstThirdChildOutputReport.Count > 0)
                                            {
                                                for (int fourth = 0; fourth < lstThirdChildOutputReport.Count; fourth++)
                                                {
                                                    lstThirdChildOutputReport[fourth].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1);

                                                    List<OutputReport> lstFourthChildOutputReport = new List<OutputReport>();

                                                    lstFourthChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstThirdChildOutputReport[fourth].ActivityId).ToList();

                                                    if (lstFourthChildOutputReport != null && lstFourthChildOutputReport.Count > 0)
                                                    {
                                                        for (int fifth = 0; fifth < lstFourthChildOutputReport.Count; fifth++)
                                                        {
                                                            lstFourthChildOutputReport[fifth].ActivityCode = alpha[first] + "." + 1 + "." + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1);

                                                            List<OutputReport> lstFiveChildOutputReport = new List<OutputReport>();

                                                            lstFiveChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstFourthChildOutputReport[fifth].ActivityId).ToList();

                                                            if (lstFiveChildOutputReport != null && lstFiveChildOutputReport.Count > 0)
                                                            {
                                                                for (int sixth = 0; sixth < lstFiveChildOutputReport.Count; sixth++)
                                                                {
                                                                    lstFiveChildOutputReport[sixth].ActivityCode = alpha[first] + "." + 1 + "." + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1);

                                                                    List<OutputReport> lstSixChildOutputReport = new List<OutputReport>();

                                                                    lstSixChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstFiveChildOutputReport[sixth].ActivityId).ToList();

                                                                    if (lstSixChildOutputReport != null && lstSixChildOutputReport.Count > 0)
                                                                    {
                                                                        for (int seventh = 0; seventh < lstSixChildOutputReport.Count; seventh++)
                                                                        {
                                                                            lstSixChildOutputReport[seventh].ActivityCode = alpha[first] + "." + 1 + "." + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1) + "." + (seventh + 1);

                                                                            List<OutputReport> lstSevenChildOutputReport = new List<OutputReport>();

                                                                            lstSevenChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstSixChildOutputReport[seventh].ActivityId).ToList();

                                                                            if (lstSevenChildOutputReport != null && lstSevenChildOutputReport.Count > 0)
                                                                            {
                                                                                for (int eighth = 0; eighth < lstSevenChildOutputReport.Count; eighth++)
                                                                                {
                                                                                    lstSevenChildOutputReport[eighth].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1) + "." + (seventh + 1) + "." + (eighth + 1);

                                                                                    List<OutputReport> lstEightChildOutputReport = new List<OutputReport>();

                                                                                    lstEightChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstSevenChildOutputReport[eighth].ActivityId).ToList();

                                                                                    if (lstEightChildOutputReport != null && lstEightChildOutputReport.Count > 0)
                                                                                    {
                                                                                        for (int ninth = 0; ninth < lstEightChildOutputReport.Count; ninth++)
                                                                                        {
                                                                                            lstEightChildOutputReport[ninth].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1) + "." + (seventh + 1) + "." + (eighth + 1) + "." + (ninth + 1);
                                                                                            List<OutputReport> lstNinthChildOutputReport = new List<OutputReport>();

                                                                                            lstNinthChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstEightChildOutputReport[ninth].ActivityId).ToList();

                                                                                            if (lstNinthChildOutputReport != null && lstNinthChildOutputReport.Count > 0)
                                                                                            {
                                                                                                for (int tenth = 0; tenth < lstNinthChildOutputReport.Count; tenth++)
                                                                                                {
                                                                                                    lstNinthChildOutputReport[tenth].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1) + "." + (seventh + 1) + "." + (eighth + 1) + "." + (ninth + 1) + "." + (tenth + 1);
                                                                                                    List<OutputReport> lstleventhChildOutputReport = new List<OutputReport>();

                                                                                                    lstleventhChildOutputReport = lstOutputReport.Where(x => x.ParentActivityId == lstNinthChildOutputReport[tenth].ActivityId).ToList();

                                                                                                    if (lstleventhChildOutputReport != null && lstleventhChildOutputReport.Count > 0)
                                                                                                    {
                                                                                                        for (int leventh = 0; leventh < lstleventhChildOutputReport.Count; leventh++)
                                                                                                        {
                                                                                                            lstleventhChildOutputReport[leventh].ActivityCode = alpha[first] + "." + 1 + "." + (second + 1) + "." + (third + 1) + "." + (fourth + 1) + "." + (fifth + 1) + "." + (sixth + 1) + "." + (seventh + 1) + "." + (eighth + 1) + "." + (ninth + 1) + "." + (tenth + 1) + "." + (leventh + 1);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        lstleventhChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstNinthChildOutputReport[tenth].ActivityId).ToList();

                                                                                                        if (lstleventhChildOutputReport != null && lstleventhChildOutputReport.Count > 0)
                                                                                                        {
                                                                                                            lstleventhChildOutputReport = GetOutPutMonwiseCalculation(lstleventhChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);

                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                lstNinthChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstEightChildOutputReport[ninth].ActivityId).ToList();

                                                                                                if (lstNinthChildOutputReport != null && lstNinthChildOutputReport.Count > 0)
                                                                                                {
                                                                                                    lstNinthChildOutputReport = GetOutPutMonwiseCalculation(lstNinthChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                                                                }
                                                                                            }

                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        lstEightChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstSevenChildOutputReport[eighth].ActivityId).ToList();

                                                                                        if (lstEightChildOutputReport != null && lstEightChildOutputReport.Count > 0)
                                                                                        {
                                                                                            lstEightChildOutputReport = GetOutPutMonwiseCalculation(lstEightChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                lstSevenChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstSixChildOutputReport[seventh].ActivityId).ToList();

                                                                                if (lstSevenChildOutputReport != null && lstSevenChildOutputReport.Count > 0)
                                                                                {
                                                                                    lstSevenChildOutputReport = GetOutPutMonwiseCalculation(lstSevenChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                                                }
                                                                            }
                                                                        }

                                                                        lstFiveChildOutputReport[sixth].lstOutputReport = new List<OutputReport>();
                                                                        lstFiveChildOutputReport[sixth].lstOutputReport.AddRange(lstSixChildOutputReport);
                                                                    }
                                                                    else
                                                                    {
                                                                        lstSixChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstFiveChildOutputReport[sixth].ActivityId).ToList();

                                                                        if (lstSixChildOutputReport != null && lstSixChildOutputReport.Count > 0)
                                                                        {
                                                                            lstSixChildOutputReport = GetOutPutMonwiseCalculation(lstSixChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                                        }
                                                                    }
                                                                }

                                                                lstFourthChildOutputReport[fifth].lstOutputReport = new List<OutputReport>();
                                                                lstFourthChildOutputReport[fifth].lstOutputReport.AddRange(lstFiveChildOutputReport);
                                                            }
                                                            else
                                                            {
                                                                lstFiveChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstFourthChildOutputReport[fifth].ActivityId).ToList();

                                                                if (lstFiveChildOutputReport != null && lstFiveChildOutputReport.Count > 0)
                                                                {
                                                                    lstFiveChildOutputReport = GetOutPutMonwiseCalculation(lstFiveChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                                }
                                                            }
                                                        }

                                                        lstThirdChildOutputReport[fourth].lstOutputReport = new List<OutputReport>();
                                                        lstThirdChildOutputReport[fourth].lstOutputReport.AddRange(lstFourthChildOutputReport);
                                                    }
                                                    else
                                                    {
                                                        lstFourthChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstThirdChildOutputReport[fourth].ActivityId).ToList();

                                                        if (lstFourthChildOutputReport != null && lstFourthChildOutputReport.Count > 0)
                                                        {
                                                            lstFourthChildOutputReport = GetOutPutMonwiseCalculation(lstFourthChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                        }
                                                    }
                                                }

                                                lstSecondChildOutputReport[third].lstOutputReport = new List<OutputReport>();
                                                lstSecondChildOutputReport[third].lstOutputReport.AddRange(lstThirdChildOutputReport);
                                            }
                                            else
                                            {
                                                lstThirdChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstSecondChildOutputReport[third].ActivityId).ToList();

                                                if (lstThirdChildOutputReport != null && lstThirdChildOutputReport.Count > 0)
                                                {
                                                    lstThirdChildOutputReport = GetOutPutMonwiseCalculation(lstThirdChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                                }
                                            }
                                        }

                                        lstFirstChildOutputReport[second].lstOutputReport = new List<OutputReport>();
                                        lstFirstChildOutputReport[second].lstOutputReport.AddRange(lstSecondChildOutputReport);
                                    }
                                    else
                                    {
                                        lstSecondChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstFirstChildOutputReport[second].ActivityId).ToList();

                                        if (lstSecondChildOutputReport != null && lstSecondChildOutputReport.Count > 0)
                                        {
                                            lstSecondChildOutputReport = GetOutPutMonwiseCalculation(lstSecondChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                        }
                                    }
                                }

                                lstParentOutputReport[first].lstOutputReport = new List<OutputReport>();
                                lstParentOutputReport[first].lstOutputReport.AddRange(lstFirstChildOutputReport);
                            }
                            else
                            {
                                lstFirstChildOutputReport = lstOutputReport.Where(x => x.ActivityId == lstParentOutputReport[first].ActivityId).ToList();

                                if (lstFirstChildOutputReport != null && lstFirstChildOutputReport.Count > 0)
                                {
                                    lstFirstChildOutputReport = GetOutPutMonwiseCalculation(lstFirstChildOutputReport, lstProjectActivity, lstProjectActivityMonthwiseData);
                                }
                            }

                            lstParentOutputReport[first].ActivityCode = alpha[first] + "." + 1;
                            lstActualOutputReport.Add(lstParentOutputReport[first]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          //  string json = JsonConvert.SerializeObject(lstActualOutputReport);
            return lstActualOutputReport;
        }
        #endregion GetOutputReport

        #region getData
        /// <summary>
        /// getData
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ProjectActivity> getData(int ProjectId = 0, string joinedActivityIds = "")
        {
            List<ActivityDataCollectionDetails> objActivityDataCollectionDetails = new List<ActivityDataCollectionDetails>();
            List<ActivityDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<ActivityDataCollectionDetails>();
            List<ProjectActivity> objProjectActivityList = new List<ProjectActivity>();

            string query = string.Empty;

            double result = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query2 = @"select ProjectActivity.ActivityId,ProjectActivity.ProjectId,ParentActivityId,ProjectActivity.ActivityName,ProjectActivity.Description,ProjectActivity.StartDate,ProjectActivity.EndDate
                                ,ProjectActivity.WeightagePercentage,ProjectActivity.ActivityType,ProjectActivity.SurveyId,ProjectActivity.SurveySectionId,BeneficiaryTypeQuestionID,
                                ProjectActivity.SurveyQuestionId,ProjectActivity.SurveyResponseId,ProjectActivity.SurveyLogic,ProjectActivity.SurveyTarget,ProjectActivity.IsPaymentLinked,ProjectActivity.PaymentLinkedBudget,
                                ProjectActivity.WorkflowId,ProjectActivity.FinanceWorkflowId,ProjectActivity.Periodicity,ProjectActivity.IsDeleted,ProjectActivity.CreatedDate,ProjectActivity.UpdatedDate,ProjectActivity.CreatedBy,ProjectActivity.UpdatedBy
                                ,TbPeriodicity.PeriodicityType as ReportingFrequencyName,ProjectActivity.Progress,ProjectSurvey.BeneficiaryTypeID as BeneficiaryTypeID,
                                (SELECT COUNT(*) FROM TbProjectActivity WHERE ParentActivityId = ProjectActivity.ActivityId) SubActivityCount  ,
                                (SELECT SUM(WeightagePercentage) FROM  TbProjectActivity WHERE ParentActivityId = ProjectActivity.ActivityId AND IFNULL(IsDeleted,0) = 0) SubActivityTotalWeightage,
                                ifnull(ProjectActivity.FinancialAdditionalSectionId,0) as FinancialAdditionalSectionId,ifnull(ProjectActivity.FinancialAdditionalQuestionType,false) as FinancialAdditionalQuestionType,ifnull(ProjectActivity.FinancialAdditionalQuestionId,0) as FinancialAdditionalQuestionId, 
                                ifnull(ProjectActivity.AdditionalSectionId,0) as AdditionalSectionId,ifnull(ProjectActivity.AdditionalQuestionType,false) as AdditionalQuestionType,ifnull(ProjectActivity.AdditionalQuestionId,0) as AdditionalQuestionId 
                                from TbProjectActivity ProjectActivity left join TbProjectSurvey ProjectSurvey on ProjectSurvey.SurveyID = ProjectActivity.SurveyId left join TbPeriodicity TbPeriodicity 
                                on TbPeriodicity.PeriodicityId = ProjectActivity.Periodicity where ProjectActivity.IsDeleted = @isdeleted and ProjectActivity.ProjectId = @projid and FIND_IN_SET(ProjectActivity.ActivityId, @joinedActivityIds)  
                                Order By ParentActivityId asc,ProjectActivity.ActivityId Desc;";

                    objProjectActivityList = _connection.Query<ProjectActivity>(query2, new { isdeleted = 0, projid = ProjectId, joinedActivityIds = joinedActivityIds }).ToList();

                    int activityID = 0;
                    int logicType = 0;
                    int PhysicallogicType = 0;
                    string SurveyTarget = "";

                    foreach (ProjectActivity actItem in objProjectActivityList)
                    {
                        if (actItem.ActivityType == "Quantitative")
                        {
                            if (actItem.SubActivityCount == 0)
                            {
                                activityID = actItem.ActivityId;
                                logicType = actItem.FinancialSurveyLogic;
                                SurveyTarget = actItem.SurveyTarget;
                                PhysicallogicType = actItem.SurveyLogic;

                                query = "SELECT ACT.ID,ACT.ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                                query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType,srvLogic.SurveyLogic,srvLogic.SurveyAnswer,ifnull(srvLogic.LogicCondition,0) as LogicCondition,ifnull(ACT.TypeofLogic,1) TypeofLogic ";
                                query += " FROM TbProjectSurvey SRV LEFT JOIN TbOutputSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                                query += " LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                                query += " LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                                query += " LEFT JOIN TbOutputSurveyLogic srvLogic ON srvLogic.SurveyCollectionID =  ACT.ID ";
                                query += "where ACT.ActivityId = @ActivityId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1 and ifnull(srvLogic.IsActive,0)=1; ";

                                objActivityDataCollectionDetails = _connection.Query<ActivityDataCollectionDetails>(query, new { ActivityId = activityID }).ToList();

                                List<ActivityDataCollectionDetails> objActivityDataCollectionDetailsFinancial = new List<ActivityDataCollectionDetails>();
                                List<ActivityDataCollectionDetails> objActivityDataCollectionDetailsPhysical = new List<ActivityDataCollectionDetails>();

                                if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                                {
                                    objActivityDataCollectionDetailsFinancial = objActivityDataCollectionDetails.Where(x => x.TypeofLogic == 2).ToList();
                                    actItem.lstOutPutCount = new List<OutPutCount>();
                                    actItem.lstOutPutCount.AddRange(GetResponseForOutputReport(objActivityDataCollectionDetails, activityID, actItem.FinancialSurveyLogic, SurveyTarget, actItem.FinancialAdditionalSectionId, actItem.FinancialAdditionalQuestionId, actItem.FinancialAdditionalQuestionType, 2));
                                    objActivityDataCollectionDetailsPhysical = objActivityDataCollectionDetails.Where(x => x.TypeofLogic == 1).ToList();
                                    actItem.lstOutPutCountPhysical = new List<OutPutCount>();
                                    if (objActivityDataCollectionDetailsPhysical != null && objActivityDataCollectionDetailsPhysical.Count > 0)
                                    {
                                        actItem.lstOutPutCountPhysical.AddRange(GetResponseForOutputReport(objActivityDataCollectionDetails, activityID, actItem.SurveyLogic, SurveyTarget, Convert.ToInt32(actItem.AdditionalSectionId), actItem.AdditionalQuestionId, actItem.AdditionalQuestionType, 1));
                                    }
                                }

                                //query = "SELECT ACT.ID,ACT.ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                                //query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType,ifnull(ACT.TypeofLogic,1) TypeofLogic FROM TbProjectSurvey SRV ";
                                //query += "LEFT JOIN TbOutputSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                                //query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                                //query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                                //query += "where ACT.ActivityId = @ActivityId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1;";

                                //objActivityDataCollectionDetails = _connection.Query<ActivityDataCollectionDetails>(query, new { ActivityId = activityID }).ToList();

                                //if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                                //{
                                //var mySKUs = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                //string joinedQuestionIds = string.Join(",", mySKUs);

                                //var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                                //string joinedBeneficiaryTypeID = string.Join(",", mySKUs1);

                                //var benficiaryIds = objActivityDataCollectionDetails.Where(x => x.BeneficiaryQuestionID > 0).Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                                //string joinedBeneficiaryQuestionID = string.Join(",", benficiaryIds);

                                //List<ActivitySurveyResponse> lstActivitySurveyResponse = new List<ActivitySurveyResponse>();
                                //List<ActivitySurveyResponse> lstActivitySurveyResponse1 = new List<ActivitySurveyResponse>();
                                //List<ActivitySurveyResponse> lstBeneficiarySurveyResponse = new List<ActivitySurveyResponse>();

                                //if (joinedQuestionIds != null && joinedQuestionIds.Length > 0)
                                //{
                                //    string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where QuestionID in(" + joinedQuestionIds + ") and ifnull(IsActive,0)=1;";
                                //    lstActivitySurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry).ToList();

                                //    lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                //}

                                //if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0)
                                //{
                                //    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                                //    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                                //    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID,reg.CreatedDate ";
                                //    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID in(" + joinedBeneficiaryQuestionID + ") and ifnull(reg.IsDeleted,0)=0;";
                                //    lstBeneficiarySurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTBenQry).ToList();

                                //    lstActivitySurveyResponse1 = lstBeneficiarySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                //}
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //  return result > 100 ? 100 : result;

            return objProjectActivityList;
        }
        #endregion getData

        #region GetResponseData
        /// <summary>
        /// GetResponseData
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public List<OutPutCount> GetResponseData(List<ActivityDataCollectionDetails> objActivityDataCollectionDetails, List<ActivitySurveyResponse> lstActivitySurveyResponse1, List<ActivitySurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<ActivitySurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            double progressPercentage = 0;
            List<OutPutCount> lstOutPutCount = new List<OutPutCount>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();
            List<ActivitySurveyResponse> lstBeneficiarySurveyResponseAll = new List<ActivitySurveyResponse>();

            List<BeneficiaryResponse> lstBeneficiaryResponse = null;
            lstBeneficiaryResponse = new List<BeneficiaryResponse>();
            List<ActivitySurveyResponse> lstActivitySurveyResponse5 = new List<ActivitySurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutPutCount objOutPutCount = new OutPutCount();

                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutputSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutputSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID AND IFNULL(srvCollection.TypeofLogic,0)=2;";
                List<SurveyPrimeLogic> lstSurveyPrimeLogic = new List<SurveyPrimeLogic>();

                lstSurveyPrimeLogic = _connection.Query<SurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();


                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0) //&& lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID,reg.CreatedDate ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse1 = new List<ActivitySurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<ActivitySurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<ActivitySurveyResponse> lstBeneficiarySurveyResponse2 = new List<ActivitySurveyResponse>();

                                //string QSTQry4 = "select DISTINCT reg.BeneficiaryID ResponseID,reg.Answer,reg.QuestionID,@SurveyID AS SurveyID,1 AS SubmissionCount,reg.CreatedDate from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID  WHERE reg.BeneficiaryID in(" + joinedResponseID + ")  and reg.QuestionID =@QuestionID ";

                                //lstBeneficiarySurveyResponse2 = _connection.Query<ActivitySurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                //if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                //{
                                //    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                //}
                                //else
                                //{
                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,CreatedDate from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<ActivitySurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
                                // }
                            }
                        }
                    }
                    else
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                        }
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<ActivitySurveyResponse> lstActivitySurveyResponse3 = new List<ActivitySurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<ActivitySurveyResponse> lstActivitySurveyResponse4 = new List<ActivitySurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    ActivitySurveyResponse objActivitySurveyResponse = new ActivitySurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        QSTQry = "select RES.ResponseID,reg.Answer,reg.QuestionID,RES.SurveyID,RES.SyncTaskID,ifnull(RES.SubmissionCount,1) SubmissionCount,reg.CreatedDate ";
                                        QSTQry += "from TbProjectBeneficiary ben JOIN TbBeneficiaryRegistrationDetails reg ON ben.BeneficiaryID = reg.BeneficiaryID ";
                                        QSTQry += "JOIN TbProjectSurveyResponses RES ON RES.ResponseID = reg.BeneficiaryID WHERE ifnull(ben.IsActive,0)=1  and ";
                                        QSTQry += "ifnull(RES.IsActive,0)=1 and RES.ResponseID=@ResponseID  and RES.SyncTaskID in(" + joinedSyncTaskID + ")  and reg.QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

                                        if (lstActivitySurveyResponse4 != null && lstActivitySurveyResponse4.Count > 0)
                                        {
                                            foreach (var itm in lstActivitySurveyResponse4)
                                            {
                                                if (lstActivitySurveyResponse5.Count == 0)
                                                {
                                                    lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    ActivitySurveyResponse objActivitySurveyResponse = new ActivitySurveyResponse();
                                                    objActivitySurveyResponse = lstActivitySurveyResponse5.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault();

                                                    if (objActivitySurveyResponse == null)
                                                    {
                                                        lstActivitySurveyResponse5.Add(lstActivitySurveyResponse4.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).FirstOrDefault());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (additionalSurveyQuestionId > 0)
                            {
                                if (i == objActivityDataCollectionDetails.Count - 1 && cnt == lstActivitySurveyResponse1.Count - 1)
                                {
                                    if (lstActivitySurveyResponse5 != null && lstActivitySurveyResponse5.Count > 0)
                                    {
                                        foreach (var item in lstActivitySurveyResponse5)
                                        {
                                            BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                                            objBeneficiaryResponse1.ResponseID = item.ResponseID;
                                            objBeneficiaryResponse1.SurveyID = item.SurveyID;
                                            objBeneficiaryResponse1.AnwerString = item.Answer;  //lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                                            objBeneficiaryResponse1.CreatedDate = item.CreatedDate;
                                            objBeneficiaryResponse1.QuestionID = item.QuestionID;
                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    foreach (var item in lstActivitySurveyResponse3)
                                    {
                                        BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                                        objBeneficiaryResponse1.ResponseID = item.ResponseID;
                                        objBeneficiaryResponse1.SurveyID = item.SurveyID;
                                        objBeneficiaryResponse1.AnwerString = item.Answer;  //lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                                        objBeneficiaryResponse1.CreatedDate = item.CreatedDate;
                                        objBeneficiaryResponse1.QuestionID = item.QuestionID;
                                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    //string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    List<OutPutCount> lastOutPutCount2 = new List<OutPutCount>();
                    //query1 = "SELECT Answer,0 as ResponseCount,SurveyID,CreatedDate,QuestionID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                    //query1 += subqury;
                    //lastOutPutCount2 = _connection.Query<OutPutCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                    //if (lastOutPutCount2 != null)
                    //{
                    //    lstOutPutCount.AddRange(lastOutPutCount2);
                    //}

                    string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT Answer,0 as ResponseCount,SurveyID,CreatedDate,QuestionID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subqury;
                        //  query1 += " GROUP BY SurveyID;";
                        lstOutPutCount = _connection.Query<OutPutCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();
                    }
                    else
                    {
                        List<OutPutQuestionRSP> lstOutPutQuestionRSP = new List<OutPutQuestionRSP>();
                        query1 = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,SurveyID,CreatedDate,QuestionID from TbProjectSurveyResponses where  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                        query1 += subqury;

                        lstOutPutQuestionRSP = _connection.Query<OutPutQuestionRSP>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        if (lstOutPutQuestionRSP != null && lstOutPutQuestionRSP.Count > 0)
                        {
                            var mySKUs = lstOutPutQuestionRSP.Select(l => l.SyncTaskID).Distinct().ToList();
                            string joinedSyncTaskID = string.Join(",", mySKUs);

                            if (joinedSyncTaskID != null && joinedSyncTaskID.Length > 0)
                            {
                                objOutPutCount.SurveyID = lstOutPutQuestionRSP[0].SurveyID;

                                List<OutPutQuestionRSP> lstOutPutQuestionRSP1 = new List<OutPutQuestionRSP>();

                                string QSTQry = "select Answer as AnswerString,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,CreatedDate,QuestionID from TbProjectSurveyResponses where SyncTaskID in(" + joinedSyncTaskID + ") and ifnull(IsActive,0)=1 and QuestionID=@QuestionID;";
                                lstOutPutQuestionRSP1 = _connection.Query<OutPutQuestionRSP>(QSTQry, new { QuestionID = additionalSurveyQuestionId }).ToList();

                                if (lstOutPutQuestionRSP1 != null && lstOutPutQuestionRSP1.Count > 0)
                                {
                                    foreach (var itm in lstOutPutQuestionRSP)
                                    {
                                        if (lstOutPutQuestionRSP2.Count == 0)
                                        {
                                            lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                        }
                                        else
                                        {
                                            OutPutQuestionRSP objOutPutQuestionRSP = new OutPutQuestionRSP();
                                            objOutPutQuestionRSP = lstOutPutQuestionRSP2.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();

                                            if (objOutPutQuestionRSP == null)
                                            {
                                                lstOutPutQuestionRSP2.Add(lstOutPutQuestionRSP1.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //if (i == objActivityDataCollectionDetails.Count - 1 && additionalSurveyQuestionId > 0)
                    //{
                    //    double answerSum = lstOutPutQuestionRSP2.Sum(a => Convert.ToDouble(a.AnswerString));
                    //    int responseCount = lstOutPutQuestionRSP2.Sum(a => a.SubmissionCount);

                    //    if (answerSum > 0 && responseCount > 0)
                    //    {
                    //        objOutPutCount.AnswerSum = answerSum;
                    //        objOutPutCount.ResponseCount = responseCount;
                    //    }
                    //}

                    if (lstOutPutQuestionRSP2 != null && lstOutPutQuestionRSP2.Count > 0)
                    {
                        foreach (var itm1 in lstOutPutQuestionRSP2)
                        {
                            OutPutCount objOutPutCount1 = new OutPutCount();
                            objOutPutCount1.Answer = itm1.AnswerString;
                            objOutPutCount1.SurveyID = itm1.SurveyID;
                            objOutPutCount1.CreatedDate = itm1.CreatedDate;
                            objOutPutCount1.QuestionID = itm1.QuestionID;
                            lstOutPutCount.Add(objOutPutCount1);
                        }
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                foreach (var item in lstBeneficiarySurveyResponseAll)
                {
                    BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = item.ResponseID;
                    objBeneficiaryResponse1.SurveyID = item.SurveyID;
                    objBeneficiaryResponse1.AnwerString = item.Answer;  //lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                    objBeneficiaryResponse1.CreatedDate = item.CreatedDate;
                    objBeneficiaryResponse1.QuestionID = item.QuestionID;
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                foreach (var item in lstBeneficiaryResponse)
                {
                    OutPutCount objOutPutCount = new OutPutCount();
                    objOutPutCount.ResponseId = item.ResponseID;
                    objOutPutCount.Answer = item.AnwerString;
                    objOutPutCount.SurveyID = item.SurveyID;
                    objOutPutCount.QuestionID = item.QuestionID;
                    objOutPutCount.CreatedDate = item.CreatedDate;
                    lstOutPutCount.Add(objOutPutCount);
                }
            }

            return lstOutPutCount;
        }
        #endregion GetQuantitativeAverage

        #region GetQuantitativeCumulativeSum
        /// <summary>
        /// GetQuantitativeCumulativeSum
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public List<OutPutCount> GetQuantitativeCumulativeSum(List<ActivityDataCollectionDetails> objActivityDataCollectionDetails, List<ActivitySurveyResponse> lstActivitySurveyResponse1, List<ActivitySurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<ActivitySurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutPutCount> lstOutPutCount = new List<OutPutCount>();
            List<BeneficiaryResponse> lstBeneficiaryResponse = new List<BeneficiaryResponse>();

            List<ActivitySurveyResponse> lstBeneficiarySurveyResponseAll = new List<ActivitySurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutPutCount objOutPutCount = new OutPutCount();

                //to get survey prime logic -  starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition  from TbOutputSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutputSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<SurveyPrimeLogic> lstSurveyPrimeLogic = new List<SurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<SurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0) //&& lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse1 = new List<ActivitySurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<ActivitySurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        List<ActivitySurveyResponse> lstActivitySurveyResponse6 = new List<ActivitySurveyResponse>();

                        string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                        string QSTQry = "SELECT ResponseID,ifnull(Answer,0) Answer,QuestionID, SurveyID,SyncTaskID,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                        QSTQry += "QuestionID = @QuestionID  ";
                        QSTQry += subqury;
                        lstActivitySurveyResponse6 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<ActivitySurveyResponse> lstActivitySurveyResponse3 = new List<ActivitySurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                lstActivitySurveyResponse3 = lstActivitySurveyResponse6.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).ToList();
                                // lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                //string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                //string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                //QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                //QSTQry += subqury;
                                //lstActivitySurveyResponse3 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //double sumValue = 0;
                                //int cnt1 = 0;

                                //for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                //{
                                //    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                //}

                                //if (sumValue > 0)
                                //{
                                BeneficiaryResponse objBeneficiaryResponse = new BeneficiaryResponse();
                                objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                objBeneficiaryResponse.Anwer = Convert.ToDouble(lstActivitySurveyResponse3[0].Answer);
                                objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                objBeneficiaryResponse.CreatedDate = lstActivitySurveyResponse3[0].CreatedDate;
                                lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                //}
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    List<OutPutCount> lastOutPutCount2 = new List<OutPutCount>();

                    string subQury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    query1 = "SELECT Answer ,0 as ResponseCount,SurveyID,CreatedDate FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                    query1 += subQury;
                    //query1 += " GROUP BY SurveyID;";

                    lastOutPutCount2 = _connection.Query<OutPutCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();
                    if (lastOutPutCount2 != null)
                    {
                        lstOutPutCount.AddRange(lastOutPutCount2);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<ActivitySurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse22 = new List<ActivitySurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = Convert.ToDouble(lstBeneficiarySurveyResponse22[0].Answer);
                        objBeneficiaryResponse1.CreatedDate = lstBeneficiarySurveyResponse22[0].CreatedDate;
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<BeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutPutCount objOutPutCount = new OutPutCount();
                        //objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.Answer = lstBeneficiaryResponse[g].Anwer.ToString();
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        objOutPutCount.CreatedDate = lstBeneficiaryResponse[g].CreatedDate;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            return lstOutPutCount;
        }
        #endregion GetQuantitativeCumulativeSum

        #region GetQuantitativeUniqueResponses
        /// <summary>
        /// GetQuantitativeUniqueResponses
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="lstOutPutCount"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public List<OutPutCount> GetQuantitativeUniqueResponses(List<ActivityDataCollectionDetails> objActivityDataCollectionDetails, List<ActivitySurveyResponse> lstActivitySurveyResponse1, List<ActivitySurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<ActivitySurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutPutCount> lstOutPutCount = new List<OutPutCount>();

            List<BeneficiaryResponse> lstBeneficiaryResponse = new List<BeneficiaryResponse>();

            List<ActivitySurveyResponse> lstBeneficiarySurveyResponseAll = new List<ActivitySurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutPutCount objOutPutCount = new OutPutCount();

                //to get survey prime logic - starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutputSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutputSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<SurveyPrimeLogic> lstSurveyPrimeLogic = new List<SurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<SurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0) //&& lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<ActivitySurveyResponse> lstBeneficiarySurveyResponse1 = new List<ActivitySurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<ActivitySurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        List<ActivitySurveyResponse> lstActivitySurveyResponse6 = new List<ActivitySurveyResponse>();

                        string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                        string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                        QSTQry += "QuestionID = @QuestionID";
                        QSTQry += subqury;
                        lstActivitySurveyResponse6 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<ActivitySurveyResponse> lstActivitySurveyResponse3 = new List<ActivitySurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                //string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                //QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                //QSTQry += subqury;
                                //lstActivitySurveyResponse3 = _connection.Query<ActivitySurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                lstActivitySurveyResponse3 = lstActivitySurveyResponse6.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                lstActivitySurveyResponse3 = lstActivitySurveyResponse3.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                                BeneficiaryResponse objBeneficiaryResponse = new BeneficiaryResponse();
                                objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                objBeneficiaryResponse.Anwer = lstActivitySurveyResponse3.Count;
                                objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                objBeneficiaryResponse.CreatedDate = lstActivitySurveyResponse3[0].CreatedDate;
                                lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    List<OutPutCount> lstOutPutCount1 = new List<OutPutCount>();
                    query1 = " SELECT Answer,CreatedDate";
                    query1 += " FROM TbProjectSurveyResponses WHERE QuestionID =  @QuestionId AND ifnull(IsActive,0) = 1";
                    query1 += GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    //query1 += " GROUP BY QuestionID;";

                    lstOutPutCount1 = _connection.Query<OutPutCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                    if (lstOutPutCount1 != null && lstOutPutCount1.Count > 0)
                    {
                        lstOutPutCount.AddRange(lstOutPutCount1);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<ActivitySurveyResponse> lstBeneficiarySurveyResponse22 = new List<ActivitySurveyResponse>();
                lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                {
                    BeneficiaryResponse objBeneficiaryResponse1 = new BeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                    objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                    objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Count();
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<BeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutPutCount objOutPutCount = new OutPutCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToInt32(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        objOutPutCount.CreatedDate = lstBeneficiaryResponse[g].CreatedDate;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }


            return lstOutPutCount;
        }
        #endregion GetQuantitativeUniqueResponses

        #region GetNonBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetNonBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetNonBeneficiarySurveyLogicQuery(List<SurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.MultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetNonBeneficiarySurveyLogicQuery

        #region GetBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <returns></returns>
        public List<ActivitySurveyResponse> GetBeneficiarySurveyLogicQuery(List<SurveyPrimeLogic> lstSurveyPrimeLogic, List<ActivitySurveyResponse> lstBeneficiarySurveyResponse, ActivityDataCollectionDetails objActivityDataCollectionDetails)
        {
            if (lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)
            {
                foreach (var itm in lstSurveyPrimeLogic)
                {
                    switch (itm.SurveyLogicPrime)
                    {
                        case 3:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() == itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 4:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) > Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 5:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) >= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 6:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) < Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 7:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) <= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                        case 8:
                            lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() != itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.BeneficiaryQuestionID).ToList();
                            break;
                    }
                }
            }
            return lstBeneficiarySurveyResponse;
        }
        #endregion GetBeneficiarySurveyLogicQuery

        #region GetNonBeneficiarySurveyLogicQueryNew
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <returns></returns>
        public List<ActivitySurveyResponse> GetNonBeneficiarySurveyLogicQueryNew(List<SurveyPrimeLogic> lstSurveyPrimeLogic, List<ActivitySurveyResponse> lstBeneficiarySurveyResponse, ActivityDataCollectionDetails objActivityDataCollectionDetails)
        {
            foreach (var itm in lstSurveyPrimeLogic)
            {
                switch (itm.SurveyLogicPrime)
                {
                    case 3:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() == itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 4:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) > Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 5:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) >= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 6:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) < Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 7:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => Convert.ToDecimal(x.Answer) <= Convert.ToDecimal(itm.SurveyLogicPrimeAnswer) && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                    case 8:
                        lstBeneficiarySurveyResponse = lstBeneficiarySurveyResponse.Where(x => x.Answer.Trim() != itm.SurveyLogicPrimeAnswer && x.QuestionID == objActivityDataCollectionDetails.QuestionId).ToList();
                        break;
                }
            }

            return lstBeneficiarySurveyResponse;
        }
        #endregion GetNonBeneficiarySurveyLogicQueryNew

        #region GetBeneficiarySurveyLogicQuery2
        /// <summary>
        /// GetBeneficiarySurveyLogicQuery2
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetBeneficiarySurveyLogicQuery2(List<SurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y')  = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.BenMultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.CascadingDropdown)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(reg.Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " reg.Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " reg.Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " reg.Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " reg.Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(reg.Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetBeneficiarySurveyLogicQuery2

        #region GetNonBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetNonBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetBeneficiarySurveyResponseLogicQuery(List<SurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
        {
            string query1 = String.Empty;

            if (lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
            {
                int PrimeLogicCount = 0;

                query1 = " and ( ";

                foreach (var itm in lstSurveyPrimeLogic)
                {
                    PrimeLogicCount++;

                    if (answerType == (int)CommonActivity.QuestionType.Date)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 4:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') > DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 5:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') >= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 6:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') < DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 7:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') <= DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                            case 8:
                                query1 += "STR_TO_DATE(Answer, '%m/%d/%Y') != DATE('" + itm.SurveyLogicPrimeAnswer + "')";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.MultiChoice)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else if (answerType == (int)CommonActivity.QuestionType.CascadingDropdown)
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                            case 8:
                                query1 += " TRIM(Answer) not like '%" + itm.SurveyLogicPrimeAnswer + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (itm.SurveyLogicPrime)
                        {
                            case 3:
                                query1 += " TRIM(Answer) = '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                            case 4:
                                query1 += " Answer > " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 5:
                                query1 += " Answer >= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 6:
                                query1 += " Answer < " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 7:
                                query1 += " Answer <= " + itm.SurveyLogicPrimeAnswer;
                                break;
                            case 8:
                                query1 += " TRIM(Answer) != '" + itm.SurveyLogicPrimeAnswer + "'";
                                break;
                        }
                    }

                    if (PrimeLogicCount < lstSurveyPrimeLogic.Count)
                    {
                        query1 += (itm.SurveyLogicCondition == 2 ? " OR " : " AND ");
                    }
                }

                query1 += " )";
            }

            return query1;
        }
        #endregion GetNonBeneficiarySurveyLogicQuery

        #region GetOutPutMonwiseCalculation
        /// <summary>
        /// GetOutPutMonwiseCalculation
        /// </summary>
        /// <param name="lstParentOutputReport"></param>
        /// <param name="lstProjectActivity"></param>GetOutPutMonwiseCalculation
        /// <returns></returns>
        public List<OutputReport> GetOutPutMonwiseCalculation(List<OutputReport> lstChildOutputReport, List<ProjectActivity> lstProjectActivity, List<ProjectActivityMonthwiseData> lstProjectActivityMonthwiseData)
        {
            double TotalCost = 0;
            double Number_of_units = 0;
            string[] MonthData = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            int ActivityId = 0;
            int financialQuestionId = 0;
            bool? IsFinancialAdditionalQuestionExists = false;
            int physicalQuestionId = 0;
            bool? IsPhysicalAdditionalQuestionExists = false;

            if (lstChildOutputReport != null && lstChildOutputReport.Count > 0)
            {
                ActivityId = lstChildOutputReport[0].ActivityId;

                if (lstProjectActivity != null && lstProjectActivity.Count > 0)
                {
                    ProjectActivity objProjectActivity = new ProjectActivity();
                    objProjectActivity = lstProjectActivity.Where(x => x.ActivityId == ActivityId).FirstOrDefault();

                    financialQuestionId = objProjectActivity.FinancialAdditionalQuestionId;
                    IsFinancialAdditionalQuestionExists = objProjectActivity.FinancialAdditionalQuestionType;
                    physicalQuestionId = objProjectActivity.AdditionalQuestionId;
                    IsPhysicalAdditionalQuestionExists = objProjectActivity.AdditionalQuestionType;
                }

                DateTime StartDate = lstChildOutputReport[0].StartDate;
                DateTime EndDate = lstChildOutputReport[0].EndDate;
                int logicType = lstChildOutputReport[0].SurveyLogic;
                int physicalLogicType = lstChildOutputReport[0].PhysicalSurveyLogic;
                TotalCost = lstChildOutputReport[0].TotalCost;
                Number_of_units = lstChildOutputReport[0].NumberOfUnits;

                if (lstChildOutputReport[0].UnitCost <= 0 && lstChildOutputReport[0].TotalCost > 0 && lstChildOutputReport[0].NumberOfUnits > 0)
                {
                    lstChildOutputReport[0].UnitCost = Math.Round(lstChildOutputReport[0].TotalCost / lstChildOutputReport[0].NumberOfUnits, 2);
                }

                var noMonths = 12 * (EndDate.Year - StartDate.Year) + (EndDate.Month - StartDate.Month) + 1;
                lstChildOutputReport[0].NumberOfMoths = noMonths;

                if (noMonths > 0)
                {
                    lstChildOutputReport[0].lstOutputMonthlyReport = new List<OutputMonthlyReport>();
                    int CurrentMonth = StartDate.Month;
                    int CurrentYear = StartDate.Year;

                    double monthUnits = 0;
                    double monthExpenditure = 0;
                    double monthBudget = 0;

                    if (lstChildOutputReport[0].TotalCost > 0)
                    {
                        monthBudget = Math.Round(lstChildOutputReport[0].TotalCost / noMonths, 2);
                    }

                    if (lstChildOutputReport[0].NumberOfUnits > 0)
                    {
                        monthUnits = Math.Round(lstChildOutputReport[0].NumberOfUnits / noMonths, 2);
                    }

                    ProjectActivity objProjectActivity = new ProjectActivity();
                    objProjectActivity = lstProjectActivity.Where(x => x.ActivityId == lstChildOutputReport[0].ActivityId).FirstOrDefault();

                    double totlExpenditure = 0;
                    double totlPhysicalAchieved = 0;

                    int count = 0;
                    ProjectActivityMonthwiseData objProjectActivityMonthwiseData = new ProjectActivityMonthwiseData();

                    for (int nMonth = 0; nMonth < noMonths; nMonth++)
                    {
                        List<OutPutCount> lstOutPutCountBen = new List<OutPutCount>();
                        List<OutPutCount> lstOutPutCountPhysicalBen = new List<OutPutCount>();
                        var monthFirstDate1 = new DateTime(CurrentYear, (CurrentMonth + count), 1);
                        var monthendDate1 = monthFirstDate1.AddMonths(1).AddDays(-1);

                        if (lstProjectActivityMonthwiseData != null && lstProjectActivityMonthwiseData.Count > 0)
                        {
                            objProjectActivityMonthwiseData = lstProjectActivityMonthwiseData.Where(x => x.ActivityId == ActivityId && x.ActYear == CurrentYear && x.ActMonth == (CurrentMonth + count)).FirstOrDefault();
                            if (objProjectActivityMonthwiseData != null &&
                                objProjectActivityMonthwiseData.ActMonth > 0 &&
                                objProjectActivityMonthwiseData.ActYear > 0)
                            {
                                monthBudget = objProjectActivityMonthwiseData.Budget;
                                monthUnits = objProjectActivityMonthwiseData.Target;
                                //Number_of_units += objProjectActivityMonthwiseData.Target;
                                //TotalCost += objProjectActivityMonthwiseData.Budget;
                            }
                        }

                        OutputMonthlyReport objOutputMonthlyReport = new OutputMonthlyReport();
                        objOutputMonthlyReport.Units = monthUnits;

                        List<OutPutCount> lstOutPutCount = new List<OutPutCount>();
                        List<OutPutCount> lstPhysicalOutPutCount = new List<OutPutCount>();

                        if (objProjectActivity != null && objProjectActivity.lstOutPutCount != null && objProjectActivity.lstOutPutCount.Count > 0)
                        {
                            if (nMonth == 0)
                            {
                                //objOutputMonthlyReport.Expenditure = objProjectActivity.lstOutPutCount.Where(x => x.CreatedDate < monthFirstDate1).Count();
                                lstOutPutCount = objProjectActivity.lstOutPutCount.Where(x => x.CreatedDate.Date <= monthendDate1).ToList();
                            }
                            else
                            {
                                //objOutputMonthlyReport.Expenditure = objProjectActivity.lstOutPutCount.Where(x => x.CreatedDate >= monthFirstDate1 && x.CreatedDate <= monthendDate1).Count();
                                lstOutPutCount = objProjectActivity.lstOutPutCount.Where(x => x.CreatedDate.Date >= monthFirstDate1 && x.CreatedDate.Date <= monthendDate1).ToList();
                            }

                            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
                            {
                                double TotSum = 0;
                                double TotCount = 0;

                                if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                {
                                    if (IsFinancialAdditionalQuestionExists != null && IsFinancialAdditionalQuestionExists.Value == true && financialQuestionId > 0)
                                    {
                                        TotSum = lstOutPutCount.Where(x => x.QuestionID == financialQuestionId).Select(p => p.Answer).Distinct().Count();
                                    }
                                    else
                                    {
                                        TotSum = lstOutPutCount.Select(p => p.Answer).Count();
                                    }
                                }
                                else
                                {


                                        if (IsFinancialAdditionalQuestionExists != null && IsFinancialAdditionalQuestionExists.Value == true && financialQuestionId > 0)
                                        {
                                            TotSum = lstOutPutCount.Where(x => x.QuestionID == financialQuestionId).Sum(x => Convert.ToDouble(x.Answer));
                                        }
                                        else
                                        {
                                        double res;
                                        TotSum = lstOutPutCount.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);

                                        //TotSum = lstOutPutCount.Sum(x => Convert.ToDouble(x.Answer));
                                    }


                                }

                                TotCount = lstOutPutCount.Count();

                                lstOutPutCountBen = lstOutPutCount.Where(x => x.ResponseId > 0).GroupBy(p => new { p.ResponseId }).Select(g => g.First()).ToList();

                                if (lstOutPutCountBen != null && lstOutPutCountBen.Count > 0)
                                {
                                    double TotBeneficiaryCount = 0;
                                    double TotBeneficiarySum = 0;
                                    TotSum = 0;
                                    TotCount = 0;

                                    foreach (var itemBen in lstOutPutCountBen)
                                    {
                                        List<OutPutCount> lstOutPutCount1 = new List<OutPutCount>();
                                        lstOutPutCount1 = lstOutPutCount.Where(x => x.ResponseId == itemBen.ResponseId).ToList();

                                        if (lstOutPutCount1 != null && lstOutPutCount1.Count > 0 && IsFinancialAdditionalQuestionExists != null && IsFinancialAdditionalQuestionExists.Value == true && financialQuestionId > 0)
                                        {
                                            lstOutPutCount1 = lstOutPutCount1.Where(x => x.QuestionID == financialQuestionId).ToList();
                                        }

                                        if (logicType == (int)OutPutLogicType.Average)
                                        {

                                            double res;
                                            TotBeneficiarySum = lstOutPutCount1.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);

                                            //TotBeneficiarySum = lstOutPutCount1.Sum((x) => Convert.ToDouble(x.Answer));
                                            TotBeneficiaryCount = lstOutPutCount1.Count();

                                            if (TotBeneficiarySum > 0 && TotBeneficiaryCount > 0)
                                            {
                                                TotSum += TotBeneficiarySum / TotBeneficiaryCount;
                                            }
                                        }
                                        else if (logicType == (int)OutPutLogicType.Cumulative_Sum)
                                        {
                                            // TotSum += lstOutPutCount1.Sum((x) => Convert.ToDouble(x.Answer));
                                            double res;
                                            TotBeneficiarySum = lstOutPutCount1.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);
                                        }
                                        else if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                        {
                                            //TotSum += lstOutPutCount1.Select(p => p.Answer).Distinct().Count();
                                            List<OutPutCount> lstOutPutCount2 = new List<OutPutCount>();
                                            List<string> lstDistinctItem = lstOutPutCount1.Select(p => p.Answer).Distinct().ToList();
                                            if (lstDistinctItem != null && lstDistinctItem.Count > 0)
                                            {
                                                TotSum = lstDistinctItem.Count();
                                            }

                                            //TotSum += lstOutPutCount1.GroupBy(a => a.Answer).Sum((x) => Convert.ToDouble(x));

                                        }

                                        TotCount++;
                                    }
                                }

                                if (logicType == (int)OutPutLogicType.Average)
                                {
                                    objOutputMonthlyReport.Expenditure = Math.Round(TotSum / TotCount, 2);// Math.Round(lstChildOutputReport[0].UnitCost * Math.Round(TotSum / TotCount, 2), 2);
                                }
                                else if (logicType == (int)OutPutLogicType.Cumulative_Sum)
                                {
                                    objOutputMonthlyReport.Expenditure = Math.Round(TotSum, 2);// Math.Round(lstChildOutputReport[0].UnitCost * TotSum, 2);
                                }
                                else if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                {
                                    objOutputMonthlyReport.Expenditure = Math.Round(TotSum, 2);//Math.Round(lstChildOutputReport[0].UnitCost * TotSum, 2);
                                }
                            }
                        }

                        if (objProjectActivity != null && objProjectActivity.lstOutPutCountPhysical != null && objProjectActivity.lstOutPutCountPhysical.Count > 0)
                        {
                            if (nMonth == 0)
                            {
                                lstPhysicalOutPutCount = objProjectActivity.lstOutPutCountPhysical.Where(x => x.CreatedDate.Date <= monthendDate1).ToList();
                            }
                            else
                            {
                                lstPhysicalOutPutCount = objProjectActivity.lstOutPutCountPhysical.Where(x => x.CreatedDate.Date >= monthFirstDate1 && x.CreatedDate.Date <= monthendDate1).ToList();
                            }

                            if (lstPhysicalOutPutCount != null && lstPhysicalOutPutCount.Count > 0)
                            {
                                double TotSum = 0;
                                double TotCount = 0;

                                if (physicalLogicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                {
                                    if (IsPhysicalAdditionalQuestionExists != null && IsPhysicalAdditionalQuestionExists.Value == true && physicalQuestionId > 0)
                                    {
                                        TotSum = lstPhysicalOutPutCount.Where(x => x.QuestionID == physicalQuestionId).Select(p => p.Answer).Distinct().Count();
                                    }
                                    else
                                    {
                                        TotSum = lstPhysicalOutPutCount.Select(p => p.Answer).Count(); //.Distinct()
                                    }
                                }
                                else
                                {
                                    if (IsPhysicalAdditionalQuestionExists != null && IsPhysicalAdditionalQuestionExists.Value == true && physicalQuestionId > 0)
                                    {
                                        //TotSum = lstPhysicalOutPutCount.Where(x => x.QuestionID == physicalQuestionId).Sum(x => Convert.ToDouble(x.Answer));
                                        double res;
                                        TotSum = lstPhysicalOutPutCount.Where(x => x.QuestionID == physicalQuestionId).Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);
                                    }
                                    else
                                    {
                                        //TotSum = lstPhysicalOutPutCount.Sum(x => Convert.ToDouble(x.Answer));
                                        double res;
                                        TotSum = lstPhysicalOutPutCount.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);
                                    }
                                }

                                TotCount = lstPhysicalOutPutCount.Count();

                                lstOutPutCountPhysicalBen = lstPhysicalOutPutCount.Where(x => x.ResponseId > 0).GroupBy(p => new { p.ResponseId }).Select(g => g.First()).ToList();

                                if (lstOutPutCountPhysicalBen != null && lstOutPutCountPhysicalBen.Count > 0)
                                {
                                    double TotBeneficiaryCount = 0;
                                    double TotBeneficiarySum = 0;
                                    TotSum = 0;
                                    TotCount = 0;

                                    foreach (var itemBen in lstOutPutCountPhysicalBen)
                                    {
                                        List<OutPutCount> lstOutPutCount1 = new List<OutPutCount>();
                                        lstOutPutCount1 = lstPhysicalOutPutCount.Where(x => x.ResponseId == itemBen.ResponseId).ToList();

                                        if (lstOutPutCount1 != null && lstOutPutCount1.Count > 0 && IsPhysicalAdditionalQuestionExists != null && IsPhysicalAdditionalQuestionExists.Value == true && physicalQuestionId > 0)
                                        {
                                            lstOutPutCount1 = lstOutPutCount1.Where(x => x.QuestionID == physicalQuestionId).ToList();
                                        }

                                        if (physicalLogicType == (int)OutPutLogicType.Average)
                                        {
                                            double res;
                                            TotBeneficiarySum = lstOutPutCount1.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);
                                            //TotBeneficiarySum = lstOutPutCount1.Sum((x) => Convert.ToDouble(x.Answer));
                                            TotBeneficiaryCount = lstOutPutCount1.Count();

                                            if (TotBeneficiarySum > 0 && TotBeneficiaryCount > 0)
                                            {
                                                TotSum += TotBeneficiarySum / TotBeneficiaryCount;
                                            }
                                        }
                                        else if (physicalLogicType == (int)OutPutLogicType.Cumulative_Sum)
                                        {
                                            //TotSum += lstOutPutCount1.Sum((x) => Convert.ToDouble(x.Answer));

                                            double res;
                                            TotSum = lstOutPutCount1.Sum(x => Double.TryParse(x.Answer, out res) ? res : 0);
                                        }
                                        else if (physicalLogicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                        {
                                            List<OutPutCount> lstOutPutCount2 = new List<OutPutCount>();
                                            List<string> lstDistinctItem = lstOutPutCount1.Select(p => p.Answer).Distinct().ToList();

                                            if (lstDistinctItem != null && lstDistinctItem.Count > 0)
                                            {
                                                TotSum += lstDistinctItem.Count();
                                            }
                                        }

                                        TotCount++;
                                    }
                                }

                                if (physicalLogicType == (int)OutPutLogicType.Average)
                                {
                                    objOutputMonthlyReport.PhysicalAchieved = Math.Round(TotSum / TotCount, 2);// Math.Round(lstChildOutputReport[0].UnitCost * Math.Round(TotSum / TotCount, 2), 2);
                                }
                                else if (physicalLogicType == (int)OutPutLogicType.Cumulative_Sum)
                                {
                                    objOutputMonthlyReport.PhysicalAchieved = Math.Round(TotSum, 2);// Math.Round(lstChildOutputReport[0].UnitCost * TotSum, 2);
                                }
                                else if (physicalLogicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                {
                                    objOutputMonthlyReport.PhysicalAchieved = Math.Round(TotSum, 2);//Math.Round(lstChildOutputReport[0].UnitCost * TotSum, 2);
                                }
                            }
                        }


                        totlExpenditure += objOutputMonthlyReport.Expenditure;
                        totlPhysicalAchieved += objOutputMonthlyReport.PhysicalAchieved;
                        //  objOutputMonthlyReport.Expenditure = 0;
                        objOutputMonthlyReport.Budget = monthBudget;
                        objOutputMonthlyReport.Month_Year = MonthData[(CurrentMonth + count) - 1] + " " + CurrentYear;

                        lstChildOutputReport[0].lstOutputMonthlyReport.Add(objOutputMonthlyReport);

                        if ((CurrentMonth + count) >= 12)
                        {
                            CurrentMonth = 1;
                            count = 0;
                            CurrentYear = CurrentYear + 1;
                        }
                        else
                        {
                            count++;
                        }
                    }

                    lstChildOutputReport[0].TotalPhysicalAchieved = totlPhysicalAchieved;
                    lstChildOutputReport[0].TotalExpenditure = totlExpenditure;
                    lstChildOutputReport[0].TotalCost = TotalCost;
                    lstChildOutputReport[0].NumberOfUnits = Number_of_units;
                }
            }

            return lstChildOutputReport;
        }
        #endregion GetOutPutMonwiseCalculation

        #region GetResponseForOutputReport
        /// <summary>
        /// GetResponseForOutcomeReport
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="logicType"></param>
        /// <param name="value"></param>
        public List<OutPutCount> GetResponseForOutputReport(List<ActivityDataCollectionDetails> objActivityDataCollectionDetails, int ActivityId, int logicType, string SurveyTarget, int FinancialAdditionalSectionId = 0, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false, int TypeofLogic = 2)
        {
            List<OutPutCount> lstOutcomeCount = new List<OutPutCount>();

            // List<ActivityDataCollectionDetails> objActivityDataCollectionDetails = new List<ActivityDataCollectionDetails>();
            List<ActivityDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<ActivityDataCollectionDetails>();

            List<ActivitySurveyResponse> lstOutputSurveyResponse = new List<ActivitySurveyResponse>();
            string query = string.Empty;

            bool IsSurveyQuestion = true;

            if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                IsSurveyQuestion = objProjectSurveyDAL.IsSurveyQuestion(FinancialAdditionalSectionId, additionalSurveyQuestionId);
            }

            double result = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                    {
                        List<ProjectOutputInitialData> lstProjectOutputInitialData = new List<ProjectOutputInitialData>();

                        for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
                        {
                            string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutputSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                            querySurveyPrime += " JOIN TbOutputSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID and ifnull(srvCollection.TypeofLogic,1) = @TypeofLogic; ";
                            List<SurveyPrimeLogic> lstSurveyPrimeLogic = new List<SurveyPrimeLogic>();

                            lstSurveyPrimeLogic = _connection.Query<SurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID, TypeofLogic = TypeofLogic }).ToList();

                            if (objActivityDataCollectionDetails[i].BeneficiaryTypeID <= 0)
                            {
                                List<ProjectOutputInitialData> lstProjectOutputInitialDataInner = new List<ProjectOutputInitialData>();
                                string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutputInitialDataInner = _connection.Query<ProjectOutputInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutputInitialDataInner != null && lstProjectOutputInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutputInitialData == null || lstProjectOutputInitialData.Count == 0))
                                    {
                                        lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutputInitialData> lstProjectOutputInitialDataFilter = new List<ProjectOutputInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutputInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }
                                            }

                                            lstProjectOutputInitialData = new List<ProjectOutputInitialData>();
                                            lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                if (lstProjectOutputInitialData.Where(x => x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutputInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutputInitialDataFilter != null && lstProjectOutputInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].QuestionId > 0)
                            {
                                List<ProjectOutputInitialData> lstProjectOutputInitialDataInner = new List<ProjectOutputInitialData>();

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutputInitialDataInner = _connection.Query<ProjectOutputInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutputInitialDataInner != null && lstProjectOutputInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutputInitialData == null || lstProjectOutputInitialData.Count == 0))
                                    {
                                        lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutputInitialData> lstProjectOutputInitialDataFilter = new List<ProjectOutputInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutputInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //if (lstProjectOutputInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() > 0)
                                                        //{
                                                        //    lstProjectOutputInitialDataFilter.Add(itm1);
                                                        //}
                                                        if (itm2.ResponseID == itm1.ResponseID && itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }
                                            }

                                            lstProjectOutputInitialData = new List<ProjectOutputInitialData>();
                                            lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                if (lstProjectOutputInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutputInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutputInitialDataFilter != null && lstProjectOutputInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].BeneficiaryQuestionID > 0)
                            {
                                List<ProjectOutputInitialData> lstProjectOutputInitialDataInner = new List<ProjectOutputInitialData>();

                                string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                                string query1 = "SELECT DISTINCT BeneficiaryID AS ResponseID,RSP.SurveyID FROM TbBeneficiaryRegistrationDetails reg  JOIN ";
                                query1 += " TbProjectSurveyResponses RSP ON reg.BeneficiaryID = RSP.ResponseID WHERE reg.QuestionID = @QuestionId AND ifnull(reg.IsDeleted,0) = 0 AND  RSP.SurveyID=@SurveyId ";
                                query1 += subqury;

                                lstProjectOutputInitialDataInner = _connection.Query<ProjectOutputInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyId = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                                if (lstProjectOutputInitialDataInner != null && lstProjectOutputInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutputInitialData == null || lstProjectOutputInitialData.Count == 0))
                                    {
                                        lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutputInitialData> lstProjectOutputInitialDataFilter = new List<ProjectOutputInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutputInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (itm2.ResponseID == itm1.ResponseID)
                                                        {
                                                            lstProjectOutputInitialDataFilter.Add(itm1);
                                                        }
                                                        //if (lstProjectOutputInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                        //{
                                                        //    lstProjectOutputInitialDataFilter.Add(itm1);
                                                        //}
                                                    }
                                                }

                                            }

                                            lstProjectOutputInitialData = new List<ProjectOutputInitialData>();
                                            lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutputInitialDataInner)
                                            {
                                                if (lstProjectOutputInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() <= 0)
                                                {
                                                    lstProjectOutputInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutputInitialDataFilter != null && lstProjectOutputInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutputInitialData.AddRange(lstProjectOutputInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        var myBenRes = lstProjectOutputInitialData.Where(x => x.SyncTaskID == 0).Select(l => l.ResponseID).Distinct().ToList();
                        string joinedBeneficiaryResponseIDs = string.Join(",", myBenRes);

                        if (lstProjectOutputInitialData != null && lstProjectOutputInitialData.Count > 0)
                        {
                            var myRes = lstProjectOutputInitialData.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseIDs = string.Join(",", myRes);

                            if (joinedResponseIDs == "0" || joinedResponseIDs == "")
                            {
                                var mySKUs = lstProjectOutputInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);

                                double sum = 0;
                                double count = 0;

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                string joinedQuestionIds = string.Join(",", mySKUs1);

                                if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                                {
                                    string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                                    lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();
                                    List<ActivitySurveyResponse> lstOutputSurveyResponseInner = new List<ActivitySurveyResponse>();

                                    foreach (var itm in lstProjectOutputInitialData)
                                    {
                                        lstOutputSurveyResponseInner = new List<ActivitySurveyResponse>();
                                        lstOutputSurveyResponseInner = lstOutputSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).ToList();

                                        if (lstOutputSurveyResponseInner.Count() > 0)
                                        {
                                            foreach (var itm1 in lstOutputSurveyResponseInner)
                                            {
                                                OutPutCount objOutPutCount = new OutPutCount();

                                                objOutPutCount.ResponseId = itm1.ResponseID;
                                                objOutPutCount.Answer = itm1.Answer;
                                                objOutPutCount.SurveyID = itm1.SurveyID;
                                                objOutPutCount.QuestionId = itm1.QuestionID;
                                                objOutPutCount.QuestionID = itm1.QuestionID;
                                                objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                lstOutcomeCount.Add(objOutPutCount);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                                    lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();

                                    List<ActivitySurveyResponse> lstOutputSurveyResponseInner = new List<ActivitySurveyResponse>();

                                    foreach (var itm in lstProjectOutputInitialData)
                                    {
                                        lstOutputSurveyResponseInner = new List<ActivitySurveyResponse>();

                                        if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                        {
                                            ActivitySurveyResponse objActivitySurveyResponse = new ActivitySurveyResponse();
                                            objActivitySurveyResponse = lstOutputSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();
                                          
                                            if (objActivitySurveyResponse != null)
                                            {
                                                lstOutputSurveyResponseInner.Add(objActivitySurveyResponse);
                                            }
                                        }
                                        else
                                        {
                                            lstOutputSurveyResponseInner = lstOutputSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).ToList();
                                        }

                                        if (lstOutputSurveyResponseInner.Count() > 0)
                                        {
                                            foreach (var itm1 in lstOutputSurveyResponseInner)
                                            {
                                                OutPutCount objOutPutCount = new OutPutCount();

                                                objOutPutCount.ResponseId = itm1.ResponseID;
                                                objOutPutCount.Answer = itm1.Answer;
                                                objOutPutCount.SurveyID = itm1.SurveyID;
                                                objOutPutCount.QuestionId = itm1.QuestionID;
                                                objOutPutCount.QuestionID = itm1.QuestionID;
                                                objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                lstOutcomeCount.Add(objOutPutCount);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (joinedResponseIDs != "0" && (joinedBeneficiaryResponseIDs == "0" || joinedBeneficiaryResponseIDs == ""))
                            {
                                var mySKUs = lstProjectOutputInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);
                                List<ProjectOutputBeneficiaryData> lstProjectOutputBeneficiaryData = new List<ProjectOutputBeneficiaryData>();

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                string joinedQuestionIds = string.Join(",", mySKUs1);

                                if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                                {
                                    if (IsSurveyQuestion)
                                    {
                                        string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                                        lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();
                                    }
                                    else
                                    {
                                        string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                                        lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();
                                    }

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        foreach (var itmres in strSplit)
                                        {
                                            List<ActivitySurveyResponse> lstOutputSurveyResponse1 = new List<ActivitySurveyResponse>();
                                            lstOutputSurveyResponse1 = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                            if (lstOutputSurveyResponse1 != null && lstOutputSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutputSurveyResponse1)
                                                {
                                                    OutPutCount objOutPutCount = new OutPutCount();
                                                    objOutPutCount.ResponseId = itm1.ResponseID;
                                                    objOutPutCount.Answer = itm1.Answer;
                                                    objOutPutCount.SurveyID = itm1.SurveyID;
                                                    objOutPutCount.QuestionId = itm1.QuestionID;
                                                    objOutPutCount.QuestionID = itm1.QuestionID;
                                                    objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                    lstOutcomeCount.Add(objOutPutCount);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE QuestionID  in(" + joinedQuestionIds + ") and  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                                    lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        double totSum = 0;
                                        double average = 0;

                                        foreach (var itmres in strSplit)
                                        {
                                            totSum = 0;
                                            List<ActivitySurveyResponse> lstOutputSurveyResponse1 = new List<ActivitySurveyResponse>();

                                            if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                            {
                                                ActivitySurveyResponse objActivitySurveyResponse = new ActivitySurveyResponse();
                                                objActivitySurveyResponse = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).FirstOrDefault();

                                                if (objActivitySurveyResponse != null)
                                                {
                                                    lstOutputSurveyResponse1.Add(objActivitySurveyResponse);
                                                }
                                            }
                                            else
                                            {
                                                lstOutputSurveyResponse1 = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();
                                            }

                                            if (lstOutputSurveyResponse1 != null && lstOutputSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutputSurveyResponse1)
                                                {
                                                    OutPutCount objOutPutCount = new OutPutCount();
                                                    objOutPutCount.ResponseId = itm1.ResponseID;
                                                    objOutPutCount.Answer = itm1.Answer;
                                                    objOutPutCount.SurveyID = itm1.SurveyID;
                                                    objOutPutCount.QuestionId = itm1.QuestionID;
                                                    objOutPutCount.QuestionID = itm1.QuestionID;
                                                    objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                    lstOutcomeCount.Add(objOutPutCount);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var mySKUs = lstProjectOutputInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);
                                List<ProjectOutputBeneficiaryData> lstProjectOutputBeneficiaryData = new List<ProjectOutputBeneficiaryData>();

                                var mySurveyId = objActivityDataCollectionDetails.Select(l => l.SurveyId).Distinct().ToList();
                                string joinedSurveyIds = string.Join(",", mySurveyId);

                                double sum = 0;
                                double count = 0;

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                                string joinedBeneficiaryQuestionIDs = string.Join(",", mySKUs1);

                                if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                                {
                                    if (IsSurveyQuestion)
                                    {
                                        string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseIDs + ") and SurveyID in(" + joinedSurveyIds + ");";
                                        lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId }).ToList();
                                    }
                                    else
                                    {
                                        string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                                        lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();
                                    }

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        foreach (var itmres in strSplit)
                                        {
                                            List<ActivitySurveyResponse> lstOutputSurveyResponse1 = new List<ActivitySurveyResponse>();
                                            lstOutputSurveyResponse1 = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                            if (lstOutputSurveyResponse1 != null && lstOutputSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutputSurveyResponse1)
                                                {
                                                    OutPutCount objOutPutCount = new OutPutCount();

                                                    objOutPutCount.ResponseId = itm1.ResponseID;
                                                    objOutPutCount.Answer = itm1.Answer;
                                                    objOutPutCount.SurveyID = itm1.SurveyID;
                                                    objOutPutCount.QuestionId = itm1.QuestionID;
                                                    objOutPutCount.QuestionID = itm1.QuestionID;
                                                    objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                    lstOutcomeCount.Add(objOutPutCount);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string QSTQry4 = "SELECT reg.BeneficiaryID AS ResponseID,reg.QuestionID,reg.Answer,reg.CreatedDate FROM TbBeneficiaryRegistrationDetails reg  WHERE reg.QuestionID  in(" + joinedBeneficiaryQuestionIDs + ");";
                                    lstOutputSurveyResponse = _connection.Query<ActivitySurveyResponse>(QSTQry4).ToList();

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        double totSum = 0;
                                        //double average = 0;

                                        foreach (var itmres in strSplit)
                                        {
                                            totSum = 0;
                                            List<ActivitySurveyResponse> lstOutputSurveyResponse1 = new List<ActivitySurveyResponse>();

                                            if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                            {
                                                ActivitySurveyResponse objActivitySurveyResponse = new ActivitySurveyResponse();
                                                objActivitySurveyResponse = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).FirstOrDefault();

                                                if (objActivitySurveyResponse != null)
                                                {
                                                    lstOutputSurveyResponse1.Add(objActivitySurveyResponse);
                                                }
                                            }
                                            else
                                            {
                                                lstOutputSurveyResponse1 = lstOutputSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();
                                            }

                                            if (lstOutputSurveyResponse1 != null && lstOutputSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutputSurveyResponse1)
                                                {
                                                    OutPutCount objOutPutCount = new OutPutCount();

                                                    objOutPutCount.ResponseId = itm1.ResponseID;
                                                    objOutPutCount.Answer = itm1.Answer;
                                                    objOutPutCount.SurveyID = itm1.SurveyID;
                                                    objOutPutCount.QuestionId = itm1.QuestionID;
                                                    objOutPutCount.QuestionID = itm1.QuestionID;
                                                    objOutPutCount.CreatedDate = itm1.CreatedDate;
                                                    lstOutcomeCount.Add(objOutPutCount);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstOutcomeCount;
        }
        #endregion GetResponseForOutputReport
    }
}
