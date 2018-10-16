﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.Models
{
    public class ProjectWorkViewModel
    {
        public List<ProjectsWorked> projectvm { get; set; }
        public ProjectsWorked projects { get; set; }
     
        public string projectname { get; set; }
        public string managername { get; set; }
    }
}