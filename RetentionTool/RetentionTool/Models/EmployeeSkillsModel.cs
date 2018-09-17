using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Models
{
    public class EmployeeSkillsModel
    {
        public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public Nullable<long> CommonField_Id { get; set; }
        public Nullable<long> Skills_Id { get; set; }
        public Nullable<int> Years { get; set; }
        public Nullable<int> Months { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public string Skills { get; set; }

        public IEnumerable<SelectListItem> CommonField { get; set; }
        public Nullable<long> SelectedCommonFields { get; set; }
       // public IEnumerable<SelectListItem> Skills { get; set; }
        public Nullable<long> SelectedSkills { get; set; }

        public virtual Commonfield Commonfield { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}