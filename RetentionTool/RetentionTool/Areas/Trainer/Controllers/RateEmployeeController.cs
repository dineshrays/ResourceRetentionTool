﻿using RetentionTool.ViewModel;
using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace RetentionTool.Areas.Trainer.Controllers
{
    public class RateEmployeeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchDet = new FetchDefaultIds();
        // GET: RateEmployee
        public ActionResult Index(int? page)
        {
            int td = int.Parse(Session["userid"].ToString());

            List<AssignResource> assignRes = db.AssignResources.Where(a => !db.EmployeeEvalTasks.Any(e=>e.AssignResource_Id==a.Id && e.IsActive==true && 
                                                                        db.EmployeeEvalTaskDets.Any(t=>t.EmployeeEvalTask_Id==e.Id && 
                                                                        t.IsEligiableMark==true && t.IsActive==true)) && a.IsActive == true
                                                                        && (db.Trainings.Any(t => t.AssignResource_Id == a.Id &&
                                                                        t.IsActive == true && db.TrainingDets.Count(d => d.Training_Id == t.Id && d.IsActive==true).Equals(
                                                                        db.Sessions.Count(s => s.TrainingDet.Training_Id == t.Id && s.IsActive == true)))
                                                                        || db.RateEmployeeEligiabilities.Any(b=>b.AssignResources_Id==a.Id && b.IsActive==true))                                                                       
                                                                         && db.Trainers.Any(t=>t.Id ==a.Trainer_Id && t.IsActive==true && t.PersonalInfo_Id==td)
                                                                        ).ToList();

            List<AssignResource> assignresList = (from assignres in assignRes
                                                  join module in db.Modules on assignres.Module_Id equals module.Id
                                                  join moduledet in db.ModuleDets on module.Id equals moduledet.Module_Id
                                                  join training in db.Trainings on assignres.Id equals training.AssignResource_Id                                                 
                                                  join trainingdet in db.TrainingDets on training.Id equals trainingdet.Training_Id
                                                  join session in db.Sessions on trainingdet.Id equals session.TrainingDet_Id
                                                 join trainer in db.Trainers on assignres.Trainer_Id equals trainer.Id
                                                  where assignres.IsActive == true && trainingdet.IsActive == true && session.IsActive == true && trainer.PersonalInfo_Id == td
                                                  && trainer.IsActive==true
                                                  select assignres).ToList();
            
            //ViewBag.assign = assignRes;
           
            ViewBag.RateEmployeeEl = db.RateEmployeeEligiabilities.Select(o => o.AssignResources_Id).Distinct().ToList();
            int pageSize = fetchDet.pageSize;
            int pageIndex = fetchDet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignResource> pagelist = null;
            pagelist = assignRes.ToPagedList(pageIndex, pageSize);
            return View(pagelist);
        }

        public ActionResult Create(int assignresid)
        {
            List<EmployeeList> assign = (from x in db.AssignResourcesDets
                                         join y in db.PersonalInfoes on x.Employee_Id equals y.Id
                                  
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
            RateEmployeeEligiability re = db.RateEmployeeEligiabilities.FirstOrDefault(a=>a.AssignResources_Id==id);


            List<RateEmployeeEligiability> rateEmpEliList = db.RateEmployeeEligiabilities.Where(a => a.AssignResources_Id == id).ToList();
            RateEmployeeViewModel rateviewmodel = new RateEmployeeViewModel();
            rateviewmodel.RateEmployee = rateEmpEliList;
            rateviewmodel.RateEmployeeVm = re;

            return View(rateviewmodel);
        }

        [HttpPost]
        public ActionResult Edit(int id,List<RateEmployeeEligiability> RateEmployeeVm)
        {
            foreach(var rateitem in RateEmployeeVm)
            {
                rateitem.AssignResources_Id = id;
                rateitem.IsActive = true;
                db.Entry(rateitem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            //RateEmployeeEligiability rateempelig = db.RateEmployeeEligiabilities.FirstOrDefault(a => a.AssignResources_Id == id);

            //rateempelig.Grade = ree.Grade;
            //rateempelig.IsEligible = ree.IsEligible;
            ////ree.IsActive = true;

            //    db.Entry(rateempelig).State = System.Data.Entity.EntityState.Modified;

            //db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(int id)
        {
            RateEmployeeEligiability re = db.RateEmployeeEligiabilities.FirstOrDefault(a => a.AssignResources_Id == id);


            List<RateEmployeeEligiability> rateEmpEliList = db.RateEmployeeEligiabilities.Where(a => a.AssignResources_Id == id).ToList();
            RateEmployeeViewModel rateviewmodel = new RateEmployeeViewModel();
            rateviewmodel.RateEmployee = rateEmpEliList;
            rateviewmodel.RateEmployeeVm = re;

            return View(rateviewmodel);
        }

        [HttpPost]
        public ActionResult Delete(int id, RateEmployeeEligiability ree)
        {
            List<RateEmployeeEligiability> rateEmpEliList = db.RateEmployeeEligiabilities.Where(a => a.AssignResources_Id == id).ToList();
foreach(var item in rateEmpEliList)
            {
                item.IsActive = false;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            //RateEmployeeEligiability rateempelig = db.RateEmployeeEligiabilities.Find(id);
            //if(rateempelig.Id== id)
            //{
            //    rateempelig.IsActive = false;

            //    db.Entry(rateempelig).State = System.Data.Entity.EntityState.Modified;

            //    db.SaveChanges();


            //}


            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult EmpDetails(int assId)
        {
            List<EmployeeList> emplist = (from rateemp in db.RateEmployeeEligiabilities
                                          join emp in db.PersonalInfoes
                                          on rateemp.Employee_Id equals emp.Id
                                          where emp.IsActive == true
                                          where rateemp.AssignResources_Id == assId
                                          select new EmployeeList
                                          {
                                              Id = emp.Id,
                                              Name = emp.Name
                                          }).ToList();

            return Json(emplist, JsonRequestBehavior.AllowGet);
        }
    }
}