using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;

namespace RetentionTool.Controllers
{
    public class EmployeeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Employee
        public ActionResult Index()
        {
            //var modules = (from module in db.Modules
            //              join moduleDet in db.Skills
            //              on module.Skill_Id equals moduleDet.id select new
            //              {
            //                  module.Id,
            //                  module.ModuleName,
            //                  moduleDet.id,
            //                  moduleDet.Name
            //              }).ToList();


            List<Employee> emp = db.Employees.Where(m=>m.IsActive==true).ToList();
            List<EmployeeViewModel> e = emp.Select(x => new EmployeeViewModel()
            {
                Id=x.Id,
                Name=x.Name,
                IsActive=x.IsActive
            }
                ).ToList();
               
            return View(e);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeViewModel empvm)
        {
            Employee e = new Employee()
            {
                Id=empvm.Id,
                Name=empvm.Name,
                IsActive=true

            };
            db.Employees.Add(e);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Employee e = db.Employees.Find(id);
            
            EmployeeViewModel empvm = new EmployeeViewModel()
            {
                Id=e.Id,
                Name=e.Name,
                IsActive=e.IsActive

            };
            return View(empvm);
        }
        [HttpPost]
        public ActionResult Edit(int id,EmployeeViewModel empvm)
        {
            //AssignResourcesDet assignResDet = db.AssignResourcesDets.FirstOrDefault(m => m.Employee_Id == id);
            Employee e = new Employee();
            e.Id = empvm.Id;
            e.Name = empvm.Name;
            e.IsActive = true;
            db.Entry(e).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Employee e = db.Employees.Find(id);

            EmployeeViewModel empvm = new EmployeeViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                IsActive = e.IsActive

            };
            AssignResourcesDet assignResDet = db.AssignResourcesDets.FirstOrDefault(m => m.Employee_Id == id);
            if(assignResDet==null)
            {
                ViewData["Message"] = null;
            }
            else
            {
                ViewData["Message"] = "Cannot Delete";
            }
            return View(empvm);
        }
        [HttpPost]
        public ActionResult Delete(int id, EmployeeViewModel empvm)
        {
            Employee e = db.Employees.Find(id);
            e.IsActive = false;
            db.Entry(e).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}