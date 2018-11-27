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
    
    public partial class AssignResource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssignResource()
        {
            this.AssignEvaluaters = new HashSet<AssignEvaluater>();
            this.AssignResourcesDets = new HashSet<AssignResourcesDet>();
            this.EmployeeEvalTasks = new HashSet<EmployeeEvalTask>();
            this.RateEmployeeEligiabilities = new HashSet<RateEmployeeEligiability>();
            this.Trainings = new HashSet<Training>();
        }
    
        public int Id { get; set; }

        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Project_Id { get; set; }
        public Nullable<int> Manager_Id { get; set; }
        public Nullable<long> Trainer_Id { get; set; }
        public Nullable<int> Module_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignEvaluater> AssignEvaluaters { get; set; }
        public virtual Module Module { get; set; }
        public virtual ProjectsDetail ProjectsDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignResourcesDet> AssignResourcesDets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEvalTask> EmployeeEvalTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RateEmployeeEligiability> RateEmployeeEligiabilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Training> Trainings { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}
