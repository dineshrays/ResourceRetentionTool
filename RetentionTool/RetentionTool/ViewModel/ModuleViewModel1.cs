using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class ModuleViewModel1
    {      
       public List<ModuleViewModel> MODView { get; set; }
        public ModuleViewModel module { get; set; }
        
    }
}