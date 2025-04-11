using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ProjectTheme
    {
        public int ThemeID { get; set; }
        public string ThemeName { get; set; }
        public List<InitiativeProjectTemplate> ProjectTemplateList { get; set; }
    }
    public class InitiativeProjectTemplate
    {
        public int ProjectTemplateID { get; set; }
        public int ThemeID { get; set; }
        public string ProjectTemplateName { get; set; }
        public string Description { get; set; }
    }
}



