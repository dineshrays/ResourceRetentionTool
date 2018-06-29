using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.ViewModel
{
    public class EmployeeInfoViewModel
    {
        public List<PersonalInfoModel> PersonalInfo { get; set; }
        public PersonalInfoModel PersonalInfoVm { get; set; }

        public List<EducationQualificationModel> EducationQualification { get; set; }
        public EducationQualificationModel EducationQualificationVm { get; set; }

        public List<ExperienceModel> Experience { get; set; }
        public ExperienceModel ExperienceVm { get; set; }

        public List<EmployeeSkillsModel> EmployeeSkills { get; set; }
        public EmployeeSkillsModel EmployeeSkillsVm { get; set; }

        public List<ProjectsWorkedmodel> ProjectsWorked { get; set; }
        public ProjectsWorkedmodel ProjectsWorkedVm { get; set; }

        public List<CurrentInfoModel> Currentinfo { get; set; }
        public CurrentInfoModel CurrentInfoVm { get; set; }

        public IEnumerable<SelectListItem> CommonField { get; set; }
        public Nullable<long> SelectedCommonFields { get; set; }
        public IEnumerable<SelectListItem> Skill { get; set; }
        public Nullable<long> SelectedSkills { get; set; }
    }
}