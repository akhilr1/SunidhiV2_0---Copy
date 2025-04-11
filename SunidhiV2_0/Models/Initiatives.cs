using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Initiatives
    {
        public int InitiativeId { get; set; }
        public string InitiativeName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }      
       
        public string InitiativeTags { get; set; }
        public string InviteUserList { get; set; }
        public string InitUserList { get; set; }
        public string InitGroupList { get; set; }
        public string InitProjectTemplate { get; set; }
        public string InitWorkflowList { get; set; }

        public int WorkspaceID { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }        

        public int ThemeID { get; set; }
        public string Theme { get; set; }

    }    
  
    public class InitUserList
    {
        public int InitiativeId { get; set; } = 0;
        public int WorkspaceID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public string EmailId { get; set; } = "";
        public string Name { get; set; } = "";
        public int RoleID { get; set; } = 0;
        public string UserRole { get; set; } = "";
        public string ProfileImage { get; set; }

    }
   
    public class InitGroupList
    {
        public int InitiativeId { get; set; } = 0;
        public int WorkspaceID { get; set; } = 0;
        public int GroupID { get; set; } = 0;
        public string GroupName { get; set; } = "";
        public int MemberCount { get; set; } = 0;

    }
   
    public class InitWorkflowList
    {
        public int InitiativeId { get; set; } = 0;
        public int WorkspaceID { get; set; } = 0;

        public int WorkflowID { get; set; } = 0;
        public string WfName { get; set; }
        public string WfDescription { get; set; }
        public int WfNoOfLevels { get; set; } = 0;
        public List<WfLevelItem> WfLevelItems { get; set; }
    }
   
    public class WfLevelItem
    {
        public int InitiativeId { get; set; } = 0;
        public int WorkflowID { get; set; } = 0;
        public int WfLevel { get; set; } = 0;
        public List<InitUserList> InitUserList { get; set; }
       
    }  

   

    //Manage Initiative classes starts here 
    public class AddGroupToInitiative
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int ParentGroupId { get; set; }
        public string UserList { get; set; }
        public bool Notify { get; set; }
        public int WorkspaceId { get; set; }
        public int InitiativeId { get; set; }
    }
    //Manage Initiative classes ends here
}



