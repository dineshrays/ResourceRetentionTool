using RetentionTool.ViewModel;
using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class RateEmployeeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: RateEmployee
        public ActionResult Index()
        {
            List<AssignResource> assignRes = db.AssignResources.Where(a => a.IsActive == true).ToList();
            ViewBag.assign = assignRes;

            ViewBag.RateEmployeeEl = db.RateEmployeeEligiabilities.Select(o => o.AssignResources_Id).Distinct().ToList();
            
            return View();
        }

        public ActionResult Create(int assignresid)
        {
            List<EmployeeList> assign = (from x in db.AssignResourcesDets
                                         join y in db.Employees on x.Employee_Id equals y.Id
                                  
                                             where x.AssignResources_Id == assignresid
                                             select new EmployeeList
                                             {
                                                Id=y.Id,
                                                Name=y.Name
                                             }).ToList();
            ViewBag.assign = assign;
            return View();
        }
        
        [HttpPost]
        public ActionResult Create( RateEmployeeEligiability[] RateEmployeeVm)
        {
            
            foreach (var i in RateEmployeeVm)
            {
                RateEmployeeEligiability re = new RateEmployeeEligiability()
                {
                    AssignResources_Id = i.AssignResources_Id,
                    Employee_Id = i.Employee_Id,
                    Grade = i.Grade,
                    IsEligible = i.IsEligible,
                    IsActive = true
                };
                db.RateEmployeeEligiabilities.Add(re);
                db.SaveChanges();               
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit( int id)
        {
            RateEmployeeEligiability re = db.RateEmployeeEligiabilities.Find(id);

            
            return View(re);
        }

        [HttpPost]
        public ActionResult Edit(int id,RateEmployeeEligiability ree)
        {
            
                ree.IsActive = true;
               
                db.Entry(ree).State = System.Data.Entity.EntityState.Modified;
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            RateEmployeeEligiability re = db.RateEmployeeEligiabilities.Find(id);


            return View(re);
        }

        [HttpPost]
        public ActionResult Delete(int id, RateEmployeeEligiability ree)
        {
            RateEmployeeEligiability rateempelig = db.RateEmployeeEligiabilities.Find(id);
            if(rateempelig.Id== id)
            {
                rateempelig.IsActive = false;

                db.Entry(rateempelig).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();


            }


            return RedirectToAction("Index");
        }
    }
}