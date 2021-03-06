﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class ExperienceModel
    {
        public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public string CompanyName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Designation { get; set; }
        public Nullable<int> ProjectWorked { get; set; }
        public string TechnologiesUsed { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}