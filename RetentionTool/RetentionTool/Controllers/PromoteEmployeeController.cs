using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;
using RetentionTool.Models;
namespace RetentionTool.Controllers
{
    public class PromoteEmployeeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: PromoteEmployee
        public ActionResult Index()
        {
            List<AssignResourceViewModel> assgnvm = (from assignres in db.AssignResources
                                                     join assignresdet in db.AssignResourcesDets
                                                     on assignres.Id equals assignresdet.AssignResources_Id
                                                     join m in db.Modules
                                                     on assignres.Module_Id equals m.Id
                                                     where assignres.IsActive == true
                                                     && !db.RateEmployeeEligiabilities.Any(a => a.IsEligible==true && a.Employee_Id==assignresdet.Employee_Id && a.AssignResources_Id==assignres.Id && a.IsActive==true)
                                                     && db.Trainings.Any(a=>a.AssignResource_Id==assignres.Id && a.IsActive==true)
                                                     && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
                                                     select new AssignResourceViewModel
                                                     {
                                                         Id = assignres.Id,
                                                      
                                                         modulename = assignres.Module.ModuleName,
                                                         managername=assignres.PersonalInfo.Name,
                                                         Manager_Id=assignres.Manager_Id,
                                                         Module_Id = assignres.Module_Id,
                                                         Project_Id = assignres.Project_Id,
                                                         projectname = assignres.ProjectsDetail.Name,
                                                         Employee_Id = assignresdet.Employee_Id,
                                                         employeename=assignresdet.PersonalInfo.Name
                                                     }).ToList();

            ViewBag.assignresource = assgnvm;
            return View();
        }
        

        public ActionResult Promote(int id,int empid)
        {
            RateEmployeeEligiability rateempelig=new RateEmployeeEligiability();
            rateempelig.Employee_Id = empid;
            rateempelig.AssignResources_Id = id;

            RateEmployeeEligiability rateEmp = db.RateEmployeeEligiabilities.FirstOrDefault(a=>a.AssignResources_Id==id && a.Employee_Id==rateempelig.Employee_Id);
            if(rateEmp!=null && rateEmp.Id>0)
            {
                rateEmp.IsEligible = true;
                db.Entry(rateEmp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            else
            {
                rateempelig.Grade = "A";
                rateempelig.IsEligible = true;
                rateempelig.IsActive = true;
                db.RateEmployeeEligiabilities.Add(rateempelig);
                db.SaveChanges();
            }
           
            return RedirectToAction("Index");
        }
        public ActionResult getTopics(int assId,int empid)
        {

            //select moduledet.Topics,sessdet.Employee_Id,sessdet.Remarks from ModuleDet moduledet
            //           inner join TrainingDet trainingdet
            // on moduledet.Id = trainingdet.ModuleDet_Id
            // inner join Sessions sess
            // on sess.TrainingDet_Id = trainingdet.Id
            // inner join SessionsDet sessdet
            // on sess.Id = sessdet.Sessions_Id
            // inner join Training training
            // on training.Id = trainingdet.Training_Id
            //where training.AssignResource_Id = 6015 and
            //sessdet.Employee_Id = 1008
            List<PromoteEmployeeModel> promote = (from moduledet in db.ModuleDets
                                                  join trainingdet in db.TrainingDets
                                                  on moduledet.Id equals trainingdet.ModuleDet_Id
                                                  join sess in db.Sessions
                                                  on trainingdet.Id equals sess.TrainingDet_Id
                                                  join sessdet in db.SessionsDets
                                                  on sess.Id equals sessdet.Sessions_Id
                                                  join training in db.Trainings
                                                  on trainingdet.Training_Id equals training.Id
                                                  where training.AssignResource_Id == assId &&
                                                  sessdet.Employee_Id == empid
                                                  select new PromoteEmployeeModel {
                                                      Topics=moduledet.Topics,
                                                      Remark=sessdet.Remarks
                                                  }



                ).ToList();

            return Json(promote, JsonRequestBehavior.AllowGet);
            //PartialView("_Empdetails", emplist);
        }
    }
}