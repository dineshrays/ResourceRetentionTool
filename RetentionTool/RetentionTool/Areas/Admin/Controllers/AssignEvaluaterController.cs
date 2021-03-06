﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RetentionTool.Models;
using RetentionTool.ViewModel;


namespace RetentionTool.Areas.Admin.Controllers
{
    public class AssignEvaluaterController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchDet = new FetchDefaultIds();
        // GET: AssignEvaluater
        public ActionResult Index(int? page)
        {
            List<AssignEvaluater> assneval = db.AssignEvaluaters.Where(a =>
           !db.EmployeeEvalTasks.Any(
                e => e.AssignResource_Id == a.AssignResource_Id && e.IsActive == true &&
                db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
                t.IsEligiableMark == true && t.IsActive == true)) &&
           
            a.IsActive == true).ToList();
            AssignEvaluterViewModel assnevalvm = new AssignEvaluterViewModel();
            assnevalvm.assvm = assneval;

            int pageSize = fetchDet.pageSize;
            int pageIndex = fetchDet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignEvaluater> pagedlist = null;
            pagedlist = assneval.ToPagedList(pageIndex, pageSize);
                //assnevalvm.assvm.ToPagedList(pageIndex, pageSize);
            //.ToPagedList(pageIndex, pageSize);
            return View(pagedlist);
        }
        public ActionResult Create()
        {
            //List<AssignResource> assignResources = db.AssignResources.Where(a => a.IsActive == true).ToList();
            List<AssignResource> assignResources = db.AssignResources.Where(a => 
            !db.AssignEvaluaters.Any(b => b.AssignResource_Id == a.Id  && b.IsActive==true) &&
                db.RateEmployeeEligiabilities.Any(r => r.AssignResources_Id == a.Id && r.IsActive == true) &&

            a.IsActive==true).ToList();
           
            ViewBag.assResDetails = assignResources;

            return View();
        }
        [HttpPost]
        public ActionResult Create(AssignEvaluater assigneval,bool isemp,int empid)
        {
            //assigneval.IsActive = true;
            //db.AssignEvaluaters.Add(assigneval);
            //db.SaveChanges();
            if(isemp==true)
            {
                AssignResource assRes = db.AssignResources.Find(assigneval.AssignResource_Id);

                CriticalResource criticalRes = db.CriticalResources.FirstOrDefault(a => a.Project_Id == assRes.Project_Id && a.IsActive == true);
             RetentionTool.Models.Trainer trainer = new Models.Trainer();
                trainer.PersonalInfo_Id = empid;
                trainer.CriticalResource_Id = criticalRes.Id;
                trainer.IsActive = true;
                db.Trainers.Add(trainer);
                db.SaveChanges();
                assigneval.Trainer_Id = trainer.Id;

                FetchDefaultIds fetchdet = new FetchDefaultIds();
                int trainerroleid = fetchdet.getDefaultTrainerRoleId();
                PersonalInfo personalinfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == empid && a.IsActive == true);
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == personalinfo.Id && a.Role_Id == trainerroleid && a.IsActive == true);
                if (userdet == null)
                {
                    UserDetail user = new UserDetail();
                    user.Emp_Id = personalinfo.Id;
                    user.EntryDate = DateTime.Now;
                    string emailid = personalinfo.EmpCode + "@gmail.com";
                    user.Email = emailid;
                        //personalinfo.Email;


                    user.Role_Id = trainerroleid;
                    user.Name = personalinfo.Name;
                    user.IsActive = true;
                    user.Password = fetchdet.password;

                    db.UserDetails.Add(user);
                    db.SaveChanges();

                }
            }
            assigneval.IsActive = true;
            db.AssignEvaluaters.Add(assigneval);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);

        }
        public ActionResult Edit(int id)
        {
            AssignEvaluater empEval = db.AssignEvaluaters.Find(id);
            
            getTrainers(empEval.AssignResource_Id);
            return View(empEval);
        }
        [HttpPost]
        public ActionResult Edit(int id, AssignEvaluater asseval, bool isemp, int empid)
        {
            AssignEvaluater assEv = db.AssignEvaluaters.FirstOrDefault(a => a.AssignResource_Id == id && a.IsActive == true);

            if (isemp == true)
            {
               
              Models.Trainer tr = db.Trainers.Find(assEv.Trainer_Id);
                tr.IsActive = false;
                db.Entry(tr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                AssignResource assRes = db.AssignResources.Find(id);

                CriticalResource criticalRes = db.CriticalResources.FirstOrDefault(a => a.Project_Id == assRes.Project_Id && a.IsActive == true);
             Models.Trainer trainer = new Models.Trainer();
                trainer.PersonalInfo_Id = empid;
                trainer.CriticalResource_Id = criticalRes.Id;
                trainer.IsActive = true;
                db.Trainers.Add(trainer);
                db.SaveChanges();
                asseval.Trainer_Id = trainer.Id;

                FetchDefaultIds fetchdet = new FetchDefaultIds();
                int trainerroleid = fetchdet.getDefaultTrainerRoleId();
                PersonalInfo personalinfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == empid && a.IsActive == true);
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == personalinfo.Id && a.Role_Id == trainerroleid && a.IsActive == true);
                if (userdet == null)
                {
                    UserDetail user = new UserDetail();
                    user.Emp_Id = personalinfo.Id;
                    user.EntryDate = DateTime.Now;
                    string emailid = personalinfo.EmpCode + "@gmail.com";
                    user.Email = emailid;
                        //personalinfo.Email;


                    user.Role_Id = trainerroleid;
                    user.Name = personalinfo.Name;
                    user.IsActive = true;
                    user.Password = fetchdet.password;

                    db.UserDetails.Add(user);
                    db.SaveChanges();

                }
            }

            asseval.IsActive = true;
            asseval.Id = assEv.Id;
            db.Entry(assEv).CurrentValues.SetValues(asseval);
           // db.Entry(asseval).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json("", JsonRequestBehavior.AllowGet);
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
                        }).Distinct().ToList();
            
            var val = new SelectList(data.Where(a => !db.AssignResources.Any(p2 => p2.Trainer_Id == a.Id && p2.Id == assignresid )).ToList(), "id", "Name");
            ViewData["trainerslist"] = val;
        }
    }
}