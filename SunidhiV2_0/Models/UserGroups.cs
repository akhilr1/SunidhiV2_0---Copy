using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class UserGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public int ParentGroupId { get; set; }
        public int UsersCount { get; set; }
        public int ProjectsCount { get; set; }
        public int SubGroupsCount { get; set; }
        public string UserGroupImage { get; set; }
    }
    public class UsersList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserRole { get; set; }
        public bool IsSelected { get; set; }
        public string ProfileImage { get; set; }
    }
    public class AddGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int ParentGroupId { get; set; }
        public string UserList { get; set; }
        public bool Notify { get; set; }
        public int WorkspaceId { get; set; }
        public string location { get; set; }
        public string UserGroupImage { get; set; }
        public int? ProjectId { get; set; }
    }

    public class Group
    {
        public int GroupId { get; set; }
    }

    public class Bulk
    {
        public string UserList { get; set; }
        public string GroupList { get; set; }
        public bool Notify { get; set; }
    }

    public class AssignUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }

    public class Projects
    {
        public int ProjectId { get; set; }
        public String ProjectName { get; set; }
        public String Status { get; set; }
    }

    public class GroupNotify
    {
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string UserEmailId { get; set; }
    }
}
