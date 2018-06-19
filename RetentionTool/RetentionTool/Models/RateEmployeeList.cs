using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class RateEmployeeList
    {
        public int Id { get; set; }
        public Nullable<long> Employee_Id { get; set; }
        public string Name { get; set; }
    }
}