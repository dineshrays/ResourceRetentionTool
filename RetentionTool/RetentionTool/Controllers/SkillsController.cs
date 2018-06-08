
using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class SkillsController : Controller
    {
        // GET: Skills
        RetentionToolEntities db = new RetentionToolEntities();
        public ActionResult Index()
        {            
            List<Skill> skillList = db.Skills.Where(m=>m.IsActive==true).ToList();

            SkillsViewModel skillvm = new SkillsViewModel();

            List<SkillsViewModel> SkillsvmList = skillList.Select(X => new SkillsViewModel
            {
                id = X.id, 
                CommonFieldName = db.Commonfields.Where(p => p.id == X.CommonField_Id).SingleOrDefault()?.Name,
                SkillName = X.Name
                
            }).ToList();
            return View(SkillsvmList);
        }

        public ActionResult Create()
        {
            var val = new SelectList(db.Commonfields.ToList(), "id", "Name");
            ViewData["CommField"] = val;
            return View();
        }

        public ActionResult Edit(int id)
        {
            Skill s = db.Skills.Find(id);
            SkillsViewModel svm = new SkillsViewModel()
            {
                id = s.id,
                SkillName = s.Name,
               selectedCommmonField=s.CommonField_Id,
               IsActive=s.IsActive
            };
            var val = new SelectList(db.Commonfields.ToList(), "id", "Name");
            ViewData["CommField"] = val;
            return View(svm);
        }
        [HttpPost]
        public ActionResult Create(SkillsViewModel svm)
        {
            Skill s = new Skill();
           // s.id = svm.id;
            s.Name = svm.SkillName;
            s.CommonField_Id = svm.selectedCommmonField;
            s.IsActive = true;
            db.Skills.Add(s);
            
            db.SaveChanges();
           // long lid = s.id;
            //var val = new SelectList(db.Commonfields.ToList(), "id", "Name");
            //ViewData["CommField"] = val;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(int id,SkillsViewModel svm)
        {
            Skill s = new Skill();
            s.id = svm.id;
            s.Name = svm.SkillName;
            s.CommonField_Id = svm.selectedCommmonField;
            s.IsActive = true;
            if(id==s.id)
            {
                db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(svm);

            
        }
        public ActionResult Delete(int id)
        {
            Module module = db.Modules.FirstOrDefault(m => m.Skill_Id == id);
            Skill s = db.Skills.Find(id);
            string skillname = db.Commonfields.Where(p => p.id == s.CommonField_Id).SingleOrDefault()?.Name;
            //(from d in db.Commonfields where d.id == id select d.Name).ToString();

            //  db.Commonfields.Find(id);
            SkillsViewModel svm = new SkillsViewModel()
            {
                id = s.id,
                SkillName = s.Name,
                CommonFieldName = skillname,
                IsActive = true
            };
            if (module == null)
            {
                ViewData["Message"] = null;
               
            }
            else
            {
                ViewData["Message"] = "Cannot be Deleted";
            }
            return View(svm);
        }
        [HttpPost]
        public ActionResult Delete(int id, SkillsViewModel svm)
        {
            Skill s = db.Skills.Find(id);
           
            if (id == s.id)
            {
                s.IsActive = false;
                db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(svm);            
        }         
    }
}