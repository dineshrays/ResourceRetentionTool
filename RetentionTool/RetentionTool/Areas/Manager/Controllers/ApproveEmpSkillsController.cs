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
            List<EmployeeSkillsAdd> emp = db.EmployeeSkillsAdds.Where(a => db.PersonalInfoes.Any(b => b.Id == a.P_Id)).ToList();
            EmployeeSkillsAddViewModel skilladd = new EmployeeSkillsAddViewModel();

            skilladd.skiladd = emp;
            return View(skilladd);
        }

        public ActionResult Evaluates1(int id)
        {
            EmployeeSkillsAdd esd = db.EmployeeSkillsAdds.Find(id);
            EmployeeSkillsAddViewModel empvm = new EmployeeSkillsAddViewModel();
            empvm.EmployeeSkillsAdd = esd;

            //  empvm.GetApproveEmpSkills=
            getEvaluaterList(esd.Skills_Id);
            return View(empvm);

        }

        [HttpPost]
        public ActionResult Evaluates1(int id, ApproveEmpSkillsModel appl)
        {
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