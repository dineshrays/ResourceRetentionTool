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

        public ActionResult Evaluates()
        {

            return View();
                
        }
    }
}