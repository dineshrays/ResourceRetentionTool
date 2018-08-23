using RetentionTool.Models;
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
            List<EmployeeSkillsAdd> emp = db.EmployeeSkillsAdds.Where(a => a.Manager_Id==managerid && a.IsApproved==null && a.IsActive==true ).ToList();
            EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();

            skilladd.skiladd = emp;
            return View(skilladd);
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
            if (appl.IsEvaluated.ToString() == "True")
            {
                EmployeeSkillsAdd empskilladd = db.EmployeeSkillsAdds.Find(id);

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