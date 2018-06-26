using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class ProjectWorkedViewModel
    {
        public List<ProjectsWorked> projectvm { get; set; }
        public ProjectsWorked projects { get; set; }
        public string projectname { get; set; }
    }
}