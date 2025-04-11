using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Owin;
using System.Net;
using System.Web;

namespace SunidhiV2_0.Code.Common.Authentication
{
    public class AuthenticationManager
    {
        private IHttpContextAccessor _contextAccessor;


        public AuthenticationManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private AuthenticationManager _AuthenticationManager
        {
            get
            {
                // return _contextAccessor.HttpContext.Authentication;

                //   return _contextAccessor.HttpContext.Authentication.HttpContext.Authentication;

                return null;
            }
        }

        public AuthenticationManager GetAuthenticationManager()
        {
            return _AuthenticationManager;
        }
    }
}