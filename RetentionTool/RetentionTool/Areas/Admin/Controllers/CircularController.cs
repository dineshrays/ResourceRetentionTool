using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;

namespace RetentionTool.Areas.Admin.Controllers
{
    public class CircularController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();
        // GET: Admin/Circular
        public ActionResult Index()
        {
            getRoleDetails(0);
            return View();
        }

        public void getRoleDetails(int type)
        {
            int adminroleid = fetchdet.getDefaultAdminRoleId();
            int trainerroleid = fetchdet.getDefaultTrainerRoleId();
            List<SelectListItem> list;
            if (type==2)
            {
                list = (from role in db.Roles
                        where role.Id!=adminroleid && role.Id!=trainerroleid
                        select new SelectListItem()
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        }).ToList();
            }
            else
            {
                list = (from role in db.Roles
                        where role.Id != adminroleid
                        select new SelectListItem()
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        }).ToList();
            }
           
            //db.Modules;
            list.Insert(0, new SelectListItem() { Value = "0", Text = "All" });
            // var val = new SelectList(db.Modules.ToList(), "id", "ModuleName");
            ViewData["roledetails"] = list;
        }

        public ActionResult getRolesDetails(int type)
        {

            int adminroleid = fetchdet.getDefaultAdminRoleId();
            int trainerroleid = fetchdet.getDefaultTrainerRoleId();
            List<SelectListItem> list;
            if (type == 2)
            {
                list = (from role in db.Roles
                        where role.Id != adminroleid && role.Id != trainerroleid
                        select new SelectListItem()
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        }).ToList();
            }
            else
            {
                list = (from role in db.Roles
                        where role.Id != adminroleid
                        select new SelectListItem()
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        }).ToList();
            }

            //db.Modules;
            list.Insert(0, new SelectListItem() { Value = "0", Text = "All" });
           // IEnumerable<SelectListItem> skilldet = getSkillsField(skillId);
            return Json(list, JsonRequestBehavior.AllowGet);

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
            if (type == 1)
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

                      }).Distinct().ToList();
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


        [HttpPost]
        public ActionResult Create(CircularViewModel circularvm,int type,int projectid)
        {
            Circular_Details circulardet = new Circular_Details();
            circulardet.Contents = circularvm.Contents;
            circulardet.Date = circularvm.Date;
            circulardet.ValidFrom = circularvm.ValidFrom;
            circulardet.ValidTo = circularvm.ValidTo;
            circulardet.CreatedOn = DateTime.Now;
            circulardet.IsActive = true;
            db.Circular_Details.Add(circulardet);
            db.SaveChanges();

            int empidroleid = fetchdet.getDefaultEmployeeRoleId();
            int managerroleid = fetchdet.getDefaultManagerRoleId();
            int trainerroleid = fetchdet.getDefaultTrainerRoleId();
            if (type == 1)
            {
                int roleid = circularvm.roleId;
                if (roleid == 0)
                {
                    getEmployeesDetails(projectid,circulardet.Id,type);
                    getManagerDetails(projectid, circulardet.Id, type);
                    getTrainersDetails(projectid, circulardet.Id, type);
                }
               
               
                if(roleid==empidroleid)
                {
                    //List<AssignResourcesDet> assResDet = db.AssignResourcesDets.Where(a => a.AssignResource.Project_Id == projectid && a.AssignResource.IsActive == true).ToList();
                    getEmployeesDetails(projectid, circulardet.Id,type);
                }
                if (roleid == managerroleid)
                {
                    //var managerid = (from ass in db.AssignResources 
                    //                  where ass.Project_Id==projectid && ass.IsActive==true

                    //                  select ass.Manager_Id).FirstOrDefault();
                    getManagerDetails(projectid, circulardet.Id, type);
                        
                       // db.AssignResources.FirstOrDefault(a => a.Project_Id == projectid && a.IsActive == true);

                }

                if (roleid == trainerroleid)
                {
                    //var trainerid = (from ass in db.AssignResources
                    //                 join train in db.Trainers
                    //                 on ass.Trainer_Id equals train.Id
                    //                 where ass.Project_Id == projectid && ass.IsActive == true

                    //                 select train.PersonalInfo_Id).FirstOrDefault();
                    getTrainersDetails(projectid, circulardet.Id, type);
                }
            }

            if (type == 2)
            {
                int roleid = circularvm.roleId;
                if (roleid == 0)
                {
                    getEmployeesDetails(projectid, circulardet.Id, type);
                    getManagerDetails(projectid, circulardet.Id, type);
                   
                }
               
               // int trainerroleid = fetchdet.getDefaultTrainerRoleId();
                if (roleid == empidroleid)
                {
                    getEmployeesDetails(projectid, circulardet.Id, type);
                    
                }
                if (roleid == managerroleid)
                {
                   
                    getManagerDetails(projectid, circulardet.Id, type);
                   
                }

               
            }

            if(type==0)
            {

                int roleid = circularvm.roleId;
                if (roleid == 0)
                {
                    getAllEmployee(empidroleid, circulardet.Id);
                    getAllEmployee(managerroleid, circulardet.Id);
                    getAllEmployee(trainerroleid, circulardet.Id);
                }

                // int trainerroleid = fetchdet.getDefaultTrainerRoleId();
                if (roleid == empidroleid)
                {
                    getAllEmployee(empidroleid, circulardet.Id);
                    
                   
                }
                if (roleid == managerroleid)
                {

                    getAllEmployee(managerroleid, circulardet.Id);

                }
                if (roleid == trainerroleid)
                {

                    getAllEmployee(trainerroleid, circulardet.Id);

                }
            }
            //if (assgnResvm != null)
            //{
            //    RetentionTool.Models.Trainer trainer = db.Trainers.FirstOrDefault(a => a.PersonalInfo_Id == assgnResvm.Trainer_Id && a.IsActive == true);
            //    AssignResource assRes = new AssignResource()
            //    {
            //        Id = assgnResvm.Id,
            //        Date = assgnResvm.Date,
            //        Project_Id = assgnResvm.Project_Id,
            //        Manager_Id = assgnResvm.Manager_Id,
            //        Trainer_Id = trainer.Id,
            //        Module_Id = assgnResvm.Module_Id,
            //        IsActive = true

            //    };
            //    db.AssignResources.Add(assRes);
            //    db.SaveChanges();
            //    // int[] empid = assgnResvm.empid;
            //    //  for (int i = 0; i < empid.Length; i++)
            //    foreach (var i in list)
            //    {
            //        AssignResourcesDet assResDet = new AssignResourcesDet()
            //        {
            //            Id = assgnResvm.AssignResourceId,
            //            AssignResources_Id = assRes.Id,
            //            Employee_Id = i.Id
            //        };
            //        db.AssignResourcesDets.Add(assResDet);
            //        db.SaveChanges();
            //    }

            //}

            return Json("", JsonRequestBehavior.AllowGet);
        }

       public void getManagerDetails(int projectid,long circularid,int type)
        {
            PersonalInfo personalinfo;
             //   ;= (from ass in db.AssignResources
             //                            join personalin in db.PersonalInfoes
             //                            on ass.Manager_Id equals personalin.Id
             //where ass.Project_Id == projectid && ass.IsActive == true

             //select personalin).FirstOrDefault();
             if(type==1)
            {
                personalinfo = (from ass in db.AssignResources
                                join personalin in db.PersonalInfoes
                                on ass.Manager_Id equals personalin.Id
                                where ass.Project_Id == projectid && ass.IsActive == true

                                select personalin).FirstOrDefault();
            }
            else
            {
                personalinfo = (from projectwork in db.ProjectsWorkeds
                                join personalin in db.PersonalInfoes
                                on projectwork.Manager_Id equals personalin.Id
                                where projectwork.Project_Id == projectid && projectwork.IsActive == true

                                select personalin).FirstOrDefault();
            }
            int managerroleid = fetchdet.getDefaultManagerRoleId();
            UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == personalinfo.Id && a.Role_Id == managerroleid && a.IsActive == true);
            if (userdet == null)
            {
                UserDetail user = new UserDetail();
                user.Emp_Id = personalinfo.Id;
                user.EntryDate = DateTime.Now;
                user.Email = personalinfo.Email;


                user.Role_Id = managerroleid;
                user.Name = personalinfo.Name;
                user.IsActive = true;
                user.Password = fetchdet.password;

                db.UserDetails.Add(user);
                db.SaveChanges();
                CircularUser_Details circularuser_det = new CircularUser_Details();
                circularuser_det.Circular_Id = circularid;
                circularuser_det.User_Id = user.Id;
                circularuser_det.IsActive = true;
                circularuser_det.IsNotified = true;
                circularuser_det.CreatedOn = DateTime.Now;
                db.CircularUser_Details.Add(circularuser_det);
                db.SaveChanges();
            }
            else
            {
                CircularUser_Details circularuser_det = new CircularUser_Details();
                circularuser_det.Circular_Id = circularid;
                circularuser_det.User_Id = userdet.Id;
                circularuser_det.IsActive = true;
                circularuser_det.IsNotified = true;
                circularuser_det.CreatedOn = DateTime.Now;
                db.CircularUser_Details.Add(circularuser_det);
                db.SaveChanges();
            }
          
            //return int.Parse(managerid.ToString());
        }
        public void getTrainersDetails(int projectid, long circularid,int type)
        {
            PersonalInfo personalinfo=null;
            if (type == 1)
            {
                personalinfo = (from ass in db.AssignResources
                                join train in db.Trainers
                                on ass.Trainer_Id equals train.Id
                                join personalinf in db.PersonalInfoes
                                on train.PersonalInfo_Id equals personalinf.Id
                                where ass.Project_Id == projectid && ass.IsActive == true

                                select personalinf).FirstOrDefault();
            }
            //else
            //{
            //    personalinfo = (from ass in db.ProjectsWorkeds
            //                    join train in db.Trainers
            //                    on ass.Trainer_Id equals train.Id
            //                    join personalinf in db.PersonalInfoes
            //                    on train.PersonalInfo_Id equals personalinf.Id
            //                    where ass.Project_Id == projectid && ass.IsActive == true

            //                    select personalinf).FirstOrDefault();
            //}
                //= (from ass in db.AssignResources
                //             join train in db.Trainers
                //             on ass.Trainer_Id equals train.Id
                //             join personalinf in db.PersonalInfoes
                //             on train.PersonalInfo_Id equals personalinf.Id
                //             where ass.Project_Id == projectid && ass.IsActive == true

                //             select personalinf).FirstOrDefault();
            int trainerroleid = fetchdet.getDefaultTrainerRoleId();
            UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == personalinfo.Id && a.Role_Id == trainerroleid && a.IsActive == true);
            if (userdet == null)
            {
                UserDetail user = new UserDetail();
                user.Emp_Id = personalinfo.Id;
                user.EntryDate = DateTime.Now;
                user.Email = personalinfo.Email;


                user.Role_Id = trainerroleid;
                user.Name = personalinfo.Name;
                user.IsActive = true;
                user.Password = fetchdet.password;

                db.UserDetails.Add(user);
                db.SaveChanges();
                CircularUser_Details circularuser_det = new CircularUser_Details();
                circularuser_det.Circular_Id = circularid;
                circularuser_det.User_Id = user.Id;
                circularuser_det.IsActive = true;
                circularuser_det.IsNotified = true;
                circularuser_det.CreatedOn = DateTime.Now;
                db.CircularUser_Details.Add(circularuser_det);
                db.SaveChanges();
            }
            else
            {
                CircularUser_Details circularuser_det = new CircularUser_Details();
                circularuser_det.Circular_Id = circularid;
                circularuser_det.User_Id = userdet.Id;
                circularuser_det.IsActive = true;
                circularuser_det.IsNotified = true;
                circularuser_det.CreatedOn = DateTime.Now;
                db.CircularUser_Details.Add(circularuser_det);
                db.SaveChanges();
            }
            //  return int.Parse(trainerid.ToString());
        }
        public void getEmployeesDetails(int projectid, long circularid,int type)
        {
            List<PersonalInfo> employeeLists;
            if(type==1)
            {
                employeeLists = (from ass in db.AssignResources
                                 join assdet in db.AssignResourcesDets
                                 on ass.Id equals assdet.AssignResources_Id
                                 join persnalinfo in db.PersonalInfoes
                                 on assdet.Employee_Id equals persnalinfo.Id
                                 where ass.Project_Id == projectid
                                 && ass.IsActive == true
                                 select persnalinfo
                                                ).ToList();
            }
            else
            {
                employeeLists = (from projectwrkd in db.ProjectsWorkeds
                                
                                 join persnalinfo in db.PersonalInfoes
                                 on projectwrkd.PersonalInfo_Id equals persnalinfo.Id
                                 where projectwrkd.Project_Id == projectid
                                 && projectwrkd.IsActive == true
                                 select persnalinfo
                                                ).ToList();
            }
        
            foreach(var empid in employeeLists)
            {
                int employeeroleid = fetchdet.getDefaultTrainerRoleId();
                UserDetail userdet = db.UserDetails.FirstOrDefault(a => a.Emp_Id == empid.Id && a.Role_Id == employeeroleid && a.IsActive == true);
                if (userdet == null)
                {
                    UserDetail user = new UserDetail();
                    user.Emp_Id = empid.Id;
                    user.EntryDate = DateTime.Now;
                    user.Email = empid.Email;


                    user.Role_Id = employeeroleid;
                    user.Name = empid.Name;
                    user.IsActive = true;
                    user.Password = fetchdet.password;

                    db.UserDetails.Add(user);
                    db.SaveChanges();
                    CircularUser_Details circularuser_det = new CircularUser_Details();
                    circularuser_det.Circular_Id = circularid;
                    circularuser_det.User_Id = user.Id;
                    circularuser_det.IsActive = true;
                    circularuser_det.IsNotified = true;
                    circularuser_det.CreatedOn = DateTime.Now;
                    db.CircularUser_Details.Add(circularuser_det);
                    db.SaveChanges();
                }
                else
                {
                    CircularUser_Details circularuser_det = new CircularUser_Details();
                    circularuser_det.Circular_Id = circularid;
                    circularuser_det.User_Id = userdet.Id;
                    circularuser_det.IsActive = true;
                    circularuser_det.IsNotified = true;
                    circularuser_det.CreatedOn = DateTime.Now;
                    db.CircularUser_Details.Add(circularuser_det);
                    db.SaveChanges();
                }
                //CircularUser_Details circularuser_det = new CircularUser_Details();
                //circularuser_det.Circular_Id = circularid;
                //circularuser_det.User_Id = empid.Id;
                //circularuser_det.IsActive = true;
                //circularuser_det.IsNotified = true;
                //circularuser_det.CreatedOn = DateTime.Now;
                //db.CircularUser_Details.Add(circularuser_det);
                //db.SaveChanges();
            }
        }


        public void getAllEmployee(int roleid, long circularid)
        {
            List<UserDetail> employeeLists = (from user in db.UserDetails
                                                join role in db.Roles
                                                on user.Role_Id equals role.Id
                                                where role.Id== roleid
                                                && user.IsActive == true
                                                select user
                                                ).ToList();
            foreach (var empid in employeeLists)
            {
                CircularUser_Details circularuser_det = new CircularUser_Details();
                circularuser_det.Circular_Id = circularid;
                circularuser_det.User_Id = empid.Id;
                circularuser_det.IsActive = true;
                circularuser_det.IsNotified = true;
                circularuser_det.CreatedOn = DateTime.Now;
                db.CircularUser_Details.Add(circularuser_det);
                db.SaveChanges();
            }
            // return employeeLists;
        }
    }
}