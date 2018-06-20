using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class UserDetailsViewModel
    {
        public List<UserDetail> userDetailsvm { get; set; }
        public UserDetail userDetail { get; set; }
    }
}