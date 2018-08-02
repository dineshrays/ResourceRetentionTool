using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class AssignResourceController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: AssignResource
        public ActionResult Index()
        {
           // AssignResourceViewModel assignResourceViewModel = new AssignResourceViewModel();
            //var assignres;
            List<AssignResourceViewModel> assgnvm  = (from assignres in db.AssignResources 
                               // join assignresdet in db.AssignResourcesDets
                               // on assignres.Id equals assignresdet.AssignResources_Id
                               
                           
                                                      where assignres.IsActive== true && assignres.ProjectsDetail.Name != "Training"
                                                      && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))

                                                      select new AssignResourceViewModel
                                {
                                    Id = assignres.Id,
                                    Manager_Id = assignres.Manager_Id,
                                    Project_Id=assignres.Project_Id,
                                    projectname=assignres.ProjectsDetail.Name,
                                 managername =   assignres.PersonalInfo.Name,
                                 Module_Id=    assignres.Module_Id,
                                modulename=    assignres.Module.ModuleName,
                                 Trainer_Id=   assignres.Trainer.PersonalInfo_Id,
                                  trainername=  assignres.Trainer.PersonalInfo.Name,
                             //    Employee_Id=   assignresdet.Employee_Id,
                              //    employeename=  assignresdet.PersonalInfo.Name,
                                  Date=  assignres.Date,
                                 IsActive=   assignres.IsActive,
                             //   AssignResourceId=  assignresdet.Id
                                }).ToList();

            //List<AssignResourceViewModel> assgnvm = assginmodule.Select(
            //    a => new AssignResourceViewModel
            //    {
            //        Id = a.Id,
            //        Date = a.Date,
            //        Manager_Id = a.Manager_Id,
            //        Module_Id=a.Module_Id,
            //        Employee_Id=a.Employee_Id,
            //        Trainer_Id=a.Trainer_Id,
            //        IsActive=a.IsActive,
            //        managername=a.Manager.Name,
            //        trainername=a.Trainer.Name,
            //        employeename=a.Employee.Name,
            //        modulename=a.Module.ModuleName

            //    }).ToList();

            //  return View(assgnvm);
            return View(assgnvm);
        }

        public ActionResult Create()
        {
            getEmployees();
            getManagers();
            getModules();
            getTrainers();
            return View();
        }
        [HttpPost]
        public ActionResult Create(AssignResourceViewModel assgnResvm,EmployeeList[] list)
        {
           
         if(assgnResvm!=null)
            {
                RetentionTool.Models.Trainer trainer = db.Trainers.FirstOrDefault(a => a.PersonalInfo_Id == assgnResvm.Trainer_Id && a.IsActive==true);
                AssignResource assRes = new AssignResource()
                {
                    Id = assgnResvm.Id,
                    Date = assgnResvm.Date,
                    Project_Id=assgnResvm.Project_Id,
                    Manager_Id = assgnResvm.Manager_Id,
                    Trainer_Id = trainer.Id,
                    Module_Id = assgnResvm.Module_Id,
                    IsActive = true

                };
                db.AssignResources.Add(assRes);
                db.SaveChanges();
               // int[] empid = assgnResvm.empid;
              //  for (int i = 0; i < empid.Length; i++)
              foreach(var i in list)
                {
                    AssignResourcesDet assResDet = new AssignResourcesDet()
                    {
                        Id = assgnResvm.AssignResourceId,
                        AssignResources_Id = assRes.Id,
                        Employee_Id = i.Id
                    };
                    db.AssignResourcesDets.Add(assResDet);
                    db.SaveChanges();
                }

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            

            AssignResource assignresourcedb = db.AssignResources.Find(id);
            List<AssignResourcesDet> assign = db.AssignResourcesDets.Where(m => m.AssignResources_Id == id).ToList();
         //   AssignResourcesDet assignresdet = db.AssignResourcesDets.FirstOrDefault(m => m.AssignResources_Id == id);
            AssignResouViewModel assVM = new AssignResouViewModel();
            assVM.assignResource = new AssignResourceViewModel();
            assVM.assignResource.Id = assignresourcedb.Id;
            assVM.assignResource.Date = assignresourcedb.Date;
            assVM.assignResource.Project_Id = assignresourcedb.Project_Id;
            assVM.assignResource.projectname = assignresourcedb.ProjectsDetail.Name;
            assVM.assignResource.managername = assignresourcedb.PersonalInfo.Name;
            assVM.assignResource.Manager_Id = assignresourcedb.Manager_Id;
            assVM.assignResource.trainername = assignresourcedb.Trainer.PersonalInfo.Name;
            assVM.assignResource.Trainer_Id = assignresourcedb.Trainer_Id;
            assVM.assignResource.modulename = assignresourcedb.Module.ModuleName;
               assVM.assignResource.Module_Id = assignresourcedb.Module_Id;

            //AssignResourceViewModel assResvm = new AssignResourceViewModel()
            //{
            //    Id= assignres.Id,
            //    Date= assignres.Date,
            //    managername= assignres.Manager.Name,
            //    Manager_Id= assignres.Manager_Id,
            //    trainername= assignres.Trainer.Name,
            //    Trainer_Id= assignres.Trainer_Id,
            //    modulename= assignres.Module.ModuleName,
            //    Module_Id= assignres.Module_Id
            //   // AssignResourceId=assignresdet.Id,
            //    //employeename=assignresdet.Employee.Name,
            //    //Employee_Id=assignresdet.Employee_Id


            //};
            //getEmployees();
            //List<EmployeeList> emplist = new List<EmployeeList>();
            //foreach (var i in assign)
            //{
            //    i.Employee.Name
            //}
            ViewBag.AssignResDetails = assign;
            getManagers();
            getModules();
            getTrainers();
            return View(assVM);
        }
        [HttpPost]
        public ActionResult Edit(int id,AssignResourceViewModel assgnResvm,EmployeeList[] list)
        {
            var x = (from y in db.AssignResourcesDets where y.AssignResources_Id == id select y);
            foreach(var i in x)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
            RetentionTool.Models.Trainer trainer = db.Trainers.FirstOrDefault(a => a.PersonalInfo_Id == assgnResvm.Trainer_Id);
            AssignResource assignRes = new AssignResource()
            {
                Id =id,
                Project_Id=assgnResvm.Project_Id,
            Manager_Id= assgnResvm.Manager_Id,
            Module_Id= assgnResvm.Module_Id,
            Trainer_Id= trainer.Id,
            Date=assgnResvm.Date,
            IsActive= true


            };
            db.Entry(assignRes).State = System.Data.Entity.EntityState.Modified;

            //AssignResourcesDet assignResDet = new AssignResourcesDet()
            //{
            //    Id = assgnResvm.AssignResourceId,
            //    AssignResources_Id=assgnResvm.Id,
            //    Employee_Id=assgnResvm.Employee_Id


            //};
            //db.Entry(assignRes).State = System.Data.Entity.EntityState.Modified;
            //db.Entry(assignResDet).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            foreach (var i in list)
            {
                AssignResourcesDet assResDet = new AssignResourcesDet()
                {
                    Id = assgnResvm.AssignResourceId,
                    AssignResources_Id = id,
                    Employee_Id = i.Id
                };
                db.AssignResourcesDets.Add(assResDet);
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            AssignResource assignres = db.AssignResources.Find(id);
            AssignResourcesDet assignresdet = db.AssignResourcesDets.FirstOrDefault(m => m.AssignResources_Id == id);
            AssignResourceViewModel assResvm = new AssignResourceViewModel()
            {
                Id = assignres.Id,
                Date = assignres.Date,
                projectname=assignres.ProjectsDetail.Name,
                Project_Id=assignres.Project_Id,
                managername = assignres.PersonalInfo.Name,
               
                trainername = assignres.Trainer.PersonalInfo.Name,
              
                modulename = assignres.Module.ModuleName,
               
                AssignResourceId = assignresdet.Id,
                employeename = assignresdet.PersonalInfo.Name


            };

            List<AssignResourcesDet> assign = db.AssignResourcesDets.Where(m => m.AssignResources_Id == id).ToList();
            ViewBag.AssignResDetails = assign;
            return View(assResvm);
        }
        [HttpPost]
        public ActionResult Delete(int id,AssignResourceViewModel assgnResvm)
        {
            AssignResource assignres = db.AssignResources.Find(id);
            assignres.IsActive = false;
            db.Entry(assignres).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void getTrainers()
        {
            var data = (from personalInfo in db.PersonalInfoes
                        join
trainer in db.Trainers on personalInfo.Id equals trainer.PersonalInfo_Id
//join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
where personalInfo.IsActive==true && trainer.IsActive==true  
//&& 
                        select new
                        {
                            Id=personalInfo.Id,
                            Name=personalInfo.Name
                        }).ToList();
            //  var val = new SelectList(db.PersonalInfoes.ToList(), "Id", "Name");
            ViewData["trainerslist"] = new SelectList(data,"Id","Name");
                //val;
        }
        public void getManagers()
        {
            int managerroleid = fetchdet.getDefaultManagerRoleId();
            var val = (from personalInfo in db.PersonalInfoes
              
                       join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
                        where personalInfo.IsActive == true && userdet.IsActive == true
                        &&  userdet.Role_Id== managerroleid
                       select new
                        {
                            Id = personalInfo.Id,
                            Name = personalInfo.Name
                        }).ToList();


            ViewData["managerslist"] = new SelectList(val, "Id", "Name"); ;
        }

        public ActionResult getEmployeeDetails(int moduleid)
        {
            List<EmployeeList> employeeList = (from personal in db.PersonalInfoes
                                               join empskills in db.EmployeeSkills on personal.Id equals empskills.P_Id
                                               join skill in db.Skills on empskills.Skills_Id equals skill.id
                                               join module in db.Modules on skill.id equals module.Skill_Id
                                               where module.Id == moduleid
                                               select new EmployeeList
                                               {
                                                   Id = personal.Id,
                                                   Name=personal.Name,
                                                   EmpCode=personal.EmpCode

                                               }
                                               ).ToList();
           // IEnumerable<SelectListItem> skilldet = getSkillsField(moduleid);
            return Json(employeeList, JsonRequestBehavior.AllowGet);

        }

        public IEnumerable<SelectListItem> getSkillsField(int commonId)
        {
            IEnumerable<SelectListItem> rs = db.Skills.Where(s => s.CommonField_Id == commonId).Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.Name
            }).ToList();

            return new SelectList(rs, "Value", "Text");
        }
        public void getEmployees()
        {
            int emproleid = fetchdet.getDefaultEmployeeRoleId();
            var data = (from personalInfo in db.PersonalInfoes
                        join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
                        where personalInfo.IsActive == true 
                        && userdet.IsActive == true
                        && userdet.Role_Id==emproleid
                        select new
                        {
                            Id = personalInfo.Id,
                            Name = personalInfo.Name
                        }).ToList();
            //  var val = new SelectList(db.PersonalInfoes.ToList(), "Id", "Name");
            ViewData["employeeslist"] = new SelectList(data, "Id", "Name");
            //var val = new SelectList(db.PersonalInfoes.Where(a => a.IsActive == true).ToList(), "id", "Name");
            //ViewData["employeeslist"] = val;
        }
        public void getModules()
        {
            List<SelectListItem> list = (from module in db.Modules
                                     select new SelectListItem()
                                     {
                                         Value = module.Id.ToString(),
                                         Text = module.ModuleName
                                     }).ToList();
            //db.Modules;
            list.Insert(0, new SelectListItem() { Value = "0", Text = "Select Module" });
           // var val = new SelectList(db.Modules.ToList(), "id", "ModuleName");
            ViewData["moduleslist"] = list;
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

            int emproleid = fetchdet.getDefaultEmployeeRoleId();
            List< EmployeeList>  va = (from emp in db.PersonalInfoes
                                   join userdet in db.UserDetails on emp.Id equals userdet.Emp_Id
                                     where emp.Name.Contains(name)
                                  
                                     && emp.IsActive==true 
                                     && userdet.IsActive==true && userdet.Role_Id==emproleid
                                     select new EmployeeList
          {
              Id=emp.Id,
              Name=emp.Name,
              EmpCode=emp.EmpCode
          }).ToList();
            return new JsonResult{ Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult getProjects(string name)
        {
            List<EmployeeList> va = (from project in db.ProjectsDetails join
                                     projectworked in db.ProjectsWorkeds on
                                     project.Id equals projectworked.Project_Id
                                     join critical in db.CriticalResources on
                                     projectworked.PersonalInfo_Id equals critical.PersonalInfo_Id

                                     where project.Name.Contains(name)

         && project.IsActive == true && projectworked.IsActive==true
                                     select new EmployeeList
                                     {
                                         Id = project.Id,
                                         Name=project.Name
                                        
                                     }).Distinct().ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        public JsonResult getProjectInfo(int id)
        {
           
            ProjectsWorked projectsWorked = db.ProjectsWorkeds.FirstOrDefault(a => a.IsActive == true && a.Project_Id == id);
            //List<ProjectsWorked> projectsWorked = (from prjct in db.ProjectsWorkeds
            //                            join critical in db.CriticalResources
            //                            on prjct.Project_Id equals critical.Project_Id
            //                            where prjct.Project_Id == id && prjct.IsActive == true
            //                            && critical.IsActive == true 
            //                            select new ProjectsWorked
            //                            {
            //                                Manager_Id=prjct.Manager_Id,
            //                                PersonalInfo_Id=critical.PersonalInfo_Id

            //                            }
            //                           ).Distinct().ToList();
            CriticalResource criticalRes = db.CriticalResources.FirstOrDefault(a => a.IsActive == true && a.Project_Id == id);
            AssignResProjectDetails assResPrjDet = new AssignResProjectDetails();
            RetentionTool.Models.Trainer trainer = db.Trainers.FirstOrDefault(a=>a.CriticalResource_Id==criticalRes.Id && a.IsActive==true);
            assResPrjDet.Manager_Id = projectsWorked.Manager_Id;
            //if (criticalRes!=null)
            //{
            //    assResPrjDet.Trainer_Id = criticalRes.PersonalInfo_Id;
            //}
            assResPrjDet.Trainer_Id = trainer.PersonalInfo_Id;
           
           
            return new JsonResult { Data = assResPrjDet, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult PopupTest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmpDetails(int assId)
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
        }
    }
}