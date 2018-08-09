using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class EmployeeInfoController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PersonalInfoCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PersonalInfoCreate(PersonalInfoModel pim)
        {
            PersonalInfo p = new PersonalInfo();
            p.EmpCode = pim.EmpCode;
            p.Name = pim.Name;
            p.FatherName = pim.FatherName;
            p.DOB = pim.DOB;
            p.Gender = pim.Gender;
            p.PermanentAddress = pim.PermanentAddress;
            p.CommunicationAddress = pim.CommunicationAddress;
            p.Contact = pim.Contact;
            p.Qualification = pim.Qualification;
            p.Email = pim.Email;
            p.PanNo = pim.PanNo;
            p.AadharNo = pim.AadharNo;
            p.BloodGroup = pim.BloodGroup;
            p.IsActive = true;

            db.PersonalInfoes.Add(p);
            db.SaveChanges();
            

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EduQualificationCreate()
        {
            getId();
            return View();

        }
        public void getId()
        {
            FetchDefaultIds fetchdet = new FetchDefaultIds();
            int emproleid = fetchdet.getDefaultEmployeeRoleId();
            var data = (from personalInfo in db.PersonalInfoes
                        join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
                        where personalInfo.IsActive == true
                        && userdet.IsActive == true
                        && userdet.Role_Id == emproleid
                        select new
                        {
                            Id = personalInfo.Id,
                            Name = personalInfo.Name
                        }).ToList();

            ViewData["Id"] = new SelectList(data, "Id","Name");
        }
        
        [HttpPost]
        public ActionResult EduQualificationCreate(EducationQualificationModel eqm)
        {
            EducationQualification edu = new EducationQualification();

            edu.P_Id = eqm.P_Id; 
            edu.Degree = eqm.Degree;
            edu.Board = eqm.Board;
            edu.YearOfPassing = eqm.YearOfPassing;
            edu.Percentage = eqm.Percentage;
            edu.IsActive = true;

            db.EducationQualifications.Add(edu);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpSkillsCreate()
        {
            getId();
            return View();

        }
        [HttpPost]
        public ActionResult EmpSkillsCreate(List<EmployeeSkillsAdd> empSkill)
        {

            if(empSkill != null)
            {
                foreach (var ski in empSkill)
                {
                    ski.IsActive = true;
                    db.EmployeeSkillsAdds.Add(ski);
                    db.SaveChanges();
                }
            }
            //EmployeeSkill e = new EmployeeSkill();
                        
            //e.P_Id = es.P_Id;
            //e.CommonField_Id = es.CommonField_Id;
            //e.Skills_Id = es.Skills_Id;
            //e.Years = es.Years;
            //e.Months = es.Months;
            //e.Status = es.Status;
            //e.IsActive = true;

            //db.EmployeeSkills.Add(e);
            //db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSkills(string name)
        {
          
            List< SkillsViewModel> va = (from skill in db.Skills
                                     where skill.Name.Contains(name)
                                     select new SkillsViewModel
                                     {
                                         id = skill.id,
                                        
                                         SkillName = skill.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}