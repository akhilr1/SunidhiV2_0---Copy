using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;

namespace SunidhiV2_0.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public int NoOfLogins { get; set; }
        public int FailedLoginCount { get; set; }
        public string Status { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public int? State { get; set; }
        public int? District { get; set; }
        public int UserType { get; set; }
        public string ProfileImage { get; set; }
        public string Organization { get; set; }
        public string workflowlevelname { get; set; }
        


    }
    public class UsersInfo
    {
       
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string userOrganisation { get; set; }
        public string userGroupID { get; set; }
        public string userGroup { get; set; }
        public int userRoleID { get; set; }
        public string userRole { get; set; }
        public bool UserRoleStatus { get; set; }
        public string userProject { get; set; }
        public int userProjectCount { get; set; }
        public bool UserStatus { get; set; }
        public string userGroupList { get; set; }
        public string ClientName { get; set; }

        public bool IsActive { get; set; }
        public bool IsRegisteredUser { get; set; }
        public bool IsDeleted { get; set; }
        public string ProfileImage { get; set; }
    }
    public class UsersDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }       
        public int RoleID { get; set; }
        public string Role { get; set; }
        public string RoleType { get; set; }
        public int ProjectCount { get; set; }
        public string ProfileImage { get; set; }
        public int IsRegisteredUser { get; set; }
        public int IsDeleted { get; set; }
        public string Organization { get; set; }
        public int TaskCount { get; set; }
        public double UserProgress { get; set; }
    }
    public class UsersRoleInfo
    {       
        public int userRoleID { get; set; }
        public string userRole { get; set; }   

    }
    
    public class InviteUsersList
    {
        public string EmailId { get; set; }
        public UsersRoleInfo UserRole { get; set; }   
    }


    public class UserGroupList
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public string UserGroupImage { get; set; }


    }
    public class UsersScreenInfo
    {

        public int userScreenID { get; set; }
        public string userScreenName { get; set; }
        public int AccessId { get; set; }
        public string userAccessType { get; set; }
    }   

   //AssignGroup
    public class userGroupList
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string EmailId { get; set; }
        public List<UGroupList> uGroupList { get; set; }
    }
     public class UGroupList
    {
        public int uGroupId { get; set; }
        public string uGroupName { get; set; }
    }
    //----------------------
    //Assign Role
    public class UserRoleList
    {

        public int userId { get; set; }
        public string userName { get; set; }
        public string EmailId { get; set; }
        public string userRoleID { get; set; }
        public string userRole { get; set; }
    }
    //---------------


    public class userList
    {
        public int userId { get; set; }
    }

   
    public class MobileUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string notificationtoken { get; set; }
    }
    public class ChangePassword
    {
        public int Id { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
    }

    public class LoggedUserInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string Organization { get; set; }
        public string location { get; set; }
        public string ProfileImageName { get; set; }
        
    }
    public class UserPageList
    {
        public int UserID { get; set; }
        public string PageName { get; set; }
    }
    //----------------------------- 

    public class SignupInfo
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string message { get; set; }

    }

    public class InviteUserInfo
    {
        public string InviteeUserName { get; set; }
        public string InviteeUserRole { get; set; }
        public string InviteeUserEmail { get; set; }
        public string InviteeUserPassword { get; set; }
        public string InviterEmail { get; set; }
        public string InviterName { get; set; }
        public string InviterRole { get; set; }
        public string WorkspaceName { get; set; }
        public string ProjectName { get; set; }
    }

    public class ForgotPasswordInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string EncodedEmailId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string Password { get; set; }
        public string key1 { get; set; }
        public int? ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? RequestId { get; set; }
    }

    public class WorkspaceInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string WorkspaceName { get; set; }
        public string Inviter { get; set; }
        public string Role { get; set; }
        public int WorkspaceID { get; set; }
        public string Name { get; set; }
    }




}
