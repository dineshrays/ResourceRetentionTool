using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class SessionSummaryModel
    {

        public int Id { get; set; }
        public Nullable<int> TrainingDet_Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Hours { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Topics { get; set; }

        public virtual TrainingDet TrainingDet { get; set; }
    }
}