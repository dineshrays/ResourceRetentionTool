using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class CircularController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Admin/Circular
        public ActionResult Index()
        {
            getRoleDetails();
            return View();
        }

        public void getRoleDetails()
        {
            List<SelectListItem> list = (from role in db.Roles
                                         select new SelectListItem()
                                         {
                                             Value = role.Id.ToString(),
                                             Text = role.Name
                                         }).ToList();
            //db.Modules;
            list.Insert(0, new SelectListItem() { Value = "0", Text = "All" });
            // var val = new SelectList(db.Modules.ToList(), "id", "ModuleName");
            ViewData["roledetails"] = list;
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

            //  int emproleid = fetchdet.getDefaultEmployeeRoleId();
            List<EmployeeList> va = (from emp in db.PersonalInfoes
                                         // join userdet in db.UserDetails on emp.Id equals userdet.Emp_Id
                                     where emp.Name.Contains(name)

                                     && emp.IsActive == true
                                     //  && userdet.IsActive==true && userdet.Role_Id==emproleid
                                     select new EmployeeList
                                     {
                                         Id = emp.Id,
                                         Name = emp.Name,
                                         EmpCode = emp.EmpCode
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        //public void getRoleDetails()
        //{
        //    var val = new SelectList(db.Roles.ToList(), "Id", "Name");
        //    ViewData["roledetails"] = val;

        //}

        public JsonResult getProjects(string name, int type)
        {
            int projectid = fetchdet.getDefaultProjectId();
            List<EmployeeList> va;
            if (type == 2)
            {
                va = (from project in db.ProjectsDetails
                      join
                      assignres in db.AssignResources
                        on project.Id equals assignres.Project_Id
                      where assignres.IsActive == true && assignres.Project_Id != projectid
                  && !db.EmployeeEvalTasks.Any(a => a.AssignResource_Id == assignres.Id && a.IsActive == true && db.EmployeeEvalTaskDets.Any(b => b.EmployeeEvalTask_Id == a.Id && b.IsEligiableMark == true && b.IsActive == true))
               && project.Name.Contains(name)

         && project.IsActive == true
                      select new EmployeeList
                      {
                          Id = project.Id,
                          Name = project.Name

                      }).ToList();
            }
            else
            {
                va = (from project in db.ProjectsDetails
                      join
projectworked in db.ProjectsWorkeds on
project.Id equals projectworked.Project_Id

                      where project.Name.Contains(name)

&& project.IsActive == true && projectworked.IsActive == true
                      select new EmployeeList
                      {
                          Id = project.Id,
                          Name = project.Name

                      }).Distinct().ToList();
               
            }
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
       
    }
}