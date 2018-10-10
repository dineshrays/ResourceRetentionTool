using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;

namespace RetentionTool.Areas.Trainer.Controllers
{
    public class TrainerHomeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: EmployeeEvaTaskl
        // GET: Trainer/TrainerHome
        public ActionResult Index()
        {
            int trainerid = fetchdet.getUserDetailsId();
            Session["empevalcount"] = null;
          Session["rateempcount"] = null;
            Session["sessionsummarycount"] = null;
            Session["topicscount"] = null;
          var assignResources = db.AssignResources.Where(a => db.AssignEvaluaters.Any(p2 => p2.AssignResource_Id == a.Id && db.Trainers.Any(t => t.Id == p2.Trainer_Id && t.PersonalInfo_Id == trainerid && t.IsActive == true) && p2.IsActive == true && a.IsActive == true)
                                                                          || db.EmployeeEvalTasks.Any(e => e.AssignResource_Id == a.Id
                                                                          && db.Trainers.Any(t => t.Id == e.Trainer_Id && t.PersonalInfo_Id == trainerid && t.IsActive == true) && e.IsActive == true &&
                                                                          db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id && t.IsEligiableMark == true && t.IsActive == true))
                                                                           ).ToList().Count;
           
           var rateemp = db.AssignResources.Where(a => !db.EmployeeEvalTasks.Any(e => e.AssignResource_Id == a.Id && e.IsActive == true &&
                                                                     db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
                                                                     t.IsEligiableMark == true && t.IsActive == true)) && a.IsActive == true
                                                                     && (db.Trainings.Any(t => t.AssignResource_Id == a.Id &&
                                                                     t.IsActive == true && db.TrainingDets.Count(d => d.Training_Id == t.Id && d.IsActive == true).Equals(
                                                                     db.Sessions.Count(s => s.TrainingDet.Training_Id == t.Id && s.IsActive == true)))
                                                                     || db.RateEmployeeEligiabilities.Any(b => b.AssignResources_Id == a.Id && b.IsActive == true))
                                                                      && db.Trainers.Any(t => t.Id == a.Trainer_Id && t.IsActive == true && t.PersonalInfo_Id == trainerid)
                                                                     ).ToList().Count;
          var sessionsum = (from assignres in db.AssignResources
                                                           join trainer in db.Trainers
                                                           on assignres.Trainer_Id equals trainer.Id
                                                           where
                                                           db.Trainings.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true)
                                                           && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true &&
                                                           db.EmployeeEvalTaskDets.Any(c => c.EmployeeEvalTask_Id == a.Id && c.IsEligiableMark == true
                                                           && c.IsActive == true)) && trainer.PersonalInfo_Id == trainerid
                                                           select assignres.Id).Distinct().ToList().Count;
            var trainingcount = (from assignres in db.AssignResources
                           join m in db.Modules
                           on assignres.Module_Id equals m.Id
                           join trainer in db.Trainers
                           on assignres.Trainer_Id equals trainer.Id
                           where assignres.IsActive == true
                           && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && 
                           db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                          && trainer.PersonalInfo_Id == trainerid
                           select assignres.Id
                               ).ToList().Count;

            Session["empevalcount"] = assignResources;
            Session["rateempcount"] = rateemp;
            Session["sessionsummarycount"] = sessionsum;
            Session["topicscount"] = trainingcount;
            return View();
        }
    }
}