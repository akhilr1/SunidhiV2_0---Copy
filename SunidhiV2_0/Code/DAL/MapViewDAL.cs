using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using static SunidhiV2_0.Models.MapView;
using SunidhiV2_0.Code.Common;

namespace SunidhiV2_0.Code.DAL
{
    public class MapViewDAL
    {
        protected IDbConnection _connection;
        public MapView GetProjectandSurveyListforWorkspace(int WorkspaceId, string VersionUniqueID)
        {
            MapView objlstMapView = new MapView();
            List<Project_list> objProjectlist = new List<Project_list>();
            List<VersionData> objVersionData = new List<VersionData>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select distinct Project.ProjectId, ProjectName from TbProject Project inner join TbProjectSurvey Survey on  Project.ProjectId=Survey.ProjectId  inner join TbProjectSurveyQuestion Question on Survey.SurveyID= Question.SurveyID 
                  where WSID=@WorkspaceID and Question.QuestionTypeID=9 and ifnull(Project.IsActive,0) =1;";
                objProjectlist = _connection.Query<Project_list>(strQuery, new { WorkspaceId = WorkspaceId }).ToList();

                foreach (Project_list surveyItem in objProjectlist)
                {
                    surveyItem.Survey = new List<Survey>();

                    strQuery = @"select distinct Survey.SurveyID ,SurveyName from TbProjectSurvey Survey
                     inner join TbProjectSurveyQuestion Question on Survey.SurveyID= Question.SurveyID
                     inner join TbProjectSurveyResponses Responses on Responses.SurveyID=Question.SurveyID 
                     where ProjectId=@ProjectId and Question.QuestionTypeID=9 and ifnull(Survey.IsActive,0) =1;";

                    surveyItem.Survey = _connection.Query<Survey>(strQuery, new { ProjectId = surveyItem.ProjectID }).ToList();
                }
                objlstMapView.Project_list = objProjectlist;

                strQuery = @"SELECT distinct StateName,DistrictName,Preset,sub.ResponseCoordinates response_coordinates,Version.CreatedBy FROM TbMapViewSharelinkVersion Version 
                             inner join TbMapViewSharelinkVersionDetails Details on Version.ID=Details.VersionID  
                           left join TbMapViewSharelinkVersionDetailsSub sub on sub.VersionDetailID=Details.ID
                              where VersionUniqueId=@VersionUniqueID";
                objVersionData = _connection.Query<VersionData>(strQuery, new { VersionUniqueID = VersionUniqueID }).ToList();
                objlstMapView.VersionData = objVersionData;
            }
            return objlstMapView;
        }
        public bool SaveShareLinkVersion(SharelinkVersion inputObj)
        {
            string strQuery = string.Empty;
            var versionid = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"INSERT INTO TbMapViewSharelinkVersion(VersionName, VersionUniqueId, WorkspaceID,ProjectID,SurveyID, 
                                                             CreatedDate, CreatedBy,UpdatedBy, UpdatedDate )
                                            VALUES(@VersionName,@VersionUniqueId,@WorkspaceID,@ProjectID,@SurveyID,
                                                   @createdate,@createby,@UpdatedBy,@upddate); 
                                             SELECT LAST_INSERT_ID();";
                versionid = _connection.Query<int>(strQuery, new
                {
                    VersionName = inputObj.VersionName,
                    VersionUniqueId = inputObj.VersionUniqueId,
                    WorkspaceID = inputObj.WorkspaceID,
                    ProjectID = inputObj.ProjectID,
                    SurveyID = inputObj.SurveyID,
                    isdeleted = 0,
                    createdate = DateTime.Now,
                    createby = inputObj.loggedUserId,
                    upddate = DateTime.Now,
                    UpdatedBy = inputObj.loggedUserId,
                }).Single();

                if (versionid > 0)
                {
                    List<linkVersionDetails> lstVersionDetails = JsonConvert.DeserializeObject<List<linkVersionDetails>>(inputObj.linkVersionDetails);
                    string QuertyStmt = string.Empty;
                    if (lstVersionDetails != null)
                    {
                        foreach (linkVersionDetails item in lstVersionDetails)
                        {

                            strQuery = @"INSERT INTO TbMapViewSharelinkVersionDetails(VersionID,StateName, DistrictName, Preset,CreatedDate, CreatedBy )
                                  VALUES(@VersionID,@StateName, @DistrictName, @Preset,  now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                            var VersionDetailID = _connection.Query<int>(strQuery, new
                            {
                                VersionID = versionid,
                                StateName = item.StateName,
                                DistrictName = item.DistrictName,
                                Preset = item.Preset,
                                ResponseCoordinates = item.ResponseCoordinates,
                                CreatedBy = inputObj.loggedUserId,

                            }).Single();

                            if (VersionDetailID > 0)
                            {
                                List<ResponseCoordinates> lstResponseCoordinates = JsonConvert.DeserializeObject<List<ResponseCoordinates>>(item.ResponseCoordinates);
                                if (lstResponseCoordinates != null)
                                {
                                    foreach (ResponseCoordinates itemcoordinates in lstResponseCoordinates)
                                    {
                                        strQuery = @"INSERT INTO TbMapViewSharelinkVersionDetailsSub(VersionID,VersionDetailID,ResponseCoordinates,CreatedDate, CreatedBy )
                                  VALUES(@VersionID,@VersionDetailID, @ResponseCoordinates, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                                        var ID = _connection.Query<int>(strQuery, new
                                        {
                                            VersionID = versionid,
                                            VersionDetailID = VersionDetailID,
                                            ResponseCoordinates = itemcoordinates.Coordinates.ToString(),
                                            CreatedBy = inputObj.loggedUserId,

                                        }).Single();
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return true;
        }
        public bool UpdateShareLinkVersion(SharelinkVersion inputObj, int idversion)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        string strQuery = @"Update TbMapViewSharelinkVersion SET VersionName=@VersionName,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy WHERE VersionUniqueId = @VersionUniqueId";

                        var rr = _connection.Execute(strQuery, new
                        {
                            VersionName = inputObj.VersionName,
                            VersionUniqueId = inputObj.VersionUniqueId,
                            UpdatedDate = DateTime.Now,
                            UpdatedBy = inputObj.loggedUserId,

                        });
                        if (inputObj.VersionUniqueId != "")
                        {
                            bool success = DeleteSharelinkVersionDetails(idversion, inputObj.loggedUserId);
                            bool IsSuccess = DeleteSharelinkVersionCoordinates(idversion, inputObj.loggedUserId);
                            List<linkVersionDetails> lstVersionDetails = JsonConvert.DeserializeObject<List<linkVersionDetails>>(inputObj.linkVersionDetails);
                            string QuertyStmt = string.Empty;
                            if (lstVersionDetails != null)
                            {
                                foreach (linkVersionDetails item in lstVersionDetails)
                                {

                                    strQuery = @"INSERT INTO TbMapViewSharelinkVersionDetails(VersionID,StateName, DistrictName, Preset,CreatedDate, CreatedBy )
                                  VALUES(@VersionID,@StateName, @DistrictName, @Preset,  now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                                    var VersionDetailID = _connection.Query<int>(strQuery, new
                                    {
                                        VersionID = idversion,
                                        StateName = item.StateName,
                                        DistrictName = item.DistrictName,
                                        Preset = item.Preset,
                                        ResponseCoordinates = item.ResponseCoordinates,
                                        CreatedBy = inputObj.loggedUserId,

                                    }).Single();

                                    if (VersionDetailID > 0)
                                    {
                                        List<ResponseCoordinates> lstResponseCoordinates = JsonConvert.DeserializeObject<List<ResponseCoordinates>>(item.ResponseCoordinates);
                                        if (lstResponseCoordinates != null)
                                        {
                                            foreach (ResponseCoordinates itemcoordinates in lstResponseCoordinates)
                                            {
                                                strQuery = @"INSERT INTO TbMapViewSharelinkVersionDetailsSub(VersionID,VersionDetailID,ResponseCoordinates,CreatedDate, CreatedBy )
                                                            VALUES(@VersionID,@VersionDetailID, @ResponseCoordinates, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                                                var ID = _connection.Query<int>(strQuery, new
                                                {
                                                    VersionID = idversion,
                                                    VersionDetailID = VersionDetailID,
                                                    ResponseCoordinates = itemcoordinates.Coordinates.ToString(),
                                                    CreatedBy = inputObj.loggedUserId,

                                                }).Single();
                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        public List<StatewiseSurveyResponse> GetStateWiseSurveyResponse(int SurveyID, int ProjectID)
        {
            List<StatewiseSurveyResponse> objSurveyResponselist = new List<StatewiseSurveyResponse>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT survey.SurveyID, survey.SurveyName, 0 Response_Count, questions.QuestionTypeID, responses.Answer  
                                , '' AS StateName, 0 AS StateID
                                , MIN(responses.CreatedDate) OVER() AS ResponseStartDate, MAX(responses.CreatedDate) OVER() AS ResponseEndDate
                                , CASE WHEN survey.BeneficiaryTypeID > 0 THEN 'true' ELSE 'false' END AS IsBeneficiarySurvey, beneficiary.BeneficiaryName,
                            responses.SyncTaskID, ifnull(responses.SubmissionCount,1)  SubmissionCount 
                            FROM TbProjectSurveyResponses responses 
                            INNER JOIN TbProjectSurvey survey
                                ON survey.SurveyID = responses.SurveyID
                            INNER JOIN TbProjectSurveyQuestion questions
                                ON questions.QuestionID = responses.QuestionID
                            LEFT JOIN TbProjectBeneficiary beneficiary
                                ON beneficiary.BeneficiaryID = responses.ResponseID
                            WHERE responses.SurveyID = @SurveyID AND survey.ProjectId =@ProjectID
                            AND questions.QuestionTypeID = 9
                            AND TRIM(responses.Answer) IS NOT NULL AND LENGTH(TRIM(responses.Answer))> 10
                            AND INSTR(responses.Answer,'-') = 0
                            "; //-- StateName,StateID


                objSurveyResponselist = _connection.Query<StatewiseSurveyResponse>(strQuery, new { SurveyID = SurveyID, ProjectID = ProjectID }).ToList();

            }
            return objSurveyResponselist;
        }

        public List<DistrictwiseSurveyResponse> GetDistrictWiseSurveyResponse(int SurveyID)
        {
            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT survey.SurveyID, survey.SurveyName, 0 AS Response_Count, questions.QuestionTypeID, responses.Answer  
                                , '' AS DistrictName, 0 AS DistrictID
                                , MIN(responses.CreatedDate) OVER() AS ResponseStartDate, MAX(responses.CreatedDate) OVER() AS ResponseEndDate
                                , CASE WHEN survey.BeneficiaryTypeID > 0 THEN 'true' ELSE 'false' END AS IsBeneficiarySurvey, beneficiary.BeneficiaryName,
                            responses.SyncTaskID SyncDBID,ifnull(responses.SubmissionCount,1)  SubmissionCount                              
                            FROM TbProjectSurveyResponses responses 
                            INNER JOIN TbProjectSurvey survey
                                ON survey.SurveyID = responses.SurveyID
                            INNER JOIN TbProjectSurveyQuestion questions
                                ON questions.QuestionID = responses.QuestionID
                            LEFT JOIN TbProjectBeneficiary beneficiary
                                ON beneficiary.BeneficiaryID = responses.ResponseID
                            WHERE responses.SurveyID = @SurveyID 
                            AND questions.QuestionTypeID = 9
                            AND TRIM(responses.Answer) IS NOT NULL AND LENGTH(TRIM(responses.Answer))> 10
                            AND INSTR(responses.Answer,'-') = 0
                            ";


                objSurveyResponselist = _connection.Query<DistrictwiseSurveyResponse>(strQuery, new { SurveyID = SurveyID }).ToList();

            }
            return objSurveyResponselist;
        }

        /// <summary>
        /// Used to fetch district details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<District> GetDistrictList(string StateName)
        {
            List<District> lstDistrict = new List<District>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT DistrictID,DistrictName FROM TbDistrict dist INNER JOIN TbStates stat ON stat.StateID = dist.StateID  WHERE stat.StateName = @StateName Order by dist.DistrictID Desc;";
                    lstDistrict = _connection.Query<District>(strQuery, new { StateName = StateName }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstDistrict;
        }
        public string GetDistrictName(int DistrictID)
        {
            string DistrictName = "";
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT DistrictName FROM  TbDistrict where DistrictID=@DistrictID Order by DistrictID Desc";
                    DistrictName = _connection.Query<string>(strQuery, new { DistrictID = DistrictID }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return DistrictName;
        }
        public List<DistrictwiseSurveyResponse> GetResponseGeoCoordinate(int SurveyID)
        {
            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT survey.SurveyID, responses.Answer,SyncTaskID SyncDBID,responses.CreatedDate,ifnull(SubmissionCount,1) SubmissionCount 
                             FROM TbProjectSurveyResponses responses 
                            INNER JOIN TbProjectSurvey survey
                                ON survey.SurveyID = responses.SurveyID
                            INNER JOIN TbProjectSurveyQuestion questions
                                ON questions.QuestionID = responses.QuestionID
                            WHERE responses.SurveyID = @SurveyID and questions.QuestionTypeID=9";


                objSurveyResponselist = _connection.Query<DistrictwiseSurveyResponse>(strQuery, new { SurveyID = SurveyID }).ToList();

            }
            return objSurveyResponselist;
        }
        public List<PresetList> GetPivotPresetList(int SurveyID, string UserID)
        {
            List<PresetList> objPresetList = new List<PresetList>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"Select PresetName, PresetData from TbPivotPresetData where UserID=@UserID and SurveyID=@SurveyID";
                objPresetList = _connection.Query<PresetList>(strQuery, new { UserID = UserID, SurveyID = SurveyID }).ToList();
            }
            return objPresetList;
        }
        public List<VersionDetails> GetVersionDetails(int WorkspaceID, int UserID)
        {
            List<VersionDetails> objVersionDetails = new List<VersionDetails>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT VersionName,VersionUniqueId,MVS.CreatedDate,Name FROM TbMapViewSharelinkVersion MVS inner join 
                             TbUser User on MVS.CreatedBy=User.ID where MVS.CreatedBy=@UserID and WorkspaceID=@WorkspaceID and User.IsActive = 1 and User.IsDeleted = 0;";
                objVersionDetails = _connection.Query<VersionDetails>(strQuery, new { UserID = UserID, WorkspaceID = WorkspaceID }).ToList();
            }
            return objVersionDetails;
        }
        public int IsVersionExists(string VersionUniqueId)
        {
            int VersionID = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    string strQuery = "SELECT IFNULL(( SELECT ID FROM TbMapViewSharelinkVersion where VersionUniqueId =@VersionUniqueId), 0) AS ID ";

                    VersionID = _connection.Query<int>(strQuery, new
                    {
                        VersionUniqueId = VersionUniqueId
                    }).Single();


                }
            }
            catch (Exception ex)
            {
            }
            return VersionID;
        }
        public bool DeleteSharelinkVersionDetails(int VersionID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbMapViewSharelinkVersionDetails SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId  Where VersionID =@VersionID ";
                    _connection.Execute(strQuery, new { VersionID = VersionID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        public bool DeleteSharelinkVersionCoordinates(int VersionID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbMapViewSharelinkVersionDetailsSub SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId  Where VersionID =@VersionID ";
                    _connection.Execute(strQuery, new { VersionID = VersionID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        public List<Geo_coordinate_Details> GetGeoCoordinateDetails(int SyncTaskID)
        {
            List<Geo_coordinate_Details> objGeoCoordinateDetails = new List<Geo_coordinate_Details>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"select QuestionName,Answer,QuestionTypeID from TbProjectSurveyResponses Responses
                             inner join TbProjectSurveyQuestion Question on Responses.QuestionID= Question.QuestionID where 
                             QuestionTypeID =9 and SyncTaskID=@SyncTaskID and Question.IsActive=1";


                objGeoCoordinateDetails = _connection.Query<Geo_coordinate_Details>(strQuery, new { SyncTaskID = SyncTaskID }).ToList();

            }
            return objGeoCoordinateDetails;
        }
        public VersionDataIDs GetVersionDetailsByVersionUniqueID(string VersionUniqueID)
        {
            VersionDataIDs objIDs = new VersionDataIDs();
            List<VersionData> objVersionData = new List<VersionData>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT ID VersionID,VersionName,ProjectID,SurveyID,CreatedBy
                              FROM TbMapViewSharelinkVersion Version 
                              where VersionUniqueId=@VersionUniqueID and IsActive=1";

                objIDs = _connection.Query<VersionDataIDs>(strQuery, new
                {
                    VersionUniqueID = VersionUniqueID
                }).Single();

                if (objIDs.VersionID > 0)
                {


                    strQuery = @"SELECT distinct StateName,DistrictName,Preset,sub.ResponseCoordinates response_coordinates
                               from TbMapViewSharelinkVersionDetails Details
                               left join TbMapViewSharelinkVersionDetailsSub sub on sub.VersionDetailID=Details.ID
                               where Details.VersionID=@VersionDataID and IFNULL(Details.IsDeleted,0) = 0 and IFNULL(sub.IsDeleted,0) = 0";


                    objIDs.VersionData = _connection.Query<VersionData>(strQuery, new { VersionDataID = objIDs.VersionID }).ToList();
                }
            }
            return objIDs;
        }

        #region GetIndividualResponseData
        /// <summary>
        /// GetIndividualResponseData
        /// </summary>
        /// <param name="SyncTaskID"></param>
        /// <param name="SubmissionCount"></param>
        /// <returns></returns>
        public ShareMapSurvey GetIndividualResponseData(int SyncTaskID, int SurveyID, int SubmissionCount)
        {
            ShareMapSurvey objProjectSurvey = new ShareMapSurvey();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string qry = "select SurveyID,SurveyName,SurveyDescription from TbProjectSurvey where SurveyID  = @SurveyID";
                    objProjectSurvey = _connection.Query<ShareMapSurvey>(qry,
                       new { SurveyID = SurveyID }).FirstOrDefault();

                    List<ShareMapSurveySection> lstProjectSurveySection = new List<ShareMapSurveySection>();
                    string qry1 = "select sec.SectionID,SectionName from TbProjectSurveySection sec ";
                    qry1 += "LEFT JOIN TbProjectSurveySectionLogic LG  ON sec.SectionID = LG.SectionID ";
                    qry1 += "WHERE sec.SurveyID = @SurveyID and sec.IsActive = 1 and ifnull(LG.SectionLogicID,0) = 0 order by SectionOrder asc;";
                    lstProjectSurveySection = _connection.Query<ShareMapSurveySection>(qry1,
                       new { SurveyID = SurveyID }).ToList();

                    List<SurveyQuestionResponse> lstSurveyQuestion = new List<SurveyQuestionResponse>();

                    string qry2 = "select QST.QuestionName,RSP.Answer,ifnull(QST.IsConditionQuestion,0) IsConditionQuestion,QST.QuestionID,QST.SectionID,TYPE.Type as QuestionType from TbProjectSurveyQuestion QST JOIN  ";
                    qry2 += "TbProjectSurveyResponses RSP ON QST.QuestionID = RSP.QuestionID JOIN TbQuestionType TYPE ON TYPE.ID = QST.QuestionTypeID ";
                    qry2 += " where ifnull(QST.IsActive,0) = 1 and ifnull(RSP.IsActive,0) = 1 ";
                    qry2 += "and  QST.SurveyID = @SurveyID and SyncTaskID =@SyncTaskID and ifnull(RSP.SubmissionCount,0) =@SubmissionCount order by QST.QuestionOrder asc;";
                    lstSurveyQuestion = _connection.Query<SurveyQuestionResponse>(qry2,
                       new { SurveyID = SurveyID, SyncTaskID = SyncTaskID, SubmissionCount = SubmissionCount }).ToList();

                    List<SurveySectionLogic> lstSurveySectionLogic = new List<SurveySectionLogic>();

                    string qry3 = "select SectionID,QuestionID,LogicType from TbProjectSurveySectionLogic WHERE SurveyID  = @SurveyID AND IFNULL(IsActive,0) = 1  ";
                    lstSurveySectionLogic = _connection.Query<SurveySectionLogic>(qry3,
                       new { SurveyID = SurveyID }).ToList();

                    if (objProjectSurvey != null)
                    {
                        if (lstProjectSurveySection != null && lstProjectSurveySection.Count > 0)
                        {
                            objProjectSurvey.SurveySectionList = lstProjectSurveySection;

                            if (objProjectSurvey.SurveySectionList != null && objProjectSurvey.SurveySectionList.Count > 0)
                            {
                                foreach (var itm in objProjectSurvey.SurveySectionList)
                                {
                                    List<SurveyQuestionResponse> lstSurveyQuestionFilter = new List<SurveyQuestionResponse>();
                                    List<SurveyQuestionResponse> lstSurveyQuestionskipFilter = new List<SurveyQuestionResponse>();
                                    lstSurveyQuestionFilter = lstSurveyQuestion.Where(x => x.SectionID == itm.SectionID).ToList();
                                    lstSurveyQuestionskipFilter = lstSurveyQuestionFilter.Where(x => x.IsConditionQuestion == true).ToList();

                                    if (lstSurveySectionLogic != null && lstSurveySectionLogic.Count > 0 &&
                                        lstSurveyQuestionskipFilter != null && lstSurveyQuestionskipFilter.Count > 0)
                                    {
                                        foreach (var itm1 in lstSurveySectionLogic)
                                        {
                                            List<SurveyQuestionResponse> lstSurveyQuestion1 = new List<SurveyQuestionResponse>();
                                            lstSurveyQuestion1 = lstSurveyQuestion.Where(x => x.SectionID == itm1.SectionID).ToList();
                                            int index = lstSurveyQuestionFilter.FindIndex(x => x.QuestionID == itm1.QuestionID);
                                            lstSurveyQuestionFilter.InsertRange(index + 1, lstSurveyQuestion1);
                                        }
                                    }
                                    itm.SurveyQuestionList = lstSurveyQuestionFilter;
                                }


                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }

            return objProjectSurvey;
        }
        #endregion GetIndividualResponseData

        #region GetShareVersionCordinates
        /// <summary>
        /// GetShareVersionCordinates
        /// </summary>
        /// <param name="versionUniqueId"></param>
        /// <returns></returns>
        public string GetShareVersionCordinates(string versionUniqueId)
        {
            string cordinates = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "select  GROUP_CONCAT(DISTINCT SUB.ResponseCoordinates SEPARATOR '|') from   TbMapViewSharelinkVersionDetailsSub SUB ";
                    query += " JOIN TbMapViewSharelinkVersionDetails DET ON  SUB.VersionDetailID = DET.ID JOIN TbMapViewSharelinkVersion VER ";
                    query += " ON VER.ID = DET.VersionID where VER.VersionUniqueId  = @VersionUniqueId  and ifnull(SUB.IsDeleted,0) = 0;";
                    cordinates = _connection.Query<string>(query, new { VersionUniqueId = versionUniqueId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return cordinates;
        }
        #endregion GetShareVersionCordinates

        #region GetSurveyResponseLocation
        /// <summary>
        /// GetSurveyResponseLocation
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public List<SurveyResponseGPSLocation> GetSurveyResponseLocation(int SurveyId)
        {
            string cordinates = string.Empty;
            List<SurveyResponseGPSLocation> lstSurveyResponseGPSLocation = new List<SurveyResponseGPSLocation>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "select SurveyID,SyncTaskID,State,District,SubmissionCount,GPS from TbProjectSurveyResponseLocation where SurveyID=@SurveyID";
                    lstSurveyResponseGPSLocation = _connection.Query<SurveyResponseGPSLocation>(query, new { SurveyID = SurveyId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstSurveyResponseGPSLocation;
        }
        #endregion GetSurveyResponseLocation



        #region SaveSurveyGPSBYSurveyID
        /// <summary>
        /// SaveSurveyGPSBYSurveyID
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public bool SaveSurveyGPSBYSurveyID(int SurveyId)
        {
            bool success = true;

            string cordinates = string.Empty;
            List<SurveyResponseGPSLocation> lstSurveyResponseGPSLocation = new List<SurveyResponseGPSLocation>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "SELECT  distinct QST.SurveyID,RESP.Answer as GPS,RESP.SyncTaskID,ifnull(RESP.SubmissionCount,1) SubmissionCount  FROM TbProjectSurveyResponses RESP JOIN TbProjectSurveyQuestion QST ON RESP.QuestionID = QST.QuestionID";
                    query += " WHERE  QST.SurveyID = @SurveyID and IFNULL(QST.IsActive,0)= 1 AND QST.QuestionTypeID = @QuestionTypeID AND ifnull(RESP.Answer,'') !='' and ifnull(RESP.Answer,'') != '0,0'";
                    lstSurveyResponseGPSLocation = _connection.Query<SurveyResponseGPSLocation>(query, new { SurveyID = SurveyId, QuestionTypeID = (int)CommonActivity.QuestionType.GPS_Capture }).ToList();
                }

                if (lstSurveyResponseGPSLocation != null && lstSurveyResponseGPSLocation.Count > 0)
                {
                    new MobileWebServiceDAL().InsertResposeGPSLocation(lstSurveyResponseGPSLocation);
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        #endregion SaveSurveyGPSBYSurveyID
    }
}
