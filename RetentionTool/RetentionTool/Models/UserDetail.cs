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
    
    public partial class UserDetail
    {
        public long Id { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual PersonalInfo PersonalInfo { get; set; }
        public virtual Role Role { get; set; }
    }
}
