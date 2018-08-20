using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class ApproveEmpSkillsViewModel
    {
        public List<ApproveEmpSkillsModel> ApproveEmpSkillVm { get; set; }
        public ApproveEmpSkillsModel GetApproveEmpSkills { get; set; }
    }
}