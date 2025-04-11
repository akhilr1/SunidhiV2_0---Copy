using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TagsDAL
    {
        protected IDbConnection _connection;

        public IEnumerable<Tags> GetTagDetails()
        {
            List<Tags> lstTags = new List<Tags>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct TagID,TagName, TagColor FROM  TbTags  order by TagID desc";
                lstTags = _connection.Query<Tags>(strQuery).ToList();
            }
            return lstTags;
        }

        public int AddNewTag(string TagName, string TagColor, int loggedUserId)
        {
            string strQuery = string.Empty;
            var tagID = 0;

            using (_connection = Utils.Database.GetDBConnection())
            {
                // Saving New Tag 
                strQuery = "INSERT INTO TbTags( TagName,TagColor,CreatedDate,CreatedBy)" +
                            " VALUES(@TagName,@TagColor,now(),@loggedUserId); SELECT LAST_INSERT_ID();";
                try
                {
                    tagID = _connection.Query<int>(strQuery, new
                    {
                        TagName = TagName,
                        TagColor = TagColor,
                        loggedUserId = loggedUserId
                    }).Single(); 
                }
                catch (Exception ex)
                {
                    //retMsg = ex.Message;
                    tagID = 0;
                    throw ex;
                }
            }
            return tagID;
        }

        public Tags GetTagDetailsByName(string tagName)
        {
            string strQuery = string.Empty;
            Tags objTags = new Tags();
            using (_connection = Utils.Database.GetDBConnection())
            {
                strQuery = @"SELECT * FROM  TbProjectTags where TagName=@tagname";
                objTags = _connection.Query<Tags>(strQuery,new { tagname = tagName }).FirstOrDefault();
            }
            return objTags;
        }
        public void UpdateTagDetails(int TagID, string TagName,string TagColor, int loggedUserId)
        {
            string query = string.Empty;


            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbTags set 
                        TagName = @TagName ,
                        TagColor=@TagColor,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where TagID =@TagID; ";
                _connection.Execute(query, new
                {
                    TagID = TagID,
                    TagName = TagName,
                    TagColor= TagColor,
                    updatedDate = DateTime.Now,
                    userID = loggedUserId
                });

            }
            return;
        }

        #region Workspace Tags
        public IEnumerable<Tags> GetWSTagDetails()
        {
            List<Tags> lstWSTags = new List<Tags>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct WSTagID TagID ,TagName, TagColor FROM  TbWorkspaceTags order by WSTagID desc";
                lstWSTags = _connection.Query<Tags>(strQuery).ToList();
            }
            return lstWSTags;
        }
        public string UpdateWSTagDetails(int TagID, string TagName, string TagColor, int loggedUserId)
        {
            string query = string.Empty;
            string retMsg = "Success";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    query = @"Update TbWorkspaceTags set 
                        TagName = @TagName ,
                        TagColor=@TagColor,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where WSTagID =@TagID; ";
                    _connection.Execute(query, new
                    {
                        TagID = TagID,
                        TagName = TagName,
                        TagColor = TagColor,
                        updatedDate = DateTime.Now,
                        userID = loggedUserId
                    });

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                {
                    retMsg = "Duplicate Entry";
                }
                else
                {
                    retMsg = "Error while saving data.";
                    throw ex;
                }
            }

            return retMsg;
        }

       
        #endregion


        public IEnumerable<Tags> GetInitTagDetails(int WorkspaceId)
        {
            List<Tags> lstTags = new List<Tags>();
            string strQuery = string.Empty;
            using (_connection = Utils.Database.GetDBConnection())
            {

                strQuery = @"SELECT Distinct TagID,TagName, TagColor FROM  TbInitiativeTags where WorkspaceId=@WorkspaceId order by TagID desc";
                lstTags = _connection.Query<Tags>(strQuery,new { WorkspaceId= WorkspaceId }).ToList();
            }
            return lstTags;
        }
        public string AddInitNewWsTag(string TagName, string TagColor, int loggedUserId,int WorkspaceId)
        {
            string strQuery = string.Empty;
            var tagID = 0;
            var retMsg = "success";

            using (_connection = Utils.Database.GetDBConnection())
            {
                // Saving New Tag 
                strQuery = "INSERT INTO TbInitiativeTags(WorkspaceID,TagName,TagColor,CreatedDate,CreatedBy)" +
                            " VALUES(@WorkspaceId,@TagName,@TagColor,now(),@loggedUserId); SELECT LAST_INSERT_ID();";
                try
                {
                    tagID = _connection.Query<int>(strQuery, new
                    {
                        TagName = TagName,
                        TagColor = TagColor,
                        loggedUserId = loggedUserId,
                        WorkspaceId= WorkspaceId
                    }).Single();

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("UNIQUE"))
                    {
                        retMsg = "Duplicate Entry";
                    }
                    else
                    {
                        retMsg = "Error while saving data.";
                        throw ex;
                    }
                }
            }

            return retMsg;
        }
        public void UpdateInitTagDetails(int TagID, string TagName, string TagColor, int loggedUserId)
        {
            string query = string.Empty;


            using (_connection = Utils.Database.GetDBConnection())
            {
                query = @"Update TbInitiativeTags set 
                        TagName = @TagName ,
                        TagColor=@TagColor,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where TagID =@TagID; ";
                _connection.Execute(query, new
                {
                    TagID = TagID,
                    TagName = TagName,
                    TagColor = TagColor,
                    updatedDate = DateTime.Now,
                    userID = loggedUserId
                });

            }
            return;
        }
        public string AddNewProjectTag(string TagName, string TagColor, int loggedUserId,int InitiativeId)
        {
            string strQuery = string.Empty;
            var tagID = 0;
            var retMsg = "success";
            using (_connection = Utils.Database.GetDBConnection())
            {
                // Saving New Tag 
                strQuery = "INSERT INTO TbProjectTags( InitiativeId,TagName,TagColor,CreatedDate,CreatedBy)" +
                            " VALUES(@InitiativeId,@TagName,@TagColor,now(),@loggedUserId); SELECT LAST_INSERT_ID();";
                try
                {
                    tagID = _connection.Query<int>(strQuery, new
                    {
                        InitiativeId= InitiativeId,
                        TagName = TagName,
                        TagColor = TagColor,
                        loggedUserId = loggedUserId
                    }).Single();
                }
                catch (Exception ex)
                {

                    if (ex.Message.Contains("UNIQUE"))
                    {
                        retMsg = "Duplicate Entry";
                    }
                    else
                    {
                        retMsg = "Error while saving data.";
                        throw ex;
                    }
                }
            }
            return retMsg;
        }
    }
}
