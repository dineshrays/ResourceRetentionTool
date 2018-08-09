using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;


namespace RetentionTool.Areas.Manager.Controllers
{
    public class SearchEmpSkillController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        public static FetchDefaultIds fetchdet = new FetchDefaultIds();
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
                                               join userdet in db.UserDetails on personal.Id equals userdet.Emp_Id

                                               where skill.id==id && userdet.Role_Id == emproleid && userdet.IsActive == true
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




        public ActionResult getNotifications(int userid)
        {
            List<NotificationModel> notification = (from noti in db.Notifications
                                                    where noti.IsActive == true && noti.IsNotified == true
                                                    && noti.User_Id == userid
                                                    select new NotificationModel
                                                    {
                                                        Id = noti.Id,
                                                        Message = noti.Message

                                                    }).OrderByDescending(x=>x.Id).ToList();
           // Session["Notifict"] = int.Parse(Session["Notifict"].ToString()) - 2;
                                               //.OrderBy();
                                               //   db.Notifications.Where(a=>a.User_Id==userid && a.IsActive==true && a.IsNotified==true).ToList();


            //int emproleid = fetchdet.getDefaultEmployeeRoleId();
            //List<EmployeeList> employeeList = (from personal in db.PersonalInfoes
            //                                   join empskills in db.EmployeeSkills on personal.Id equals empskills.P_Id
            //                                   join skill in db.Skills on empskills.Skills_Id equals skill.id
            //                                   join module in db.Modules on skill.id equals module.Skill_Id
            //                                   join userdet in db.UserDetails on personal.Id equals userdet.Emp_Id
            //                                   where module.Id == moduleid && userdet.Role_Id == emproleid && userdet.IsActive == true
            //                                   select new EmployeeList
            //                                   {
            //                                       Id = personal.Id,
            //                                       Name = personal.Name,
            //                                       EmpCode = personal.EmpCode

            //                                   }
            //                                   ).ToList();
            // IEnumerable<SelectListItem> skilldet = getSkillsField(moduleid);
            return Json(notification, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Notification(int userid)
        {
            List<Notification> notification = db.Notifications.Where(a=>a.IsActive==true && a.IsNotified==true && a.User_Id==userid).OrderByDescending(x => x.Id).ToList();
            Session["Notifict"] = 0;
            return View(notification);
        }
    }
}