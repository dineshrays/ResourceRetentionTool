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
        public string password = "123";

        public int getUserId()
        {
             int id = int.Parse(HttpContext.Current.Session["userId"].ToString());
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
                personlInfo.Name = projectName;
                personlInfo.IsActive = true;
                personlInfo.EmpCode = "000";
                db.PersonalInfoes.Add(personlInfo);
                db.SaveChanges();
                return personlInfo.Id;
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
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }

        public int getDefaultInternRoleId()
        {
            Role role = db.Roles.FirstOrDefault(a => a.Name == internRoleName && a.IsActive == true);
            if (role != null)
            {
                return role.Id;
            }
            else
            {
                Role roles = new Role();
                roles.Name = internRoleName;
                roles.IsActive = true;
                db.Roles.Add(roles);
                db.SaveChanges();
                return roles.Id;
            }

        }

    }
}