//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetentionTool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeSkillsAdd
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
        public string Evaluator { get; set; }
    }
}
