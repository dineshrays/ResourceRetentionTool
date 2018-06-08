using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class AssignResourceController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: AssignResource
        public ActionResult Index()
        {
            AssignResourceViewModel assignvm = new AssignResourceViewModel();
            //var assignres;
            List<AssignResourceViewModel> assgnvm  = (from assignres in db.AssignResources 
                                join assignresdet in db.AssignResourcesDets
                                on assignres.Id equals assignresdet.AssignResources_Id where assignres.IsActive==true
                                select new AssignResourceViewModel
                                {
                                    Id = assignres.Id,
                                    Manager_Id = assignres.Manager_Id,
                                 managername=   assignres.Manager.Name,
                                Module_Id=    assignres.Module_Id,
                                modulename=    assignres.Module.ModuleName,
                                 Trainer_Id=   assignres.Trainer_Id,
                                  trainername=  assignres.Trainer.Name,
                                 Employee_Id=   assignresdet.Employee_Id,
                                  employeename=  assignresdet.Employee.Name,
                                  Date=  assignres.Date,
                                 IsActive=   assignres.IsActive,
                                AssignResourceId=  assignresdet.Id
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
                AssignResource assRes = new AssignResource()
                {
                    Id = assgnResvm.Id,
                    Date = assgnResvm.Date,
                    Manager_Id = assgnResvm.Manager_Id,
                    Trainer_Id = assgnResvm.Trainer_Id,
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
            assVM.assignResource.managername = assignresourcedb.Manager.Name;
            assVM.assignResource.Manager_Id = assignresourcedb.Manager_Id;
            assVM.assignResource.trainername = assignresourcedb.Trainer.Name;
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
            AssignResource assignRes = new AssignResource()
            {
                Id =id,
            Manager_Id= assgnResvm.Manager_Id,
            Module_Id= assgnResvm.Module_Id,
            Trainer_Id= assgnResvm.Trainer_Id,
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
                managername = assignres.Manager.Name,
               
                trainername = assignres.Trainer.Name,
              
                modulename = assignres.Module.ModuleName,
               
                AssignResourceId = assignresdet.Id,
                employeename = assignresdet.Employee.Name


            };

            List<AssignResourcesDet> assign = db.AssignResourcesDets.Where(m => m.AssignResources_Id == id).ToList();
            ViewBag.AssignResDetails = assign;
            return View(assResvm);
        }
        [HttpPost]
        public ActionResult k(int id,AssignResourceViewModel assgnResvm)
        {
            AssignResource assignres = db.AssignResources.Find(id);
            assignres.IsActive = false;
            db.Entry(assignres).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void getTrainers()
        {
            var val = new SelectList(db.Trainers.ToList(), "id", "Name");
            ViewData["trainerslist"] = val;
        }
        public void getManagers()
        {
            var val = new SelectList(db.Managers.ToList(), "id", "Name");
            ViewData["managerslist"] = val;
        }
        public void getEmployees()
        {
            var val = new SelectList(db.Employees.Where(a => a.IsActive == true).ToList(), "id", "Name");
            ViewData["employeeslist"] = val;
        }
        public void getModules()
        {
            var val = new SelectList(db.Modules.ToList(), "id", "ModuleName");
            ViewData["moduleslist"] = val;
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
          List< EmployeeList>  va = (from emp in db.Employees where emp.Name.Contains(name)
                                  

                                     select new EmployeeList
          {
              Id=emp.Id,
              Name=emp.Name
          }).ToList();
            return new JsonResult{ Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}