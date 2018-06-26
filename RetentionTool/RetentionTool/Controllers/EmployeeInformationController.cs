using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class EmployeeInformationController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInformation
        public ActionResult Index()
        {
            List<PersonalInfoModel> details = (from a in db.PersonalInfoes
                                                          select new PersonalInfoModel
                                                          {
                                                              EmpCode =a.EmpCode,
                                                              Name = a.Name
                                                         }).ToList();
            ViewBag.details = details;
            return View();
        }

        public IEnumerable<SelectListItem> getCommonFields()
        {
            var val = db.Commonfields.ToList();
            IEnumerable<SelectListItem> cf = new SelectList(val, "id", "Name");
            return cf;

        }
        public IEnumerable<SelectListItem> getSkill()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem{Value=null,Text=""}
            };
            return list;

        }
        public ActionResult getSkills(int skillId)
        {
            IEnumerable<SelectListItem> skilldet = getSkillsField(skillId);
            return Json(skilldet,JsonRequestBehavior.AllowGet);
            
        }

        public IEnumerable<SelectListItem> getSkillsField(int commonId)
        {
            IEnumerable<SelectListItem> rs = db.Skills.Where(s => s.CommonField_Id == commonId).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Name
            }).ToList();

            return new SelectList(rs, "Value", "Text");
        }

        public ActionResult Create()
        {
            EmployeeInformationViewModel ei = new EmployeeInformationViewModel();
            
            ei.CommonField = getCommonFields();
                
            ei.Skills = getSkill();
            return View(ei);
            
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}