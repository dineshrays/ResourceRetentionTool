using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;
namespace RetentionTool.ViewModel
{
    public class AssignResouViewModel
    {
        public List<AssignResourceViewModel> assignResVM { get; set; }
        public AssignResourceViewModel assignResource { get; set; }
    }
}