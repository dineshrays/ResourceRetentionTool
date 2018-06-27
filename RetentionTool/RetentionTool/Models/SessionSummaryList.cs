using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class SessionSummaryList
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Attendance { get; set; }
        public string Remark { get; set; }
    }
}