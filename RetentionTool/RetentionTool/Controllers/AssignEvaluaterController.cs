using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class AssignEvaluaterController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();

        // GET: AssignEvaluater
        public ActionResult Index()
        {
            
            List<AssignEvaluater> assneval = db.AssignEvaluaters.Where(a => a.IsActive == true).ToList();
            AssignEvaluterViewModel assnevalvm = new AssignEvaluterViewModel();
            assnevalvm.assvm = assneval;
            return View(assnevalvm);
        }
        public ActionResult Create()
        {
            List<AssignResource> assignResources = db.AssignResources.Where(a => a.IsActive == true).ToList();
         

            ViewBag.assResDetails = assignResources;

            return View();
        }
        [HttpPost]
        public ActionResult Create(AssignEvaluater assigneval)
        {
            assigneval.IsActive = true;
            db.AssignEvaluaters.Add(assigneval);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int id)
        {
            AssignEvaluater empEval = db.AssignEvaluaters.Find(id);
            
            getTrainers(empEval.AssignResource_Id);
            return View(empEval);
        }
        [HttpPost]
        public ActionResult Edit(int id, AssignEvaluater asseval)
        {
            
            asseval.IsActive = true;
            db.Entry(asseval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            AssignEvaluater empEval = db.AssignEvaluaters.Find(id);

            getTrainers(empEval.AssignResource_Id);
            return View(empEval);
        }
        [HttpPost]
        public ActionResult Delete(int id, AssignEvaluater assgnEva)
        {
            AssignEvaluater assval = db.AssignEvaluaters.Find(id);
            assval.IsActive = false;
            db.Entry(assval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Index");

        }
        public ActionResult CreateAssignEvaluter(int assignresid)
        {

            AssignEvaluater assgneval = new AssignEvaluater();
            assgneval.AssignResource_Id = assignresid;
            getTrainers(assignresid);
            return View(assgneval);
        }
       
        public void getTrainers(int? assignresid)
        {

            var data = (from personalInfo in db.PersonalInfoes
                        join
trainer in db.Trainers on personalInfo.Id equals trainer.PersonalInfo_Id
//join assignres in db.AssignResources on personalInfo.Id 
                        //join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
                        where personalInfo.IsActive == true && trainer.IsActive == true

                        //&& 
                        select new
                        {
                            Id = trainer.Id,
                            Name = personalInfo.Name
                        }).ToList();
            //  var val = new SelectList(db.PersonalInfoes.ToList(), "Id", "Name");
            //ViewData["trainerslist"] = new SelectList(data, "Id", "Name");
            var val = new SelectList(data.Where(a => !db.AssignResources.Any(p2 => p2.Trainer_Id == a.Id && p2.Id == assignresid )).ToList(), "id", "Name");
            ViewData["trainerslist"] = val;
        }
    }
}