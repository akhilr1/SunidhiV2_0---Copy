using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.DAL
{
    public class MyProjectDAL
    {
        protected IDbConnection _connection;

        #region CreateProject

        /// <summary>
        /// Next Project Code -> -> function used to increment Project Code with maximum 6 length based on ProjectId
        /// </summary>
        /// <returns></returns>
        public string GetNextProjectCode()
        {
            int nextID = 0;
            string nextCode = "";
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
        /// Used to fetch State details based on CountryID
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
        /// Get Project Themelist -> function used to fetch project themelist based on InitiativeID
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

                    strQuery = @"SELECT Distinct IP.ThemeID ,T.ThemeName Theme,Count(case when IP.ProjectTemplateID =0 then null else IP.ProjectTemplateID end) NoOfTemplates ,0 IsSelected " +
                                ",IFNULL(T.ThemeImage,'') AS TemplateImage From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID " +
                                " Where IP.IsDeleted =0 And IP.InitiativeId = @InitiativeID  Group By IP.ThemeID ,T.ThemeName";
                    lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { InitiativeID = InitiativeID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstPrjTemp;
        }

        /// <summary>
        /// Get Project Themelist -> function used to fetch project themelist based on InitiativeID,ThemeID
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
                    if (NoOfTemplates != 0)
                        strQuery = @"SELECT Distinct IP.ThemeID ,PT.ProjectTemplateID ProjectTemplateID,PT.ProjectTemplateName TemplateName,PT.Description ,0 IsSelected,IFNULL(PT.TemplateImage,'') AS TemplateImage " +
                                    " From TbInitiativeToProjectTemplates IP left join TbProjectTheme T On T.ThemeID =IP.ThemeID  " +
                                    " left join  TbProjectTemplate PT On IP.ProjectTemplateID= PT.ProjectTemplateID Where IP.ThemeID =@ThemeID  And IP.InitiativeId =@InitiativeID";
                    else
                        strQuery = @"SELECT Distinct T.ThemeID ,T.ThemeName Theme ,PT.ProjectTemplateID ProjectTemplateID," +
                                    " PT.ProjectTemplateName TemplateName,PT.Description ,0 IsSelected,IFNULL(PT.TemplateImage,'') AS TemplateImage From TbProjectTemplate PT " +
                                    "inner join TbProjectTheme T On T.ThemeID = PT.ThemeID Where PT.IsActive =1 And PT.ThemeID =@ThemeID ";
                    lstPrjTemp = _connection.Query<ProjectTemplate>(strQuery, new { ThemeID = ThemeID, InitiativeID = InitiativeID }).ToList();
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
            var retStatus = "";

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

        /// <summary>
        /// Used to fetch Initiative details based on WorkspaceID,loggedUserId
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public IEnumerable<Project> GetInitiative(int WorkspaceID,int loggedUserId)
        {
            List<Project> lstItems = new List<Project>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"SELECT Distinct InitiativeId, (select InitiativeName from TbInitiative where InitiativeId=ITU.InitiativeId And WorkspaceID =@WorkspaceID) as Initiative  
                    //         From TbInitiativeToUsers ITU Where UserID =@loggedUserId  Order By Initiative ";

                    strQuery = @"SELECT Distinct ITU.InitiativeId,t.InitiativeName as Initiative From TbInitiativeToUsers ITU left outer join TbInitiative t on t.InitiativeId=ITU.InitiativeId And t.WorkspaceID = @WorkspaceID
                                    Where ITU.UserID =@loggedUserId  Order By t.InitiativeName";
                    lstItems = _connection.Query<Project>(strQuery, new { WorkspaceID = WorkspaceID, loggedUserId = loggedUserId }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        #endregion CreateProject

        #region DeployAndArchive

        /// <summary>
        /// Used to fetch project details based on stage,loggedUserId,WorkspaceID
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<Project> GetProjectListByInitID(string stage, int loggedUserId,int WorkspaceID)
        {
            List<Project> lstProjects = new List<Project>();
            DashboardDAL objDashboard = new DashboardDAL();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    //strQuery = @"SELECT ProjectId,ProjectName,StartDate,EndDate,Budget,TP.IsActive,TP.Code,TP.InitiativeId,TI.InitiativeName as Initiative ,TI.WorkspaceID as WorkspaceID FROM  
                    //             TbProject TP inner join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 and  ProjectStatus=@stage and WorkspaceID=@WorkspaceID and
                    //       (TP.CreatedBy =@loggedUserId or GroupId in (select GroupId from TbUserAssignedToGroup where UserId=@loggedUserId)) Order by ProjectId Desc";
                    //lstProjects = _connection.Query<Project>(strQuery, new { stage = stage, loggedUserId = loggedUserId , WorkspaceID = WorkspaceID }).ToList();

                    //strQuery = @"SELECT ProjectId,ProjectName,StartDate,EndDate,Budget,TP.IsActive,TP.Code,TP.InitiativeId,IFNULL(Progress,0) as Progress,ProjectStatus,
                    //        TI.InitiativeName as Initiative ,(Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) as WorkspaceID FROM  TbProject TP left join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 and  ProjectStatus=@stage and (Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) =@WorkspaceID  and (TP.CreatedBy =@loggedUserId or TP.GroupId in (select GroupId from TbUserAssignedToGroup where UserId=@loggedUserId)) And TP.IsCreated = 1 Order by ProjectId Desc";

                    strQuery = @"SELECT TP.ProjectId,TP.ProjectName,TP.StartDate,TP.EndDate,TP.Budget,TP.IsActive,TP.Code,TP.InitiativeId,CASE WHEN IFNULL(TP.Weightage,0) >0 AND IFNULL(TP.OutputProgress,0) > 0 THEN ROUND((TP.OutputProgress/TP.Weightage)*100)  ELSE 0 END Progress,TP.ProjectStatus,
                                 TI.InitiativeName as Initiative ,(Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) as WorkspaceID FROM 
                                 TbProject TP left join TbInitiative TI on TP.InitiativeId=TI.InitiativeId where TP.IsDeleted=0 
                                  and TP.ProjectStatus=@stage and (Case when ifnull(TP.WSID,0) > 0 then TP.WSID else TI.WorkspaceID End ) = @WorkspaceID
                                 and (TP.CreatedBy =@loggedUserId or exists (select p.GroupId from TbUserAssignedToGroup p where p.UserId=@loggedUserId and p.GroupId = TP.GroupId)
                                 ) And TP.IsCreated = 1 Order by TP.ProjectId Desc";


                    string strQueryexec = @"SELECT  ifnull(DATE_FORMAT(ExecutedDateTime, '%d-%m-%Y %H:%i:%s'),'')  From TbOutputScheduler order by ID desc limit 1";
                    string stringifydate = _connection.Query<string>(strQueryexec).FirstOrDefault();


                    lstProjects = _connection.Query<Project>(strQuery, new { stage = stage, loggedUserId = loggedUserId, WorkspaceID = WorkspaceID }).ToList();
                   
                    if(lstProjects != null && lstProjects.Count >0 && stringifydate != null && stringifydate.Length >0)
                    {
                        lstProjects[0].ExecutedDateTime = stringifydate;
                    }
                    //foreach (Project Item in lstProjects)
                    //{
                    //    Item.Progress = objDashboard.GetProjectProgress(Item.ProjectId);
                    //}
                }
            }
            catch (Exception ex)
            {
            }
            return lstProjects;
        }

        /// <summary>
        /// Delete Project -> function used to delete project based on ProjectID
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
        /// Used to change project status based on ProjectID
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
        /// Used to change project state based on ProjectID
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
        
        /// <summary>
        /// Used to fetch project themelist
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectTheme> GetTheme()
        {
            List<ProjectTheme> lstItems = new List<ProjectTheme>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT Distinct ThemeID,ThemeName  
                             From TbProjectTheme Order By ThemeName ";
                    lstItems = _connection.Query<ProjectTheme>(strQuery).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItems;
        }

        /// <summary>
        /// Add project template
        /// </summary>
        /// <param name="ThemeID"></param>
        /// <param name="ProjectTemplateName"></param>
        /// <param name="Description"></param>
        /// <param name="loggedUserId"></param>
        /// <param name="ProjectID"></param>
        /// <param name="coverName"></param>
        /// <param name="coverOrginalName"></param>
        public void InsertProjectTemplate(int ThemeID,string ProjectTemplateName,string Description, int loggedUserId,int ProjectID, string coverName, string coverOrginalName)
        {
            string query = string.Empty;
            var id = 0;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {


                    query = @"SELECT EXISTS(SELECT ProjectTemplateID from TbProjectTemplate WHERE ProjectId=@ProjectId);";
                    id = _connection.Query<int>(query, new { ProjectId = ProjectID }).Single();

                    if (id == 0)
                    {
                        query = "INSERT INTO TbProjectTemplate(ThemeID,ProjectTemplateName,Description,IsActive,ProjectId,CreatedDate,UpdatedDate,CreatedBy,UniqueCoverName,CoverName)";
                        query += " VALUES(@ThemeID,@ProjectTemplateName,@Description,@IsActive,@ProjectId,now(),now(),@CreatedBy,@coverName,@coverOrginalName); SELECT LAST_INSERT_ID();";
                        id = _connection.Query<int>(query, new
                        {
                            ThemeID = ThemeID,
                            ProjectTemplateName = ProjectTemplateName,
                            Description = Description,
                            IsActive = 1,
                            ProjectId = ProjectID,
                            CreatedBy = loggedUserId,
                            coverName = coverName,
                            coverOrginalName = coverOrginalName,

                        }).Single();
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return ;
        }

        /// <summary>
        /// Used to fetch project template based on ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public IEnumerable<InitiativeProjectTemplate> GetProjectTemplateByprjtID(int ProjectID)
        {
            List<InitiativeProjectTemplate> lstItems = new List<InitiativeProjectTemplate>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    strQuery = @"SELECT  ProjectTemplateID,ThemeID,ProjectTemplateName  
                             From TbProjectTemplate WHERE ProjectId=@ProjectId ";
                    lstItems = _connection.Query<InitiativeProjectTemplate>(strQuery, new { ProjectId = ProjectID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return lstItems;
        }
      
        #endregion DeployAndArchive

        #region ManageProjectDetails

        /// <summary>
        /// Used to fetch project details based on prjID,initID
        /// </summary>
        /// <param name="prjID"></param>
        /// <param name="initID"></param>
        /// <returns></returns>
        public Project GetProjectDetailsByID(int prjID, int initID)
        {
            Project objProject = new Project();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"SELECT t.StartDate,t.EndDate,t.ProjectId,t.InitiativeId,t.ProjectName,t.GroupId,t.Progress,t.Description,t.Code,t.Budget,t.CountryID,t.StateID,t.DistrictID,t.PaymentTermAmount FROM  TbProject t where t.ProjectId=@prjid and t.InitiativeId=@initid";
                    objProject = _connection.Query<Project>(strQuery, new
                    {
                        prjid = prjID,
                        initid = initID
                    }).FirstOrDefault();

                    var projectTags = new List<Tags>();
                    //strQuery = @"select TagID , TagName, TagColor from TbProjectTags where TagID in 
                    //        ( select TagID from TbProjectToTagList where IsDeleted <>1 and ProjectId=@projid)  Order By TagID Desc";

                    strQuery = @"select t.TagID , t.TagName, t.TagColor from TbProjectTags t inner join TbProjectToTagList p 
                                where p.IsDeleted <>1 and p.ProjectId=@projid and t.TagID = p.TagID Order By t.TagID Desc";

                    projectTags = _connection.Query<Tags>(strQuery, new
                    {
                        projid = prjID
                    }).ToList();
                    objProject.ProjectTags = JsonConvert.SerializeObject(projectTags);
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
        /// Edit project details -> function used to update project details based on projectID & existing project taglist deleted & create new projet taglist
        /// </summary>
        /// <param name="inputProject"></param>
        /// <param name="loguser"></param>
        public void UpdateEditProjectDetails(Project inputProject, int loguser)
        {
            string query = string.Empty;
            //DateTime? startDate = inputProject.StartDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.StartDate) : null;
            //DateTime? endDate = inputProject.EndDate != null ? Code.Common.CommonActivity.GetDateTimeValue(inputProject.EndDate) : null;
            using (var _Tran = new System.Transactions.TransactionScope())
            {
                try
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
                            description = inputProject.Description,
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
                }
                catch (Exception ex)
                {
                }
                _Tran.Complete();
            }
            return;
        }
        
        #endregion ManageProjectDetails
    }
}
