using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ProjectTemplate
    {
        public int InitiativeId { get; set; } = 0;
        public int WorkspaceID { get; set; } = 0;
        public int ThemeID { get; set; } = 0;
        public string Theme { get; set; } = "";
        public int ProjectTemplateID { get; set; } = 0;
        public string TemplateName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Usecase { get; set; }
        public int NoOfTemplates { get; set; }
        public string TemplateImage { get; set; }
        public string ThemeImage { get; set; }
    }
}
