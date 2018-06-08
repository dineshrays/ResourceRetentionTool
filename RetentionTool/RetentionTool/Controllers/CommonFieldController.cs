using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;

namespace RetentionTool.Controllers
{
    public class CommonFieldController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: CommanField
        public ActionResult Index()
        {
            
            List<Commonfield> commfieldlist = db.Commonfields.Where(m=>m.IsActive==true).ToList();
            List<CommonFieldViewModel> commfieldvm = commfieldlist.Select(
                C => new CommonFieldViewModel
                {
                    id = C.id,
                    Name=C.Name,
                   // IsActive=C.IsActive

                }
                ).ToList();
           
            return View(commfieldvm);
        }

        

        // GET: CommanField/Create
        public ActionResult Create()
        {
            //var val = new SelectList(db.Commonfields.ToList(), "id", "Name");
            //ViewData["commnd"] = val;
            return View();
        }

        // POST: CommanField/Create
        [HttpPost]
        public ActionResult Create(CommonFieldViewModel commfieldvm)
        {
            try
            {
                int selectedval = commfieldvm.SelectedVal;
                Commonfield comm = new Commonfield();
                comm.id = commfieldvm.id;
                comm.Name = commfieldvm.Name;
                comm.IsActive = true;
                
                
                // TODO: Add insert logic here
               if(ModelState.IsValid)
                {
                    db.Commonfields.Add(comm);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(commfieldvm);
            }
            catch
            {
                return View();
            }
        }

        // GET: CommanField/Edit/5
        public ActionResult Edit(int id)
        {
            Commonfield commfield = db.Commonfields.Find(id);
            CommonFieldViewModel commfieldvm = new CommonFieldViewModel();
            commfieldvm.id = commfield.id;
            commfieldvm.IsActive = commfield.IsActive;
            commfieldvm.Name = commfield.Name;
            return View(commfieldvm);
        }

        // POST: CommanField/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CommonFieldViewModel commfieldvm)
        {
            try
            {
                // TODO: Add update logic here
                Commonfield comm = new Commonfield();
                comm.id = commfieldvm.id;
                comm.Name = commfieldvm.Name;
                comm.IsActive = true;
                db.Entry(comm).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CommanField/Delete/5
        public ActionResult Delete(int id)

        {
            Commonfield commfield = db.Commonfields.Find(id);
            CommonFieldViewModel commfieldvm = new CommonFieldViewModel();
            commfieldvm.id = commfield.id;
            commfieldvm.IsActive = commfield.IsActive;
            commfieldvm.Name = commfield.Name;
            Module module = db.Modules.FirstOrDefault(m => m.Commonfield_Id == id);
            if (module == null)
            {
                ViewData["Message"] = null;
            }
         else
            {
               ViewData["Message"] = "Cannot Delete";
            }
                return View(commfieldvm);
        }

        // POST: CommanField/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CommonFieldViewModel commfieldvm)
        {
            try
            {
                // TODO: Add delete logic here
                
                    Commonfield comm = db.Commonfields.Find(id);
                    comm.IsActive = false;
                    db.Entry(comm).State=System.Data.Entity.EntityState.Modified;
                    //comm.id = commfieldvm.id;
                    //comm.Name = commfieldvm.Name;
                    //comm.IsActive = commfieldvm.IsActive;
                   // db.Commonfields.Remove(comm);
                    db.SaveChanges();
                    ViewBag.Message = "Successfully Deleted";
                    return RedirectToAction("Index");
                //RedirectToAction("Index");



            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
