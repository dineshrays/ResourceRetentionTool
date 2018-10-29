
using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

using RetentionTool.ViewModel;
namespace RetentionTool.Areas.Admin.Controllers
{
    public class ModuleController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchDet = new FetchDefaultIds();
        // GET: Module
       public ActionResult Index(int? page)
        {
           
            List<Module> modulelist = db.Modules.Where(m=>m.IsActive==true).ToList();
            List<ModuleDet> modulelist1 = db.ModuleDets.ToList();
            ModuleViewModel moduleviewm = new ModuleViewModel();

            moduleviewm.modulevm = (from m in db.Modules
                          join md in db.ModuleDets
                          on m.Id equals md.Module_Id
                          where m.IsActive == true
                          select new ModuleViewModel
                          {
                              Id = m.Id,
                              ModuleName = m.ModuleName,
                              CommonFieldName = m.Commonfield.Name,
                              SkillName = m.Skill.Name,
                              Date = m.Date,
                              IsActive = m.IsActive,
                              Commonfield_Id = m.Commonfield_Id,
                              Skill_Id = m.Skill_Id,
                              ModuleDetId = md.Id,
                              Module_Id = md.Module_Id,
                              Topics = md.Topics,
                              HoursRequired = md.HoursRequired,

                          }).ToList();


            int pageSize = fetchDet.pageSize;
            int pageIndex = fetchDet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ModuleViewModel> modulepaged = null;
            modulepaged = moduleviewm.modulevm.ToPagedList(pageIndex, pageSize);

            return View(modulepaged) ;
        }
      

        public IEnumerable<SelectListItem> getCommonFields()
        {
            var val = db.Commonfields.Where(a=>a.IsActive==true).ToList();
            var cf = new SelectList(val, "id","Name");
            return cf;

        }

        public ActionResult Create()
        {
            ModuleViewModel mv = new ModuleViewModel();
            mv.CommonField = getCommonFields();
            mv.Skill = getSkill();
            return View(mv);
        }

        public IEnumerable<SelectListItem> getSkill()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem{Value=null,Text=""}
            };
            return list;

        }
        [HttpPost]
        public ActionResult Create(Item[] itemlist,ModuleViewModel mvm)
            {

        
                Module m = new Module();
                ModuleDet m1 = new ModuleDet();
                m.ModuleName = mvm.ModuleName;
                m.Commonfield_Id = fetchDet.getDefaultPrimarySkillId();
                //mvm.SelectedCommonFields;
                m.Skill_Id = mvm.SelectedSkills;
                m.Date = mvm.Date;
                m.IsActive = true;

                db.Modules.Add(m);
                db.SaveChanges();

                foreach (var i in itemlist)
                {
                    m1.Module_Id = m.Id;
                    m1.Topics = i.Topics;
                    m1.HoursRequired = i.HoursRequired;
                    db.ModuleDets.Add(m1);
                    db.SaveChanges();
                }

                return Json("", JsonRequestBehavior.AllowGet);
         
            
        }

       

        public ActionResult getSkills(int skillId)
        {
            IEnumerable<SelectListItem> skilldet = getSkillsField(skillId);
            return Json(skilldet,JsonRequestBehavior.AllowGet);
            
        }

        public IEnumerable<SelectListItem> getSkillsField(int commonId)
        {
            IEnumerable<SelectListItem> rs = db.Skills.Where(s => s.CommonField_Id == commonId && s.IsActive==true).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Name
            }).ToList();

            return new SelectList(rs, "Value", "Text");
        }

        public ActionResult Edit(int id)
        {
            Module mod = db.Modules.Find(id);
           List<ModuleDet> mod2 = db.ModuleDets.Where(m => m.Module_Id == id).ToList();
                      

            ModuleViewModel1 obj = new ModuleViewModel1();

            obj.module = new ModuleViewModel();
            obj.module.Id = mod.Id;
            obj.module.ModuleName = mod.ModuleName;
            
            obj.module.CommonField = getCommonFields();
            obj.module.Skill = getSkillsField(Convert.ToInt32(mod.Commonfield_Id));
            obj.module.SelectedCommonFields = mod.Commonfield_Id;
            obj.module.SelectedSkills = mod.Skill_Id;
            obj.module.Date = mod.Date;
            obj.module.IsActive = mod.IsActive;
            
            ViewBag.ModuleDetails = mod2;

            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(int id,ModuleViewModel mvm, Item[] itemlist)
        {
           
                var x = (from y in db.ModuleDets
                         where y.Module_Id == id
                         select y);
                foreach (var item in x)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                db.SaveChanges();

                Module m = new Module()
                {
                    Id = id,
                    ModuleName = mvm.ModuleName,
                    IsActive = true,
                    Commonfield_Id = fetchDet.getDefaultPrimarySkillId(),
                    ///mvm.SelectedCommonFields,
                    Skill_Id = mvm.SelectedSkills,
                    Date = mvm.Date
                };
                // db.Modules.Add(m);
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ModuleDet m1 = new ModuleDet();

                foreach (var i in itemlist)
                {
                    m1.Module_Id = id;
                    m1.Topics = i.Topics;
                    m1.HoursRequired = i.HoursRequired;
                    db.ModuleDets.Add(m1);
                    db.SaveChanges();
                }
                return Json("", JsonRequestBehavior.AllowGet);
           

        }

        public ActionResult Delete(int id)
        {
            Module mod = db.Modules.Find(id);
             ModuleDet mod1 = db.ModuleDets.FirstOrDefault(m=>m.Module_Id==id);
            ModuleViewModel mvm = new ModuleViewModel()
            {
                Id=mod.Id,
                ModuleName=mod.ModuleName,
                IsActive=mod.IsActive,
                CommonFieldName=mod.Commonfield.Name,
                SkillName=mod.Skill.Name,
                Date =mod.Date,
                Topics=mod1.Topics,
                HoursRequired=mod1.HoursRequired
            }; List<ModuleDet> mod2 = db.ModuleDets.Where(m => m.Module_Id == id).ToList();
            ViewBag.ModuleDetails = mod2;
            return View(mvm);
        }

       
        [HttpPost]
        public ActionResult Delete(int id, ModuleViewModel mvm)
        {
            Module m = db.Modules.Find(id);
            if(m.Id==id)
            {
                m.IsActive = false;
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CheckIfNameExists(string name,int skillid)
        {
            Module mod = db.Modules.FirstOrDefault(a => a.ModuleName == name && a.Skill_Id == skillid && a.IsActive == true);
            if (mod == null)
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