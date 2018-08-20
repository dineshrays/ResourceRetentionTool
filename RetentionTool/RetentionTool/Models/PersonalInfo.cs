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
    
    public partial class PersonalInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonalInfo()
        {
            this.AssignResources = new HashSet<AssignResource>();
            this.AssignResourcesDets = new HashSet<AssignResourcesDet>();
            this.CriticalResources = new HashSet<CriticalResource>();
            this.CurrentInfoes = new HashSet<CurrentInfo>();
            this.EducationQualifications = new HashSet<EducationQualification>();
            this.EmployeeEvalTaskDets = new HashSet<EmployeeEvalTaskDet>();
            this.EmployeeSkills = new HashSet<EmployeeSkill>();
            this.Experiences = new HashSet<Experience>();
            this.ProjectsWorkeds = new HashSet<ProjectsWorked>();
            this.ProjectsWorkeds1 = new HashSet<ProjectsWorked>();
            this.RateEmployeeEligiabilities = new HashSet<RateEmployeeEligiability>();
            this.SessionsDets = new HashSet<SessionsDet>();
            this.UserDetails = new HashSet<UserDetail>();
            this.Trainers = new HashSet<Trainer>();
            this.EmployeeSkillsAdds = new HashSet<EmployeeSkillsAdd>();
            this.ApproveEmpSkills = new HashSet<ApproveEmpSkill>();
        }
    
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string PermanentAddress { get; set; }
        public string CommunicationAddress { get; set; }
        public Nullable<long> Contact { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public Nullable<long> AadharNo { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignResource> AssignResources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignResourcesDet> AssignResourcesDets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriticalResource> CriticalResources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CurrentInfo> CurrentInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationQualification> EducationQualifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEvalTaskDet> EmployeeEvalTaskDets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Experience> Experiences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectsWorked> ProjectsWorkeds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectsWorked> ProjectsWorkeds1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RateEmployeeEligiability> RateEmployeeEligiabilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessionsDet> SessionsDets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trainer> Trainers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkillsAdd> EmployeeSkillsAdds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveEmpSkill> ApproveEmpSkills { get; set; }
    }
}
