using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RetentionTool.Controllers
{
    public class TrainerController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Trainer
        public ActionResult Index()
        {
            List<Trainer> emp = db.Trainers.Where(m =>m.IsActive == true).ToList();
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
        public ActionResult Create(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                trainer.IsActive = true;
                db.Trainers.Add(trainer);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(trainer);
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