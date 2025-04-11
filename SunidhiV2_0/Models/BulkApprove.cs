using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SunidhiV2_0.Models
{
    public class BulkApprove
    {

            //==== MyTaskList===============
            public int SyncDBID { get; set; }
            public string Type { get; set; }
            public int TypeID { get; set; }
            public string Status { get; set; }
            public int WorkflowID { get; set; }
            public int TotalWFLevel { get; set; }
            //public int SubmittedWFLevel { get; set; }
            //public int SubmittedUser { get; set; }
            public int WFLevel { get; set; }
            public string WFLevelStatus { get; set; }
            public int ResponseSyncID { get; set; }
            public int DataCollectedBy { get; set; }
  
            public DateTime TaskDate { get; set; }
            public DateTime TaskSubmittedDate { get; set; }
      
            public int WorkspaceID { get; set; }
            public string TaskdataAddedBy { get; set; }

            public string WorkflowStage { get; set; }
            public string ActionRequired { get; set; }
            public string NextLevelAssignedUsers { get; set; }
           public string NextLevelAssignedUsersIds { get; set; }

    }
}
