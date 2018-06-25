using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;

namespace RetentionTool.Controllers
{
    public class ProjectsDetailsController : Controller
    {
        private RetentionToolEntities db = new RetentionToolEntities();

        // GET: ProjectsDetails
        public ActionResult Index()
        {
            return View(db.ProjectsDetails.ToList());
        }

       
        // GET: ProjectsDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectsDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ProjectsDetail projectsDetail)
        {
            if (ModelState.IsValid)
            {
                projectsDetail.IsActive = true;
                db.ProjectsDetails.Add(projectsDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectsDetail);
        }

        // GET: ProjectsDetails/Edit/5
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
        [ValidateAntiForgeryToken]
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

        // POST: ProjectsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
    }
}
