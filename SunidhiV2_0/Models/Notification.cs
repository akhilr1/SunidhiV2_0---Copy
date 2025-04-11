using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunidhiV2_0.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }
    }
}
