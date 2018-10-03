using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
namespace RetentionTool.Areas.Admin.Controllers
{
    public class ProjectWorkedController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: ProjectWorked
        public ActionResult Index()
        {
              List<ProjectsWorked> prjctwrk = db.ProjectsWorkeds.ToList();

            var result1 = prjctwrk.Where(a => a.IsActive == true).GroupBy(p => p.Project_Id).Select(grp => grp.FirstOrDefault());
            //var result = prjctwrk.Select(s => s.Project_Id).Distinct();
            //var value = db.ProjectsWorkeds.Where(a => a.IsActive == true).ToList();

            //.Distinct().ToList();
            //var prjctwrk = (from projectworked in db.ProjectsWorkeds
            //                                   where projectworked.IsActive == true

            //                                   select new ProjectsWorked
            //                                   {
            //                                      Project_Id=projectworked.Project_Id,
            //                                    //  ProjectsDetail=projectworked.ProjectsDetail,
            //                                      StartDate=projectworked.StartDate,
            //                                      EndDate=projectworked.EndDate,
            //                                      Description=projectworked.Description,
            //                                      TeamMembers=projectworked.TeamMembers,
            //                                      Manager_Id=projectworked.Manager_Id
            //                                   }).Distinct().ToList();
            //foreach (var value in result1)
            //{

            //}
            ProjectWorkedViewModel prjctwrkvm = new ProjectWorkedViewModel();

            prjctwrkvm.projectvm = result1.ToList();
            //select   AssignResource_Id from EmployeeEvalTask

            //            select assres.Project_Id from EmployeeEvalTask empeval
            //inner join AssignResources assres on
            //empeval.AssignResource_Id = assres.Id
            ViewBag.CompletedAssId = (from empeval in db.EmployeeEvalTasks 
                                      join assignres in db.AssignResources
                                      on empeval.AssignResource_Id equals assignres.Id
                                      where empeval.IsActive==true && assignres.IsActive==true
                                      select assignres.Project_Id).ToList();
            //db.EmployeeEvalTasks.ToList().Select(a => a.AssignResource_Id);
            //ViewBag.pro = prjctwrk;
            ViewBag.CriticalRes = db.CriticalResources.Where(a => a.IsActive == true).Select(a => a.Project_Id).Distinct().ToList();
            return View(prjctwrkvm);
        }

        public ActionResult Create()
        {
            getManagers();
            getProjectList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(List<ProjectsWorked> prjctwrkvm)
        {
            if(prjctwrkvm!=null)
            {
                // System.Threading.Thread.Sleep(10000);
                foreach (var prjctwrk in prjctwrkvm)
                {
                    prjctwrk.IsActive = true;
                    db.ProjectsWorkeds.Add(prjctwrk);
                    db.SaveChanges();
                }
            }
          
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
           List< ProjectsWorked> prjctwrk = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive==true).ToList();
            ProjectsWorked prjctwrok = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive==true).FirstOrDefault();
            ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            prjwrkvm.projectvm = prjctwrk;
            prjwrkvm.projects = prjctwrok;
            getManagers();
            getProjectList();
            return View(prjwrkvm);
        }
        [HttpPost]
        public ActionResult Edit(int id, List<ProjectsWorked> prjctwrkvm)
        {
            if (prjctwrkvm != null)
            {
                List<ProjectsWorked> projectworkedlist = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive == true).ToList();
                var prjctwrkinactive=projectworkedlist.Where(x => !prjctwrkvm.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();

                var prjctaddlist=prjctwrkvm.Where(x => !projectworkedlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id && x1.IsActive==true)).ToList();
                var prjctwrkmodified = prjctwrkvm.Where(x => projectworkedlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id && x1.IsActive==true)).ToList();


                foreach (var prjctwrkisactive in prjctwrkinactive)
                {
                    prjctwrkisactive.IsActive = false;
                    db.Entry(prjctwrkisactive).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                foreach (var prjctwrk in prjctwrkmodified)
                {
                    var entity = db.ProjectsWorkeds.Find(prjctwrk.Id);
                    prjctwrk.IsActive = true;
                    db.Entry(entity).CurrentValues.SetValues(prjctwrk);
                    // db.Entry(prjctwrk).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //ProjectsWorked projectsWorked = prjctwrkvm.Where(a=>a.Id==prjctwrk.Id);
                    //foreach(var prjctlist in prjctwrkvm)
                    //{
                    //    if(prjctlist.Id== prjctwrk.Id)
                    //    {
                    //        ProjectsWorked projectsworked = new ProjectsWorked();
                    //        projectsworked.Id = prjctlist.Id;
                    //        projectsworked.Project_Id = id;
                    //        projectsworked.PersonalInfo_Id = prjctlist.PersonalInfo_Id;
                    //        projectsworked.Manager_Id = prjctlist.Manager_Id;
                    //        projectsworked.Designation = prjctlist.Designation;
                    //        projectsworked.Responsibilities = prjctlist.Responsibilities;
                    //        projectsworked.StartDate = prjctlist.StartDate;
                    //        projectsworked.EndDate = prjctlist.EndDate;
                    //        projectsworked.Description = prjctlist.Description;
                    //        projectsworked.TeamMembers = prjctlist.TeamMembers;
                    //        projectsworked.IsActive = true;
                    //        db.Entry(projectsworked).State = System.Data.Entity.EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //}

                }

                foreach (var prjctwrk in prjctaddlist)
                {
                    prjctwrk.IsActive = true;
                    db.ProjectsWorkeds.Add(prjctwrk);
                    db.SaveChanges();
                }
                //List<CriticalResource> personalIdlist = db.CriticalResources.Where(a => a.Project_Id == id && a.IsActive == true).ToList();



                //var criticalResFalseList = personalIdlist.Where(x => !prjctwrkvm.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
                //var criticalResAddList = criticallist.Where(x => !personalIdlist.Any(x1 => x1.PersonalInfo_Id == x.PersonalInfo_Id)).ToList();
                //foreach (var personalid in criticalResFalseList)
                //{
                //    personalid.IsActive = false;
                //    db.Entry(personalid).State = System.Data.Entity.EntityState.Modified;
                //    db.SaveChanges();

                //}

                //foreach (var personalinfoid in criticalResAddList)
                //{
                //    personalinfoid.IsActive = true;
                //    personalinfoid.Project_Id = id;
                //    db.CriticalResources.Add(personalinfoid);
                //    db.SaveChanges();

                //}
                //foreach (var prjctwrk in prjctwrkvm)
                //{
                //    prjctwrk.IsActive = true;
                //    db.ProjectsWorkeds.Add(prjctwrk);
                //    db.SaveChanges();
                //}
            }

            return Json("", JsonRequestBehavior.AllowGet);
            //ProjectsWorked projectWorked = new ProjectsWorked();
            //projectWorked = prjctworkvm.projects;
            //projectWorked.IsActive = true;
            //db.Entry(projectWorked).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            // return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            List<ProjectsWorked> prjctwrk = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive == true).ToList();
            ProjectsWorked prjctwrok = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive == true).FirstOrDefault();
            ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            prjwrkvm.projectvm = prjctwrk;
            prjwrkvm.projects = prjctwrok;
            //ProjectsWorked prjctwrk = db.ProjectsWorkeds.Find(id);
            //ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            //prjwrkvm.projects = prjctwrk;
            return View(prjwrkvm);
        }
        [HttpPost]
        public ActionResult Delete(int id, ProjectWorkedViewModel prjctworkvm)
        {
            List<ProjectsWorked> projectworkedlist = db.ProjectsWorkeds.Where(a => a.Project_Id == id && a.IsActive == true).ToList();

            if (projectworkedlist!=null)
            {
                foreach(var prjctwrk in projectworkedlist)
                {
                    prjctwrk.IsActive = false;
                    db.Entry(prjctwrk).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
               
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        public void getManagers()
        {
            int managerroleid = fetchdet.getDefaultManagerRoleId();
            var val = (from personalInfo in db.PersonalInfoes

                       join userdet in db.UserDetails on personalInfo.Id equals userdet.Emp_Id
                       where personalInfo.IsActive == true && userdet.IsActive == true
                       && userdet.Role_Id == managerroleid
                       select new
                       {
                           Id = personalInfo.Id,
                           Name = personalInfo.Name
                       }).ToList();

           
            var data = new SelectList(val, "Id", "Name");
           ViewData["managerslist"] = data;
        }
        public void getProjectList()
        {
            ViewData["projectlist"] = new SelectList(db.ProjectsDetails.Where(a=>a.IsActive==true && !db.AssignResources.Any(b=>b.IsActive==true && b.Project_Id==a.Id)).ToList(),"Id","Name");
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
           // int emproleid = fetchdet.getDefaultEmployeeRoleId();
          
            List<EmployeeList> va = (from emp in db.PersonalInfoes
                                    // join userdet in db.UserDetails on emp.Id equals userdet.Emp_Id
                                     where emp.Name.Contains(name)

            && emp.IsActive == true 
            //&& userdet.Role_Id == emproleid && userdet.IsActive == true
                                     select new EmployeeList
                                     {
                                         Id = emp.Id,
                                         EmpCode = emp.EmpCode,
                                         Name = emp.Name
                                     }).ToList();
            return new JsonResult { Data = va, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult EmpDetails(int projectid)
        {
          //  int emproleid = fetchdet.getDefaultEmployeeRoleId();
            List<EmployeeList> emplist = (from projectwork in db.ProjectsWorkeds
                                          join personalinfo in db.PersonalInfoes
                                         
                                          on projectwork.PersonalInfo_Id equals personalinfo.Id
                                         // join user in db.UserDetails
                                        // on personalinfo.Id equals user.Emp_Id
                                          where projectwork.Project_Id == projectid
                                         // && user.IsActive==true && user.Role_Id==emproleid
                                          select new EmployeeList
                                          {
                                              Id = personalinfo.Id,
                                              Name = personalinfo.Name
                                          }
                                            ).ToList();
      
            return Json(emplist, JsonRequestBehavior.AllowGet);
        }

    }
}