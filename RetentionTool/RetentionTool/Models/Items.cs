using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class Item
    {
        public int Id { get; set; }
            public string Topics { get; set; }
        public Nullable<System.TimeSpan> HoursRequired { get; set; }
    }
}