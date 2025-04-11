using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class MapPath
{
    public static IHostingEnvironment _env;
    public MapPath(IHostingEnvironment env)
    {
        _env = env;
    }
    public static string GetAbsoluteUri(HttpRequest request)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = request.Scheme;
        uriBuilder.Host = request.Host.Host;
        uriBuilder.Path = request.Path.ToString();
        uriBuilder.Query = request.QueryString.ToString();
        return uriBuilder.Path + uriBuilder.Query;
    }
}
