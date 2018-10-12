using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
using PagedList;

namespace RetentionTool.Areas.Trainer.Controllers
{
    public class SessionSummaryController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: SessionSummary
        public ActionResult Index(int? page)
        {

            int td = int.Parse(Session["userid"].ToString());
            List<AssignResourceViewModel> assignreslist = (from assignres in db.AssignResources
                                                           join trainer in db.Trainers
                                                           on assignres.Trainer_Id equals trainer.Id
                                                           where
                                                           db.Trainings.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true)
                                                           && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true &&
                                                           db.EmployeeEvalTaskDets.Any(c => c.EmployeeEvalTask_Id == a.Id && c.IsEligiableMark == true
                                                           && c.IsActive == true)) && trainer.PersonalInfo_Id == td
                                                           select new AssignResourceViewModel
                                                           {
                                                               Id=assignres.Id,
                                                               Module_Id=assignres.Module_Id,
                                                               modulename=assignres.Module.ModuleName,
                                                               Project_Id=assignres.Project_Id,
                                                               projectname=assignres.ProjectsDetail.Name 
                                                           }).Distinct().ToList();
            //ViewBag.ModuleList = assignreslist;
            int pageSize = fetchdet.pageSize;
            int pageIndex = fetchdet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignResourceViewModel> modulepaged = null;
            modulepaged = assignreslist.ToPagedList(pageIndex, pageSize);

            List<SessionView> sessionview = (from assignres in assignreslist
                                             join training in db.Trainings
                                             on assignres.Id equals training.AssignResource_Id
                                             join trainingdet in db.TrainingDets
                                      on training.Id equals trainingdet.Training_Id
                                            
                                             join session in db.Sessions
                                             on trainingdet.Id equals session.TrainingDet_Id
                                             where 
                                            trainingdet.IsActive == true
                                            && training.IsActive==true
                                             && session.IsActive == true

                                             select new SessionView
                                             {
                                                 assignresid =assignres.Id,
                                                 moduleid = assignres.Module_Id,
                                                 modulename = assignres.modulename,
                                                 date = session.Date
                                             }
                                             ).OrderByDescending(s => s.date).ToList();

            List<SessionView> sessionlist = (from sess in sessionview
                                             group sess by sess.moduleid into s
                                             select s.OrderByDescending(t => t.date).FirstOrDefault()
                                ).ToList();
            ViewBag.sessionlist = sessionlist;

            return View(modulepaged);


        }
        public ActionResult Create(int id)
        {

            List<TrainingDet> trainingdet = (from traindet in db.TrainingDets
                                             join training in db.Trainings
                                             on traindet.Training_Id equals training.Id
                                             where training.AssignResource_Id == id && training.IsActive == true
                                             select traindet).ToList();
                //db.TrainingDets.Where(a => a.Training_Id == id && a.IsActive == true).ToList();
                //(from trainindets in db.TrainingDets
                //                            // join moduledet in db.ModuleDets
                //                            // on trainindets.ModuleDet_Id equals moduledet.Id
                //                             where
                //                             //moduledet.Module_Id == id && trainindets.IsActive == true
                //                             select trainindets
                //                            ).ToList();
          //  List<TrainingDet> training = db.TrainingDets.Where(a=> !db.Sessions.Any(b=>b.TrainingDet_Id==a.Id && b.IsActive==true) && a.IsActive==true).ToList();
            //foreach (var i in training)
            //{
            //   // i.ModuleDet.Module.ModuleName;
            //    //i.Training.AssignResource_Id
            //    // i.Training_Id
            //    //  i.Training.AssignResource_Id
            //}

            ViewBag.trainingDetails = trainingdet;
            List<Session> sessions = db.Sessions.Where(a=>a.IsActive==true).ToList();
            SessionSummaryViewModel sessionvm = new SessionSummaryViewModel();
            sessionvm.sessionsvm = sessions;
            return View(sessionvm);
        }
        [HttpPost]
        public ActionResult Create(Session sessionVM,SessionSummaryList[] list)
        {
            //sessionVM.TrainingDet_Id

            //var sessioncount=  (db.Trainings.Any(t => t.AssignResource_Id == a.Id &&
            //                                                              t.IsActive == true && db.TrainingDets.Count(d => d.Training_Id == t.Id && d.IsActive == true).Equals(
            //                                                              db.Sessions.Count(s => s.TrainingDet.Training_Id == t.Id && s.IsActive == true)))
            //                                                              || db.RateEmployeeEligiabilities.Any(b => b.AssignResources_Id == a.Id && b.IsActive == true))
            //AssignResource assRes = new AssignResource()
            //{
            //    Id = assgnResvm.Id,
            //    Date = assgnResvm.Date,
            //    Manager_Id = assgnResvm.Manager_Id,
            //    Trainer_Id = assgnResvm.Trainer_Id,
            //    Module_Id = assgnResvm.Module_Id,
            //    IsActive = true

            //};

           
            sessionVM.IsActive = true;
            db.Sessions.Add(sessionVM);
            db.SaveChanges();
            // int[] empid = assgnResvm.empid;
            //  for (int i = 0; i < empid.Length; i++)

            TrainingDet trainingdet = db.TrainingDets.FirstOrDefault(a => a.Id == sessionVM.TrainingDet_Id && a.IsActive == true);

            Training training = db.Trainings.FirstOrDefault(a => a.Id == trainingdet.Training_Id && a.IsActive == true);

            var traindetcount = db.TrainingDets.Where(a => a.Training_Id == training.Id).Count();
            var sessiondet = db.Sessions.Where(a => db.TrainingDets.Any(b => b.Training_Id == training.Id && b.Id == a.TrainingDet_Id && b.IsActive == true) && a.IsActive == true).Count();
            if(traindetcount==sessiondet)
            {
                AssignResource assRes = db.AssignResources.FirstOrDefault(a => a.Id == training.AssignResource_Id && a.IsActive == true);
               
                int managerroleid = fetchdet.getDefaultManagerRoleId();
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == assRes.Manager_Id && a.Role_Id==managerroleid && a.IsActive == true);
                long id =0;
                if (userdet==null)
                {
                    UserDetail userdetails = new UserDetail();
                    PersonalInfo personalInfo = db.PersonalInfoes.Find(assRes.Manager_Id);
                    userdetails.Role_Id = managerroleid;
                    userdetails.IsActive = true;
                    userdetails.Email = personalInfo.Email;
                    userdetails.Name = personalInfo.Name;
                    userdetails.Password = fetchdet.password;
                    userdetails.EntryDate = DateTime.Now;
                    userdetails.Emp_Id = personalInfo.Id;
                    db.UserDetails.Add(userdetails);
                    db.SaveChanges();
                    id = userdetails.Id;
                }
                else
                {
                    id = userdet.Id;
                }
                Notification notif = new Notification();
                notif.User_Id = id;
                notif.Sessions_Id = sessionVM.Id;
                notif.Message = fetchdet.SessionCompletedMsg;
                notif.IsActive = true;
                notif.IsNotified = true;
                notif.CreatedOn = DateTime.Now;

                db.Notifications.Add(notif);
                db.SaveChanges();
            }
            foreach (var i in list)
            {
                SessionsDet sessionDet = new SessionsDet()
                {
                    Sessions_Id = sessionVM.Id,
                   Employee_Id=i.Id,
                   Attendance=i.Attendance,
                   Remarks=i.Remark,
                   IsActive=true
                };
                if(i.Attendance==false)
                {
                    PersonalInfo personalInfo = db.PersonalInfoes.Find(i.Id);

                    int employeroleid = fetchdet.getDefaultEmployeeRoleId();
                    UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == i.Id && a.Role_Id == employeroleid && a.IsActive == true);
                    Notification notif = new Notification();
                    if (userdet==null)
                    {
                        notif.User_Id = 1;
                       
                    }
                    else
                    {
                        notif.User_Id = userdet.Id;
                    
                    }

                    notif.Sessions_Id = sessionVM.Id;
                    notif.Message =personalInfo.Name+" "+fetchdet.EmployeeAbsenceMsg;
                    notif.IsActive = true;
                    notif.IsNotified = true;
                    notif.CreatedOn = DateTime.Now;

                    db.Notifications.Add(notif);
                    db.SaveChanges();
                }
                db.SessionsDets.Add(sessionDet);
                db.SaveChanges();
            }
            
            //int userid = 0;
           

            //(from traindet in db.TrainingDets
                //                 where traindet.Id == sessionVM.TrainingDet_Id

            //                 );
            //db.Trainings.FirstOrDefault(a => db.TrainingDets.FirstOrDefault(c => c.Id == sessionVM.TrainingDet_Id));

            //   var traindecount = db.Trainers.Where(a => db.TrainingDets.Where(b=>b.Id==sessionVM.TrainingDet_Id && ) && a.IsActive == true).Count();
            //   var sessdetcount=
            // Training training=db.Trainers.FirstOrDefault(a=>a.)
            //AssignResource assignRes = db.AssignResources.Where(a => !db.EmployeeEvalTasks.Any(e => e.AssignResource_Id == a.Id && e.IsActive == true &&
            //                                                             db.EmployeeEvalTaskDets.Any(t => t.EmployeeEvalTask_Id == e.Id &&
            //                                                             t.IsEligiableMark == true && t.IsActive == true)) && a.IsActive == true
            //                                                             && (db.Trainings.Any(t => t.AssignResource_Id == a.Id &&
            //                                                             t.IsActive == true && db.TrainingDets.Count(d => d.Training_Id == t.Id && d.IsActive == true).Equals(
            //                                                             db.Sessions.Count(s => s.TrainingDet.Training_Id == t.Id && s.IsActive == true)))
            //                                                             || db.RateEmployeeEligiabilities.Any(b => b.AssignResources_Id == a.Id && b.IsActive == true))
            //                                                              && db.Trainers.Any(t => t.Id == a.Trainer_Id && t.IsActive == true && t.PersonalInfo_Id == userid)
            //                                                            );
            return Json("", JsonRequestBehavior.AllowGet);
        }
       //[HttpPost]
       public ActionResult CreateSession(int trainingdetid, int trainingid,int moduledetid)
        {

            List<EmployeeList> emplist = (from assResDet in db.AssignResourcesDets
                                          join training in db.Trainings
                                           on assResDet.AssignResources_Id equals training.AssignResource_Id
                                          join trainingdet in db.TrainingDets
                                          on training.Id equals trainingdet.Training_Id
                                          join emp in db.PersonalInfoes
                                          on assResDet.Employee_Id equals emp.Id
                                          where
                                          !db.RateEmployeeEligiabilities.Any(a=>a.AssignResources_Id==training.AssignResource_Id && 
                                          a.IsActive==true && a.IsEligible==true && a.Employee_Id==assResDet.Employee_Id) &&
                                           emp.IsActive == true && trainingdet.ModuleDet_Id == moduledetid && trainingdet.Training_Id == trainingid
                                          select new EmployeeList
                                          {
                                              Id = emp.Id,
                                              Name = emp.Name

                                          }).ToList();

            ViewBag.empDetails = emplist;
            return View();
        }

        public ActionResult Edit(int id)
        {
            Session s = db.Sessions.Find(id);
            ModuleDet mod = db.ModuleDets.Find(s.TrainingDet.ModuleDet_Id);
            SessionSummaryViewModel sessvm = new SessionSummaryViewModel();
            sessvm.session = new SessionSummaryModel();
            sessvm.session.Id = s.Id;
            sessvm.session.Hours = s.Hours;
            sessvm.session.Date = s.Date;
            sessvm.session.Description = s.Description;
            sessvm.session.TrainingDet_Id = s.TrainingDet_Id;
            sessvm.session.Topics = mod.Topics;


            List<SessionSummaryList> sessionlist = (from sessDet in db.SessionsDets
                                                    
                                          join emp in db.PersonalInfoes
                                         on sessDet.Employee_Id equals emp.Id
                                          where
                                     //     !db.RateEmployeeEligiabilities.Any(a=>
                                       //   a.IsActive == true && a.IsEligible == true && a.Employee_Id == sessDet.Employee_Id) &&
                                           sessDet.IsActive == true &&
                                           sessDet.Sessions_Id==id
                                          select new SessionSummaryList
                                          {
                                              //SessionDetId=sessDet.Id,
                                              Id = emp.Id,
                                              Name = emp.Name,
                                              Remark=sessDet.Remarks,
                                              Attendance=sessDet.Attendance

                                          }).ToList();

            ViewBag.sessionDetails = sessionlist;


            return View(sessvm);

        }
        [HttpPost]
        public ActionResult Edit(int id, Session sessionVM, SessionSummaryList[] list)
        {
            var x = (from y in db.SessionsDets where y.Sessions_Id == id select y);
            foreach (var i in x)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
            sessionVM.IsActive = true;
            db.Entry(sessionVM).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            foreach (var i in list)
            {
                SessionsDet sessionDet = new SessionsDet()
                {
                    Sessions_Id = id,
                    Employee_Id = i.Id,
                    Attendance = i.Attendance,
                    Remarks = i.Remark,
                    IsActive = true
                };
                db.SessionsDets.Add(sessionDet);
                db.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }
        public ActionResult Delete(int id)
        {
            Session s = db.Sessions.Find(id);
            ModuleDet mod = db.ModuleDets.Find(s.TrainingDet.ModuleDet_Id);
            SessionSummaryViewModel sessvm = new SessionSummaryViewModel();
            sessvm.session = new SessionSummaryModel();
            sessvm.session.Id = s.Id;
            sessvm.session.Hours = s.Hours;
            sessvm.session.Date = s.Date;
            sessvm.session.Description = s.Description;
            sessvm.session.TrainingDet_Id = s.TrainingDet_Id;
            sessvm.session.Topics = mod.Topics;


            List<SessionSummaryList> sessionlist = (from sessDet in db.SessionsDets
                                                    join emp in db.PersonalInfoes
                                                   on sessDet.Employee_Id equals emp.Id
                                                    where
                                                     sessDet.IsActive == true &&
                                                     sessDet.Sessions_Id == id
                                                    select new SessionSummaryList
                                                    {
                                                        //SessionDetId=sessDet.Id,
                                                        Id = emp.Id,
                                                        Name = emp.Name,
                                                        Remark = sessDet.Remarks,
                                                        Attendance = sessDet.Attendance

                                                    }).ToList();

            ViewBag.sessionDetails = sessionlist;
            return View(sessvm);

          

        }
        [HttpPost]
        public ActionResult Delete(int id, SessionSummaryViewModel session)
        {
            Session s = db.Sessions.Find(id);
            s.IsActive = false;
            db.Entry(s).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult getTrainingDetails(int trainingid,int moduledet_id)
        {
           // var details = db.TrainingDets.Find(trainingid);
            //           select emp.Id, emp.Name from
            //AssignResourcesDet assResDet
            //inner join Training training on
            //assResDet.AssignResources_Id = training.AssignResource_Id
            //inner join TrainingDet transdet on
            //transdet.Training_Id = training.Id
            //inner join Employees emp on
            //emp.id = assResDet.Employee_Id
            //where transdet.ModuleDet_Id = 2002 and transdet.Training_Id = 1
          
            List<EmployeeList> emplist = (from assResDet in db.AssignResourcesDets
                      join training in db.Trainings
                       on assResDet.AssignResources_Id equals training.AssignResource_Id
                      join trainingdet in db.TrainingDets
                      on training.Id equals trainingdet.Training_Id
                      join emp in db.PersonalInfoes
                      on assResDet.Employee_Id equals emp.Id
                      where
                       emp.IsActive == true && trainingdet.ModuleDet_Id == moduledet_id && trainingdet.Training_Id == trainingid
                      select new EmployeeList
                      {
                          Id=emp.Id,
                          Name=emp.Name

                      }).ToList();

           // ViewBag.empDetails = emplist;
            //emp in db.Employees
            //join assgnResDet in db.AssignResourcesDets 
            //on assgnResDet.Id equals Employee.


            //where  emp.IsActive == true
            //select new EmployeeList
            //{
            //    Id = emp.Id,
            //    Name = emp.Name
            //}).ToList();

            return Json(emplist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmployee(string name)
        {
            //List<string> employee = new List<string>();
            //var val = (from e in db.Employees
            //where e.Name.Contains(name)
            //select new { e.Name });
            // var val = db.Employees.Where(a => a.Name.Contains(name)).ToList();
            //IEnumerable<SelectListItem> skilldet =  
            //  List<string> va = new List<string>();
            List<EmployeeList> va = (from emp in db.PersonalInfoes
                                     where emp.Name.Contains(name)

            && emp.IsActive == true
                                     select new EmployeeList
                                     {
                                         Id = emp.Id,
                                         Name = emp.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}