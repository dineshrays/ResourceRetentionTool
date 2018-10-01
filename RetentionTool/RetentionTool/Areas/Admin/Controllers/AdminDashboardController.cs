using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Areas.Admin.Controllers
{
  
    public class AdminDashboardController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Admin/AdminDashboard
        public ActionResult Index()
        {
            var projectworkcount = 0;
            var backupcount = 0;
            var assignevalcount = 0;
            var plantrainingcount = 0;
            var modulecount = 0;
            var employeecount = 0;
            modulecount = db.Modules.Where(a => a.IsActive == true).Count();
            employeecount = db.PersonalInfoes.Where(a => a.IsActive == true).Count();
            Session["modulecount"] = modulecount;
            Session["employeecount"] = employeecount;
            projectworkcount = db.ProjectsWorkeds.Where(a => a.IsActive == true).GroupBy(p => p.Project_Id).Count();
            //.Count();
            //.GroupBy(p => p.Project_Id).Select(grp => grp.FirstOrDefault());
            Session["projectworkcount"] = projectworkcount;
            int projectid = fetchdet.getDefaultProjectId();
            backupcount = (from assignres in db.AssignResources
                           where assignres.IsActive == true && assignres.Project_Id != projectid
                        
                         && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.
                        Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                           select assignres).Count();
            Session["backupcount"] = backupcount;

            assignevalcount = db.AssignEvaluaters.Where(a =>
          !db.EmployeeEvalTasks.Any(
               e => e.AssignResource_Id == a.AssignResource_Id && e.IsActive == true &&
               db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
               t.IsEligiableMark == true && t.IsActive == true)) &&
          db.AssignResources.Any(b => b.Id == a.AssignResource_Id &&
      b.IsActive == true) &&
           a.IsActive == true).Count();
            Session["assignevalcount"] = assignevalcount;
            plantrainingcount = (from assignres in db.AssignResources

                                 where assignres.IsActive == true
                                 && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                                 && assignres.Project_Id == projectid
                             
                                 select assignres).Count();
            Session["plantrainingcount"] = plantrainingcount;
            return View();
        }
    }
}