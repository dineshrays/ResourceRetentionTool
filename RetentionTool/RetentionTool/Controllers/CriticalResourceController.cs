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
            ViewBag.CriticalResPrjct = db.CriticalResources.Where(a => a.IsActive == true).Select(a=>a.Project_Id).Distinct().ToList();

            
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
            prjctwrk.Project_Id = projectDetails.Id;
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
            List<ProjectsWorked> prjctwrklist = db.ProjectsWorkeds.Where(a => a.Project_Id == id).ToList();
            ProjectWorkedViewModel prjctwrkvm = new ProjectWorkedViewModel();
            ProjectsWorked prjctwrk = new ProjectsWorked();
            ProjectsDetail projectDetails = db.ProjectsDetails.Find(id);
            prjctwrkvm.projectname = projectDetails.Name;
            prjctwrkvm.projects = prjctwrk;
            prjctwrkvm.projectvm = prjctwrklist;
            ViewBag.CriticalRes = db.CriticalResources.Where(a => a.IsActive == true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            ViewBag.TrainerDet = db.Trainers.Where(a => a.IsActive == true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            return View(prjctwrkvm);
          
        }

        [HttpPost]
        public ActionResult Edit(int id, List<CriticalResource> criticallist, List<Trainer> trainerlist)
        {
            List<CriticalResource> personalIdlist = db.CriticalResources.Where(a => a.Project_Id == id && a.IsActive==true).ToList();

            //var criticalResFalseList = personalIdlist.Except(criticallist).ToList();
            //var criticalResAddList = criticallist.Except(personalIdlist).ToList();

            var criticalResFalseList = personalIdlist.Where(x => !criticallist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
            var criticalResAddList = criticallist.Where(x => !personalIdlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
            foreach (var personalid in criticalResFalseList)
            {
                personalid.IsActive = false;
                db.Entry(personalid).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                
            }

            foreach (var personalinfoid in criticalResAddList)
            {
                personalinfoid.IsActive = true;
                personalinfoid.Project_Id = id;
                db.CriticalResources.Add(personalinfoid);     
                db.SaveChanges();

            }
            if(trainerlist!=null)
            {
                List<Trainer> traineroriginlist = db.Trainers.Where(a=> db.ProjectsWorkeds.Any(p=> p.Project_Id==id  && p.IsActive==true && a.IsActive == true)).ToList();

                var trainerFalseList = traineroriginlist.Where(x => !trainerlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
                var trainerResAddList = trainerlist.Where(x => !traineroriginlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
                foreach (var personalid in trainerFalseList)
                {
                    personalid.IsActive = false;
                    db.Entry(personalid).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

                foreach (var personalinfoid in trainerResAddList)
                {
                    personalinfoid.IsActive = true;
                   
                    db.Trainers.Add(personalinfoid);
                    db.SaveChanges();

                }

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            List<ProjectsWorked> prjctwrklist = db.ProjectsWorkeds.Where(a => a.Project_Id == id).ToList();
            ProjectWorkedViewModel prjctwrkvm = new ProjectWorkedViewModel();
            ProjectsWorked prjctwrk = new ProjectsWorked();
            ProjectsDetail projectDetails = db.ProjectsDetails.Find(id);
            prjctwrkvm.projectname = projectDetails.Name;
            prjctwrkvm.projects = prjctwrk;
            prjctwrkvm.projectvm = prjctwrklist;
            ViewBag.CriticalRes = db.CriticalResources.Where(a => a.IsActive == true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            ViewBag.TrainerDet = db.Trainers.Where(a => a.IsActive == true).ToList().Select(a => a.PersonalInfo_Id).Distinct();
            return View(prjctwrkvm);
            
        }

        [HttpPost]
        public ActionResult Delete(int id, CriticalResource criticalres)
        {
            List<CriticalResource> criticalRes = db.CriticalResources.Where(a => a.Project_Id == id).ToList();
            foreach(var criticallist in criticalRes)
            {
                criticallist.IsActive = false;
                db.Entry(criticallist).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}