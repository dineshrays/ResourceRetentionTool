using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;


namespace RetentionTool.Areas.Employee.Controllers
{
    public class ApproveEmpSkillsController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Manager/ApproveEmpSkills
        public ActionResult Index()
        {
            int employeeid = fetchdet.getUserDetailsId();
            List<EmployeeSkillsAdd> emp = (from approv in db.ApproveEmpSkills
                                           join empskill in db.EmployeeSkillsAdds
                                           on approv.EmpskillAdd_Id equals empskill.Id
                                           where approv.Emp_Id==employeeid
                                           && approv.IsEvaluated==null
                                           select empskill 

                
                ).ToList();
                
              //  db.EmployeeSkillsAdds.Where(a => db.PersonalInfoes.Any(b => b.Id == a.P_Id)).ToList();
            EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();

            skilladd.skiladd = emp;
            return View(skilladd);
        }

        public ActionResult Evaluates1(int id)
        {
            EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find(id);
            EmployeeSkillsAddViewModel empvm = new EmployeeSkillsAddViewModel();
            empvm.EmployeeSkillsAdd = esd;
            int empid = fetchdet.getUserDetailsId();
            ApproveEmpSkill approveskill = db.ApproveEmpSkills.FirstOrDefault(a=>a.EmpskillAdd_Id==esd.Id && a.Emp_Id== empid && a.IsActive==true);
            //  empvm.GetApproveEmpSkills=
            empvm.approveskill = approveskill;
            getEvaluaterList(esd.Skills_Id);
            return View(empvm);

        }

        [HttpPost]
        public ActionResult Evaluates1(int id, ApproveEmpSkillsModel appl)
        {
            
            ApproveEmpSkill app = db.ApproveEmpSkills.Find(appl.Id);
            //app=db.ApproveEmpSkills.Find(id);
            app.EmpskillAdd_Id = app.EmpskillAdd_Id;
            app.Emp_Id = app.Emp_Id;
            app.TaskAssigned = appl.TaskAssigned;
            app.Remark = appl.Remark;
            app.IsEvaluated = appl.IsEvaluated;
            app.IsActive = true;
            app.CreatedOn = System.DateTime.Now;

            db.Entry(app).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmployeeSkillsAdd empskilladd = db.EmployeeSkillsAdds.Find(id);

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