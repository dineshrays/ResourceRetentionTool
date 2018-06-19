using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class TrainingController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Training
        public ActionResult Index()
        {


            List<AssignResourceViewModel> assgnvm = (from assignres in db.AssignResources
                                                     join m in db.Modules
                                                     on assignres.Module_Id equals m.Id
                                                     where assignres.IsActive == true
                                                     select new AssignResourceViewModel
                                                     {
                                                         Id = assignres.Id,                                                         
                                                         Module_Id = assignres.Module_Id
                                                         
                                                     }).ToList();

            ViewBag.details = assgnvm;

            

            return View();
        }
        [HttpPost]
        public ActionResult EmpDetails(int assId)
        {
            List<EmployeeList> emplist = (from assresdet in db.AssignResourcesDets
                                      join emp in db.Employees
                                      on assresdet.Employee_Id equals emp.Id
                                      where emp.IsActive == true
                                      where assresdet.AssignResources_Id == assId
                                      select new EmployeeList
                                      {
                                          Id = emp.Id,
                                          Name = emp.Name
                                      }).ToList();

            return Json (emplist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int moduleid,int assignresid)
        {
            
            List<ModuleViewModel> Topics = (

                from assin in db.AssignResources
                join mod in db.ModuleDets on assin.Module_Id equals mod.Module_Id
                
                    where assin.Module_Id==moduleid
                    select new ModuleViewModel
                    {
                        Id=mod.Id,
                        Topics=mod.Topics
                    }).ToList();

            ViewBag.Topics = Topics;
            return View();
        }

        [HttpPost]
        public ActionResult Createtraining( Item[] itemlist, Training TrainingVM)
        {
            Training t = new Training();
            TrainingDet td = new TrainingDet();

            t.AssignResource_Id = TrainingVM.AssignResource_Id;
            t.FromDate = TrainingVM.FromDate;
            t.ToDate = TrainingVM.ToDate;
            t.IsActive = true;
            db.Trainings.Add(t);
            db.SaveChanges();
            
            foreach (var i in itemlist)
            {
                td.Training_Id = t.Id;
                td.ModuleDet_Id=Convert.ToInt32(i.Topics);
                td.HoursRequired = i.HoursRequired;
                td.IsActive = true;
                db.TrainingDets.Add(td);
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            Training tr = db.Trainings.Find(id);
            //List<TrainingDet> tdetails = db.TrainingDets.Where(t => t.Training_Id == id).ToList();
            List<Item> tdetails = (from trainingdet in db.TrainingDets
                                   join moduledet in db.ModuleDets on
                                   trainingdet.ModuleDet_Id equals moduledet.Id
                                   where trainingdet.Training_Id == id
                                   select new Item
                                   {
                                       Id = moduledet.Id,
                                       Topics=moduledet.Topics,
                                       HoursRequired=trainingdet.HoursRequired
                                   }
                               ).ToList();
            try
            {
                TrainingViewModel obj = new TrainingViewModel();

                obj.TrainingVm = new Training();

                obj.TrainingVm.Id = tr.Id;
                obj.TrainingVm.AssignResource_Id = tr.AssignResource_Id;
                obj.TrainingVm.FromDate = tr.FromDate;
                obj.TrainingVm.ToDate = tr.ToDate;

                ViewBag.details = tdetails;
                return View(obj);
            }
            catch (Exception ex)
            {
                return View();            
            }        
        }               

        [HttpPost]
        public ActionResult Edit(int id, Training tvm, Item[] itemlist)
        {
            var x = (from y in db.TrainingDets
                     where y.Training_Id == id
                     select y);
            foreach (var item in x)
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            tvm.IsActive = true;
            db.Entry(tvm).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TrainingDet td = new TrainingDet();
            foreach (var i in itemlist)
            {
                td.Training_Id = id;
                td.ModuleDet_Id = i.Id;
                td.HoursRequired = i.HoursRequired;
                td.IsActive = true;
                db.TrainingDets.Add(td);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult delete(int id)
        {
            Training tr = db.Trainings.Find(id);
            //List<TrainingDet> tdetails = db.TrainingDets.Where(t => t.Training_Id == id).ToList();
            List<Item> tdetails = (from trainingdet in db.TrainingDets
                                   join moduledet in db.ModuleDets on
                                   trainingdet.ModuleDet_Id equals moduledet.Id
                                   where trainingdet.Training_Id == id
                                   select new Item
                                   {
                                       Id = moduledet.Id,
                                       Topics = moduledet.Topics,
                                       HoursRequired = trainingdet.HoursRequired

                                   }
                               ).ToList();


            TrainingViewModel obj = new TrainingViewModel();

            obj.TrainingVm = new Training();

            obj.TrainingVm.Id = tr.Id;
            obj.TrainingVm.AssignResource_Id = tr.AssignResource_Id;
            obj.TrainingVm.FromDate = tr.FromDate;
            obj.TrainingVm.ToDate = tr.ToDate;

            ViewBag.details = tdetails;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(int id,TrainingViewModel tvm)
        {
            Training m = db.Trainings.Find(id);
            if (m.Id == id)
            {
                m.IsActive = false;
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}