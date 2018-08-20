using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class ApproveEmpSkillsModel
    {
        public int Id { get; set; }
        public Nullable<int> EmpskillAdd_Id { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public string TaskAssigned { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsEvaluated { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual EmployeeSkillsAdd EmployeeSkillsAdd { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}