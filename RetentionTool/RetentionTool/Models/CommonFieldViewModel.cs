using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RetentionTool.Models
{
    public class CommonFieldViewModel
    {
        public long id { get; set; }
        //[DisplayName("dgh")]
        [Required(ErrorMessage = "Required")]
        //[Display(Name ="Required")]
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; }
       // public List<Commonfield> commList { get; set; }
       // [DisplayName("dgh")]
        public int SelectedVal { get; set; }
       // public IEnumerable<SelectListItem> commandname { get; set; }
        //   public IEnumerable<SelectListItems> 
    }
    //public class commandname
    //{
    //    public int id { get; set; }
    //    public string value { get; set; }
    //}
}