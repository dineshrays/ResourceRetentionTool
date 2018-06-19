using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace RetentionTool.ViewModel
{
    public class RateEmployeeViewModel
    {
        public List<RateEmployeeEligiability> RateEmployee { get; set; }
        //[Required(ErrorMessage ="Must fill this property")]
        public RateEmployeeEligiability RateEmployeeVm { get; set; }
    }

}