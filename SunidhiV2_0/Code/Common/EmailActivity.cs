using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Controllers;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Http;

namespace SunidhiV2_0.Code.Common
{
    public class EmailActivity
    {

        #region SendMail
        /// <summary>
        /// SendMail
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="obj"></param>
        /// <param name="userID"></param>
        /// <param name="ID"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool SendMail(string Action, object obj, int userID = 0)
        {

            string subject = string.Empty;
            string body = string.Empty;

            EmailTemplate objEmail = new EmailDAL().GetEmailTemplateDetails(Action);


            if (objEmail == null)
                return false;
            else
            {
                subject = objEmail.TemplateName;
                body = objEmail.TemplateContent;
                switch (Action)
                {
                    case "Invite User":
                        {
                            UsersInfo objUsr = (UsersInfo)obj;
                            if (objUsr != null)
                            {
                                body = body.Replace("{UserRole}", objUsr.userRole);
                                body = body.Replace("{ClientName}", objUsr.ClientName);
                                new Code.Utils.MailUtil().SendInviteMail(subject, objUsr.EmailId, body);
                            }
                        }
                        break;

                    case "AssignGroup":
                        {
                            GroupNotify objGrp = (GroupNotify)obj;
                            if (objGrp != null)
                            {
                                body = body.Replace("{FirstName}", objGrp.UserName);
                                body = body.Replace("{GroupName}", objGrp.GroupName);
                                new Code.Utils.MailUtil().SendMail(subject, objGrp.UserEmailId, body);
                            }
                        }
                        break;

                    case "AssignRole":
                        {
                            UserRoleList objRole = (UserRoleList)obj;
                            if (objRole != null)
                            {
                                body = body.Replace("{FirstName}", objRole.userName);
                                body = body.Replace("{UserRole}", objRole.userRole);
                                new Code.Utils.MailUtil().SendMail(subject, objRole.EmailId, body);
                            }
                        }
                        break;

                    case "DeleteUser":
                        {
                            UsersInfo objUsr = (UsersInfo)obj;
                            if (objUsr != null)
                            {
                                body = body.Replace("{FirstName}", objUsr.Name);
                                new Code.Utils.MailUtil().SendMail(subject, objUsr.EmailId, body);
                            }
                        }
                        break;

                    case "ChangeUserStatus":
                        {
                            UsersInfo objUsr = (UsersInfo)obj;
                            if (objUsr != null)
                            {
                                body = body.Replace("{FirstName}", objUsr.Name);
                                body = body.Replace("{UserStatus}", objUsr.UserStatus ? "Activated" : "Deactivated");
                                new Code.Utils.MailUtil().SendMail(subject, objUsr.EmailId, body);
                            }
                        }
                        break;

                    //Template for Admin Created User
                    case "Group Add Users":
                        {
                            GroupNotify details = (GroupNotify)obj;

                            if (details != null)
                            {
                                body = body.Replace("{FirstName}", details.UserName);
                                body = body.Replace("{GroupName}", details.GroupName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;

                    case "AssignFinanceUser":
                        {
                            NotifyExpenseTags details = (NotifyExpenseTags)obj;

                            if (details != null)
                            {
                                body = body.Replace("{FirstName}", details.UserName);
                                body = body.Replace("{ProjectName}", details.ProjectName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;
                    case "Request Fund":
                        {
                            RequestFundNotify details = (RequestFundNotify)obj;

                            if (details != null)
                            {
                                body = body.Replace("{UserName}", details.UserName);
                                body = body.Replace("{FundAmount}", details.FundAmount);
                                body = body.Replace("{DonorUser}", details.DonorUser);
                                body = body.Replace("{ProjectName}", details.ProjectName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;
                    case "Request Payment":
                        {
                            RequestFundNotify details = (RequestFundNotify)obj;

                            if (details != null)
                            {
                                body = body.Replace("{UserName}", details.UserName);
                                body = body.Replace("{FundAmount}", details.FundAmount);
                                body = body.Replace("{ProjectName}", details.ProjectName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;
                    case "Notify Expense Tags":
                        {
                            NotifyExpenseTags details = (NotifyExpenseTags)obj;

                            if (details != null)
                            {
                                body = body.Replace("{UserName}", details.UserName);
                                body = body.Replace("{ProjectName}", details.ProjectName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;
                    case "PaymentTermNotifyUsers":
                        {
                            NotifyExpenseTags details = (NotifyExpenseTags)obj;

                            if (details != null)
                            {
                                body = body.Replace("{UserName}", details.UserName);
                                body = body.Replace("{ProjectName}", details.ProjectName);
                                new Code.Utils.MailUtil().SendMail(subject, details.UserEmailId, body);
                            }
                        }
                        break;
                    case "SignupUser":
                        {
                            SignupInfo signupdetails = (SignupInfo)obj;

                            if (signupdetails != null)
                            {
                                body = body.Replace("{Name}", signupdetails.Name);
                                body = body.Replace("{OTP}", signupdetails.OTP);
                                new Code.Utils.MailUtil().SendMail(subject, signupdetails.EmailId, body);
                            }
                        }
                        break;
                    case "ImpactWorks - New Signup":
                        {
                            SignupInfo signupdetails = (SignupInfo)obj;

                            if (signupdetails != null)
                            {
                                body = body.Replace("{ApplicationName}", signupdetails.Name);
                                body = body.Replace("{Email}", signupdetails.EmailId);
                                new Code.Utils.MailUtil().SendMail(subject, "impactworks.sales@billionlives.in", body);
                            }
                        }
                        break;
                    case "ImpactWorks - Login Request":
                        {
                            MobileUser Userdetails = (MobileUser)obj;

                            if (Userdetails != null)
                            {
                                body = body.Replace("{Email}", Userdetails.username);
                                new Code.Utils.MailUtil().SendMail(subject, "impactworks.sales@billionlives.in", body);
                            }
                        }
                        break;
                    case "SignupNewUser":
                        {
                            SignupInfo signupdetails = (SignupInfo)obj;

                            if (signupdetails != null)
                            {
                                body = body.Replace("{Name}", signupdetails.Name);
                                body = body.Replace("{OTP}", signupdetails.OTP);
                                body = body.Replace("{ImWorks Link}", Startup.ImWorksLink);

                                //string path = "http://" + request.Host.Value;

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";
                                string welcomePoject = emailPath + "Project.png";
                                string welcomeDataforms = emailPath + "Dataforms.png";
                                string welcomeOffline = emailPath + "Offline.png";
                                string welcomeImpact = emailPath + "Impact.png";
                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";
                                string welcomeCopyBtn = emailPath + "CopyBtn.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                body = body.Replace("{welcome/Project}", welcomePoject);
                                body = body.Replace("{welcome/Dataforms}", welcomeDataforms);
                                body = body.Replace("{welcome/Offline}", welcomeOffline);
                                body = body.Replace("{welcome/Impact}", welcomeImpact);
                                body = body.Replace("{welcome/CopyBtn}", welcomeCopyBtn);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, signupdetails.EmailId, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = null;
                                ObjEmailTrackLog.ToAddress = signupdetails.EmailId;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = 0;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;

                    case "InviteUserToWorkspaceWithPassword":
                        {
                            InviteUserInfo objInviteUser = (InviteUserInfo)obj;

                            if (objInviteUser != null)
                            {
                                string[] names = !string.IsNullOrEmpty(objInviteUser.InviteeUserEmail) ? objInviteUser.InviteeUserEmail.Split("@") : null;

                                if (names != null && names.Length > 0)
                                {
                                    objInviteUser.InviteeUserName = names[0];
                                }

                                body = body.Replace("{Name}", !string.IsNullOrEmpty(objInviteUser.InviteeUserName) ? objInviteUser.InviteeUserName : objInviteUser.InviteeUserEmail);
                                body = body.Replace("{Inviter}", objInviteUser.InviterName);
                                body = body.Replace("{Workspace Name}", objInviteUser.WorkspaceName);
                                body = body.Replace("{Password}", objInviteUser.InviteeUserPassword);
                                body = body.Replace("{Role}", objInviteUser.InviterRole);

                                string str = !string.IsNullOrEmpty(objInviteUser.InviterName) ? objInviteUser.InviterName.ToString() : "I";
                                body = body.Replace("{I}", str[0].ToString().ToUpper());

                                body = body.Replace("{ImWorks Link}", Startup.ImWorksLink);

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";

                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";
                                string welcomeCopyBtn = emailPath + "CopyBtn.png";
                                string folderIcon = emailPath + "folderIcon.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);

                                body = body.Replace("{welcome/CopyBtn}", welcomeCopyBtn);
                                body = body.Replace("{folderIcon}", folderIcon);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, objInviteUser.InviteeUserEmail, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = objInviteUser.InviterEmail;
                                ObjEmailTrackLog.ToAddress = objInviteUser.InviteeUserEmail;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = userID;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;

                    case "InviteUserToProjectWithPassword":
                        {
                            InviteUserInfo objInviteUser = (InviteUserInfo)obj;

                            if (objInviteUser != null)
                            {
                                string[] names = !string.IsNullOrEmpty(objInviteUser.InviteeUserEmail) ? objInviteUser.InviteeUserEmail.Split("@") : null;

                                if (names != null && names.Length > 0)
                                {
                                    objInviteUser.InviteeUserName = names[0];
                                }

                                body = body.Replace("{Name}", !string.IsNullOrEmpty(objInviteUser.InviteeUserName) ? objInviteUser.InviteeUserName : objInviteUser.InviteeUserEmail);
                                body = body.Replace("{Inviter}", objInviteUser.InviterName);
                                body = body.Replace("{Workspace Name}", objInviteUser.WorkspaceName);
                                body = body.Replace("{Project Name}", objInviteUser.ProjectName);
                                body = body.Replace("{Password}", objInviteUser.InviteeUserPassword);
                                body = body.Replace("{Role}", objInviteUser.InviterRole);

                                string str = !string.IsNullOrEmpty(objInviteUser.InviterName) ? objInviteUser.InviterName.ToString() : "I";
                                body = body.Replace("{I}", str[0].ToString().ToUpper());

                                body = body.Replace("{ImWorks Link}", Startup.ImWorksLink);

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";

                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";
                                string welcomeCopyBtn = emailPath + "CopyBtn.png";
                                string folderIcon = emailPath + "folderIcon.png";
                                string projectIcon = emailPath + "ProjectIIcon.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);

                                body = body.Replace("{welcome/CopyBtn}", welcomeCopyBtn);
                                body = body.Replace("{folderIcon}", folderIcon);
                                body = body.Replace("{projectIcon}", projectIcon);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, objInviteUser.InviteeUserEmail, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = objInviteUser.InviterEmail;
                                ObjEmailTrackLog.ToAddress = objInviteUser.InviteeUserEmail;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = userID;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;
                    case "TaskSubmission":
                        {
                            SurveyEmailTemplate objSurveyEmailTemplate = (SurveyEmailTemplate)obj;

                            if (objSurveyEmailTemplate != null && objSurveyEmailTemplate.SubmittedtoEmail != null)
                            {
                                string[] splitemail = objSurveyEmailTemplate.SubmittedtoEmail.Split(',');
                                string[] splitname = objSurveyEmailTemplate.SubmittedtoName.Split(',');
                                int count = 0;

                                foreach (var itm in splitemail)
                                {
                                    body = body.Replace("{Name}", splitname[count]);
                                    body = body.Replace("{Submitter}", objSurveyEmailTemplate.SubmittedBy);
                                    body = body.Replace("{WorkspaceName}", objSurveyEmailTemplate.WorkspaceName);
                                    body = body.Replace("{ProjectName}", objSurveyEmailTemplate.ProjectName);
                                    body = body.Replace("{SurveyName}", objSurveyEmailTemplate.SurveyName);
                                    body = body.Replace("{SubmitterShortName}", objSurveyEmailTemplate.SubmittedByShortName);
                                    body = body.Replace("{SubmitterRole}", objSurveyEmailTemplate.SubmittedByRole);

                                    string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                    string imWorksLogo = emailPath + "ImWorksLogo.png";
                                    string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                    string twitterLogo = emailPath + "twitterLogo.png";
                                    string linkedInLogo = emailPath + "LinkedInLogo.png";
                                    string WorkspaceImage = emailPath + "folderIcon.png";
                                    string projectLogo = emailPath + "ProjectIIcon.png";
                                    string surveyLogo = emailPath + "surveyIcon.png";

                                    body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                    body = body.Replace("{ImWorksLogoOnly}", imWorksLogoOnly);
                                    body = body.Replace("{twitterLogo}", twitterLogo);
                                    body = body.Replace("{LinkedInLogo}", linkedInLogo);
                                    body = body.Replace("{WorkspaceImage}", WorkspaceImage);
                                    body = body.Replace("{projectImage}", projectLogo);
                                    body = body.Replace("{SurveyImage}", surveyLogo);

                                    string imworkslink = Startup.ImWorksLink + "/app/MyTasks?WsID=" + objSurveyEmailTemplate.WorkspaceID + "&Access=3";
                                    body = body.Replace("{ImWorks Link}", imworkslink);

                                    new Code.Utils.MailUtil().SendMail(subject, itm, body);

                                    EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                    EmailDAL objEmailDAL = new EmailDAL();

                                    ObjEmailTrackLog.FromAddress = objSurveyEmailTemplate.SubmittedBy;
                                    ObjEmailTrackLog.ToAddress = itm;
                                    ObjEmailTrackLog.SectionType = Action;
                                    ObjEmailTrackLog.LoggedUser = userID;
                                    objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                                    count++;
                                }
                            }
                        }
                        break;
                    case "TaskApproved":
                        {
                            SurveyEmailTemplate objSurveyEmailTemplate = (SurveyEmailTemplate)obj;

                            if (objSurveyEmailTemplate != null && objSurveyEmailTemplate.SubmittedtoEmail != null)
                            {
                                string[] splitemail = objSurveyEmailTemplate.SubmittedtoEmail.Split(',');
                                string[] splitname = objSurveyEmailTemplate.SubmittedtoName.Split(',');
                                int count = 0;

                                foreach (var itm in splitemail)
                                {
                                    body = body.Replace("{Name}", splitname[count]);
                                    body = body.Replace("{Submitter}", objSurveyEmailTemplate.SubmittedBy);
                                    body = body.Replace("{WorkspaceName}", objSurveyEmailTemplate.WorkspaceName);
                                    body = body.Replace("{ProjectName}", objSurveyEmailTemplate.ProjectName);
                                    body = body.Replace("{SurveyName}", objSurveyEmailTemplate.SurveyName);
                                    body = body.Replace("{SubmitterShortName}", objSurveyEmailTemplate.SubmittedByShortName);
                                    body = body.Replace("{SubmitterRole}", objSurveyEmailTemplate.SubmittedByRole);

                                    string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                    string imWorksLogo = emailPath + "ImWorksLogo.png";
                                    string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                    string twitterLogo = emailPath + "twitterLogo.png";
                                    string linkedInLogo = emailPath + "LinkedInLogo.png";
                                    string WorkspaceImage = emailPath + "folderIcon.png";
                                    string projectLogo = emailPath + "ProjectIIcon.png";
                                    string surveyLogo = emailPath + "surveyIcon.png";

                                    body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                    body = body.Replace("{ImWorksLogoOnly}", imWorksLogoOnly);
                                    body = body.Replace("{twitterLogo}", twitterLogo);
                                    body = body.Replace("{LinkedInLogo}", linkedInLogo);
                                    body = body.Replace("{WorkspaceImage}", WorkspaceImage);
                                    body = body.Replace("{projectImage}", projectLogo);
                                    body = body.Replace("{SurveyImage}", surveyLogo);

                                    string imworkslink = Startup.ImWorksLink + "/app/MyTasks?WsID=" + objSurveyEmailTemplate.WorkspaceID + "&Access=3";
                                    body = body.Replace("{ImWorks Link}", imworkslink);

                                    new Code.Utils.MailUtil().SendMail(subject, itm, body);

                                    EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                    EmailDAL objEmailDAL = new EmailDAL();
                                    ObjEmailTrackLog.FromAddress = objSurveyEmailTemplate.SubmittedBy;
                                    ObjEmailTrackLog.ToAddress = itm;
                                    ObjEmailTrackLog.SectionType = Action;
                                    ObjEmailTrackLog.LoggedUser = userID;
                                    objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                                    count++;
                                }
                            }
                        }
                        break;
                    case "TaskRejection":
                        {
                            SurveyEmailTemplate objSurveyEmailTemplate = (SurveyEmailTemplate)obj;

                            if (objSurveyEmailTemplate != null && objSurveyEmailTemplate.DataCollectorUserEmail != null)
                            {
                                string[] splitemail = objSurveyEmailTemplate.DataCollectorUserEmail.Split(',');
                                string[] splitname = objSurveyEmailTemplate.DataCollectorUserName.Split(',');
                                int count = 0;

                                foreach (var itm in splitemail)
                                {
                                    body = body.Replace("{Name}", splitname[count]);
                                    body = body.Replace("{Submitter}", objSurveyEmailTemplate.SubmittedBy);
                                    body = body.Replace("{WorkspaceName}", objSurveyEmailTemplate.WorkspaceName);
                                    body = body.Replace("{ProjectName}", objSurveyEmailTemplate.ProjectName);
                                    body = body.Replace("{SurveyName}", objSurveyEmailTemplate.SurveyName);
                                    body = body.Replace("{SubmitterShortName}", objSurveyEmailTemplate.SubmittedByShortName);
                                    body = body.Replace("{SubmitterRole}", objSurveyEmailTemplate.SubmittedByRole);

                                    string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                    string imWorksLogo = emailPath + "ImWorksLogo.png";
                                    string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                    string twitterLogo = emailPath + "twitterLogo.png";
                                    string linkedInLogo = emailPath + "LinkedInLogo.png";
                                    string WorkspaceImage = emailPath + "folderIcon.png";
                                    string projectLogo = emailPath + "ProjectIIcon.png";
                                    string surveyLogo = emailPath + "surveyIcon.png";

                                    body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                    body = body.Replace("{ImWorksLogoOnly}", imWorksLogoOnly);
                                    body = body.Replace("{twitterLogo}", twitterLogo);
                                    body = body.Replace("{LinkedInLogo}", linkedInLogo);
                                    body = body.Replace("{WorkspaceImage}", WorkspaceImage);
                                    body = body.Replace("{projectImage}", projectLogo);
                                    body = body.Replace("{SurveyImage}", surveyLogo);

                                    string imworkslink = Startup.ImWorksLink + "/app/MyTasks?WsID=" + objSurveyEmailTemplate.WorkspaceID + "&Access=3";
                                    body = body.Replace("{ImWorks Link}", imworkslink);

                                    new Code.Utils.MailUtil().SendMail(subject, itm, body);

                                    EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                    EmailDAL objEmailDAL = new EmailDAL();
                                    ObjEmailTrackLog.FromAddress = objSurveyEmailTemplate.SubmittedBy;
                                    ObjEmailTrackLog.ToAddress = itm;
                                    ObjEmailTrackLog.SectionType = Action;
                                    ObjEmailTrackLog.LoggedUser = userID;
                                    objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                                    count++;
                                }
                            }
                        }
                        break;
                    case "TaskAssigned":
                        {
                            SurveyEmailTemplate objSurveyEmailTemplate = (SurveyEmailTemplate)obj;

                            if (objSurveyEmailTemplate != null && objSurveyEmailTemplate.SubmittedtoEmail != null)
                            {
                                string[] splitemail = objSurveyEmailTemplate.SubmittedtoEmail.Split(',');
                                string[] splitname = objSurveyEmailTemplate.SubmittedtoName.Split(',');
                                int count = 0;

                                foreach (var itm in splitemail)
                                {
                                    body = body.Replace("{Name}", splitname[count]);
                                    body = body.Replace("{Submitter}", objSurveyEmailTemplate.SubmittedBy);
                                    body = body.Replace("{WorkspaceName}", objSurveyEmailTemplate.WorkspaceName);
                                    body = body.Replace("{ProjectName}", objSurveyEmailTemplate.ProjectName);
                                    body = body.Replace("{SurveyName}", objSurveyEmailTemplate.SurveyName);
                                    body = body.Replace("{SubmitterShortName}", objSurveyEmailTemplate.SubmittedByShortName);
                                    body = body.Replace("{SubmitterRole}", objSurveyEmailTemplate.SubmittedByRole);

                                    string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                    string imWorksLogo = emailPath + "ImWorksLogo.png";
                                    string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                    string twitterLogo = emailPath + "twitterLogo.png";
                                    string linkedInLogo = emailPath + "LinkedInLogo.png";
                                    string WorkspaceImage = emailPath + "folderIcon.png";
                                    string projectLogo = emailPath + "ProjectIIcon.png";
                                    string surveyLogo = emailPath + "surveyIcon.png";

                                    body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                    body = body.Replace("{ImWorksLogoOnly}", imWorksLogoOnly);
                                    body = body.Replace("{twitterLogo}", twitterLogo);
                                    body = body.Replace("{LinkedInLogo}", linkedInLogo);
                                    body = body.Replace("{WorkspaceImage}", WorkspaceImage);
                                    body = body.Replace("{projectImage}", projectLogo);
                                    body = body.Replace("{SurveyImage}", surveyLogo);

                                    string imworkslink = Startup.ImWorksLink + "/app/MyTasks?WsID=" + objSurveyEmailTemplate.WorkspaceID + "&Access=3";
                                    body = body.Replace("{ImWorks Link}", imworkslink);

                                    new Code.Utils.MailUtil().SendMail(subject, itm, body);

                                    EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                    EmailDAL objEmailDAL = new EmailDAL();
                                    ObjEmailTrackLog.FromAddress = objSurveyEmailTemplate.SubmittedBy;
                                    ObjEmailTrackLog.ToAddress = itm;
                                    ObjEmailTrackLog.SectionType = Action;
                                    ObjEmailTrackLog.LoggedUser = userID;
                                    objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                                    count++;
                                }
                            }
                        }
                        break;

                    case "InviteUserToWorkspaceWithOutPassword":
                        {
                            InviteUserInfo objInviteUser = (InviteUserInfo)obj;

                            if (objInviteUser != null)
                            {
                                body = body.Replace("{Name}", !string.IsNullOrEmpty(objInviteUser.InviteeUserName) ?
                                    objInviteUser.InviteeUserName : objInviteUser.InviteeUserEmail);
                                body = body.Replace("{Inviter}", objInviteUser.InviterName);
                                body = body.Replace("{Workspace Name}", objInviteUser.WorkspaceName);
                                body = body.Replace("{Role}", objInviteUser.InviterRole);

                                string str = !string.IsNullOrEmpty(objInviteUser.InviterName) ? objInviteUser.InviterName.ToString() : "I";
                                body = body.Replace("{I}", str[0].ToString().ToUpper());

                                body = body.Replace("{ImWorks Link}", Startup.ImWorksLink);

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";

                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";
                                string folderIcon = emailPath + "folderIcon.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                body = body.Replace("{folderIcon}", folderIcon);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, objInviteUser.InviteeUserEmail, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = objInviteUser.InviterEmail;
                                ObjEmailTrackLog.ToAddress = objInviteUser.InviteeUserEmail;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = userID;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;

                    case "InviteUserToProjectWithOutPassword":
                        {
                            InviteUserInfo objInviteUser = (InviteUserInfo)obj;

                            if (objInviteUser != null)
                            {
                                body = body.Replace("{Name}", !string.IsNullOrEmpty(objInviteUser.InviteeUserName) ?
                                    objInviteUser.InviteeUserName : objInviteUser.InviteeUserEmail);
                                body = body.Replace("{Inviter}", objInviteUser.InviterName);
                                body = body.Replace("{Workspace Name}", objInviteUser.WorkspaceName);
                                body = body.Replace("{Project Name}", objInviteUser.ProjectName);
                                body = body.Replace("{Password}", objInviteUser.InviteeUserPassword);
                                body = body.Replace("{Role}", objInviteUser.InviterRole);

                                string str = !string.IsNullOrEmpty(objInviteUser.InviterName) ? objInviteUser.InviterName.ToString() : "I";
                                body = body.Replace("{I}", str[0].ToString().ToUpper());

                                body = body.Replace("{ImWorks Link}", Startup.ImWorksLink);

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";

                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";
                                string folderIcon = emailPath + "folderIcon.png";
                                string projectIcon = emailPath + "ProjectIIcon.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                body = body.Replace("{folderIcon}", folderIcon);
                                body = body.Replace("{projectIcon}", projectIcon);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, objInviteUser.InviteeUserEmail, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = objInviteUser.InviterEmail;
                                ObjEmailTrackLog.ToAddress = objInviteUser.InviteeUserEmail;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = userID;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;

                    case "ForgotPassword":
                        {
                            ForgotPasswordInfo forgotPassword = (ForgotPasswordInfo)obj;

                            if (forgotPassword != null)
                            {
                                body = body.Replace("{Name}", forgotPassword.Name);
                                body = body.Replace("{Role}", forgotPassword.Role);

                                string str = !string.IsNullOrEmpty(forgotPassword.Name) ? forgotPassword.Name.ToString() : "I";
                                body = body.Replace("{I}", str[0].ToString().ToUpper());

                                //var encodedEmailId = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(forgotPassword.EmailId));

                                string imworkslink = Startup.ImWorksLink + "/app/ResetPassword?key=" + forgotPassword.EncodedEmailId;
                                body = body.Replace("{ImWorks Link}", imworkslink);

                                //string path = "http://" + request.Host.Value;

                                string emailPath = Startup.DomainName + Startup.EmailTemplateImages;
                                string imWorksLogo = emailPath + "ImWorksLogo.png";                                
                                string imWorksLogoOnly = emailPath + "imWorksLogoOnly.png";
                                string twitterLogo = emailPath + "twitterLogo.png";
                                string linkedInLogo = emailPath + "LinkedInLogo.png";

                                body = body.Replace("{ImWorksLogo}", imWorksLogo);
                                body = body.Replace("{imWorksLogoOnly}", imWorksLogoOnly);
                                body = body.Replace("{twitterLogo}", twitterLogo);
                                body = body.Replace("{LinkedInLogo}", linkedInLogo);

                                new Code.Utils.MailUtil().SendMail(subject, forgotPassword.EmailId, body);

                                EmailTrackLog ObjEmailTrackLog = new EmailTrackLog();
                                EmailDAL objEmailDAL = new EmailDAL();
                                ObjEmailTrackLog.FromAddress = null;
                                ObjEmailTrackLog.ToAddress = forgotPassword.EmailId;
                                ObjEmailTrackLog.SectionType = Action;
                                ObjEmailTrackLog.LoggedUser = 0;
                                objEmailDAL.SaveEmailTrackLog(ObjEmailTrackLog);
                            }
                        }
                        break;
                }

                return true;
            }
        }
        #endregion SendMail


        #region SaveEmailTrackLog
        /// <summary>
        /// SaveEmailTrackLog
        /// </summary>
        /// <param name="ObjEmailTrackLog"></param>
        public void SaveEmailTrackLog(EmailTrackLog ObjEmailTrackLog)
        {
            new EmailDAL().SaveEmailTrackLog(ObjEmailTrackLog);
        }
        #endregion SaveEmailTrackLog
        /*

        #region SendBillionLivesMail
        /// <summary>
        /// SendBillionLivesMail
        /// </summary>
        /// <param name="objSendMail"></param>
        /// <returns></returns>
        public bool SendBillionLivesMail(string ToEmail, string Name, string Message, string Subject)
        {
            Message = Message + " <br/>Regards <br/>" + Name;
            return new Code.Utils.MailUtil().SendMailBillionLives(Subject, ToEmail, Message, true, false, "");
        }
        #endregion SendBillionLivesMail

       */
    }
}