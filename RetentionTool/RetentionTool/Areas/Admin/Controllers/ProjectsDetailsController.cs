using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using PagedList;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class ProjectsDetailsController : Controller
    {
        private RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchDet = new FetchDefaultIds();
      
        public ActionResult Index(int? page)
        {
            List<ProjectsDetail> projectDet = db.ProjectsDetails.Where(a=>a.IsActive==true).ToList();
            int pageSize = fetchDet.pageSize;
            int pageIndex = fetchDet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ProjectsDetail> modulepaged = null;
            modulepaged = projectDet.ToPagedList(pageIndex, pageSize);
            return View(modulepaged);
        }

       
       
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        public ActionResult Create(ProjectsDetail projectsDetail)
        {
            if (ModelState.IsValid && projectsDetail.Name!=null)
            {
                projectsDetail.IsActive = true;
                db.ProjectsDetails.Add(projectsDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectsDetail);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectsDetail projectsDetail = db.ProjectsDetails.Find(id);
            if (projectsDetail == null)
            {
                return HttpNotFound();
            }
            return View(projectsDetail);
        }

        // POST: ProjectsDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,IsActive")] ProjectsDetail projectsDetail)
        {
            if (ModelState.IsValid)
            {
                projectsDetail.IsActive = true;
                db.Entry(projectsDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectsDetail);
        }

        // GET: ProjectsDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectsDetail projectsDetail = db.ProjectsDetails.Find(id);
            if (projectsDetail == null)
            {
                return HttpNotFound();
            }
            return View(projectsDetail);
        }

     
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectsDetail projectsDetail = db.ProjectsDetails.Find(id);
            projectsDetail.IsActive = false;
            db.Entry(projectsDetail).State = EntityState.Modified;
           // db.ProjectsDetails.Remove(projectsDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult CheckIfNameExists(string projectname)
        {
            ProjectsDetail projectdet = db.ProjectsDetails.FirstOrDefault(a => a.Name == projectname && a.IsActive == true);
            if(projectdet==null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditCheckIfNameExists(string projectname,int projectid)
        {
            ProjectsDetail projectdet = db.ProjectsDetails.FirstOrDefault(a => a.Name == projectname && a.IsActive == true
            && a.Id != projectid);
            if (projectdet == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
