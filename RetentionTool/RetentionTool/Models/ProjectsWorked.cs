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
    using System.ComponentModel.DataAnnotations;

    public partial class ProjectsWorked
    {
        public int Id { get; set; }
        public Nullable<int> PersonalInfo_Id { get; set; }
        //[Required(ErrorMessage = "Required")]
        public Nullable<int> Project_Id { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string Designation { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string Responsibilities { get; set; }
        //[Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }
        //[Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }
       // [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
       // [Required(ErrorMessage = "Required")]
        public Nullable<int> TeamMembers { get; set; }
       // [Required(ErrorMessage = "Required")]
        public Nullable<int> Manager_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ProjectsDetail ProjectsDetail { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
        public virtual PersonalInfo PersonalInfo1 { get; set; }
    }
}
