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
using Dapper;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace SunidhiV2_0.Code.DAL
{
    public class InitiativeTemplateDAL
    {
        protected IDbConnection _connection;

        public List<ProjectTheme> GetProjectThemeByIntitiativeID(int InitiativeId)
        {
            List<ProjectTheme> objProjectThemeList = new List<ProjectTheme>();
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"select ThemeID,ThemeName from TbProjectTheme where IsDeleted=0 and ThemeID in 
                            (select distinct ThemeID from TbInitiativeToProjectTemplates where InitiativeId=@initid and IsDeleted=0) Order By ThemeID Desc";
                objProjectThemeList = _connection.Query<ProjectTheme>(query,new { initid=InitiativeId}).ToList();

                foreach (ProjectTheme themeItem in objProjectThemeList)
                {
                    themeItem.ProjectTemplateList = new List<InitiativeProjectTemplate>();
                    query = @"select * from TbProjectTemplate where IsDeleted=0 and ProjectTemplateID in 
                                (select distinct ProjectTemplateID from TbInitiativeToProjectTemplates where IsDeleted=0 and ThemeID=@themeid and InitiativeId=@initid)";
                    themeItem.ProjectTemplateList = _connection.Query<InitiativeProjectTemplate>(query, new { themeid= themeItem.ThemeID, initid = InitiativeId }).ToList();
                }
            }
            return objProjectThemeList;
        }

        public void RemoveProjectThemeFromInitiative(int loggedUserId, string ThemeIDList, int InitiaveId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiativeToProjectTemplates Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggedUserId 
                              Where InitiativeId=@initid and FIND_IN_SET(ThemeID,@themeidlist)";
                _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId,
                                                  initid=InitiaveId, themeidlist= ThemeIDList });
            }
            return;
        }
        public IEnumerable<ProjectTheme> GetAllProjectTheme()
        {
            List<ProjectTheme> lstTheme = new List<ProjectTheme>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT ThemeID,ThemeName FROM TbProjectTheme Where IsDeleted=0 Order By ThemeID Desc";
                lstTheme = _connection.Query<ProjectTheme>(strQuery).ToList();
            }
            return lstTheme;
        }

        public void AddProjectThemeToInitiative(int loggedUserId, string ThemeIDList, int initid)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                List<ProjectTheme> objThemeIDList = JsonConvert.DeserializeObject<List<ProjectTheme>>(ThemeIDList);
                foreach (ProjectTheme themeItem in objThemeIDList)
                {
                   query = @"INSERT INTO TbInitiativeToProjectTemplates (InitiativeId, ThemeID, ProjectTemplateID, IsDeleted, CreatedDate, CreatedBy)
                            VALUES (@InitiativeId, @ThemeID, @ProjectTemplateID, @IsDeleted, now(), @CreatedBy); SELECT LAST_INSERT_ID();";
                    _connection.Query<int>(query, new
                    {
                        InitiativeId = initid,
                        ThemeID = themeItem.ThemeID,
                        ProjectTemplateID = 0,
                        IsDeleted = 0,
                        CreatedBy = loggedUserId
                    }).Single();
                }
            }
            return;
        }

        public void RemoveProjectTemplateFromInitiative(int loggedUserId, string TemplateIDList, int InitiaveId)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiativeToProjectTemplates Set IsDeleted=@IsDeleted, UpdatedDate=@updatedDate,UpdatedBy=@loggedUserId 
                              Where InitiativeId=@initid and FIND_IN_SET(ProjectTemplateID,@templateidlist)";
                _connection.Execute(query, new { IsDeleted = 1, updatedDate = DateTime.Now, loggedUserId = loggedUserId,
                                                  initid = InitiaveId, templateidlist = TemplateIDList });
            }
            return;
        }

        public IEnumerable<InitiativeProjectTemplate> GetAllProjectTemplateByThemeID(int ThemeID)
        {
            List<InitiativeProjectTemplate> lstTemplate = new List<InitiativeProjectTemplate>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT ProjectTemplateID,ProjectTemplateName,ThemeID,Description FROM TbProjectTemplate Where IsDeleted=0 and ThemeID=@themeid Order By ProjectTemplateID Desc";
                lstTemplate = _connection.Query<InitiativeProjectTemplate>(strQuery,new { themeid= ThemeID }).ToList();
            }
            return lstTemplate;
        }

        public void AddProjectTemplateToInitiative(int loggeduserid, string templatelist, int InitiativeId, int ThemeID)
        {
            string query = string.Empty;

            using (_connection = Utils.Database.GetDBConnection())
            {
                List<InitiativeProjectTemplate> objTemplateList = JsonConvert.DeserializeObject<List<InitiativeProjectTemplate>>(templatelist);
                foreach (InitiativeProjectTemplate templateItem in objTemplateList)
                {
                    query = @"INSERT INTO TbInitiativeToProjectTemplates (InitiativeId, ThemeID, ProjectTemplateID, IsDeleted, CreatedDate, CreatedBy)
                            VALUES (@initid, @themeid, @projtempid, @isdeleted, now(), @createdby); SELECT LAST_INSERT_ID();";
                    _connection.Query<int>(query, new
                    {
                        initid = InitiativeId,
                        themeid= ThemeID,
                        projtempid = templateItem.ProjectTemplateID,
                        isdeleted = 0,
                        createdby = loggeduserid
                    }).Single();
                }
            }
            return;
        }
    }
}
