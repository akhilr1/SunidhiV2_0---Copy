using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Workspace
    {
        public int WorkspaceID { get; set; }
        public string WSName { get; set; }
        public string WSDescription { get; set; }
        public bool WSIsPrivate { get; set; }
        public int CreatedBy { get; set; }
        public List<Tags> WSTags { get; set; }        
        public List<User> WSUsers { get; set; }
        public string WorkspaceTags { get; set; }
        public string WorkSpaceImage { get; set; }
        
    }
    public class WSTags
    {
        public int WSTagID { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }

    }

  
}
