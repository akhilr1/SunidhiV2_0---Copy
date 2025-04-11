using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using Newtonsoft.Json;

namespace SunidhiV2_0.Controllers
{
    public class ErrorsController : Controller
    {
        private IHttpContextAccessor _accessor;

        public ErrorsController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;           
        }

        public ActionResult Index()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
           
            var uname = "";
            if (User.Identity.IsAuthenticated)
                uname = User.Identity.Name;
           
            try
            {
                var obj = new ErrorLog();
                String ipAddress;
                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                    obj.UserName = User.Identity.Name;
                if(exception != null && exception.Error != null)
                {
                    obj.Source = exception.Error.Source;
                    obj.Message = exception.Error.Message;
                    obj.InnerException = exception.Error.InnerException == null ? "" : exception.Error.InnerException.Message ?? "" + "\n" + exception.Error.InnerException.StackTrace ?? "";
                    if (obj.InnerException.Length > 10000)
                        obj.InnerException = obj.InnerException.Substring(0, 9999);
                    obj.StackTrace = exception.Error.StackTrace;
                }

                if(Request.HttpContext.Connection !=null)
                {
                    obj.RequestURL =  ((Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature)exception).Path;
                    ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                obj.Date = DateTime.Now;
                obj.MachineName = System.Environment.MachineName;

                ErrorsDAL objErrorsDAL = new ErrorsDAL();
                objErrorsDAL.SaveErrorLog(obj);
            }
            catch { }
            return Json(new { status = "Unhandled Exception" }, new JsonSerializerSettings());
            //return View("/areas/backoffice/views/customerror.cshtml");
        }
    }
}