using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class EmployeeEvaTasklController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeEvaTaskl
        public ActionResult Index()
        {
            List<AssignResource> assignResources = db.AssignResources.Where(a => db.AssignEvaluaters.Any(p2 => p2.AssignResource_Id == a.Id && p2.IsActive == true && a.IsActive == true)).ToList();
            ViewBag.assResDetails = assignResources;
           
            List<EmployeeEvalTask> empevaltask = db.EmployeeEvalTasks.Where(a => a.IsActive == true).Distinct().ToList();
            ViewBag.empevalDetails = empevaltask;
            
            return View(); 
        }
        public ActionResult Create()
        {
            //List<AssignResource> assignResources = db.AssignResources.Where(a => db.AssignEvaluaters.Any(p2 => p2.AssignResource_Id == a.Id && p2.IsActive==true && a.IsActive == true)).ToList();
            ////(from ass in db.AssignResources join
            ////                                    training in db.Trainings on ass.Id equals training.AssignResource_Id
            ////                                    join trainingdet in db.TrainingDets on  training.Id equals trainingdet.Training_Id
            ////                                    join session in db.Sessions on trainingdet.Id equals session.TrainingDet_Id
            ////                                   where ass.IsActive == true && session.IsActive==true && training.IsActive==true
            ////                                    select ass

            ////                                    ).ToList();
            ////db.AssignResources.Where(a=>a.IsActive==true).ToList();
            ////a=> !db.AssignResources.Any(p2=>p2.Trainer_Id==a.Id
            //ViewBag.assResDetails = assignResources;
          
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeEvalTask empeval, EmployeeEvalTaskDet[] list)
        {
            empeval.IsActive = true;
            db.EmployeeEvalTasks.Add(empeval);
            db.SaveChanges();

            foreach (var i in list)
            {
                EmployeeEvalTaskDet empevaldet = new EmployeeEvalTaskDet()
                {
                    EmployeeEvalTask_Id = empeval.Id,
                    Employee_Id =i.Employee_Id,
                    Status = i.Status,
                    TaskAssigned = i.TaskAssigned,
                    IsEligiableMark = i.IsEligiableMark,
                    IsActive = true
                };
                db.EmployeeEvalTaskDets.Add(empevaldet);
                db.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        
        }
        public ActionResult Edit(int id)
        {
            EmployeeEvalTask empEval = db.EmployeeEvalTasks.Find(id);
            List<EmployeeEvalTaskDet> empevaltask = db.EmployeeEvalTaskDets.Where(a => a.EmployeeEvalTask_Id == id).ToList();
          
            ViewBag.empDetails = empevaltask;
            getTrainers(empEval.AssignResource_Id);
            return View(empEval);
        }
        [HttpPost]
        public ActionResult Edit(int id, EmployeeEvalTask empeval, EmployeeEvalTaskDet[] list)
        {


            //var x = (from y in db.EmployeeEvalTaskDets where y.EmployeeEvalTask_Id == id select y);
            //foreach (var i in x)
            //{
            //    db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            //}
            //db.SaveChanges();
            empeval.IsActive = true;
            db.Entry(empeval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            foreach (var i in list)
            {
                i.EmployeeEvalTask_Id = id;
                i.IsActive = true;
                db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //EmployeeEvalTaskDet empevaldet = new EmployeeEvalTaskDet()
                //{
                //    EmployeeEvalTask_Id = empeval.Id,

                //    Employee_Id = i.Employee_Id,
                //    Status = i.Status,
                //    TaskAssigned = i.TaskAssigned,
                //    IsEligiableMark = i.IsEligiableMark,
                //    IsActive = true
                //};
                //db.EmployeeEvalTaskDets.Add(empevaldet);
                //db.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            EmployeeEvalTask empEval = db.EmployeeEvalTasks.Find(id);
            List<EmployeeEvalTaskDet> empevaltask = db.EmployeeEvalTaskDets.Where(a => a.EmployeeEvalTask_Id == id).ToList();
            EmployeeEvalViewModel empvm = new EmployeeEvalViewModel();
            empvm.EmployeeEvalTask = empEval;
            empvm.empEvalTaskDetvm = empevaltask;

           // ViewBag.empDetails = empevaltask;
            return View(empvm);
        }
        [HttpPost]
        public ActionResult Delete(int id,EmployeeEvalViewModel empvm)
        {
            EmployeeEvalTask empeval = db.EmployeeEvalTasks.Find(id);
            empeval.IsActive = false;
            db.Entry(empeval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateEmployeeEval(int assignresid)
        {

            List<EmployeeList> emplist = (from assResDet in db.AssignResourcesDets
                                          //join training in db.Trainings
                                          // on assResDet.AssignResources_Id equals training.AssignResource_Id
                                          //join trainingdet in db.TrainingDets
                                          //on training.Id equals trainingdet.Training_Id
                                          join emp in db.Employees
                                          on assResDet.Employee_Id equals emp.Id

                                          where
                                          assResDet.AssignResources_Id==assignresid &&
                                           emp.IsActive == true 
                                          select new EmployeeList
                                          {
                                              Id = emp.Id,
                                              Name = emp.Name

                                          }).ToList();

            ViewBag.empDetails = emplist;
            getTrainers(assignresid);
            return View();
        }
        public void getTrainers(int? assignresid)
        {
            var val = new SelectList(db.Trainers.Where(a=> !db.AssignResources.Any(p2=>p2.Trainer_Id==a.Id && p2.Id== assignresid && a.IsActive==true) ).ToList(), "id", "Name");
            ViewData["trainerslist"] = val;
        }
    }
}