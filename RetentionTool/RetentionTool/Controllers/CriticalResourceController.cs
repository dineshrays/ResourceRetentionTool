using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class CriticalResourceController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: CriticalResource
        public ActionResult Index()
        {
            List<ProjectsDetail> projectDet = db.ProjectsDetails.Where(a => db.ProjectsWorkeds.Any(p=>p.Project_Id==a.Id && p.IsActive==true && a.IsActive == true)).ToList();
            ViewBag.prjctDetails = projectDet;

            
            //.Select(a=>a.ProjectName).Distinct();
            //  ProjectWorkedViewModel prjctWrkvm = new ProjectWorkedViewModel();
            //    prjctWrkvm.
            return View();
        }
        public ActionResult Create(int id)
        {
            List<ProjectsWorked> prjctwrklist = db.ProjectsWorkeds.Where(a => a.Project_Id == id).ToList();
            ProjectWorkedViewModel prjctwrkvm = new ProjectWorkedViewModel();
            ProjectsWorked prjctwrk = new ProjectsWorked();
            ProjectsDetail projectDetails = db.ProjectsDetails.Find(id);
            prjctwrkvm.projectname = projectDetails.Name;
            prjctwrkvm.projects = prjctwrk;
            prjctwrkvm.projectvm = prjctwrklist;
         //   List<CriticalResource> criticalRes
                 ViewBag.CriticalRes = db.CriticalResources.Where(a=>a.IsActive==true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            ViewBag.TrainerDet= db.Trainers.Where(a => a.IsActive == true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            return View(prjctwrkvm);
        }
        [HttpPost]
        public ActionResult Create(List<CriticalResource> list,List<Trainer> trainerlist)
        {

            foreach(var criticalRes in list)
            {
                criticalRes.IsActive = true;
                db.CriticalResources.Add(criticalRes);
                db.SaveChanges();
               
                
            }
            if(trainerlist!=null)
            {
                foreach (var trainer in trainerlist)
                {
                    trainer.IsActive = true;
                    db.Trainers.Add(trainer);
                    db.SaveChanges();
                }
            }
          
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id,CriticalResource criticalres)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delet(int id, CriticalResource criticalres)
        {
            return View();
        }
    }
}