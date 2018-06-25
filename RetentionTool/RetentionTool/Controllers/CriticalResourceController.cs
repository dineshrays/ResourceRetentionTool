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
            ViewBag.prjctDetails = db.ProjectsDetails.Where(a => a.IsActive == true).ToList();
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
         //   prjctwrk.ProjectsDetail.Name = ;
            prjctwrkvm.projects = prjctwrk;
            prjctwrkvm.projectvm = prjctwrklist;
            return View(prjctwrkvm);
        }
        [HttpPost]
        public ActionResult Create(List<CriticalResource> list)
        {

            foreach(var criticalRes in list)
            {
                criticalRes.IsActive = true;
                db.CriticalResources.Add(criticalRes);
                db.SaveChanges();
                
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}