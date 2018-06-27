using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class EmployeeInformationController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInformation
        public ActionResult Index()
        {
            List<PersonalInfoModel> details = (from a in db.PersonalInfoes
                                                          select new PersonalInfoModel
                                                          {
                                                              EmpCode =a.EmpCode,
                                                              Name = a.Name
                                                         }).ToList();
            ViewBag.details = details;
            return View();
        }

        public IEnumerable<SelectListItem> getCommonFields()
        {
            var val = db.Commonfields.ToList();
            var cf = new SelectList(val, "id", "Name");
            return cf;

        }
        public IEnumerable<SelectListItem> getSkill()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem{Value=null,Text=""}
            };
            return list;

        }
        public ActionResult getSkills(int skillId)
        {
            IEnumerable<SelectListItem> skilldet = getSkillsField(skillId);
            return Json(skilldet,JsonRequestBehavior.AllowGet);
            
        }

        public IEnumerable<SelectListItem> getSkillsField(int commonId)
        {
            IEnumerable<SelectListItem> rs = db.Skills.Where(s => s.CommonField_Id == commonId).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Name
            }).ToList();

            return new SelectList(rs, "Value", "Text");
        }

        public ActionResult Create()
        {
            EmployeeInformationViewModel ei = new EmployeeInformationViewModel();
            
            ei.CommonField = getCommonFields();
                
            ei.Skills = getSkill();
            return View(ei);
            
        }

        [HttpPost]
        public ActionResult Create(PersonalInfoModel PersonalInfoVm, EducationQualificationModel EducationQualificationVm, ExperienceModel ExperienceVm, EmployeeSkillsModel EmployeeSkillsVm, ProjectsWorkedmodel ProjectsWorkedVm, CurrentInfoModel CurrentInfoVm)
        {
            PersonalInfo p = new PersonalInfo();
            EducationQualification edu = new EducationQualification();
            Experience ex = new Experience();
            EmployeeSkill es = new EmployeeSkill();
            ProjectsWorked pw = new ProjectsWorked();
            CurrentInfo ci = new CurrentInfo();

            p.EmpCode = PersonalInfoVm.EmpCode;
            p.Name = PersonalInfoVm.Name;
            p.FatherName = PersonalInfoVm.FatherName;
            p.DOB = PersonalInfoVm.DOB;
            p.Gender = PersonalInfoVm.Gender;
            p.PermanentAddress = PersonalInfoVm.PermanentAddress;
            p.CommunicationAddress = PersonalInfoVm.CommunicationAddress;
            p.Contact = PersonalInfoVm.Contact;
            p.Qualification = PersonalInfoVm.Qualification;
            p.Email = PersonalInfoVm.Email;
            p.PanNo = PersonalInfoVm.PanNo;
            p.AadharNo = PersonalInfoVm.AadharNo;
            p.BloodGroup = PersonalInfoVm.BloodGroup;
            p.IsActive = true;
            db.PersonalInfoes.Add(p);
            db.SaveChanges();

            edu.P_Id =p.Id;
            edu.Degree = EducationQualificationVm.Degree;
            edu.Board = EducationQualificationVm.Board;
            edu.YearOfPassing = EducationQualificationVm.YearOfPassing;
            edu.Percentage = EducationQualificationVm.Percentage;
            edu.IsActive = true;
            db.EducationQualifications.Add(edu);
            db.SaveChanges();

            ex.P_Id = p.Id;
            ex.CompanyName = ExperienceVm.CompanyName;
            ex.FromDate = ExperienceVm.FromDate;
            ex.ToDate = ExperienceVm.ToDate;
            ex.Designation = ExperienceVm.Designation;
            ex.ProjectWorked = ExperienceVm.ProjectWorked;
            ex.TechnologiesUsed = ExperienceVm.TechnologiesUsed;
            ex.IsActive = true;
            db.Experiences.Add(ex);
            db.SaveChanges();

            es.P_Id = p.Id;
            es.CommonField_Id = EmployeeSkillsVm.CommonField_Id;
            es.Skills_Id = EmployeeSkillsVm.Skills_Id;
            es.Years = EmployeeSkillsVm.Years;
            es.Months = EmployeeSkillsVm.Months;
            es.Status = EmployeeSkillsVm.Status;
            es.IsActive = true;
            db.EmployeeSkills.Add(es);
            db.SaveChanges();

            pw.PersonalInfo_Id = p.Id;
            pw.Project_Id = ProjectsWorkedVm.Project_Id;
            pw.Designation = ProjectsWorkedVm.Designation;
            pw.Responsibilities = ProjectsWorkedVm.Responsibilities;
            pw.StartDate = ProjectsWorkedVm.StartDate;
            pw.EndDate = ProjectsWorkedVm.EndDate;
            pw.Description = ProjectsWorkedVm.Description;
            pw.TeamMembers = ProjectsWorkedVm.TeamMembers;
            pw.Manager_Id = ProjectsWorkedVm.Manager_Id;
            pw.IsActive = true;
            db.ProjectsWorkeds.Add(pw);
            db.SaveChanges();

            ci.P_Id = p.Id;
            ci.Designation = CurrentInfoVm.Designation;
            ci.DOJ = CurrentInfoVm.DOJ;
            ci.DateOfRelieving = CurrentInfoVm.DateOfRelieving;
            ci.ReportingManager = CurrentInfoVm.ReportingManager;
            ci.JobType = CurrentInfoVm.JobType;
            ci.DeployedCompanyDetails = CurrentInfoVm.DeployedCompanyDetails;
            ci.DeployedFromDate = CurrentInfoVm.DeployedFromDate;
            ci.DeployedToDate = CurrentInfoVm.DeployedToDate;
            ci.CompanyMailId = CurrentInfoVm.CompanyMailId;
            ci.BankName = CurrentInfoVm.BankName;
            ci.IFSC = CurrentInfoVm.IFSC;
            ci.ModeOfPayment = CurrentInfoVm.ModeOfPayment;
            ci.WorkLocation = CurrentInfoVm.WorkLocation;
            ci.Department = CurrentInfoVm.Department;
            ci.Grade = CurrentInfoVm.Grade;
            ci.Salary = CurrentInfoVm.Salary;
            ci.CTC = CurrentInfoVm.CTC;
            db.CurrentInfoes.Add(ci);
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}