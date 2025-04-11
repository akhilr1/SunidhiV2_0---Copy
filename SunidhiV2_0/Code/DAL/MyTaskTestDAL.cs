using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Web;
using SunidhiV2_0.Controllers;
using System.Globalization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SunidhiV2_0.Code.Utils;
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace SunidhiV2_0.Code.DAL
{
    public class MyTaskTestDAL
    {
        protected IDbConnection _connection;

        public ListSyncItem GetProjectAccountsByID(int userId, int SyncServerID, int workspaceId, string LastSyncDatetime)
        {
            ListSyncItem objListSyncItem = new ListSyncItem();

            List<MyTaskDM> lstMyTaskDM = new List<MyTaskDM>();
            List<MyTask> lstMyTask_Survey = new List<MyTask>();
            List<MyTask> lstMyTask1 = new List<MyTask>();
            string strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = "sp_GetMyTaskList_Survey_NEWBackUp";
                lstMyTask_Survey = _connection.Query<MyTask>(strQuery, new { loggedUserID = userId, varSyncDbID = SyncServerID, varWorkspaceID = workspaceId, rownNmber = 1, rowCount = 20 }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                objListSyncItem.lstMyTaskList = lstMyTask_Survey;

                //commented on 28 -12-2021
                //string strQuery1 = "sp_GetMyTaskList";
                // lstMyTask1 = _connection.Query<MyTask>(strQuery1, new { loggedUserID = userId, varSyncDbID = SyncServerID, varWorkspaceID = workspaceId }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();

                // objListSyncItem.lstMyTaskList.AddRange(lstMyTask1);


                if (lstMyTask_Survey != null && lstMyTask_Survey.Count() > 0)//&& SyncServerID == 0)
                {
                    // objListSyncItem.lstMyTaskDM = lstMyTaskDM.OrderByDescending(x => x.SyncDBID).ToList();

                    var mySKUs = lstMyTask_Survey.Select(l => l.TypeID).Distinct().ToList();
                    var dataCollectionSyncDBID = lstMyTask_Survey.Where(x => (x.WFLevel <= x.CurrentUserWFLevel && x.WFLevelStatus == "Pending" && x.DataCollectionSyncDBID > 0)).Select(l => l.DataCollectionSyncDBID).Distinct().ToList(); //

                    string joinedSurveyIds = string.Join(",", mySKUs);
                    string joineddataCollectionSyncDBID = string.Join(",", dataCollectionSyncDBID);

                    //START survey section
                    string strSurveySection = "SELECT SectionID,SectionName,SectionDescription,SectionOrder,SurveyID FROM TbProjectSurveySection WHERE ifnull(IsActive, 0) = 1 AND ";
                    strSurveySection += " SurveyID in(" + joinedSurveyIds + ") order by SurveyID,SectionOrder asc;";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveySection> lstMyTaskSurveySection = new List<MyTaskSurveySection>();
                        lstMyTaskSurveySection = _connection.Query<MyTaskSurveySection>(strSurveySection).ToList();

                        objListSyncItem.lstMyTaskSurveySection = lstMyTaskSurveySection;
                    }
                    //ENDS survey section

                    //START Survey Question
                    string strSurveyQuestion = "SELECT QuestionID,QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,SurveyID,SectionID,CreatedBy,UpdatedBy,CreatedDate,";
                    strSurveyQuestion += "UpdatedDate,IsConditionQuestion,IsRepeatSection,RepeatSectionId,IsUnique,IsSearchable,MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,";
                    strSurveyQuestion += "TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables,";
                    strSurveyQuestion += "HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,ifnull(IsOtherOptionSelected,0) IsOtherOptionSelected FROM TbProjectSurveyQuestion WHERE ";
                    strSurveyQuestion += " SurveyID in(" + joinedSurveyIds + ") and IFNULL(IsActive,0) = 1;";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveyQuestion> lstMyTaskSurveyQuestion = new List<MyTaskSurveyQuestion>();
                        lstMyTaskSurveyQuestion = _connection.Query<MyTaskSurveyQuestion>(strSurveyQuestion).ToList();
                        objListSyncItem.lstMyTaskSurveyQuestion = lstMyTaskSurveyQuestion;
                    }
                    //var QuestionIDList = lstMyTaskSurveyQuestion.Select(l => l.QuestionID).ToList();
                    //string joinedQuestionids = string.Join(",", QuestionIDList);
                    //ENDS Survey Question

                    //START Survey Section Logic
                    string strSurveySkipLogic = "SELECT SectionLogicID,SurveyID,SectionID,LogicType,QuestionID,ConditionID,Answer,LogicOperator FROM TbProjectSurveySectionLogic WHERE IFNULL(IsActive,0)=1";
                    strSurveySkipLogic += " and SurveyID in(" + joinedSurveyIds + ");";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveySectionLogic> lstMyTaskSurveySectionLogic = new List<MyTaskSurveySectionLogic>();
                        lstMyTaskSurveySectionLogic = _connection.Query<MyTaskSurveySectionLogic>(strSurveySkipLogic).ToList();
                        objListSyncItem.lstMyTaskSurveySectionLogic = lstMyTaskSurveySectionLogic;
                    }
                    //ENDS Survey Section Logic

                    //STARTS TRACK DATE
                    string strSurveyTrackDate = "SELECT TrackDate,SurveyID FROM TbProjectSurveyTrackDate WHERE";
                    strSurveyTrackDate += " SurveyID in(" + joinedSurveyIds + ")";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveyTrackDate> lstMyTaskSurveyTrackDate = new List<MyTaskSurveyTrackDate>();
                        lstMyTaskSurveyTrackDate = _connection.Query<MyTaskSurveyTrackDate>(strSurveyTrackDate).ToList();
                        objListSyncItem.lstMyTaskSurveyTrackDate = lstMyTaskSurveyTrackDate;
                    }
                    //ENDS TRACK DATE

                    //STARTS Survey QST Condition
                    string strSurveyQSTCondition = "SELECT qstcondition.QuestionID,qstcondition.ValidationCondition,qstcondition.ConditionValue,qstcondition.MinValue,";
                    strSurveyQSTCondition += "qstcondition.`MaxValue`,qstcondition.RegEx,qstcondition.ErrorText,qstcondition.LogicOperator,qst.SurveyID FROM  TbProjectSurveyQSTCondition qstcondition";
                    strSurveyQSTCondition += " JOIN TbProjectSurveyQuestion qst ON  qstcondition.QuestionID = qst.QuestionID  WHERE qst.SurveyID IN";
                    strSurveyQSTCondition += "(" + joinedSurveyIds + ") AND IFNULL(qst.IsActive,0) = 1;";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskProjectSurveyQSTCondition> lstMyTaskProjectSurveyQSTCondition = new List<MyTaskProjectSurveyQSTCondition>();
                        lstMyTaskProjectSurveyQSTCondition = _connection.Query<MyTaskProjectSurveyQSTCondition>(strSurveyQSTCondition).ToList();
                        objListSyncItem.lstMyTaskProjectSurveyQSTCondition = lstMyTaskProjectSurveyQSTCondition;
                    }
                    //ENDS Survey QST Condition

                    //STARTS Project Survey QST Option
                    string strSurveyQSTOption = "SELECT QSTOption.OptionID,QSTOption.OptionValue,QSTOption.QuestionID,false as IsOptSelected,QST.SurveyID  FROM TbProjectSurveyQSTOption QSTOption JOIN TbProjectSurveyQuestion QST ON QST.QuestionID = QSTOption.QuestionID WHERE";
                    strSurveyQSTOption += " QST.SurveyID IN (" + joinedSurveyIds + ")";

                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskProjectSurveyQSTOption> lstMyTaskProjectSurveyQSTOption = new List<MyTaskProjectSurveyQSTOption>();
                        lstMyTaskProjectSurveyQSTOption = _connection.Query<MyTaskProjectSurveyQSTOption>(strSurveyQSTOption).ToList();
                        objListSyncItem.lstMyTaskProjectSurveyQSTOption = lstMyTaskProjectSurveyQSTOption;
                    }
                    //ENDS Project Survey QST Option

                    //STARTS Survey Cascading Question Options
                    string strCascadingQuestionOptions = "SELECT OptionGroupID,OptionValue,HierarchyOrder,TableName,ColumnName,PrimaryColumn,ParentItemColumn FROM TbSurveyCascadingQuestionOptions";

                    List<MyTaskSurveyCascadingQuestionOptions> lstMyTaskSurveyCascadingQuestionOptions = new List<MyTaskSurveyCascadingQuestionOptions>();
                    lstMyTaskSurveyCascadingQuestionOptions = _connection.Query<MyTaskSurveyCascadingQuestionOptions>(strCascadingQuestionOptions).ToList();

                    objListSyncItem.lstMyTaskSurveyCascadingQuestionOptions = lstMyTaskSurveyCascadingQuestionOptions;
                    //ENDS Survey Cascading Question Options

                    //STARTS Survey Project Logic Conditions
                    string strProjectLogicConditions = "SELECT ID,`Condition`,ConditionValue FROM TbProjectLogicConditions";
                    List<TbMyTaskProjectLogicConditions> lstTbMyTaskProjectLogicConditions = new List<TbMyTaskProjectLogicConditions>();
                    lstTbMyTaskProjectLogicConditions = _connection.Query<TbMyTaskProjectLogicConditions>(strProjectLogicConditions).ToList();
                    objListSyncItem.lstTbMyTaskProjectLogicConditions = lstTbMyTaskProjectLogicConditions;
                    //ENDS Survey Project Logic Conditions

                    //STARTS  TbMyTask
                    //string strMyTask = "SELECT SyncTaskID,UserID,`Type`,TypeID,WFLevel,WFLevelStatus,TaskDate,TaskSubmittedDate,TaskUpdateComment,ParentSyncDBID,DataCollectionSyncDBID FROM TbMyTask WHERE Type = 'Survey' and ";
                    //strMyTask += " TypeID in(" + joinedSurveyIds + ")";

                    //List<TbMyTaskTable> lstMyTask = new List<TbMyTaskTable>();
                    //lstMyTask = _connection.Query<TbMyTaskTable>(strMyTask).ToList();

                    //objListSyncItem.lstTbMyTaskTable = lstMyTask;
                    //ENDS TbMyTask

                    //beneficiary

                    //STARTS TbProject BeneficiaryType
                    string strBeneficiaryType = "SELECT  BType.BeneficiaryTypeID,BType.`Name`,BType.Description,BType.ProjectID,SRV.SurveyID FROM TbProjectBeneficiaryType BType  ";
                    strBeneficiaryType += " JOIN TbProjectSurvey SRV ON SRV.BeneficiaryTypeID = BType.BeneficiaryTypeID";
                    strBeneficiaryType += " JOIN TbProject prj ON prj.ProjectId = BType.ProjectID";
                    strBeneficiaryType += " WHERE  IFNULL(BType.IsActive,0) =1  and IFNULL(BType.IsDeleted,0) = 0 AND prj.WSID = @WSID";
                    //and IFNULL(SRV.IsActive,0) =1 and IFNULL(prj.IsActive,0) =1 
                    List<TbMyTaskProjectBeneficiaryType> lstTbMyTaskProjectBeneficiaryType = new List<TbMyTaskProjectBeneficiaryType>();
                    lstTbMyTaskProjectBeneficiaryType = _connection.Query<TbMyTaskProjectBeneficiaryType>(strBeneficiaryType, new { WSID = workspaceId }).ToList();

                    objListSyncItem.lstTbMyTaskProjectBeneficiaryType = lstTbMyTaskProjectBeneficiaryType;
                    //ENDS TbProject BeneficiaryType

                    var MyTypeIds = lstTbMyTaskProjectBeneficiaryType.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                    string joinedBeneficiaryTypeID = string.Join(",", MyTypeIds);

                    //STARTS  Beneficiary Type Question
                    string strBeneficiaryTypeQuestion = "SELECT QuestionID,QuestionName,AnswerTypeID,BeneficiaryTypeID,IsUnique,IsSearchable,IsMandatoryResponse,QuestionHint,IsValidationQuestion,RegexExpression,ifnull(QuestionOrder,0) QuestionOrder FROM TbBeneficiaryTypeQuestion WHERE IFNULL(IsActive,0)=1 AND IFNULL(IsDeleted,0)=0";
                    strBeneficiaryTypeQuestion += " and BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ")";

                    if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                    {
                        List<MyTaskBeneficiaryTypeQuestion> lstBeneficiaryTypeQuestion = new List<MyTaskBeneficiaryTypeQuestion>();
                        lstBeneficiaryTypeQuestion = _connection.Query<MyTaskBeneficiaryTypeQuestion>(strBeneficiaryTypeQuestion).ToList();

                        objListSyncItem.lstMyTaskBeneficiaryTypeQuestion = lstBeneficiaryTypeQuestion;
                    }
                    //ENDS Beneficiary Type Question


                    //STARTS Beneficiary Survey QST Option
                    string strBeneficiaryQSTOption = "SELECT QSTOption.QstnAnsOptionID,QSTOption.BeneficiaryTypeID,QSTOption.QuestionID,QSTOption.AnswerTypeID,QSTOption.OptionValue  FROM TbBeneficiaryTypeQstnAnswerOptions QSTOption JOIN TbBeneficiaryTypeQuestion QST ON QST.QuestionID = QSTOption.QuestionID WHERE";
                    strBeneficiaryQSTOption += " QST.BeneficiaryTypeID IN (" + joinedBeneficiaryTypeID + ")";

                    if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                    {
                        List<MyTaskBeneficiarySurveyQSTOption> lstMyTaskBeneficiarySurveyQSTOption = new List<MyTaskBeneficiarySurveyQSTOption>();
                        lstMyTaskBeneficiarySurveyQSTOption = _connection.Query<MyTaskBeneficiarySurveyQSTOption>(strBeneficiaryQSTOption).ToList();

                        objListSyncItem.lstMyTaskBeneficiarySurveyQSTOption = lstMyTaskBeneficiarySurveyQSTOption;
                    }
                    //ENDS Beneficiary Survey QST Option

                    //STARTS  Beneficiary TbBeneficiaryQSTCondition

                    //var MyBenQuestion = lstBeneficiaryTypeQuestion.Select(l => l.QuestionID).ToList();
                    //string joinedMyBenQuestion = string.Join(",", MyBenQuestion);

                    string strBeneficiaryQSTCondition = "SELECT qstCondition.ID,qstCondition.QuestionID,qstCondition.ValidationCondition,qstCondition.ConditionValue,qstCondition.MinValue,qstCondition.`MaxValue`,qstCondition.RegEx,qstCondition.ErrorText,qstCondition.LogicOperator,bnfQST.BeneficiaryTypeID FROM TbBeneficiaryQSTCondition";
                    strBeneficiaryQSTCondition += " qstCondition JOIN TbBeneficiaryTypeQuestion bnfQST ON qstCondition.QuestionID = bnfQST.QuestionID";
                    strBeneficiaryQSTCondition += " WHERE bnfQST.BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ")";

                    if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                    {
                        List<MyTaskProjectSurveyQSTCondition> lstBenMyTaskProjectSurveyQSTCondition = new List<MyTaskProjectSurveyQSTCondition>();
                        lstBenMyTaskProjectSurveyQSTCondition = _connection.Query<MyTaskProjectSurveyQSTCondition>(strBeneficiaryQSTCondition).ToList();
                        objListSyncItem.lstMyTaskProjectBenSurveyQSTCondition = lstBenMyTaskProjectSurveyQSTCondition;
                    }
                    //ENDS Beneficiary TbBeneficiaryQSTCondition

                    //STARTS  Beneficiary Cascading QstnOptions
                    string strProjectBeneficiary = "select BeneficiaryID,BeneficiaryName,BeneficiaryTypeID, DATE_FORMAT(CreatedDate, '%Y-%m-%d %T.%f') CreatedDate,TokenValue  FROM TbProjectBeneficiary where IFNULL(IsActive,0)=1 AND IFNULL(IsDeleted,0)=0";
                    strProjectBeneficiary += " AND BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ")";

                    if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                    {
                        List<TbMyTaskProjectBeneficiary> lstProjectBeneficiary = new List<TbMyTaskProjectBeneficiary>();
                        lstProjectBeneficiary = _connection.Query<TbMyTaskProjectBeneficiary>(strProjectBeneficiary).ToList();
                        objListSyncItem.lstTbMyTaskProjectBeneficiary = lstProjectBeneficiary;
                    }
                    //ENDS Beneficiary Cascading QstnOptions


                    //STARTS  Beneficiary Beneficiary Registration Details
                    string strBeneficiaryRegistrationDetails = "select benreg.BeneficiaryRegID,benreg.BeneficiaryID,benreg.QuestionID,benreg.AnswerTypeID,benreg.Answer,ben.BeneficiaryTypeID FROM TbBeneficiaryRegistrationDetails benreg JOIN TbProjectBeneficiary ben ON benreg.BeneficiaryID = ben.BeneficiaryID WHERE IFNULL(benreg.IsDeleted,0) = 0";
                    strBeneficiaryRegistrationDetails += " AND ben.BeneficiaryTypeID IN(" + joinedBeneficiaryTypeID + ")";

                    if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                    {
                        List<TbMyTaskBeneficiaryRegistrationDetails> lstBeneficiaryRegistrationDetails = new List<TbMyTaskBeneficiaryRegistrationDetails>();
                        lstBeneficiaryRegistrationDetails = _connection.Query<TbMyTaskBeneficiaryRegistrationDetails>(strBeneficiaryRegistrationDetails).ToList();
                        objListSyncItem.lstTbMyTaskBeneficiaryRegistrationDetails = lstBeneficiaryRegistrationDetails;
                    }
                    //ENDS Beneficiary Beneficiary Registration Details

                    //STARTS TbProject TbBeneficiaryAnswerType
                    string strBeneficiaryAnswerType = "SELECT AnswerTypeID,AnswerType FROM TbBeneficiaryAnswerType WHERE IFNULL(IsActive,0) =1 AND  IFNULL(IsDeleted,0) =0";
                    List<TbMyTaskBeneficiaryAnswerType> lstBeneficiaryAnswerType = new List<TbMyTaskBeneficiaryAnswerType>();
                    lstBeneficiaryAnswerType = _connection.Query<TbMyTaskBeneficiaryAnswerType>(strBeneficiaryAnswerType).ToList();

                    objListSyncItem.lstTbMyTaskBeneficiaryAnswerType = lstBeneficiaryAnswerType;
                    //ENDS TbProject BeneficiaryType

                    //STARTS  Beneficiary Cascading QstnOptions
                    string strTbBeneficiaryCascadingQstnOptions = "SELECT OptionGroupID,OptionValue,HierarchyOrder,TableName,ColumnName,PrimaryColumn,ParentItemColumn FROM TbBeneficiaryCascadingQstnOptions WHERE IFNULL(IsDeleted,0) = 0";
                    List<MyTaskSurveyCascadingQuestionOptions> lstBeneficiaryCascadingQstnOptions = new List<MyTaskSurveyCascadingQuestionOptions>();
                    lstBeneficiaryCascadingQstnOptions = _connection.Query<MyTaskSurveyCascadingQuestionOptions>(strTbBeneficiaryCascadingQstnOptions).ToList();

                    objListSyncItem.lstMyTaskBeneficiarySurveyCascadingQuestionOptions = lstBeneficiaryCascadingQstnOptions;
                    //ENDS Beneficiary Cascading QstnOptions

                    //STARTS  Master List
                    string strMasterQuery = "SELECT CountryID AS ID,CountryName AS Name,'Country' ItemTable,0 AS ParentID,0 AS CensusCode  FROM  TbCountry";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT StateID AS ID,StateName AS Name,'State' ItemTable,CountryID AS ParentID, Census_Code AS CensusCode FROM  TbStates ";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT DistrictID AS ID,DistrictName AS Name,'District' ItemTable, StateID AS ParentID,Census_Code AS CensusCode FROM TbDistrict ";
                    strMasterQuery += " UNION ";
                    strMasterQuery += " SELECT SubDistrictID AS ID,SubDistrictName AS Name,'SubDistrict' ItemTable,DistrictID  AS ParentID,Census_Code AS CensusCode FROM TbSubDistrict ";
                    List<MyTaskMasterList> lstMasterList = new List<MyTaskMasterList>();
                    lstMasterList = _connection.Query<MyTaskMasterList>(strMasterQuery).ToList();

                    objListSyncItem.lstMyTaskMasterList = lstMasterList;
                    //ENDS  Master List

                    //STARTS  My task Question Type
                    string strQuestionType = "select ID,`Type`,Category from TbQuestionType";
                    List<MyTaskQuestionType> lstQuestionType = new List<MyTaskQuestionType>();
                    lstQuestionType = _connection.Query<MyTaskQuestionType>(strQuestionType).ToList();

                    objListSyncItem.lstMyTaskQuestionType = lstQuestionType;
                    //ENDS  Master List

                    //STARTS  My task SURVEY RESPONSES
                    string strSurveyResponse = "SELECT ID,ResponseID,Answer,QuestionID,SurveyID,SectionID,OptionID,RepeatOrder,SubRepeatOrder,SyncTaskID FROM TbProjectSurveyResponses ";
                    strSurveyResponse += " WHERE (IFNULL(IsActive,0)=1 or IFNULL(IsActive,0)=0) and  SurveyID in(" + joinedSurveyIds + ")";

                    if (!string.IsNullOrEmpty(joineddataCollectionSyncDBID))
                    {
                        strSurveyResponse += " AND SyncTaskID IN (" + joineddataCollectionSyncDBID + ")";
                    }
                    //AND ResponseID = 0
                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveyResponse> lstSurveyResponse = new List<MyTaskSurveyResponse>();
                        lstSurveyResponse = _connection.Query<MyTaskSurveyResponse>(strSurveyResponse).ToList();
                        objListSyncItem.lstMyTaskSurveyResponse = lstSurveyResponse;
                    }
                    //ENDS  My task SURVEY RESPONSES
                }
                else
                {
                    var mySKUs = lstMyTask_Survey.Select(l => l.SyncDBID).Distinct().ToList();
                    string joinedSyncDBIds = string.Join(",", mySKUs);

                    if (joinedSyncDBIds != null && joinedSyncDBIds.Length > 0)
                    {
                        //STARTS  My task SURVEY RESPONSES
                        string strSurveyResponse = "SELECT ID,ResponseID,Answer,QuestionID,SurveyID,SectionID,OptionID,RepeatOrder,SubRepeatOrder,SyncTaskID FROM TbProjectSurveyResponses ";
                        strSurveyResponse += " WHERE (IFNULL(IsActive,0)=1 or IFNULL(IsActive,0)=0) and  SyncTaskID in(" + joinedSyncDBIds + ")";
                        //AND ResponseID = 0

                        List<MyTaskSurveyResponse> lstSurveyResponse = new List<MyTaskSurveyResponse>();
                        lstSurveyResponse = _connection.Query<MyTaskSurveyResponse>(strSurveyResponse).ToList();
                        objListSyncItem.lstMyTaskSurveyResponse = lstSurveyResponse;

                        //STARTS TbProject BeneficiaryType
                        string strBeneficiaryType = "SELECT  BType.BeneficiaryTypeID,BType.`Name`,BType.Description,BType.ProjectID,SRV.SurveyID FROM TbProjectBeneficiaryType BType  ";
                        strBeneficiaryType += " JOIN TbProjectSurvey SRV ON SRV.BeneficiaryTypeID = BType.BeneficiaryTypeID";
                        strBeneficiaryType += " JOIN TbProject prj ON prj.ProjectId = BType.ProjectID";
                        strBeneficiaryType += " WHERE  IFNULL(BType.IsActive,0) =1  and IFNULL(BType.IsDeleted,0) = 0 AND prj.WSID = @WSID";
                        //and IFNULL(SRV.IsActive,0) =1 and IFNULL(prj.IsActive,0) =1 
                        List<TbMyTaskProjectBeneficiaryType> lstTbMyTaskProjectBeneficiaryType = new List<TbMyTaskProjectBeneficiaryType>();
                        lstTbMyTaskProjectBeneficiaryType = _connection.Query<TbMyTaskProjectBeneficiaryType>(strBeneficiaryType, new { WSID = workspaceId }).ToList();

                        if (lstTbMyTaskProjectBeneficiaryType != null && lstTbMyTaskProjectBeneficiaryType.Count() > 0)
                        {
                            var MyTypeIds = lstTbMyTaskProjectBeneficiaryType.Select(l => l.BeneficiaryTypeID).Distinct().ToList();
                            string joinedBeneficiaryTypeID = string.Join(",", MyTypeIds);

                            if (joinedBeneficiaryTypeID != null && joinedBeneficiaryTypeID.Length > 0)
                            {
                                //STARTS  Beneficiary 
                                string strProjectBeneficiary = "select BeneficiaryID,BeneficiaryName,BeneficiaryTypeID, DATE_FORMAT(CreatedDate, '%Y-%m-%d %T.%f') CreatedDate,TokenValue  FROM TbProjectBeneficiary where IFNULL(IsActive,0)=1 AND IFNULL(IsDeleted,0)=0";
                                strProjectBeneficiary += " AND BeneficiaryTypeID in(" + joinedBeneficiaryTypeID + ") AND CASE  LENGTH(@LastSyncDate)  WHEN 0 then 1=1 ELSE  CreatedDate > STR_TO_DATE(@LastSyncDate,'%Y-%m-%d %H:%i:%s') END;";

                                List<TbMyTaskProjectBeneficiary> lstProjectBeneficiary = new List<TbMyTaskProjectBeneficiary>();
                                lstProjectBeneficiary = _connection.Query<TbMyTaskProjectBeneficiary>(strProjectBeneficiary, new { LastSyncDate = LastSyncDatetime }).ToList();
                                objListSyncItem.lstTbMyTaskProjectBeneficiary = lstProjectBeneficiary;

                                if (lstProjectBeneficiary != null && lstProjectBeneficiary.Count() > 0)
                                {
                                    var BeneficiaryIds = lstProjectBeneficiary.Select(l => l.BeneficiaryID).Distinct().ToList();
                                    string joinedBeneficiaryID = string.Join(",", BeneficiaryIds);

                                    if (joinedBeneficiaryID != null && joinedBeneficiaryID.Length > 0)
                                    {
                                        //STARTS  Beneficiary Beneficiary Registration Details
                                        string strBeneficiaryRegistrationDetails = "select benreg.BeneficiaryRegID,benreg.BeneficiaryID,benreg.QuestionID,benreg.AnswerTypeID,benreg.Answer,ben.BeneficiaryTypeID FROM TbBeneficiaryRegistrationDetails benreg JOIN TbProjectBeneficiary ben ON benreg.BeneficiaryID = ben.BeneficiaryID WHERE IFNULL(benreg.IsDeleted,0) = 0";
                                        strBeneficiaryRegistrationDetails += " AND ben.BeneficiaryID IN(" + joinedBeneficiaryID + ")";

                                        List<TbMyTaskBeneficiaryRegistrationDetails> lstBeneficiaryRegistrationDetails = new List<TbMyTaskBeneficiaryRegistrationDetails>();
                                        lstBeneficiaryRegistrationDetails = _connection.Query<TbMyTaskBeneficiaryRegistrationDetails>(strBeneficiaryRegistrationDetails).ToList();
                                        objListSyncItem.lstTbMyTaskBeneficiaryRegistrationDetails = lstBeneficiaryRegistrationDetails;
                                    }
                                    //ENDS Beneficiary Beneficiary Registration Details
                                }
                            }
                        }
                    }
                }
            }

            return objListSyncItem;
        }

        #region GetMyTaskActivityLog
        /// <summary>
        /// GetMyTaskActivityLog
        /// </summary>
        /// <param name="syncTaskID"></param>
        /// <returns></returns>
        public List<MyTaskActivityLog> GetMyTaskActivityLog(int syncTaskID)
        {
            List<MyTaskActivityLog> lstActivityLog = new List<MyTaskActivityLog>();
            MyTaskActivityLog objActivityLog = new MyTaskActivityLog();

            int? count = null;
            using (_connection = Utils.Database.GetDBConnection())
            {
                string strMyTask = "select count(ID) FROM TbMyTask WHERE SyncTaskID = @SyncTaskID";
                count = _connection.Query<int>(strMyTask, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                if (count > 0)
                {
                    MyTaskResponseLog MyTaskResponseLog1 = new MyTaskResponseLog();
                    MyTaskResponseLog1.ParentSyncDBID = 0;

                    while (MyTaskResponseLog1.ParentSyncDBID >= 0)
                    {
                        MyTaskActivityLog objActivityLog1 = new MyTaskActivityLog();

                        if (MyTaskResponseLog1.ParentSyncDBID > 0)
                        {
                            syncTaskID = MyTaskResponseLog1.ParentSyncDBID;
                        }

                        string strMyTaskResponse = "SELECT SYNCDB.CreatedBy,SYNCDB.CreatedDate,TSK.ParentSyncDBID,SYNCDB.TotalWFLevel,TSK.WFLevel,SYNCDB.Command,SYNCDB.Status";
                        strMyTaskResponse += ",SYNCDB.WorkflowID,USR.Name SubmittedBy,TSK.TaskUpdateComment,IFNULL(USR.ProfileImage,'') ProfileImage FROM  TbMyTask TSK JOIN TaskSyncDB SYNCDB ON TSK.SyncTaskID = SYNCDB.ID";
                        strMyTaskResponse += " JOIN TbUser USR ON USR.ID = SYNCDB.CreatedBy  WHERE SyncTaskID=  @SyncTaskID";
                        MyTaskResponseLog1 = _connection.Query<MyTaskResponseLog>(strMyTaskResponse, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                        if (MyTaskResponseLog1 == null)
                        {
                            MyTaskResponseLog1 = new MyTaskResponseLog();
                            MyTaskResponseLog1.ParentSyncDBID = -1;

                            //strMyTaskResponse = "SELECT SYNCDB.CreatedBy,SYNCDB.CreatedDate,0 as ParentSyncDBID,SYNCDB.TotalWFLevel,0 as WFLevel,SYNCDB.Command,SYNCDB.Status";
                            //strMyTaskResponse += ",SYNCDB.WorkflowID,USR.Name SubmittedBy FROM  TaskSyncDB SYNCDB ";
                            //strMyTaskResponse += " JOIN TbUser USR ON USR.ID = SYNCDB.CreatedBy  WHERE SYNCDB.ID=  @SyncTaskID";
                            //MyTaskResponseLog1 = _connection.Query<MyTaskResponseLog>(strMyTaskResponse, new { SyncTaskID = syncTaskID }).FirstOrDefault();
                            MyTaskActivityLog objActivityLog2 = new MyTaskActivityLog();

                            objActivityLog1.WorkflowLevelName = "Submission";
                            objActivityLog1.WorkflowStageStatus = "Assigned";
                            objActivityLog1.DefaultText = "Survey assigned to ";
                            objActivityLog1.Comments = "";
                            //objActivityLog1.SubmittedBy = "Sent by ";


                            string strMyTaskAdd = "SELECT GROUP_CONCAT(USR.Name) DefaultText,SYNCDB.CreatedDate SubmissionDate,US.SubmittedBy,US.ProfileImage ";
                            strMyTaskAdd += " FROM TaskSyncDB SYNCDB JOIN TbProjectSurveyUser SRVUSER ON SYNCDB.TypeID = SRVUSER.SurveyID  JOIN TbUser USR ON USR.ID = SRVUSER.UserID";
                            strMyTaskAdd += " JOIN (SELECT ID AS userId, `Name` SubmittedBy,ifnull(ProfileImage,'') ProfileImage FROM TbUser WHERE IFNULL(IsActive,0) = 1 AND IFNULL(IsDeleted,0) = 0) US ON US.userId = SYNCDB.CreatedBy";
                            strMyTaskAdd += " WHERE SYNCDB.ID = @SyncTaskID  AND IFNULL(SRVUSER.IsDeleted,0) = 0  AND IFNULL(SRVUSER.IsDeleted,0) =0";
                            strMyTaskAdd += " AND SYNCDB.Type = 'Survey' GROUP BY  SRVUSER.SurveyID ";

                            objActivityLog2 = _connection.Query<MyTaskActivityLog>(strMyTaskAdd, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                            objActivityLog1.SubmittedBy = objActivityLog2.SubmittedBy;
                            objActivityLog1.SubmissionDate = objActivityLog2.SubmissionDate;
                            objActivityLog1.DefaultText += objActivityLog2.DefaultText;
                            objActivityLog1.ProfileImage = objActivityLog2.ProfileImage;
                        }
                        else
                        {
                            objActivityLog1.WorkflowLevelName = (MyTaskResponseLog1.WFLevel > 0 ? "Level " + MyTaskResponseLog1.WFLevel : "Submission");

                            switch (MyTaskResponseLog1.Status)
                            {
                                case "DataApproved":
                                    objActivityLog1.WorkflowStageStatus = "Approved";
                                    break;
                                case "DataCollected":
                                    objActivityLog1.WorkflowStageStatus = "Submitted";
                                    break;
                                case "Rejected":
                                    objActivityLog1.WorkflowStageStatus = "Rejected";
                                    break;
                                default:
                                    objActivityLog1.WorkflowStageStatus = "Assigned";
                                    break;
                            }

                            objActivityLog1.SubmissionDate = MyTaskResponseLog1.CreatedDate;
                            objActivityLog1.SubmittedBy = MyTaskResponseLog1.SubmittedBy;
                            objActivityLog1.Comments = MyTaskResponseLog1.TaskUpdateComment;
                            objActivityLog1.ProfileImage = MyTaskResponseLog1.ProfileImage;

                            if (MyTaskResponseLog1.WFLevel >= MyTaskResponseLog1.TotalWFLevel && objActivityLog1.WorkflowStageStatus != "Rejected")
                            {
                                objActivityLog1.DefaultText = "Survey has been completed";
                            }
                            else
                            {
                                if (objActivityLog1.WorkflowStageStatus == "Rejected")
                                {
                                    string strMyTaskWorkflow = "SELECT GROUP_CONCAT(USR.Name) DefaultText ";
                                    strMyTaskWorkflow += "  FROM TaskSyncDB SYNCDB JOIN TbProjectSurveyUser SRVUSER ON SYNCDB.TypeID = SRVUSER.SurveyID  JOIN TbUser USR ON USR.ID = SRVUSER.UserID";
                                    strMyTaskWorkflow += " WHERE SYNCDB.ID = @SyncTaskID  AND IFNULL(SRVUSER.IsDeleted,0) = 0  AND IFNULL(SRVUSER.IsDeleted,0) =0";
                                    strMyTaskWorkflow += " AND SYNCDB.Type = 'Survey' GROUP BY  SRVUSER.SurveyID ";
                                    string workflowAssigned = _connection.Query<string>(strMyTaskWorkflow, new { SyncTaskID = syncTaskID }).FirstOrDefault();
                                    objActivityLog1.DefaultText = "Survey has been Sent to " + workflowAssigned;
                                }
                                else
                                {
                                    string strMyTaskWorkflow = "SELECT GROUP_CONCAT(USR.Name) FROM  TbProjectWorkflowDetails wkf JOIN ";
                                    strMyTaskWorkflow += "TbUser USR ON  wkf.UserID  = USR.ID WHERE wkf.WfLevel = @WfLevel  AND wkf.WorkflowID = @WorkflowID";
                                    strMyTaskWorkflow += " AND IFNULL(wkf.IsDeleted,0) = 0  group by wkf.WorkflowID";
                                    string workflowAssigned = _connection.Query<string>(strMyTaskWorkflow, new { WorkflowID = MyTaskResponseLog1.WorkflowID, WfLevel = (MyTaskResponseLog1.WFLevel + 1) }).FirstOrDefault();

                                    objActivityLog1.DefaultText = "Survey has been Sent to " + workflowAssigned;
                                }
                            }
                        }

                        lstActivityLog.Add(objActivityLog1);
                    }
                }
                else
                {
                    MyTaskActivityLog objActivityLog1 = new MyTaskActivityLog();

                    objActivityLog.WorkflowLevelName = "Submission";
                    objActivityLog.WorkflowStageStatus = "Assigned";
                    objActivityLog.DefaultText = "Survey assigned to ";
                    objActivityLog.Comments = "";
                    //objActivityLog.SubmittedBy = "Sent by ";

                    string strMyTaskAdd = "SELECT GROUP_CONCAT(USR.Name) DefaultText,SYNCDB.CreatedDate SubmissionDate,US.SubmittedBy,US.ProfileImage ";
                    strMyTaskAdd += "  FROM TaskSyncDB SYNCDB JOIN TbProjectSurveyUser SRVUSER ON SYNCDB.TypeID = SRVUSER.SurveyID  JOIN TbUser USR ON USR.ID = SRVUSER.UserID";
                    strMyTaskAdd += " JOIN (SELECT ID AS userId, `Name` SubmittedBy,ifnull(ProfileImage,'') ProfileImage FROM TbUser WHERE IFNULL(IsActive,0) = 1 AND IFNULL(IsDeleted,0) = 0) US ON US.userId = SYNCDB.CreatedBy";
                    strMyTaskAdd += " WHERE SYNCDB.ID = @SyncTaskID  AND IFNULL(SRVUSER.IsDeleted,0) = 0  AND IFNULL(SRVUSER.IsDeleted,0) =0";
                    strMyTaskAdd += " AND SYNCDB.Type = 'Survey' GROUP BY  SRVUSER.SurveyID ";

                    objActivityLog1 = _connection.Query<MyTaskActivityLog>(strMyTaskAdd, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                    if (objActivityLog1 != null)
                    {
                        objActivityLog.SubmittedBy = objActivityLog1.SubmittedBy;
                        objActivityLog.SubmissionDate = objActivityLog1.SubmissionDate;
                        objActivityLog.DefaultText += objActivityLog1.DefaultText;
                        objActivityLog.ProfileImage = objActivityLog1.ProfileImage;
                    }

                    lstActivityLog.Add(objActivityLog);
                }
            }

            //ENDS survey section

            return lstActivityLog;
        }
        #endregion GetMyTaskActivityLog

        #region GetResubmittedSurvey
        /// <summary>
        /// GetResubmittedSurvey
        /// </summary>
        /// <param name="syncTaskID"></param>
        /// <returns></returns>
        public List<SurveyDataCollection> GetResubmittedSurvey(int syncTaskID, int userId)
        {
            List<SurveyDataCollection> lstSurveyDataCollection = new List<SurveyDataCollection>();

            using (_connection = Utils.Database.GetDBConnection())
            {
                //START Survey Question
                string strSurveyQuestion = "select SS.DataCollectionSyncDBID,MSK.SyncTaskID,SS.maxsyncdb as RejectId from TbMyTask MSK JOIN ";
                strSurveyQuestion += "( SELECT MYTSK.DataCollectionSyncDBID,MAX(MYTSK.SyncTaskID) maxsyncdb  FROM TbMyTask MYTSK JOIN TbProjectSurvey srv";
                strSurveyQuestion += " ON MYTSK.TypeID = srv.SurveyID JOIN TbProjectWorkflow flw ON flw.WorkflowID = srv.WorkflowID ";
                strSurveyQuestion += " JOIN TbProjectWorkflowDetails flwde on flw.WorkflowID = flwde.WorkflowID";
                strSurveyQuestion += " where flwde.UserID = @userId AND IFNULL(MYTSK.IsActive,0) = 0 AND IFNULL(flwde.IsDeleted,0) = 0 and ifnull(MYTSK.WFLevel,0) >1";
                strSurveyQuestion += " and MYTSK.SyncTaskID <= @syncServerId group by MYTSK.DataCollectionSyncDBID)  SS on MSK.ParentSyncDBID = SS.maxsyncdb";
                strSurveyQuestion += " 	where ifnull(MSK.IsActive,0) = 1 and  ifnull(MSK.WFLevel,0) = 0 AND MSK.SyncTaskID > @syncServerId;";
                lstSurveyDataCollection = _connection.Query<SurveyDataCollection>(strSurveyQuestion, new { userId = userId, syncServerId = syncTaskID }).ToList();
            }

            return lstSurveyDataCollection;
        }
        #endregion

        public ListSyncItem GetLoadmoreTasks(int userId, int workspaceId, string LastSyncDatetime, int rownNmber, int TypeID)
        {
            rownNmber = rownNmber + 1;
            ListSyncItem objListSyncItem = new ListSyncItem();

            List<MyTaskDM> lstMyTaskDM = new List<MyTaskDM>();
            List<MyTask> lstMyTask_Survey = new List<MyTask>();
            List<MyTask> lstMyTask1 = new List<MyTask>();
            string strQuery = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = "sp_GetMyTaskList_Survey_LoadMore";
                lstMyTask_Survey = _connection.Query<MyTask>(strQuery, new { loggedUserID = userId, varSyncDbID = 0, varWorkspaceID = workspaceId, intTypeID = TypeID, rownNmber = rownNmber, rowCount = 20 }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();
                objListSyncItem.lstMyTaskList = lstMyTask_Survey;

                if (lstMyTask_Survey != null && lstMyTask_Survey.Count() > 0)//&& SyncServerID == 0)
                {
                    // objListSyncItem.lstMyTaskDM = lstMyTaskDM.OrderByDescending(x => x.SyncDBID).ToList();

                    var mySKUs = lstMyTask_Survey.Select(l => l.TypeID).Distinct().ToList();
                    var dataCollectionSyncDBID = lstMyTask_Survey.Where(x => (x.WFLevel <= x.CurrentUserWFLevel && x.WFLevelStatus == "Pending" && x.DataCollectionSyncDBID > 0)).Select(l => l.DataCollectionSyncDBID).Distinct().ToList(); //

                    string joinedSurveyIds = string.Join(",", mySKUs);
                    string joineddataCollectionSyncDBID = string.Join(",", dataCollectionSyncDBID);

                    //STARTS  My task SURVEY RESPONSES
                    string strSurveyResponse = "SELECT ID,ResponseID,Answer,QuestionID,SurveyID,SectionID,OptionID,RepeatOrder,SubRepeatOrder,SyncTaskID FROM TbProjectSurveyResponses ";
                    strSurveyResponse += " WHERE (IFNULL(IsActive,0)=1 or IFNULL(IsActive,0)=0) and  SurveyID in(" + joinedSurveyIds + ")";

                    if (!string.IsNullOrEmpty(joineddataCollectionSyncDBID))
                    {
                        strSurveyResponse += " AND SyncTaskID IN (" + joineddataCollectionSyncDBID + ")";
                    }
                    //AND ResponseID = 0
                    if (joinedSurveyIds != null && joinedSurveyIds.Length > 0)
                    {
                        List<MyTaskSurveyResponse> lstSurveyResponse = new List<MyTaskSurveyResponse>();
                        lstSurveyResponse = _connection.Query<MyTaskSurveyResponse>(strSurveyResponse).ToList();
                        objListSyncItem.lstMyTaskSurveyResponse = lstSurveyResponse;
                    }
                    //ENDS  My task SURVEY RESPONSES
                }

            }

            return objListSyncItem;
        }
    }
}
