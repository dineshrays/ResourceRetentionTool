using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;



namespace RetentionTool.Models
{
    public class FetchDefaultIds
    {
        RetentionToolEntities db = new RetentionToolEntities();
        public string projectName = "Training";
        public string adminName = "Admin";
        public string adminRoleName = "Admin";
        public string trainerRoleName = "Trainer";
        public string employeeRoleName = "Employee";
        public string managerRoleName = "Manager";
        public string internRoleName = "Intern";
        public string commonFieldName = "Development";
        //public string skillName = "Java";
        //public string moduleName = "Java Training";
        //public string topics1 = "OOPs Concepts";
        //public string topics2 = "Exception Handling";
        //public string topics3 = "Swings";
        public string password = "123";
        //public int commonfieldid = ;
        public string SessionCompletedMsg = "Session Has been Completed Successfully";
        public string EmployeeAbsenceMsg = " is Absent for the Session";
        public string AssignTrainerMsg = " has been Assigned take Session";
        public string ApproveEmployeeMsg = " Skill has been Approved Successfully";

        public void getNotificationCount(long userid)
        {
         int notifcount= db.Notifications.Where(a => a.User_Id == userid && a.IsActive == true && a.IsNotified == true).Count();
            int circularcount = (from circu in db.Circular_Details
                                 join circudet in db.CircularUser_Details
                                 on circu.Id equals circudet.Circular_Id
                                 where circu.IsActive==true && 
                                 circudet.User_Id== userid && circudet.IsActive==true && circudet.IsNotified==true
                                 select circudet.Id
                                 ).Count();
                
                
               // db.CircularUser_Details.Where(a => a.User_Id == userid && a.IsActive == true && a.IsNotified == true).Count();
            int total = notifcount + circularcount;
             HttpContext.Current.Session["Notifict"] = total;

        }

      

        public void updateCircular()
        {
            List<Circular_Details> circular_Details = db.Circular_Details.Where(a => a.ValidTo <= DateTime.Now && a.IsActive == true).ToList();
            foreach (var circular in circular_Details)
            {
                circular.IsActive = false;
                db.Entry(circular).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                List<CircularUser_Details> circularUser_detail = db.CircularUser_Details.Where(a => a.Circular_Id == circular.Id && a.IsActive == true).ToList();
                foreach (var circUserdet in circularUser_detail)
                {
                    circUserdet.IsActive = false;
                    db.Entry(circUserdet).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public int getUserId()
        {
            int id = int.Parse(HttpContext.Current.Session["uid"].ToString());
            return id;
        }

        public int getUserDetailsId()
        {
            int id = int.Parse(HttpContext.Current.Session["userid"].ToString());
            return id;
        }
        public int getDefaultProjectId()
        {
            ProjectsDetail projectsDet = db.ProjectsDetails.FirstOrDefault(a => a.Name == projectName && a.IsActive == true);
            if(projectsDet!=null)
            {
                return projectsDet.Id;
            }
            else
            {
                ProjectsDetail prjct = new ProjectsDetail();
                prjct.Name = projectName;
                prjct.IsActive = true;
                
                db.ProjectsDetails.Add(prjct);
                db.SaveChanges();
                return prjct.Id;
            }
           
        }

        public int getDefaultAdminId()
        {
            PersonalInfo personalInfo = db.PersonalInfoes.FirstOrDefault(a => a.Name == adminName && a.IsActive == true);
            if (personalInfo != null)
            {
                return personalInfo.Id;
            }
            else
            {
               
                PersonalInfo personlInfo = new PersonalInfo();
                personlInfo.Name = adminName;
                personlInfo.IsActive = true;
                personlInfo.EmpCode = "000";
                db.PersonalInfoes.Add(personlInfo);
                db.SaveChanges();
               
                return personlInfo.Id;
            }

        }
        public void addDefaultAdminName()
        {
            int id = getDefaultAdminId();
            int adminroleid = getDefaultAdminRoleId();
            UserDetail userResult = db.UserDetails.FirstOrDefault(a => a.Email == adminName && a.Password == adminName && a.IsActive == true && a.Role_Id == adminroleid);

            if(userResult==null)
            {
                UserDetail userDet = new UserDetail();
                userDet.Email = adminName;
                userDet.Name = adminName;
                userDet.Emp_Id = id;
                userDet.IsActive = true;
                userDet.Role_Id = adminroleid;
                userDet.Password = adminName;
                userDet.EntryDate = DateTime.Now;
                db.UserDetails.Add(userDet);
                db.SaveChanges();
            }

           
            
        }

        public int getDefaultCriticalResourceId()
        {
            int prjctId = getDefaultProjectId();
            int adminId = getDefaultAdminId();
          CriticalResource criticalRes= db.CriticalResources.FirstOrDefault(a => a.Project_Id==prjctId && a.PersonalInfo_Id==adminId && a.IsActive == true);
            if (criticalRes != null)
            {
                return criticalRes.Id;
            }
            else
            {
                CriticalResource criticalResource = new CriticalResource();
                criticalResource.PersonalInfo_Id = adminId;
                criticalResource.Project_Id = prjctId;
                criticalResource.IsActive = true;
                db.CriticalResources.Add(criticalResource);
                db.SaveChanges();
                return criticalResource.Id;
            }

        }


        public int getDefaultTrainerRoleId()
        {
            Role role = db.Roles.FirstOrDefault(a => a.Name == trainerRoleName && a.IsActive == true);
            if (role != null)
            {
                return role.Id;
            }
            else
            {
                Role roles = new Role();
                roles.Name = trainerRoleName;
                roles.IsActive = true;
                roles.EntryDate = DateTime.Now;
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }

        public int getDefaultAdminRoleId()
        {
            Role role = db.Roles.FirstOrDefault(a => a.Name == adminRoleName && a.IsActive == true);
            if (role != null)
            {
                return role.Id;
            }
            else
            {
                Role roles = new Role();
                roles.Name = adminRoleName;
                roles.IsActive = true;
                roles.EntryDate = DateTime.Now;
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }
        public int getDefaultManagerRoleId()
        {
            Role role = db.Roles.FirstOrDefault(a => a.Name == managerRoleName && a.IsActive == true);
            if (role != null)
            {
                return role.Id;
            }
            else
            {
                Role roles = new Role();
                roles.Name = managerRoleName;
                roles.IsActive = true;
                roles.EntryDate = DateTime.Now;
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }


        public int getDefaultEmployeeRoleId()
        {
            Role role = db.Roles.FirstOrDefault(a => a.Name == employeeRoleName && a.IsActive == true);
            if (role != null)
            {
                return role.Id;
            }
            else
            {
                Role roles = new Role();
                roles.Name = employeeRoleName;
                roles.IsActive = true;
                roles.EntryDate = DateTime.Now;
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }



        public  long getDefaultPrimarySkillId()
        {
            Commonfield commfield = db.Commonfields.FirstOrDefault(a => a.Name == commonFieldName && a.IsActive == true);

            if (commfield != null)
            {
                return commfield.id;
            }
            else
            {
                Commonfield commField = new Commonfield();
                commField.Name = commonFieldName;
                commField.IsActive = true;

                return commField.id;
            }

        }

    }
}