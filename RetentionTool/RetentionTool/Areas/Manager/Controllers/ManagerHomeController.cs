using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
namespace RetentionTool.Areas.Manager.Controllers
{
    public class ManagerHomeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        public static FetchDefaultIds fetchdet = new FetchDefaultIds();
        int managerid = fetchdet.getUserDetailsId();
        // GET: Manager/ManagerHome
        public ActionResult Index()
        {
            var projectworkcount = 0;
            var backupcount = 0;
            var assignevalcount = 0;
            var plantrainingcount = 0;
            projectworkcount = db.ProjectsWorkeds.Where(a => a.Manager_Id == managerid && a.IsActive == true).Count();
            Session["projectworkcount"] = projectworkcount;
            int projectid = fetchdet.getDefaultProjectId();
            backupcount = (from assignres in db.AssignResources
                           where assignres.IsActive == true && assignres.Project_Id != projectid
                          
                         && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.
                        Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                         && assignres.Manager_Id == managerid select assignres).Count();
            Session["backupcount"] = backupcount;

            assignevalcount = db.AssignEvaluaters.Where(a =>
          !db.EmployeeEvalTasks.Any(
               e => e.AssignResource_Id == a.AssignResource_Id && e.IsActive == true &&
               db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
               t.IsEligiableMark == true && t.IsActive == true)) &&
          db.AssignResources.Any(b => b.Id == a.AssignResource_Id &&
          b.Manager_Id == managerid && b.IsActive == true) &&
           a.IsActive == true).Count();
            Session["assignevalcount"] = assignevalcount;
            plantrainingcount = (from assignres in db.AssignResources
                          
                       where assignres.IsActive == true
                       && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                       && assignres.Project_Id == projectid
                       && assignres.Manager_Id == managerid
                       select assignres).Count();
            Session["plantrainingcount"] = plantrainingcount;
            return View();
        }
    }
}