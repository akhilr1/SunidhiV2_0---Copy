using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunidhiV2_0.Models 
{
    public class EmailTemplate
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string TemplateType { get; set; }
        public string TemplateDescription { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmailTrackLog
    {
        public int ID { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string SectionType { get; set; }
        public DateTime CreateDate { get; set; }
        public int LoggedUser { get; set; }
    }


    public class SurveyEmailTemplate
    {
        public string WorkspaceName { get; set; }
        public string SurveyName { get; set; }
        public string ProjectName { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmittedtoName { get; set; }
        public string SubmittedtoEmail { get; set; }
        public string SubmittedByRole { get; set; }
        public string SubmittedByShortName{ get; set; }
        public int WorkspaceID { get; set; }
        public string DataCollectorUserEmail { get; set; }
        public string DataCollectorUserName { get; set; }
        public string SubmittedByEmail { get; set; }
    }

    public class SurveyEmailUser
    {
        public int userId { get; set; }
        public string Name { get; set; }
        public string UserRole { get; set; }
        public int workflowLevel { get; set; }
        public string EmailId { get; set; }
    }

    public class EmailSurveyList
    {
        public int SurveyID { get; set; }
        public string EmailTemplateName { get; set; }
    }
}