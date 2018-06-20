using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class EducationQualificationModel
    {
        public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public string Degree { get; set; }
        public string Board { get; set; }
        public Nullable<int> YearOfPassing { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}