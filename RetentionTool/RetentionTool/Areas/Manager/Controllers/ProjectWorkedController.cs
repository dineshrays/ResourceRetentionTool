using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
//using RetentionTool.ViewModel;
using PagedList;

namespace RetentionTool.Areas.Manager.Controllers
{
    public class ProjectWorkedController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        public static FetchDefaultIds fetchdet = new FetchDefaultIds();
        int managerid = fetchdet.getUserDetailsId();
        // GET: ProjectWorked
        public ActionResult Index(int? page)
        {
              List<ProjectsWorked> prjctwrk = db.ProjectsWorkeds.Where(a=>a.Manager_Id==managerid && a.IsActive==true).ToList();

            var result1 = prjctwrk.Where(a => a.IsActive == true).GroupBy(p => p.Project_Id).Select(grp => grp.FirstOrDefault());
            
            ProjectWorkViewModel prjctwrkvm = new ProjectWorkViewModel();

            prjctwrkvm.projectvm = result1.ToList();
           
            ViewBag.CompletedAssId = (from empeval in db.EmployeeEvalTasks 
                                      join assignres in db.AssignResources
                                      on empeval.AssignResource_Id equals assignres.Id
                                      where empeval.IsActive==true && assignres.IsActive==true
                                      && assignres.Manager_Id==managerid
                                      select assignres.Project_Id).ToList();
    
            ViewBag.CriticalRes = db.CriticalResources.Where(a => a.IsActive == true).Select(a => a.Project_Id).Distinct().ToList();

            int pageSize = fetchdet.pageSize;
            int pageIndex = fetchdet.pageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ProjectsWorked> modulepaged = null;
            modulepaged = prjctwrkvm.projectvm.ToPagedList(pageIndex, pageSize);
            return View(modulepaged);
            //return View(prjctwrkvm);
        }

        public ActionResult Create()
        {
            ProjectWorkViewModel prjctwrkdvm = new ProjectWorkViewModel();
            ProjectsWorked prjctswrkd = new ProjectsWorked();
            prjctswrkd.Manager_Id = managerid;
           
           // prjctwrkdvm.projects.Manager_Id = managerid;
            PersonalInfo personalInfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == managerid && a.IsActive == true);

            prjctwrkdvm.managername = personalInfo.Name;
            // getManagers();
            prjctwrkdvm.projects = prjctswrkd;
            getProjectList();
            return View(prjctwrkdvm);
        }
        [HttpPost]
        public ActionResult Create(List<ProjectsWorked> prjctwrkvm)
        {
            if(prjctwrkvm!=null)
            {
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
            ProjectWorkViewModel prjwrkvm = new ProjectWorkViewModel();
            prjwrkvm.projectvm = prjctwrk;
            prjwrkvm.projects = prjctwrok;
            ProjectsDetail projectdet = db.ProjectsDetails.Find(id);
            prjwrkvm.projectname = projectdet.Name;

            PersonalInfo personalInfo = db.PersonalInfoes.FirstOrDefault(a => a.Id == managerid && a.IsActive == true);
            prjwrkvm.managername = personalInfo.Name;
            //   getManagers();
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
            ProjectWorkViewModel prjwrkvm = new ProjectWorkViewModel();
            prjwrkvm.projectvm = prjctwrk;
            prjwrkvm.projects = prjctwrok;
            //ProjectsWorked prjctwrk = db.ProjectsWorkeds.Find(id);
            //ProjectWorkedViewModel prjwrkvm = new ProjectWorkedViewModel();
            //prjwrkvm.projects = prjctwrk;
            return View(prjwrkvm);
        }
        [HttpPost]
        public ActionResult Delete(int id, ProjectWorkViewModel prjctworkvm)
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
            var val = new SelectList(db.PersonalInfoes.ToList(), "id", "Name");
            ViewData["managerslist"] = val;
        }
        public void getProjectList()
        {
            ViewData["projectlist"] = new SelectList(db.ProjectsDetails.Where(a=>a.IsActive==true && !db.AssignResources.Any(b=>b.IsActive==true && b.Project_Id==a.Id)).ToList(),"Id","Name");
        }
        public JsonResult getEmployee(string name)
        {
            int emproleid = fetchdet.getDefaultEmployeeRoleId();
            //List<string> employee = new List<string>();
            //var val = (from e in db.Employees
            //where e.Name.Contains(name)
            //select new { e.Name });
            // var val = db.Employees.Where(a => a.Name.Contains(name)).ToList();
            //IEnumerable<SelectListItem> skilldet =  
            //  List<string> va = new List<string>();
            List<EmployeeList> va = (from emp in db.PersonalInfoes
                                     join userdet in db.UserDetails on emp.Id equals userdet.Emp_Id
                                     where emp.Name.Contains(name)

            && emp.IsActive == true && userdet.Role_Id == emproleid && userdet.IsActive == true
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
            int emproleid = fetchdet.getDefaultEmployeeRoleId();

            List<EmployeeList> emplist = (from projectwork in db.ProjectsWorkeds
                                          join personalinfo in db.PersonalInfoes
                                          on projectwork.PersonalInfo_Id equals personalinfo.Id
                                          join userdet in db.UserDetails on personalinfo.Id equals userdet.Emp_Id
                                          where projectwork.Project_Id == projectid
                                          && userdet.Role_Id == emproleid && userdet.IsActive == true
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