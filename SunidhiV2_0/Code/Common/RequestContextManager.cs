﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Code.Common
{
    public class RequestContextManager
    {
        public static RequestContextManager Instance { get; set; }

        static RequestContextManager()
        {
            Instance = new RequestContextManager(null);
        }

        private readonly IHttpContextAccessor contextAccessor;

        public RequestContextManager(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public HttpContext CurrentContext
        {
            get
            {
                if (contextAccessor == null)
                    return null;
                return contextAccessor.HttpContext;
            }
        }
    }
}
