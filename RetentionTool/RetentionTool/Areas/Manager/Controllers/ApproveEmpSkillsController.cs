using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;


namespace RetentionTool.Areas.Manager.Controllers
{
    public class ApproveEmpSkillsController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Manager/ApproveEmpSkills
        public ActionResult Index()
        {
            List<EmployeeSkillsAdd> emp = db.EmployeeSkillsAdds.Where(a => db.PersonalInfoes.Any(b => b.Id == a.P_Id)).ToList();
            EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();
            
            skilladd.skiladd = emp;
            return View(skilladd);
        }

        public ActionResult Evaluates(int id)
        {
            EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find(id);
            EmployeeSkillsAddViewModel empvm = new EmployeeSkillsAddViewModel();
            empvm.EmployeeSkillsAdd = esd;
            getEvaluaterList(esd.Skills_Id);
            return View(empvm);
                
        }
        
        [HttpPost]
        public ActionResult Evaluates(int id, ApproveEmpSkill appl)
        {
            ApproveEmpSkill app = db.ApproveEmpSkills.Find(id);


            return View();
        }

        public void getEvaluaterList(long? skillid)
        {
            List<EmployeeList> employeeList = (from personal in db.PersonalInfoes
                                               join empskills in db.EmployeeSkills on personal.Id equals empskills.P_Id
                                               join skill in db.Skills on empskills.Skills_Id equals skill.id
                                               where skill.id==skillid
                                               select new EmployeeList
                                               {
                                                   Id = personal.Id,
                                                   Name = personal.Name,
                                                   EmpCode = personal.EmpCode

                                               }).ToList();
            
           // return Json(employeeList, JsonRequestBehavior.AllowGet);
           ViewData["Evaluater"] = new SelectList(employeeList,"Id","Name");
        }


    }
}