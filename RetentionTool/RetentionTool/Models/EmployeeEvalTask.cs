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
    
    public partial class EmployeeEvalTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeEvalTask()
        {
            this.EmployeeEvalTaskDets = new HashSet<EmployeeEvalTaskDet>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AssignResource_Id { get; set; }
        public Nullable<long> Trainer_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual AssignResource AssignResource { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEvalTaskDet> EmployeeEvalTaskDets { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}
