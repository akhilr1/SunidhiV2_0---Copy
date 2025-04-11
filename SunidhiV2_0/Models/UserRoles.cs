using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class UserRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UpdatedDate { get; set; }
        public string Description { get; set; }
        public int NoOfScreens { get; set; }
        public int AccessId { get; set; }
        public string ScreenList { get; set; }
        public int WorkspaceID { get; set; }
    }
    /*
    public class SaveUserRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int AccessId { get; set; }
        public string ScreenList { get; set; }
    }*/
    public class RoleBasedScreen
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int AccessId { get; set; } = 0;
        public string AccessName { get; set; }        
        public int WorkspaceID { get; set; }
    }
    public class ScreenList
    {
        public int id { get; set; }
        public int RoleId { get; set; }
        public int AccessId { get; set; }
        public int Screenid { get; set; }
    }

    public class Screens
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int AccessId { get; set; } = 0;
        public string AccessName { get; set; }
        public string ScreenPath { get; set; }
        public bool DefaultScreen { get; set; }
    }

    public class AccessTypes
    {
        public int AccessId { get;set; }
        public string AccessName { get; set; }
    }

    public class UsersByRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProjectsCount { get; set; }
        public string  EmailID { get; set; }
        public string Status { get; set; }
        public int Tasks { get; set; }
        public int RoleId { get; set; }
        public string ProfileImage { get; set; }
    }
    
    public class UsersByscreens
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenPath { get; set; }
        public int AccessId { get; set; }

    }

    public class Role
    {
        public int RoleId { get; set; }
        public int WorkspaceID { get; set; }
        public string UsersList { get; set; }
        public bool Notify { get; set; }
    }

    public class AssignUsersToRole
    {
        public int RoleId { get; set; }
        public string UserList { get; set; }
        public string RoleName { get; set; }
        public bool notify { get; set; }
        public int WorkspaceID { get; set; }
    }

    public class UserList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
    }
    
    public class UserRoleDetails
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public  string Description { get; set; }
    }

    

    public class RoleBasedUserList_New
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public string Status { get; set; }
        public int Tasks { get; set; }
        public UserRoles RoleId { get; set; }
    }


    public class UserAssignRoleNotify
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string UserEmailId { get; set; }
    }

}

