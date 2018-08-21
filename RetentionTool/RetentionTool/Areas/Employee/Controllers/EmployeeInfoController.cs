using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Areas.Employee.Controllers
{
    public class EmployeeInfoController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: EmployeeInfo
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult PersonalInfoCreate()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult PersonalInfoCreate(PersonalInfoModel pim)
        //{
        //    PersonalInfo p = new PersonalInfo();
        //    p.EmpCode = pim.EmpCode;
        //    p.Name = pim.Name;
        //    p.FatherName = pim.FatherName;
        //    p.DOB = pim.DOB;
        //    p.Gender = pim.Gender;
        //    p.PermanentAddress = pim.PermanentAddress;
        //    p.CommunicationAddress = pim.CommunicationAddress;
        //    p.Contact = pim.Contact;
        //    p.Qualification = pim.Qualification;
        //    p.Email = pim.Email;
        //    p.PanNo = pim.PanNo;
        //    p.AadharNo = pim.AadharNo;
        //    p.BloodGroup = pim.BloodGroup;
        //    p.IsActive = true;

        //    db.PersonalInfoes.Add(p);
        //    db.SaveChanges();
            

        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult EduQualificationCreate()
        //{
        //    getId();
        //    return View();

        //}
        //public void getId()
        //{
        //    FetchDefaultIds fetchdet = new FetchDefaultIds();
        //    int id = fetchdet.getUserId();
        //    ViewData["Id"] = new SelectList(db.PersonalInfoes.Where(a=>a.Id==id).ToList(), "Id","Name");
        //}
        
        //[HttpPost]
        //public ActionResult EduQualificationCreate(EducationQualificationModel eqm)
        //{
        //    EducationQualification edu = new EducationQualification();

        //    edu.P_Id = eqm.P_Id; 
        //    edu.Degree = eqm.Degree;
        //    edu.Board = eqm.Board;
        //    edu.YearOfPassing = eqm.YearOfPassing;
        //    edu.Percentage = eqm.Percentage;
        //    edu.IsActive = true;

        //    db.EducationQualifications.Add(edu);
        //    db.SaveChanges();
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EmpSkillsCreate()
        {
            FetchDefaultIds fetchdet = new FetchDefaultIds();
            int id = fetchdet.getUserDetailsId();
            PersonalInfo personalinfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == id);
            EmployeeInfoViewModel empvm = new EmployeeInfoViewModel();
            PersonalInfoModel personalInfovm = new PersonalInfoModel();
            personalInfovm.Id = personalinfo.Id;
            personalInfovm.Name = personalinfo.Name;
            empvm.PersonalInfoVm = personalInfovm;
           // ViewData["Id"] = new SelectList(db.PersonalInfoes.Where(a => a.Id == id).ToList(), "Id", "Name");
            //getId();
            return View(empvm);

        }
        [HttpPost]
        public ActionResult EmpSkillsCreate(List<EmployeeSkillsAdd> empSkill)
        {
            if (empSkill != null)
            {
                foreach (var ski in empSkill)
                {
                    CurrentInfo currInfo = db.CurrentInfoes.FirstOrDefault(a => a.P_Id == ski.P_Id);
                    int mangerid = int.Parse(currInfo.ReportingManager);
                    ski.Manager_Id =mangerid ;
                    ski.IsApproved = false;
                    ski.IsActive = true;
                    db.EmployeeSkillsAdds.Add(ski);
                    db.SaveChanges();
                }
            }
            
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSkills(string name)
        {
          
            List< SkillsViewModel> va = (from skill in db.Skills
                                     where skill.Name.Contains(name)
                                     select new SkillsViewModel
                                     {
                                         id = skill.id,
                                        
                                         SkillName = skill.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult getNotifications(int userid)
        {
            List<NotificationModel> notification = getNotifiDetails(userid);

            return Json(notification, JsonRequestBehavior.AllowGet);

        }


        public List<NotificationModel> getNotifiDetails(int userid)
        {
            List<NotificationModel> notificatModel = new List<NotificationModel>();


            List<NotificationModel> notification = (from noti in db.Notifications
                                                    join sess in db.Sessions
                                                    on noti.Sessions_Id equals sess.Id
                                                    join trainingdet in db.TrainingDets
                                                    on sess.TrainingDet_Id equals trainingdet.Id
                                                    join training in db.Trainings
                                                    on trainingdet.Training_Id equals training.Id
                                                    join assignres in db.AssignResources
                                                    on training.AssignResource_Id equals assignres.Id
                                                    //join module in db.Modules
                                                    //on assignres.Module_Id equals module.Id
                                                    where noti.IsActive == true && noti.IsNotified == true
                                                    && noti.User_Id == userid
                                                    select new NotificationModel
                                                    {
                                                        Id = noti.Id,
                                                        Message = noti.Message,
                                                        type = 1,
                                                        subMessage = "Assign Resource id :" + assignres.Id + " Project Name: " + assignres.ProjectsDetail.Name
                                                        + " of Module  " + assignres.Module.ModuleName + " "

                                                    }).OrderByDescending(x => x.Id).ToList();




            List<NotificationModel> circular = (from circula in db.Circular_Details
                                                join circulardet in db.CircularUser_Details
                                                on circula.Id equals circulardet.Circular_Id
                                                where circula.IsActive == true && circulardet.IsActive == true
                                                && circulardet.IsNotified == true && circulardet.User_Id == userid

                                                select new NotificationModel
                                                {
                                                    Id = circulardet.Id,
                                                    Message = circula.Contents,
                                                    type = 2,
                                                    subMessage = "Circular Message"

                                                }).OrderByDescending(x => x.Id).ToList();

            foreach (var notif in notification)
            {
                notificatModel.Add(notif);
            }
            foreach (var circu in circular)
            {
                notificatModel.Add(circu);
            }
            return notificatModel;
        }

        public ActionResult Notification(int userid)
        {
            List<NotificationModel> notification = getNotifiDetails(userid);
            //db.Notifications.Where(a=>a.IsActive==true && a.IsNotified==true && a.User_Id==userid).OrderByDescending(x => x.Id).ToList();
            //List<Notification> notif = db.Notifications.Where(a => a.User_Id == userid).ToList();

            //foreach (var notific in notif)
            //{
            //    notific.IsNotified = false;
            //    notific.ModifiedOn = DateTime.Now;
            //    db.Entry(notific).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
            //List<CircularUser_Details> circularuserdet = db.CircularUser_Details.Where(a => a.User_Id == userid).ToList();
            //foreach (var circul in circularuserdet)
            //{
            //    circul.IsNotified = false;
            //    circul.ModifiedOn = DateTime.Now;
            //    db.Entry(circul).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
            setNotification(userid);

            return View(notification);
        }
        public void setNotification(int userid)
        {
            fetchdet.getNotificationCount(userid);
            //Session["Notifict"] = db.Notifications.Where(a => a.User_Id == userid && a.IsActive == true && a.IsNotified == true).Count();
        }
        public ActionResult Notif(int notificationid, int type)
        {
            if (type == 1)
            {
                Notification notification = db.Notifications.Find(notificationid);
                notification.IsNotified = false;
                db.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                CircularUser_Details notification = db.CircularUser_Details.Find(notificationid);
                notification.IsNotified = false;
                db.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            int userid = fetchdet.getUserId();

            setNotification(userid);
            return Json("", JsonRequestBehavior.AllowGet);

        }
    }
}