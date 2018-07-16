using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class SessionSummaryController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: SessionSummary
        public ActionResult Index()
        {

            //List<AssignResourceViewModel> assgnvm = (from assignres in db.AssignResources
            //                                         join m in db.Modules
            //                                         on assignres.Module_Id equals m.Id
            //                                         where assignres.IsActive == true
            //                                         && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
            //                                         select new AssignResourceViewModel
            //                                         {
            //                                             Id = assignres.Id,
            //                                             modulename = assignres.Module.ModuleName,
            //                                             Module_Id = assignres.Module_Id,
            //                                             Project_Id = assignres.Project_Id,
            //                                             projectname = assignres.ProjectsDetail.Name
            //                                         }).ToList();

            //ViewBag.details = assgnvm;

            List<AssignResourceViewModel> assignreslist = (from assignres in db.AssignResources
                                       //                    join   module in db.Modules
                                       //                    on assignres.Module_Id equals module.Id
                                       //                    join moduledet in db.ModuleDets

                                       //on module.Id equals moduledet.Module_Id
                                    
                                       
                                       //join trainingdet in db.TrainingDets
                                       //on moduledet.Id equals trainingdet.ModuleDet_Id
                                       where 
                                       //module.IsActive == true 
                                       //&& trainingdet.IsActive == true
                                       //&& 
                                       db.Trainings.Any(a=>a.AssignResource_Id==assignres.Id && a.IsActive==true)
                                       && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true &&

                                       db.EmployeeEvalTaskDets.Any(c => c.EmployeeEvalTask_Id == a.Id && c.IsEligiableMark == true
                                       && c.IsActive == true))
                                       select new AssignResourceViewModel
                                       {
                                           Id=assignres.Id,
                                           Module_Id=assignres.Module_Id,
                                           modulename=assignres.Module.ModuleName,
                                           Project_Id=assignres.Project_Id,
                                           projectname=assignres.ProjectsDetail.Name
                                         //  TrainingId = trainingdet.Training_Id
                                           
                                       }).Distinct().ToList();

            ViewBag.ModuleList = assignreslist;
            List<SessionView> sessionview = (from assignres in assignreslist
                                             join training in db.Trainings
                                             on assignres.Id equals training.AssignResource_Id
                                             join trainingdet in db.TrainingDets
                                      on training.Id equals trainingdet.Training_Id
                                             //join moduledet in db.ModuleDets
                                             //on trainingdet.ModuleDet_Id equals moduledet.Id
                                             //assignres.Module_Id equals moduledet.Module_Id
                                             
                                             join session in db.Sessions
                                             on trainingdet.Id equals session.TrainingDet_Id
                                             where 
                                             //assignres.IsActive == true &&
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

            //List<SessionSummaryModel> sessionvm = (from session in db.Sessions
            //                                         join training in db.TrainingDets
            //                                         on session.TrainingDet_Id equals training.Id
            //                                         join moduledet in db.ModuleDets on
            //                                           training.ModuleDet_Id equals moduledet.Id
            //                                         where session.IsActive == true
            //                                           && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == training.Training.AssignResource_Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))

            //                                       select new SessionSummaryModel
            //                                         {
            //                                             Id = session.Id,
            //                                             Topics=moduledet.Topics,
            //                                             Hours=session.Hours,
            //                                             Description=session.Description,
            //                                             Date = session.Date,
            //                                             IsActive = session.IsActive
                            
            //                                         }).ToList();
            return View(sessionlist);


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
                db.SessionsDets.Add(sessionDet);
                db.SaveChanges();
            }

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
                                          !db.RateEmployeeEligiabilities.Any(a=>
                                          a.IsActive == true && a.IsEligible == true && a.Employee_Id == sessDet.Employee_Id) &&
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