using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
namespace RetentionTool.Controllers
{
    public class ProjectWorkedController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: ProjectWorked
        public ActionResult Index()
        {
            List<ProjectsWorked> prjctwrk = db.ProjectsWorkeds.Where(a => a.IsActive == true).ToList();
            ProjectWorkedViewModel prjctwrkvm = new ProjectWorkedViewModel();

            prjctwrkvm.projectvm = prjctwrk;
            //ViewBag.pro = prjctwrk;
            return View(prjctwrkvm);
        }

        public ActionResult Create()
        {
            getManagers();
            getProjectList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(List<ProjectsWorked> prjctwrkvm)
        {
            if(prjctwrkvm!=null)
            {
                foreach(var prjctwrk in prjctwrkvm)
                {
                    prjctwrk.IsActive = true;
                    db.ProjectsWorkeds.Add(prjctwrk);
                    db.SaveChanges();
                }
            }
          
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            ProjectsWorked prjctwrk = db.ProjectsWorkeds.Find(id);
            ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            prjwrkvm.projects = prjctwrk;
            getManagers();
            getProjectList();
            return View(prjwrkvm);
        }
        [HttpPost]
        public ActionResult Edit(int id,ProjectWorkedViewModel prjctworkvm)
        {
            ProjectsWorked projectWorked = new ProjectsWorked();
            projectWorked = prjctworkvm.projects;
            projectWorked.IsActive = true;
            db.Entry(projectWorked).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            ProjectsWorked prjctwrk = db.ProjectsWorkeds.Find(id);
            ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            prjwrkvm.projects = prjctwrk;
            return View(prjwrkvm);
        }
        [HttpPost]
        public ActionResult Delete(int id, ProjectWorkedViewModel prjctworkvm)
        {
            ProjectsWorked projectWorked = db.ProjectsWorkeds.Find(id);
            if(projectWorked.Id==id)
            {
                projectWorked.IsActive = false;
                db.Entry(projectWorked).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        public void getManagers()
        {
            var val = new SelectList(db.PersonalInfoes.ToList(), "id", "Name");
            ViewData["managerslist"] = val;
        }
        public void getProjectList()
        {
            ViewData["projectlist"] = new SelectList(db.ProjectsDetails.ToList(),"Id","Name");
        }
        public JsonResult getEmployee(string name)
        {
            //List<string> employee = new List<string>();
            //var val = (from e in db.Employees
            //where e.Name.Contains(name)
            //select new { e.Name });
            // var val = db.Employees.Where(a => a.Name.Contains(name)).ToList();
            //IEnumerable<SelectListItem> skilldet =  
            //  List<string> va = new List<string>();
            List<EmployeeList> va = (from emp in db.PersonalInfoes
                                     where emp.EmpCode.Contains(name)

            && emp.IsActive == true
                                     select new EmployeeList
                                     {
                                         Id = emp.Id,
                                         EmpCode = emp.EmpCode,
                                         Name = emp.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}