using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class EmployeeSkillsAddModel
    {
        public int Id { get; set; }
        public int P_Id { get; set; }
        public Nullable<long> CommonField_Id { get; set; }
        public Nullable<long> Skills_Id { get; set; }
        public Nullable<int> Years { get; set; }
        public Nullable<int> Months { get; set; }
        public string Status { get; set; }
        public Nullable<int> Manager_Id { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual Commonfield Commonfield { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
        public virtual Skill Skill { get; set; }
    }
}