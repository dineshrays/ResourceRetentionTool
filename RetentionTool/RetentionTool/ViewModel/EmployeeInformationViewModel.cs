using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RetentionTool.ViewModel
{
    public class EmployeeInformationViewModel
    {
        public List<PersonalInfoModel> PersonalInfo { get; set; }
        public PersonalInfoModel PersonalInfoVm { get; set; }

        public List<EducationQualificationModel> EducationQualification { get; set; }
        public EducationQualificationModel EducationQualificationVm { get; set; }

        public List<ExperienceModel> Experience { get; set; }
        public ExperienceModel ExperienceVm { get; set; }
    }
}