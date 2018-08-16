using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.ViewModel
{
    public class CircularViewModel
    {
        public long Id { get; set; }
        public string Contents { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public int roleId { get; set; }

    }
}