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
    
    public partial class AssignResourcesDet
    {
        public int Id { get; set; }
        public Nullable<int> AssignResources_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
    
        public virtual AssignResource AssignResource { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}
