using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SunidhiV2_0.Models
{
    public class ErrorLog
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string RequestURL { get; set; }
        public string Source { get; set; }
        public string InnerException { get; set; }
        public DateTime Date { get; set; }       
    }
}