using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace RetentionTool.Models
{
    public class AssignResourceViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Project_Id { get; set; }
        public Nullable<int> Manager_Id { get; set; }
        public Nullable<long> Trainer_Id { get; set; }
        public Nullable<int> Module_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int AssignResourceId { get; set; }
        public Nullable<long> Employee_Id { get; set; }
        [DisplayName("Manager Name")]
        public string managername { get; set; }
        [DisplayName("Trainer Name")]
        public string trainername { get; set; }
        [DisplayName("Employee Name")]
        public string employeename { get; set; }
        [DisplayName("Module Name")]
        public string modulename { get; set; }
        [DisplayName("Project Name")]
        public string projectname { get; set; }
        public int[] empid { get; set; }
        public virtual PersonalInfo Employee { get; set; }
        public virtual PersonalInfo Manager { get; set; }
        public virtual Module Module { get; set; }
        public virtual Trainer Trainer { get; set; }
        public virtual ProjectsDetail ProjectsDetail { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ToDate { get; set; }
       // public Training Training { get; set; }
    }
}