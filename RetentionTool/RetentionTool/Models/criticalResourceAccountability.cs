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
    
    public partial class criticalResourceAccountability
    {
        public int Id { get; set; }
        public int criticalresource_Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual CriticalResource CriticalResource { get; set; }
    }
}
