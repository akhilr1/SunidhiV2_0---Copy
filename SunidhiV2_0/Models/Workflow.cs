using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Workflow
    {
        public int WorkflowID { get; set; }
        public string WfName { get; set; }
        public string WfDescription { get; set; }
        public int WfNoOfLevels { get; set; }
        public int ProjectId { get; set; }
        public List<WorkflowLevelDetails> WfLevelDetails { get; set; }
    }
    public class WorkflowLevelDetails {
        public int WfLevel { get; set; }
        public List<User> usersList { get; set; }
        public string workflowlevelname { get; set; }
    }
    public class UpdateWorkflowLevelDetails
    {
        public int WorkflowID { get; set; }
        public int WfNoOfLevels { get; set; }
        public string WfLevelDetails { get; set; }
    }
    public class AddWorkflowLevelDetails
    {
        public int WorkspaceID { get; set; }
        public int InitiativeId { get; set; }
        public int ProjectId { get; set; }
        public string WfName { get; set; }
        public string WfDescription { get; set; }
        public int WfNoOfLevels { get; set; }
        public string WfLevelDetails { get; set; }
    }
}



