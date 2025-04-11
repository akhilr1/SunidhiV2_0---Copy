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
using SunidhiV2_0.Code.Common;
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace SunidhiV2_0.Code.DAL
{
    public class ThemeTemplateDAL
    {
        protected IDbConnection _connection;

        #region CreateTemplateProjectKPI
        /// <summary>
        /// CreateTemplateProjectKPI
        /// </summary>
        /// <param name="objProjectKPI"></param>
        /// <param name="project_id"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateTemplateProjectKPI(KPITemplate objProjectKPI, int project_id, int loggedUser)
        {
            string strQuery = string.Empty;
            int KPI_ID = 0;
            DateTime startDate = CommonActivity.GetDateTimeValue(objProjectKPI.StartDate).Value;
            DateTime endDate = CommonActivity.GetDateTimeValue(objProjectKPI.EndDate).Value;
            int periodicity = Convert.ToInt32(objProjectKPI.KPIPeriodicity);
            int month = (!string.IsNullOrEmpty(objProjectKPI.Month) ? Convert.ToInt32(objProjectKPI.Month) : 0);
            int day = (!string.IsNullOrEmpty(objProjectKPI.Day) ? Convert.ToInt32(objProjectKPI.Day) : 0);

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbKPI(KPIName,Description,KPIPeriodicity,StartDate,EndDate,WorkflowID,";
                    strQuery += "ProjectID,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)";
                    strQuery += "VALUES(@KPIName,@Description,@KPIPeriodicity,@StartDate,@EndDate,@WorkflowID,";
                    strQuery += "@ProjectID,@IsDeleted,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    KPI_ID = _connection.Query<int>(strQuery, new
                    {
                        KPIName = objProjectKPI.KPIName,
                        Description = objProjectKPI.Description,
                        KPIPeriodicity = objProjectKPI.KPIPeriodicity,
                        StartDate = startDate,
                        EndDate = endDate,
                        WorkflowID = 0,
                        ProjectID = project_id,
                        IsDeleted = 0,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser
                    }).Single();
                }

                if (KPI_ID > 0)
                {
                    List<string> lstTrackDates = new List<string>();
                    lstTrackDates = CommonActivity.GetReportingFrequencyDateList(startDate, endDate, periodicity, day, month, objProjectKPI.OnceDate); // Get Track Dates  
                    InsertIntoTrackDates(lstTrackDates, KPI_ID, "KPI"); // Inserting Track Dates into table
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return KPI_ID;
        }
        #endregion CreateTemplateProjectKPI

        #region CreateTemplateProjectRisk
        /// <summary>
        /// CreateTemplateProjectRisk
        /// </summary>
        /// <param name="objProjectRisk"></param>
        /// <param name="project_id"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateTemplateProjectRisk(RiskTemplate objProjectRisk, int project_id, int loggedUser)
        {
            string strQuery = string.Empty;
            int risk_Id = 0;
            DateTime startDate = CommonActivity.GetDateTimeValue(objProjectRisk.StartDate).Value;
            DateTime endDate = CommonActivity.GetDateTimeValue(objProjectRisk.EndDate).Value;
            int periodicity = Convert.ToInt32(objProjectRisk.RiskPeriodicity);
            int month = (!string.IsNullOrEmpty(objProjectRisk.Month) ? Convert.ToInt32(objProjectRisk.Month) : 0);
            int day = (!string.IsNullOrEmpty(objProjectRisk.Day) ? Convert.ToInt32(objProjectRisk.Day) : 0);

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbRisk(RiskName,RiskType,Description,RiskPeriodicity,RiskRating,StartDate,";
                    strQuery += "EndDate,WorkflowID,ProjectID,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)";
                    strQuery += "VALUES(@RiskName,@RiskType,@Description,@RiskPeriodicity,@RiskRating,@StartDate,";
                    strQuery += "@EndDate,@WorkflowID,@ProjectID,@IsDeleted,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    risk_Id = _connection.Query<int>(strQuery, new
                    {
                        RiskName = objProjectRisk.RiskName,
                        RiskType = objProjectRisk.RiskType,
                        Description = objProjectRisk.Description,
                        RiskPeriodicity = objProjectRisk.RiskPeriodicity,
                        RiskRating = objProjectRisk.RiskRating,
                        StartDate = startDate,
                        EndDate = endDate,
                        WorkflowID = 0,
                        ProjectID = project_id,
                        IsDeleted = 0,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser
                    }).Single();
                }

                // TRACK DATE DETAILS 
                if (risk_Id > 0)
                {
                    List<string> lstTrackDates = new List<string>();
                    lstTrackDates = CommonActivity.GetReportingFrequencyDateList(startDate, endDate, periodicity, day, month, objProjectRisk.OnceDate); // Get Track Dates  
                    InsertIntoTrackDates(lstTrackDates, risk_Id, "RISK"); // Inserting Track Dates into table
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return risk_Id;
        }
        #endregion CreateTemplateProject

        #region CreateTemplateProjectActivity
        /// <summary>
        /// CreateTemplateProjectActivity
        /// </summary>
        /// <param name="objActivity"></param>
        /// <param name="project_id"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateTemplateProjectActivity(ActivityTemplate objActivity, int project_id, int loggedUser)
        {
            string strQuery = string.Empty;
            int activity_Id = 0;
            DateTime startDate = CommonActivity.GetDateTimeValue(objActivity.StartDate).Value;
            DateTime endDate = CommonActivity.GetDateTimeValue(objActivity.EndDate).Value;
            int periodicity = Convert.ToInt32(objActivity.Periodicity);
            int month = (!string.IsNullOrEmpty(objActivity.Month) ? Convert.ToInt32(objActivity.Month) : 0);
            int day = (!string.IsNullOrEmpty(objActivity.Day) ? Convert.ToInt32(objActivity.Day) : 0);

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectActivity(ProjectId,ParentActivityId,ActivityName,Description";
                    strQuery += ",StartDate,EndDate,WeightagePercentage,ActivityType,IsPaymentLinked,WorkflowId";
                    strQuery += ",FinanceWorkflowId,Periodicity,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)";
                    strQuery += "VALUES(@ProjectId,@ParentActivityId,@ActivityName,@Description,@StartDate,@EndDate,";
                    strQuery += "@WeightagePercentage,@ActivityType,@IsPaymentLinked,@WorkflowId,@FinanceWorkflowId,";
                    strQuery += "@Periodicity,@IsDeleted,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    activity_Id = _connection.Query<int>(strQuery, new
                    {
                        ProjectId = project_id,
                        ParentActivityId = 0,
                        ActivityName = objActivity.ActivityName,
                        Description = objActivity.Description,
                        StartDate = startDate,
                        EndDate = endDate,
                        WeightagePercentage = objActivity.WeightagePercentage,
                        ActivityType = objActivity.ActivityType,
                        IsPaymentLinked = objActivity.IsPaymentLinked,
                        WorkflowId = 0,
                        FinanceWorkflowId = 0,
                        Periodicity = objActivity.Periodicity,
                        IsDeleted = 0,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                    }).Single();
                }

                // TRACK DATE DETAILS 
                if (objActivity.SubActivityList != null &&
                    objActivity.SubActivityList.Count > 0)
                {
                    InsertSubActivityList(objActivity.SubActivityList, activity_Id, loggedUser, project_id);
                }
                else
                {
                    List<string> lstTrackDates = new List<string>();
                    lstTrackDates = CommonActivity.GetReportingFrequencyDateList(startDate, endDate, periodicity, day, month, objActivity.OnceDate); // Get Track Dates  
                    InsertIntoTrackDates(lstTrackDates, activity_Id, "ACTIVITY"); // Inserting Track Dates into table
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return activity_Id;
        }
        #endregion CreateTemplateProjectActivity

        #region CreateTemplateProjectSurvey
        /// <summary>
        /// CreateTemplateProjectSurvey
        /// </summary>
        /// <param name="objSurvey"></param>
        /// <param name="project_id"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateTemplateProjectSurvey(SurveyTemplate objSurvey, int project_id, int loggedUser)
        {
            string strQuery = string.Empty;
            int Survey_Id = 0;
            DateTime startDate = CommonActivity.GetDateTimeValue(objSurvey.StartDate).Value;
            DateTime endDate = CommonActivity.GetDateTimeValue(objSurvey.EndDate).Value;
            int periodicity = Convert.ToInt32(objSurvey.SurveyPeriodicity);
            int month = (!string.IsNullOrEmpty(objSurvey.Month) ? Convert.ToInt32(objSurvey.Month) : 0);
            int day = (!string.IsNullOrEmpty(objSurvey.Day) ? Convert.ToInt32(objSurvey.Day) : 0);

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectSurvey(SurveyName,SurveyDescription,ProjectId,WorkflowID,ReportingFrequency";
                    strQuery += ",IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,StartDate,EndDate)";
                    strQuery += "VALUES(@SurveyName,@SurveyDescription,@ProjectId,@WorkflowID,@ReportingFrequency";
                    strQuery += ",@IsActive,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy,@StartDate,@EndDate);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    Survey_Id = _connection.Query<int>(strQuery, new
                    {
                        SurveyName = objSurvey.SurveyName,
                        SurveyDescription = objSurvey.SurveyDescription,
                        ProjectId = project_id,
                        WorkflowID = 0,
                        ReportingFrequency = periodicity,
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                        StartDate = startDate,
                        EndDate = endDate
                    }).Single();
                }

                if (Survey_Id > 0)
                {
                    List<string> lstTrackDates = new List<string>();
                    lstTrackDates = CommonActivity.GetReportingFrequencyDateList(startDate, endDate, periodicity, day, month, objSurvey.OnceDate); // Get Track Dates  
                    InsertIntoTrackDates(lstTrackDates, Survey_Id, "SURVEY"); // Inserting Track Dates into table
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Survey_Id;
        }
        #endregion CreateTemplateProjectSurvey

        #region CreateSurveySection
        /// <summary>
        /// CreateSurveySection
        /// </summary>
        /// <param name="objSection">objSection</param>
        /// <param name="project_id">project_id</param>
        /// <param name="loggedUser">loggedUser</param>
        /// <returns>int</returns>
        public int CreateSurveySection(SectionTemplate objSection, int surveyId, int loggedUser)
        {
            string strQuery = string.Empty;
            int Section_Id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectSurveySection(SectionName,SectionDescription,SectionOrder";
                    strQuery += ",SurveyID,IsActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)";
                    strQuery += "VALUES(@SectionName,@SectionDescription,@SectionOrder,@SurveyID";
                    strQuery += ",@IsActive,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    Section_Id = _connection.Query<int>(strQuery, new
                    {
                        SectionName = objSection.SectionName,
                        SectionDescription = objSection.SectionDescription,
                        SectionOrder = objSection.SectionOrder,
                        SurveyID = surveyId,
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser
                    }).Single();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Section_Id;
        }
        #endregion CreateSurveySection

        #region CreateSurveyQuestion
        /// <summary>
        /// CreateSurveyQuestion
        /// </summary>
        /// <param name="objQuestion"></param>
        /// <param name="surveyId"></param>
        /// <param name="sectionId"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateSurveyQuestion(QuestionTemplate objQuestion, int surveyId, int sectionId, int loggedUser)
        {
            string strQuery = string.Empty;
            int Question_Id = 0;
            try
            {
                int? IsUnique = (!string.IsNullOrEmpty(objQuestion.IsUnique) ? Convert.ToInt32(objQuestion.IsUnique) : (int?)null);
                int? IsSearchable = (!string.IsNullOrEmpty(objQuestion.IsSearchable) ? Convert.ToInt32(objQuestion.IsSearchable) : (int?)null);
                int? MinValue = (!string.IsNullOrEmpty(objQuestion.MinValue) ? Convert.ToInt32(objQuestion.MinValue) : (int?)null);
                int? MaxValue = (!string.IsNullOrEmpty(objQuestion.MaxValue) ? Convert.ToInt32(objQuestion.MaxValue) : (int?)null);
                int IsMandatory = (!string.IsNullOrEmpty(objQuestion.IsMandatory) ? Convert.ToInt32(objQuestion.IsMandatory) : 0);
                int? TextLength = (!string.IsNullOrEmpty(objQuestion.TextLength) ? Convert.ToInt32(objQuestion.TextLength) : (int?)null);
                int QuestionTypeID = (!string.IsNullOrEmpty(objQuestion.QuestionTypeID) ? Convert.ToInt32(objQuestion.QuestionTypeID) : 0);
                int QuestionOrder = (!string.IsNullOrEmpty(objQuestion.QuestionOrder) ? Convert.ToInt32(objQuestion.QuestionOrder) : 0);

                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectSurveyQuestion(QuestionName,QuestionTypeID,QuestionOrder,DefaultValue,";
                    strQuery += "SurveyID,SectionID,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,IsUnique,IsSearchable,";
                    strQuery += "MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,TextLength,ErrorText,FieldVar)";
                    strQuery += " VALUES(@QuestionName,@QuestionTypeID,@QuestionOrder,@DefaultValue,@SurveyID,@SectionID";
                    strQuery += ",@CreatedBy,@UpdatedBy,@CreatedDate,@UpdatedDate,@IsUnique,@IsSearchable,@MinValue";
                    strQuery += ",@MaximumValue,@IsActive,@RegEx,@IsMandatory,@TextLength,@ErrorText,@FieldVar);";
                    strQuery += " SELECT LAST_INSERT_ID();";
                    Question_Id = _connection.Query<int>(strQuery, new
                    {
                        QuestionName = objQuestion.QuestionName,
                        QuestionTypeID = QuestionTypeID,
                        QuestionOrder = QuestionOrder,
                        DefaultValue = objQuestion.DefaultValue,
                        SurveyID = surveyId,
                        SectionID = sectionId,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsUnique = IsUnique,
                        IsSearchable = IsSearchable,
                        MinValue = MinValue,
                        MaximumValue = MaxValue,
                        IsActive = 1,
                        RegEx = objQuestion.RegEx,
                        IsMandatory = IsMandatory,
                        TextLength = TextLength,
                        ErrorText = objQuestion.ErrorText,
                        FieldVar = objQuestion.FieldVar
                    }).Single();

                    if (objQuestion.lstQuestionOptions != null && objQuestion.lstQuestionOptions.Count > 0)
                    {
                        foreach (var opt in objQuestion.lstQuestionOptions)
                        {
                            string strOptQry = "INSERT INTO TbProjectSurveyQSTOption(OptionValue,QuestionID,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate)";
                            strOptQry += " VALUES(@OptionValue,@QuestionID,@CreatedBy,@UpdatedBy,@CreatedDate,@UpdatedDate);";

                            _connection.Execute(strOptQry, new
                            {
                                OptionValue = opt.OptionValue,
                                QuestionID = Question_Id,
                                CreatedBy = loggedUser,
                                UpdatedBy = loggedUser,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Question_Id;
        }
        #endregion CreateSurveyQuestion

        #region CreateTemplateProjectBeneficiaryType
        /// <summary>
        /// CreateTemplateProjectBeneficiaryType
        /// </summary>
        /// <param name="objBeneficiaryType"></param>
        /// <param name="project_id"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateTemplateProjectBeneficiaryType(BeneficiaryTypeTemplate objBeneficiaryType, int project_id, int loggedUser)
        {
            string strQuery = string.Empty;
            int beneficiary_type_Id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectBeneficiaryType(Name,Description,ProjectID,CreatedDate";
                    strQuery += ",UpdatedDate,CreatedBy,UpdatedBy,IsActive,IsDeleted)";
                    strQuery += "VALUES(@Name,@Description,@ProjectID,@CreatedDate,@UpdatedDate";
                    strQuery += ",@CreatedBy,@UpdatedBy,@IsActive,@IsDeleted);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    beneficiary_type_Id = _connection.Query<int>(strQuery, new
                    {
                        Name = objBeneficiaryType.BeneficiaryTypeName,
                        Description = objBeneficiaryType.BeneficiaryTypeDescription,
                        ProjectID = project_id,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                        IsActive = 1,
                        IsDeleted = 0
                    }).Single();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return beneficiary_type_Id;
        }
        #endregion CreateTemplateProjectBeneficiaryType

        #region CreateBeneficiaryTypeQuestion
        /// <summary>
        /// CreateBeneficiaryTypeQuestion
        /// </summary>
        /// <param name="objBeneficiaryQuestion">objBeneficiaryQuestion</param>
        /// <param name="beneficiaryTypeId">beneficiaryTypeId</param>
        /// <param name="loggedUser">loggedUser</param>
        /// <returns>int</returns>
        public int CreateBeneficiaryTypeQuestion(BeneficiaryQuestionTemplate objBeneficiaryQuestion, int beneficiaryTypeId, int loggedUser)
        {
            string strQuery = string.Empty;
            int Question_Id = 0;

            try
            {
                int? IsUnique = (!string.IsNullOrEmpty(objBeneficiaryQuestion.IsUnique) ? Convert.ToInt32(objBeneficiaryQuestion.IsUnique) : (int?)null);
                int? IsSearchable = (!string.IsNullOrEmpty(objBeneficiaryQuestion.IsSearchable) ? Convert.ToInt32(objBeneficiaryQuestion.IsSearchable) : (int?)null);
                int IsMandatory = (!string.IsNullOrEmpty(objBeneficiaryQuestion.IsMandatoryResponse) ? Convert.ToInt32(objBeneficiaryQuestion.IsMandatoryResponse) : 0);
                int QuestionTypeID = (!string.IsNullOrEmpty(objBeneficiaryQuestion.QuestionType) ? Convert.ToInt32(objBeneficiaryQuestion.QuestionType) : 0);

                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbBeneficiaryTypeQuestion(QuestionName,AnswerTypeID,BeneficiaryTypeID,IsUnique,IsSearchable,";
                    strQuery += "IsMandatoryResponse,QuestionHint,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,IsActive,IsDeleted)";
                    strQuery += "VALUES(@QuestionName,@AnswerTypeID,@BeneficiaryTypeID,@IsUnique,@IsSearchable,@IsMandatoryResponse";
                    strQuery += ",@QuestionHint,@CreatedBy,@UpdatedBy,@CreatedDate,@UpdatedDate,@IsActive,@IsDeleted);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    Question_Id = _connection.Query<int>(strQuery, new
                    {
                        QuestionName = objBeneficiaryQuestion.QuestionName,
                        AnswerTypeID = QuestionTypeID,
                        BeneficiaryTypeID = beneficiaryTypeId,
                        IsUnique = IsUnique,
                        IsSearchable = IsSearchable,
                        IsMandatoryResponse = IsMandatory,
                        QuestionHint = objBeneficiaryQuestion.QuestionHint,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsActive = 1,
                        IsDeleted = 0
                    }).Single();

                    if (objBeneficiaryQuestion.OptionList != null && objBeneficiaryQuestion.OptionList.Count > 0)
                    {
                        foreach (var opt in objBeneficiaryQuestion.OptionList)
                        {
                            string strOptQry = "INSERT INTO TbBeneficiaryTypeQstnAnswerOptions(BeneficiaryTypeID,QuestionID,AnswerTypeID";
                            strOptQry += ",OptionValue,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,IsActive,IsDeleted) VALUES(@BeneficiaryTypeID";
                            strOptQry += ",@QuestionID,@AnswerTypeID,@OptionValue,@CreatedBy,@UpdatedBy,@CreatedDate,@UpdatedDate,@IsActive,@IsDeleted);";
                            _connection.Execute(strOptQry, new
                            {
                                BeneficiaryTypeID = beneficiaryTypeId,
                                QuestionID = Question_Id,
                                AnswerTypeID = QuestionTypeID,
                                OptionValue = opt.OptionValue,
                                CreatedBy = loggedUser,
                                UpdatedBy = loggedUser,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                IsActive = 1,
                                IsDeleted = 0
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Question_Id;
        }
        #endregion CreateBeneficiaryTypeQuestion

        #region CreateProjectBeneficiary
        /// <summary>
        /// CreateProjectBeneficiary
        /// </summary>
        /// <param name="objBeneficiary"></param>
        /// <param name="beneficiaryTypeId"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public int CreateProjectBeneficiary(BeneficiaryTemplate objBeneficiary, int beneficiaryTypeId, int loggedUser)
        {
            string strQuery = string.Empty;
            int beneficiary_Id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "INSERT INTO TbProjectBeneficiary(BeneficiaryName,BeneficiaryTypeID,CreatedBy";
                    strQuery += ",UpdatedBy,CreatedDate,UpdatedDate,IsActive,IsDeleted)";
                    strQuery += "VALUES(@BeneficiaryName,@BeneficiaryTypeID,@CreatedBy,@UpdatedBy";
                    strQuery += ",@CreatedDate,@UpdatedDate,@IsActive,@IsDeleted);";
                    strQuery += "SELECT LAST_INSERT_ID();";
                    beneficiary_Id = _connection.Query<int>(strQuery, new
                    {
                        BeneficiaryName = objBeneficiary.BeneficiaryName,
                        BeneficiaryTypeID = beneficiaryTypeId,
                        CreatedBy = loggedUser,
                        UpdatedBy = loggedUser,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsActive = 1,
                        IsDeleted = 0
                    }).Single();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return beneficiary_Id;
        }
        #endregion CreateProjectBeneficiary

        #region InsertIntoTrackDates
        /// <summary>
        /// InsertIntoTrackDates
        /// </summary>
        /// <param name="lstTrackDates"></param>
        /// <param name="Id"></param>
        /// <param name="type"></param>
        public void InsertIntoTrackDates(List<string> lstTrackDates, int Id, string type)
        {
            string query = string.Empty;
            bool success = false;

            switch (type)
            {
                case "KPI":
                    query = "INSERT INTO TbKPITrackDate(TrackDate,KPIID,TrackStatus)";
                    break;
                case "RISK":
                    query = "INSERT INTO TbRiskTrackDate(TrackDate,RiskID,TrackStatus)";
                    break;
                case "ACTIVITY":
                    query = "INSERT INTO TbProjectActivityTrackDate(TrackDate,ActivityId,TrackStatus)";
                    break;
                case "SURVEY":
                    query = "INSERT INTO TbProjectSurveyTrackDate(TrackDate,SurveyID,TrackStatus)";
                    break;
            }

            if (lstTrackDates != null && lstTrackDates.Count > 0)
            {
                for (int i = 0; i < lstTrackDates.Count; i++)
                {
                    query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + Id + ",0" : " SELECT '" + lstTrackDates[i] + "'," + Id + ",0";
                }

                if (query.Length > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        var result = _connection.Execute(query);

                        if (result > 0)
                        {
                            success = true;
                        }
                    }
                }
            }
        }
        #endregion InsertIntoTrackDates

        #region InsertTemplatesProjectDetails
        /// <summary>
        /// InsertTemplatesProjectDetails
        /// </summary>
        /// <param name="objProjectclass"></param>
        /// <param name="loggedUser"></param>
        public void InsertTemplatesProjectDetails(ThemeTemplate objProjectclass, int loggedUser, string projectCode)
        {
            int project_id = 0;
            project_id = GetProjectIdByProjectCode(projectCode);

            //project_id = CreateTemplateProject(objProjectclass, loggedUser); // To Create Project

            // KPI Inserting
            if (objProjectclass.KpiList != null &&
                objProjectclass.KpiList.Count() > 0)
            {
                foreach (var itm in objProjectclass.KpiList)
                {
                    int KPI_ID = CreateTemplateProjectKPI(itm, project_id, loggedUser);
                }
            }

            // RISK Inserting
            if (objProjectclass.RiskList != null &&
                objProjectclass.RiskList.Count() > 0)
            {
                foreach (var itm in objProjectclass.RiskList)
                {
                    int Risk_Id = CreateTemplateProjectRisk(itm, project_id, loggedUser);
                }
            }

            // Activity Inserting
            if (objProjectclass.ActivityList != null &&
                objProjectclass.ActivityList.Count() > 0)
            {
                foreach (var itm in objProjectclass.ActivityList)
                {
                    int Activity_Id = CreateTemplateProjectActivity(itm, project_id, loggedUser);
                }
            }

            // Survey Inserting
            if (objProjectclass.SurveyList != null &&
                objProjectclass.SurveyList.Count() > 0)
            {
                foreach (var srv in objProjectclass.SurveyList)
                {
                    int survey_Id = CreateTemplateProjectSurvey(srv, project_id, loggedUser);

                    if (survey_Id > 0)
                    {

                        foreach (var sec in srv.SectionList)
                        {
                            int section_Id = CreateSurveySection(sec, survey_Id, loggedUser);

                            if (section_Id > 0)
                            {
                                foreach (var qst in sec.QuestioinList)
                                {
                                    int question_Id = CreateSurveyQuestion(qst, survey_Id, section_Id, loggedUser);
                                }
                            }
                        }
                    }
                }
            }


            // Beneficiary Inserting
            if (objProjectclass.BeneficiaryTypeList != null &&
                objProjectclass.BeneficiaryTypeList.Count() > 0)
            {
                foreach (var bnfType in objProjectclass.BeneficiaryTypeList)
                {
                    int beneficiary_type_id = CreateTemplateProjectBeneficiaryType(bnfType, project_id, loggedUser);

                    if (beneficiary_type_id > 0)
                    {

                        foreach (var qst in bnfType.BeneficiaryQuestionList)
                        {
                            int question_Id = CreateBeneficiaryTypeQuestion(qst, beneficiary_type_id, loggedUser);
                        }

                        foreach (var bnf in bnfType.BeneficiaryList)
                        {
                            int beneficiary_Id = CreateProjectBeneficiary(bnf, beneficiary_type_id, loggedUser);
                        }
                    }
                }
            }
        }
        #endregion  InsertTemplatesProjectDetails

        #region InsertSubActivityList
        /// <summary>
        /// InsertSubActivityList
        /// </summary>
        /// <param name="lstSubActivity"></param>
        /// <param name="activityId"></param>
        /// <param name="loggedUser"></param>
        /// <returns></returns>
        public void InsertSubActivityList(List<SubActivityTemplate> lstSubActivity, int activityId, int loggedUser, int project_Id = 0)
        {
            string strQuery = string.Empty;
            try
            {
                foreach (var itm in lstSubActivity)
                {
                    int new_activity_Id = 0;
                    DateTime startDate = CommonActivity.GetDateTimeValue(itm.StartDate).Value;
                    DateTime endDate = CommonActivity.GetDateTimeValue(itm.EndDate).Value;
                    int periodicity = Convert.ToInt32(itm.Periodicity);
                    int month = (!string.IsNullOrEmpty(itm.Month) ? Convert.ToInt32(itm.Month) : 0);
                    int day = (!string.IsNullOrEmpty(itm.Day) ? Convert.ToInt32(itm.Day) : 0);

                    string parentTempActivityId = itm.TempActivityId.Substring(0, itm.TempActivityId.LastIndexOf("."));
                    int parent_Activity_Id = 0;

                    if (parentTempActivityId != null && parentTempActivityId.Length == 1)
                    {
                        parent_Activity_Id = activityId;
                    }
                    else
                    {
                        var parentItem = lstSubActivity.Where(n => n.TempActivityId == parentTempActivityId).FirstOrDefault();

                        if (parentItem != null && parentItem.TempActivityId != null)
                        {
                            parent_Activity_Id = (!string.IsNullOrEmpty(parentItem.ActualActivityId) ? Convert.ToInt32(parentItem.ActualActivityId) : 0);
                        }
                    }

                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = "INSERT INTO TbProjectActivity(ProjectId,ParentActivityId,ActivityName,Description";
                        strQuery += ",StartDate,EndDate,WeightagePercentage,ActivityType,IsPaymentLinked,WorkflowId";
                        strQuery += ",FinanceWorkflowId,Periodicity,IsDeleted,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)";
                        strQuery += "VALUES(@ProjectId,@ParentActivityId,@ActivityName,@Description,@StartDate,@EndDate,";
                        strQuery += "@WeightagePercentage,@ActivityType,@IsPaymentLinked,@WorkflowId,@FinanceWorkflowId,";
                        strQuery += "@Periodicity,@IsDeleted,@CreatedDate,@UpdatedDate,@CreatedBy,@UpdatedBy);";
                        strQuery += "SELECT LAST_INSERT_ID();";
                        new_activity_Id = _connection.Query<int>(strQuery, new
                        {
                            ProjectId = project_Id,
                            ParentActivityId = parent_Activity_Id,
                            ActivityName = itm.ActivityName,
                            Description = itm.Description,
                            StartDate = startDate,
                            EndDate = endDate,
                            WeightagePercentage = itm.WeightagePercentage,
                            ActivityType = itm.ActivityType,
                            IsPaymentLinked = itm.IsPaymentLinked,
                            WorkflowId = 0,
                            FinanceWorkflowId = 0,
                            Periodicity = itm.Periodicity,
                            IsDeleted = 0,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            CreatedBy = loggedUser,
                            UpdatedBy = loggedUser,
                        }).Single();
                    }

                    itm.ActualActivityId = new_activity_Id.ToString();

                    var nextLevelActivity = itm.TempActivityId + ".";
                    var nextLevelItem = lstSubActivity.Where(n => n.TempActivityId.Contains(nextLevelActivity)).ToList();

                    if (nextLevelItem == null || nextLevelItem.Count == 0)
                    {
                        List<string> lstTrackDates = new List<string>();
                        lstTrackDates = CommonActivity.GetReportingFrequencyDateList(startDate, endDate, periodicity, day, month, itm.OnceDate); // Get Track Dates  
                        InsertIntoTrackDates(lstTrackDates, new_activity_Id, "ACTIVITY"); // Inserting Track Dates into table
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion InsertSubActivityList

        #region GetProjectIdByProjectCode
        /// <summary>
        /// GetProjectIdByProjectCode
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns>int</returns>
        public int GetProjectIdByProjectCode(string projectCode)
        {
            int project_Id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                string strQuery = @"SELECT ProjectId From TbProject WHERE Code=@Code";
                project_Id = _connection.ExecuteScalar<int>(strQuery, new
                {
                    Code = projectCode
                });
            }
            return project_Id;
        }
        #endregion GetProjectIdByProjectCode
    }
}
