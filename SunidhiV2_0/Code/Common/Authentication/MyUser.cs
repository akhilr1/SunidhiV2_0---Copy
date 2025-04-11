using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunidhiV2_0.Code.Common.Authentication
{
    public class MyUser : IdentityUser
    {
        /// <summary>
        /// Id
        /// </summary>
        public override string  Id
        {
            get;
            set;
        }

        /// <summary>
        /// UserName
        /// </summary>
        public override string UserName
        {

            get;
            set;
        }

        /// <summary>
        /// RoleId
        /// </summary>
        public string RoleId
        {

            get;
            set;
        }
    }
}