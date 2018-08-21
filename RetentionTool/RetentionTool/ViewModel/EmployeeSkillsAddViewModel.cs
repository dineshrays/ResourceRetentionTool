using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetentionTool.Models;

namespace RetentionTool.ViewModel
{
    public class EmployeeSkillsAddViewModel
    {
        public List<EmployeeSkillsAdd> skiladd { get; set; }
        public EmployeeSkillsAdd EmployeeSkillsAdd { get; set; }


        public List<ApproveEmpSkillsModel> ApproveEmpSkillVm { get; set; }
        public ApproveEmpSkillsModel GetApproveEmpSkills { get; set; }

        public int managerid { get; set; }
        public ApproveEmpSkill approveskill { get; set; }

    }
}