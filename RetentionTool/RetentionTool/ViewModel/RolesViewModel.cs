using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class RolesViewModel
    {
        public List<Role> rolevm { get; set; }
        public Role role { get; set; }
        public string name { get; set; }
    }
}