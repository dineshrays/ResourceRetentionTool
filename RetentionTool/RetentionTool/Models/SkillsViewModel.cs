using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Models
{
    public class SkillsViewModel
    {
        public long id { get; set; }
        public string SkillName { get; set; }
        public Nullable<long> CommonField_Id { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public string CommonFieldName { get; set; }

        //public IEnumerable<SelectListItem> CommonFieldDropDown{ get; set; }
        public Nullable<long> selectedCommmonField { set; get; }
    }

    //public class CommonFieldDropDown
    //{
    //    public int id { get; set; }
    //    public string value { get; set; }
    //}
}