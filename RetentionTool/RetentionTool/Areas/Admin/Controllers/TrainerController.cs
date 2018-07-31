using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RetentionTool.Areas.Admin.Controllers
{
    public class TrainerController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Trainer
        public ActionResult Index()
        {
            List<RetentionTool.Models.Trainer> emp = db.Trainers.Where(m =>m.IsActive == true).ToList();
            List<TrainerModel> e = emp.Select(x => new TrainerModel()
            {
                Id = x.Id,
                Name = x.PersonalInfo.Name,
                IsActive = x.IsActive
            }).ToList();

            return View(e);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(RetentionTool.Models.Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                trainer.CriticalResource_Id = 18;
                trainer.IsActive = true;
                db.Trainers.Add(trainer);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(trainer);
        }

        public ActionResult Edit(int id)
        {
            
          RetentionTool.Models.  Trainer train = db.Trainers.Find(id);
            TrainerModel trainvm = new TrainerModel();
            trainvm.Id = train.Id;
            trainvm.CriticalResource_Id = train.CriticalResource_Id;
            trainvm.IsActive = train.IsActive;
            trainvm.Name = train.PersonalInfo.Name;
            return View(trainvm);
        }
        
        [HttpPost]
        public ActionResult Edit(int id, TrainerModel trainvm)
        {
            try
            {
                // TODO: Add update logic here
                RetentionTool.Models. Trainer train = new RetentionTool.Models.Trainer();
                train.Id = trainvm.Id;
                train.PersonalInfo_Id = trainvm.PersonalInfo_Id;
                train.CriticalResource_Id = trainvm.CriticalResource_Id;
                train.IsActive = true;// trainvm.IsActive;
                db.Entry(train).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


       
        public ActionResult Delete(int id)

        {
          RetentionTool.Models. Trainer train = db.Trainers.Find(id);
            TrainerModel trainvm = new TrainerModel();
            trainvm.Id = train.Id;
            trainvm.PersonalInfo_Id = train.PersonalInfo_Id;
            trainvm.IsActive = train.IsActive;
            trainvm.CriticalResource_Id = train.CriticalResource_Id;
            
            return View(trainvm);
        }

      
        [HttpPost]
        public ActionResult Delete(int id, TrainerModel trainvm)
        {
            try
            {
              RetentionTool.Models.Trainer train = db.Trainers.Find(id);
                train.IsActive = false;                
                db.Entry(train).State = System.Data.Entity.EntityState.Modified;                
                db.SaveChanges();
                ViewBag.Message = "Successfully Deleted";
                return RedirectToAction("Index");
            
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult getTrainerName(string name)
        {
            List<TrainerModel> va = (from perso in db.PersonalInfoes
                                        join trin in db.Trainers
                                on perso.Id equals trin.PersonalInfo_Id  
                                
                                     
                                     select new TrainerModel
                                     {
                                         Id = perso.Id,
                                         Name = perso.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
    }
}