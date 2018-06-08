using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class EmployeeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}