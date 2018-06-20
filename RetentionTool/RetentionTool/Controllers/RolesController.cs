using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
namespace RetentionTool.Controllers
{
    public class RolesController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Roles
        public ActionResult Index()
        {
            List<Role> role = db.Roles.Where(a => a.IsActive == true).ToList();
            RolesViewModel roleviewmodel = new RolesViewModel();
            roleviewmodel.rolevm = role;
            return View(roleviewmodel);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RolesViewModel rolevm)
        {
            Role role = new Role();
            role = rolevm.role;
            role.IsActive = true;
            role.EntryDate = DateTime.Now;
            db.Roles.Add(role);
            db.SaveChanges();
           

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Role role = db.Roles.Find(id);
            RolesViewModel rolevm = new RolesViewModel();
            rolevm.role = role;
            return View(rolevm);
        }
        [HttpPost]
        public ActionResult Edit(int id,RolesViewModel rolevm)
        {
            Role role = new Role();
            if(id==rolevm.role.Id)
            {
                role = rolevm.role;
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Role role = db.Roles.Find(id);
            RolesViewModel rolevm = new RolesViewModel();
            rolevm.role = role;
            return View(rolevm);
        }
        [HttpPost]
        public ActionResult Delete(int id, RolesViewModel rolevm)
        {
            Role role = db.Roles.Find(id);
            if(role.Id==id)
            {
                role.IsActive = false;
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}