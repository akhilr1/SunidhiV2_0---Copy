using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SunidhiV2_0.Code.Utils
{
    public class MailUtil
    {
        public bool SendInviteMail(string sMailSubject, string sMailTo, string sMailMessage, bool isHtml = true, string cc = "")
        {
            if (string.IsNullOrWhiteSpace(sMailTo) || string.IsNullOrWhiteSpace(sMailTo))
                return false;

            bool result = false;
            string sMailServer = "";
            int smtpPort = 25;
            string sMailFrom = "";
            string sUser = "";
            string sPassword = "";
            bool EnableSSl = false;

            sMailServer = Startup.MailServerHost;
            smtpPort = Convert.ToInt32(Startup.MailServerPort);
            sUser = Startup.MailServerUserName;  // System.Configuration.ConfigurationManager.AppSettings["AppAdminEmailId"];
            sPassword = Startup.SystemSenderEmailPassword;

            sMailFrom = Startup.AppAdminEmailId;            
            string sTestEmailTo =Startup.SendAllTestMailsTo;

            EnableSSl = bool.Parse(Startup.MailServerEnableSSL);
            sMailTo = sMailTo.Equals("") ? sMailFrom : sMailTo;

            SmtpClient MailClient = new SmtpClient(sMailServer, smtpPort);
            MailClient.UseDefaultCredentials = false;
            MailClient.Credentials = (ICredentialsByHost)new System.Net.NetworkCredential(sUser, sPassword);
            MailAddress mMailsTo;
            if (sTestEmailTo == null || sTestEmailTo == "")
                mMailsTo = new MailAddress(sMailTo);
            else
                mMailsTo = new MailAddress(sTestEmailTo, "Test - " + sMailTo);
            MailMessage MailMessage = new MailMessage(); //(new MailAddress(sMailFrom), mMailsTo);
            MailClient.EnableSsl = EnableSSl;
           

            MailMessage.Subject = sMailSubject;
            if (!string.IsNullOrEmpty(cc))
            {
                MailAddress addressCC = new MailAddress(cc);
                MailMessage.CC.Add(addressCC);
            }
            MailMessage.Body += sMailMessage;// +"\r\n\r\n\r\n" + sDisclaimer;
            MailMessage.IsBodyHtml = isHtml;
            MailMessage.To.Add(sMailTo);
            MailMessage.From = new MailAddress(sMailFrom);
            try
            {
                result = true;
                MailClient.Send(MailMessage);
               
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                MailClient.Dispose();
            }

            return result;
        }
        
         public bool SendMail(string sMailSubject, string sMailTo, string sMailMessage, bool isHtml = true, bool bccToSiteManager = false, string cc = "")
         {
            if (string.IsNullOrWhiteSpace(sMailTo) || string.IsNullOrWhiteSpace(sMailTo))
                return false;
            bool result = false;
            string sMailServer = "";
            int smtpPort = 25;
            string sMailFrom = "";
            string sUser = "";
            string sPassword = "";
            bool EnableSSl = false;

            sMailServer = Startup.MailServerHost;
            smtpPort = Convert.ToInt32(Startup.MailServerPort);
            sMailFrom = Startup.AppAdminEmailId;
            sUser = Startup.MailServerUserName; 
            sPassword = Startup.SystemSenderEmailPassword;
            string sTestEmailTo = Startup.SendAllTestMailsTo;

            EnableSSl = bool.Parse(Startup.MailServerEnableSSL);

            sMailTo = sMailTo.Equals("") ? sMailFrom : sMailTo;
            SmtpClient MailClient = new SmtpClient(sMailServer, smtpPort);
            MailClient.UseDefaultCredentials = false;
            MailClient.Credentials = (ICredentialsByHost)new System.Net.NetworkCredential(sUser, sPassword);
            MailAddress mMailsTo;
            if (sTestEmailTo == null || sTestEmailTo == "")
                mMailsTo = new MailAddress(sMailTo);
            else
                mMailsTo = new MailAddress(sTestEmailTo, "Test - " + sMailTo);
            MailMessage MailMessage = new MailMessage(); //(new MailAddress(sMailFrom), mMailsTo);
            MailClient.EnableSsl = EnableSSl;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            if (bccToSiteManager)
                MailMessage.Bcc.Add(sMailFrom);

            MailMessage.Subject = sMailSubject;
            if (!string.IsNullOrEmpty(cc))
            {
                MailAddress addressCC = new MailAddress(cc);
                MailMessage.CC.Add(addressCC);
            }
            MailMessage.Body += sMailMessage;
            MailMessage.IsBodyHtml = isHtml;
            MailMessage.To.Add(sMailTo);
            MailMessage.From = new MailAddress(sMailFrom);
            try
            {
                result = true;
                MailClient.Send(MailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MailClient.Dispose();
            }

            return result;
        }
        /*
         public8 bool SendMailBillionLives(string sMailSubject, string sMailFrom1, string sMailMessage, bool isHtml = true, bool bccToSiteManager = false, string cc = "")
         {
             string sMailTo = sMailSubject.ToLower() == "career" ? Startup.billionLivesCareer : Startup.billionLivesContactus;
             if (string.IsNullOrWhiteSpace(sMailTo) || string.IsNullOrWhiteSpace(sMailTo))
                 return false;
             bool result = false;
             string sMailServer = "";
             int smtpPort = 25;
             string sMailFrom = "";
             string sUser = "";
             string sPassword = "";
             string sDisclaimer = "";
             bool EnableSSl = false;
             cc = sMailSubject.ToLower() == "career" ? "" : Startup.billionLivesContactusCC;

             sMailServer = Startup.MailServerHost;
             smtpPort = Convert.ToInt32(Startup.MailServerPort);
             sMailFrom = sMailFrom1;
             sUser = Startup.MailServerUserName;  // System.Configuration.ConfigurationManager.AppSettings["AppAdminEmailId"];
             sPassword = Startup.SystemSenderEmailPassword;
             string sTestEmailTo = Startup.SendAllTestMailsTo;

             EnableSSl = bool.Parse(Startup.MailServerEnableSSL);

             sMailTo = sMailTo.Equals("") ? sMailFrom : sMailTo;
             SmtpClient MailClient = new SmtpClient(sMailServer, smtpPort);
             MailClient.UseDefaultCredentials = false;
             MailClient.Credentials = (ICredentialsByHost)new System.Net.NetworkCredential(sUser, sPassword);
             MailAddress mMailsTo;
             if (sTestEmailTo == null || sTestEmailTo == "")
                 mMailsTo = new MailAddress(sMailTo);
             else
                 mMailsTo = new MailAddress(sTestEmailTo, "Test - " + sMailTo);
             MailMessage MailMessage = new MailMessage(); //(new MailAddress(sMailFrom), mMailsTo);
             MailClient.EnableSsl = EnableSSl;
             //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
             if (bccToSiteManager)
                 MailMessage.Bcc.Add(sMailFrom);

             MailMessage.Subject = sMailSubject;
             if (!string.IsNullOrEmpty(cc))
             {
                 MailAddress addressCC = new MailAddress(cc);
                 MailMessage.CC.Add(addressCC);
             }
             MailMessage.Body += sMailMessage;// +"\r\n\r\n\r\n" + sDisclaimer;
             //MailMessage.Priority = MailPriority.Normal;
             MailMessage.IsBodyHtml = isHtml;
             MailMessage.To.Add(sMailTo);
             MailMessage.From = new MailAddress(sMailFrom);
             // MailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null,System.Net.Mime.MediaTypeNames.Text.Plain));
             // MailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, System.Net.Mime.Text.Html));
             try
             {
                 result = true;
                 MailClient.Send(MailMessage);
                 //mail.LogEmail(ApplciationID, MailMessage, true);
             }
             catch (Exception ex)
             {
                 //email failed.
                 // mail.LogEmail(ApplciationID, MailMessage, false);
                 throw ex;
             }
             finally
             {
                 MailClient.Dispose();
             }

             return result;
         }
         */
    }
}