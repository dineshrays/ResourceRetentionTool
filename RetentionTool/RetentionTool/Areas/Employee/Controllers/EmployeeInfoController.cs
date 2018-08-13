using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Areas.Employee.Controllers
{
    public class EmployeeInfoController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInfo
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult PersonalInfoCreate()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult PersonalInfoCreate(PersonalInfoModel pim)
        //{
        //    PersonalInfo p = new PersonalInfo();
        //    p.EmpCode = pim.EmpCode;
        //    p.Name = pim.Name;
        //    p.FatherName = pim.FatherName;
        //    p.DOB = pim.DOB;
        //    p.Gender = pim.Gender;
        //    p.PermanentAddress = pim.PermanentAddress;
        //    p.CommunicationAddress = pim.CommunicationAddress;
        //    p.Contact = pim.Contact;
        //    p.Qualification = pim.Qualification;
        //    p.Email = pim.Email;
        //    p.PanNo = pim.PanNo;
        //    p.AadharNo = pim.AadharNo;
        //    p.BloodGroup = pim.BloodGroup;
        //    p.IsActive = true;

        //    db.PersonalInfoes.Add(p);
        //    db.SaveChanges();
            

        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult EduQualificationCreate()
        //{
        //    getId();
        //    return View();

        //}
        //public void getId()
        //{
        //    FetchDefaultIds fetchdet = new FetchDefaultIds();
        //    int id = fetchdet.getUserId();
        //    ViewData["Id"] = new SelectList(db.PersonalInfoes.Where(a=>a.Id==id).ToList(), "Id","Name");
        //}
        
        //[HttpPost]
        //public ActionResult EduQualificationCreate(EducationQualificationModel eqm)
        //{
        //    EducationQualification edu = new EducationQualification();

        //    edu.P_Id = eqm.P_Id; 
        //    edu.Degree = eqm.Degree;
        //    edu.Board = eqm.Board;
        //    edu.YearOfPassing = eqm.YearOfPassing;
        //    edu.Percentage = eqm.Percentage;
        //    edu.IsActive = true;

        //    db.EducationQualifications.Add(edu);
        //    db.SaveChanges();
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EmpSkillsCreate()
        {
            FetchDefaultIds fetchdet = new FetchDefaultIds();
            int id = fetchdet.getUserId();
            PersonalInfo personalinfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == id);
            EmployeeInfoViewModel empvm = new EmployeeInfoViewModel();
            PersonalInfoModel personalInfovm = new PersonalInfoModel();
            personalInfovm.Id = personalinfo.Id;
            personalInfovm.Name = personalinfo.Name;
            empvm.PersonalInfoVm = personalInfovm;
           // ViewData["Id"] = new SelectList(db.PersonalInfoes.Where(a => a.Id == id).ToList(), "Id", "Name");
            //getId();
            return View(empvm);

        }
        [HttpPost]
        public ActionResult EmpSkillsCreate(List<EmployeeSkillsAdd> empSkill)
        {
            if (empSkill != null)
            {
                foreach (var ski in empSkill)
                {
                    CurrentInfo currInfo = db.CurrentInfoes.FirstOrDefault(a => a.P_Id == ski.P_Id);
                    int mangerid = int.Parse(currInfo.ReportingManager);
                    ski.Manager_Id =mangerid ;
                    ski.IsApproved = false;
                    ski.IsActive = true;
                    db.EmployeeSkillsAdds.Add(ski);
                    db.SaveChanges();
                }
            }
            
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