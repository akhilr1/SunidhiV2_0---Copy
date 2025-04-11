using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.Common
{
    public class SessionUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Set Session syntax

        public void SetSession(string key, string value)
        {      
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        //Get Session syntax

        public string GetSession(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }

    }
}
