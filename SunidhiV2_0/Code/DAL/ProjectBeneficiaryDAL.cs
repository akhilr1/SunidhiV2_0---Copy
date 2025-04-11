using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.DAL
{
    public class ProjectBeneficiaryDAL
    {
        protected IDbConnection _connection;

        #region Get
        /// <summary>
        /// Function used to fetch beneficiary types based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBeneficiaryType> GetAllBeneficiaryTypes(int ProjectID)
        {
            List<ProjectBeneficiaryType> lstBType = new List<ProjectBeneficiaryType>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT BeneficiaryTypeID, Name, CreatedDate, UpdatedDate FROM TbProjectBeneficiaryType Where IsActive =1 and IsDeleted =0 And ProjectID=@ProjectID Order By BeneficiaryTypeID desc";
                    lstBType = _connection.Query<ProjectBeneficiaryType>(strQuery, new { ProjectID = ProjectID }).ToList();


                }
            }
            catch (Exception ex)
            {
            }
            return lstBType;
        }

        /// <summary>
        /// Function used to fetch beneficiary types for task filter based on WorkspaceID
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectBeneficiaryType> GetProjectBeneficiaryTypeForFilter(int WorkspaceID)
        {
            List<ProjectBeneficiaryType> lstBType = new List<ProjectBeneficiaryType>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT BT.Name, BT.BeneficiaryTypeID, P.WSID, BT.CreatedDate, BT.UpdatedDate FROM TbProjectBeneficiaryType BT inner join TbProject P on BT.ProjectID=P.ProjectId Where BT.IsDeleted =0 And P.WSID=@WSID Order By BT.BeneficiaryTypeID desc";
                lstBType = _connection.Query<ProjectBeneficiaryType>(strQuery, new { WSID = WorkspaceID }).ToList();


            }
            return lstBType;
        }

        /// <summary>
        /// Function used to fetch beneficiary answer type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BeneficiaryAnswerType> GetAnswerTypes()
        {
            List<BeneficiaryAnswerType> lstAnswerType = new List<BeneficiaryAnswerType>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT AnswerTypeID, AnswerType FROM TbBeneficiaryAnswerType Where IsActive =1 and IsDeleted =0 order by AnswerType asc;";
                    lstAnswerType = _connection.Query<BeneficiaryAnswerType>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstAnswerType;
        }

        /// <summary>
        /// Function used to fetch beneficiary cascading question options
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BeneficiaryTypeCascadingQstnOptions> GetCascadingQuestionOptions()
        {
            List<BeneficiaryTypeCascadingQstnOptions> lst = new List<BeneficiaryTypeCascadingQstnOptions>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select ID,OptionValue  From TbBeneficiaryCascadingQstnOptions Where IsActive =1 and IsDeleted =0";
                    lst = _connection.Query<BeneficiaryTypeCascadingQstnOptions>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        /// <summary>
        /// Function used to fetch cascading question subOptions based on ParentOptionID
        /// </summary>
        /// <param name="ParentOptionID"></param>
        /// <returns></returns>
        public IEnumerable<BeneficiaryTypeCascadingQstnOptions> GetCascadingQuestionSubOptions(int ParentOptionID)
        {
            List<BeneficiaryTypeCascadingQstnOptions> lst = new List<BeneficiaryTypeCascadingQstnOptions>();
            try
            {
                string strQuery = string.Empty;
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select B.HierarchyOrder,B.OptionValue,B.TableName, B.ColumnName, B.PrimaryColumn, B.ParentItemColumn 
                            From TbBeneficiaryCascadingQstnOptions A inner Join TbBeneficiaryCascadingQstnOptions B On A.OptionGroupID =B.OptionGroupID
                            Where B.IsActive =1 And B.IsDeleted =0 And A.ID =@ParentOptionID And A.HierarchyOrder <=B.HierarchyOrder  Order By B.HierarchyOrder";
                    lst = _connection.Query<BeneficiaryTypeCascadingQstnOptions>(strQuery, new { ParentOptionID = ParentOptionID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }
        //=============Beneficiaries===============================

        /// <summary>
        /// Function used to fetch beneficiary type details based on BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBeneficiaryType> GetBeneficiaryTypeDetails(int BeneficiaryTypeID)
        {
            List<ProjectBeneficiaryType> lstBeneficiaryType = new List<ProjectBeneficiaryType>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT BeneficiaryTypeID, Name,Description, CreatedDate, UpdatedDate FROM TbProjectBeneficiaryType Where BeneficiaryTypeID= @BeneficiaryTypeID and IsActive =1 And IsDeleted =0";
                    lstBeneficiaryType = _connection.Query<ProjectBeneficiaryType>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).ToList();
                    foreach (ProjectBeneficiaryType btItem in lstBeneficiaryType)
                    {
                        btItem.BTTags = new List<Tags>();
                        strQuery = @"Select BT.TagID , T.TagName, T.TagColor From TbProjectTags T
                                Inner Join TbBeneficiaryTypeTagDetails BT On BT.TagID =T.TagID Where BT.BeneficiaryTypeID= @BeneficiaryTypeID and BT.IsDeleted=@IsDeleted and T.IsDeleted = 0 ";

                        var BeneficiaryTypeTags = _connection.Query<Tags>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID, IsDeleted = 0 }).ToList();
                        btItem.BeneficiaryTypeTags = JsonConvert.SerializeObject(BeneficiaryTypeTags);

                        btItem.BTQuestions = new List<BeneficiaryTypeQuestions>();
                        strQuery = @"Select BTQ.BeneficiaryTypeID,BTQ.QuestionID, BTQ.QuestionName, BTQ.AnswerTypeID, BTA.AnswerType,BTQ.IsUnique, BTQ.IsSearchable, BTQ.IsMandatoryResponse, BTQ.QuestionHint,BTQ.IsValidationQuestion,BTQ.RegexExpression,BTQ.QuestionOrder " +
                                        " From TbBeneficiaryTypeQuestion BTQ Inner Join TbBeneficiaryAnswerType BTA On BTA.AnswerTypeID = BTQ.AnswerTypeID " +
                                        " Where BTQ.IsActive =1 And BTQ.IsDeleted =0 and BTA.IsActive =1 And BTA.IsDeleted =0 and BTQ.BeneficiaryTypeID= @BeneficiaryTypeID order by BTQ.QuestionOrder asc;";

                        btItem.BTQuestions = _connection.Query<BeneficiaryTypeQuestions>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).ToList();
                        foreach (BeneficiaryTypeQuestions btQItem in btItem.BTQuestions)
                        {
                            btQItem.BTQstnAnsOptionList = new List<BeneficiaryTypeQstnAnsOptionList>();
                            strQuery = @"Select BTQ.BeneficiaryTypeID,AO.QstnAnsOptionID,BTQ.QuestionID, BTQ.AnswerTypeID, AO.OptionValue, 0 IsOptSelected " +
                                        " From TbBeneficiaryTypeQstnAnswerOptions AO Inner join TbBeneficiaryTypeQuestion BTQ On BTQ.QuestionID = AO.QuestionID And BTQ.AnswerTypeID = AO.AnswerTypeID  " +
                                        " Where AO.IsActive =1 And AO.IsDeleted =0 and BTQ.IsActive =1 And BTQ.IsDeleted =0 and BTQ.BeneficiaryTypeID= @BeneficiaryTypeID And BTQ.QuestionID =@QuestionID order by optionvalue";

                            btQItem.BTQstnAnsOptionList = _connection.Query<BeneficiaryTypeQstnAnsOptionList>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID, QuestionID = btQItem.QuestionID }).ToList();

                            // validations
                            btQItem.BTQstnValidationList = new List<BeneficiaryTypeValidation>();
                            strQuery = @"Select ID, QuestionID,ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator,CreatedDate,CreatedBy from TbBeneficiaryQSTCondition where QuestionID = @QuestionID";
                            btQItem.BTQstnValidationList = _connection.Query<BeneficiaryTypeValidation>(strQuery, new { QuestionID = btQItem.QuestionID }).ToList();

                            if (btQItem.AnswerType == "Cascading Dropdown")
                            {

                                List<BeneficiaryTypeCascadingQstnOptions> lstCascadingItems = new List<BeneficiaryTypeCascadingQstnOptions>();
                                for (int i = 0; i < btQItem.BTQstnAnsOptionList.Count; i++)
                                {
                                    var lstItems = new List<BeneficiaryTypeCascadingQstnOptions>();

                                    strQuery = @"Select OptionGroupID, OptionValue, HierarchyOrder, TableName, ColumnName, PrimaryColumn, ParentItemColumn
                                            from TbBeneficiaryCascadingQstnOptions Where OptionValue=@OptionValue and IsActive =1 And IsDeleted =0";
                                    lstItems = _connection.Query<BeneficiaryTypeCascadingQstnOptions>(strQuery, new { OptionValue = btQItem.BTQstnAnsOptionList[i].OptionValue }).ToList();

                                    if (lstItems[0].OptionValue == "Country")
                                        strQuery = @"Select CountryID ID ,CountryName Name ,0 ParentID from TbCountry Order By CountryName";
                                    if (lstItems[0].OptionValue == "States")
                                        strQuery = @"Select StateID ID ,StateName Name , CountryID ParentID from TbStates Order By StateName";
                                    if (lstItems[0].OptionValue == "District")
                                        strQuery = @"Select DistrictID ID ,DistrictName Name,StateID  ParentID from TbDistrict Order By DistrictName";
                                    if (lstItems[0].OptionValue == "SubDistrict")
                                        strQuery = @"Select SubDistrictID ID ,SubDistrictName Name,DistrictID ParentID from TbSubDistrict Order By SubDistrictName";
                                    lstItems[0].BTQstnAnsCascadingListItems = _connection.Query<BeneficiaryTypeCascadingList>(strQuery).ToList();

                                    lstCascadingItems.InsertRange(i, lstItems);
                                }
                                btQItem.BTQstnAnsCascadingOptionList = lstCascadingItems;
                            }
                            strQuery = @"SELECT EXISTS(SELECT  distinct BeneficiaryID from  TbBeneficiaryRegistrationDetails   where QuestionID =@QuestionID)";
                            btQItem.IsRegistrationDone = _connection.Query<int>(strQuery, new { QuestionID = btQItem.QuestionID }).Single();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiaryType;
        }

        /// <summary>
        /// Function used to fetch cascading options list based on optionValue
        /// </summary>
        /// <param name="optionValue"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public IEnumerable<BeneficiaryTypeCascadingQstnOptions> GetCascadingOptionsList(string optionValue, int parentID)
        {
            string strQuery = string.Empty;
            var lstItems = new List<BeneficiaryTypeCascadingQstnOptions>();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {


                    strQuery = @"Select OptionGroupID, OptionValue, HierarchyOrder, TableName, ColumnName, PrimaryColumn, ParentItemColumn
                                            from TbBeneficiaryCascadingQstnOptions Where OptionValue=@OptionValue and IsActive =1 And IsDeleted =0";
                    lstItems = _connection.Query<BeneficiaryTypeCascadingQstnOptions>(strQuery, new { OptionValue = optionValue }).ToList();


                    if (optionValue == "Country")
                        strQuery = @"Select StateID ID ,StateName Name , CountryID ParentID from TbStates Where CountryID = @parentID Order By StateName";
                    if (optionValue == "States")
                        strQuery = @"Select DistrictID ID ,DistrictName Name,StateID  ParentID from TbDistrict Where StateID = @parentID Order By DistrictName";
                    if (optionValue == "District")
                        strQuery = @"Select SubDistrictID ID ,SubDistrictName Name,DistrictID ParentID from TbSubDistrict Where DistrictID = @parentID Order By SubDistrictName";
                    if (optionValue != "SubDistrict")
                        lstItems[0].BTQstnAnsCascadingListItems = _connection.Query<BeneficiaryTypeCascadingList>(strQuery, new { parentID = parentID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        /// <summary>
        /// Function used to fetch all beneficiaries based on BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBeneficiary> GetAllBeneficiaries(int BeneficiaryTypeID)
        {
            List<ProjectBeneficiary> lstBType = new List<ProjectBeneficiary>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT BeneficiaryTypeID,BeneficiaryID, BeneficiaryName, CreatedDate, UpdatedDate FROM TbProjectBeneficiary 
                                    Where IsDeleted =0 and IsActive =1 And BeneficiaryTypeID=@BeneficiaryTypeID Order By BeneficiaryID desc";
                    lstBType = _connection.Query<ProjectBeneficiary>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBType;
        }

        public IEnumerable<BeneficiaryRegistration> GetBeneficiaryQstnAnsw(string BeneficiaryIDs)
        {
            List<BeneficiaryRegistration> lstBeneficiary = new List<BeneficiaryRegistration>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select B.BeneficiaryTypeID,BT.Name BeneficiaryTypeName, B.BeneficiaryName,R.BeneficiaryID,R.QuestionID,Q.QuestionName,R.AnswerTypeID,A.AnswerType,R.Answer,Q.IsUnique, Q.IsSearchable, Q.IsMandatoryResponse, Q.QuestionHint,Q.IsValidationQuestion,Q.RegexExpression From TbBeneficiaryRegistrationDetails R inner join TbProjectBeneficiary B On B.BeneficiaryID=R.BeneficiaryID and ifnull(B.IsActive,0) =1 And ifnull(B.IsDeleted,0) =0 inner join TbProjectBeneficiaryType BT On BT.BeneficiaryTypeID=B.BeneficiaryTypeID and ifnull(BT.IsActive,0) =1 And ifnull(BT.IsDeleted,0) =0 inner join TbBeneficiaryTypeQuestion Q on Q.BeneficiaryTypeID =B.BeneficiaryTypeID And Q.QuestionID =R.QuestionID and ifnull(Q.IsActive,0) =1 And ifnull(Q.IsDeleted,0) =0 inner join TbBeneficiaryAnswerType A On Q.AnswerTypeID=A.AnswerTypeID and ifnull(A.IsActive,0) =1 And ifnull(A.IsDeleted,0) =0 Where FIND_IN_SET(R.BeneficiaryID, @BeneficiaryIDs) and ifnull(R.IsDeleted,0) =0 ORDER BY R.BeneficiaryID, R.QuestionID ASC ";
                    lstBeneficiary = _connection.Query<BeneficiaryRegistration>(strQuery, new { BeneficiaryIDs = BeneficiaryIDs }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiary;
        }

        /// <summary>
        /// Function used to fetch beneficary details based on BeneficiaryID
        /// </summary>
        /// <param name="BeneficiaryID"></param>
        /// <returns></returns>
        public IEnumerable<BeneficiaryRegistration> GetBeneficiaryDetails(int BeneficiaryID)
        {
            List<BeneficiaryRegistration> lstBeneficiary = new List<BeneficiaryRegistration>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //-- Query
                    //strQuery = @"Select B.BeneficiaryTypeID,Name BeneficiaryTypeName, BeneficiaryName,R.BeneficiaryID,R.QuestionID,QuestionName, "+
                    //            " R.AnswerTypeID,AnswerType,Answer From TbBeneficiaryRegistrationDetails R inner join TbProjectBeneficiary B On B.BeneficiaryID=R.BeneficiaryID "+
                    //            "inner join TbProjectBeneficiaryType BT On BT.BeneficiaryTypeID=B.BeneficiaryTypeID Inner Join (Select BeneficiaryTypeID,QuestionID,QuestionName, "+
                    //            " Q.AnswerTypeID,AnswerType From TbBeneficiaryTypeQuestion Q Inner Join TbBeneficiaryAnswerType A On Q.AnswerTypeID=A.AnswerTypeID) QA "+
                    //            " On QA.BeneficiaryTypeID =B.BeneficiaryTypeID And QA.QuestionID =R.QuestionID Where R.BeneficiaryID = @BeneficiaryID";

                    strQuery = @"Select IFNULL(B.BeneficiaryTypeID,Q.BeneficiaryTypeID) BeneficiaryTypeID,BT.Name BeneficiaryTypeName,B.BeneficiaryName,
                                 B.BeneficiaryID,Q.QuestionID,Q.QuestionName,A.AnswerTypeID,A.AnswerType,ifnull(REG.Answer,'') Answer,Q.IsUnique, Q.IsSearchable, Q.IsMandatoryResponse, 
                                 Q.QuestionHint,Q.IsValidationQuestion,Q.RegexExpression From TbBeneficiaryTypeQuestion Q
                                 LEFT JOIN (SELECT R.QuestionID,R.BeneficiaryID,R.AnswerTypeID,R.Answer FROM  TbBeneficiaryRegistrationDetails R 
                                 WHERE R.BeneficiaryID = @BeneficiaryID AND ifnull(R.IsDeleted,0) =0 )  REG ON Q.QuestionID =REG.QuestionID
                                inner join TbProjectBeneficiary B On B.BeneficiaryTypeID=Q.BeneficiaryTypeID 
                                inner join TbProjectBeneficiaryType BT On BT.BeneficiaryTypeID=Q.BeneficiaryTypeID
                                inner join TbBeneficiaryAnswerType A On Q.AnswerTypeID=A.AnswerTypeID 
                                and ifnull(Q.IsActive,0) =1 And ifnull(Q.IsDeleted,0) =0 and ifnull(A.IsActive,0) =1 And ifnull(A.IsDeleted,0) =0
                                and ifnull(BT.IsActive,0) =1 And ifnull(BT.IsDeleted,0) =0 and ifnull(B.IsActive,0) =1 And ifnull(B.IsDeleted,0) =0 
                                and B.BeneficiaryID = @BeneficiaryID order by  Q.QuestionID asc;";
                    lstBeneficiary = _connection.Query<BeneficiaryRegistration>(strQuery, new { BeneficiaryID = BeneficiaryID }).ToList();
                    foreach (BeneficiaryRegistration optItem in lstBeneficiary)
                    {
                        optItem.BenefAnswerOptions = new List<BeneficiaryTypeQstnAnsOptionList>();
                        strQuery = @"Select BTQ.BeneficiaryTypeID,AO.QstnAnsOptionID,BTQ.QuestionID, BTQ.AnswerTypeID, AO.OptionValue, 0 IsOptSelected " +
                                    " From TbBeneficiaryTypeQstnAnswerOptions AO Inner join TbBeneficiaryTypeQuestion BTQ On BTQ.QuestionID = AO.QuestionID And BTQ.AnswerTypeID = AO.AnswerTypeID  " +
                                    " Where ifnull(AO.IsActive,0) =1 And ifnull(AO.IsDeleted,0) =0 and ifnull(BTQ.IsActive,0) =1 And ifnull(BTQ.IsDeleted,0) =0 and BTQ.QuestionID =@QuestionID ";

                        optItem.BenefAnswerOptions = _connection.Query<BeneficiaryTypeQstnAnsOptionList>(strQuery, new { QuestionID = optItem.QuestionID }).ToList();
                        if (optItem.AnswerType == "Cascading Dropdown")
                        {

                            List<BeneficiaryTypeCascadingQstnOptions> lstCascadingItems = new List<BeneficiaryTypeCascadingQstnOptions>();
                            for (int i = 0; i < optItem.BenefAnswerOptions.Count; i++)
                            {
                                var lstItems = new List<BeneficiaryTypeCascadingQstnOptions>();

                                strQuery = @"Select OptionGroupID, OptionValue, HierarchyOrder, TableName, ColumnName, PrimaryColumn, ParentItemColumn
                                            from TbBeneficiaryCascadingQstnOptions Where OptionValue=@OptionValue";
                                lstItems = _connection.Query<BeneficiaryTypeCascadingQstnOptions>(strQuery, new { OptionValue = optItem.BenefAnswerOptions[i].OptionValue }).ToList();

                                if (lstItems[0].OptionValue == "Country")
                                    strQuery = @"Select CountryID ID ,CountryName Name ,0 ParentID from TbCountry Order By CountryName";
                                if (lstItems[0].OptionValue == "States")
                                    strQuery = @"Select StateID ID ,StateName Name , CountryID ParentID from TbStates Order By StateName";
                                if (lstItems[0].OptionValue == "District")
                                    strQuery = @"Select DistrictID ID ,DistrictName Name,StateID  ParentID from TbDistrict Order By DistrictName";
                                if (lstItems[0].OptionValue == "SubDistrict")
                                    strQuery = @"Select SubDistrictID ID ,SubDistrictName Name,DistrictID ParentID from TbSubDistrict Order By SubDistrictName";
                                lstItems[0].BTQstnAnsCascadingListItems = _connection.Query<BeneficiaryTypeCascadingList>(strQuery).ToList();

                                lstCascadingItems.InsertRange(i, lstItems);
                            }
                            optItem.BenefAnswerCascadingOptionList = lstCascadingItems;
                        }

                        // validations
                        optItem.BTQstnValidationList = new List<BeneficiaryTypeValidation>();
                        strQuery = @"Select ID, QuestionID,ValidationCondition,MinValue,`MaxValue`,ConditionValue,LogicOperator,CreatedDate,CreatedBy from TbBeneficiaryQSTCondition where QuestionID = @QuestionID";
                        optItem.BTQstnValidationList = _connection.Query<BeneficiaryTypeValidation>(strQuery, new { QuestionID = optItem.QuestionID }).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiary;
        }

        /// <summary>
        /// Function used to fetch beneficiary datacollection list based on BeneficiaryID
        /// </summary>
        /// <param name="BeneficiaryID"></param>
        /// <returns></returns>
        public IEnumerable<BeneficiaryRegistration> GetBeneficiaryDataCollectionList(int BeneficiaryID)
        {
            List<BeneficiaryRegistration> lstBeneficiary = new List<BeneficiaryRegistration>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"Select B.BeneficiaryTypeID,BT.Name BeneficiaryTypeName, B.BeneficiaryName,R.BeneficiaryID,R.QuestionID,QA.QuestionName,R.AnswerTypeID,QA.AnswerType,R.Answer " +
                    //            " From TbBeneficiaryRegistrationDetails R inner join TbProjectBeneficiary B On B.BeneficiaryID=R.BeneficiaryID " +
                    //            "inner join TbProjectBeneficiaryType BT On BT.BeneficiaryTypeID=B.BeneficiaryTypeID Inner Join (Select Q.BeneficiaryTypeID,Q.QuestionID,Q.QuestionName,Q.AnswerTypeID,A.AnswerType " +
                    //            " From TbBeneficiaryTypeQuestion Q Inner Join TbBeneficiaryAnswerType A On Q.AnswerTypeID=A.AnswerTypeID) QA " +
                    //            " On QA.BeneficiaryTypeID =B.BeneficiaryTypeID And QA.QuestionID =R.QuestionID Where R.BeneficiaryID = @BeneficiaryID";

                    strQuery = @"Select B.BeneficiaryTypeID,BT.Name BeneficiaryTypeName, B.BeneficiaryName,R.BeneficiaryID,R.QuestionID,Q.QuestionName,R.AnswerTypeID,A.AnswerType,R.Answer 
                                 From TbBeneficiaryRegistrationDetails R inner join TbProjectBeneficiary B On B.BeneficiaryID=R.BeneficiaryID and ifnull(B.IsActive,0) =1 And ifnull(B.IsDeleted,0) =0 
                                 inner join TbProjectBeneficiaryType BT On BT.BeneficiaryTypeID=B.BeneficiaryTypeID and ifnull(BT.IsActive,0) =1 And ifnull(BT.IsDeleted,0) =0 
                                 Inner Join TbBeneficiaryTypeQuestion Q on Q.BeneficiaryTypeID =B.BeneficiaryTypeID And Q.QuestionID =R.QuestionID and ifnull(Q.IsActive,0) =1 And ifnull(Q.IsDeleted,0) =0 
                                 Inner Join TbBeneficiaryAnswerType A On Q.AnswerTypeID=A.AnswerTypeID and ifnull(A.IsActive,0) =1 And ifnull(A.IsDeleted,0) =0 
                                 Where ifnull(R.IsDeleted,0) =0 and R.BeneficiaryID = @BeneficiaryID";

                    lstBeneficiary = _connection.Query<BeneficiaryRegistration>(strQuery, new { BeneficiaryID = BeneficiaryID }).ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return lstBeneficiary;
        }

        /// <summary>
        /// Function used to survey details based on BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectSurvey> GetSurveyDetailsByBenefTypeID(int BeneficiaryTypeID)
        {
            List<ProjectSurvey> lstBType = new List<ProjectSurvey>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT SurveyID,SurveyName,Date(UpdatedDate) UpdatedDate FROM TbProjectSurvey Where IsActive =1 And BeneficiaryTypeID=@BeneficiaryTypeID Order By SurveyName";
                    lstBType = _connection.Query<ProjectSurvey>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).ToList();


                }
            }
            catch (Exception ex)
            {
            }
            return lstBType;
        }

        #endregion Get

        #region CreateBeneficiaries   

        /// <summary>
        /// Function uesed to add beneficiary type
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="BenefName"></param>
        /// <param name="BenefDesc"></param>
        /// <param name="BenefTagsList"></param>
        /// <param name="BenefQstns"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int CreateBeneficiaryType(int ProjectID, string BenefName, string BenefDesc, string BenefTagsList, string BenefQstns, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = "sp_CreateBeneficiaryType";
                        id = _connection.ExecuteScalar<int>(strQuery, new
                        {
                            pProjectID = ProjectID,
                            BenefName = BenefName,
                            BenefDesc = BenefDesc,
                            BenefTagsList = BenefTagsList,
                            BenefQstns = BenefQstns,
                            loggedUserId = loggedUserId
                        }, commandType: CommandType.StoredProcedure);

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectID;
                        objLog.AuditFrom = "Beneficiary";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = id;
                        objLog.FromIDColumnName = "BeneficiaryTypeID";
                        objLog.Message = "Added new BeneficiaryType named '" + BenefName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }

            }

            catch (Exception ex)
            {
                //retMsg = "Error while saving data." + ex.Message;
                id = 0;
                throw ex;
            }
            return id;

        }

        /// <summary>
        /// Function uesed to add beneficiary
        /// </summary>
        /// <param name="BenefName"></param>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="BenefQstnAnswerList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int CreateBeneficiary(string BenefName, int BeneficiaryTypeID, string BenefQstnAnswerList, int loggedUserId, string TokenValue)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        strQuery = "sp_CreateBeneficiary";
                        id = _connection.ExecuteScalar<int>(strQuery, new
                        {

                            BenefName = BenefName,
                            BenefTID = BeneficiaryTypeID,
                            BenefQstnsAns = BenefQstnAnswerList,
                            loggedUserId = loggedUserId,
                            vaTokenValue = TokenValue,
                        }, commandType: CommandType.StoredProcedure);

                        strQuery = "Select ProjectId  From TbProjectBeneficiaryType Where BeneficiaryTypeID =@BeneficiaryTypeID ";
                        var ProjectID = _connection.Query<int>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectID;
                        objLog.AuditFrom = "Beneficiary";
                        objLog.AuditEvent = "ADD";
                        objLog.AuditFromID = id;
                        objLog.FromIDColumnName = "BeneficiaryID";
                        objLog.Message = "Added new Beneficiary  named '" + BenefName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                //retMsg = "Error while saving data." + ex.Message;
                id = 0;
                throw ex;
            }
            return id;

        }

        /// <summary>
        /// Function used to import register beneficiary
        /// </summary>
        /// <param name="BenefImportList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int ImportRegisterBeneficiary(string BenefImportList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_CreateImportBeneficiary";
                    id = _connection.ExecuteScalar<int>(strQuery, new
                    {
                        BenefImportList = BenefImportList,
                        loggedUserId = loggedUserId
                    }, commandTimeout: 5000, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                //retMsg = "Error while saving data." + ex.Message;
                id = 0;
                throw ex;
            }
            return id;

        }

        /// <summary>
        /// Function used to update beneficiary based on BeneficiaryID,BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="BeneficiaryID"></param>
        /// <param name="QID"></param>
        /// <param name="AnsID"></param>
        /// <param name="Answer"></param>
        /// <param name="BeneficiaryName"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateBeneficiary(int BeneficiaryTypeID, int BeneficiaryID, int QID, int AnsID, string Answer, string BeneficiaryName, int loggedUserId)
        {

            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        string query = @"UPDATE TbProjectBeneficiary SET BeneficiaryName =@BeneficiaryName,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                        " where BeneficiaryID =@BeneficiaryID and BeneficiaryTypeID =@BeneficiaryTypeID";
                        _connection.Execute(query, new
                        {
                            BeneficiaryTypeID = BeneficiaryTypeID,
                            BeneficiaryID = BeneficiaryID,
                            BeneficiaryName = BeneficiaryName,
                            updatedDate = DateTime.Now,
                            loggedUserId = loggedUserId
                        });

                        query = @"UPDATE TbBeneficiaryRegistrationDetails SET Answer =@Answer,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                       " where BeneficiaryID =@BeneficiaryID and QuestionID=@QID and AnswerTypeID=@AnsID";
                        _connection.Execute(query, new
                        {
                            BeneficiaryTypeID = BeneficiaryTypeID,
                            BeneficiaryID = BeneficiaryID,
                            Answer = Answer,
                            QID = QID,
                            AnsID = AnsID,
                            updatedDate = DateTime.Now,
                            loggedUserId = loggedUserId
                        });
                        query = "Select ProjectId  From TbProjectBeneficiaryType Where BeneficiaryTypeID =@BeneficiaryTypeID ";
                        var ProjectID = _connection.Query<int>(query, new { BeneficiaryTypeID = BeneficiaryTypeID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectID;
                        objLog.AuditFrom = "Beneficiary";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = BeneficiaryID;
                        objLog.FromIDColumnName = "BeneficiaryID";
                        objLog.Message = "Updated Beneficiary  named '" + BeneficiaryName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while deleting data";
                        throw ex;
                    }
                    _Tran.Complete();
                }

            }
            return retMsg;
        }

        /// <summary>
        /// Function used to update beneficiary type details based on BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="BeneficiaryTypeName"></param>
        /// <param name="BeneficiaryTypeDesc"></param>
        /// <param name="BeneficiaryTypeTagList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateBeneficiaryTypeDetails(int BeneficiaryTypeID, string BeneficiaryTypeName, string BeneficiaryTypeDesc, string BeneficiaryTypeTagList, int loggedUserId)
        {
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        string query = @"UPDATE TbProjectBeneficiaryType SET Name =@BeneficiaryTypeName,Description =@BeneficiaryTypeDesc, " +
                                        "UpdatedDate = @updatedDate, UpdatedBy=@loggedUserId  where BeneficiaryTypeID =@BeneficiaryTypeID";
                        _connection.Execute(query, new
                        {
                            BeneficiaryTypeName = BeneficiaryTypeName,
                            BeneficiaryTypeDesc = BeneficiaryTypeDesc,
                            BeneficiaryTypeID = BeneficiaryTypeID,
                            updatedDate = DateTime.Now,
                            loggedUserId = loggedUserId
                        });
                        // --- Tag List
                        if (!string.IsNullOrEmpty(BeneficiaryTypeTagList))
                        {
                            bool success = DeleteProjectBeneficiaryTag(BeneficiaryTypeID, loggedUserId);

                            List<Tags> lstNotifyUser = JsonConvert.DeserializeObject<List<Tags>>(BeneficiaryTypeTagList);
                            foreach (Tags tagItem in lstNotifyUser)
                            {
                                SaveProjectBeneficiaryTag(BeneficiaryTypeID, tagItem.TagID, loggedUserId);
                            }
                        }
                        query = "Select ProjectId  From TbProjectBeneficiaryType Where BeneficiaryTypeID =@BeneficiaryTypeID ";
                        var ProjectID = _connection.Query<int>(query, new { BeneficiaryTypeID = BeneficiaryTypeID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectID;
                        objLog.AuditFrom = "Beneficiary";
                        objLog.AuditEvent = "EDIT";
                        objLog.AuditFromID = BeneficiaryTypeID;
                        objLog.FromIDColumnName = "BeneficiaryTypeID";
                        objLog.Message = "Updated Beneficiary Type named '" + BeneficiaryTypeName + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while deleting data";
                        throw ex;
                    }
                    _Tran.Complete();
                }

            }
            return retMsg;
        }

        /// <summary>
        /// Function used to update beneficiary type questions based on BeneficiaryTypeID,QuestionID 
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="QID"></param>
        /// <param name="QuestionName"></param>
        /// <param name="AnsID"></param>
        /// <param name="IsUnique"></param>
        /// <param name="IsSearchable"></param>
        /// <param name="IsMandatoryResponse"></param>
        /// <param name="QuestionHint"></param>
        /// <param name="IsValidationQuestion"></param>
        /// <param name="RegexExpression"></param>
        /// <param name="QstnAnsOptions"></param>
        /// <param name="QstnValidation"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateBeneficiaryTypeQuestions(int BeneficiaryTypeID, int QID, string QuestionName, int AnsID, bool IsUnique,
                        bool IsSearchable, bool IsMandatoryResponse, string QuestionHint, int IsValidationQuestion, string RegexExpression, string QstnAnsOptions, string QstnValidation, int loggedUserId,int QuestionOrder)
        {
            var retMsg = "success";
            string strQuery = "";

            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {

                        if (QID == 0)
                        {
                            strQuery = "INSERT INTO TbBeneficiaryTypeQuestion(BeneficiaryTypeID,QuestionName,AnswerTypeID,IsUnique, IsSearchable, IsMandatoryResponse,QuestionHint, IsValidationQuestion,CreatedDate,CreatedBy,IsDeleted,IsActive,QuestionOrder,RegexExpression) VALUES(@BeneficiaryTypeID,@QuestionName,@AnsID,@IsUnique, @IsSearchable, @IsMandatoryResponse,@QuestionHint, @IsValidationQuestion,now(),@loggedUserId,0,1,@QuestionOrder,@RegexExpression); SELECT LAST_INSERT_ID();";
                            QID = _connection.Query<int>(strQuery, new
                            {
                                BeneficiaryTypeID = BeneficiaryTypeID,
                                QuestionName = QuestionName,
                                AnsID = AnsID,
                                IsUnique = IsUnique,
                                IsSearchable = IsSearchable,
                                IsMandatoryResponse = IsMandatoryResponse,
                                QuestionHint = QuestionHint,
                                IsValidationQuestion = IsValidationQuestion,
                                loggedUserId = loggedUserId,
                                QuestionOrder = QuestionOrder,
                                RegexExpression = RegexExpression,
                            }).Single();

                        }
                        else
                        {
                            strQuery = @"UPDATE TbBeneficiaryTypeQuestion SET QuestionName =@QuestionName,AnswerTypeID =@AnsID ,IsUnique = @IsUnique,  IsSearchable = @IsSearchable, " +
                                        " IsMandatoryResponse = @IsMandatoryResponse, QuestionHint = @QuestionHint,IsValidationQuestion = @IsValidationQuestion, RegexExpression = @RegexExpression,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                        " ,QuestionOrder = @QuestionOrder where BeneficiaryTypeID =@BeneficiaryTypeID and QuestionID =@QID";
                            _connection.Execute(strQuery, new
                            {
                                BeneficiaryTypeID = BeneficiaryTypeID,
                                QID = QID,
                                QuestionName = QuestionName,
                                AnsID = AnsID,
                                IsUnique = IsUnique,
                                IsSearchable = IsSearchable,
                                IsMandatoryResponse = IsMandatoryResponse,
                                QuestionHint = QuestionHint,
                                IsValidationQuestion = IsValidationQuestion,
                                RegexExpression = RegexExpression,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId,
                                QuestionOrder = QuestionOrder
                            });
                            strQuery = @"Delete from TbBeneficiaryTypeQstnAnswerOptions where BeneficiaryTypeID =@BeneficiaryTypeID And QuestionID =@QuestionID";
                            _connection.Execute(strQuery, new
                            {
                                BeneficiaryTypeID = BeneficiaryTypeID,
                                @QuestionID = QID
                            });

                        }

                        if (QstnAnsOptions != null)
                        {
                            List<BeneficiaryTypeQstnAnsOptionList> lstQstnOpt = JsonConvert.DeserializeObject<List<BeneficiaryTypeQstnAnsOptionList>>(QstnAnsOptions);


                            foreach (BeneficiaryTypeQstnAnsOptionList Item in lstQstnOpt)
                            {
                                strQuery = @"INSERT INTO TbBeneficiaryTypeQstnAnswerOptions(BeneficiaryTypeID,QuestionID,AnswerTypeID,OptionValue,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted,IsActive) 	
                                VALUES (@BeneficiaryTypeID,@QuestionID,@AnswerTypeID,@OptionValue,now(),@loggedUserId,now(),@loggedUserId,0,1);";

                                _connection.Execute(strQuery, new
                                {
                                    BeneficiaryTypeID = BeneficiaryTypeID,
                                    @QuestionID = QID,
                                    AnswerTypeID = AnsID,
                                    OptionValue = Item.OptionValue,
                                    loggedUserId = loggedUserId
                                });
                            }
                        }

                        // Delete and Insert Question Validation
                        strQuery = @"Delete from TbBeneficiaryQSTCondition where QuestionID =@QuestionID";
                        _connection.Execute(strQuery, new
                        {
                            QuestionID = QID
                        });

                        if (QstnValidation != null)
                        {
                            List<BeneficiaryTypeValidation> lstQstnValidation = JsonConvert.DeserializeObject<List<BeneficiaryTypeValidation>>(QstnValidation);
                            foreach (BeneficiaryTypeValidation Item in lstQstnValidation)
                            {
                                strQuery = @"INSERT INTO TbBeneficiaryQSTCondition(QuestionID,ValidationCondition,ConditionValue,MinValue,`MaxValue`,LogicOperator,CreatedBy,CreatedDate) 	
                                VALUES (@QuestionID,@ValidationCondition,@ConditionValue,@MinValue,@MaxValue,@LogicOperator,@CreatedBy,now());";

                                _connection.Execute(strQuery, new
                                {
                                    QuestionID = QID,
                                    ValidationCondition = Item.ValidationCondition,
                                    ConditionValue = Item.ConditionValue,
                                    MinValue = Item.MinValue,
                                    MaxValue = Item.MaxValue,
                                    LogicOperator = Item.LogicOperator,
                                    CreatedBy = loggedUserId
                                });
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error while deleting data";
                        throw ex;
                    }
                    _Tran.Complete();
                }

            }
            return retMsg;
        }

        //Tags

        /// <summary>
        /// Function used to save project beneficary tag based on BeneficiaryTypeID,TagID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="TagID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveProjectBeneficiaryTag(int BeneficiaryTypeID, int TagID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var id = 0;
            var retMsg = "success";

            using (_connection = Utils.Database.GetDBConnection())
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        strQuery = @"SELECT EXISTS(SELECT ID FROM TbBeneficiaryTypeTagDetails WHERE BeneficiaryTypeID=@BeneficiaryTypeID and TagID=@tagid);";
                        id = _connection.Query<int>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID, tagid = TagID }).Single();

                        if (id > 0)
                        {
                            strQuery = "Update TbBeneficiaryTypeTagDetails SET IsDeleted = @IsDeleted,UpdatedDate = @updatedDate,UpdatedBy =@loggedUserId ";
                            strQuery += " Where BeneficiaryTypeID =@BeneficiaryTypeID and TagID =@tagid";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, BeneficiaryTypeID = BeneficiaryTypeID, tagid = TagID, IsDeleted = 0 });
                        }
                        else
                        {
                            strQuery = "INSERT INTO TbBeneficiaryTypeTagDetails(BeneficiaryTypeID,TagID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted)";
                            strQuery += " VALUES(@BeneficiaryTypeID,@TagID,@CreatedDate,@loggedUserId,@CreatedDate,@loggedUserId,@IsDeleted); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID, TagID = TagID, loggedUserId = loggedUserId, CreatedDate = DateTime.Now, IsDeleted = 0 }).Single();
                        }
                    }
                    catch (Exception ex)
                    {
                        retMsg = "Error";
                        throw ex;
                    }
                    _Tran.Complete();
                }
                return retMsg;
            }
        }

        /// <summary>
        /// Function used to delete project beneficiary tag based on BeneficiaryTypeID
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteProjectBeneficiaryTag(int BeneficiaryTypeID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbBeneficiaryTypeTagDetails SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId ";
                    strQuery += " Where BeneficiaryTypeID =@BeneficiaryTypeID ";
                    _connection.Execute(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }

            return true;
        }

        /// <summary>
        /// Function used to check beneficiary registration id exists based on Answer
        /// </summary>
        /// <param name="Answer"></param>
        /// <returns></returns>
        public bool IsUniqueResponse(string Answer)
        {
            bool isExists = false;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "SELECT COUNT(BeneficiaryRegID) FROM TbBeneficiaryRegistrationDetails WHERE ";
                    strQuery += "Answer= @Answer";
                    int cnt = _connection.Query<int>(strQuery, new
                    {
                        Answer = Answer.ToLower()
                    }).Single();

                    if (cnt > 0)
                    {
                        isExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return isExists;
        }

        /// <summary>
        /// Function used to add beneficiary type questions
        /// </summary>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool CopyBeneficiaryQuestion(int BeneficiaryTypeID, string strQuestion, int loggedUserId)
        {
            var QID = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        int questionCount = _connection.Query<int>("select count(*) from TbBeneficiaryTypeQuestion where BeneficiaryTypeID = @BeneficiaryTypeID and  ifnull(IsActive, 0) = 1", new { BeneficiaryTypeID = BeneficiaryTypeID }).FirstOrDefault();

                        if (questionCount == null)
                        {
                            questionCount = 0;
                        }
                        else
                        {
                            questionCount = questionCount + 1;
                        }

                        List<BeneficiaryTypeQuestions> ObjeBenQstn = JsonConvert.DeserializeObject<List<BeneficiaryTypeQuestions>>(strQuestion);

                        string strQuery = "INSERT INTO TbBeneficiaryTypeQuestion(BeneficiaryTypeID,QuestionName,AnswerTypeID,IsUnique, IsSearchable, IsMandatoryResponse,QuestionHint, CreatedDate,CreatedBy,IsDeleted,IsActive,QuestionOrder) VALUES(@BeneficiaryTypeID,@QuestionName,@AnsID,@IsUnique, @IsSearchable, @IsMandatoryResponse, @QuestionHint, now(),@loggedUserId,0,1,@QuestionOrder); SELECT LAST_INSERT_ID();";
                        QID = _connection.Query<int>(strQuery, new
                        {
                            BeneficiaryTypeID = BeneficiaryTypeID,
                            QuestionName = ObjeBenQstn[0].QuestionName,
                            AnsID = ObjeBenQstn[0].AnswerTypeID,
                            IsUnique = ObjeBenQstn[0].IsUnique,
                            IsSearchable = ObjeBenQstn[0].IsSearchable,
                            IsMandatoryResponse = ObjeBenQstn[0].IsMandatoryResponse,
                            QuestionHint = ObjeBenQstn[0].QuestionHint,
                            loggedUserId = loggedUserId,
                            QuestionOrder = questionCount
                        }).Single();

                        if (ObjeBenQstn[0].BTQstnAnsOptionList != null)
                        {
                            //List<BeneficiaryTypeQstnAnsOptionList> lstQstnOpt = JsonConvert.DeserializeObject<List<BeneficiaryTypeQstnAnsOptionList>>(ObjeBenQstn[0].BTQstnAnsOptions);


                            foreach (BeneficiaryTypeQstnAnsOptionList Item in ObjeBenQstn[0].BTQstnAnsOptionList)
                            {
                                strQuery = @"INSERT INTO TbBeneficiaryTypeQstnAnswerOptions(BeneficiaryTypeID,QuestionID,AnswerTypeID,OptionValue,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted,IsActive) 	
                                VALUES (@BeneficiaryTypeID,@QuestionID,@AnswerTypeID,@OptionValue,now(),@loggedUserId,now(),@loggedUserId,0,1);";

                                _connection.Execute(strQuery, new
                                {
                                    BeneficiaryTypeID = BeneficiaryTypeID,
                                    @QuestionID = QID,
                                    AnswerTypeID = ObjeBenQstn[0].AnswerTypeID,
                                    OptionValue = Item.OptionValue,
                                    loggedUserId = loggedUserId
                                });
                            }
                        }
                        if (ObjeBenQstn[0].BTQstnValidationList != null)
                        {
                            //List<BeneficiaryTypeValidation> lstQstnValidation = JsonConvert.DeserializeObject<List<BeneficiaryTypeValidation>>(ObjeBenQstn[0].BTQstnValidation);
                            foreach (BeneficiaryTypeValidation Item in ObjeBenQstn[0].BTQstnValidationList)
                            {
                                strQuery = @"INSERT INTO TbBeneficiaryQSTCondition(QuestionID,ValidationCondition,ConditionValue,MinValue,`MaxValue`,LogicOperator,CreatedBy,CreatedDate) 	
                                VALUES (@QuestionID,@ValidationCondition,@ConditionValue,@MinValue,@MaxValue,@LogicOperator,@CreatedBy,now());";

                                _connection.Execute(strQuery, new
                                {
                                    QuestionID = QID,
                                    ValidationCondition = Item.ValidationCondition,
                                    ConditionValue = Item.ConditionValue,
                                    MinValue = Item.MinValue,
                                    MaxValue = Item.MaxValue,
                                    LogicOperator = Item.LogicOperator,
                                    CreatedBy = loggedUserId
                                });
                            }
                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }

            if (QID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion CreateProjectBeneficiary

        #region Delete

        /// <summary>
        /// Function used to delete beneficiary type based on ProjectID,BeneficiaryTypeID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="BeneficiaryTypeID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveBeneficiaryType(int ProjectID, int BeneficiaryTypeID, int loggedUserId)
        {
            var retMsg = "success";
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        try
                        {
                            string query = @"UPDATE TbProjectBeneficiaryType SET IsDeleted =1,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                            " where ProjectID =@ProjectID and BeneficiaryTypeID =@BeneficiaryTypeID";
                            _connection.Execute(query, new { ProjectID = ProjectID, BeneficiaryTypeID = BeneficiaryTypeID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                        }
                        catch (Exception ex)
                        {
                            retMsg = "Error while deleting data";
                            throw ex;
                        }

                        string strQuery = "Select Name  From TbProjectBeneficiaryType Where BeneficiaryTypeID =@BeneficiaryTypeID ";
                        var name = _connection.Query<string>(strQuery, new { BeneficiaryTypeID = BeneficiaryTypeID }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectID;
                        objLog.AuditFrom = "Beneficiary";
                        objLog.AuditEvent = "REMOVE";
                        objLog.AuditFromID = BeneficiaryTypeID;
                        objLog.FromIDColumnName = "BeneficiaryTypeID";
                        objLog.Message = "Removed Beneficiary Type named '" + name + "'.";
                        objLog.CreatedBy = loggedUserId;
                        ProjectDAL.AddAuditLog(objLog);

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        /// <summary>
        /// Function used to delete beneficiary based on BeneficiaryID
        /// </summary>
        /// <param name="BeneficiaryID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveBeneficiary(int BeneficiaryID, int loggedUserId)
        {
            var retMsg = "success";
            ProjectBeneficiaryType objProjectBeneficiaryType = new ProjectBeneficiaryType();
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        try
                        {
                            string strQuery = @"Select Distinct t.ProjectId as ProjectID,b.BeneficiaryName as Name From TbProjectBeneficiaryType t inner join TbProjectBeneficiary b on t.BeneficiaryTypeID = b.BeneficiaryTypeID and ifnull(b.IsActive,0) =1 And ifnull(b.IsDeleted,0) =0 and b.BeneficiaryID =@BeneficiaryID and ifnull(t.IsActive,0) =1 And ifnull(t.IsDeleted,0) =0";
                            objProjectBeneficiaryType = _connection.Query<ProjectBeneficiaryType>(strQuery, new
                            {
                                BeneficiaryID = BeneficiaryID
                            }).FirstOrDefault();

                            string query = @"UPDATE TbProjectBeneficiary SET IsDeleted =1,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                            " where BeneficiaryID =@BeneficiaryID";
                            _connection.Execute(query, new { BeneficiaryID = BeneficiaryID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                            query = @"UPDATE TbBeneficiaryRegistrationDetails SET IsDeleted =1,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                           " where BeneficiaryID =@BeneficiaryID";
                            _connection.Execute(query, new { BeneficiaryID = BeneficiaryID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });

                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = objProjectBeneficiaryType.ProjectID;
                            objLog.AuditFrom = "Beneficiary";
                            objLog.AuditEvent = "REMOVE";
                            objLog.AuditFromID = BeneficiaryID;
                            objLog.FromIDColumnName = "BeneficiaryID";
                            objLog.Message = "Removed Beneficiary  named '" + objProjectBeneficiaryType.Name + "'.";
                            objLog.CreatedBy = loggedUserId;
                            ProjectDAL.AddAuditLog(objLog);
                        }
                        catch (Exception ex)
                        {
                            retMsg = "Error while deleting data";
                            throw ex;
                        }
                        //string strQuery = "Select BeneficiaryName  From TbProjectBeneficiary Where BeneficiaryID =@BeneficiaryID ";
                        //var name = _connection.Query<string>(strQuery, new { BeneficiaryID = BeneficiaryID }).Single();
                        //strQuery = "Select Distinct t.ProjectId From TbProjectBeneficiaryType t inner join TbProjectBeneficiary b on t.BeneficiaryTypeID = b.BeneficiaryTypeID and ifnull(b.IsActive,0) =1 And ifnull(b.IsDeleted,0) =0 and b.BeneficiaryID =@BeneficiaryID and ifnull(t.IsActive,0) =1 And ifnull(t.IsDeleted,0) =0";
                        //var ProjectID = _connection.Query<int>(strQuery, new { BeneficiaryID = BeneficiaryID }).Single();

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        // function to Check is the BeneficiaryType is used in Survey
        // If it is used Deletion not works
        public bool CheckBeneficiaryTypeInSurvey(string lstBeneficiaryTypeID)
        {
            bool isExists = false;
            int id = 0;
            string BeneficiaryTypeIDList = string.Empty;

            try
            {
                BeneficiaryTypeIDList = "";
                List<ProjectBeneficiaryType> objBeneficiryType = JsonConvert.DeserializeObject<List<ProjectBeneficiaryType>>(lstBeneficiaryTypeID);
                foreach (ProjectBeneficiaryType obj in objBeneficiryType)
                {
                    BeneficiaryTypeIDList += obj.BeneficiaryTypeID + ",";
                }
                BeneficiaryTypeIDList = BeneficiaryTypeIDList.TrimEnd(',');

                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"select count(BeneficiaryTypeID) as id from TbProjectSurvey t where FIND_IN_SET(t.BeneficiaryTypeID,@BeneficiaryTypeIDList) and t.IsActive = 1;";
                    id = _connection.Query<int>(strQuery, new { BeneficiaryTypeIDList = BeneficiaryTypeIDList }).Single();

                    if (id > 0)
                    {
                        isExists = true;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return isExists;
        }
        /// <summary>
        /// Function used to delete beneficiary type questions based on QuestionID
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveBeneficiaryTypeQuestion(int QuestionID, int loggedUserId)
        {
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbBeneficiaryTypeQuestion SET IsDeleted =1,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId " +
                                    " where QuestionID =@QuestionID";
                    _connection.Execute(query, new { QuestionID = QuestionID, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
                catch (Exception ex)
                {
                    retMsg = "Error while deleting data";
                    throw ex;
                }

            }
            return retMsg;
        }
        #endregion Delete


        // API //

        public BeneficiarySurveySummary GetBeneSurveyDataAPI(int SyncTaskIDPass)
        {
            BeneficiarySurveySummary ObjProjectSurvSubmit = new BeneficiarySurveySummary();
            List<SurveyQuestions> ObjProjectSurQuestAnsw = new List<SurveyQuestions>();
            List<BeneficiaryQuestions> ObjProjectBenQuestAnsw = new List<BeneficiaryQuestions>();

            int SyncTaskID = 0;

            string strQuery, strQuerySur, strQueryBen = string.Empty;
            string StrUser = "";


            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQeury = @"SELECT max(ifnull(t.DataCollectionSyncDBID,0))'DataCollectionSyncDBID' FROM TbMyTask t WHERE t.SyncTaskID = @SyncTaskIDPass and t.IsActive = 1 and t.IsDeleted = 0";
                    var DCSyncDBID = _connection.Query<int>(strQeury, new { SyncTaskIDPass = SyncTaskIDPass }).FirstOrDefault();

                    dynamic obj;

                    if (DCSyncDBID != 0)
                    {
                        SyncTaskID = DCSyncDBID;
                    }
                    else
                    {
                        SyncTaskID = SyncTaskIDPass;
                    }


                    if (SyncTaskID != 0)
                    {
                        strQuery = @"select a.TaskSubmittedDate 'SubmittedDateTime',ifnull(u.Name,'') 'SubmittedBy' from (select max(t.TaskSubmittedDate)'TaskSubmittedDate',max(UserID)'UserID' from TbMyTask t where t.SyncTaskID = @SyncTaskID and t.IsActive = 1 and t.IsDeleted = 0) a inner join TbUser u on a.UserID = u.ID";
                        ObjProjectSurvSubmit = _connection.Query<BeneficiarySurveySummary>(strQuery, new { SyncTaskID = SyncTaskID }).FirstOrDefault();

                        string query = @"select ifnull(u.Name,'')'Name' from TbProjectWorkflowDetails w inner join (select b.*,s.WorkflowID,p.WfNoOfLevels from (select max(ifnull(a.WFLevel,0))'WFLevel',a.TypeID from TbMyTask a where a.Type = 'Survey' and a.TypeID in (select TypeID from TbMyTask t where t.SyncTaskID = @SyncTaskID and t.IsActive = 1) group by a.TypeID) b inner join TbProjectSurvey s on s.SurveyID = b.TypeID inner join TbProjectWorkflow p on p.WorkflowID = s.WorkflowID) c on w.WorkflowID = c.WorkflowID and w.IsDeleted = 0 inner join TbUser u on w.UserID = u.ID where c.WFLevel <> c.WfNoOfLevels and w.WfLevel = (c.WFLevel + 1)";
                        obj = _connection.Query<object>(query, new { SyncTaskID = SyncTaskID }).ToList();

                        foreach (dynamic item in obj)
                        {
                            if (StrUser == "")
                            {
                                StrUser = item.Name;
                            }
                            else
                            {
                                StrUser += "," + item.Name;
                            }
                        }
                        if (StrUser != "")
                        {
                            ObjProjectSurvSubmit.WorkflowLevelUsers = StrUser;
                        }



                        strQuerySur = @"select t.QuestionName,t.QuestionID,s.Answer from TbProjectSurveyQuestion t inner join TbProjectSurveyResponses s on s.QuestionID = t.QuestionID where t.IsActive = 1 and t.SurveyID = s.SurveyID and s.SyncTaskID = @SyncTaskID and s.IsActive = 1 order by t.QuestionOrder";
                        ObjProjectSurQuestAnsw = _connection.Query<SurveyQuestions>(strQuerySur, new { SyncTaskID = SyncTaskID }).ToList();

                        if (ObjProjectSurQuestAnsw.Count > 0)
                        {
                            ObjProjectSurvSubmit.SurveyQuestions = ObjProjectSurQuestAnsw;
                        }

                        strQueryBen = @"select a1.BeneficiaryName,a1.ResponseID'BeneficiaryId',a1.QuestionName,a1.QuestionID,r.Answer from (select aa.BeneficiaryName,aa.ResponseID,f.QuestionID,f.QuestionName from (select distinct t.ResponseID,b.BeneficiaryTypeID,b.BeneficiaryName from TbProjectSurveyResponses t inner join TbProjectBeneficiary b on t.ResponseID = b.BeneficiaryID where t.SyncTaskID = @SyncTaskID and t.IsActive = 1) aa inner join TbBeneficiaryTypeQuestion f on aa.BeneficiaryTypeID = f.BeneficiaryTypeID) a1 inner join TbBeneficiaryRegistrationDetails r on r.BeneficiaryID = a1.ResponseID and r.QuestionID = a1.QuestionID  order by r.QuestionID";
                        ObjProjectBenQuestAnsw = _connection.Query<BeneficiaryQuestions>(strQueryBen, new { SyncTaskID = SyncTaskID }).ToList();

                        if (ObjProjectBenQuestAnsw.Count > 0)
                        {
                            ObjProjectSurvSubmit.BeneficiaryQuestions = ObjProjectBenQuestAnsw;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ObjProjectSurvSubmit;
        }

        // API //

        #region GetDownloadSummaryForMyTask
        /// <summary>
        /// GetDownloadSummaryForMyTask
        /// </summary>
        /// <param name="SyncTaskID">SyncTaskID</param>
        /// <returns></returns>
        public DownloadMyTaskSummary GetDownloadSummaryForMyTask(int SyncTaskID)
        {
            DownloadMyTaskSummary ObjDownloadMyTaskSummary = new DownloadMyTaskSummary();
            List<SurveyQuestions> lstSurveyQuestions = new List<SurveyQuestions>();
            List<BeneficiaryQuestions> lstBeneficiaryQuestions = new List<BeneficiaryQuestions>();
            List<ResponseDetails> lstResponseDetails = new List<ResponseDetails>();
            List<DownloadSurveySectionLogic> lstRDownloadSurveySectionLogic = new List<DownloadSurveySectionLogic>();

            string joinedBeneficiaryIds = string.Empty;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = @"SELECT CASE WHEN  IFNULL(T.DataCollectionSyncDBID,0) = 0 THEN T.SyncTaskID ELSE T.DataCollectionSyncDBID END DataCollectionSyncDBID,";
                    strQuery += "T.TaskSubmittedDate SubmittedDateTime,IFNULL(U.Name,'') SubmittedBy,T.TypeID AS SurveyID,ifnull(sur.BeneficiaryTypeID,0) BeneficiaryTypeID ";
                    strQuery += ",ifnull(sur.WorkflowID,0) WorkflowID,ifnull(T.WFLevel,0) WFLevel FROM TbMyTask T JOIN TbUser U ON T.CreatedBy = U.ID JOIN TbProjectSurvey sur ON sur.SurveyID = T.TypeID ";
                    strQuery += " WHERE T.SyncTaskID = @SyncTaskID and T.IsActive = 1 and T.IsDeleted = 0;";

                    ObjDownloadMyTaskSummary = _connection.Query<DownloadMyTaskSummary>(strQuery, new { SyncTaskID = SyncTaskID }).FirstOrDefault();

                    if (ObjDownloadMyTaskSummary != null && ObjDownloadMyTaskSummary.SurveyID > 0)
                    {
                        string strQuerySectionLogic = @"SELECT SectionLogicID,SurveyID,SectionID,QuestionID,Answer FROM  TbProjectSurveySectionLogic WHERE SurveyID = @SurveyID";
                        lstRDownloadSurveySectionLogic = _connection.Query<DownloadSurveySectionLogic>(strQuerySectionLogic, new { SurveyID = ObjDownloadMyTaskSummary.SurveyID }).ToList();
                    }

                    if (ObjDownloadMyTaskSummary.WorkflowID > 0)
                    {
                        string strWorkflowQuery = @"SELECT GROUP_CONCAT(usr.Name) wflevelusers FROM TbProjectWorkflowDetails WFLevel JOIN TbUser usr ON WFLevel.UserID = usr.ID  ";
                        strWorkflowQuery += "WHERE WFLevel.WorkflowID =@WorkflowID AND ifnull(WFLevel.IsDeleted,0) = 0 AND ifnull(WFLevel.WfLevel,0) = @WfLevel;";
                        string workflowUsers = _connection.Query<string>(strWorkflowQuery, new { WorkflowID = ObjDownloadMyTaskSummary.WorkflowID, WfLevel = ObjDownloadMyTaskSummary.WFLevel + 1 }).FirstOrDefault();

                        if (!string.IsNullOrEmpty(workflowUsers))
                        {
                            ObjDownloadMyTaskSummary.WorkflowStageName = "WFLevel " + (ObjDownloadMyTaskSummary.WFLevel + 1);
                            ObjDownloadMyTaskSummary.WorkflowStageUser = workflowUsers;
                            ObjDownloadMyTaskSummary.WorkflowStatus = "WIP";
                        }
                        else
                        {
                            ObjDownloadMyTaskSummary.WorkflowStageName = "";
                            ObjDownloadMyTaskSummary.WorkflowStageUser = "";
                            ObjDownloadMyTaskSummary.WorkflowStatus = "Completed";
                        }
                    }

                    if (ObjDownloadMyTaskSummary != null && ObjDownloadMyTaskSummary.DataCollectionSyncDBID != 0)
                    {
                        string strSurQuery = @"SELECT srvRES.ResponseID BeneficiaryId,srvQST.QuestionID,srvQST.QuestionName,REPLACE(ifnull(srvRES.Answer,''), '\n', '') Answer,ifnull(srvQST.IsConditionQuestion,0) IsConditionQuestion ";
                        strSurQuery += ",srvQST.SectionID FROM TbProjectSurveyQuestion srvQST JOIN TbProjectSurveyResponses srvRES ON srvQST.QuestionID  = srvRES.QuestionID ";
                        strSurQuery += " WHERE srvRES.SyncTaskID = @SyncTaskID and srvRES.IsActive=1  order by srvRES.ID asc;";
                        lstSurveyQuestions = _connection.Query<SurveyQuestions>(strSurQuery, new { SyncTaskID = ObjDownloadMyTaskSummary.DataCollectionSyncDBID }).ToList();

                        ObjDownloadMyTaskSummary.ResponseDetails = new List<ResponseDetails>();

                        if (lstSurveyQuestions != null && lstSurveyQuestions.Count() > 0)
                        {
                            var mySKUs = lstSurveyQuestions.Select(l => l.BeneficiaryId).Distinct().ToList();
                            joinedBeneficiaryIds = string.Join(",", mySKUs);
                        }

                        if (ObjDownloadMyTaskSummary != null && ObjDownloadMyTaskSummary.BeneficiaryTypeID > 0)
                        {
                            string strBenQuery = @"SELECT BenReg.BeneficiaryID BeneficiaryId,BenQST.QuestionID,BenQST.QuestionName,REPLACE(ifnull(BenReg.Answer,''), '\n', '') Answer FROM TbBeneficiaryTypeQuestion BenQST  ";
                            strBenQuery += "JOIN TbBeneficiaryRegistrationDetails BenReg ON BenQST.QuestionID  = BenReg.QuestionID ";
                            strBenQuery += "WHERE BenQST.BeneficiaryTypeID = @BeneficiaryTypeID and  BenReg.BeneficiaryID in(" + joinedBeneficiaryIds + ") order by BeneficiaryID asc;";
                            lstBeneficiaryQuestions = _connection.Query<BeneficiaryQuestions>(strBenQuery, new { BeneficiaryTypeID = ObjDownloadMyTaskSummary.BeneficiaryTypeID }).ToList();

                            if (lstBeneficiaryQuestions != null && lstBeneficiaryQuestions.Count() > 0)
                            {
                                var uniqueBeneficiaryId = lstBeneficiaryQuestions.Select(p => p.BeneficiaryId).Distinct().ToList();

                                foreach (var itm in uniqueBeneficiaryId)
                                {
                                    ResponseDetails objResponseDetails = new ResponseDetails();
                                    objResponseDetails.BeneficiaryID = itm;

                                    List<BeneficiaryQuestions> lstFilteredBeneficiary = new List<BeneficiaryQuestions>();
                                    List<SurveyQuestions> lstFilteredSurveyQuestions = new List<SurveyQuestions>();
                                    List<SurveyQuestions> lstFilteredSurveyQuestionsfilter = new List<SurveyQuestions>();

                                    lstFilteredBeneficiary = lstBeneficiaryQuestions.Where(x => x.BeneficiaryId == itm).ToList();
                                    lstFilteredSurveyQuestions = lstSurveyQuestions.Where(x => x.BeneficiaryId == itm).ToList();
                                    objResponseDetails.BeneficiaryQuestions = lstFilteredBeneficiary;

                                    if (lstRDownloadSurveySectionLogic != null && lstRDownloadSurveySectionLogic.Count() > 0)
                                    {
                                        for (var k = 0; k < lstRDownloadSurveySectionLogic.Count(); k++)
                                        {
                                            List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                            lstSurveyQuestions1 = lstFilteredSurveyQuestions.Where(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID).ToList();

                                            if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                            {
                                                lstFilteredSurveyQuestions.RemoveAll(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID);

                                                SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                                objlstSurveyQuestions1 = lstFilteredSurveyQuestions.Where(x => x.QuestionId == lstRDownloadSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                                int index = lstFilteredSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                                for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                                {
                                                    lstFilteredSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                                }
                                            }
                                        }
                                    }

                                    objResponseDetails.SurveyQuestions = lstFilteredSurveyQuestions;
                                    ObjDownloadMyTaskSummary.ResponseDetails.Add(objResponseDetails);
                                }
                            }
                            else
                            {
                                ResponseDetails objResponseDetails = new ResponseDetails();
                                objResponseDetails.BeneficiaryID = 0;
                                objResponseDetails.BeneficiaryQuestions = null;

                                if (lstRDownloadSurveySectionLogic != null && lstRDownloadSurveySectionLogic.Count() > 0)
                                {
                                    for (var k = 0; k < lstRDownloadSurveySectionLogic.Count(); k++)
                                    {
                                        List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                        lstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID).ToList();

                                        if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                        {
                                            lstSurveyQuestions.RemoveAll(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID);

                                            SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                            objlstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.QuestionId == lstRDownloadSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                            int index = lstSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                            for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                            {
                                                lstSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                            }
                                        }
                                    }
                                }

                                objResponseDetails.SurveyQuestions = lstSurveyQuestions;
                                ObjDownloadMyTaskSummary.ResponseDetails.Add(objResponseDetails);
                            }
                        }
                        else
                        {
                            ResponseDetails objResponseDetails = new ResponseDetails();
                            objResponseDetails.BeneficiaryID = 0;
                            objResponseDetails.BeneficiaryQuestions = null;

                            if (lstRDownloadSurveySectionLogic != null && lstRDownloadSurveySectionLogic.Count() > 0)
                            {
                                for (var k = 0; k < lstRDownloadSurveySectionLogic.Count(); k++)
                                {
                                    List<SurveyQuestions> lstSurveyQuestions1 = new List<SurveyQuestions>();
                                    lstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID).ToList();

                                    if (lstSurveyQuestions1 != null && lstSurveyQuestions1.Count() > 0)
                                    {
                                        lstSurveyQuestions.RemoveAll(x => x.SectionID == lstRDownloadSurveySectionLogic[k].SectionID);

                                        SurveyQuestions objlstSurveyQuestions1 = new SurveyQuestions();
                                        objlstSurveyQuestions1 = lstSurveyQuestions.Where(x => x.QuestionId == lstRDownloadSurveySectionLogic[k].QuestionID).FirstOrDefault();

                                        int index = lstSurveyQuestions.IndexOf(objlstSurveyQuestions1);

                                        for (var j = 0; j < lstSurveyQuestions1.Count(); j++)
                                        {
                                            lstSurveyQuestions.Insert(index + j + 1, lstSurveyQuestions1[j]);
                                        }
                                    }
                                }
                            }

                            objResponseDetails.SurveyQuestions = lstSurveyQuestions;
                            ObjDownloadMyTaskSummary.ResponseDetails.Add(objResponseDetails);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ObjDownloadMyTaskSummary;
        }
        #endregion GetDownloadSummaryForMyTask


        #region UpdateQuestionOrder
        /// <summary>
        /// Function used to update Beneficiary question order based on QuestionID
        /// </summary>
        /// <param name="strQstnOrderList"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateQuestionOrder(string strQstnOrderList, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retMsg = "success";

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    List<BeneficiaryTypeQuestions> lstQstnOrder = JsonConvert.DeserializeObject<List<BeneficiaryTypeQuestions>>(strQstnOrderList);

                    if (lstQstnOrder != null)
                    {
                        foreach (BeneficiaryTypeQuestions Item in lstQstnOrder)
                        {
                            strQuery = @"UPDATE TbBeneficiaryTypeQuestion SET QuestionOrder =@QuestionOrder, UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where QuestionID =@QuestionID";
                            _connection.Execute(strQuery, new
                            {
                                @QuestionID = Item.QuestionID,
                                @QuestionOrder = Item.QuestionOrder,
                                updatedDate = DateTime.Now,
                                loggedUserId = loggedUserId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retMsg = "Error while updating data";
                throw ex;
            }

            return retMsg;

        }
        #endregion UpdateQuestionOrder

        #region GetBeneficiaryAnswerOption
        /// <summary>
        /// GetBeneficiaryAnswerOption
        /// </summary>
        /// <param name="QstnID"></param>
        /// <returns></returns>
        public IEnumerable<QuestionOptionList> GetBeneficiaryAnswerOption(int QstnID)
        {
            List<QuestionOptionList> lst = new List<QuestionOptionList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select QstnAnsOptionID as OptionID,OptionValue from TbBeneficiaryTypeQstnAnswerOptions where  QuestionID = @QuestionID  and  ifnull(IsActive,0)=1 and ifnull(IsDeleted,0) = 0;";
                    lst = _connection.Query<QuestionOptionList>(strQuery, new { QuestionID = QstnID }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        #endregion GetBeneficiaryAnswerOption
    }
}
