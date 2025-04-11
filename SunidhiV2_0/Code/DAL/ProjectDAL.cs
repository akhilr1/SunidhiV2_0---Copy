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
    public class ProjectDAL
    {
        protected IDbConnection _connection;
        static protected IDbConnection _conn;

        #region CreateProject

        /// <summary>
        /// Next Project Code -> -> function used to increment Project Code with maximum 6 length based on ProjectId
        /// </summary>
        /// <returns></returns>
        public string GetNextProjectCode()
        {
            int nextID=0;
            string nextCode ="";
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT Max(ProjectId) From TbProject ";
                    nextID = _connection.ExecuteScalar<int>(strQuery);
                    nextID++;
                    var n = nextID.ToString().Length;
                    string Code = n == 5 ? "P" : (n == 4 ? "P0" : (n == 3 ? "P00" : (n == 2 ? "P000" : (n == 1 ? "P0000" : "P00001"))));
                    nextCode = Code + nextID.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return nextCode;
        }

        /// <summary>
        /// Used to fetch country details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Country> GetCountryDetails()
        {
            List<Country> lstItems = new List<Country>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct CountryID, CountryName From TbCountry  Order By CountryName ";
                    lstItems = _connection.Query<Country>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        /// <summary>
        /// Used to fetch state details based on CountryID
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns></returns>
        public IEnumerable<State> GetStateDetails(int CountryID)
        {
            List<State> lstItems = new List<State>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct StateID, StateName From TbStates Where CountryID =@CountryID  Order By StateName "; ;
                    lstItems = _connection.Query<State>(strQuery, new { CountryID = CountryID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        /// <summary>
        /// Used to fetch district details based on StateID
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IEnumerable<District> GetDistrictDetails(int StateID)
        {
            List<District> lstItems = new List<District>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct DistrictID, DistrictName From TbDistrict Where StateID = @StateID  Order By DistrictName ";
                    lstItems = _connection.Query<District>(strQuery, new { StateID = StateID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        /// <summary>
        /// Get Project Themelist -> function used to fetch project themelist
        /// </summary>
        /// <param name="InitiativeID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTemplate> GetProjectThemeList(int InitiativeID)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //-------- Old Query 
                    //strQuery = @"SELECT Distinct IP.ThemeID ,T.ThemeName Theme,Count(case when ProjectTemplateID =0 then null else ProjectTemplateID end) NoOfTemplates ,0 IsSelected ,T.ThemeImage as ThemeImage, IP.InitiativeId as InitiativeId
                    //            From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID
                    //            Where IP.IsDeleted =0 And IP.InitiativeId = @InitiativeID  Group By IP.ThemeID ,T.ThemeName";
                    // lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { InitiativeID = InitiativeID }).ToList();
                    //=-------------- Changed on 17-11-2020 -- removed Initiative Id conditions

                    strQuery = @"Select Distinct T.ThemeID ,T.ThemeName Theme,Count(case when PT.ProjectTemplateID =0 then null else PT.ProjectTemplateID end) NoOfTemplates ,
                                0 IsSelected ,T.ThemeImage as ThemeImage From TbProjectTheme T
                        left join TbProjectTemplate PT on PT.ThemeID = T.ThemeID Group By T.ThemeID ,T.ThemeName";
                    lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstPrjTemp;
        }

        /// <summary>
        /// Get Project Themelist -> function used to fetch project themelist based on ThemeID
        /// </summary>
        /// <param name="InitiativeID"></param>
        /// <param name="ThemeID"></param>
        /// <param name="NoOfTemplates"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTemplate> GetProjectTemplateList(int InitiativeID, int ThemeID, int NoOfTemplates)
        {
            List<ProjectTemplate> lstPrjTemp = new List<ProjectTemplate>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //if (NoOfTemplates != 0)
                    //{
                    //    strQuery = @"SELECT Distinct IP.ThemeID ,PT.ProjectTemplateID ProjectTemplateID,ProjectTemplateName TemplateName,Description ,0 IsSelected, IFNULL(T.ThemeImage, '') as ThemeImage,IP.InitiativeId as InitiativeId From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID left join  TbProjectTemplate PT On IP.ProjectTemplateID= PT.ProjectTemplateID Where IP.ThemeID =@ThemeID  And IP.InitiativeId =@InitiativeID";
                    //}
                    //else
                    //{
                    //    strQuery = @"SELECT Distinct T.ThemeID ,T.ThemeName Theme ,PT.ProjectTemplateID ProjectTemplateID,
                    //                ProjectTemplateName TemplateName,Description ,0 IsSelected,IFNULL(T.ThemeImage, '') as ThemeImage From TbProjectTemplate PT
                    //                inner join TbProjectTheme T On T.ThemeID = PT.ThemeID Where PT.IsActive =1 And PT.ThemeID =@ThemeID";
                    //}
                    //lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { ThemeID = ThemeID, InitiativeID = InitiativeID }).ToList();

                    //---- //-- changed on 17-11-2020 -- Select template from theme .avoided initiative id
                    strQuery = @"select Distinct T.ThemeID,PT.ProjectTemplateID,PT.ProjectTemplateName TemplateName, PT.Description, 0 IsSelected,IFNULL(T.ThemeImage, '') as ThemeImage, 0 as InitiativeId from TbProjectTemplate PT inner join TbProjectTheme T on T.ThemeID = PT.ThemeID where PT.ThemeID = @ThemeID and PT.IsActive = 1";
                    lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { ThemeID = ThemeID }).ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return lstPrjTemp;
        }

        /// <summary>
        /// Add new project
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <param name="ProjectDescription"></param>
        /// <param name="ProjectCode"></param>
        /// <param name="ProjectBudget"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="CountryID"></param>
        /// <param name="StateID"></param>
        /// <param name="DistrictID"></param>
        /// <param name="ProjectTags"></param>
        /// <param name="ProjectTemplateID"></param>
        /// <param name="InitiativeID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string CreateProject(string ProjectName, string ProjectDescription, string ProjectCode, double ProjectBudget, DateTime StartDate, DateTime EndDate,
                   int CountryID, int StateID, int DistrictID, string ProjectTags, int ProjectTemplateID, int InitiativeID, int loggedUserId)
        {
            string strQuery = string.Empty;            
            var retStatus ="";

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = "sp_CreateProject";
                    retStatus = _connection.ExecuteScalar<string>(strQuery, new
                    {
                        INProjectName = ProjectName,
                        INProjectDescription = ProjectDescription,
                        INProjectCode = ProjectCode,
                        INProjectBudget = ProjectBudget,
                        INStartDate = StartDate,
                        INEndDate = EndDate,
                        INCountryID = CountryID,
                        INStateID = StateID,
                        INDistrictID = DistrictID,
                        INProjectTags = ProjectTags,
                        INProjectTemplateID = ProjectTemplateID,
                        INInitiativeID = InitiativeID,                        
                        loggedUserId = loggedUserId
                    }, commandType: CommandType.StoredProcedure);
                }

            }

            catch (Exception ex)
            {
                retStatus = "Failed while saving new project...";
                throw ex;
            }
            return retStatus;
        }


        #endregion CreateProject

        #region Document

        /// <summary>
        /// Used to fetch project name based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetProjectName(int  ProjectID)
        {
           
            string ProjectName = "";
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT ProjectName From TbProject Where ProjectId=@ProjectID ";
                    ProjectName = _connection.ExecuteScalar<string>(strQuery, new { ProjectID = ProjectID });
                }
            }
            catch (Exception ex)
            {
            }
            return ProjectName;
        }

        /// <summary>
        /// Used to fetch document details based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<Document> GetAllDocumentList(int ProjectID)
        {
            List<Document> lstDoc = new List<Document>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //strQuery = @"Select  DocumentID, UniqueDocumentName,DocumentName, DocumentOriginalName, DocumentSize, DocumentTypeIcon, DocumentType, " +
                    //                " DocumentLastModifiedDate, UploadedDate, UploadedBy, Name UploadedUser,ifnull(U.ProfileImage,'') as ProfileImage, UploadedFrom, ProjectId , LinkedTo " +
                    //                " From TbDocument D Inner Join TbUser U On D.UploadedBy =U.ID Where D.IsDeleted =0 And ProjectId =@ProjectID Order by DocumentID Desc";

                    strQuery = @"Select D.DocumentID, D.UniqueDocumentName,D.DocumentName, D.DocumentOriginalName, D.DocumentSize, D.DocumentTypeIcon, D.DocumentType,  
                                 D.DocumentLastModifiedDate, D.UploadedDate, D.UploadedBy, U.Name UploadedUser,ifnull(U.ProfileImage,'') as ProfileImage, 
                                 D.UploadedFrom, D.ProjectId , D.LinkedTo From TbDocument D Inner Join TbUser U On D.UploadedBy =U.ID Where D.IsDeleted =0 And 
                                 D.ProjectId = @ProjectID Order by D.DocumentID Desc";

                    lstDoc = _connection.Query<Document>(strQuery, new { ProjectID = ProjectID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstDoc;
        }

        /// <summary>
        /// Add new document details
        /// </summary>
        /// <param name="docName"></param>
        /// <param name="docOrginalName"></param>
        /// <param name="DocumentSize"></param>
        /// <param name="DocumentTypeIcon"></param>
        /// <param name="DocumentType"></param>
        /// <param name="DocumentLastModifiedDate"></param>
        /// <param name="ProjectId"></param>
        /// <param name="UploadedFrom"></param>
        /// <param name="LinkedTo"></param>
        /// <param name="LinkedToID"></param>
        /// <param name="TaskSyncID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveDocumentDetails(string docName, string docOrginalName,  string DocumentSize, string DocumentTypeIcon, string DocumentType,
            string DocumentLastModifiedDate, int ProjectId,string UploadedFrom, string LinkedTo, int LinkedToID,int TaskSyncID,int loggedUserId)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "INSERT INTO TbDocument(UniqueDocumentName,DocumentName, DocumentOriginalName, DocumentSize, DocumentTypeIcon, DocumentType, " +
                                    " DocumentLastModifiedDate, UploadedDate, UploadedBy, UploadedFrom,CreatedDate, CreatedBy, ProjectId ,IsDeleted,LinkedTo,LinkedToID,TaskSyncID)" +
                                    " VALUES(@docName,@DocumentName,@docOrginalName,@DocumentSize,@DocumentTypeIcon,@DocumentType,@DocumentLastModifiedDate," +
                                    " now(),@loggedUserId,@UploadedFrom,now(),@loggedUserId,@ProjectId,0,@LinkedTo,@LinkedToID,@TaskSyncID);SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(strQuery, new
                    {
                        docName = docName,
                        DocumentName = docOrginalName,
                        docOrginalName = docOrginalName,
                        DocumentSize = DocumentSize,
                        DocumentTypeIcon = DocumentTypeIcon,
                        DocumentType = DocumentType,
                        DocumentLastModifiedDate = DocumentLastModifiedDate,
                        loggedUserId = loggedUserId,
                        ProjectId = ProjectId,
                        UploadedFrom = UploadedFrom,
                        LinkedTo= LinkedTo,
                        LinkedToID = LinkedToID,
                        TaskSyncID = TaskSyncID
                    }).Single();
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project document...";
                    throw ex;
                }
            }
            return retStatus;
        }

        /// <summary>
        /// Add new response document details
        /// </summary>
        /// <param name="docName"></param>
        /// <param name="DocumentTypeIcon"></param>
        /// <param name="ProjectId"></param>
        /// <param name="ResponseFrom"></param>
        /// <param name="UploadedFrom"></param>
        /// <param name="LinkedTo"></param>
        /// <param name="LinkedToID"></param>
        /// <param name="TaskSyncID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveResponseDocumentDetails(string docName, string DocumentTypeIcon, int ProjectId,string ResponseFrom, string UploadedFrom, 
                string LinkedTo,int LinkedToID, int TaskSyncID, int loggedUserId)
        {
           
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "INSERT INTO TbResponsesDocument(DocumentName, DocumentTypeIcon,ResponseFrom, UploadedDate, UploadedBy, UploadedFrom,CreatedDate, CreatedBy,ProjectId ,LinkedTo,LinkedToID,TaskSyncID)" +
                                    " VALUES(@docName,@DocumentTypeIcon,@ResponseFrom,now(),@loggedUserId,@UploadedFrom,now(),@loggedUserId,@ProjectId,@LinkedTo,@LinkedToID,@TaskSyncID);SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(strQuery, new
                    {
                        docName = docName,                       
                        DocumentTypeIcon = DocumentTypeIcon,
                        ResponseFrom = ResponseFrom,                        
                        loggedUserId = loggedUserId,
                        ProjectId = ProjectId,
                        UploadedFrom = UploadedFrom,
                        LinkedTo= LinkedTo,
                        LinkedToID= LinkedToID,
                        TaskSyncID= TaskSyncID

                    }).Single();
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project document...";
                    throw ex;
                }
            }
            return retStatus;
        }

        /// <summary>
        /// Delete Document -> function used to delete document based on docID
        /// </summary>
        /// <param name="docID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string DeleteDocument(int docID, int loggedUserId)
        {
            var retMsg = "";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbDocument SET IsDeleted =@IsDeleted,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where DocumentID =@docID";
                    _connection.Execute(query, new { IsDeleted = true, updatedDate = DateTime.Now, docID = docID, loggedUserId = loggedUserId });
                }
                catch (Exception ex)
                {
                    retMsg = "Error while saving data";
                    throw ex;
                }
               
            }
            return retMsg;
        }

        /// <summary>
        /// Rename Document -> function used to change document name based on docID
        /// </summary>
        /// <param name="docID"></param>
        /// <param name="newFileName"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RenameDocument(int docID, string newFileName ,int loggedUserId)
        {
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    string query = @"UPDATE TbDocument SET DocumentName =@newFileName,UpdatedDate = @updatedDate,UpdatedBy=@loggedUserId where DocumentID =@docID";
                    _connection.Execute(query, new { newFileName = newFileName, updatedDate = DateTime.Now, docID = docID, loggedUserId = loggedUserId });
                }
                catch (Exception ex)
                {
                    retMsg = "Error while updating document name";
                    throw ex;

                }
                return retMsg;
            }
        }

        #endregion Document

        #region Finance_BudgetAndFunds

        /// <summary>
        /// Used to fetch project budget details based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBudgetAndFundsDetails> GetProjectBudgetDetails(int ProjectID)
        {
            List<ProjectBudgetAndFundsDetails> lstBudgetInfo = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"Select Distinct P.ProjectID, P.GroupId, Budget,PB.ID BudgetFundID,PB.UserID, PB.UserType, U.Name UserName, U.EmailId,DonorBudgetAmount, PB.AccountId, AccountName " +
                    //            " From TbProject P Left Join TbProjectBudgetAndFundDetails PB On PB.ProjectID = P.ProjectId and PB.IsDeleted =0 Left Join TbAccount A On A.AccountId= PB.AccountId " +
                    //            " Left join TbUser U On U.ID =PB.UserID  Where P.IsActive =1 and P.IsDeleted =0 And P.ProjectId =@ProjectID ";

                    strQuery = @"Select Distinct P.ProjectID, P.GroupId, P.Budget,PB.ID BudgetFundID,PB.UserID, PB.UserType, U.Name UserName, U.EmailId,PB.DonorBudgetAmount, PB.AccountId, A.AccountName 
                                 From TbProject P Left Join TbProjectBudgetAndFundDetails PB On PB.ProjectID = P.ProjectId and PB.IsDeleted =0 Left Join TbAccount A On A.AccountId= PB.AccountId 
                                 Left join TbUser U On U.ID =PB.UserID Where P.IsActive =1 and P.IsDeleted =0 And P.ProjectId = @ProjectID";

                    lstBudgetInfo = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { ProjectID = ProjectID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBudgetInfo;
        }

        /// <summary>
        /// Used to fetch project users based on GroupID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBudgetAndFundsDetails> GetProjectUsers(int GroupID, string Type)
        {
            List<ProjectBudgetAndFundsDetails> lstUsers = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (Type == "Finance")
                        //strQuery = @"Select Distinct TG.UserID ,Name UserName,EmailId,'F' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,IFNULL(ProfileImage, '') as ProfileImage " +
                        //            " from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID " +
                        //            " Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 " +
                        //            " And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='My Finance'))R On R.RoleId = W.RoleID Where GroupId =@GroupID ";

                        strQuery = @"Select Distinct TG.UserID ,U.Name UserName,U.EmailId,'F' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,
                                    IFNULL(U.ProfileImage, '') as ProfileImage from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID 
                                     Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT b.RoleId  FROM TbRoleBasedAccessScreens b inner join TbScreen s on 
                                      s.ScreenName ='My Finance' and b.ScreenID = s.ScreenId Where b.AccessId =3)R On R.RoleId = W.RoleID Where TG.GroupId =@GroupID";

                    else if (Type == "Donor")
                        //strQuery = @"Select Distinct TG.UserID ,Name UserName,EmailId,'D' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,IFNULL(ProfileImage, '') as ProfileImage  from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID " +
                        //            " Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 " +
                        //            " And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='Venture'))R On R.RoleId = W.RoleID Where GroupId =@GroupID ";

                        strQuery = @"Select Distinct TG.UserID ,U.Name UserName,U.EmailId,'D' UserType,0 DonorBudgetAmount,null AccountId, null AccountName,IFNULL(U.ProfileImage, '') as ProfileImage
                                      from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID 
                                     Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT t.RoleId  FROM TbRoleBasedAccessScreens t 
                                     inner join TbScreen s on s.ScreenName ='Venture' and t.ScreenID = s.ScreenId Where t.AccessId =3)R On R.RoleId = W.RoleID Where GroupId =@GroupID";

                    lstUsers = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { GroupID = GroupID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Used to fetch account details based on ProjectID,UserID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectBudgetAndFundsDetails> GetDonorAccountDetails(int ProjectID, int UserID)
        {
            List<ProjectBudgetAndFundsDetails> lstBudgetInfo = new List<ProjectBudgetAndFundsDetails>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"Select AccountId,AccountName From TbAccount Where ProjectId =@ProjectID And UserId =@UserID";
                    lstBudgetInfo = _connection.Query<ProjectBudgetAndFundsDetails>(strQuery, new { ProjectID = ProjectID, UserID = UserID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstBudgetInfo;
        }

        /// <summary>
        /// Add new project budget details
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="UserType"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="Amount"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public string SaveProjectBudgetsDetails(int ProjectID, int UserID, string UserType, int loggedUserId, double Amount = 0, int AccountID = 0)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "INSERT INTO TbProjectBudgetAndFundDetails(ProjectID,UserID, UserType,CreatedDate, CreatedBy, IsDeleted,DonorBudgetAmount,AccountId)" +
                                    " VALUES(@ProjectID,@UserID,@UserType,now(),@loggedUserId,0,@Amount,@AccountID);SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(strQuery, new
                    {
                        ProjectID = ProjectID,
                        UserID = UserID,
                        UserType = UserType,
                        loggedUserId = loggedUserId,
                        Amount = Amount,
                        AccountID = AccountID
                    }).Single();
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project Budget users...";
                    throw ex;
                }
            }
            return retStatus;
        }

        /// <summary>
        /// Remove Project Budget User -> function used to delete project budget details based on ProjectID,UserID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string RemoveProjectBudgetUser(int ProjectID, int UserID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails Set IsDeleted =1 ,UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID And UserID = @UserID;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        UserID = UserID,
                        loggedUserId = loggedUserId,

                    });
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving new project Budget users...";
                    throw ex;
                }
            }
            return retStatus;
        }

        /// <summary>
        /// Function used to update project budget based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TotalBudgetAmount"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateProjectBudget(int ProjectID, double TotalBudgetAmount, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProject Set Budget =@TotalBudgetAmount ,UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID;";
                    _connection.Execute(strQuery, new { ProjectID = ProjectID, TotalBudgetAmount = TotalBudgetAmount, loggedUserId = loggedUserId });
                }
                catch (Exception ex)
                {
                    retStatus = "Failed while saving project Budget ...";
                    throw ex;
                }
            }
            return retStatus;
        }

        /// <summary>
        /// Function used to update donor buget amount based on ProjectID,UserID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="BudgetAmount"></param>
        /// <param name="AccountID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateDonorAmontAndAccount(int ProjectID, int UserID, double BudgetAmount, int AccountID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;
            using (_connection = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = "Update  TbProjectBudgetAndFundDetails  Set DonorBudgetAmount =@BudgetAmount , AccountId =@AccountID, " +
                                " UpdatedDate =now(),UpdatedBy=@loggedUserId  where ProjectID =@ProjectID And UserID= @UserID;";
                    _connection.Execute(strQuery, new
                    {
                        ProjectID = ProjectID,
                        loggedUserId = loggedUserId,
                        UserID = UserID,
                        BudgetAmount = BudgetAmount,
                        AccountID = AccountID
                    });
                }
                catch (Exception ex)
                {
                    retStatus = "Failed while saving  project budget donor details...";
                    throw ex;
                }
            }
            return retStatus;
        }

        #endregion Finance_BudgetAndFunds

        #region DeployAndArchive

        /// <summary>
        /// Used to fetch project details based on InitiativeId
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="InitiativeId"></param>
        /// <returns></returns>
        public IEnumerable<Project> GetProjectListByInitID(string stage, int InitiativeId = 0)
        {
            List<Project> lstProjects = new List<Project>();
            DashboardDAL objDashboard = new DashboardDAL();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT ProjectId,ProjectName,StartDate,EndDate,Budget,IsActive,Code,IFNULL(Progress,0) as Progress FROM  TbProject where IsDeleted=0 and  ProjectStatus=@stage  and InitiativeId =@InitiativeId and IsCreated = 1 Order by ProjectId Desc";
                    lstProjects = _connection.Query<Project>(strQuery, new { stage = stage, InitiativeId = InitiativeId }).ToList();

                    foreach (Project Item in lstProjects)
                    {
                        Item.Progress = objDashboard.GetProjectProgress(Item.ProjectId);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lstProjects;
        }

        /// <summary>
        /// Function used to delete project details based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        public void DeleteProject(int ProjectID)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProject set IsDeleted = @IsDeleted where ProjectID =@ID; ";
                    _connection.Execute(query, new { ID = ProjectID, IsDeleted = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to change project status based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ProjectStatus"></param>
        public void ChangeProjectStatus(int ProjectID, string ProjectStatus)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProject set ProjectStatus = @ProjectStatus where ProjectID =@ID; ";
                    _connection.Execute(query, new { ID = ProjectID, ProjectStatus = ProjectStatus });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to change state based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserId"></param>
        /// <param name="State"></param>
        public void ChangeState(int ProjectID, int UserId, bool State)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProject set IsActive = @IsActive ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectID =@ID; ";
                    _connection.Execute(query, new { ID = ProjectID, IsActive = State, updatedDate = DateTime.Now, userID = UserId });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion DeployAndArchive

        #region ManageProjectDetails

        /// <summary>
        /// Function used to fetch project start and end date based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public dynamic GetProjectDate(int ProjectId)
        {
            dynamic obj;
            
            using (_connection = Utils.Database.GetDBConnection())
            {

                string strQeury = @"SELECT StartDate,EndDate FROM TbProject WHERE ProjectId = @ProjectId";
                obj = _connection.Query<object>(strQeury, new { ProjectId = ProjectId, IsActive = 1 }).FirstOrDefault();
            }
             
            return obj;

        }

        /// <summary>
        /// Used to fetch project details based on prjID
        /// </summary>
        /// <param name="prjID"></param>
        /// <param name="initID"></param>
        /// <returns></returns>
        public Project GetProjectDetailsByID(int prjID, int initID)
        {
            Project objProject = new Project();
            string strQuery = string.Empty;
            DashboardDAL objDashboard = new DashboardDAL();
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT t.StartDate,t.EndDate,t.ProjectId,t.InitiativeId,t.ProjectName,t.GroupId,t.Progress,t.Description,t.Code,t.Budget,t.CountryID,t.StateID,t.DistrictID,t.PaymentTermAmount FROM  TbProject t where t.ProjectId=@prjid";
                    objProject = _connection.Query<Project>(strQuery, new
                    {
                        prjid = prjID,
                        initid = initID
                    }).FirstOrDefault();

                    var projectTags = new List<Tags>();
                    //strQuery = @"select TagID , TagName, TagColor from TbProjectTags where TagID in 
                    //        ( select TagID from TbProjectToTagList where IsDeleted <>1 and ProjectId=@projid)  Order By TagID Desc";

                    strQuery = @"select t.TagID , t.TagName, t.TagColor from TbProjectTags t inner join TbProjectToTagList p where p.IsDeleted <>1
                                and p.ProjectId=@projid and p.TagID = t.TagID Order By t.TagID Desc";

                    projectTags = _connection.Query<Tags>(strQuery, new
                    {
                        projid = prjID
                    }).ToList();
                    if (projectTags.Count > 0)
                    {
                        objProject.ProjectTags = JsonConvert.SerializeObject(projectTags);
                    }


                    objProject.Progress = objDashboard.GetProjectProgress(prjID);

                }
            }
            catch (Exception ex)
            {
            }
            return objProject;
        }

        /// <summary>
        /// Used to fetch state details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<State> GetStateList()
        {
            List<State> lstState = new List<State>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT StateID,StateName,CountryID FROM  TbStates Order by StateId Desc";
                    lstState = _connection.Query<State>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstState;
        }

        /// <summary>
        /// Used to fetch district details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<District> GetDistrictList()
        {
            List<District> lstDistrict = new List<District>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT DistrictID,StateID FROM  TbDistrict Order by StateId Desc";
                    lstDistrict = _connection.Query<District>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstDistrict;
        }

        /// <summary>
        /// Function used to update project details 
        /// </summary>
        /// <param name="inputProject"></param>
        /// <param name="loguser"></param>
        public void UpdateEditProjectDetails(Project inputProject, int loguser)
        {
            string query = string.Empty;
            //DateTime? startDate = inputProject.StartDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.StartDate) : null;
            //DateTime? endDate = inputProject.EndDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.EndDate) : null;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbProject set 
                        ProjectName = @projectName ,
                        Description = @description ,
                        Code = @code ,
                        Budget = @budget ,
                        StartDate = @startdate ,
                        EndDate = @enddate ,
                        CountryID = @countryID ,
                        StateID = @stateID ,
                        DistrictID = @districtID ,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ProjectID =@projectID; ";
                        _connection.Execute(query, new
                        {
                            projectID = inputProject.ProjectId,
                            projectName = inputProject.ProjectName,
                            description = inputProject.Description != "null" ? inputProject.Description : null,
                            code = inputProject.Code,
                            budget = inputProject.Budget,
                            startdate = inputProject.StartDate,
                            enddate = inputProject.EndDate,
                            countryID = inputProject.CountryID,
                            stateID = inputProject.StateID,
                            districtID = inputProject.DistrictID,
                            updatedDate = DateTime.Now,
                            userID = loguser
                        });
                        //first remove the tag list for this project ID
                        query = @"Update TbProjectToTagList set IsDeleted=1,UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ProjectID =@projectID; ";
                        _connection.Execute(query, new
                        {
                            projectID = inputProject.ProjectId,
                            updatedDate = DateTime.Now,
                            userID = loguser
                        });

                        List<Tags> objTagsList = JsonConvert.DeserializeObject<List<Tags>>(inputProject.ProjectTags);
                        foreach (Tags tagItem in objTagsList)
                        {
                            query = @"insert TbProjectToTagList (ProjectId, TagID, CreatedDate, CreatedBy, IsDeleted)
                                                 values (@projectid,@tagid,@createddate,@createdby,@isdeleted)";
                            _connection.Execute(query, new
                            {
                                projectid = inputProject.ProjectId,
                                tagid = tagItem.TagID,
                                createddate = DateTime.Now,
                                createdby = loguser,
                                isdeleted = 0
                            });
                        }
                    }
                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = inputProject.ProjectId;
                    objLog.AuditFrom = "Project";
                    objLog.AuditEvent = "EDIT";
                    objLog.AuditFromID = inputProject.ProjectId;
                    objLog.FromIDColumnName = "ProjectId";
                    objLog.Message = "Updated Project details";
                    objLog.CreatedBy = loguser;
                    AddAuditLog(objLog);
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion ManageProjectDetails

        #region ManageTeam

        /// <summary>
        /// Function used to update update group id based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="GroupID"></param>
        /// <param name="UserId"></param>
        public void UpdateGroupInProject(int ProjectId, int GroupID, int UserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProject set GroupId = @GroupId ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectId =@ID; ";
                    _connection.Execute(query, new { ID = ProjectId, GroupId = GroupID, updatedDate = DateTime.Now, userID = UserId });
                }

                query = "Select GroupName  From TbGroup Where GroupId =@GroupId ";
                var GroupName = _connection.Query<string>(query, new { GroupId = GroupID }).Single();
                ProjectAuditLog objLog = new ProjectAuditLog();
                objLog.ProjectID = ProjectId;
                objLog.AuditFrom = "ProjectTeam";
                objLog.AuditEvent = "EDIT";
                objLog.AuditFromID = ProjectId;
                objLog.FromIDColumnName = "ProjectId";
                objLog.Message = "Updated Project Team -" + GroupName;
                objLog.CreatedBy = UserId;
                AddAuditLog(objLog);
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Used to fetch group details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public UserGroups GetGroupDetailsByprjID(int ProjectId)
        {
            UserGroups objGroup = new UserGroups();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT P.ProjectId,P.GroupId,G.GroupName FROM  TbProject P left join TbGroup G on P.GroupId=G.GroupId where P.ProjectId=@ID";
                    objGroup = _connection.Query<UserGroups>(strQuery, new { ID = ProjectId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return objGroup;
        }

        /// <summary>
        /// Used to fetch user details based on WorkspaceID,initid
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="initid"></param>
        /// <returns></returns>
        public IEnumerable<InitUserList> GetUsersListByID(int WorkspaceID, int initid = 0)
        {
            List<InitUserList> lstUsers = new List<InitUserList>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //strQuery = @"SELECT Distinct U.ID UserID, Name ,WT.RoleID  RoleID,RoleName UserRole,IFNULL(U.ProfileImage, '') AS ProfileImage FROM TbUser U" +
                    //            " Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID " +
                    //             " Inner JOIN TbInitiativeToUsers IU ON IU.UserID =U.ID  Where IsRegisteredUser = 1 and U.IsDeleted = 0 " +
                    //            " And U.IsActive =1 AND WT.WorkspaceID =@WorkspaceID and  IU.InitiativeId =@initid and IU.IsDeleted = 0 Order by U.ID Desc ";

                    strQuery = @"SELECT Distinct U.ID UserID, U.Name ,WT.RoleID  RoleID,R.RoleName UserRole,IFNULL(U.ProfileImage, '') AS ProfileImage FROM TbUser U
                                 Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID 
                                 Inner JOIN TbInitiativeToUsers IU ON IU.UserID =U.ID  Where U.IsRegisteredUser = 1 and U.IsDeleted = 0 
                                 And U.IsActive =1 AND WT.WorkspaceID = @WorkspaceID and  IU.InitiativeId = @initid and IU.IsDeleted = 0 Order by U.ID Desc";

                    lstUsers = _connection.Query<InitUserList>(strQuery, new { WorkspaceID = WorkspaceID, initid = initid }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to fetch users details not included in the group based on WorkspaceID,GroupId
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="initid"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public IEnumerable<UsersInfo> GetUsersListNotInTeam(int WorkspaceID, int initid = 0, int GroupId = 0)
        {
            List<UsersInfo> lstUsers = new List<UsersInfo>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    // --- Old Query ----
                    //strQuery = @"SELECT Distinct U.ID, Name,EmailId ,WT.RoleID  RoleID,RoleName UserRole FROM TbUser U" +
                    //            " Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  Left join TbRole R On R.RoleId = WT.RoleID " +
                    //             " Inner JOIN TbInitiativeToUsers IU ON IU.UserID =U.ID  Where IsRegisteredUser = 1 and U.IsDeleted = 0 " +
                    //            " And U.IsActive =1 AND WT.WorkspaceID = @WorkspaceID and  IU.InitiativeId =@initid  and IU.IsDeleted = 0 " +
                    //             " AND U.ID Not in (select UAG.UserId from TbUserAssignedToGroup UAG where UAG.GroupId = @GroupId and UAG.IsDeleted = 0 ) Order by U.ID Desc";
                    //strQuery = @"SELECT Distinct U.ID, Name,EmailId ,WT.RoleID  RoleID,RoleName UserRole,IFNULL(U.ProfileImage, '') AS ProfileImage  FROM TbUser U Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID  
                    //        Left join TbRole R On R.RoleId = WT.RoleID  Where IsRegisteredUser = 1 and U.IsDeleted = 0  And U.IsActive =1 AND WT.WorkspaceID = @WorkspaceID  AND U.ID Not in 
                    //        (select UAG.UserId from TbUserAssignedToGroup UAG where UAG.GroupId = @GroupId and UAG.IsDeleted = 0 ) Order by U.ID Desc";


                    strQuery = @"SELECT Distinct U.ID, U.Name,U.EmailId ,WT.RoleID  RoleID,R.RoleName UserRole,IFNULL(U.ProfileImage, '') AS ProfileImage  FROM                 TbUser U Inner JOIN TbWorkspaceTeam WT ON WT.UserID =U.ID Left join TbRole R On R.RoleId = WT.RoleID  
                            left outer join TbUserAssignedToGroup UAG on UAG.GroupId = @GroupId and UAG.IsDeleted = 0 and U.ID = UAG.UserId
                            Where U.IsRegisteredUser = 1 and U.IsDeleted = 0  And U.IsActive =1 AND WT.WorkspaceID = @WorkspaceID 
                            and UAG.UserId is null and  WT.IsDtd = 0 and WT.IsActive = 1 Order by U.ID Desc";

                    lstUsers = _connection.Query<UsersInfo>(strQuery, new { initid = initid, WorkspaceID = WorkspaceID, GroupId = GroupId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to fetch group based users details
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="GroupId"></param>
        /// <param name="initid"></param>
        /// <returns></returns>
        public List<InitUserList> GetTeamUsersList(int WorkspaceID, int GroupId, int initid)
        {
            List<InitUserList> objUsersList = new List<InitUserList>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"Select distinct U.ID UserID,Name,EmailId,(Select RoleName from TbRole where RoleId=WT.RoleId) UserRole from TbUser U
                    //        inner join TbWorkspaceTeam WT On U.ID = WT.UserID inner join TbInitiativeToUsers IU on U.ID=IU.UserID
                    //        inner join TbUserAssignedToGroup UAG on U.ID=UAG.UserID  Where WT.WorkspaceID=@WorkspaceID and
                    //        IU.InitiativeId=@initid and  UAG.IsDeleted = 0 and UAG.GroupId =@GroupId and U.IsDeleted=0 and U.IsActive=1
                    //         and IU.IsDeleted=0 Order by U.ID Desc";


                    //query = @"Select distinct U.ID UserID,U.Name,U.EmailId,(Select RoleName from TbRole where RoleId=WT.RoleId) UserRole from TbUser U
                    //             inner join TbWorkspaceTeam WT On U.ID = WT.UserID inner join TbInitiativeToUsers IU on U.ID=IU.UserID
                    //             inner join TbUserAssignedToGroup UAG on U.ID=UAG.UserID  Where WT.WorkspaceID=@WorkspaceID and
                    //             IU.InitiativeId=@initid and  UAG.IsDeleted = 0 and UAG.GroupId =@GroupId and U.IsDeleted=0 and U.IsActive=1 and IU.IsDeleted=0 Order by U.ID Desc";

                    query = @"Select distinct U.ID UserID,U.Name,U.EmailId, r.RoleName UserRole from TbUser U inner join TbWorkspaceTeam WT On U.ID = WT.UserID 
                                 inner join TbRole r on r.RoleId=WT.RoleId inner join TbInitiativeToUsers IU on U.ID=IU.UserID
                                 inner join TbUserAssignedToGroup UAG on U.ID=UAG.UserID  Where WT.WorkspaceID=@WorkspaceID and
                                 IU.InitiativeId=@initid and  UAG.IsDeleted = 0 and UAG.GroupId =@GroupId and U.IsDeleted=0 and U.IsActive=1 and IU.IsDeleted=0 Order by U.ID Desc";

                    objUsersList = _connection.Query<InitUserList>(query, new { WorkspaceID = WorkspaceID, GroupId = GroupId, initid = initid }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUsersList;
        }

        /// <summary>
        /// Used to fetch group details based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public List<UserGroups> GetGroupsListToProject(int WorkspaceID)
        {
            List<UserGroups> objUserGroupsList = new List<UserGroups>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"select G.GroupId,G.GroupName,count(distinct UAG.UserId) UsersCount, IFNULL(G.UserGroupImage, '') AS UserGroupImage 
                            FROM TbGroup G inner join TbUserAssignedToGroup UAG on G.GroupId = UAG.GroupId  
                            inner join TbUser U on UAG.UserId=U.ID 
                            where G.IsDeleted=0 and UAG.IsDeleted=0 and U.IsDeleted=0 and U.IsActive=1 
                            and G.WorkspaceID = @WorkspaceID  group by G.GroupId,G.GroupName Order by G.GroupId Desc";
                    objUserGroupsList = _connection.Query<UserGroups>(query, new { WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUserGroupsList;
        }

        /// <summary>
        /// Used to fetch groupwise user details based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public List<UsersDetails> GetGroupUsersList(int WorkspaceID, int GroupId)
        {
            List<UsersDetails> objUsersList = new List<UsersDetails>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    //query = @"Select Distinct PT.UserID ID, U.Name,U.EmailId , WR.RoleId RoleID, RoleName Role,ProjectCount,IFNULL(U.ProfileImage, '') AS ProfileImage From TbUser U 
                    //    Inner join 	(Select Distinct UserID from TbUserAssignedToGroup Where GroupID = @GroupId and IsDeleted =0) PT On PT.UserID =U.ID 
                    //    Left Join (Select UserID,W.RoleID,RoleName From TbWorkspaceTeam W Inner Join TbRole R On R.RoleId=W.RoleId Where W.WorkspaceID=@WorkspaceID) WR  On WR.UserID=PT.UserID 
                    //    Left Join (Select Distinct UserID , Count(P.ProjectId) ProjectCount from TbUserAssignedToGroup AG Inner Join TbGroup G On G.GroupId =AG.GroupId  
                    //                Left Join TbProject P On P.GroupId =G.GroupId Where WorkspaceID=@WorkspaceID Group By UserID ) P On P.UserID=U.ID
                    //    Where U.IsActive=1 and U.IsDeleted=0 and U.IsRegisteredUser =1";


                    query = @"Select Distinct PT.UserID ID, U.Name,U.EmailId , WR.RoleId RoleID, WR.RoleName Role,P.ProjectCount,IFNULL(U.ProfileImage, '') AS ProfileImage,IFNULL(U.Organization,'') AS Organization From TbUser U 
                             Inner join (Select Distinct UserID from TbUserAssignedToGroup Where GroupID = @GroupId and IsDeleted =0) PT On PT.UserID =U.ID 
                             Left Join (Select W.UserID,W.RoleID,R.RoleName , IsDtd , W.IsActive From TbWorkspaceTeam W Inner Join TbRole R On R.RoleId=W.RoleId Where 
                             W.WorkspaceID=@WorkspaceID) WR  On WR.UserID=PT.UserID Left Join (Select Distinct AG.UserID , Count(P.ProjectId) ProjectCount from TbUserAssignedToGroup AG Inner Join TbGroup G 
                             On G.GroupId =AG.GroupId Left Join TbProject P On P.GroupId =G.GroupId Where G.WorkspaceID=@WorkspaceID Group By AG.UserID ) P On P.UserID=U.ID
                              Where U.IsActive=1 and U.IsDeleted=0 and U.IsRegisteredUser =1 and WR.IsDtd =0 and WR.IsActive = 1";

                    objUsersList = _connection.Query<UsersDetails>(query, new { WorkspaceID = WorkspaceID, GroupId = GroupId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objUsersList;
        }

        /// <summary>
        /// Function used to delete groupwise user details based on GroupId,UserID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="GroupId"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string DeleteTeamUser(int ProjectID,int GroupId,int UserID, int loggedUserId)
        {
            string strQuery = string.Empty; var retMsg = "success";
            var id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        var cmd = _connection.CreateCommand();
                        cmd.Transaction = transaction;
                        cmd.Connection = _connection;

                        MySqlParameter pGroupId = new MySqlParameter("@GroupId", MySqlDbType.Int32);
                        MySqlParameter pUserID = new MySqlParameter("@UserID", MySqlDbType.Int32);
                        MySqlParameter ploggedUserId = new MySqlParameter("@loggedUserId", MySqlDbType.Int32);
                        MySqlParameter pProjectID = new MySqlParameter("@ProjectID", MySqlDbType.Int32);
                        pGroupId.Value = GroupId;
                        pUserID.Value = UserID;
                        ploggedUserId.Value = loggedUserId;
                        pProjectID.Value = ProjectID;

                        cmd.Parameters.Add(pGroupId);
                        cmd.Parameters.Add(pUserID);
                        cmd.Parameters.Add(ploggedUserId);
                        cmd.Parameters.Add(pProjectID);

                        try
                        {
                            strQuery = "UPDATE TbUserAssignedToGroup SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserID and GroupId =@GroupId ";

                            cmd.CommandText = strQuery;
                            cmd.ExecuteNonQuery();
                            /*
                             * TbPaymentTermNotifyUsers                         
                           */

                            // TbProjectWorkflowDetails
                            strQuery = "SELECT EXISTS(SELECT ID from TbProjectWorkflowDetails WHERE UserID =@UserID And WorkflowID In (Select WorkflowID from TbProjectWorkflow Where ProjectId= @ProjectID));";
                            id = _connection.Query<int>(strQuery, new { UserID = UserID, ProjectID = ProjectID }).Single();
                            if (id != 0)
                            {
                                strQuery = "UPDATE TbProjectWorkflowDetails SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserID And WorkflowID In (Select WorkflowID from TbProjectWorkflow Where ProjectId= @ProjectID)";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();
                            }

                            // TbKPIAssignUsers
                            strQuery = "SELECT EXISTS(SELECT ID from TbKPIAssignUsers WHERE UserID =@UserID And ProjectId= @ProjectID);";
                            id = _connection.Query<int>(strQuery, new { UserID = UserID, ProjectID = ProjectID }).Single();
                            if (id != 0)
                            {
                                strQuery = "UPDATE TbKPIAssignUsers SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserID And ProjectId= @ProjectID";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();
                            }

                            // TbRiskAssignUsers -
                            strQuery = "SELECT EXISTS(SELECT ID from TbRiskAssignUsers WHERE UserID =@UserID And ProjectId= @ProjectID);";
                            id = _connection.Query<int>(strQuery, new { UserID = UserID, ProjectID = ProjectID }).Single();
                            if (id != 0)
                            {
                                strQuery = "UPDATE TbRiskAssignUsers SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserID And ProjectId= @ProjectID";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();
                            }
                            // TbProjectActivityToUsers -
                            strQuery = "SELECT EXISTS(SELECT ID from TbProjectActivityToUsers WHERE UserId =@UserID And  ActivityID In (Select ActivityId from TbProjectActivity Where ProjectId= @ProjectID));";
                            id = _connection.Query<int>(strQuery, new { UserID = UserID, ProjectID = ProjectID }).Single();
                            if (id != 0)
                            {
                                strQuery = "UPDATE TbProjectActivityToUsers SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserId =@UserID And ActivityID In (Select ActivityId from TbProjectActivity Where ProjectId= @ProjectID)";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();
                            }
                            // TbProjectSurveyUser -
                            strQuery = "SELECT EXISTS(SELECT ID from TbProjectSurveyUser WHERE UserID =@UserID And SurveyID In (Select SurveyID from TbProjectSurvey Where ProjectId= @ProjectID));";
                            id = _connection.Query<int>(strQuery, new { UserID = UserID, ProjectID = ProjectID }).Single();
                            if (id != 0)
                            {
                                strQuery = "UPDATE TbProjectSurveyUser SET IsDeleted=1, UpdatedDate =now()  ,UpdatedBy =@loggedUserId Where  UserID =@UserID And SurveyID In (Select SurveyID from TbProjectSurvey Where ProjectId= @ProjectID)";

                                cmd.CommandText = strQuery;
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            retMsg = "Error while updating data.";
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (MySqlException ex)
                            {
                                if (transaction.Connection != null)
                                {
                                    retMsg = "An exception of type " + ex.GetType() +
                                    " was encountered while attempting to roll back the transaction.";
                                }
                                throw ex;
                            }
                            throw e;

                        }
                        finally
                        {
                            _connection.Close();
                        }
                    }
                }
                ProjectAuditLog objLog = new ProjectAuditLog();
                objLog.ProjectID = ProjectID;
                objLog.AuditFrom = "Project";
                objLog.AuditEvent = "REMOVE";
                objLog.AuditFromID = UserID;
                objLog.FromIDColumnName = "UserID";
                objLog.Message = "Removed Project Team User";
                objLog.CreatedBy = loggedUserId;
                AddAuditLog(objLog);
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        #endregion ManageTeam

        #region ManagePaymentTerm

        /// <summary>
        /// Add new payment terms
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="PaymentTermName"></param>
        /// <param name="Logic"></param>
        /// <param name="paymentdate"></param>
        /// <param name="Target"></param>
        /// <param name="Amount"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int SavepaymentTerms(int ProjectId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;

            using (var _Tran = new System.Transactions.TransactionScope())
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    try
                    {
                        query = "INSERT INTO TbPaymentTerms(ProjectId,PaymentTermName,Logic,paymentdate,Target,Amount,CreatedDate,CreatedBy)";
                        query += " VALUES(@ProjectId,@PaymentTermName,@Logic,@paymentdate,@Target,@Amount,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectId = ProjectId,
                            PaymentTermName = PaymentTermName,
                            Logic = Logic,
                            paymentdate = paymentdate,
                            Target = Target,
                            Amount = Amount,
                            CreatedBy = loggedUserId,

                        }).Single();
                        query = @"Update TbProject set PaymentTermAmount = PaymentTermAmount+@PaymentTermAmount ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectId =@ID; ";
                        _connection.Execute(query, new { ID = ProjectId, PaymentTermAmount = Amount, updatedDate = DateTime.Now, userID = loggedUserId });
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                    }

                }

                if (id > 0)
                {
                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "PaymentTerm";
                    objLog.AuditEvent = "ADD";
                    objLog.AuditFromID = ProjectId;
                    objLog.FromIDColumnName = "ID";
                    objLog.Message = "Added New PaymentTerm" + PaymentTermName;
                    objLog.CreatedBy = loggedUserId;
                    AddAuditLog(objLog);
                }

                _Tran.Complete();
            }
            return id;
        }
    
        /// <summary>
        /// Add new payement notify uesrs details
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="PaymentId"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int InsertpaymentTermsNotifyUsers(int ProjectId, int PaymentId, int UserID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = "INSERT INTO TbPaymentTermNotifyUsers(PaymentId,ProjectId,UserID,CreatedDate,CreatedBy)";
                    query += " VALUES(@PaymentId,@ProjectId,@UserID,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                    id = _connection.Query<int>(query, new
                    {
                        ProjectId = ProjectId,
                        PaymentId = PaymentId,
                        UserID = UserID,
                        CreatedBy = loggedUserId,

                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Used to update number of payment terms based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="NoOfPaymentTerms"></param>
        /// <param name="UserId"></param>
        public void UpdatePaymentTermInProject(int ProjectId, int NoOfPaymentTerms, int UserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbProject set NoOfPaymentTerms = NoOfPaymentTerms+@NoOfPaymentTerms ,UpdatedDate = @updatedDate,UpdatedBy=@userID where ProjectId =@ID; ";
                    _connection.Execute(query, new { ID = ProjectId, NoOfPaymentTerms = NoOfPaymentTerms, updatedDate = DateTime.Now, userID = UserId });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Used to fetch payment terms details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<PaymentTerms> GetPaymentTermsByprjID(int ProjectId)
        {

            List<PaymentTerms> objPaymentTerms = new List<PaymentTerms>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //                    strQuery = @"SELECT P.ProjectId,PT.ID PaymentId,NoOfPaymentTerms,PaymentTermName,Logic,paymentdate,Target,Amount,PaymentStatus,
                    //(case when PaymentStatus='Done' then 'green' when PaymentStatus='Cancelled' then 'red' else 'yellow' end) as Color FROM  TbProject P 
                    //                             inner join TbPaymentTerms PT on P.ProjectId=PT.ProjectId where P.ProjectId=@ID and PT.IsDeleted=0; ";

                    strQuery = @"SELECT P.ProjectId,PT.ID PaymentId,P.NoOfPaymentTerms,PT.PaymentTermName,PT.Logic,PT.paymentdate,PT.Target,PT.Amount,PT.PaymentStatus,
                                (case when PT.PaymentStatus='Done' then 'green' when PT.PaymentStatus='Cancelled' then 'red' else 'yellow' end) as Color FROM  TbProject P 
                                 inner join TbPaymentTerms PT on P.ProjectId=PT.ProjectId where P.ProjectId=@ID and PT.IsDeleted=0";

                    objPaymentTerms = _connection.Query<PaymentTerms>(strQuery, new { ID = ProjectId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objPaymentTerms;
        }

        /// <summary>
        /// Used to delete payment terms based on PaymentId
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <param name="loggedUserId"></param>
        public void DeletePaymentTerm(int PaymentId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {

                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbPaymentTerms set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId  where ID =@ID; ";
                        _connection.Execute(query, new { ID = PaymentId, IsDeleted = 1, loggedUserId = loggedUserId });
                    }

                    var strQuery = "Select PaymentTermName  From TbPaymentTerms Where ID =@PaymentId ";
                    var name = _connection.Query<string>(strQuery, new { PaymentId = PaymentId }).Single();
                    strQuery = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                    var ProjectId = _connection.Query<int>(strQuery, new { PaymentId = PaymentId }).Single();

                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "PaymentTerm";
                    objLog.AuditEvent = "REMOVE";
                    objLog.AuditFromID = PaymentId;
                    objLog.FromIDColumnName = "ID";
                    objLog.Message = "Removed PaymentTerm" + name;
                    objLog.CreatedBy = loggedUserId;
                    AddAuditLog(objLog);

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Used to update payment status based on PaymentId
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <param name="PaymentStatus"></param>
        /// <param name="loggedUserId"></param>
        public void UpdatePaymnetStatus(int PaymentId, string PaymentStatus,int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {

                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbPaymentTerms set PaymentStatus = @PaymentStatus, UpdatedDate=now(),UpdatedBy=@loggedUserId where ID =@ID; ";
                        _connection.Execute(query, new { ID = PaymentId, PaymentStatus = PaymentStatus, loggedUserId = loggedUserId });
                    }
                    var strQuery = "Select PaymentTermName  From TbPaymentTerms Where ID =@PaymentId ";
                    var name = _connection.Query<string>(strQuery, new { PaymentId = PaymentId }).Single();
                    strQuery = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                    var ProjectId = _connection.Query<int>(strQuery, new { PaymentId = PaymentId }).Single();

                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "PaymentTerm";
                    objLog.AuditEvent = "EDIT";
                    objLog.AuditFromID = PaymentId;
                    objLog.FromIDColumnName = "ID";
                    objLog.Message = "Updated PaymentStatus" + name;
                    objLog.CreatedBy = loggedUserId;
                    AddAuditLog(objLog);

                    _Tran.Complete();
                }
                }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Used to fetch payment term details based on PaymentId
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <returns></returns>
        public PaymentTerms GetPaymentTermsByPaymentId(int PaymentId)
        {

            PaymentTerms objPaymentTerms = new PaymentTerms();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT ProjectId,ID PaymentId,PaymentTermName,Logic,paymentdate,Target,Amount,PaymentStatus FROM  TbPaymentTerms where ID=@ID and IsDeleted=0  ";
                    objPaymentTerms = _connection.Query<PaymentTerms>(strQuery, new { ID = PaymentId }).FirstOrDefault();


                    strQuery = @"select Name,UserID ID,IFNULL(ProfileImage, '') as ProfileImage from TbPaymentTermNotifyUsers PTNU inner join TbUser U on PTNU.UserID=U.ID where PTNU.PaymentId=@ID and PTNU.IsDeleted=0 Order By PTNU.ID Desc";
                    var NotifyUsers = _connection.Query<UsersDetails>(strQuery, new
                    {
                        ID = PaymentId
                    }).ToList();
                    objPaymentTerms.users = JsonConvert.SerializeObject(NotifyUsers);

                }
            }
            catch (Exception ex)
            {
            }
            return objPaymentTerms;
        }

        /// <summary>
        /// Used to update payment terms based on PaymentId
        /// </summary>
        /// <param name="PaymentId"></param>
        /// <param name="PaymentTermName"></param>
        /// <param name="Logic"></param>
        /// <param name="paymentdate"></param>
        /// <param name="Target"></param>
        /// <param name="Amount"></param>
        /// <param name="loggedUserId"></param>
        public void UpdatepaymentTerms(int PaymentId, String PaymentTermName, string Logic, DateTime paymentdate, string Target, string Amount, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbPaymentTerms set PaymentTermName = @PaymentTermName,Logic=@Logic,paymentdate=@paymentdate,Target=@Target,Amount=@Amount, UpdatedDate = @updatedDate,
                        UpdatedBy=@UpdatedBy where ID =@PaymentId; ";

                        _connection.Query<int>(query, new
                        {
                            PaymentId = PaymentId,
                            PaymentTermName = PaymentTermName,
                            Logic = Logic,
                            paymentdate = paymentdate,
                            Target = Target,
                            Amount = Amount,
                            UpdatedBy = loggedUserId,
                            UpdatedDate = DateTime.Now

                        });
                    }
                    query = "Select ProjectId  From TbPaymentTerms Where ID =@PaymentId ";
                    var ProjectId = _connection.Query<int>(query, new { PaymentId = PaymentId }).Single();
                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "PaymentTerm";
                    objLog.AuditEvent = "EDIT";
                    objLog.AuditFromID = PaymentId;
                    objLog.FromIDColumnName = "ID";
                    objLog.Message = "Updated Payment Terms details";
                    objLog.CreatedBy = loggedUserId;
                    AddAuditLog(objLog);

                    _Tran.Complete();
                }
                }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Used to fetch notify users by assigned group (2 type - Finance,Donor) based on GroupID,WorkspaceID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="Type"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<UsersDetails> GetNotifyUsers(int GroupID, string Type, int WorkspaceID)
        {
            List<UsersDetails> lstUsers = new List<UsersDetails>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    if (Type == "Finance")
                        //strQuery = @"Select Distinct TG.UserID ID ,Name ,EmailId,IFNULL(ProfileImage, '') as ProfileImage " +
                        //            " from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID " +
                        //            " Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 " +
                        //            " And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='My Finance'))R On R.RoleId = W.RoleID Where GroupId =@GroupID And W.WorkspaceID = @WorkspaceID ";

                        strQuery = @"Select Distinct TG.UserID ID ,U.Name ,U.EmailId,IFNULL(U.ProfileImage, '') as ProfileImage 
                                     from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID 
                                     Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT t.RoleId  FROM TbRoleBasedAccessScreens t
                                      inner join TbScreen s on s.ScreenName ='My Finance' and t.ScreenID = s.ScreenId Where t.AccessId =3)R On R.RoleId = W.RoleID Where TG.GroupId = @GroupID And W.WorkspaceID = @WorkspaceID";

                    else if (Type == "Donor")
                        //strQuery = @"Select Distinct TG.UserID ID,Name ,EmailId,IFNULL(ProfileImage, '') as ProfileImage from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID " +
                        //            " Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT RoleId  FROM TbRoleBasedAccessScreens Where AccessId =3 " +
                        //            " And  ScreenID in (SELECT ScreenId FROM TbScreen  where ScreenName ='Venture'))R On R.RoleId = W.RoleID Where GroupId =@GroupID And W.WorkspaceID = @WorkspaceID";

                        strQuery = @"Select Distinct TG.UserID ID,U.Name,U.EmailId,IFNULL(U.ProfileImage, '') as ProfileImage from TbUserAssignedToGroup TG Inner Join TbUser U  On U.ID =TG.UserID 
                                     Inner  Join TbWorkspaceTeam W On W.UserID = U.ID Inner join (SELECT t.RoleId  FROM TbRoleBasedAccessScreens t inner join TbScreen s on s.ScreenName ='Venture' and s.ScreenId = t.ScreenID 
                                     Where t.AccessId =3)R On R.RoleId = W.RoleID Where GroupId =@GroupID And W.WorkspaceID = @WorkspaceID";

                    lstUsers = _connection.Query<UsersDetails>(strQuery, new { GroupID = GroupID, WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to delete payment term notify users based on PaymentId
        /// </summary>
        /// <param name="PaymentId"></param>
        public void DeletePaymentTermNotifyUsers(int PaymentId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbPaymentTermNotifyUsers set IsDeleted = @IsDeleted where PaymentId =@ID; ";
                    _connection.Execute(query, new { ID = PaymentId, IsDeleted = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion ManagePaymentTerm

        #region ManageProjectRisk

        /// <summary>
        /// Add new risk details
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="RiskName"></param>
        /// <param name="RiskType"></param>
        /// <param name="Description"></param>
        /// <param name="RiskPeriodicity"></param>
        /// <param name="RiskRating"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="WorkflowID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int CreateRisk(int ProjectID, string RiskName, string RiskType, string Description,int RiskPeriodicity, int RiskRating, DateTime StartDate, DateTime EndDate, int WorkflowID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "INSERT INTO TbRisk(ProjectId,RiskName,RiskType,Description,RiskPeriodicity,RiskRating,StartDate,EndDate,WorkflowID,CreatedDate,CreatedBy)";
                        query += " VALUES(@ProjectId,@RiskName,@RiskType,@Description,@RiskPeriodicity,@RiskRating,@StartDate,@EndDate,@WorkflowID,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectId = ProjectID,
                            RiskName = RiskName,
                            RiskType = RiskType,
                            Description = Description,
                            RiskPeriodicity = RiskPeriodicity,
                            RiskRating = RiskRating,
                            StartDate = StartDate,
                            EndDate = EndDate,
                            WorkflowID = WorkflowID,
                            CreatedBy = loggedUserId,

                        }).Single();
                        if (id != 0)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectID;
                            objLog.AuditFrom = "Risk";
                            objLog.AuditEvent = "ADD";
                            objLog.AuditFromID = id;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Added new Risk" + RiskName;
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);

                            // Add into Sync DB
                            //string Qeury = @"Select A.WorkflowID, 0 FinanceWorkflowId,WfNoOfLevels ,B.WorkspaceID From TbRisk A 
                            //                Inner Join    (Select WorkspaceID,ProjectId From TbProject  A Inner Join TbInitiative B On B.InitiativeId =A.InitiativeId Where ProjectId =@ProjectID) B On A.ProjectId =B.ProjectId                               
                            //                    Left Join TbProjectWorkflow  TbProjectWorkflow On TbProjectWorkflow.WorkflowID = A.WorkflowID
                            //                    Where A.IsDeleted = 0 And TbProjectWorkflow.IsDeleted = 0 And A.ID = @RISKID";

                            string Qeury = @"Select A.WorkflowID, 0 FinanceWorkflowId,TbProjectWorkflow.WfNoOfLevels ,P.WSID as WorkspaceID From TbRisk A 
                                    Inner Join   TbProject P on P.ProjectId = A.ProjectId                       
                                    Left Join TbProjectWorkflow  TbProjectWorkflow On TbProjectWorkflow.WorkflowID = A.WorkflowID
                                    Where A.IsDeleted = 0 And TbProjectWorkflow.IsDeleted = 0 And A.ID = @RISKID  and P.ProjectId = @ProjectID";
                            dynamic objWF = _connection.Query<object>(Qeury, new { RISKID = id, ProjectID = ProjectID }).FirstOrDefault();

                            Qeury = @"Select WSID as WorkspaceID From TbProject  Where ProjectId = @ProjectId";
                            var WID = _connection.Query<int>(Qeury, new { ProjectId = ProjectID }).FirstOrDefault();

                            //AddTo SyncDB
                            string strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                            var syncDBId = _connection.Query<int>(strQuery, new
                            {
                                Type = "RISK",
                                Command = "ADD",
                                TypeIDColumn = "RISKID",
                                TypeID = id,
                                WorkflowID = WorkflowID,
                                FinanceWorkflowID = 0,
                                WfNoOfLevels = objWF == null ? 0 : objWF.WfNoOfLevels,
                                Status = "Pending",
                                WorkspaceID = WID,
                                CreatedBy = loggedUserId
                            }).Single();
                        }
                        //if (id != 0)
                        //{

                        //    query = "INSERT INTO TbRiskActivityLog(ProjectId,RiskId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@RiskId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = ProjectID,
                        //        RiskId = id,
                        //        Message = "Created a new risk " + RiskName ,
                        //        CreatedBy = loggedUserId,

                        //    });
                        //}

                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Used to fetch project workflow details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<Workflow> GetWorkflows(int ProjectId, int WorkspaceID)
        {
            List<Workflow> objWorkflowsList = new List<Workflow>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Select WorkflowID ,WfName ,WfDescription ,WfNoOfLevels  From TbProjectWorkflow 
                            Where WorkspaceID =@wsid and  ProjectId=@projid and IsDeleted=0 ";
                    objWorkflowsList = _connection.Query<Workflow>(query, new { projid = ProjectId, wsid = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objWorkflowsList;
        }

        /// <summary>
        /// Function used to fetch risk details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<Risk> GetRiskByprjID(int ProjectId)
        {
            List<Risk> objRisk = new List<Risk>();
            string strQuery = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {

                        //       strQuery = @"SELECT R.ProjectID,R.ID riskId,RiskName,RiskType,StartDate,EndDate,Description,RiskRating,WorkflowID,
                        //             (select WfName From TbProjectWorkflow where WorkflowID= R.WorkflowID) WorkflowName 
                        //FROM TbRisk R where R.ProjectId=@ID and R.IsDeleted=0  Order by R.ID desc";

                        strQuery = @"SELECT R.ProjectID,R.ID riskId,R.RiskName,R.RiskType,R.StartDate,R.EndDate,R.Description,R.RiskRating,R.WorkflowID,
                                     w.WfName WorkflowName FROM TbRisk R inner join TbProjectWorkflow w on w.WorkflowID= R.WorkflowID 
                                     where R.ProjectId=@ID and R.IsDeleted=0  Order by R.ID desc";

                        objRisk = _connection.Query<Risk>(strQuery, new { ID = ProjectId }).ToList();

                        foreach (Risk Item in objRisk)
                        {
                            strQuery = @"select U.Name,RNU.UserID ID,ifnull(U.ProfileImage,'') as ProfileImage from TbRiskAssignUsers RNU inner join TbUser U on RNU.UserID=U.ID inner join TbRisk R
                             on RNU.riskId=R.ID where R.ProjectID=@ID and RNU.IsDeleted=0 and RNU.riskId=@riskId Order By RNU.ID Desc";
                            Item.UserList = _connection.Query<UsersDetails>(strQuery, new { ID = ProjectId, riskId = Item.riskId }).ToList();
                        }

                    }

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return objRisk;
        }

        /// <summary>
        /// Function used to delete risk users based on riskId
        /// </summary>
        /// <param name="riskId"></param>
        public void DeleteRiskUser(int riskId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbRiskAssignUsers set IsDeleted = @IsDeleted where riskId =@ID; ";
                    _connection.Execute(query, new { ID = riskId, IsDeleted = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to delete risk based on riskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteRisk(int riskId, int loggedUserId)
        {
            string query = string.Empty;

            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbRisk set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId  where ID =@ID; ";
                        _connection.Execute(query, new { ID = riskId, IsDeleted = 1, loggedUserId = loggedUserId });
                    }

                    query = "Select ProjectId  From TbRisk Where ID =@riskId ";
                    var ProjectId = _connection.Query<int>(query, new { riskId = riskId }).Single();
                    var strQuery = "Select RiskName  From TbRisk Where ID =@riskId ";
                    var name = _connection.Query<string>(strQuery, new { riskId = riskId }).Single();

                    ProjectAuditLog objLog = new ProjectAuditLog();
                    objLog.ProjectID = ProjectId;
                    objLog.AuditFrom = "Risk";
                    objLog.AuditEvent = "REMOVE";
                    objLog.AuditFromID = riskId;
                    objLog.FromIDColumnName = "ID";
                    objLog.Message = "Removed Risk" + name;
                    objLog.CreatedBy = loggedUserId;
                    AddAuditLog(objLog);

                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }
     
        /// <summary>
        /// Add new risk notification users
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="riskId"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int InsertriskNotifyUsers(int ProjectId, int riskId, int UserID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "INSERT INTO TbRiskAssignUsers(riskId,ProjectId,UserID,CreatedDate,CreatedBy)";
                        query += " VALUES(@riskId,@ProjectId,@UserID,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectId = ProjectId,
                            riskId = riskId,
                            UserID = UserID,
                            CreatedBy = loggedUserId,

                        }).Single();

                        query = "Select Name  From TbUser Where ID =@UserID ";
                        var AssgName = _connection.Query<string>(query, new { UserID = UserID }).Single();
                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "RISK",
                            TaskTypeID = riskId,
                            TypeIDColumn = "ID",
                            AssignedTo = UserID,
                            AssignedBy = loggedUserId,
                            Message = "Assigned the task to " + AssgName,
                            createdby = loggedUserId
                        }).Single();


                        // Send Notification 
                        try
                        {
                            // Send Notification 
                            var title = "";
                            var sub = "";
                            query = "Select RiskName  From TbRisk Where  ID =@riskId ";
                            var RiskName = _connection.Query<string>(query, new { riskId = riskId }).Single();

                            title = AssgName + " RISK ASSINGED";
                            sub = AssgName + "Added an new Risk :" + RiskName + "' on " + DateTime.Now;
                            Notification objNotification = new Notification();
                            objNotification.UserId = UserID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }
                        catch
                        {

                        }
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Function used to fetch risk details based on riskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public Risk GetRiskByriskId(int riskId)
        {

            Risk objRisk = new Risk();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"SELECT ProjectId,ID riskId,RiskName,RiskType,RiskRating,StartDate,EndDate,Description,RiskPeriodicity,WorkflowID,
                    //          (select WfName From TbProjectWorkflow where WorkflowID= TbRisk.WorkflowID) WorkflowName  FROM  TbRisk where ID=@ID  and IsDeleted=0 ";


                    strQuery = @"SELECT t.ProjectId,t.ID riskId,t.RiskName,t.RiskType,t.RiskRating,t.StartDate,t.EndDate,t.Description,t.RiskPeriodicity,t.WorkflowID,
                                w.WfName WorkflowName FROM  TbRisk t inner join TbProjectWorkflow w on w.WorkflowID= t.WorkflowID where t.ID=@ID  and t.IsDeleted=0";

                    objRisk = _connection.Query<Risk>(strQuery, new { ID = riskId }).FirstOrDefault();


                    strQuery = @"select U.Name,PTNU.UserID as ID , IFNULL(U.ProfileImage, '') as ProfileImage from TbRiskAssignUsers PTNU inner join TbUser U on PTNU.UserID=U.ID where PTNU.riskId=@ID and PTNU.IsDeleted=0  Order By PTNU.ID Desc";
                    var NotifyUsers = _connection.Query<UsersDetails>(strQuery, new
                    {
                        ID = riskId
                    }).ToList();
                    objRisk.Users = JsonConvert.SerializeObject(NotifyUsers);
                    DateTime? RptDate = null;

                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbRiskTrackDate WHERE RiskID = @RiskID order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { RiskID = objRisk.riskId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }
                    if (objRisk.RiskPeriodicity != 1)
                    {
                        if (RptDate != null)
                        {
                            switch (objRisk.RiskPeriodicity)
                            {
                                case (int)CommonActivity.ReportingFrequency.Once:
                                    objRisk.ReportingOnceDate = RptDate.Value.ToString("dd-MM-yyyy");
                                    objRisk.ReportingFrequencyName = "Once";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Weekly:
                                    objRisk.ReportingWeek = (int)RptDate.Value.DayOfWeek;
                                    objRisk.ReportingFrequencyName = "Weekly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Monthly:
                                    objRisk.ReportingMonthDay = RptDate.Value.Day;
                                    objRisk.ReportingFrequencyName = "Monthly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Quarterly:
                                    objRisk.ReportingMonthDateDay = RptDate.Value.Day;
                                    objRisk.ReportingMonth = RptDate.Value.Month;
                                    objRisk.ReportingFrequencyName = "Quarterly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                    objRisk.ReportingMonthDateDay = RptDate.Value.Day;
                                    objRisk.ReportingMonth = RptDate.Value.Month;
                                    objRisk.ReportingFrequencyName = "Half_Yearly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Yearly:
                                    objRisk.ReportingFrequencyName = "Yearly";
                                    objRisk.ReportingMonthDateDay = RptDate.Value.Day;
                                    objRisk.ReportingMonth = RptDate.Value.Month;
                                    break;
                            }
                        }
                    }
                    else
                    { objRisk.ReportingFrequencyName = "Never"; }

                }
            }
            catch (Exception ex)
            {
            }
            return objRisk;
        }

        /// <summary>
        /// Function used to update risk based on riskId
        /// </summary>
        /// <param name="objRisk"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateRisk(Risk objRisk, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbRisk set 
                        RiskName = @RiskName ,
                        RiskType = @RiskType ,
                        Description = @Description ,
                        RiskPeriodicity=@RiskPeriodicity,
                        RiskRating = @RiskRating ,
                        StartDate = @StartDate ,
                        EndDate = @EndDate ,
                        WorkflowID = @WorkflowID ,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ID =@riskId; ";
                        _connection.Execute(query, new
                        {
                            riskId = objRisk.riskId,
                            RiskName = objRisk.RiskName,
                            RiskType = objRisk.RiskType,
                            Description = objRisk.Description,
                            RiskPeriodicity = objRisk.RiskPeriodicity,
                            RiskRating = objRisk.RiskRating,
                            StartDate = objRisk.StartDate,
                            EndDate = objRisk.EndDate,
                            WorkflowID = objRisk.WorkflowID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });

                        if (objRisk.riskId != 0)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = objRisk.ProjectID;
                            objLog.AuditFrom = "Risk";
                            objLog.AuditEvent = "EDIT";
                            objLog.AuditFromID = objRisk.riskId;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Updated Risk" + objRisk.RiskName + " details";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
                        }
                        //if (objRisk.riskId != 0)
                        //{

                        //    query = "INSERT INTO TbRiskActivityLog(ProjectId,RiskId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@RiskId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = objRisk.ProjectID,
                        //        RiskId = objRisk.riskId,
                        //        Message = "Updated risk " + objRisk.RiskName ,
                        //        CreatedBy = loggedUserId,

                        //    });
                        //}
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to fetch log details based on ProjectId,RiskId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="RiskId"></param>
        /// <returns></returns>
        public IEnumerable<ActivityLog> GetActivityLog(int ProjectId, int RiskId)
        {
            List<ActivityLog> lstUsers = new List<ActivityLog>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"Select  RAL.CreatedBy UserId ,Name ,Message ,RAL.CreatedDate,IFNULL(ProfileImage, '') as ProfileImage
                    //            from TbProjectAuditLog RAL Inner Join TbUser U  On U.ID =RAL.CreatedBy 
                    //            Where ProjectId =@ProjectId and AuditFromID=@RiskId And AuditFrom = 'Risk' order by RAL.CreatedDate desc";


                    strQuery = @"Select  RAL.CreatedBy UserId ,U.Name ,RAL.Message ,RAL.CreatedDate,IFNULL(U.ProfileImage, '') as ProfileImage 
                                  from TbProjectAuditLog RAL Inner Join TbUser U  On U.ID =RAL.CreatedBy Where RAL.ProjectId =@ProjectId and RAL.AuditFromID=@RiskId And RAL.AuditFrom = 'Risk' order by RAL.CreatedDate desc";

                    lstUsers = _connection.Query<ActivityLog>(strQuery, new { ProjectId = ProjectId, RiskId = RiskId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to update workflow details in risk based on RiskId
        /// </summary>
        /// <param name="WorkFlowID"></param>
        /// <param name="ProjectId"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="RiskId"></param>
        public void UpdateWorkflow(int WorkFlowID, int ProjectId, int loggedUserId, int RiskId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbRisk set 
                        WorkflowID = @WorkflowID ,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ID =@riskId; ";
                        _connection.Execute(query, new
                        {
                            riskId = RiskId,
                            WorkflowID = WorkFlowID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });
                        if (RiskId != 0)
                        {
                            var strQuery = "Select RiskName  From TbRisk Where ID =@riskId ";
                            var name = _connection.Query<string>(strQuery, new { riskId = RiskId }).Single();

                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectId;
                            objLog.AuditFrom = "Risk";
                            objLog.AuditEvent = "EDIT";
                            objLog.AuditFromID = RiskId;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Updated Risk '" + name + "' workflow details";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
                        }

                        //if (RiskId != 0)
                        //{

                        //    query = "INSERT INTO TbRiskActivityLog(ProjectId,RiskId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@RiskId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = ProjectId,
                        //        RiskId = RiskId,
                        //        Message = "Assign new workflow ",
                        //        CreatedBy = loggedUserId,

                        //    });
                        //}
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to fetch risk reporting frequency date details based on riskId
        /// </summary>
        /// <param name="objRisk"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public bool GenerateReportingFrequencyTrackDateRisk(Risk objRisk, int loggedUserId, int riskId)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;
            try
            {

                using (var _Tran = new System.Transactions.TransactionScope())
                {

                    bool isDeleted = DeleteRiskTrackDate(objRisk.riskId);

                    switch (objRisk.RiskPeriodicity)
                    {
                        case (int)CommonActivity.ReportingFrequency.Once:
                            DateTime? reportvalue = CommonActivity.GetDateTimeValue(objRisk.ReportingOnceDate);
                            lstTrackDates.Add(reportvalue.Value.ToString("yyyy-MM-dd"));
                            break;
                        case (int)CommonActivity.ReportingFrequency.Daily:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, 0, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Weekly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingWeek, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Monthly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonthDay, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Quarterly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonthDateDay, objRisk.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonthDateDay, objRisk.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objRisk.StartDate, objRisk.EndDate, objRisk.RiskPeriodicity, objRisk.ReportingMonthDateDay, objRisk.ReportingMonth);
                            break;
                    }

                    if (lstTrackDates != null && lstTrackDates.Count > 0)
                    {
                        query = "INSERT INTO TbRiskTrackDate(TrackDate,RiskID,TrackStatus) ";

                        for (int i = 0; i < lstTrackDates.Count; i++)
                        {
                            query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + riskId + ",0" : " SELECT '" + lstTrackDates[i] + "'," + riskId + ",0";
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

                    _Tran.Complete();
                }
                }
            catch (Exception ex)
            {
            }
            return success;
        }

        /// <summary>
        /// Function used to delete risk track date based on riskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public bool DeleteRiskTrackDate(int riskId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "DELETE FROM TbRiskTrackDate WHERE riskId=@riskId ";
                    _connection.Execute(strQuery, new { riskId = riskId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        /// <summary>
        /// Function used to delete old risk users and assign new risk users based on riskId
        /// </summary>
        /// <param name="objRisk"></param>
        /// <param name="loggeduserid"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        public bool AssignUserstoRisk(Risk objRisk, int loggeduserid, bool notify)
        {
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    if (!string.IsNullOrEmpty(objRisk.Users))
                    {
                        DeleteRiskUser(objRisk.riskId);

                        List<UsersDetails> lstNotifyUser = JsonConvert.DeserializeObject<List<UsersDetails>>(objRisk.Users);
                        foreach (UsersDetails userItem in lstNotifyUser)
                        {
                            InsertriskNotifyUsers(objRisk.ProjectID, objRisk.riskId, userItem.ID, loggeduserid);
                            if (notify)
                            {
                                bool bStatus = false;
                                EmailActivity objEmail = new EmailActivity();
                                GroupNotify obj = new GroupNotify();
                                obj.UserName = userItem.Name;
                                obj.UserEmailId = userItem.EmailId;
                                bStatus = objEmail.SendMail("PaymentTerm", obj);
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

        #endregion ManageProjectRisk

        #region ManageProjectKPI

        /// <summary>
        /// Add new KPI
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="KPIName"></param>
        /// <param name="Description"></param>
        /// <param name="KPIPeriodicity"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="WorkflowID"></param>
        /// <param name="SurveyID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int CreateKPI(int ProjectID, string KPIName, string Description, int KPIPeriodicity, DateTime StartDate, DateTime EndDate, int WorkflowID,int SurveyID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "INSERT INTO TbKPI(ProjectId,KPIName,Description,KPIPeriodicity,StartDate,EndDate,WorkflowID,SurveyID,CreatedDate,UpdatedDate,CreatedBy)";
                        query += " VALUES(@ProjectId,@KPIName,@Description,@KPIPeriodicity,@StartDate,@EndDate,@WorkflowID,@SurveyID,now(),now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectId = ProjectID,
                            KPIName = KPIName,
                            Description = Description,
                            KPIPeriodicity = KPIPeriodicity,
                            StartDate = StartDate,
                            EndDate = EndDate,
                            WorkflowID = WorkflowID,
                            SurveyID = SurveyID,
                            CreatedBy = loggedUserId,

                        }).Single();

                        if (id != 0)
                        {
                            // Add into Sync DB
                            string Qeury = @"Select A.WorkflowID, 0 FinanceWorkflowId,TbProjectWorkflow.WfNoOfLevels ,TbProjectWorkflow.WorkspaceID From TbKPI A 
                                    Left Join TbProjectWorkflow TbProjectWorkflow On TbProjectWorkflow.WorkflowID = A.WorkflowID
                                        Where A.IsDeleted = 0 And TbProjectWorkflow.IsDeleted = 0 And A.ID = @KPIID";
                            dynamic objWF = _connection.Query<object>(Qeury, new { KPIID = id }).FirstOrDefault();

                            Qeury = @"Select B.WorkspaceID From TbProject  A Inner Join TbInitiative B On B.InitiativeId =A.InitiativeId Where A.ProjectId = @ProjectId";
                            var WID = _connection.Query<int>(Qeury, new { ProjectID = ProjectID }).FirstOrDefault();

                            //AddTo SyncDB
                            string strQuery = @"INSERT INTO TaskSyncDB(Type, Command, TypeIDColumn, TypeID,WorkflowID,FinanceWorkflowID,TotalWFLevel,Status,WorkspaceID,CreatedDate, CreatedBy )
                                  VALUES(@Type, @Command, @TypeIDColumn, @TypeID,@WorkflowID,@FinanceWorkflowID,@WfNoOfLevels,@Status,@WorkspaceID, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                            var syncDBId = _connection.Query<int>(strQuery, new
                            {
                                Type = "KPI",
                                Command = "ADD",
                                TypeIDColumn = "KPIID",
                                TypeID = id,
                                WorkflowID = objWF == null ? 0 : objWF.WorkflowId,
                                FinanceWorkflowID = 0,
                                WfNoOfLevels = objWF == null ? 0 : objWF.WfNoOfLevels,
                                Status = "Pending",
                                WorkspaceID = WID,
                                CreatedBy = loggedUserId
                            }).Single();

                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectID;
                            objLog.AuditFrom = "KPI";
                            objLog.AuditEvent = "ADD";
                            objLog.AuditFromID = id;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Added New KPI named '" + KPIName + "'.";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
                        }
                        //if (id != 0)
                        //{

                        //    query = "INSERT INTO TbKPIActivityLog(ProjectId,KPIId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@KPIId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = ProjectID,
                        //        KPIId = id,
                        //        Message = "Created a new KPI " + KPIName ,
                        //        CreatedBy = loggedUserId,

                        //    });


                        //}

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Function used to fetch KPI details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<KPI> GetKPIByprjID(int ProjectId)
        {
            List<KPI> objKPI = new List<KPI>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT R.ProjectID,R.ID KPIId,R.KPIName,R.KPIPeriodicity,R.StartDate,R.EndDate,R.Description,R.SurveyID,R.WorkflowID,
                              DATE_FORMAT(R.UpdatedDate , "" %d/%m/%Y"") as UpdatedDate,
                              w.WfName WorkflowName ,
                              s.SurveyName SurveyName
                 FROM TbKPI R inner join TbProjectWorkflow w on w.WorkflowID= R.WorkflowID inner join TbProjectSurvey s on s.SurveyID= R.SurveyID where R.ProjectId=@ID and R.IsDeleted=0 Order By R.ID desc ";
                    objKPI = _connection.Query<KPI>(strQuery, new { ID = ProjectId }).ToList();

                    foreach (KPI Item in objKPI)
                    {
                        strQuery = @"select U.Name,RNU.UserID ID from TbKPIAssignUsers RNU inner join TbUser U on RNU.UserID=U.ID inner join TbKPI R
                             on RNU.KPIId=R.ID where R.ProjectID=@ID and RNU.IsDeleted=0 and RNU.KPIId=@KPIId Order By RNU.ID Desc";
                        Item.UserList = _connection.Query<UsersDetails>(strQuery, new { ID = ProjectId, KPIId = Item.KPIId }).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return objKPI;
        }

        /// <summary>
        /// Function used to delete KPI users based on KPIId
        /// </summary>
        /// <param name="KPIId"></param>
        public void DeleteKPIUser(int KPIId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbKPIAssignUsers set IsDeleted = @IsDeleted where KPIId =@ID; ";
                    _connection.Execute(query, new { ID = KPIId, IsDeleted = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to delete KPI based on KPIId
        /// </summary>
        /// <param name="KPIId"></param>
        /// <param name="loggedUserId"></param>
        public void DeleteKPI(int KPIId, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbKPI set IsDeleted = @IsDeleted, UpdatedDate=now(),UpdatedBy=@loggedUserId  where ID =@ID; ";
                        _connection.Execute(query, new { ID = KPIId, IsDeleted = 1, loggedUserId = loggedUserId });

                        query = "Select ProjectId  From TbKPI Where ID =@KPIId ";
                        var ProjectId = _connection.Query<int>(query, new { KPIId = KPIId }).Single();
                        query = "Select KPIName  From TbKPI Where ID =@KPIId ";
                        var name = _connection.Query<string>(query, new { KPIId = KPIId }).Single();

                        ProjectAuditLog objLog = new ProjectAuditLog();
                        objLog.ProjectID = ProjectId;
                        objLog.AuditFrom = "KPI";
                        objLog.AuditEvent = "REMOVE";
                        objLog.AuditFromID = KPIId;
                        objLog.FromIDColumnName = "ID";
                        objLog.Message = "Removed KPI named '" + name + "'.";
                        objLog.CreatedBy = loggedUserId;
                        AddAuditLog(objLog);
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }
    
        /// <summary>
        /// Add new KPI users 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="KPIId"></param>
        /// <param name="UserID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int InsertKPINotifyUsers(int ProjectId, int KPIId, int UserID, int loggedUserId)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = "INSERT INTO TbKPIAssignUsers(KPIId,ProjectId,UserID,CreatedDate,CreatedBy)";
                        query += " VALUES(@KPIId,@ProjectId,@UserID,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectId = ProjectId,
                            KPIId = KPIId,
                            UserID = UserID,
                            CreatedBy = loggedUserId,

                        }).Single();


                        //query = "Select KPIName  From TbKPI Where ID =@KPIId ";
                        //var name = _connection.Query<string>(query, new { KPIId = KPIId }).Single();
                        //ProjectAuditLog objLog = new ProjectAuditLog();
                        //objLog.ProjectID = ProjectId;
                        //objLog.AuditFrom = "KPI";
                        //objLog.AuditEvent = "ADD";
                        //objLog.AuditFromID = KPIId;
                        //objLog.FromIDColumnName = "ID";
                        //objLog.Message = "Assigned KPI '" + name +"' users";
                        //objLog.CreatedBy = loggedUserId;
                        //AddAuditLog(objLog);

                        query = "Select Name  From TbUser Where ID =@UserID ";
                        var AssgName = _connection.Query<string>(query, new { UserID = UserID }).Single();
                        query = @"INSERT INTO TbTaskAuditLog(ProjectID,TaskType,TaskTypeID,TypeIDColumn,AssignedTo,AssignedBy,Message,CreatedDate,CreatedBy)
                            VALUES(@ProjectID,@TaskType,@TaskTypeID,@TypeIDColumn,@AssignedTo,@AssignedBy,@Message,now(),@CreatedBy); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ProjectID = ProjectId,
                            TaskType = "KPI",
                            TaskTypeID = KPIId,
                            TypeIDColumn = "ID",
                            AssignedTo = UserID,
                            AssignedBy = loggedUserId,
                            Message = "Assigned the task to " + AssgName,
                            createdby = loggedUserId
                        }).Single();

                        // Send Notification 
                        try
                        {
                            // Send Notification 
                            var title = "";
                            var sub = "";
                            query = "Select KPIName  From TbKPI Where  ID =@KPIId ";
                            var KPIname = _connection.Query<string>(query, new { KPIId = KPIId }).Single();

                            title = AssgName + "KPI ASSINGED";
                            sub = AssgName + "Added an new KPI :" + KPIname + "' on " + DateTime.Now;
                            Notification objNotification = new Notification();
                            objNotification.UserId = UserID;
                            objNotification.Title = title;
                            objNotification.Subject = sub;
                            new NotificationDAL().SendUserNotification(objNotification);
                        }
                        catch
                        {

                        }

                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Function used to fetch KPI details based on KPIId
        /// </summary>
        /// <param name="KPIId"></param>
        /// <returns></returns>
        public KPI GetKPIByKPIId(int KPIId)
        {
           
            KPI objKPI = new KPI();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"SELECT ProjectId,ID KPIId,KPIName,KPIPeriodicity,StartDate,EndDate,Description,WorkflowID,
                    //          (select WfName From TbProjectWorkflow where WorkflowID= TbKPI.WorkflowID) WorkflowName,SurveyID,
                    //           (Select SurveyName  From TbProjectSurvey  where SurveyID= TbKPI.SurveyID) Survey  FROM  TbKPI where ID=@ID  and IsDeleted=0 ";


                    strQuery = @"SELECT TbKPI.ProjectId,TbKPI.ID KPIId,TbKPI.KPIName,TbKPI.KPIPeriodicity,TbKPI.StartDate,TbKPI.EndDate,TbKPI.Description,TbKPI.WorkflowID,
                                 w.WfName WorkflowName,TbKPI.SurveyID, s.SurveyName Survey FROM TbKPI inner join TbProjectWorkflow w on w.WorkflowID= TbKPI.WorkflowID
                                 inner join TbProjectSurvey s on s.SurveyID= TbKPI.SurveyID where TbKPI.ID=@ID and TbKPI.IsDeleted=0";

                    objKPI = _connection.Query<KPI>(strQuery, new { ID = KPIId }).FirstOrDefault();


                    strQuery = @"select U.Name,PTNU.UserID from TbKPIAssignUsers PTNU inner join TbUser U on PTNU.UserID=U.ID where PTNU.KPIId=@ID and PTNU.IsDeleted=0  Order By PTNU.ID Desc";
                    var NotifyUsers = _connection.Query<InitUserList>(strQuery, new
                    {
                        ID = KPIId
                    }).ToList();
                    objKPI.Users = JsonConvert.SerializeObject(NotifyUsers);
                    objKPI.Tags = GetKPIAssignedTags(KPIId);

                    DateTime? RptDate = null;

                    dynamic obj;
                    string strQeury = @"SELECT TrackDate FROM TbKPITrackDate WHERE KPIId = @KPIId order by ID LIMIT 1";
                    obj = _connection.Query<object>(strQeury, new { KPIId = objKPI.KPIId }).FirstOrDefault();

                    if (obj != null)
                    {
                        RptDate = obj.TrackDate;
                    }
                    if (objKPI.KPIPeriodicity != 1)
                    {
                        if (RptDate != null)
                        {
                            switch (objKPI.KPIPeriodicity)
                            {
                                case (int)CommonActivity.ReportingFrequency.Once:
                                    objKPI.ReportingOnceDate = RptDate.Value.ToString("dd-MM-yyyy");
                                    objKPI.ReportingFrequencyName = "Once";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Weekly:
                                    objKPI.ReportingWeek = (int)RptDate.Value.DayOfWeek;
                                    objKPI.ReportingFrequencyName = "Weekly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Monthly:
                                    objKPI.ReportingMonthDay = RptDate.Value.Day;
                                    objKPI.ReportingFrequencyName = "Monthly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Quarterly:
                                    objKPI.ReportingMonthDateDay = RptDate.Value.Day;
                                    objKPI.ReportingMonth = RptDate.Value.Month;
                                    objKPI.ReportingFrequencyName = "Quarterly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                                    objKPI.ReportingMonthDateDay = RptDate.Value.Day;
                                    objKPI.ReportingMonth = RptDate.Value.Month;
                                    objKPI.ReportingFrequencyName = "Half_Yearly";
                                    break;
                                case (int)CommonActivity.ReportingFrequency.Yearly:
                                    objKPI.ReportingFrequencyName = "Yearly";
                                    objKPI.ReportingMonthDateDay = RptDate.Value.Day;
                                    objKPI.ReportingMonth = RptDate.Value.Month;
                                    break;
                            }
                        }
                    }
                    else
                    { objKPI.ReportingFrequencyName = "Never"; }
                }
            }
            catch (Exception ex)
            {
            }
            return objKPI;
        }

        /// <summary>
        /// Function used to update KPI details based on KPIId
        /// </summary>
        /// <param name="objKPI"></param>
        /// <param name="loggedUserId"></param>
        public void UpdateKPI(KPI objKPI, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbKPI set 
                        KPIName = @KPIName ,
                        Description = @Description ,
                        KPIPeriodicity = @KPIPeriodicity ,
                        StartDate = @StartDate ,
                        EndDate = @EndDate ,
                        WorkflowID = @WorkflowID ,
                        SurveyID=@SurveyID,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ID =@KPIId; ";
                        _connection.Execute(query, new
                        {
                            KPIId = objKPI.KPIId,
                            KPIName = objKPI.KPIName,
                            Description = objKPI.Description,
                            KPIPeriodicity = objKPI.KPIPeriodicity,
                            StartDate = objKPI.StartDate,
                            EndDate = objKPI.EndDate,
                            WorkflowID = objKPI.WorkflowID,
                            SurveyID = objKPI.SurveyID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });
                        if (objKPI.KPIId != 0)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = objKPI.ProjectID;
                            objLog.AuditFrom = "KPI";
                            objLog.AuditEvent = "EDIT";
                            objLog.AuditFromID = objKPI.KPIId;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Updated KPI details named '" + objKPI.KPIName + "'.";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
                        }
                        //if (objKPI.KPIId != 0)
                        //{

                        //    query = "INSERT INTO TbKPIActivityLog(ProjectId,KPIId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@KPIId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = objKPI.ProjectID,
                        //        KPIId = objKPI.KPIId,
                        //        Message = "Updated risk " + objKPI.KPIName ,
                        //        CreatedBy = loggedUserId,

                        //    });
                        //}
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to fetch log details (Outcome) based on ProjectId,OutcomeId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="OutcomeId"></param>
        /// <returns></returns>
        public IEnumerable<ActivityLog> GetKPIActivityLog(int ProjectId, int OutcomeId)
        {
            List<ActivityLog> lstUsers = new List<ActivityLog>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"Select  RAL.CreatedBy UserId ,U.Name ,RAL.Message ,RAL.CreatedDate,IFNULL(U.ProfileImage, '') AS ProfileImage
                                from TbProjectAuditLog RAL Inner Join TbUser U  On U.ID =RAL.CreatedBy 
                                Where RAL.ProjectId =@ProjectId and RAL.AuditFromID=@OutcomeId And RAL.AuditFrom = 'Outcome' order by RAL.CreatedDate desc";

                    lstUsers = _connection.Query<ActivityLog>(strQuery, new { ProjectId = ProjectId, OutcomeId = OutcomeId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstUsers;
        }

        /// <summary>
        /// Function used to update workflow in KPI based on KPIId
        /// </summary>
        /// <param name="WorkFlowID"></param>
        /// <param name="ProjectId"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="KPIId"></param>
        public void UpdateKPIWorkflow(int WorkFlowID, int ProjectId, int loggedUserId, int KPIId)
        {
            string query = string.Empty;
            try
            {
                using (var _Tran = new System.Transactions.TransactionScope())
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"Update TbKPI set 
                        WorkflowID = @WorkflowID ,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where ID =@KPIId; ";
                        _connection.Execute(query, new
                        {
                            KPIId = KPIId,
                            WorkflowID = WorkFlowID,
                            updatedDate = DateTime.Now,
                            userID = loggedUserId
                        });
                        if (KPIId != 0)
                        {
                            ProjectAuditLog objLog = new ProjectAuditLog();
                            objLog.ProjectID = ProjectId;
                            objLog.AuditFrom = "KPI";
                            objLog.AuditEvent = "EDIT";
                            objLog.AuditFromID = KPIId;
                            objLog.FromIDColumnName = "ID";
                            objLog.Message = "Updated KPI workflow";
                            objLog.CreatedBy = loggedUserId;
                            AddAuditLog(objLog);
                        }
                        //if (KPIId != 0)
                        //{

                        //    query = "INSERT INTO TbKPIActivityLog(ProjectId,KPIId,Message,CreatedBy,CreatedDate)";
                        //    query += " VALUES(@ProjectId,@KPIId,@Message,@CreatedBy,now());";
                        //    _connection.Query<int>(query, new
                        //    {
                        //        ProjectId = ProjectId,
                        //        KPIId = KPIId,
                        //        Message = "Assign new workflow ",
                        //        CreatedBy = loggedUserId,

                        //    });
                        //}
                    }
                    _Tran.Complete();
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to fetch survey details based on ProjectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IEnumerable<ProjectSurvey> GetSurveyName(int ProjectId)
        {
            List<ProjectSurvey> objSurvey = new List<ProjectSurvey>();
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Select SurveyID ,SurveyName  From TbProjectSurvey 
                            Where  ProjectId=@projid and IsActive=1 ";
                    objSurvey = _connection.Query<ProjectSurvey>(query, new { projid = ProjectId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return objSurvey;
        }

        /// <summary>
        /// Function used to fetch KPI reporting frequency date details based on KPIId
        /// </summary>
        /// <param name="objKPI"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="KPIId"></param>
        /// <returns></returns>
        public bool GenerateReportingFrequencyTrackDate(KPI objKPI, int loggedUserId, int KPIId)
        {
            bool success = false;
            List<string> lstTrackDates = new List<string>();
            string query = string.Empty;

            bool isDeleted = DeleteKPITrackDate(objKPI.KPIId);
            try
            {

                using (var _Tran = new System.Transactions.TransactionScope())
                {

                    switch (objKPI.KPIPeriodicity)
                    {
                        case (int)CommonActivity.ReportingFrequency.Once:
                            DateTime? reportvalue = CommonActivity.GetDateTimeValue(objKPI.ReportingOnceDate);
                            lstTrackDates.Add(reportvalue.Value.ToString("yyyy-MM-dd"));
                            break;
                        case (int)CommonActivity.ReportingFrequency.Daily:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, 0, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Weekly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingWeek, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Monthly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonthDay, 0);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Quarterly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonthDateDay, objKPI.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Half_Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonthDateDay, objKPI.ReportingMonth);
                            break;
                        case (int)CommonActivity.ReportingFrequency.Yearly:
                            lstTrackDates = CommonActivity.GetReportingFrequencyDateList(objKPI.StartDate, objKPI.EndDate, objKPI.KPIPeriodicity, objKPI.ReportingMonthDateDay, objKPI.ReportingMonth);
                            break;
                    }

                    if (lstTrackDates != null && lstTrackDates.Count > 0)
                    {
                        query = "INSERT INTO TbKPITrackDate(TrackDate,KPIID,TrackStatus) ";

                        for (int i = 0; i < lstTrackDates.Count; i++)
                        {
                            query += i > 0 ? " UNION  SELECT '" + lstTrackDates[i] + "'," + KPIId + ",0" : " SELECT '" + lstTrackDates[i] + "'," + KPIId + ",0";
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

                    _Tran.Complete();
                }
                }
            catch (Exception ex)
            {
            }
            return success;
        }

        /// <summary>
        /// Function used to delete KPI track date based on KPIID
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public bool DeleteKPITrackDate(int KPIID)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "DELETE FROM TbKPITrackDate WHERE KPIID=@KPIID ";
                    _connection.Execute(strQuery, new { KPIID = KPIID });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }
              
        /// <summary>
        /// Function used to save KPI tag details 
        /// </summary>
        /// <param name="KPIID"></param>
        /// <param name="TagID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string SaveProjectKPITag(int KPIID, int TagID, int loggedUserId)
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
                        strQuery = @"SELECT EXISTS(SELECT ID FROM TbKPITag WHERE KPIID=@KPIID and TagID=@tagid);";
                        id = _connection.Query<int>(strQuery, new { KPIID = KPIID, tagid = TagID }).Single();

                        if (id > 0)
                        {
                            strQuery = "Update TbKPITag SET IsDeleted = @IsDeleted,UpdatedDate = @updatedDate,UpdatedBy =@loggedUserId ";
                            strQuery += " Where KPIID =@KPIID and TagID =@tagid";
                            _connection.Execute(strQuery, new { updatedDate = DateTime.Now, loggedUserId = loggedUserId, KPIID = KPIID, tagid = TagID, IsDeleted = 0 });
                        }
                        else
                        {
                            strQuery = "INSERT INTO TbKPITag(KPIID,TagID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted)";
                            strQuery += " VALUES(@KPIID,@TagID,@CreatedDate,@loggedUserId,@CreatedDate,@loggedUserId,@IsDeleted); SELECT LAST_INSERT_ID();";
                            id = _connection.Query<int>(strQuery, new { KPIID = KPIID, TagID = TagID, loggedUserId = loggedUserId, CreatedDate = DateTime.Now, IsDeleted = 0 }).Single();
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
        /// Function used to delete KPI tag based on KPIID
        /// </summary>
        /// <param name="KPIID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public bool DeleteProjectKPITag(int KPIID, int loggedUserId)
        {
            string query = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string strQuery = "Update  TbKPITag SET IsDeleted =@IsDeleted ,UpdatedDate = @updatedDate ,UpdatedBy =@loggedUserId ";
                    strQuery += " Where KPIID =@KPIID ";
                    _connection.Execute(strQuery, new { KPIID = KPIID, IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId });
                }
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        /// <summary>
        /// Function used to delete old assign users and add new assign users
        /// </summary>
        /// <param name="objKPI"></param>
        /// <param name="loggeduserid"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        public bool AssignUserstoKPI(KPI objKPI, int loggeduserid,bool notify)
        {
            using (var _Tran = new System.Transactions.TransactionScope())
            {
                try
                {
                    if (!string.IsNullOrEmpty(objKPI.Users))
                    {
                        DeleteKPIUser(objKPI.KPIId);

                        List<userGroupList> lstNotifyUser = JsonConvert.DeserializeObject<List<userGroupList>>(objKPI.Users);
                        foreach (userGroupList userItem in lstNotifyUser)
                        {
                            InsertKPINotifyUsers(objKPI.ProjectID, objKPI.KPIId, userItem.userId, loggeduserid);
                            if (notify)
                            {
                                bool bStatus = false;
                                EmailActivity objEmail = new EmailActivity();
                                GroupNotify obj = new GroupNotify();
                                obj.UserName = userItem.userName;
                                obj.UserEmailId = userItem.EmailId;
                                bStatus = objEmail.SendMail("PaymentTerm", obj);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }
                _Tran.Complete();
            }
            return true;
        }
     
        /// <summary>
        /// Function used to delete old KPI tags and add new KPI tags
        /// </summary>
        /// <param name="objKPI"></param>
        /// <param name="loggeduserid"></param>
        /// <returns></returns>
        public bool AssignTagstoKPI(KPI objKPI, int loggeduserid)
        {
            using (var _Tran = new System.Transactions.TransactionScope())
            {
                try
                {
                    if (!string.IsNullOrEmpty(objKPI.Tags))
                    {
                        bool success = DeleteProjectKPITag(objKPI.KPIId, loggeduserid);

                        List<Tags> lstNotifyUser = JsonConvert.DeserializeObject<List<Tags>>(objKPI.Tags);
                        foreach (Tags tagItem in lstNotifyUser)
                        {
                            SaveProjectKPITag(objKPI.KPIId, tagItem.TagID, loggeduserid);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                _Tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// Function used to fetch KPI tags based on KPIID
        /// </summary>
        /// <param name="KPIID"></param>
        /// <returns></returns>
        public string GetKPIAssignedTags(int KPIID = 0)
        {
            string AssignedTags = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = @"select tgs.TagName,TKT.TagID,tgs.TagColor from TbKPITag TKT inner join TbProjectTags tgs on TKT.TagID=tgs.TagID ";
                    query += "where TKT.KPIID=@ID and TKT.IsDeleted=0  Order By TKT.ID Desc";

                    var asndTags = _connection.Query<Tags>(query, new
                    {
                        ID = KPIID
                    }).ToList();

                    AssignedTags = JsonConvert.SerializeObject(asndTags);
                }
            }
            catch (Exception ex)
            {
            }
            return AssignedTags;
        }

        /// <summary>
        /// Function used to fetch surveyid based on ProjectId,SurveyName
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="SurveyName"></param>
        /// <returns></returns>
        public int GetSurveyIdByName(int ProjectId, string SurveyName)
        {
            int SurveyID = 0;
            try
            {
                using (_connection = Database.GetDBConnection())
                {
                    string query = "SELECT SurveyID FROM TbProjectSurvey WHERE LOWER(TRIM(SurveyName)) = @SurveyName AND ProjectId = @ProjectId AND IFNULL(IsActive,1)=@IsActive;";
                    SurveyID = _connection.Query<int>(query, new { SurveyName = SurveyName.ToLower(), ProjectId = ProjectId, IsActive = 1 }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return SurveyID;
        }

        #endregion ManageProjectKPI

        #region AuditLog

        /// <summary>
        /// Add new auditlog details
        /// </summary>
        /// <param name="objPrjAuditLog"></param>
        /// <returns></returns>
        public static string AddAuditLog(ProjectAuditLog objPrjAuditLog)            
        {
            string strQuery = string.Empty;
            var retStatus = "success";
            var id = 0;

            using (_conn = Utils.Database.GetDBConnection())
            {
                try
                {
                    strQuery = " INSERT INTO TbProjectAuditLog(ProjectID,AuditFrom,AuditEvent,AuditFromID,FromIDColumnName,Message,CreatedDate,CreatedBy) " +
                               "  VALUES(@ProjectID,@AuditFrom,@AuditEvent,@AuditFromID,@FromIDColumnName,@Message,now(),@loggedUserId);SELECT LAST_INSERT_ID();";
                    id = _conn.Query<int>(strQuery, new
                    {                       
                        ProjectID = objPrjAuditLog.ProjectID,
                        AuditFrom = objPrjAuditLog.AuditFrom ,
                        AuditEvent = objPrjAuditLog.AuditEvent,
                        AuditFromID= objPrjAuditLog.AuditFromID,
                        FromIDColumnName = objPrjAuditLog.FromIDColumnName,
                        Message = objPrjAuditLog.Message,
                        loggedUserId = objPrjAuditLog.CreatedBy                       
                    }).Single();
                }

                catch (Exception ex)
                {
                    retStatus = "Failed while saving audit log...";
                    throw ex;
                }
            }
            return retStatus;
        }

        #endregion AuditLog

    }
}
