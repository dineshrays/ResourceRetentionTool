using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;
namespace RetentionTool.ViewModel
{
    public class SessionSummaryViewModel
    {
        //List<Sess>
        //List<SessionSummModel> SessMVM { get; set; }
       // public SessionSummModel SessionModel { get; set; };
       public List<SessionSummaryModel> sessionvm { get; set; }
        public SessionSummaryModel session { get; set; }
       
      public  List<SessionsDet> sessionsDetsvm { get; set; }
        public SessionsDet sessionsDet { get; set; }

        
        
    }
}