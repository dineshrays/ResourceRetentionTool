using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.ViewModel
{
    public class SessionView
    {
        public int? assignresid { get; set; }
        public int? moduleid { get; set; }
        public string modulename { get; set; }
        public DateTime? date { get; set; }
    }
}