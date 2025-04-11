using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunidhiV2_0.Models;
using SunidhiV2_0.Controllers;
using Dapper;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;
using SunidhiV2_0.Code.Utils;

namespace SunidhiV2_0.Code.DAL
{
    public class EmailDAL
    {
        protected IDbConnection _connection;

        #region GetEmailTemplateDetails
        /// <summary>
        /// GetEmailTemplateDetails
        /// </summary>
        /// <param name="ActionName"></param>
        /// <returns></returns>
        public EmailTemplate GetEmailTemplateDetails(string ActionName)
        {
            EmailTemplate objEmail = new EmailTemplate();

            //try
            //{
            using (_connection = Utils.Database.GetDBConnection())
            {
                objEmail = _connection.Query<EmailTemplate>(@"select TemplateId,TemplateName,TemplateContent,TemplateType,TemplateDescription,IsActive FROM TbTemplatesForEmail WHERE TemplateName=@templateName", new { templateName = ActionName }).FirstOrDefault();

                if (objEmail == null)
                {
                    return objEmail;
                }

                return objEmail;
            }
            //}
            //catch (Exception ex)
            //{
            //    return objEmail;
            //}
        }
        #endregion GetEmailTemplateDetails

        #region SaveEmailTrackLog
        /// <summary>
        /// ObjEmailTrackLog
        /// </summary>
        /// <param name="ObjEmailTrackLog"></param>
        public void SaveEmailTrackLog(EmailTrackLog ObjEmailTrackLog)
        {
            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = string.Empty;
                    query = "INSERT INTO TbEmailTrackLog(ToAddress,FromAddress,SectionType,CreateDate,LoggedUser)";
                    query += " VALUES(@ToAddress,@FromAddress,@SectionType,now(),@LoggedUser);";
                    _connection.Execute(query, new
                    {
                        ToAddress = ObjEmailTrackLog.ToAddress,
                        FromAddress = ObjEmailTrackLog.FromAddress,
                        SectionType = ObjEmailTrackLog.SectionType,
                        LoggedUser = ObjEmailTrackLog.LoggedUser
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion SaveEmailTrackLog

        #region GetTemplateDetails
        /// <summary>
        /// GetTemplateDetails
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <param name="LoggedUser"></param>
        /// <returns></returns>
        public SurveyEmailTemplate GetTemplateDetails(int SurveyId, int LoggedUser)
        {
            SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
            List<SurveyEmailUser> lstSurveyEmailUser = new List<SurveyEmailUser>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "SELECT IFNULL(wrk.WSName,'') WorkspaceName,ifnull(prj.ProjectName,'') ProjectName ";
                    query += ",ifnull(srv.SurveyName,'') SurveyName,wrk.WorkspaceID FROM TbWorkspace wrk JOIN TbProject prj  ";
                    query += " ON wrk.WorkspaceID = prj.WSID JOIN TbProjectSurvey srv ON srv.ProjectId = prj.ProjectId WHERE srv.SurveyID = @SurveyID";

                    objSurveyEmailTemplate = _connection.Query<SurveyEmailTemplate>(query, new
                    {
                        SurveyID = SurveyId
                    }).FirstOrDefault();

                    string query1 = "select usr.ID as userId ,ifnull(usr.Name,'') Name, 'Data Collector' as UserRole,0 as workflowLevel,usr.EmailId from TbProjectSurveyUser srvuser ";
                    query1 += " JOIN TbUser usr ON usr.ID = srvuser.UserID WHERE srvuser.SurveyID = @SurveyID  and ifnull(srvuser.IsDeleted,0) = 0 and usr.ID != @sharelinkuser ";
                    query1 += " union all ";
                    query1 += " select usr.ID as userId ,usr.Name, concat('Approver',wrkflow.WfLevel) as UserRole,wrkflow.WfLevel as workflowLevel,usr.EmailId from TbProjectSurvey srv  ";
                    query1 += " JOIN TbProjectWorkflowDetails wrkflow  ON srv.WorkflowID = wrkflow.WorkflowID ";
                    query1 += " JOIN TbUser usr ON usr.ID = wrkflow.UserID WHERE srv.SurveyID = @SurveyID and ifnull(wrkflow.IsDeleted,0) = 0";

                    lstSurveyEmailUser = _connection.Query<SurveyEmailUser>(query1, new
                    {
                        SurveyID = SurveyId,
                        sharelinkuser = Convert.ToInt32(Startup.ShareLinkUserID)
                    }).ToList();

                    if (lstSurveyEmailUser != null && lstSurveyEmailUser.Count > 0)
                    {
                        SurveyEmailUser objSurveyEmailUser = new SurveyEmailUser();
                        objSurveyEmailUser = lstSurveyEmailUser.Where(x => x.userId == LoggedUser).FirstOrDefault();

                        var dataCollectorUserEmail = string.Join(",", lstSurveyEmailUser.Where(x => x.workflowLevel == 0)
                             .Select(p => p.EmailId.ToString()));

                        var dataCollectorUserName = string.Join(",", lstSurveyEmailUser.Where(x => x.workflowLevel == 0)
                            .Select(p => p.Name.ToString()));

                        if (dataCollectorUserEmail != null && dataCollectorUserEmail.Length > 0 &&
                            dataCollectorUserName != null && dataCollectorUserName.Length > 0)
                        {
                            objSurveyEmailTemplate.DataCollectorUserEmail = dataCollectorUserEmail;
                            objSurveyEmailTemplate.DataCollectorUserName = dataCollectorUserName;
                        }

                        if (objSurveyEmailUser != null)
                        {
                            objSurveyEmailTemplate.SubmittedBy = objSurveyEmailUser.Name;
                            objSurveyEmailTemplate.SubmittedByRole = objSurveyEmailUser.UserRole;

                            if (objSurveyEmailTemplate.SubmittedBy != null && objSurveyEmailTemplate.SubmittedBy.Length > 0)
                            {
                                string[] splt = objSurveyEmailTemplate.SubmittedBy.Trim().Split(' ');

                                if (splt != null && splt.Length > 0)
                                {
                                    objSurveyEmailTemplate.SubmittedByShortName = splt[0].Substring(0, 1);

                                    if (splt.Length > 1)
                                    {
                                        objSurveyEmailTemplate.SubmittedByShortName += splt[splt.Length - 1].Substring(0, 1);
                                    }
                                    else
                                    {
                                        if(splt[0] != null && splt[0].Length > 0)
                                        {
                                            objSurveyEmailTemplate.SubmittedByShortName += splt[0].Substring(splt[0].Length - 1);
                                        }
                                    }
                                }
                            } 
                            else
                            {
                                objSurveyEmailTemplate.SubmittedByShortName = objSurveyEmailUser.EmailId.Substring(0,1);
                            }

                            List<SurveyEmailUser> lstSurveyEmailUser1 = new List<SurveyEmailUser>();
                            lstSurveyEmailUser1 = lstSurveyEmailUser.Where(x => x.workflowLevel == objSurveyEmailUser.workflowLevel + 1).ToList();

                            if (lstSurveyEmailUser1 != null && lstSurveyEmailUser1.Count > 0)
                            {
                                foreach (var item in lstSurveyEmailUser1)
                                {
                                    if (objSurveyEmailTemplate.SubmittedtoName != null && objSurveyEmailTemplate.SubmittedtoName.Length > 0)
                                    {
                                        objSurveyEmailTemplate.SubmittedtoName += ',' + item.Name;
                                    }
                                    else
                                    {
                                        objSurveyEmailTemplate.SubmittedtoName = item.Name;
                                    }

                                    if (objSurveyEmailTemplate.SubmittedtoEmail != null && objSurveyEmailTemplate.SubmittedtoEmail.Length > 0)
                                    {
                                        objSurveyEmailTemplate.SubmittedtoEmail += ',' + item.EmailId;
                                    }
                                    else
                                    {
                                        objSurveyEmailTemplate.SubmittedtoEmail = item.EmailId;
                                    }
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objSurveyEmailTemplate;
        }
        #endregion GetTemplateDetails

        #region GetTemplateDetails
        /// <summary>
        /// GetTemplateDetails
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <param name="LoggedUser"></param>
        /// <returns></returns>
        public SurveyEmailTemplate GetTemplateDetailsForTaskAsssigned(int SurveyId, int LoggedUser, string notifyUserIds)
        {
            SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
            List<SurveyEmailUser> lstSurveyEmailUser = new List<SurveyEmailUser>();

            try
            {
                using (_connection = Utils.Database.GetDBConnection())
                {
                    string query = "SELECT IFNULL(wrk.WSName,'') WorkspaceName,ifnull(prj.ProjectName,'') ProjectName ";
                    query += ",ifnull(srv.SurveyName,'') SurveyName,wrk.WorkspaceID FROM TbWorkspace wrk JOIN TbProject prj  ";
                    query += " ON wrk.WorkspaceID = prj.WSID JOIN TbProjectSurvey srv ON srv.ProjectId = prj.ProjectId WHERE srv.SurveyID = @SurveyID";

                    objSurveyEmailTemplate = _connection.Query<SurveyEmailTemplate>(query, new { SurveyID = SurveyId }).FirstOrDefault();

                    SurveyEmailTemplate objSurveyEmailTemplate1 = new SurveyEmailTemplate();

                    string query1 = "select usr.Name SubmittedBy,rl.RoleName SubmittedByRole,usr.EmailId as SubmittedByEmail from TbUser usr JOIN TbWorkspaceTeam team ON  usr.ID = team.UserID JOIN TbRole rl ";
                    query1 += " ON team.RoleID = rl.RoleId WHERE team.WorkspaceID  = @WorkspaceID AND ifnull(team.IsActive,0) = 1 AND ifnull(team.IsDtd,0) = 0 and usr.ID=  @userId;";
                    objSurveyEmailTemplate1 = _connection.Query<SurveyEmailTemplate>(query1, new { WorkspaceID = objSurveyEmailTemplate.WorkspaceID, userId = LoggedUser }).FirstOrDefault();


                    string query2 = "select Name ,EmailId  from TbUser where FIND_IN_SET(ID, @userIds); ";
                    lstSurveyEmailUser = _connection.Query<SurveyEmailUser>(query2, new { userIds = notifyUserIds }).ToList();

                    if (lstSurveyEmailUser != null && lstSurveyEmailUser.Count > 0)
                    {
                        var notifyUserEmail = string.Join(",", lstSurveyEmailUser.Where(x => x.workflowLevel == 0)
                            .Select(p => p.EmailId.ToString()));

                        var notifynUserName = string.Join(",", lstSurveyEmailUser.Where(x => x.workflowLevel == 0)
                            .Select(p => p.Name.ToString()));

                        if (notifyUserEmail != null && notifyUserEmail.Length > 0 &&
                          notifynUserName != null && notifynUserName.Length > 0)
                        {
                            objSurveyEmailTemplate.SubmittedtoEmail = notifyUserEmail;
                            objSurveyEmailTemplate.SubmittedtoName = notifynUserName;
                        }
                    }

                    if (objSurveyEmailTemplate1 != null)
                    {
                        objSurveyEmailTemplate.SubmittedBy = objSurveyEmailTemplate1.SubmittedBy;
                        objSurveyEmailTemplate.SubmittedByRole = objSurveyEmailTemplate1.SubmittedByRole;

                        if (objSurveyEmailTemplate.SubmittedBy != null && objSurveyEmailTemplate.SubmittedBy.Length > 0)
                        {
                            string[] splt = objSurveyEmailTemplate.SubmittedBy.Trim().Split(' ');

                            if (splt != null && splt.Length > 0)
                            {
                                objSurveyEmailTemplate.SubmittedByShortName = splt[0].Substring(0, 1);

                                if (splt.Length > 1)
                                {
                                    objSurveyEmailTemplate.SubmittedByShortName += splt[splt.Length - 1].Substring(0, 1);
                                }
                                else
                                {
                                    if (splt[0] != null && splt[0].Length > 0)
                                    {
                                        objSurveyEmailTemplate.SubmittedByShortName += splt[0].Substring(splt[0].Length - 1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            objSurveyEmailTemplate.SubmittedByShortName = objSurveyEmailTemplate1.SubmittedByEmail.Substring(0, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objSurveyEmailTemplate;
        }
        #endregion GetTemplateDetails


        public bool SendMailToDataUsers(int SurveyId, int LoggedUser, string ActionStatus)
        {
            //SurveyEmailTemplate objSurveyEmailTemplate = new SurveyEmailTemplate();
            //EmailActivity objEmail = new EmailActivity();
            //objSurveyEmailTemplate = GetTemplateDetails(SurveyId, LoggedUser, ActionStatus);

            return true;
        }
    }
}