using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class ProjectTags
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }
    }

    public class ProjectExpenseTags
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
    }
}
