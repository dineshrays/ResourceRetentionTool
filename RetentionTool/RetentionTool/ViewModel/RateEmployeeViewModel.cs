using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.ViewModel
{
    public class RateEmployeeViewModel
    {
        public List<RateEmployeeEligiability> RateEmployee { get; set; }
        public RateEmployeeEligiability RateEmployeeVm { get; set; }
    }

}