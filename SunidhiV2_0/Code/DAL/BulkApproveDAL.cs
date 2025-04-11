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
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using Ubiety.Dns.Core;

namespace SunidhiV2_0.Code.DAL
{
    public class BulkApproveDAL
    {
        protected IDbConnection _connection;
        public List<BulkApprove> GetAllApprovalDataBulk(int UserID, int SyncServerID, int WorkspaceID, int TypeID)
        {
            List<BulkApprove> lstBulkApproveList = new List<BulkApprove>();
            string strQuery = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_GetAllApprovalDataBulk";
                    lstBulkApproveList = _connection.Query<BulkApprove>(strQuery, new { loggedUserID = UserID, varSyncDbID = SyncServerID, varWorkspaceID = WorkspaceID, varTypeID = TypeID }, commandType: CommandType.StoredProcedure, commandTimeout: 5000).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstBulkApproveList;

        }

        #region UpdateBulkData
        public bool UpdateBulkData(string gridData, int loggedUserId)
        {
            string strUpdateQuery = string.Empty;
            bool success = true;
            var pId1 = 0;
            List<MyTaskGridData> MyTaskGridDataObj = JsonConvert.DeserializeObject<List<MyTaskGridData>>(gridData);
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    foreach (MyTaskGridData Obj in MyTaskGridDataObj)
                    {
                        if (Obj.IsBeneficiary)
                        {
                            strUpdateQuery = @"UPDATE TbBeneficiaryRegistrationDetails SET 
                                   Answer=@Answer
                                   WHERE BeneficiaryRegID = @ID;SELECT LAST_INSERT_ID();";
                        }
                        else
                        {
                            strUpdateQuery = @"UPDATE TbProjectSurveyResponses SET 
                                   Answer=@Answer
                                   WHERE ID = @ID;SELECT LAST_INSERT_ID();";
                        }
                        pId1 = _connection.Query<int>(strUpdateQuery, new
                        {
                            Obj.ID,
                            Obj.Answer
                        }).Single();
                    }
                    if (pId1 < 0)
                    {
                        success = false;
                    }

                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        #endregion

        #region DeleteBulkData
        public bool DeleteBulkDataOld(string ids, int loggedUserId)
        {
            string strDeleteQuery = string.Empty;
            string strSelectquery = string.Empty;
            bool success = true;
            var pId1 = 0;

            // List<MyTaskGridData> MyTaskGridDataObj = JsonConvert.DeserializeObject<List<MyTaskGridData>>(ids);

            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(ids);
            List<int> IDS = jsonObject["ids"];
            try
            {
                strSelectquery = @"SELECT COUNT(ID) FROM TbProjectSurveyResponses 
                                   WHERE ID = @ID;";

                strDeleteQuery = @"DELETE FROM TbProjectSurveyResponses 
                                   WHERE ID = @ID;";
                using (_connection = Utils.Database.GetDBConnection())
                {
                    foreach (int ID in IDS)
                    {
                        pId1 = _connection.Query<int>(strSelectquery, new
                        {
                            ID
                        }).Single();
                        if (pId1 > 0)
                        {
                            _connection.Execute(strDeleteQuery, new
                            {
                                ID
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        public bool DeleteBulkData(string gridData,int loggedUserId=308)
        {
            string strQuery = string.Empty;
            bool success = true;
            List<MyTaskGridData> MyTaskGridDataObj = JsonConvert.DeserializeObject<List<MyTaskGridData>>(gridData);
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    foreach (MyTaskGridData Obj in MyTaskGridDataObj)
                    {
                        strQuery = "UPDATE TbProjectSurveyResponses SET IsActive =0 ,UpdatedDate  = now() ,  UpdatedBy = @loggedUserId Where SyncTaskID=@SyncTaskID and ResponseID=@BeneficiaryID ";
                        _connection.Execute(strQuery, new { SyncTaskID = Obj.SyncTaskID, BeneficiaryID = Obj.ResponseID, loggedUserId= loggedUserId });


                        strQuery = @"SELECT EXISTS( SELECT QuestionID  FROM TbProjectSurveyResponses WHERE SyncTaskID=@SyncTaskID and IsActive =1 )";
                        var id = _connection.Query<int>(strQuery, new { SyncTaskID = Obj.SyncTaskID }).Single();
                        if (id == 0)
                        {
                            strQuery = "UPDATE TaskSyncDB SET IsActive =0 Where ID=@SyncTaskID  ";
                            _connection.Execute(strQuery, new { SyncTaskID = Obj.SyncTaskID });

                            strQuery = "Update TbMyTask Set IsActive = 0 where SyncTaskID=@SyncTaskID  ";
                            _connection.Execute(strQuery, new { SyncTaskID = Obj.SyncTaskID });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        #endregion
        public IEnumerable<ProjectAuditLog> GetActivityLogList(int ProjectId, int ActivityId)
        {
            List<ProjectAuditLog> objActivityLogList = new List<ProjectAuditLog>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"Select U.Name, A.Message, DATE_FORMAT(A.CreatedDate, '%d/%m/%Y %h:%i:%S') as CreatedDate, 
                                 IFNULL(U.ProfileImage, '') AS ProfileImage from TbProjectActivityLog A Inner Join TbUser U  On U.ID=A.CreatedBy  
                                 Where ActivityId=@actid";

                    objActivityLogList = _connection.Query<ProjectAuditLog>(strQuery, new { projid = ProjectId, actid = ActivityId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objActivityLogList;
        }


        public ListSyncItem GetApprovalResponse(int TypeID, int syncTaskID, int workspaceId)
        {
            ListSyncItem objListSyncItem = new ListSyncItem();

            int? count = null;
            using (_connection = Utils.Database.GetDBConnection())
            {
                if (syncTaskID != 0 && TypeID != 0)//&& SyncServerID == 0)
                {

                    //START survey section
                    string strSurveySection = "SELECT SectionID,SectionName,SectionDescription,SectionOrder,SurveyID FROM TbProjectSurveySection WHERE ifnull(IsActive, 0) = 1 AND ";
                    strSurveySection += " SurveyID =@TypeID order by SurveyID,SectionOrder asc;";
                    List<MyTaskSurveySection> lstMyTaskSurveySection = new List<MyTaskSurveySection>();
                    lstMyTaskSurveySection = _connection.Query<MyTaskSurveySection>(strSurveySection, new { TypeID = TypeID }).ToList();

                    objListSyncItem.lstMyTaskSurveySection = lstMyTaskSurveySection;

                    //ENDS survey section

                    //START Survey Question
                    string strSurveyQuestion = "SELECT QuestionID,QuestionName,QuestionTypeID,QuestionOrder,UOM,DefaultValue,SurveyID,SectionID,CreatedBy,UpdatedBy,CreatedDate,";
                    strSurveyQuestion += "UpdatedDate,IsConditionQuestion,IsRepeatSection,RepeatSectionId,IsUnique,IsSearchable,MinValue,`MaxValue`,IsActive,RegEx,IsMandatory,";
                    strSurveyQuestion += "TextLength,XMLTag,ErrorText,FieldVar,IsFormulaQuestion,FormulaExpression,FormulaOperation,FormulaFieldVariables,";
                    strSurveyQuestion += "HierarchyQuestionOrder,RepeatCondition,ValidationCondition,QuestionHint,IsValidationQuestion,ifnull(IsOtherOptionSelected,0) IsOtherOptionSelected FROM TbProjectSurveyQuestion WHERE ";
                    strSurveyQuestion += " SurveyID =@TypeID and IFNULL(IsActive,0) = 1;";

                    List<MyTaskSurveyQuestion> lstMyTaskSurveyQuestion = new List<MyTaskSurveyQuestion>();
                    lstMyTaskSurveyQuestion = _connection.Query<MyTaskSurveyQuestion>(strSurveyQuestion, new { TypeID = TypeID }).ToList();
                    objListSyncItem.lstMyTaskSurveyQuestion = lstMyTaskSurveyQuestion;

                    //var QuestionIDList = lstMyTaskSurveyQuestion.Select(l => l.QuestionID).ToList();
                    //string joinedQuestionids = string.Join(",", QuestionIDList);
                    //ENDS Survey Question

                    //START Survey Section Logic
                    string strSurveySkipLogic = "SELECT SectionLogicID,SurveyID,SectionID,LogicType,QuestionID,ConditionID,Answer,LogicOperator FROM TbProjectSurveySectionLogic WHERE IFNULL(IsActive,0)=1";
                    strSurveySkipLogic += " and SurveyID =@TypeID;";
                    List<MyTaskSurveySectionLogic> lstMyTaskSurveySectionLogic = new List<MyTaskSurveySectionLogic>();
                    lstMyTaskSurveySectionLogic = _connection.Query<MyTaskSurveySectionLogic>(strSurveySkipLogic, new { TypeID = TypeID }).ToList();
                    objListSyncItem.lstMyTaskSurveySectionLogic = lstMyTaskSurveySectionLogic;

                    //ENDS Survey Section Logic

                    //STARTS TRACK DATE
                    string strSurveyTrackDate = "SELECT TrackDate,SurveyID FROM TbProjectSurveyTrackDate WHERE SurveyID =@TypeID";
                    List<MyTaskSurveyTrackDate> lstMyTaskSurveyTrackDate = new List<MyTaskSurveyTrackDate>();
                    lstMyTaskSurveyTrackDate = _connection.Query<MyTaskSurveyTrackDate>(strSurveyTrackDate, new { TypeID = TypeID }).ToList();
                    objListSyncItem.lstMyTaskSurveyTrackDate = lstMyTaskSurveyTrackDate;
                }
                //ENDS TRACK DATE

                //STARTS Survey QST Condition
                string strSurveyQSTCondition = "SELECT qstcondition.QuestionID,qstcondition.ValidationCondition,qstcondition.ConditionValue,qstcondition.MinValue,";
                strSurveyQSTCondition += "qstcondition.`MaxValue`,qstcondition.RegEx,qstcondition.ErrorText,qstcondition.LogicOperator,qst.SurveyID FROM  TbProjectSurveyQSTCondition qstcondition";
                strSurveyQSTCondition += " JOIN TbProjectSurveyQuestion qst ON  qstcondition.QuestionID = qst.QuestionID  WHERE qst.SurveyID =@TypeID AND IFNULL(qst.IsActive,0) = 1;";

                List<MyTaskProjectSurveyQSTCondition> lstMyTaskProjectSurveyQSTCondition = new List<MyTaskProjectSurveyQSTCondition>();
                lstMyTaskProjectSurveyQSTCondition = _connection.Query<MyTaskProjectSurveyQSTCondition>(strSurveyQSTCondition, new { TypeID = TypeID }).ToList();
                objListSyncItem.lstMyTaskProjectSurveyQSTCondition = lstMyTaskProjectSurveyQSTCondition;

                //ENDS Survey QST Condition

                //STARTS Project Survey QST Option
                string strSurveyQSTOption = "SELECT QSTOption.OptionID,QSTOption.OptionValue,QSTOption.QuestionID,false as IsOptSelected,QST.SurveyID  FROM TbProjectSurveyQSTOption QSTOption JOIN TbProjectSurveyQuestion QST ON QST.QuestionID = QSTOption.QuestionID WHERE QST.SurveyID=@TypeID ";

                List<MyTaskProjectSurveyQSTOption> lstMyTaskProjectSurveyQSTOption = new List<MyTaskProjectSurveyQSTOption>();
                lstMyTaskProjectSurveyQSTOption = _connection.Query<MyTaskProjectSurveyQSTOption>(strSurveyQSTOption, new { TypeID = TypeID }).ToList();
                objListSyncItem.lstMyTaskProjectSurveyQSTOption = lstMyTaskProjectSurveyQSTOption;

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
                strSurveyResponse += " WHERE IsActive=1 and  SurveyID =@TypeID ";

                if (syncTaskID != 0)
                {
                    strSurveyResponse += " AND SyncTaskID =@syncTaskID";
                }

                List<MyTaskSurveyResponse> lstSurveyResponse = new List<MyTaskSurveyResponse>();
                lstSurveyResponse = _connection.Query<MyTaskSurveyResponse>(strSurveyResponse, new { TypeID = TypeID, SyncTaskID = syncTaskID }).ToList();
                objListSyncItem.lstMyTaskSurveyResponse = lstSurveyResponse;

                //ENDS  My task SURVEY RESPONSES
            }

            return objListSyncItem;
        }

        public int BulkApproveAndReject(int WFLevel, bool IsBeneficiaryLinked, string objSynData, string Periodicity, int loggedUserId, string clientGuid)
        {
            string strQuery = string.Empty;
            //var id = 1;
            var IsSubmitted = 0;
            var syncDBId = 0;
            int varSurveyId = 0;
            string varWorkflowLevel = "";

            List<EmailSurveyList> lstEmailSurveyList = new List<EmailSurveyList>();

            using (_connection = Utils.Database.GetDBConnection())
            {


                var datacollSyncDBID = 0;
                var ID = 0;
                try
                {
                    List<MyTask> lstTaskData = JsonConvert.DeserializeObject<List<MyTask>>(objSynData);
                    if (lstTaskData != null)
                    {
                        for (var i = 0; i < lstTaskData.Count; i++)
                        {
                            var Item = lstTaskData[i];


                            if (WFLevel == 0)
                            {
                                datacollSyncDBID = 0;
                            }
                            else
                            {
                                datacollSyncDBID = GetDataCollectedSyncDBID(Item.SyncDBID);
                            }


                            strQuery = @"SELECT EXISTS(SELECT T.ID from TaskSyncDB T Inner Join TbMyTask MT On T.ID = MT.SyncTaskID WHERE T.IsActive =1 And IsDeleted=0 And " +
                                        " MT.IsActive =1 And Status = @TaskStatus And T.DataCollectedBy = @DataCollector And MT.Type = @TaskType And MT.TypeID = @TypeId  " +
                                        " And MT.TaskDate = @TaskDate And UserID =@UserID And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed');";
                            var IsExists = _connection.Query<int>(strQuery, new
                            {
                                CurrWFLevel = WFLevel,
                                DataCollector = Item.DataCollectedBy == -1 ? loggedUserId : Item.DataCollectedBy,
                                TaskStatus = Item.Status,
                                TaskType = Item.Type,
                                TypeId = Item.TypeID,
                                TaskDate = Item.TaskDate,
                                UserID = loggedUserId,
                            }).Single();
                            if (IsExists == 0 || (IsExists == 1 && Periodicity == "Anytime"))
                            {


                                if (WFLevel > 0)
                                {
                                    if (Periodicity == "Anytime")
                                    {
                                        strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive =1 And IsDeleted=0 And
                                                         T.IsActive =1 And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate
                                                         And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectWorkflowDetails Where IsDeleted =0 And WorkflowID = @WFID)
                                                         AND MT.ParentSyncDBID = @ParentSyncDBID );";
                                    }
                                    else
                                    {
                                        strQuery = @"SELECT EXISTS(SELECT MT.ID from TbMyTask MT Inner Join TaskSyncDB T On T.ID = MT.SyncTaskID WHERE MT.IsActive =1 And IsDeleted=0 And
                                                         T.IsActive =1 And WFLevel=@CurrWFLevel  And  WFLevelStatus ='Completed' And T.DataCollectedBy = @DataCollector And MT.TaskDate = @TaskDate
                                                         And MT.Type = @TaskType and MT.TypeID = @TypeId And UserID = ANY  (SELECT UserID From TbProjectWorkflowDetails Where IsDeleted =0 And WorkflowID = @WFID) );";
                                    }

                                    IsSubmitted = _connection.Query<int>(strQuery, new
                                    {
                                        CurrWFLevel = Item.WFLevel,
                                        DataCollector = Item.DataCollectedBy,
                                        WFID = Item.WorkflowID,
                                        TaskType = Item.Type,
                                        TypeId = Item.TypeID,
                                        TaskDate = Item.TaskDate,
                                        ParentSyncDBID = Item.SyncDBID
                                    }).Single();
                                }


                                if (IsSubmitted == 0 || (IsSubmitted == 1 && WFLevel == 0))
                                {

                                    strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,Status, WorkflowID,TotalWFLevel, DataCollectedBy,ClientData,CreatedDate, CreatedBy ,IsActive,SubmittedDataStatus,WorkspaceID,ClientGuid)
                                                    VALUES(@Type,@Command, @TypeIDColumn, @TypeID, @Status, @WorkflowID, @TotalWFLevel, @DataCollectedBy,@ClientData,now(), @loggedUserId,@IsActive,@SubmittedDataStatus,@WorkspaceID,@ClientGuid); SELECT LAST_INSERT_ID();";


                                    syncDBId = _connection.Query<int>(strQuery, new
                                    {
                                        Type = Item.Type,
                                        Command = "TaskResponse",
                                        TypeIDColumn = Item.TypeIDColumn,
                                        TypeID = Item.TypeID,
                                        Status = Item.Status,
                                        WorkflowID = Item.WorkflowID,
                                        TotalWFLevel = Item.TotalWFLevel,
                                        DataCollectedBy = Item.DataCollectedBy,
                                        ClientData = Item.ClientData,
                                        loggedUserId = loggedUserId,
                                        IsActive = 1,
                                        SubmittedDataStatus = "Submitted",
                                        WorkspaceID = Item.WorkspaceID,
                                        ClientGuid = clientGuid
                                    }).Single();

                                    strQuery = @"INSERT INTO TbMyTask(SyncTaskID,UserID,Type, TypeID, WFLevel, WFLevelStatus,TaskDate,TaskSubmittedDate, TaskUpdateComment, AttachedDocumentName, IsActive, IsDeleted, CreatedDate, CreatedBy,ParentSyncDBID,DataCollectionSyncDBID)
                                             VALUES(@syncDBId,@loggedUserId,@Type,@TypeID,@WFLevel,@WFLevelStatus,@TaskDate,@TaskSubmittedDate,@TaskUpdateComment,@AttachedDocumentName,@IsActive,0,now(),@loggedUserId,@parentSyncDBID,@datacollSyncDBID); SELECT LAST_INSERT_ID();";
                                    var taskId = _connection.Query<int>(strQuery, new
                                    {
                                        syncDBId = syncDBId,
                                        loggedUserId = loggedUserId,
                                        Type = Item.Type,
                                        TypeID = Item.TypeID,
                                        WFLevel = Item.WFLevel,
                                        WFLevelStatus = Item.WFLevelStatus,
                                        TaskDate = Item.TaskDate,
                                        TaskSubmittedDate = Item.TaskSubmittedDate,
                                        TaskUpdateComment = Item.TaskUpdateComment,
                                        AttachedDocumentName = Item.AttachedDocumentName,
                                        IsActive = 1,
                                        parentSyncDBID = Item.WorkspaceID,
                                        datacollSyncDBID = datacollSyncDBID
                                    }).Single();

                                    if (Item.WFLevelStatus == "Rejected")
                                    {


                                        strQuery = "UPDATE TbProjectSurveyResponses SET IsActive =0 ,UpdatedDate  = now() ,  UpdatedBy = @loggedUserId Where SyncTaskID =@ResponseSyncID";
                                        _connection.Execute(strQuery, new { loggedUserId = loggedUserId, ResponseSyncID = Item.ResponseSyncID });

                                        // Changing Status in TbMyTask
                                        List<int> lstSyncTaskID = new List<int>();
                                        strQuery = "Select SyncTaskID from TbMyTask where DataCollectionSyncDBID = @SyncDBID";
                                        lstSyncTaskID = _connection.Query<int>(strQuery, new
                                        {
                                            SyncDBID = datacollSyncDBID
                                        }).ToList();
                                        lstSyncTaskID.Add(Convert.ToInt32(datacollSyncDBID));

                                        for (var Count = 0; Count <= lstSyncTaskID.Count - 1; Count++)
                                        {
                                            ID = lstSyncTaskID[Count];
                                            strQuery = @"Update TbMyTask Set IsActive = 0 where SyncTaskID = @ID";
                                            _connection.Execute(strQuery, new { ID = ID });

                                            strQuery = @"UPDATE TaskSyncDB SET IsActive =0 Where ID = @ID";
                                            _connection.Execute(strQuery, new { ID = ID });
                                        }
                                    }

                                    SendTaskNotification(loggedUserId, Item.WorkflowID, WFLevel, Item.Type, Item.TypeID, Item.WFLevelStatus, Item.TypeIDColumn, 0);

                                    var emailTemplateName = string.Empty;
                                    var userRole = string.Empty;

                                    if (Item.Status != null)
                                    {
                                        if (Item.Status.ToLower() == "datacollected")
                                        {
                                            emailTemplateName = "TaskSubmission";
                                            //userRole = "Data Collector";
                                        }
                                        else if (Item.Status.ToLower() == "dataapproved")
                                        {
                                            emailTemplateName = "TaskApproved";
                                            //userRole = "Data Approver";
                                        }
                                        else if (Item.Status.ToLower() == "rejected")
                                        {
                                            emailTemplateName = "TaskRejection";
                                            //userRole = "Data Approver";
                                        }
                                    }

                                    EmailSurveyList objEmailSurveyList = new EmailSurveyList();
                                    objEmailSurveyList.SurveyID = Item.TypeID;
                                    objEmailSurveyList.EmailTemplateName = emailTemplateName;
                                    lstEmailSurveyList.Add(objEmailSurveyList);


                                }

                            }


                        }
                    }


                }
                catch (Exception e)
                {
                    lstEmailSurveyList = null;



                }


                if (lstEmailSurveyList != null && lstEmailSurveyList.Count > 0)
                {
                    foreach (var itm in lstEmailSurveyList)
                    {
                        try
                        {
                            EmailDAL emailDAL = new EmailDAL();
                            EmailActivity objEmailActivity = new EmailActivity();
                            SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
                            objSurveyEmailTemplate = emailDAL.GetTemplateDetails(itm.SurveyID, loggedUserId);

                            if (objSurveyEmailTemplate != null &&
                                objSurveyEmailTemplate.SubmittedtoEmail != null &&
                                objSurveyEmailTemplate.SubmittedtoEmail.Length > 0)
                            {
                                bool mailStatus = objEmailActivity.SendMail(itm.EmailTemplateName, objSurveyEmailTemplate, loggedUserId);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

            }
            return syncDBId;

        }
        #region GetDataCollectedSyncDBID
        /// <summary>
        /// GetDataCollectedSyncDBID
        /// </summary>
        /// <param name="CurrentSyncDBID"></param>
        /// <returns></returns>
        public int GetDataCollectedSyncDBID(int CurrentSyncDBID)
        {
            int currentvalue = CurrentSyncDBID;

            try
            {
                string strQeury = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQeury = @"SELECT IFNULL(DataCollectionSyncDBID,0) FROM TbMyTask WHERE  SyncTaskID = @CurrentSyncDBID and ifnull(IsActive,0)=1 and ifnull(IsDeleted,0)=0 limit 1;";

                    int syncdbid = _connection.Query<int>(strQeury, new { CurrentSyncDBID = CurrentSyncDBID }).SingleOrDefault();

                    if (syncdbid > 0)
                    {
                        CurrentSyncDBID = syncdbid;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CurrentSyncDBID;
        }
        #endregion GetDataCollectedSyncDBID
        public void SendTaskNotification(int logUserID, int WorkflowID, int WFLevel, string Type, int TypeID, string WFLevelStatus, string TypeIdColumn, int ParentSyncDBID)
        {
            string strQuery = string.Empty;
            dynamic obj;
            var title = "";
            var sub = "";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (Type != "Payment Request" && Type != "Fund Request" && Type != "Issue" && Type != "Payment")
                    {
                        strQuery = @"SELECT UserID From TbProjectWorkflowDetails Where WorkflowID = @WorkflowID And WfLevel = @WFLevel and IsDeleted =0";
                        obj = _connection.Query<object>(strQuery, new { WorkflowID = WorkflowID, WFLevel = WFLevel + 1 }).ToList();

                        strQuery = "Select Name  From TbUser Where  ID =@logUserID ";
                        var name = _connection.Query<string>(strQuery, new { logUserID = logUserID }).Single();
                        switch (Type)
                        {
                            case "Activity":
                                strQuery = "Select ActivityName TaskName From TbProjectActivity Where  ActivityId =@TypeID ";
                                break;
                            case "Survey":
                                strQuery = "Select SurveyName TaskName From TbProjectSurvey Where  SurveyID =@TypeID ";
                                break;
                            case "KPI":
                                strQuery = "Select KPIName TaskName From TbKPI Where  ID =@TypeID ";
                                break;
                            case "Outcome":
                                strQuery = "Select OutcomeName TaskName From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                                break;
                            case "RISK":
                                strQuery = "Select RiskName TaskName From TbRisk Where  ID =@TypeID ";
                                break;
                            case "Project":
                                strQuery = "Select ProjectName TaskName From TbProject Where  ProjectId =@TypeID ";
                                break;
                            case "Issue":
                                if (TypeIdColumn == "ActivityId")
                                {
                                    strQuery = "Select ActivityName TaskName From TbProjectActivity Where  ActivityId =@TypeID ";
                                }
                                else if (TypeIdColumn == "KPIID")
                                {
                                    strQuery = "Select KPIName TaskName From TbKPI Where  ID =@TypeID ";
                                }
                                else if (TypeIdColumn == "pk_OutcomeID")
                                {
                                    strQuery = "Select OutcomeName TaskName From TbOutcomes Where  pk_OutcomeID =@TypeID ";
                                }
                                else if (TypeIdColumn == "RISKID")
                                {
                                    strQuery = "Select RiskName TaskName From TbRisk Where  ID  =@TypeID ";
                                }
                                break;
                        }
                        var taskname = _connection.Query<string>(strQuery, new { TypeID = TypeID }).Single();



                        if (WFLevel > 0 && WFLevelStatus != "Rejected")
                        {
                            title = name + " data approved.";
                            sub = name + " data approved  for the " + Type + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        if (WFLevel > 0 && WFLevelStatus == "Rejected")
                        {
                            title = name + " data rejected.";
                            sub = name + " data rejected  for the " + Type + " named '" + taskname + "' on " + DateTime.Now;
                        }
                        foreach (dynamic item in obj)
                        {
                            Notification objNotification = new Notification();
                            objNotification.UserId = item.UserID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region GetActivityLOG
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

                        string strMyTaskResponse = "SELECT SYNCDB.CreatedBy,SYNCDB.CreatedDate,SYNCDB.isBulk,TSK.ParentSyncDBID,SYNCDB.TotalWFLevel,TSK.WFLevel,SYNCDB.Command,SYNCDB.Status";
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
                           // objActivityLog1.DefaultText = "";
                            objActivityLog1.Comments = "";
                            //objActivityLog1.SubmittedBy = "Sent by ";


                            string strMyTaskAdd = "SELECT GROUP_CONCAT(USR.Name) DefaultText,SYNCDB.CreatedDate SubmissionDate,SYNCDB.isBulk,US.SubmittedBy,US.ProfileImage ";
                            strMyTaskAdd += " FROM TaskSyncDB SYNCDB JOIN TbProjectSurveyUser SRVUSER ON SYNCDB.TypeID = SRVUSER.SurveyID  JOIN TbUser USR ON USR.ID = SRVUSER.UserID";
                            strMyTaskAdd += " JOIN (SELECT ID AS userId, `Name` SubmittedBy,ifnull(ProfileImage,'') ProfileImage FROM TbUser WHERE IFNULL(IsActive,0) = 1 AND IFNULL(IsDeleted,0) = 0) US ON US.userId = SYNCDB.CreatedBy";
                            strMyTaskAdd += " WHERE SYNCDB.ID = @SyncTaskID  AND IFNULL(SRVUSER.IsDeleted,0) = 0  AND IFNULL(SRVUSER.IsDeleted,0) =0";
                            strMyTaskAdd += " AND SYNCDB.Type = 'Survey' GROUP BY  SRVUSER.SurveyID ";

                            objActivityLog2 = _connection.Query<MyTaskActivityLog>(strMyTaskAdd, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                            objActivityLog1.SubmittedBy = objActivityLog2.SubmittedBy;
                            objActivityLog1.SubmissionDate = objActivityLog2.SubmissionDate;
                            objActivityLog1.DefaultText = objActivityLog2.DefaultText;
                            objActivityLog1.ProfileImage = objActivityLog2.ProfileImage;
                            objActivityLog1.isBulk = objActivityLog2.isBulk;
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
                            objActivityLog1.isBulk = MyTaskResponseLog1.isBulk;

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
                                    objActivityLog1.DefaultText = workflowAssigned;
                                }
                                else
                                {
                                    string strMyTaskWorkflow = "SELECT GROUP_CONCAT(USR.Name) FROM  TbProjectWorkflowDetails wkf JOIN ";
                                    strMyTaskWorkflow += "TbUser USR ON  wkf.UserID  = USR.ID WHERE wkf.WfLevel = @WfLevel  AND wkf.WorkflowID = @WorkflowID";
                                    strMyTaskWorkflow += " AND IFNULL(wkf.IsDeleted,0) = 0  group by wkf.WorkflowID";
                                    string workflowAssigned = _connection.Query<string>(strMyTaskWorkflow, new { WorkflowID = MyTaskResponseLog1.WorkflowID, WfLevel = (MyTaskResponseLog1.WFLevel + 1) }).FirstOrDefault();

                                    objActivityLog1.DefaultText =  workflowAssigned;
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
                    //objActivityLog.DefaultText = "";
                    objActivityLog.Comments = "";
                    //objActivityLog.SubmittedBy = "Sent by ";

                    string strMyTaskAdd = "SELECT GROUP_CONCAT(USR.Name) DefaultText,SYNCDB.CreatedDate SubmissionDate,SYNCDB.isBulk,US.SubmittedBy,US.ProfileImage ";
                    strMyTaskAdd += "  FROM TaskSyncDB SYNCDB JOIN TbProjectSurveyUser SRVUSER ON SYNCDB.TypeID = SRVUSER.SurveyID  JOIN TbUser USR ON USR.ID = SRVUSER.UserID";
                    strMyTaskAdd += " JOIN (SELECT ID AS userId, `Name` SubmittedBy,ifnull(ProfileImage,'') ProfileImage FROM TbUser WHERE IFNULL(IsActive,0) = 1 AND IFNULL(IsDeleted,0) = 0) US ON US.userId = SYNCDB.CreatedBy";
                    strMyTaskAdd += " WHERE SYNCDB.ID = @SyncTaskID  AND IFNULL(SRVUSER.IsDeleted,0) = 0  AND IFNULL(SRVUSER.IsDeleted,0) =0";
                    strMyTaskAdd += " AND SYNCDB.Type = 'Survey' GROUP BY  SRVUSER.SurveyID ";

                    objActivityLog1 = _connection.Query<MyTaskActivityLog>(strMyTaskAdd, new { SyncTaskID = syncTaskID }).FirstOrDefault();

                    if (objActivityLog1 != null)
                    {
                        objActivityLog.SubmittedBy = objActivityLog1.SubmittedBy;
                        objActivityLog.SubmissionDate = objActivityLog1.SubmissionDate;
                        objActivityLog.DefaultText = objActivityLog1.DefaultText;
                        objActivityLog.ProfileImage = objActivityLog1.ProfileImage;
                        objActivityLog.isBulk = objActivityLog1.isBulk;
                    }

                    lstActivityLog.Add(objActivityLog);
                }
            }

            //ENDS survey section

            return lstActivityLog;
        }
        #endregion
    }
}
