﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
using PagedList;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class EmployeeEvaTasklController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchDet = new FetchDefaultIds();
        // GET: EmployeeEvaTaskl
        public ActionResult Index(int? page)
        {
            List<AssignResource> assignResources = db.AssignResources.Where(a => 
            db.AssignEvaluaters.Any(p2 => p2.AssignResource_Id == a.Id && p2.IsActive == true && a.IsActive == true)
         ||   db.EmployeeEvalTasks.Any(
                e => e.AssignResource_Id == a.Id && e.IsActive == true &&
                db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
                t.IsEligiableMark == true && t.IsActive == true)) 
            ).ToList();
            ViewBag.assResDetails = assignResources;

            List<AssignEvaluater> assignEval = db.AssignEvaluaters.Where(a => a.IsActive == true).ToList();
            ViewBag.asseval = assignEval;
            List<EmployeeEvalTask> empevaltask = db.EmployeeEvalTasks.Where(a => a.IsActive == true).Distinct().ToList();
            ViewBag.empevalDetails = empevaltask;
            int pageSize = fetchDet.pageSize;
            int pageIndex = fetchDet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignResource> modulepaged = null;
            modulepaged = assignResources.ToPagedList(pageIndex, pageSize);
            return View(modulepaged); 
        }
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeEvalTask empeval, EmployeeEvalTaskDet[] list)
        {
            empeval.IsActive = true;
            db.EmployeeEvalTasks.Add(empeval);
            db.SaveChanges();
            AssignResource assignRes = db.AssignResources.Find(empeval.AssignResource_Id);
            CriticalResource critcal = db.CriticalResources.FirstOrDefault(a => a.Project_Id == assignRes.Project_Id && a.IsActive == true);
            critcal.IsActive = false;
           
           List<RetentionTool.Models.Trainer> trainer = db.Trainers.Where(a=>a.CriticalResource_Id==critcal.Id && a.IsActive==true).ToList();
            foreach( var tra in trainer)
            {
                tra.IsActive = false;
                db.Entry(tra).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
           
            db.Entry(critcal).State = System.Data.Entity.EntityState.Modified;
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
            //Trainer t = db.Trainers.Find(empeval.Trainer_Id);
            //t.IsActive = false;
            //db.Entry(t).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            
            return Json("", JsonRequestBehavior.AllowGet);
        
        }
        public ActionResult Edit(int id)
        {
            EmployeeEvalTask empEval = db.EmployeeEvalTasks.FirstOrDefault(a=>a.AssignResource_Id== id);
            List<EmployeeEvalTaskDet> empevaltask = db.EmployeeEvalTaskDets.Where(a => a.EmployeeEvalTask_Id == empEval.Id ).ToList();

            //EmployeeEvalTask empvm = new EmployeeEvalTask();
            //empvm.Trainer_Id = empEval.Trainer_Id;
            EmployeeEvalViewModel empviewmodel = new EmployeeEvalViewModel();
            empviewmodel.EmployeeEvalTask = empEval;
            empviewmodel.empEvalTaskDetvm = empevaltask;

         //   ViewBag.empDetails = empevaltask;
         // getTrainers(empEval.AssignResource_Id);

            return View(empviewmodel);
        }
        [HttpPost]
        public ActionResult Edit(int id, EmployeeEvalTask empeval, EmployeeEvalTaskDet[] list)
        {
            EmployeeEvalTask empevaltask = db.EmployeeEvalTasks.Find(id);
          
           
            empeval.IsActive = true;
            db.Entry(empeval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            foreach (var i in list)
            {
                i.EmployeeEvalTask_Id = empeval.Id;
                i.IsActive = true;
                db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                
            }
            //Trainer t = db.Trainers.Find(empeval.Trainer_Id);
            //t.IsActive = false;
            //db.Entry(t).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            EmployeeEvalTask empEval = db.EmployeeEvalTasks.FirstOrDefault(a => a.AssignResource_Id == id);
        
            List<EmployeeEvalTaskDet> empevaltask = db.EmployeeEvalTaskDets.Where(a => a.EmployeeEvalTask_Id == empEval.Id).ToList();
                       
            EmployeeEvalViewModel empvm = new EmployeeEvalViewModel();
            empvm.EmployeeEvalTask = empEval;
            empvm.empEvalTaskDetvm = empevaltask;

           // ViewBag.empDetails = empevaltask;
            return View(empvm);
        }
        [HttpPost]
        public ActionResult Delete(int id,EmployeeEvalViewModel empvm)
        {
            EmployeeEvalTask empeval = db.EmployeeEvalTasks.FirstOrDefault(a => a.AssignResource_Id == id);
            empeval.IsActive = false;
            AssignResource assignRes = db.AssignResources.Find(empeval.AssignResource_Id);
            CriticalResource critcal = db.CriticalResources.FirstOrDefault(a=> a.Project_Id == assignRes.Project_Id);
            critcal.IsActive = true;
            List<RetentionTool.Models.Trainer> trainer = db.Trainers.Where(a => a.CriticalResource_Id == critcal.Id && a.IsActive == true).ToList();
            foreach (var tra in trainer)
            {
                tra.IsActive = true;
                db.Entry(tra).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //Trainer trainer = db.Trainers.FirstOrDefault(a => a.CriticalResource_Id == critcal.Id );
            //trainer.IsActive = true;
            //db.Entry(trainer).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            db.Entry(critcal).State = System.Data.Entity.EntityState.Modified;
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
                                          join emp in db.PersonalInfoes
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
          
            AssignEvaluater ass = db.AssignEvaluaters.FirstOrDefault(a => a.AssignResource_Id == assignresid && a.IsActive == true);
            EmployeeEvalViewModel empvm = new EmployeeEvalViewModel();
            EmployeeEvalTask emp1 = new EmployeeEvalTask();
           
            emp1.Trainer = ass.Trainer;

            emp1.Trainer_Id = ass.Trainer_Id;
            emp1.Trainer.PersonalInfo = ass.Trainer.PersonalInfo;
            empvm.EmployeeEvalTask = emp1;

            return View(empvm);
        }
        public void getTrainers(int? assignresid)
        {
            AssignEvaluater ass = db.AssignEvaluaters.FirstOrDefault(a => a.AssignResource_Id == assignresid && a.IsActive == true);
            EmployeeEvalViewModel empvm = new EmployeeEvalViewModel();
            EmployeeEvalTask emp1 = new EmployeeEvalTask();

            emp1.Trainer = ass.Trainer;

            emp1.Trainer_Id = ass.Trainer_Id;
            emp1.Trainer.PersonalInfo = ass.Trainer.PersonalInfo;
            empvm.EmployeeEvalTask = emp1;

            ViewData["trainerslist"] = ass;
        }
    }
}