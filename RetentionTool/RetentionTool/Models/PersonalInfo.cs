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
            this.CurrentInfoes = new HashSet<CurrentInfo>();
            this.EducationQualifications = new HashSet<EducationQualification>();
            this.EmployeeSkills = new HashSet<EmployeeSkill>();
            this.Experiences = new HashSet<Experience>();
            this.ProjectsWorkeds = new HashSet<ProjectsWorked>();
        }
    
        public int Id { get; set; }
        public string EmpId { get; set; }
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CurrentInfo> CurrentInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationQualification> EducationQualifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Experience> Experiences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectsWorked> ProjectsWorkeds { get; set; }
    }
}
