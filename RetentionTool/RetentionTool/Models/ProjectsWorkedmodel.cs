using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class ProjectsWorkedmodel
    {

        public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public string ProjectName { get; set; }
        public string Designation { get; set; }
        public string Responsibilities { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Description { get; set; }
        public Nullable<int> TeamMembers { get; set; }
        public Nullable<int> Manager_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}