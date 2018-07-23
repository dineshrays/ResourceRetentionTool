using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class TrainerModel
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public Nullable<int> PersonalInfo_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CriticalResource_Id { get; set; }
        
        public virtual ICollection<AssignEvaluater> AssignEvaluaters { get; set; }
        public virtual ICollection<AssignResource> AssignResources { get; set; }
        public virtual CriticalResource CriticalResource { get; set; }
        public virtual ICollection<EmployeeEvalTask> EmployeeEvalTasks { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}