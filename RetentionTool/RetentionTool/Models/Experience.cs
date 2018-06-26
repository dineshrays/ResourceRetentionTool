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
    
    public partial class Experience
    {
        public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Designation { get; set; }
        public Nullable<int> ProjectWorked { get; set; }
        public string TechnologiesUsed { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}