using System;
using System.Collections.Generic;
using System.Linq;
using SunidhiV2_0.Models;
using System.Data;
using Dapper;

namespace SunidhiV2_0.Code.DAL
{
    public class ProjectTagsDAL
    {
        protected IDbConnection _connection;

        #region ProjectExpenseTags

        /// <summary>
        /// Function used to add project expense tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="projID"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public int AddNewExpenseTag(string tagName, int projID, int loggedUserId)
        {
            string strQuery = string.Empty;
            var tagID = 0;

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    // Saving New Expense Tag 
                    strQuery = "INSERT INTO TbProjectExpenseTags( ProjectId,TagName,CreatedDate,CreatedBy)" +
                                " VALUES(@projid,@tagname,@createdate,@createby); SELECT LAST_INSERT_ID();";
                    tagID = _connection.Query<int>(strQuery, new
                    {
                        projid = projID,
                        tagname = tagName,
                        createdate = DateTime.Now,
                        createby = loggedUserId
                    }).Single();
                }
            }
            catch (Exception ex)
            {
            }
            return tagID;
        }

        /// <summary>
        /// Function used to update project expense tag based on tagID
        /// </summary>
        /// <param name="tagID"></param>
        /// <param name="tagName"></param>
        /// <param name="loggedUserId"></param>
        public void EditExpenseTag(int tagID, string tagName, int loggedUserId)
        {
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    // Saving New Expense Tag 
                    strQuery = @"UPDATE TbProjectExpenseTags SET TagName=@tagname,UpdatedDate=@updateddate,UpdatedBy=@updatedby WHERE TagID=@tagid ";
                    tagID = _connection.Execute(strQuery, new
                    {
                        tagid = tagID,
                        tagname = tagName,
                        updateddate = DateTime.Now,
                        updatedby = loggedUserId
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return;
        }

        /// <summary>
        /// Function used to fetch all project expense tag based on projID
        /// </summary>
        /// <param name="projID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectExpenseTags> GetAllExpenseTags(int projID)
        {
            List<ProjectExpenseTags> lstExpenseTags = new List<ProjectExpenseTags>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    strQuery = @"select TagName,TagID from TbProjectExpenseTags where ProjectId=@projid";
                    lstExpenseTags = _connection.Query<ProjectExpenseTags>(strQuery, new { projid = projID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstExpenseTags;
        }

        #endregion ProjectExpenseTags

        #region ProjectTags

        /// <summary>
        /// Function used to add new project tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="tagColor"></param>
        /// <param name="WorkspaceID"></param>
        /// <param name="initId"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string CreateNewTag(string tagName, string tagColor, int WorkspaceID,int initId, int loggedUserId)
        {
            string strQuery = string.Empty;
            var tagID = 0;
            var retMsg = "success";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    // Saving New Expense Tag 
                    strQuery = "INSERT INTO TbProjectTags(TagName,TagColor,WSID,InitiativeID,CreatedDate,CreatedBy)" +
                                " VALUES(@tagname,@tagcolor, @WorkspaceID,@initid,@createdate,@createby); SELECT LAST_INSERT_ID();";
                    try
                    {

                        tagID = _connection.Query<int>(strQuery, new
                        {
                            tagname = tagName,
                            tagcolor = tagColor,
                            WorkspaceID = WorkspaceID,
                            initid = initId,
                            createdate = DateTime.Now,
                            createby = loggedUserId
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
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        /// <summary>
        /// Function used to update project tag based on tagID
        /// </summary>
        /// <param name="tagID"></param>
        /// <param name="tagName"></param>
        /// <param name="tagColor"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public string UpdateTagDetails(int tagID, string tagName, string tagColor, int loggedUserId)
        {
            string query = string.Empty;
            var retMsg = "success";
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {

                    string strQuery = @"SELECT EXISTS(SELECT TagID FROM TbProjectTags WHERE TagName=@tagname and TagID!=@tagid);";
                    var id = _connection.Query<int>(strQuery, new { tagname = tagName, tagid = tagID }).Single();

                    if (id == 0)
                    {
                        query = @"Update TbProjectTags set 
                        TagName = @tagname ,
                        TagColor=@tagcolor,
                        UpdatedDate = @updatedDate,
                        UpdatedBy=@userID where TagID=@tagid; ";
                        try
                        {
                            _connection.Execute(query, new
                            {
                                tagid = tagID,
                                tagname = tagName,
                                tagcolor = tagColor,
                                updatedDate = DateTime.Now,
                                userID = loggedUserId
                            });
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
                    else
                    {
                        retMsg = "Duplicate Entry";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return retMsg;
        }

        /// <summary>
        /// Function used to fetch project tags based on WorkspaceID
        /// </summary>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public IEnumerable<ProjectTags> GetAllProjectTags(int WorkspaceID)
        {
            List<ProjectTags> lstProjectTags = new List<ProjectTags>();
            string strQuery = string.Empty;
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    // strQuery = @"select * from TbProjectTags where InitiativeID=@initid order by TagID desc";
                    strQuery = @"select TagName,TagID,TagColor from TbProjectTags where WSID=@WorkspaceID and IsDeleted = 0 order by TagID desc";
                    lstProjectTags = _connection.Query<ProjectTags>(strQuery, new { WorkspaceID = WorkspaceID }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstProjectTags;
        }

        /// <summary>
        /// Function used to delete project tag based on TagID
        /// </summary>
        /// <param name="TagID"></param>
        /// <param name="WorkspaceID"></param>
        /// <returns></returns>
        public bool DeleteProjectTag(int TagID, int WorkspaceID)
        {
            string query = string.Empty;
            var id = 0;
            var returnVal = false;
            try
            {
                if (TagID > 0)
                {
                    using (_connection = Utils.Database.GetDBConnection())
                    {
                        query = @"select Exists(select ID from TbProjectToTagList where TagID = @TagID and isDeleted = 0)";
                        id = _connection.Query<int>(query, new
                        {
                            TagID = TagID,
                        }).Single();

                        if (id == 0)
                        {
                            query = @"Update TbProjectTags set IsDeleted = 1 where TagID = @TagID  and WSID = @WorkspaceID";
                            _connection.Execute(query, new
                            {
                                TagID = TagID,
                                WorkspaceID = WorkspaceID
                            });
                        }
                    }
                    if (id > 0)
                    {
                        returnVal = false;
                    }
                    else { returnVal = true; }
                }
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }
        #endregion ProjectTags
    }
}
