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
    public class OutcomeReportDAL
    {
        protected IDbConnection _connection;

        #region GetOutputReport
        /// <summary>
        /// GetOutputReport
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<OutcomeReport> GetOutComeReport(int ProjectID, int PageNumber = 0)
        {
            List<OutcomeReport> lstOutcomeReport = new List<OutcomeReport>();
            List<ProjectOutcomeMonthwiseData> lstProjectOutcomeMonthwiseData = new List<ProjectOutcomeMonthwiseData>();

            PageNumber = PageNumber * 10;

            try
            {
                string[] MonthData = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT oct.pk_OutcomeID AS OutcomeID,oct.OutcomeName,oct.StartDate,oct.EndDate,IFNULL(oct.SurveyTarget,0) SurveyTarget,oct.SurveyLogic,
                                (select count(*) from TbOutcomes where ProjectId = @ProjectID  and IFNULL(IsDeleted,0) = 0) as TotalRecords,IFNULL(SDG.SDG_Goal,'') as SDG_Goal,ifnull(SDG_Image,'') SDG_Goal_Image FROM TbOutcomes oct 
                                LEFT JOIN TbSDGMaster SDG ON oct.SDG_ID = SDG.pk_SDGID WHERE oct.ProjectId = @ProjectID and IFNULL(oct.IsDeleted,0) = 0 order by  OutcomeID DESC;"; // LIMIT @PageNumber,10  and  pk_OutcomeID in (1039,1040,1041)
                    lstOutcomeReport = _connection.Query<OutcomeReport>(strQuery, new { ProjectID = ProjectID }).ToList(); //, PageNumber = PageNumber

                    string strMonthwiseData = "SELECT ActMonth.ID,ActMonth.OutcomeId,ActMonth.Target,ActMonth.Budget,ActMonth.ActMonth,ActMonth.ActYear FROM TbOutcomeMonthwiseData ActMonth JOIN TbOutcomes prjOut ";
                    strMonthwiseData += "ON ActMonth.OutcomeId = prjOut.pk_OutcomeID WHERE prjOut.ProjectId = @ProjectId AND IFNULL(ActMonth.IsActive,0) =1 AND IFNULL(prjOut.IsDeleted,0)=0;";

                    lstProjectOutcomeMonthwiseData = _connection.Query<ProjectOutcomeMonthwiseData>(strMonthwiseData, new { ProjectId = ProjectID }).ToList();
                }

                List<ProjectOutcome> lstOutcome = new List<ProjectOutcome>();

                if (lstOutcomeReport != null && lstOutcomeReport.Count > 0)
                {
                    var SelOutcomeID = lstOutcomeReport.Select(l => l.OutcomeID).Distinct().ToList();
                    string joinedSelOutcomeIDs = string.Join(",", SelOutcomeID);

                    lstOutcome = getDataForOutcomeReport(ProjectID, joinedSelOutcomeIDs);

                    foreach (var item in lstOutcomeReport)
                    {
                        DateTime StartDate = item.StartDate;
                        DateTime EndDate = item.EndDate;
                        int logicType = item.SurveyLogic;

                        if (logicType == 0)   ///  some time logic is 0 then getting error, so if logic is 0 then set at  1 - count of unique response
                        {
                            logicType = 1;
                        }

                        int OutcomeId = item.OutcomeID;

                        DateTime now = DateTime.Now;
                        var monthFirstDate = new DateTime(now.Year, now.Month, 1);

                        ProjectOutcome lstOutcomeFilter = new ProjectOutcome();

                        lstOutcomeFilter = lstOutcome.Where(x => x.pk_OutcomeID == item.OutcomeID).FirstOrDefault();

                        if (lstOutcomeFilter != null)
                        {
                            int lastmonth = DateTime.Today.AddMonths(-1).Month;
                            int lastyear = DateTime.Today.Year;

                            if (lastmonth >= 12)
                            {
                                lastyear = DateTime.Today.AddYears(-1).Year;
                            }

                            DateTime date = new DateTime(lastyear, lastmonth, 1);

                            item.AchievedTillText = date.ToString("MMM") + " " + lastyear;

                            if (lstOutcomeFilter.lstOutcomeCount != null && lstOutcomeFilter.lstOutcomeCount.Count > 0)
                            {
                                item.AchievedTillValue = lstOutcomeFilter.lstOutcomeCount.Where(x => x.CreatedDate < monthFirstDate).Count();
                            }
                        }

                        var noMonths = 12 * (EndDate.Year - StartDate.Year) + (EndDate.Month - StartDate.Month) + 1;
                        item.NumberOfMoths = noMonths;

                        if (noMonths > 0)
                        {
                            int CurrentMonth = StartDate.Month;
                            int CurrentYear = StartDate.Year;

                            item.lstOutcomeMonthlyReport = new List<OutcomeMonthlyReport>();

                            int count = 0;
                            double plannedCount = 0;
                            plannedCount = Math.Round(Convert.ToDouble(item.SurveyTarget) / noMonths, 2);

                            ProjectOutcomeMonthwiseData objProjectOutcomeMonthwiseData = new ProjectOutcomeMonthwiseData();

                            for (int nMonth = 0; nMonth < noMonths; nMonth++)
                            {
                                var monthFirstDate1 = new DateTime(CurrentYear, (CurrentMonth + count), 1);
                                var monthendDate1 = monthFirstDate1.AddMonths(1).AddDays(-1);
                                List<OutcomeCount> lstOutcomeCountBen = new List<OutcomeCount>();

                                OutcomeMonthlyReport objOutcomeMonthlyReport = new OutcomeMonthlyReport();
                                //objOutcomeMonthlyReport.Planned = plannedCount;

                                if (lstProjectOutcomeMonthwiseData != null && lstProjectOutcomeMonthwiseData.Count > 0)
                                {
                                    objProjectOutcomeMonthwiseData = lstProjectOutcomeMonthwiseData.Where(x => x.OutcomeId == OutcomeId && x.ActYear == CurrentYear && x.ActMonth == (CurrentMonth + count)).FirstOrDefault();
                                    if (objProjectOutcomeMonthwiseData != null &&
                                        objProjectOutcomeMonthwiseData.ActMonth > 0 &&
                                        objProjectOutcomeMonthwiseData.ActYear > 0)
                                    {
                                        objOutcomeMonthlyReport.Planned = objProjectOutcomeMonthwiseData.Target;
                                    }
                                    else
                                    {
                                        objOutcomeMonthlyReport.Planned = plannedCount;
                                    }
                                }
                                else
                                {
                                    objOutcomeMonthlyReport.Planned = plannedCount;
                                }

                                List<OutcomeCount> lstOutcomeCount = new List<OutcomeCount>();

                                if (lstOutcomeFilter != null && lstOutcomeFilter.lstOutcomeCount != null && lstOutcomeFilter.lstOutcomeCount.Count > 0)
                                {
                                    if (nMonth == 0)
                                    {
                                        lstOutcomeCount = lstOutcomeFilter.lstOutcomeCount.Where(x => x.CreatedDate.Date <= monthendDate1).ToList();
                                    }
                                    else
                                    {
                                        lstOutcomeCount = lstOutcomeFilter.lstOutcomeCount.Where(x => x.CreatedDate.Date >= monthFirstDate1 && x.CreatedDate.Date <= monthendDate1).ToList();
                                    }


                                    if (lstOutcomeCount != null && lstOutcomeCount.Count > 0)
                                    {
                                        double TotSum = 0;
                                        double TotCount = 0;

                                        if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                        {
                                            //TotSum = lstOutcomeCount.Select(p => p.Answer).Distinct().Count();
                                            TotSum = lstOutcomeCount.Select(p => p.Answer).Count();
                                        }
                                        else
                                        {
                                            TotSum = lstOutcomeCount.Sum(x => Convert.ToDouble(x.Answer));
                                        }

                                        TotCount = lstOutcomeCount.Count();

                                        lstOutcomeCountBen = lstOutcomeCount.Where(x => x.ResponseId > 0).GroupBy(p => new { p.ResponseId }).Select(g => g.First()).ToList();

                                        if (lstOutcomeCountBen != null && lstOutcomeCountBen.Count > 0)
                                        {
                                            double TotBeneficiaryCount = 0;
                                            double TotBeneficiarySum = 0;
                                            TotSum = 0;
                                            TotCount = 0;

                                            foreach (var itemBen in lstOutcomeCountBen)
                                            {
                                                if (logicType == (int)OutPutLogicType.Average)
                                                {
                                                    TotBeneficiarySum = lstOutcomeCount.Where(x => x.ResponseId == itemBen.ResponseId).Sum((x) => Convert.ToDouble(x.Answer));
                                                    TotBeneficiaryCount = lstOutcomeCount.Where(x => x.ResponseId == itemBen.ResponseId).Count();

                                                    if (TotBeneficiarySum > 0 && TotBeneficiaryCount > 0)
                                                    {
                                                        TotSum += TotBeneficiarySum / TotBeneficiaryCount;
                                                    }
                                                }
                                                else if (logicType == (int)OutPutLogicType.Cumulative_Sum)
                                                {
                                                    TotSum += lstOutcomeCount.Where(x => x.ResponseId == itemBen.ResponseId).Sum((x) => Convert.ToDouble(x.Answer));
                                                }
                                                else if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                                {
                                                    TotSum += lstOutcomeCount.Where(x => x.ResponseId == itemBen.ResponseId).Select(p => p.Answer).Distinct().Count();
                                                }

                                                TotCount++;
                                            }
                                        }

                                        if (logicType == (int)OutPutLogicType.Average)
                                        {
                                            objOutcomeMonthlyReport.Achieved = Math.Round(TotSum / TotCount, 2);
                                        }
                                        else if (logicType == (int)OutPutLogicType.Cumulative_Sum)
                                        {
                                            objOutcomeMonthlyReport.Achieved = TotSum;
                                        }
                                        else if (logicType == (int)OutPutLogicType.Count_of_Unique_Reponses)
                                        {
                                            objOutcomeMonthlyReport.Achieved = TotSum;
                                        }
                                    }
                                }


                                item.TotalAchieved += objOutcomeMonthlyReport.Achieved;

                                objOutcomeMonthlyReport.Month_Year = MonthData[(CurrentMonth + count) - 1] + " " + CurrentYear;

                                item.lstOutcomeMonthlyReport.Add(objOutcomeMonthlyReport);

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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstOutcomeReport;
        }
        #endregion GetOutComeReport

        #region getData
        /// <summary>
        /// getData
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ProjectOutcome> getData(int ProjectId = 0, string joinedOutcomeIds = "")
        {
            List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();
            List<OutcomeDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();

            List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();

            string query = string.Empty;

            double result = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.pk_OutcomeID ActivityId,TbOutcomes.ProjectID, TbOutcomes.OutcomeName ActivityName,TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                            TbOutcomes.DatacollectionType ActivityType, TbOutcomes.DatacollectionType ,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                                 TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                            TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,ifnull(TbOutcomes.UpdatedDate,TbOutcomes.CreatedDate) UpdatedDate,TbPeriodicity.PeriodicityType as ReportingFrequencyName,TbOutcomes.Description,
                            ifnull(TbOutcomes.AdditionalSectionId,0) as AdditionalSectionId,ifnull(TbOutcomes.AdditionalQuestionId,0) as AdditionalQuestionId,ifnull(TbOutcomes.AdditionalQuestionType,false) as AdditionalQuestionType  
                            from TbOutcomes TbOutcomes 
                            left join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                            left join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                            left join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                            where TbOutcomes.IsDeleted = 0 and TbOutcomes.ProjectId = @ProjectId and FIND_IN_SET(TbOutcomes.pk_OutcomeID, @joinedOutcomeIds) 
                            order by TbOutcomes.pk_OutcomeID desc";
                    ObjOutcome = _connection.Query<ProjectOutcome>(strQuery, new { ProjectId = ProjectId, joinedOutcomeIds = joinedOutcomeIds }).ToList();

                    int OutcomeID = 0;
                    int logicType = 0;
                    string SurveyTarget = "";

                    foreach (ProjectOutcome Item in ObjOutcome)
                    {
                        if (Item.DatacollectionType == "Quantitative")
                        {
                            OutcomeID = Item.pk_OutcomeID;
                            logicType = Item.SurveyLogic;
                            SurveyTarget = Item.SurveyTarget;

                            query = "SELECT ACT.ID,ACT.OutcomeId ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                            query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType FROM TbProjectSurvey SRV ";
                            query += "LEFT JOIN TbOutcomeSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                            query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                            query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                            query += "where ACT.OutcomeId = @OutcomeId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1; ";

                            objActivityDataCollectionDetails = _connection.Query<OutcomeDataCollectionDetails>(query, new { OutcomeId = OutcomeID }).ToList();

                            if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                            {
                                var mySKUs = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                string joinedQuestionIds = string.Join(",", mySKUs);

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                                string joinedBeneficiaryTypeID = string.Join(",", mySKUs1);

                                var benficiaryIds = objActivityDataCollectionDetails.Where(x => x.BeneficiaryQuestionID > 0).Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                                string joinedBeneficiaryQuestionID = string.Join(",", benficiaryIds);

                                List<OutcomeSurveyResponse> lstActivitySurveyResponse = new List<OutcomeSurveyResponse>();
                                List<OutcomeSurveyResponse> lstActivitySurveyResponse1 = new List<OutcomeSurveyResponse>();
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse = new List<OutcomeSurveyResponse>();

                                if (joinedQuestionIds != null && joinedQuestionIds.Length > 0)
                                {
                                    string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where QuestionID in(" + joinedQuestionIds + ") and ifnull(IsActive,0)=1;";
                                    lstActivitySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry).ToList();

                                    lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                }

                                if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0)
                                {
                                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID,reg.CreatedDate ";
                                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID in(" + joinedBeneficiaryQuestionID + ") and ifnull(reg.IsDeleted,0)=0;";
                                    lstBeneficiarySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTBenQry).ToList();

                                    lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                }

                                Item.lstOutcomeCount = new List<OutcomeCount>();
                                Item.lstOutcomeCount.AddRange(GetQuantitativeAverage(objActivityDataCollectionDetails, lstActivitySurveyResponse1, lstActivitySurveyResponse, joinedBeneficiaryQuestionID, SurveyTarget, lstBeneficiarySurveyResponse, Item.AdditionalQuestionId, Item.AdditionalQuestionType));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjOutcome;
        }
        #endregion getData

        #region GetQuantitativeAverage
        /// <summary>
        /// GetQuantitativeAverage
        /// </summary>
        /// <param name="objActivityDataCollectionDetails"></param>
        /// <param name="lstActivitySurveyResponse1"></param>
        /// <param name="lstActivitySurveyResponse"></param>
        /// <param name="joinedBeneficiaryQuestionID"></param>
        /// <param name="SurveyTarget"></param>
        /// <param name="lstBeneficiarySurveyResponse"></param>
        /// <returns></returns>
        public List<OutcomeCount> GetQuantitativeAverage(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            double progressPercentage = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();

            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();
            List<OutPutQuestionRSP> lstOutPutQuestionRSP2 = new List<OutPutQuestionRSP>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = null;
            lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();
            List<OutcomeSurveyResponse> lstActivitySurveyResponse5 = new List<OutcomeSurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();

                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID,reg.CreatedDate ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                    {
                        if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                        {
                            var mySKUs3 = lstBeneficiarySurveyResponse1.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseID = string.Join(",", mySKUs3);

                            if (!string.IsNullOrEmpty(joinedResponseID))
                            {
                                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse2 = new List<OutcomeSurveyResponse>();

                                string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,CreatedDate from TbProjectSurveyResponses  WHERE ResponseID in(" + joinedResponseID + ")  and QuestionID =@QuestionID and  SurveyID=@SurveyID;";

                                lstBeneficiarySurveyResponse2 = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId, SurveyID = lstBeneficiarySurveyResponse1[lstBeneficiarySurveyResponse1.Count - 1].SurveyID }).ToList();

                                if (lstBeneficiarySurveyResponse2 != null && lstBeneficiarySurveyResponse2.Count > 0)
                                {
                                    lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse2);
                                }
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
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();

                                if (additionalSurveyQuestionId > 0 && lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    List<OutcomeSurveyResponse> lstActivitySurveyResponse4 = new List<OutcomeSurveyResponse>();
                                    var mySKUs = lstActivitySurveyResponse3.Select(l => l.SyncTaskID).Distinct().ToList();

                                    string joinedSyncTaskID = string.Join(",", mySKUs);

                                    if (additionalQuestionType.Value)
                                    {
                                        QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,ifnull(SubmissionCount,1) SubmissionCount,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                        QSTQry += "ResponseID=@ResponseID  and SyncTaskID in(" + joinedSyncTaskID + ")  and QuestionID =@QuestionID";
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

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
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
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
                                        lstActivitySurveyResponse4 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { ResponseID = lstActivitySurveyResponse1[cnt].ResponseID, QuestionID = additionalSurveyQuestionId }).ToList();

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
                                                    OutcomeSurveyResponse objActivitySurveyResponse = new OutcomeSurveyResponse();
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
                                            OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                                            objBeneficiaryResponse1.ResponseID = item.ResponseID;
                                            objBeneficiaryResponse1.SurveyID = item.SurveyID;
                                            objBeneficiaryResponse1.AnwerString = item.Answer;  //lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer)) / lstBeneficiarySurveyResponse22.Count;
                                            objBeneficiaryResponse1.CreatedDate = item.CreatedDate;
                                            objBeneficiaryResponse1.QuestionId = item.QuestionID;
                                            lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                                {
                                    foreach (var itm in lstActivitySurveyResponse3)
                                    {
                                        OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                        objBeneficiaryResponse.ResponseID = itm.ResponseID;
                                        objBeneficiaryResponse.AnwerString = itm.Answer;
                                        objBeneficiaryResponse.SurveyID = itm.SurveyID;
                                        objBeneficiaryResponse.QuestionId = itm.QuestionID;
                                        objBeneficiaryResponse.CreatedDate = itm.CreatedDate;
                                        lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    List<OutcomeCount> lstOutcomeCount2 = new List<OutcomeCount>();

                    string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                    if (additionalSurveyQuestionId <= 0)
                    {
                        query1 = "SELECT Answer,0 as ResponseCount,SurveyID,CreatedDate,resp.QuestionID FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                        query1 += subqury;
                        lstOutcomeCount2 = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();
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

                    if (lstOutcomeCount2 != null && lstOutcomeCount2.Count > 0)
                    {
                        lstOutPutCount.AddRange(lstOutcomeCount2);
                    }

                    if (lstOutPutQuestionRSP2 != null && lstOutPutQuestionRSP2.Count > 0)
                    {
                        foreach (var itm1 in lstOutPutQuestionRSP2)
                        {
                            OutcomeCount objOutPutCount1 = new OutcomeCount();
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
                foreach (var itm in lstBeneficiarySurveyResponseAll)
                {
                    OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = itm.ResponseID;
                    objBeneficiaryResponse1.SurveyID = itm.SurveyID;
                    objBeneficiaryResponse1.QuestionId = itm.QuestionID;
                    objBeneficiaryResponse1.AnwerString = itm.Answer;
                    objBeneficiaryResponse1.CreatedDate = itm.CreatedDate;
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                foreach (var item in lstBeneficiaryResponse)
                {
                    OutcomeCount objOutPutCount = new OutcomeCount();
                    objOutPutCount.ResponseId = item.ResponseID;
                    objOutPutCount.Answer = item.AnwerString;
                    objOutPutCount.SurveyID = item.SurveyID;
                    objOutPutCount.QuestionId = item.QuestionId;
                    objOutPutCount.QuestionID = item.QuestionId;
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
        public double GetQuantitativeCumulativeSum(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();
            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic -  starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition  from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                // lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                QSTQry += subqury;
                                lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                double sumValue = 0;
                                int cnt1 = 0;

                                for (cnt1 = 0; cnt1 < lstActivitySurveyResponse3.Count; cnt1++)
                                {
                                    sumValue += (lstActivitySurveyResponse3[cnt1].Answer != null && lstActivitySurveyResponse3[cnt1].Answer != "") ? Convert.ToDouble(lstActivitySurveyResponse3[cnt1].Answer) : 0;
                                }

                                if (sumValue > 0)
                                {
                                    OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                    objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                    objBeneficiaryResponse.Anwer = sumValue;
                                    objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                    lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                                }
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    string subQury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    query1 = "SELECT SUM(Answer) AnswerSum,(SELECT COUNT(QuestionID) FROM TbProjectSurveyResponses WHERE QuestionID = resp.QuestionID " + subQury + ") ResponseCount,SurveyID,CreatedDate FROM TbProjectSurveyResponses resp WHERE  QuestionID = @QuestionId AND ifnull(IsActive,0) = 1  ";
                    query1 += subQury;
                    query1 += " GROUP BY SurveyID;";

                    objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();
                    if (objOutPutCount != null)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBenActivitySurveyResponse2 = lstBeneficiarySurveyResponseAll.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                for (int cnt10 = 0; cnt10 < lstBenActivitySurveyResponse2.Count; cnt10++)
                {
                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                    lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.Where(x => x.ResponseID == lstBenActivitySurveyResponse2[cnt10].ResponseID).ToList();
                    if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                    {
                        OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                        objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                        objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                        objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Sum(x => Convert.ToDouble(x.Answer));
                        lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                    }
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToDouble(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            {
                var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
                string joinedSurveyId = string.Join(",", mySKUs3);
                double totsum = 0;
                if (joinedSurveyId != null && joinedSurveyId.Length > 0)
                {
                    string[] spltsurveyId = joinedSurveyId.Split(',');
                    for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
                    {
                        OutcomeCount obj1OutPutCount = new OutcomeCount();
                        totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
                    }
                }

                result = (totsum * 100) / Convert.ToDouble(SurveyTarget);
            }

            return result;
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
        public List<OutcomeCount> GetQuantitativeUniqueResponses(List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails, List<OutcomeSurveyResponse> lstActivitySurveyResponse1, List<OutcomeSurveyResponse> lstActivitySurveyResponse, string joinedBeneficiaryQuestionID, string SurveyTarget, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse)
        {
            string query = string.Empty;
            string query1 = string.Empty;
            double result = 0;
            List<OutcomeCount> lstOutPutCount = new List<OutcomeCount>();

            List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse = new List<OutcomeBeneficiaryResponse>();

            List<OutcomeSurveyResponse> lstBeneficiarySurveyResponseAll = new List<OutcomeSurveyResponse>();

            for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
            {
                OutcomeCount objOutPutCount = new OutcomeCount();

                //to get survey prime logic - starts
                string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID; ";
                List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();
                lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();
                //to get survey prime logic - ends

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstBeneficiarySurveyResponse != null && lstBeneficiarySurveyResponse.Count > 0)// && lstSurveyPrimeLogic != null && lstSurveyPrimeLogic.Count > 0)
                {
                    //  lstBeneficiarySurveyResponse = GetBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, lstBeneficiarySurveyResponse, objActivityDataCollectionDetails[i]);

                    string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID ";
                    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID = @BeneficiaryQuestionID and ifnull(reg.IsDeleted,0)=0  ";
                    QSTBenQry += subqury;

                    List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse1 = new List<OutcomeSurveyResponse>();

                    lstBeneficiarySurveyResponse1 = _connection.Query<OutcomeSurveyResponse>(QSTBenQry, new { BeneficiaryQuestionID = objActivityDataCollectionDetails[i].BeneficiaryQuestionID }).ToList();

                    if (lstBeneficiarySurveyResponse1 != null && lstBeneficiarySurveyResponse1.Count > 0)
                    {
                        lstBeneficiarySurveyResponseAll.AddRange(lstBeneficiarySurveyResponse1);
                    }
                }

                if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && lstActivitySurveyResponse1.Count > 0)
                {
                    if (objActivityDataCollectionDetails[i].QuestionId > 0)
                    {
                        List<OutcomeSurveyResponse> lstActivitySurveyResponse6 = new List<OutcomeSurveyResponse>();

                        string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                        string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                        QSTQry += "QuestionID = @QuestionID ";
                        QSTQry += subqury;
                        lstActivitySurveyResponse6 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId }).ToList();


                        for (int cnt = 0; cnt < lstActivitySurveyResponse1.Count; cnt++)
                        {
                            List<OutcomeSurveyResponse> lstActivitySurveyResponse3 = new List<OutcomeSurveyResponse>();
                            lstActivitySurveyResponse3 = lstActivitySurveyResponse.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID && x.QuestionID == objActivityDataCollectionDetails[i].QuestionId).ToList();

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                //  lstActivitySurveyResponse3 = GetNonBeneficiarySurveyLogicQueryNew(lstSurveyPrimeLogic, lstActivitySurveyResponse3, objActivityDataCollectionDetails[i]);

                                //    string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                //    string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID,CreatedDate FROM TbProjectSurveyResponses where ifnull(IsActive,0)=1 and ";
                                //    QSTQry += "QuestionID = @QuestionID and ResponseID=@ResponseID  ";
                                //    QSTQry += subqury;
                                //    lstActivitySurveyResponse3 = _connection.Query<OutcomeSurveyResponse>(QSTQry, new { QuestionID = objActivityDataCollectionDetails[i].QuestionId, ResponseID = lstActivitySurveyResponse1[cnt].ResponseID }).ToList();


                                lstActivitySurveyResponse3 = lstActivitySurveyResponse6.Where(x => x.ResponseID == lstActivitySurveyResponse1[cnt].ResponseID).ToList();
                            }

                            if (lstActivitySurveyResponse3 != null && lstActivitySurveyResponse3.Count > 0)
                            {
                                lstActivitySurveyResponse3 = lstActivitySurveyResponse3.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                                OutcomeBeneficiaryResponse objBeneficiaryResponse = new OutcomeBeneficiaryResponse();
                                objBeneficiaryResponse.ResponseID = lstActivitySurveyResponse3[0].ResponseID;
                                objBeneficiaryResponse.Anwer = lstActivitySurveyResponse3.Count;
                                objBeneficiaryResponse.CreatedDate = lstActivitySurveyResponse3[0].CreatedDate;
                                objBeneficiaryResponse.SurveyID = lstActivitySurveyResponse3[0].SurveyID;
                                lstBeneficiaryResponse.Add(objBeneficiaryResponse);
                            }
                        }
                    }
                }
                else if (objActivityDataCollectionDetails[i].QuestionId > 0)
                {
                    query1 = " SELECT COUNT(DISTINCT Answer) AnswerSum,0 AS ResponseCount,max(SurveyID) SurveyID,CreatedDate";
                    query1 += " FROM TbProjectSurveyResponses WHERE QuestionID =  @QuestionId AND ifnull(IsActive,0) = 1  ";
                    query1 += GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);
                    query1 += " GROUP BY QuestionID;";

                    objOutPutCount = _connection.Query<OutcomeCount>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).FirstOrDefault();

                    if (objOutPutCount != null)
                    {
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0 && lstBeneficiarySurveyResponseAll != null && lstBeneficiarySurveyResponseAll.Count > 0)
            {
                List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse22 = new List<OutcomeSurveyResponse>();
                lstBeneficiarySurveyResponse22 = lstBeneficiarySurveyResponseAll.GroupBy(x => x.Answer).Select(x => x.First()).ToList();

                if (lstBeneficiarySurveyResponse22 != null && lstBeneficiarySurveyResponse22.Count > 0)
                {
                    OutcomeBeneficiaryResponse objBeneficiaryResponse1 = new OutcomeBeneficiaryResponse();
                    objBeneficiaryResponse1.ResponseID = lstBeneficiarySurveyResponse22[0].ResponseID;
                    objBeneficiaryResponse1.SurveyID = lstBeneficiarySurveyResponse22[0].SurveyID;
                    objBeneficiaryResponse1.Anwer = lstBeneficiarySurveyResponse22.Count();
                    lstBeneficiaryResponse.Add(objBeneficiaryResponse1);
                }
            }

            if (lstBeneficiaryResponse != null && lstBeneficiaryResponse.Count > 0)
            {
                List<OutcomeBeneficiaryResponse> lstBeneficiaryResponse3 = lstBeneficiaryResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();

                if (lstBeneficiaryResponse3 != null && lstBeneficiaryResponse3.Count > 0)
                {
                    for (int g = 0; g < lstBeneficiaryResponse3.Count; g++)
                    {
                        OutcomeCount objOutPutCount = new OutcomeCount();
                        objOutPutCount.ResponseCount = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID).Count();
                        objOutPutCount.AnswerSum = lstBeneficiaryResponse.Where(x => x.ResponseID == lstBeneficiaryResponse3[g].ResponseID && x.Anwer != null).Sum(x => Convert.ToInt32(x.Anwer));
                        objOutPutCount.SurveyID = lstBeneficiaryResponse[g].SurveyID;
                        objOutPutCount.CreatedDate = lstBeneficiaryResponse[g].CreatedDate;
                        lstOutPutCount.Add(objOutPutCount);
                    }
                }
            }

            //if (lstOutPutCount != null && lstOutPutCount.Count > 0)
            //{
            //    var mySKUs3 = lstOutPutCount.Select(l => l.SurveyID).Distinct().ToList();
            //    string joinedSurveyId = string.Join(",", mySKUs3);
            //    double totsum = 0;
            //    if (joinedSurveyId != null && joinedSurveyId.Length > 0)
            //    {
            //        string[] spltsurveyId = joinedSurveyId.Split(',');
            //        for (int cnt6 = 0; cnt6 < spltsurveyId.Length; cnt6++)
            //        {
            //            OutcomeCount obj1OutPutCount = new OutcomeCount();
            //            totsum += lstOutPutCount.Where(x => x.SurveyID == Convert.ToDouble(spltsurveyId[cnt6])).Sum(x => x.AnswerSum);
            //        }
            //    }

            //    result = (totsum * 100) / Convert.ToDouble(SurveyTarget);
            //}

            return lstOutPutCount;
        }
        #endregion GetQuantitativeUniqueResponses

        #region GetNonBeneficiarySurveyLogicQuery
        /// <summary>
        /// GetNonBeneficiarySurveyLogicQuery
        /// </summary>
        /// <param name="lstSurveyPrimeLogic"></param>
        /// <returns></returns>
        public string GetNonBeneficiarySurveyLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
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
        public List<OutcomeSurveyResponse> GetBeneficiarySurveyLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, OutcomeDataCollectionDetails objActivityDataCollectionDetails)
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
        public List<OutcomeSurveyResponse> GetNonBeneficiarySurveyLogicQueryNew(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse, OutcomeDataCollectionDetails objActivityDataCollectionDetails)
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
        public string GetBeneficiarySurveyLogicQuery2(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
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
                                query1 += "STR_TO_DATE(reg.Answer, '%m/%d/%Y') = DATE('" + itm.SurveyLogicPrimeAnswer + "')";
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
        public string GetBeneficiarySurveyResponseLogicQuery(List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic, int answerType)
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

        #region getDataForOutcomeReport
        /// <summary>
        /// getDataForOutcomeReport
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ProjectOutcome> getDataForOutcomeReport(int ProjectId = 0, string joinedOutcomeIds = "")
        {
            List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();
            List<OutcomeDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();

            List<ProjectOutcome> ObjOutcome = new List<ProjectOutcome>();

            string query = string.Empty;

            double result = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select TbOutcomes.pk_OutcomeID ,TbOutcomes.pk_OutcomeID ActivityId,TbOutcomes.ProjectID, TbOutcomes.OutcomeName ActivityName,TbOutcomes.OutcomeName, TbOutcomes.StartDate,TbOutcomes.EndDate ,
                            TbOutcomes.DatacollectionType ActivityType, TbOutcomes.DatacollectionType ,TbOutcomes.SurveyId,TbProjectSurvey.SurveyName ,TbOutcomes.SurveySectionId,TbOutcomes.SurveyQuestionId,
                                 TbOutcomes.SurveyLogic,TbOutcomes.SurveyTarget,TbOutcomes.WorkflowId,TbProjectWorkflow.WfName as WorkflowName,TbOutcomes.Periodicity,TbOutcomes.IsDeleted,
                            TbOutcomes.CreatedDate,TbOutcomes.CreatedBy,TbOutcomes.UpdatedBy,ifnull(TbOutcomes.UpdatedDate,TbOutcomes.CreatedDate) UpdatedDate,TbPeriodicity.PeriodicityType as ReportingFrequencyName,TbOutcomes.Description,
                            ifnull(TbOutcomes.AdditionalSectionId,0) as AdditionalSectionId,ifnull(TbOutcomes.AdditionalQuestionId,0) as AdditionalQuestionId,ifnull(TbOutcomes.AdditionalQuestionType,false) as AdditionalQuestionType  
                            from TbOutcomes TbOutcomes 
                            left join TbProjectWorkflow TbProjectWorkflow on TbProjectWorkflow.WorkflowId = TbOutcomes.WorkflowId
                            left join TbProjectSurvey TbProjectSurvey on TbProjectSurvey.SurveyId = TbOutcomes.SurveyId
                            left join TbPeriodicity TbPeriodicity on TbPeriodicity.PeriodicityId = TbOutcomes.Periodicity
                            where TbOutcomes.IsDeleted = 0 and TbOutcomes.ProjectId = @ProjectId and FIND_IN_SET(TbOutcomes.pk_OutcomeID, @joinedOutcomeIds) 
                            order by TbOutcomes.pk_OutcomeID desc";
                    ObjOutcome = _connection.Query<ProjectOutcome>(strQuery, new { ProjectId = ProjectId, joinedOutcomeIds = joinedOutcomeIds }).ToList();

                    int OutcomeID = 0;
                    int logicType = 0;
                    string SurveyTarget = "";

                    foreach (ProjectOutcome Item in ObjOutcome)
                    {
                        if (Item.DatacollectionType == "Quantitative")
                        {
                            OutcomeID = Item.pk_OutcomeID;
                            logicType = Item.SurveyLogic;
                            SurveyTarget = Item.SurveyTarget;

                            query = "SELECT ACT.ID,ACT.OutcomeId ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                            query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType FROM TbProjectSurvey SRV ";
                            query += "LEFT JOIN TbOutcomeSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                            query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                            query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                            query += "where ACT.OutcomeId = @OutcomeId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1; ";

                            objActivityDataCollectionDetails = _connection.Query<OutcomeDataCollectionDetails>(query, new { OutcomeId = OutcomeID }).ToList();

                            if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                            {
                                //var mySKUs = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                //string joinedQuestionIds = string.Join(",", mySKUs);

                                //var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                                //string joinedBeneficiaryTypeID = string.Join(",", mySKUs1);

                                //var benficiaryIds = objActivityDataCollectionDetails.Where(x => x.BeneficiaryQuestionID > 0).Select(l => l.BeneficiaryQuestionID).Distinct().ToList();
                                //string joinedBeneficiaryQuestionID = string.Join(",", benficiaryIds);

                                //List<OutcomeSurveyResponse> lstActivitySurveyResponse = new List<OutcomeSurveyResponse>();
                                //List<OutcomeSurveyResponse> lstActivitySurveyResponse1 = new List<OutcomeSurveyResponse>();
                                //List<OutcomeSurveyResponse> lstBeneficiarySurveyResponse = new List<OutcomeSurveyResponse>();

                                //if (joinedQuestionIds != null && joinedQuestionIds.Length > 0)
                                //{
                                //    string QSTQry = "SELECT ResponseID,Answer,QuestionID, SurveyID,SyncTaskID FROM TbProjectSurveyResponses where QuestionID in(" + joinedQuestionIds + ") and ifnull(IsActive,0)=1;";
                                //    lstActivitySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry).ToList();

                                //    lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                //}

                                //if (joinedBeneficiaryQuestionID != null && joinedBeneficiaryQuestionID.Length > 0)
                                //{
                                //    string QSTBenQry = "SELECT reg.BeneficiaryID as ResponseID,reg.Answer,reg.QuestionID, ";
                                //    QSTBenQry += "(select SurveyID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID limit 1) as SurveyID,";
                                //    QSTBenQry += "(select SyncTaskID from TbProjectSurveyResponses where ResponseID = reg.BeneficiaryID  limit 1) as SyncTaskID,reg.CreatedDate ";
                                //    QSTBenQry += " FROM TbBeneficiaryRegistrationDetails reg where reg.QuestionID in(" + joinedBeneficiaryQuestionID + ") and ifnull(reg.IsDeleted,0)=0;";
                                //    lstBeneficiarySurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTBenQry).ToList();

                                //    lstActivitySurveyResponse1 = lstActivitySurveyResponse.GroupBy(p => new { p.ResponseID }).Select(g => g.First()).ToList();
                                //}

                                Item.lstOutcomeCount = new List<OutcomeCount>();
                                Item.lstOutcomeCount.AddRange(GetResponseForOutcomeReport(OutcomeID, logicType, SurveyTarget, Item.AdditionalSectionId, Item.AdditionalQuestionId, Item.AdditionalQuestionType));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjOutcome;
        }
        #endregion getDataForOutcomeReport

        #region GetResponseForOutcomeReport
        /// <summary>
        /// GetResponseForOutcomeReport
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="logicType"></param>
        /// <param name="value"></param>
        public List<OutcomeCount> GetResponseForOutcomeReport(int OutcomeID, int logicType, string SurveyTarget, int AdditionalSectionId, int additionalSurveyQuestionId = 0, bool? additionalQuestionType = false)
        {
            List<OutcomeCount> lstOutcomeCount = new List<OutcomeCount>();

            List<OutcomeDataCollectionDetails> objActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();
            List<OutcomeDataCollectionDetails> lstBeneficiaryActivityDataCollectionDetails = new List<OutcomeDataCollectionDetails>();

            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse = new List<OutcomeSurveyResponse>();
            string query = string.Empty;

            double result = 0;


            bool IsSurveyQuestion = true;

            if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
            {
                ProjectSurveyDAL objProjectSurveyDAL = new ProjectSurveyDAL();
                IsSurveyQuestion = objProjectSurveyDAL.IsSurveyQuestion(AdditionalSectionId, additionalSurveyQuestionId);
            }

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "SELECT ACT.ID,ACT.OutcomeId ActivityId,ACT.SurveyId,ACT.SectionId, ACT.QuestionId,BeneficiaryQuestionID,SRV.BeneficiaryTypeID,SRV.StartDate,SRV.EndDate ";
                    query += ",ifnull(SRVQST.QuestionTypeID,0) SurveyAnswerType,ifnull(BENQST.AnswerTypeID,0) BenAnswerType,srvLogic.SurveyLogic,srvLogic.SurveyAnswer,ifnull(srvLogic.LogicCondition,0) as LogicCondition ";
                    query += " FROM TbProjectSurvey SRV  LEFT JOIN TbOutcomeSurveyCollection ACT  ON ACT.SurveyId = SRV.SurveyId ";
                    query += "LEFT JOIN TbProjectSurveyQuestion SRVQST ON SRVQST.QuestionID =  ACT.QuestionId ";
                    query += "LEFT JOIN TbBeneficiaryTypeQuestion BENQST ON BENQST.QuestionID =  ACT.BeneficiaryQuestionID ";
                    query += "left join TbOutcomeSurveyLogic srvLogic ON srvLogic.SurveyCollectionID =  ACT.ID ";
                    query += "where ACT.OutcomeId = @OutcomeId  and  ifnull(ACT.IsActive,0)=1 AND ifnull(SRV.IsActive,0)=1 and ifnull(srvLogic.IsActive,0)=1; ";

                    objActivityDataCollectionDetails = _connection.Query<OutcomeDataCollectionDetails>(query, new { OutcomeId = OutcomeID }).ToList();

                    if (objActivityDataCollectionDetails != null && objActivityDataCollectionDetails.Count > 0)
                    {
                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();

                        for (int i = 0; i < objActivityDataCollectionDetails.Count; i++)
                        {
                            string querySurveyPrime = "select LG.SurveyLogic SurveyLogicPrime,LG.SurveyAnswer SurveyLogicPrimeAnswer, LGType.Type SurveyLogicPrimeType,srvCollection.BeneficiaryQuestionID,ifnull(LG.LogicCondition,0) SurveyLogicCondition from TbOutcomeSurveyLogic LG JOIN TbLogicType LGType ON LG.SurveyLogic = LGType.ID";
                            querySurveyPrime += " JOIN TbOutcomeSurveyCollection srvCollection ON LG.SurveyCollectionID = srvCollection.ID WHERE srvCollection.ID = @ID AND ifnull(LG.IsActive,0) = 1; ";
                            List<OutcomeSurveyPrimeLogic> lstSurveyPrimeLogic = new List<OutcomeSurveyPrimeLogic>();

                            lstSurveyPrimeLogic = _connection.Query<OutcomeSurveyPrimeLogic>(querySurveyPrime, new { ID = objActivityDataCollectionDetails[i].ID }).ToList();

                            if (objActivityDataCollectionDetails[i].BeneficiaryTypeID <= 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetNonBeneficiarySurveyLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)//(lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() > 0)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].QuestionId > 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetBeneficiarySurveyResponseLogicQuery(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].SurveyAnswerType);

                                string query1 = "SELECT ResponseID,SyncTaskID,SubmissionCount from TbProjectSurveyResponses where QuestionID = @QuestionId AND ifnull(IsActive,0) = 1 ";
                                query1 += subqury;
                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].QuestionId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() > 0)
                                                        if (itm2.ResponseID == itm1.ResponseID && itm2.SyncTaskID == itm1.SyncTaskID && itm2.SubmissionCount == itm1.SubmissionCount)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID && x.SyncTaskID == itm1.SyncTaskID && x.SubmissionCount == itm1.SubmissionCount).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (objActivityDataCollectionDetails[i].BeneficiaryTypeID > 0 && objActivityDataCollectionDetails[i].BeneficiaryQuestionID > 0)
                            {
                                List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataInner = new List<ProjectOutcomeInitialData>();
                                string subqury = GetBeneficiarySurveyLogicQuery2(lstSurveyPrimeLogic, objActivityDataCollectionDetails[i].BenAnswerType);

                                string query1 = "SELECT DISTINCT BeneficiaryID AS ResponseID,RSP.SurveyID FROM TbBeneficiaryRegistrationDetails reg  JOIN ";
                                query1 += " TbProjectSurveyResponses RSP ON reg.BeneficiaryID = RSP.ResponseID WHERE reg.QuestionID = @QuestionId AND ifnull(reg.IsDeleted,0) = 0 AND  RSP.SurveyID=@SurveyId ";
                                query1 += subqury;

                                lstProjectOutcomeInitialDataInner = _connection.Query<ProjectOutcomeInitialData>(query1, new { QuestionId = objActivityDataCollectionDetails[i].BeneficiaryQuestionID, SurveyId = objActivityDataCollectionDetails[i].SurveyId }).ToList();

                                if (lstProjectOutcomeInitialDataInner != null && lstProjectOutcomeInitialDataInner.Count > 0)
                                {
                                    if (i == 0 && (lstProjectOutcomeInitialData == null || lstProjectOutcomeInitialData.Count == 0))
                                    {
                                        lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataInner);
                                    }
                                    else
                                    {
                                        List<ProjectOutcomeInitialData> lstProjectOutcomeInitialDataFilter = new List<ProjectOutcomeInitialData>();

                                        if (i > 0 && objActivityDataCollectionDetails[i - 1].LogicCondition == (int)CommonActivity.LogicCondition.AND)
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                foreach (var itm2 in lstProjectOutcomeInitialData)
                                                {
                                                    if (itm1.SyncTaskID == 0 && itm1.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm2);
                                                        }
                                                    }
                                                    else if (itm2.SyncTaskID == 0 && itm2.SubmissionCount == 0)
                                                    {
                                                        if (itm1.ResponseID == itm2.ResponseID)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (itm2.ResponseID == itm1.ResponseID)
                                                        //if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() > 0)
                                                        {
                                                            lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                        }
                                                    }
                                                }
                                            }

                                            lstProjectOutcomeInitialData = new List<ProjectOutcomeInitialData>();
                                            lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                        }
                                        else
                                        {
                                            foreach (var itm1 in lstProjectOutcomeInitialDataInner)
                                            {
                                                if (lstProjectOutcomeInitialData.Where(x => x.ResponseID == itm1.ResponseID).Count() <= 0)
                                                {
                                                    lstProjectOutcomeInitialDataFilter.Add(itm1);
                                                }
                                            }

                                            if (lstProjectOutcomeInitialDataFilter != null && lstProjectOutcomeInitialDataFilter.Count > 0)
                                            {
                                                lstProjectOutcomeInitialData.AddRange(lstProjectOutcomeInitialDataFilter);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        var myBenRes = lstProjectOutcomeInitialData.Where(x => x.SyncTaskID == 0).Select(l => l.ResponseID).Distinct().ToList();
                        string joinedBeneficiaryResponseIDs = string.Join(",", myBenRes);

                        if (lstProjectOutcomeInitialData != null && lstProjectOutcomeInitialData.Count > 0)
                        {
                            var myRes = lstProjectOutcomeInitialData.Select(l => l.ResponseID).Distinct().ToList();
                            string joinedResponseIDs = string.Join(",", myRes);

                            if (joinedResponseIDs == "0" || joinedResponseIDs == "")
                            {
                                var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);

                                double sum = 0;
                                double count = 0;

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                string joinedQuestionIds = string.Join(",", mySKUs1);

                                if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                                {
                                    string QSTQry4 = "select ResponseID,Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ");";
                                    lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                                    List<OutcomeSurveyResponse> lstOutcomeSurveyResponseInner = new List<OutcomeSurveyResponse>();

                                    foreach (var itm in lstProjectOutcomeInitialData)
                                    {
                                        lstOutcomeSurveyResponseInner = new List<OutcomeSurveyResponse>();
                                        lstOutcomeSurveyResponseInner = lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount && x.QuestionID == additionalSurveyQuestionId).ToList();

                                        if (lstOutcomeSurveyResponseInner.Count() > 0)
                                        {
                                            foreach (var itm1 in lstOutcomeSurveyResponseInner)
                                            {
                                                OutcomeCount objOutPutCount = new OutcomeCount();

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
                                    lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                                    List<OutcomeSurveyResponse> lstOutcomeSurveyResponseInner = new List<OutcomeSurveyResponse>();

                                    foreach (var itm in lstProjectOutcomeInitialData)
                                    {
                                        lstOutcomeSurveyResponseInner = new List<OutcomeSurveyResponse>();

                                        if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                        {
                                            OutcomeSurveyResponse objOutcomeSurveyResponse = new OutcomeSurveyResponse();
                                            objOutcomeSurveyResponse = lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).FirstOrDefault();
                                            
                                            if (objOutcomeSurveyResponse != null)
                                            {
                                                lstOutcomeSurveyResponseInner.Add(objOutcomeSurveyResponse);
                                            }
                                        }
                                        else
                                        {
                                            lstOutcomeSurveyResponseInner = lstOutcomeSurveyResponse.Where(x => x.SyncTaskID == itm.SyncTaskID && x.SubmissionCount == itm.SubmissionCount).ToList();
                                        }

                                        if (lstOutcomeSurveyResponseInner.Count() > 0)
                                        {
                                            foreach (var itm1 in lstOutcomeSurveyResponseInner)
                                            {
                                                OutcomeCount objOutPutCount = new OutcomeCount();

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
                                var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);
                                List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

                                var mySKUs1 = objActivityDataCollectionDetails.Select(l => l.QuestionId).Distinct().ToList();
                                string joinedQuestionIds = string.Join(",", mySKUs1);

                                if (additionalSurveyQuestionId > 0 && additionalQuestionType.Value == true)
                                {
                                    if (IsSurveyQuestion)
                                    {
                                        string QSTQry4 = "select ResponseID,ifnull(Answer,0) Answer,QuestionID,SurveyID,SubmissionCount,SyncTaskID,CreatedDate from TbProjectSurveyResponses  WHERE  SyncTaskID in(" + joinedSyncTaskIDs + ") and ResponseID in(" + joinedResponseIDs + ");";
                                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                                    }
                                    else
                                    {
                                        string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                                    }

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        foreach (var itmres in strSplit)
                                        {
                                            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                            lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                            if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutcomeSurveyResponse1)
                                                {
                                                    OutcomeCount objOutPutCount = new OutcomeCount();
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
                                    lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        double totSum = 0;
                                        double average = 0;

                                        foreach (var itmres in strSplit)
                                        {
                                            totSum = 0;
                                            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();

                                            if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                            {
                                                OutcomeSurveyResponse objOutcomeSurveyResponse = new OutcomeSurveyResponse();
                                                objOutcomeSurveyResponse = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).FirstOrDefault();

                                                if (objOutcomeSurveyResponse != null)
                                                {
                                                    lstOutcomeSurveyResponse1.Add(objOutcomeSurveyResponse);
                                                }
                                            }
                                            else
                                            {
                                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();
                                            }

                                            if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutcomeSurveyResponse1)
                                                {
                                                    OutcomeCount objOutPutCount = new OutcomeCount();
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
                                var mySKUs = lstProjectOutcomeInitialData.Select(l => l.SyncTaskID).Distinct().ToList();
                                string joinedSyncTaskIDs = string.Join(",", mySKUs);
                                List<ProjectOutcomeBeneficiaryData> lstProjectOutcomeBeneficiaryData = new List<ProjectOutcomeBeneficiaryData>();

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
                                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4, new { QuestionID = additionalSurveyQuestionId }).ToList();
                                    }
                                    else
                                    {
                                        string QSTQry4 = "select BeneficiaryID as ResponseID,ifnull(Answer,0) Answer,QuestionID FROM TbBeneficiaryRegistrationDetails WHERE BeneficiaryID in(" + joinedResponseIDs + ");";
                                        lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();
                                    }

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        foreach (var itmres in strSplit)
                                        {
                                            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();
                                            lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres) && x.QuestionID == additionalSurveyQuestionId).ToList();

                                            if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutcomeSurveyResponse1)
                                                {
                                                    OutcomeCount objOutPutCount = new OutcomeCount();

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
                                    string QSTQry4 = "SELECT reg.BeneficiaryID AS ResponseID,reg.QuestionID,reg.Answer FROM TbBeneficiaryRegistrationDetails reg  WHERE reg.QuestionID  in(" + joinedBeneficiaryQuestionIDs + ");";
                                    lstOutcomeSurveyResponse = _connection.Query<OutcomeSurveyResponse>(QSTQry4).ToList();

                                    string[] strSplit = joinedResponseIDs.Split(",");

                                    if (strSplit != null && strSplit.Length > 0)
                                    {
                                        double totSum = 0;
                                        //double average = 0;

                                        foreach (var itmres in strSplit)
                                        {
                                            totSum = 0;
                                            List<OutcomeSurveyResponse> lstOutcomeSurveyResponse1 = new List<OutcomeSurveyResponse>();

                                            if (logicType == (int)OutcomeLogicType.Count_of_Unique_Reponses)
                                            {
                                                OutcomeSurveyResponse objOutcomeSurveyResponse = new OutcomeSurveyResponse();
                                                objOutcomeSurveyResponse = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).FirstOrDefault();

                                                if (objOutcomeSurveyResponse != null)
                                                {
                                                    lstOutcomeSurveyResponse1.Add(objOutcomeSurveyResponse);
                                                }
                                            }
                                            else
                                            {
                                                lstOutcomeSurveyResponse1 = lstOutcomeSurveyResponse.Where(x => x.ResponseID == Convert.ToInt32(itmres)).ToList();
                                            }

                                            if (lstOutcomeSurveyResponse1 != null && lstOutcomeSurveyResponse1.Count > 0)
                                            {
                                                foreach (var itm1 in lstOutcomeSurveyResponse1)
                                                {
                                                    OutcomeCount objOutPutCount = new OutcomeCount();

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
        #endregion GetResponseForOutcomeReport
    }
}
