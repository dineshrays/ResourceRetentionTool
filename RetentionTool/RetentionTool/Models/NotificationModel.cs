using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string subMessage { get; set; }
    }
}