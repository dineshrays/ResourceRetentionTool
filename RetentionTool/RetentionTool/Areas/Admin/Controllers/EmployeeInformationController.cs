using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class EmployeeInformationController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInformation
        public ActionResult Index()
        {
            int td = int.Parse(Session["userid"].ToString());

            List<PersonalInfoModel> details = (from a in db.PersonalInfoes
                                               //where a.Id == td

                                               select new PersonalInfoModel
                                               {
                                                   Id = a.Id,
                                                   EmpCode = a.EmpCode,
                                                   Name = a.Name,
                                                   Image = a.Image
                                                   // "UserImages/logo2.png"
                                               }).OrderByDescending(a => a.Id).ToList();
            ViewBag.details = details;
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {

            string path = "";
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];


                if (pic != null && pic.ContentLength > 0)
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/UserImages"),
                                                  Path.GetFileName(pic.FileName));
                        pic.SaveAs(path);
                        path = "/UserImages/" + pic.FileName;
                        TempData["path"] = path;
                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
            }
            //return 
            //View("Index");
            return Json(new { filepath = path }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Create()
        {
            EmployeeInformationViewModel ei = new EmployeeInformationViewModel();

            getId();
            ei.CommonField = getCommonFields();
            ei.Skill = getSkill();
            return View(ei);

        }

        public void getId()
        {
            ViewData["Id"] = new SelectList(db.PersonalInfoes.ToList(), "Id", "Name");
        }

        public ActionResult CreateEmployee()
        {
            EmployeeInformationViewModel ei = new EmployeeInformationViewModel();

            getId();
            getManagers();
            getProjectList();
            ei.CommonField = getCommonFields();
            ei.Skill = getSkill();
            return View(ei);
        }

        public void getManagers()
        {
            ViewData["managerslist"] = new SelectList(db.PersonalInfoes.ToList(), "id", "Name");
        }

        public void getProjectList()
        {
            ViewData["projectlist"] = new SelectList(db.ProjectsDetails.ToList(), "Id", "Name");
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
            return Json(skilldet, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult Create(string path, PersonalInfo personalInfo, List<EducationQualification> eductionQualifi, List<Experience> exper, List<EmployeeSkill> empSkill, ProjectsWorked projectWorked, CurrentInfo currentInfo)
        {

            PersonalInfo p = new PersonalInfo();
            ProjectsWorked pw = new ProjectsWorked();
            CurrentInfo ci = new CurrentInfo();

            p.EmpCode = personalInfo.EmpCode;
            p.Name = personalInfo.Name;
            p.FatherName = personalInfo.FatherName;
            p.DOB = personalInfo.DOB;
            p.Gender = personalInfo.Gender;
            p.PermanentAddress = personalInfo.PermanentAddress;
            p.CommunicationAddress = personalInfo.CommunicationAddress;
            p.Contact = personalInfo.Contact;
            p.Qualification = personalInfo.Qualification;
            p.Email = personalInfo.Email;
            p.PanNo = personalInfo.PanNo;
            p.AadharNo = personalInfo.AadharNo;
            p.BloodGroup = personalInfo.BloodGroup;
            p.Image = path;
            //Convert.ToString(TempData["path"]);  
            p.IsActive = true;
            db.PersonalInfoes.Add(p);
            db.SaveChanges();

            if (eductionQualifi != null)
            {
                foreach (var equali in eductionQualifi)
                {
                    equali.P_Id = p.Id;
                    equali.IsActive = true;
                    db.EducationQualifications.Add(equali);
                    db.SaveChanges();
                }
            }

            if (exper != null)
            {
                foreach (var exp in exper)
                {
                    exp.P_Id = p.Id;
                    exp.IsActive = true;
                    db.Experiences.Add(exp);
                    db.SaveChanges();
                }
            }

            if (empSkill != null)
            {
                foreach (var ski in empSkill)
                {
                    ski.P_Id = p.Id;
                    ski.IsActive = true;
                    db.EmployeeSkills.Add(ski);
                    db.SaveChanges();
                }
            }

            pw.PersonalInfo_Id = p.Id;
            pw.Project_Id = projectWorked.Project_Id;
            pw.Designation = projectWorked.Designation;
            pw.Responsibilities = projectWorked.Responsibilities;
            pw.StartDate = projectWorked.StartDate;
            pw.EndDate = projectWorked.EndDate;
            pw.Description = projectWorked.Description;
            pw.TeamMembers = projectWorked.TeamMembers;
            pw.Manager_Id = projectWorked.Manager_Id;
            pw.IsActive = true;
            db.ProjectsWorkeds.Add(pw);
            db.SaveChanges();

            ci.P_Id = p.Id;
            ci.Designation = currentInfo.Designation;
            ci.DOJ = currentInfo.DOJ;
            ci.DateOfRelieving = currentInfo.DateOfRelieving;
            ci.ReportingManager = currentInfo.ReportingManager;
            ci.JobType = currentInfo.JobType;
            ci.DeployedCompanyDetails = currentInfo.DeployedCompanyDetails;
            ci.DeployedFromDate = currentInfo.DeployedFromDate;
            ci.DeployedToDate = currentInfo.DeployedToDate;
            ci.CompanyMailId = currentInfo.CompanyMailId;
            ci.BankName = currentInfo.BankName;
            ci.IFSC = currentInfo.IFSC;
            ci.ModeOfPayment = currentInfo.ModeOfPayment;
            ci.WorkLocation = currentInfo.WorkLocation;
            ci.Department = currentInfo.Department;
            ci.Grade = currentInfo.Grade;
            ci.Salary = currentInfo.Salary;
            ci.CTC = currentInfo.CTC;
            db.CurrentInfoes.Add(ci);
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id, string path)
        {
            getManagers();
            getProjectList();
            PersonalInfo p = db.PersonalInfoes.Find(id);
            PersonalInfoModel pm = new PersonalInfoModel();
            EmployeeInformationViewModel Eivw = new EmployeeInformationViewModel();

            Eivw.PersonalInfoVm = new PersonalInfoModel();

            Eivw.PersonalInfoVm.EmpCode = p.EmpCode;
            Eivw.PersonalInfoVm.Name = p.Name;
            Eivw.PersonalInfoVm.FatherName = p.FatherName;
            Eivw.PersonalInfoVm.DOB = p.DOB;
            Eivw.PersonalInfoVm.Gender = p.Gender;
            Eivw.PersonalInfoVm.PermanentAddress = p.PermanentAddress;
            Eivw.PersonalInfoVm.CommunicationAddress = p.CommunicationAddress;
            Eivw.PersonalInfoVm.Contact = p.Contact;
            Eivw.PersonalInfoVm.Qualification = p.Qualification;
            Eivw.PersonalInfoVm.Email = p.Email;
            Eivw.PersonalInfoVm.PanNo = p.PanNo;
            Eivw.PersonalInfoVm.AadharNo = p.AadharNo;
            Eivw.PersonalInfoVm.BloodGroup = p.BloodGroup;
            // Eivw.PersonalInfoVm.Image = path;

            List<EducationQualificationModel> edu = (from educat in db.EducationQualifications
                                                     where educat.IsActive == true && educat.P_Id == id
                                                     select new EducationQualificationModel
                                                     {
                                                         Id = educat.Id,
                                                         P_Id = educat.P_Id,
                                                         Degree = educat.Degree,
                                                         Board = educat.Board,
                                                         YearOfPassing = educat.YearOfPassing,
                                                         Percentage = educat.Percentage
                                                     }
                                                    ).ToList();

            Eivw.EducationQualification = edu;

            List<ExperienceModel> exp = (from ex in db.Experiences
                                         where ex.IsActive == true && ex.P_Id == id
                                         select new ExperienceModel
                                         {
                                             Id = ex.Id,
                                             P_Id = ex.P_Id,
                                             CompanyName = ex.CompanyName,
                                             FromDate = ex.FromDate,
                                             ToDate = ex.ToDate,
                                             Designation = ex.Designation,
                                             ProjectWorked = ex.ProjectWorked,
                                             TechnologiesUsed = ex.TechnologiesUsed
                                         }).ToList();
            Eivw.Experience = exp;

            //List<EmployeeSkill> esm = db.EmployeeSkills.Where(a => a.IsActive == true).ToList();

            //Eivw.empskilllist= esm;

            List<EmployeeSkillsModel> esm = (from es in db.EmployeeSkills
                                             where es.IsActive == true && es.P_Id == id
                                             select new EmployeeSkillsModel
                                             {
                                                 Id = es.Id,
                                                 P_Id = es.P_Id,
                                                 Skills=es.Skill.Name,
                                                 Years = es.Years,
                                                 Months = es.Months,
                                                 Status = es.Status
                                             }).ToList();
            Eivw.EmployeeSkills = esm;


            ProjectsWorkedmodel pwm = new ProjectsWorkedmodel();
            ProjectsWorked pwork = db.ProjectsWorkeds.FirstOrDefault(a => a.PersonalInfo_Id == id);

            pwm.Id = pwork.Id;
            pwm.PersonalInfo_Id = pwork.PersonalInfo_Id;
            pwm.Project_Id = pwork.Project_Id;
            pwm.Designation = pwork.Designation;
            pwm.Responsibilities = pwork.Responsibilities;
            pwm.StartDate = pwork.StartDate;
            pwm.EndDate = pwork.EndDate;
            pwm.Description = pwork.Description;
            pwm.TeamMembers = pwork.TeamMembers;
            pwm.Manager_Id = pwork.Manager_Id;

            Eivw.ProjectsWorkedVm = pwm;


            CurrentInfoModel curre = new CurrentInfoModel();

            CurrentInfo curr = db.CurrentInfoes.FirstOrDefault(a => a.P_Id == id);
            curre.Id = curr.Id;
            curre.P_Id = curr.P_Id;
            curre.Designation = curr.Designation;
            curre.DOJ = curr.DOJ;
            curre.DateOfRelieving = curr.DateOfRelieving;
            curre.ReportingManager = curr.ReportingManager;
            curre.JobType = curr.JobType;
            curre.DeployedCompanyDetails = curr.DeployedCompanyDetails;
            curre.DeployedFromDate = curr.DeployedFromDate;
            curre.DeployedToDate = curr.DeployedToDate;
            curre.CompanyMailId = curr.CompanyMailId;
            curre.BankName = curr.BankName;
            curre.IFSC = curr.IFSC;
            curre.ModeOfPayment = curr.ModeOfPayment;
            curre.WorkLocation = curr.WorkLocation;
            curre.Department = curr.Department;
            curre.Grade = curr.Grade;
            curre.Salary = curr.Salary;
            curre.CTC = curr.CTC;

            Eivw.CurrentInfoVm = curre;

            return View(Eivw);
        }
        [HttpPost]
        public ActionResult Edit(PersonalInfo personalInfo, List<EducationQualification> eductionQualifi, List<Experience> exper, List<EmployeeSkill> empSkill, ProjectsWorked projectWorked, CurrentInfo currentInfo)
        {
            PersonalInfo p = new PersonalInfo();
            ProjectsWorked pw = new ProjectsWorked();
            CurrentInfo ci = new CurrentInfo();

            p.EmpCode = personalInfo.EmpCode;
            p.Name = personalInfo.Name;
            p.FatherName = personalInfo.FatherName;
            p.DOB = personalInfo.DOB;
            p.Gender = personalInfo.Gender;
            p.PermanentAddress = personalInfo.PermanentAddress;
            p.CommunicationAddress = personalInfo.CommunicationAddress;
            p.Contact = personalInfo.Contact;
            p.Qualification = personalInfo.Qualification;
            p.Email = personalInfo.Email;
            p.PanNo = personalInfo.PanNo;
            p.AadharNo = personalInfo.AadharNo;
            p.BloodGroup = personalInfo.BloodGroup;
            //p.Image = path;
            //Convert.ToString(TempData["path"]);  
            p.IsActive = true;
            db.PersonalInfoes.Add(p);
            db.SaveChanges();

            if (eductionQualifi != null)
            {
                foreach (var equali in eductionQualifi)
                {
                    equali.P_Id = p.Id;
                    equali.IsActive = true;
                    db.EducationQualifications.Add(equali);
                    db.SaveChanges();
                }
            }

            if (exper != null)
            {
                foreach (var exp in exper)
                {
                    exp.P_Id = p.Id;
                    exp.IsActive = true;
                    db.Experiences.Add(exp);
                    db.SaveChanges();
                }
            }

            if (empSkill != null)
            {
                foreach (var ski in empSkill)
                {
                    ski.P_Id = p.Id;
                    ski.IsActive = true;
                    db.EmployeeSkills.Add(ski);
                    db.SaveChanges();
                }
            }

            pw.PersonalInfo_Id = p.Id;
            pw.Project_Id = projectWorked.Project_Id;
            pw.Designation = projectWorked.Designation;
            pw.Responsibilities = projectWorked.Responsibilities;
            pw.StartDate = projectWorked.StartDate;
            pw.EndDate = projectWorked.EndDate;
            pw.Description = projectWorked.Description;
            pw.TeamMembers = projectWorked.TeamMembers;
            pw.Manager_Id = projectWorked.Manager_Id;
            pw.IsActive = true;
            db.ProjectsWorkeds.Add(pw);
            db.SaveChanges();

            ci.P_Id = p.Id;
            ci.Designation = currentInfo.Designation;
            ci.DOJ = currentInfo.DOJ;
            ci.DateOfRelieving = currentInfo.DateOfRelieving;
            ci.ReportingManager = currentInfo.ReportingManager;
            ci.JobType = currentInfo.JobType;
            ci.DeployedCompanyDetails = currentInfo.DeployedCompanyDetails;
            ci.DeployedFromDate = currentInfo.DeployedFromDate;
            ci.DeployedToDate = currentInfo.DeployedToDate;
            ci.CompanyMailId = currentInfo.CompanyMailId;
            ci.BankName = currentInfo.BankName;
            ci.IFSC = currentInfo.IFSC;
            ci.ModeOfPayment = currentInfo.ModeOfPayment;
            ci.WorkLocation = currentInfo.WorkLocation;
            ci.Department = currentInfo.Department;
            ci.Grade = currentInfo.Grade;
            ci.Salary = currentInfo.Salary;
            ci.CTC = currentInfo.CTC;
            db.CurrentInfoes.Add(ci);
            db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);

        }

        public ActionResult viewdetails(int id)
        {
            getManagers();
            getProjectList();
            PersonalInfo p = db.PersonalInfoes.Find(id);
            PersonalInfoModel pm = new PersonalInfoModel();
            EmployeeInformationViewModel Eivw = new EmployeeInformationViewModel();

            Eivw.PersonalInfoVm = new PersonalInfoModel();

            Eivw.PersonalInfoVm.EmpCode = p.EmpCode;
            Eivw.PersonalInfoVm.Name = p.Name;
            Eivw.PersonalInfoVm.FatherName = p.FatherName;
            Eivw.PersonalInfoVm.DOB = p.DOB;
            Eivw.PersonalInfoVm.Gender = p.Gender;
            Eivw.PersonalInfoVm.PermanentAddress = p.PermanentAddress;
            Eivw.PersonalInfoVm.CommunicationAddress = p.CommunicationAddress;
            Eivw.PersonalInfoVm.Contact = p.Contact;
            Eivw.PersonalInfoVm.Qualification = p.Qualification;
            Eivw.PersonalInfoVm.Email = p.Email;
            Eivw.PersonalInfoVm.PanNo = p.PanNo;
            Eivw.PersonalInfoVm.AadharNo = p.AadharNo;
            Eivw.PersonalInfoVm.BloodGroup = p.BloodGroup;

            List<EducationQualificationModel> edu = (from educat in db.EducationQualifications
                                                     where educat.IsActive == true && educat.P_Id == id
                                                     select new EducationQualificationModel
                                                     {
                                                         Id = educat.Id,
                                                         P_Id = educat.P_Id,
                                                         Degree = educat.Degree,
                                                         Board = educat.Board,
                                                         YearOfPassing = educat.YearOfPassing,
                                                         Percentage = educat.Percentage
                                                     }
                                                    ).ToList();

            Eivw.EducationQualification = edu;

            List<ExperienceModel> exp = (from ex in db.Experiences
                                         where ex.IsActive == true && ex.P_Id == id
                                         select new ExperienceModel
                                         {
                                             Id = ex.Id,
                                             P_Id = ex.P_Id,
                                             CompanyName = ex.CompanyName,
                                             FromDate = ex.FromDate,
                                             ToDate = ex.ToDate,
                                             Designation = ex.Designation,
                                             ProjectWorked = ex.ProjectWorked,
                                             TechnologiesUsed = ex.TechnologiesUsed
                                         }).ToList();
            Eivw.Experience = exp;


            List<EmployeeSkillsModel> esm = (from es in db.EmployeeSkills
                                             where es.IsActive == true && es.P_Id == id
                                             select new EmployeeSkillsModel
                                             {
                                                 Id = es.Id,
                                                 P_Id = es.P_Id,
                                                 // Skills=es.Skills,
                                                 Years = es.Years,
                                                 Months = es.Months,
                                                 Status = es.Status
                                             }).ToList();
            Eivw.EmployeeSkills = esm;


            ProjectsWorkedmodel pwm = new ProjectsWorkedmodel();
            ProjectsWorked pwork = db.ProjectsWorkeds.FirstOrDefault(a => a.PersonalInfo_Id == id);

            pwm.Id = pwork.Id;
            pwm.PersonalInfo_Id = pwork.PersonalInfo_Id;
            pwm.Project_Id = pwork.Project_Id;
            pwm.Designation = pwork.Designation;
            pwm.Responsibilities = pwork.Responsibilities;
            pwm.StartDate = pwork.StartDate;
            pwm.EndDate = pwork.EndDate;
            pwm.Description = pwork.Description;
            pwm.TeamMembers = pwork.TeamMembers;
            pwm.Manager_Id = pwork.Manager_Id;

            Eivw.ProjectsWorkedVm = pwm;


            CurrentInfoModel curre = new CurrentInfoModel();

            CurrentInfo curr = db.CurrentInfoes.FirstOrDefault(a => a.P_Id == id);
            curre.Id = curr.Id;
            curre.P_Id = curr.P_Id;
            curre.Designation = curr.Designation;
            curre.DOJ = curr.DOJ;
            curre.DateOfRelieving = curr.DateOfRelieving;
            curre.ReportingManager = curr.ReportingManager;
            curre.JobType = curr.JobType;
            curre.DeployedCompanyDetails = curr.DeployedCompanyDetails;
            curre.DeployedFromDate = curr.DeployedFromDate;
            curre.DeployedToDate = curr.DeployedToDate;
            curre.CompanyMailId = curr.CompanyMailId;
            curre.BankName = curr.BankName;
            curre.IFSC = curr.IFSC;
            curre.ModeOfPayment = curr.ModeOfPayment;
            curre.WorkLocation = curr.WorkLocation;
            curre.Department = curr.Department;
            curre.Grade = curr.Grade;
            curre.Salary = curr.Salary;
            curre.CTC = curr.CTC;

            Eivw.CurrentInfoVm = curre;

            return View(Eivw);
        }

        public ActionResult Delete(int id)
        {
            getManagers();
            getProjectList();
            PersonalInfo p = db.PersonalInfoes.Find(id);
            PersonalInfoModel pm = new PersonalInfoModel();
            EmployeeInformationViewModel Eivw = new EmployeeInformationViewModel();

            Eivw.PersonalInfoVm = new PersonalInfoModel();

            Eivw.PersonalInfoVm.EmpCode = p.EmpCode;
            Eivw.PersonalInfoVm.Name = p.Name;
            Eivw.PersonalInfoVm.FatherName = p.FatherName;
            Eivw.PersonalInfoVm.DOB = p.DOB;
            Eivw.PersonalInfoVm.Gender = p.Gender;
            Eivw.PersonalInfoVm.PermanentAddress = p.PermanentAddress;
            Eivw.PersonalInfoVm.CommunicationAddress = p.CommunicationAddress;
            Eivw.PersonalInfoVm.Contact = p.Contact;
            Eivw.PersonalInfoVm.Qualification = p.Qualification;
            Eivw.PersonalInfoVm.Email = p.Email;
            Eivw.PersonalInfoVm.PanNo = p.PanNo;
            Eivw.PersonalInfoVm.AadharNo = p.AadharNo;
            Eivw.PersonalInfoVm.BloodGroup = p.BloodGroup;

            List<EducationQualificationModel> edu = (from educat in db.EducationQualifications
                                                     where educat.IsActive == true && educat.P_Id == id
                                                     select new EducationQualificationModel
                                                     {
                                                         Id = educat.Id,
                                                         P_Id = educat.P_Id,
                                                         Degree = educat.Degree,
                                                         Board = educat.Board,
                                                         YearOfPassing = educat.YearOfPassing,
                                                         Percentage = educat.Percentage
                                                     }
                                                    ).ToList();

            Eivw.EducationQualification = edu;

            List<ExperienceModel> exp = (from ex in db.Experiences
                                         where ex.IsActive == true && ex.P_Id == id
                                         select new ExperienceModel
                                         {
                                             Id = ex.Id,
                                             P_Id = ex.P_Id,
                                             CompanyName = ex.CompanyName,
                                             FromDate = ex.FromDate,
                                             ToDate = ex.ToDate,
                                             Designation = ex.Designation,
                                             ProjectWorked = ex.ProjectWorked,
                                             TechnologiesUsed = ex.TechnologiesUsed
                                         }).ToList();
            Eivw.Experience = exp;


            List<EmployeeSkillsModel> esm = (from es in db.EmployeeSkills
                                             where es.IsActive == true && es.P_Id == id
                                             select new EmployeeSkillsModel
                                             {
                                                 Id = es.Id,
                                                 P_Id = es.P_Id,
                                                 Skills=es.Skill.Name,
                                                 Years = es.Years,
                                                 Months = es.Months,
                                                 Status = es.Status
                                             }).ToList();
            Eivw.EmployeeSkills = esm;

            ProjectsWorkedmodel pwm = new ProjectsWorkedmodel();
            ProjectsWorked pwork = db.ProjectsWorkeds.FirstOrDefault(a => a.PersonalInfo_Id == id);

            pwm.Id = pwork.Id;
            pwm.PersonalInfo_Id = pwork.PersonalInfo_Id;
            pwm.Project_Id = pwork.Project_Id;
            pwm.Designation = pwork.Designation;
            pwm.Responsibilities = pwork.Responsibilities;
            pwm.StartDate = pwork.StartDate;
            pwm.EndDate = pwork.EndDate;
            pwm.Description = pwork.Description;
            pwm.TeamMembers = pwork.TeamMembers;
            pwm.Manager_Id = pwork.Manager_Id;

            Eivw.ProjectsWorkedVm = pwm;

            CurrentInfoModel curre = new CurrentInfoModel();

            CurrentInfo curr = db.CurrentInfoes.FirstOrDefault(a => a.P_Id == id);
            curre.Id = curr.Id;
            curre.P_Id = curr.P_Id;
            curre.Designation = curr.Designation;
            curre.DOJ = curr.DOJ;
            curre.DateOfRelieving = curr.DateOfRelieving;
            curre.ReportingManager = curr.ReportingManager;
            curre.JobType = curr.JobType;
            curre.DeployedCompanyDetails = curr.DeployedCompanyDetails;
            curre.DeployedFromDate = curr.DeployedFromDate;
            curre.DeployedToDate = curr.DeployedToDate;
            curre.CompanyMailId = curr.CompanyMailId;
            curre.BankName = curr.BankName;
            curre.IFSC = curr.IFSC;
            curre.ModeOfPayment = curr.ModeOfPayment;
            curre.WorkLocation = curr.WorkLocation;
            curre.Department = curr.Department;
            curre.Grade = curr.Grade;
            curre.Salary = curr.Salary;
            curre.CTC = curr.CTC;

            Eivw.CurrentInfoVm = curre;

            return View(Eivw);
        }

        [HttpPost]
        public ActionResult Delete(int id,PersonalInfoModel perm)
        {
            try
            {
                PersonalInfo per = db.PersonalInfoes.Find(id);                
                per.IsActive = false;
                db.Entry(per).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();
                ViewBag.Message = "Successfully Deleted";
                return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}