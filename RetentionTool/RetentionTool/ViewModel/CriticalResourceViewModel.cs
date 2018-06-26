using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class CriticalResourceViewModel
    {
        public List<CriticalResource> criticalResvm { get; set; }
        public CriticalResource criticalResource { get; set; }
    }
}