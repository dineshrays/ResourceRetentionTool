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
    
    public partial class Module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Module()
        {
            this.AssignResources = new HashSet<AssignResource>();
            this.ModuleDets = new HashSet<ModuleDet>();
        }
    
        public int Id { get; set; }
        public Nullable<long> Commonfield_Id { get; set; }
        public Nullable<long> Skill_Id { get; set; }
        public string ModuleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Commonfield Commonfield { get; set; }
        public virtual Skill Skill { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignResource> AssignResources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleDet> ModuleDets { get; set; }
    }
}
