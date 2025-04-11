using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using SunidhiV2_0.Code.Utils;
using SunidhiV2_0.Code.Common;
using Dapper;
using Newtonsoft.Json;
using static SunidhiV2_0.Models.SurveyResponse;
using Ubiety.Dns.Core;

namespace SunidhiV2_0.Code.DAL
{
    public class SurveyResponseImportDAL
    {
        protected IDbConnection _connection;
        public List<SurveyQuestion> GetQuestionDetails(int SurveyID)
        {
            string strQuery = string.Empty;
            List<SurveyQuestion> lstSurveyQuestion = new List<SurveyQuestion>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select SQ.SurveyID,SQ.SectionID,SQ.QuestionID,SQ.QuestionName,SQ.QuestionTypeID,Q.Type QuestionType,Q.Category QuestionCategory,
                        SQ.QuestionOrder,SQ.QuestionHint,SQ.DefaultValue,SQ.IsUnique,SQ.IsSearchable,SQ.IsMandatory,SQ.FormulaOperation,SQ.FormulaExpression,
                        SQ.IsFormulaQuestion,SQ.FormulaFieldVariables,SQ.RegEx,SQ.ErrorText,SQ.FieldVar,SQ.IsConditionQuestion,IFNULL(SQ.IsOtherOptionSelected,0) IsOtherOptionSelected From TbProjectSurveyQuestion SQ Inner Join TbQuestionType Q On Q.ID =SQ.QuestionTypeID Where SQ.SurveyID= @SurveyID
                       and SQ.QuestionTypeID not in (8,10,11,12,14,15) and SQ.IsActive=1 and SectionID not in(SELECT SectionID FROM
                      TbProjectSurveySectionLogic where SurveyID= @SurveyID ) order by SectionID,QuestionOrder";
                    lstSurveyQuestion = _connection.Query<SurveyQuestion>(strQuery, new { SurveyID = SurveyID }).ToList();

                    foreach (SurveyQuestion qstnItem in lstSurveyQuestion)
                    {

                        qstnItem.lstQuestionOptions = new List<QuestionOptionList>();
                        strQuery = "SELECT opt.OptionID,opt.OptionValue FROM TbProjectSurveyQSTOption opt JOIN TbProjectSurveyQuestion qst ON opt.QuestionID =  qst.QuestionID ";
                        strQuery += "WHERE qst.QuestionID = @QuestionID AND opt.OptionValue != CASE WHEN ifnull(qst.IsOtherOptionSelected,0) = 1 THEN 'Others' ELSE '' END; ";
                        qstnItem.lstQuestionOptions = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        qstnItem.lstQuestionCondition = new List<QuestionCondition>();
                        strQuery = @"Select ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator From TbProjectSurveyQSTCondition Where QuestionID = @QuestionID";
                        qstnItem.lstQuestionCondition = _connection.Query<QuestionCondition>(strQuery, new { QuestionID = qstnItem.QuestionID }).ToList();

                        //START Survey Section Logic
                        qstnItem.lstSurveySectionLogic = new List<SurveySectionLogic>();
                        string strSurveySkipLogic = "SELECT SectionLogicID,SL.SurveyID,SL.SectionID,LogicType,SQ.QuestionID,ConditionID,Answer,LogicOperator,SQ.QuestionName,Q.Type QuestionType,ConditionValue as 'Condition' FROM TbProjectSurveySectionLogic SL Inner Join TbProjectSurveyQuestion SQ On SL.SectionID =SQ.SectionID Inner Join TbQuestionType Q On Q.ID =SQ.QuestionTypeID left join TbProjectLogicConditions LC on SL.ConditionID=LC.ID WHERE IFNULL(SL.IsActive,0)=1 and SL.QuestionID = @QuestionID";
                        qstnItem.lstSurveySectionLogic = _connection.Query<SurveySectionLogic>(strSurveySkipLogic, new { QuestionID = qstnItem.QuestionID }).ToList();
                        foreach (SurveySectionLogic SectionLogicItem in qstnItem.lstSurveySectionLogic)
                        {
                            SectionLogicItem.lstQuestionOptions = new List<QuestionOptionList>();
                            strQuery = "SELECT opt.OptionID,opt.OptionValue FROM TbProjectSurveyQSTOption opt JOIN TbProjectSurveyQuestion qst ON opt.QuestionID =  qst.QuestionID ";
                            strQuery += "WHERE qst.QuestionID = @QuestionID AND opt.OptionValue != CASE WHEN ifnull(qst.IsOtherOptionSelected,0) = 1 THEN 'Others' ELSE '' END; ";
                            SectionLogicItem.lstQuestionOptions = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = SectionLogicItem.QuestionID }).ToList();

                            SectionLogicItem.lstQuestionCondition = new List<QuestionCondition>();
                            strQuery = @"Select ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator From TbProjectSurveyQSTCondition Where QuestionID = @QuestionID";
                            SectionLogicItem.lstQuestionCondition = _connection.Query<QuestionCondition>(strQuery, new { QuestionID = SectionLogicItem.QuestionID }).ToList();
                        }

                            //ENDS Survey Section Logic
                        }
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQuestion;
        }
        public List<SurveyQuestion> GetQuestionDetailsforImport(int SurveyID)
        {
            string strQuery = string.Empty;
            List<SurveyQuestion> lstSurveyQuestion = new List<SurveyQuestion>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select SQ.SurveyID,SQ.SectionID,SQ.QuestionID,SQ.QuestionName,SQ.QuestionTypeID,Q.Type QuestionType,Q.Category QuestionCategory,
                        SQ.QuestionOrder,SQ.QuestionHint,SQ.DefaultValue,SQ.IsUnique,SQ.IsSearchable,SQ.IsMandatory,SQ.FormulaOperation,SQ.FormulaExpression,
                        SQ.IsFormulaQuestion,SQ.FormulaFieldVariables,SQ.RegEx,SQ.ErrorText,SQ.FieldVar,SQ.IsConditionQuestion,IFNULL(SQ.IsOtherOptionSelected,0) IsOtherOptionSelected From TbProjectSurveyQuestion SQ Inner Join TbQuestionType Q On Q.ID =SQ.QuestionTypeID Where SQ.SurveyID= @SurveyID
                       and SQ.QuestionTypeID not in (8,10,11,12,14,15) and SQ.IsActive=1";
                    lstSurveyQuestion = _connection.Query<SurveyQuestion>(strQuery, new { SurveyID = SurveyID }).ToList();

                   
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyQuestion;
        }

    }
}
