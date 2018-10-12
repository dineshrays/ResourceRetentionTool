using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace RetentionTool.Areas.Trainer.Controllers
{
    public class TrainingController : Controller
    {

        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        public ActionResult PopupView()
        {
            return View();
        }
        // GET: Training
        public ActionResult Index(int? page)
        {

            int trainerid = int.Parse(Session["userid"].ToString());
            List<AssignResourceViewModel> assgnvm = (from assignres in db.AssignResources
                                                     join m in db.Modules
                                                     on assignres.Module_Id equals m.Id
                                                     join trainer in db.Trainers
                                                     on assignres.Trainer_Id equals trainer.Id
                                                     where assignres.IsActive == true
                                                     && !db.EmployeeEvalTasks.Any(a=>a.AssignResource_Id==assignres.Id && a.IsActive==true && db.EmployeeEvalTaskDets.Any(b=>b.EmployeeEvalTask_Id==a.Id && b.IsEligiableMark==true && b.IsActive==true))
                                                    && trainer.PersonalInfo_Id==trainerid 
                                                    select new AssignResourceViewModel
                                                     {
                                                         Id = assignres.Id,     
                                                         modulename= assignres.Module.ModuleName,
                                                         Module_Id = assignres.Module_Id,
                                                         Project_Id=assignres.Project_Id,
                                                         projectname=assignres.ProjectsDetail.Name
                                                     }).ToList();

            ViewBag.details = assgnvm;
            List<Training> trainings = db.Trainings.ToList();
            TrainingViewModel trainingvm = new TrainingViewModel();
            trainingvm.Training = trainings;

            ViewBag.Training = trainings;
            int pageSize = fetchdet.pageSize;
            int pageIndex = fetchdet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignResourceViewModel> modulepaged = null;
            modulepaged = assgnvm.ToPagedList(pageIndex, pageSize);
            return View(modulepaged);
            //return View(trainingvm);
        }
        // [HttpPost]
        //public ActionResult EmpDetails(int assId)
        //{
        //    List<EmployeeList> emplist = (from assresdet in db.AssignResourcesDets
        //                                  join emp in db.PersonalInfoes
        //                                  on assresdet.Employee_Id equals emp.Id
        //                                  where emp.IsActive == true
        //                                  where assresdet.AssignResources_Id == assId
        //                                  select new EmployeeList
        //                                  {
        //                                      Id = emp.Id,
        //                                      Name = emp.Name
        //                                  }).ToList();

        //    return Json(emplist, JsonRequestBehavior.AllowGet);
        //}

        
        public ActionResult Empdetails(int assId)
        {
            List<EmployeeList> emplist = (from assresdet in db.AssignResourcesDets
                                          join emp in db.PersonalInfoes
                                          on assresdet.Employee_Id equals emp.Id
                                          where emp.IsActive == true
                                          where assresdet.AssignResources_Id == assId
                                          select new EmployeeList
                                          {
                                              Id = emp.Id,
                                              Name = emp.Name
                                          }).ToList();

            return Json(emplist, JsonRequestBehavior.AllowGet);
                //PartialView("_Empdetails", emplist);
        }

        public ActionResult Create(int moduleid,int assignresid)
        {
            
            List<ModuleViewModel> Topics = (

                from assin in db.AssignResources
                join mod in db.ModuleDets on assin.Module_Id equals mod.Module_Id
                
                    where assin.Module_Id==moduleid && assin.Id ==assignresid
                    select new ModuleViewModel
                    {
                        Id=mod.Id,
                        Topics=mod.Topics
                    }).ToList();

            ViewBag.Topics = Topics;
            return View();
        }

        [HttpPost]
        public ActionResult Createtraining( Item[] itemlist, Training TrainingVM)
        {
            Training t = new Training();
            TrainingDet td = new TrainingDet();

            t.AssignResource_Id = TrainingVM.AssignResource_Id;
            t.FromDate = TrainingVM.FromDate;
            t.ToDate = TrainingVM.ToDate;
            t.IsActive = true;
            db.Trainings.Add(t);
            db.SaveChanges();
            
            foreach (var i in itemlist)
            {
                td.Training_Id = t.Id;
                td.ModuleDet_Id=Convert.ToInt32(i.Topics);
                td.HoursRequired = i.HoursRequired;
                td.IsActive = true;
                db.TrainingDets.Add(td);
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            Training tr = db.Trainings.Find(id);
            //List<TrainingDet> tdetails = db.TrainingDets.Where(t => t.Training_Id == id).ToList();
            List<Item> tdetails = (from trainingdet in db.TrainingDets
                                   join moduledet in db.ModuleDets on
                                   trainingdet.ModuleDet_Id equals moduledet.Id
                                   where trainingdet.Training_Id == id
                                   select new Item
                                   {
                                       Id = moduledet.Id,
                                       Topics=moduledet.Topics,
                                       HoursRequired=trainingdet.HoursRequired
                                   }
                               ).ToList();
            try
            {
                TrainingViewModel obj = new TrainingViewModel();

                obj.TrainingVm = new Training();

                obj.TrainingVm.Id = tr.Id;
                obj.TrainingVm.AssignResource_Id = tr.AssignResource_Id;
                obj.TrainingVm.FromDate = tr.FromDate;
                obj.TrainingVm.ToDate = tr.ToDate;

                ViewBag.details = tdetails;
                return View(obj);
            }
            catch (Exception ex)
            {
                return View();            
            }        
        }               

        [HttpPost]
        public ActionResult Edit(int id, Training tvm, Item[] itemlist)
        {
            var x = (from y in db.TrainingDets
                     where y.Training_Id == id
                     select y);
            foreach (var item in x)
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            tvm.IsActive = true;
            db.Entry(tvm).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TrainingDet td = new TrainingDet();
            foreach (var i in itemlist)
            {
                td.Training_Id = id;
                td.ModuleDet_Id = i.Id;
                td.HoursRequired = i.HoursRequired;
                td.IsActive = true;
                db.TrainingDets.Add(td);
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult delete(int id)
        {
            Training tr = db.Trainings.Find(id);
            //List<TrainingDet> tdetails = db.TrainingDets.Where(t => t.Training_Id == id).ToList();
            List<Item> tdetails = (from trainingdet in db.TrainingDets
                                   join moduledet in db.ModuleDets on
                                   trainingdet.ModuleDet_Id equals moduledet.Id
                                   where trainingdet.Training_Id == id
                                   select new Item
                                   {
                                       Id = moduledet.Id,
                                       Topics = moduledet.Topics,
                                       HoursRequired = trainingdet.HoursRequired

                                   }
                               ).ToList();


            TrainingViewModel obj = new TrainingViewModel();

            obj.TrainingVm = new Training();

            obj.TrainingVm.Id = tr.Id;
            obj.TrainingVm.AssignResource_Id = tr.AssignResource_Id;
            obj.TrainingVm.FromDate = tr.FromDate;
            obj.TrainingVm.ToDate = tr.ToDate;

            ViewBag.details = tdetails;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(int id,TrainingViewModel tvm)
        {
            Training m = db.Trainings.Find(id);
            if (m.Id == id)
            {
                m.IsActive = false;
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
                                                   
                                                    where noti.IsActive == true && noti.IsNotified == true
                                                    && noti.User_Id == userid
                                                    select new NotificationModel
                                                    {
                                                        Id = noti.Id,
                                                        Message = noti.Message,
                                                        type = 1,
                                                        subMessage="Assign Project for Training"
                                                        //,subMessage = "Assign Resource id :" + assignres.Id + " Project Name: " + assignres.ProjectsDetail.Name
                                                        //+ " of Module  " + assignres.Module.ModuleName + " "

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
            List<Notification> notif = db.Notifications.Where(a => a.User_Id == userid).ToList();

            foreach (var notific in notif)
            {
                notific.IsNotified = false;
                notific.ModifiedOn = DateTime.Now;
                db.Entry(notific).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            List<CircularUser_Details> circularuserdet = db.CircularUser_Details.Where(a => a.User_Id == userid).ToList();
            foreach (var circul in circularuserdet)
            {
                circul.IsNotified = false;
                circul.ModifiedOn = DateTime.Now;
                db.Entry(circul).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
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