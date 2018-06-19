using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class EmployeeEvalViewModel
    {
      public  List<EmployeeEvalTask> empEvalTaskvm = new List<EmployeeEvalTask>();
        public EmployeeEvalTask EmployeeEvalTask;
        public List<EmployeeEvalTaskDet> empEvalTaskDetvm = new List<EmployeeEvalTaskDet>();
        public EmployeeEvalTaskDet EmployeeEvalTaskDet;
        

    }
}