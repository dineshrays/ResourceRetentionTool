﻿using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;


namespace RetentionTool.Areas.Manager.Controllers
{
    public class ApproveEmpSkillsController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Manager/ApproveEmpSkills
        public ActionResult Index()
        {
            int managerid = fetchdet.getUserDetailsId();
            List<EmployeeSkillsAdd> emp = db.EmployeeSkillsAdds.Where(a => a.Manager_Id==managerid && a.IsPromoted == false && a.IsActive==true ).ToList();
            EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();

            //EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find();
            //skilladd.EmployeeSkillsAdd = esd;
            skilladd.skiladd = emp;
            return View(skilladd);
        }

        [HttpPost]
        public ActionResult Index(int id,ApproveEmpSkillsModel appl)
        {
            //EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();
            //EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find(id);
            //skilladd.EmployeeSkillsAdd = esd;
            ApproveEmpSkill app = new ApproveEmpSkill();
            
            app.EmpskillAdd_Id = appl.EmpskillAdd_Id;
            app.Emp_Id = appl.Emp_Id;
            app.TaskAssigned = "Manager Approved";
            app.Remark = "Manager Approved";
            app.IsEvaluated = true;
            app.IsActive = true;
            app.CreatedOn = System.DateTime.Now;

            db.ApproveEmpSkills.Add(app);
            db.SaveChanges();

            EmployeeSkillsAdd add = db.EmployeeSkillsAdds.Find(appl.EmpskillAdd_Id);
            add.IsPromoted = true;
            db.Entry(add).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            EmployeeSkill empskill = new EmployeeSkill();
            empskill.P_Id = add.P_Id;
            empskill.Skills_Id = add.Skills_Id;
            empskill.CommonField_Id = add.CommonField_Id;
            empskill.IsActive = true;
            empskill.Months = add.Months;
            empskill.Years = add.Years;
            empskill.Status = add.Status;
            db.EmployeeSkills.Add(empskill);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Evaluates1(int id)
        {
            EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find(id);
            EmployeeSkillsAddViewModel empvm = new EmployeeSkillsAddViewModel();
            empvm.EmployeeSkillsAdd = esd;
            empvm.managerid = fetchdet.getUserDetailsId();
            //  empvm.GetApproveEmpSkills=
            getEvaluaterList(esd.Skills_Id);
            return View(empvm);

        }

        [HttpPost]
        public ActionResult Evaluates1(int id, ApproveEmpSkillsModel appl,int type)
        {
            if(type==2)
            {
                int emproleid = fetchdet.getDefaultEmployeeRoleId();
                PersonalInfo personalinfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == appl.Emp_Id && a.IsActive == true);
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == personalinfo.Id && a.Role_Id == emproleid && a.IsActive == true);
                if (userdet == null)
                {
                    UserDetail user = new UserDetail();
                    user.Emp_Id = personalinfo.Id;
                    user.EntryDate = DateTime.Now;
                    user.Email = personalinfo.Email;


                    user.Role_Id = emproleid;
                    user.Name = personalinfo.Name;
                    user.IsActive = true;
                    user.Password = fetchdet.password;

                    db.UserDetails.Add(user);
                    db.SaveChanges();

                }

            }
            ApproveEmpSkill app = new ApproveEmpSkill();
            //db.ApproveEmpSkills.Find(id);
            app.EmpskillAdd_Id = appl.EmpskillAdd_Id;
            app.Emp_Id = appl.Emp_Id;
            app.TaskAssigned = appl.TaskAssigned;
            app.Remark = appl.Remark;
            app.IsEvaluated = appl.IsEvaluated;
            app.IsActive = true;
            app.CreatedOn = System.DateTime.Now;

            db.ApproveEmpSkills.Add(app);
            db.SaveChanges();
            EmployeeSkillsAdd empskilladd = db.EmployeeSkillsAdds.Find(appl.EmpskillAdd_Id);
            if (appl.IsEvaluated.ToString() == "True")
            {
               

                int employeeroleid = fetchdet.getDefaultEmployeeRoleId();
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == empskilladd.P_Id && a.Role_Id == employeeroleid && a.IsActive == true);

                Notification notif = new Notification();
                notif.User_Id = userdet.Id;
                //  notif.Sessions_Id = sessionVM.Id;
                notif.Message = empskilladd.Skill.Name + fetchdet.ApproveEmployeeMsg;
                //fetchdet.SessionCompletedMsg;
                notif.IsActive = true;
                notif.IsNotified = true;
                notif.CreatedOn = DateTime.Now;

                db.Notifications.Add(notif);
                db.SaveChanges();
            }
            empskilladd.IsPromoted = true;
            if (type==1)
            {
              
                EmployeeSkill empskill = new EmployeeSkill();
                empskill.P_Id = empskilladd.P_Id;
                empskill.Skills_Id = empskilladd.Skills_Id;
                empskill.CommonField_Id = empskilladd.CommonField_Id;
                empskill.IsActive = true;
                empskill.Months = empskilladd.Months;
                empskill.Years = empskilladd.Years;
                empskill.Status = empskilladd.Status;
                db.EmployeeSkills.Add(empskill);
                db.SaveChanges();
               
            }
            db.Entry(empskilladd).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //empskilladd.IsPromoted = true;

            //EmployeeSkill empskill = new EmployeeSkill();
            //empskill.P_Id = empskilladd.P_Id;
            //empskill.Skills_Id = empskilladd.Skills_Id;
            //empskill.CommonField_Id = empskilladd.CommonField_Id;
            //empskill.IsActive = true;
            //empskill.Months = empskilladd.Months;
            //empskill.Years = empskilladd.Years;
            //empskill.Status = empskilladd.Status;
            //db.EmployeeSkills.Add(empskill);
            // db.SaveChanges();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public void getEvaluaterList(long? skillid)
        {
            List<EmployeeList> employeeList = (from personal in db.PersonalInfoes
                                               join empskills in db.EmployeeSkills on personal.Id equals empskills.P_Id
                                               join skill in db.Skills on empskills.Skills_Id equals skill.id
                                               where skill.id == skillid
                                               select new EmployeeList
                                               {
                                                   Id = personal.Id,
                                                   Name = personal.Name,
                                                   EmpCode = personal.EmpCode

                                               }).ToList();

            // return Json(employeeList, JsonRequestBehavior.AllowGet);
            ViewData["Evaluater"] = new SelectList(employeeList, "Id", "Name");
        }


    }
}