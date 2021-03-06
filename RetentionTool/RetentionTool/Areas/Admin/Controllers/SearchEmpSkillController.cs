﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;


namespace RetentionTool.Areas.Admin.Controllers
{
    public class SearchEmpSkillController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: SearchEmpSkill
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getEmployeeDetails(int id)
        {
            int emproleid = fetchdet.getDefaultEmployeeRoleId();

           
            List<EmployeeList> employeeList = (from personal in db.PersonalInfoes
                                               join empskills in db.EmployeeSkills on personal.Id equals empskills.P_Id
                                               join skill in db.Skills on empskills.Skills_Id equals skill.id
                                              join user in db.UserDetails on personal.Id equals user.Emp_Id
                                               where skill.id==id && user.IsActive==true && user.Role_Id== emproleid
                                               select new EmployeeList
                                               {
                                                   Id = personal.Id,
                                                   Name = personal.Name,
                                                   EmpCode = personal.EmpCode

                                               }
                                               ).ToList();
            
            return Json(employeeList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getSkillName(string name)
        {
           var va = db.Skills.Where(a => a.Name.Contains(name) && a.IsActive == true).ToList();
            List<SkillsViewModel> skilllist = new List<SkillsViewModel>() ;
            foreach(var value in va)
            {
                SkillsViewModel skill = new SkillsViewModel();
                skill.id = value.id;
                skill.SkillName = value.Name;
                skilllist.Add(skill);


            }
            //         var va = (from skill in db.Skills


            //                           where skill.Name.Contains(name)

            //&& skill.IsActive == true
            //                           select new Skill
            //                           {
            //                               id = skill.id,
            //                               Name = skill.Name

            //                           }).ToList();
            return new JsonResult { Data = skilllist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

         //   return Json(va, JsonRequestBehavior.AllowGet);
        }

    }
}