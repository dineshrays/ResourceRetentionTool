using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.ViewModel;
using RetentionTool.Models;

namespace RetentionTool.Controllers
{
    public class UserDetailsController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: UserDetails
        public ActionResult Index()
        {
            List<UserDetail> userdet = db.UserDetails.Where(a => a.IsActive == true).ToList();
            UserDetailsViewModel uservm = new UserDetailsViewModel();
            uservm.userDetailsvm = userdet;
            return View(uservm);
        }
        public ActionResult Create()
        {
            getRoles();
            return View();
        }
        [HttpPost]
        public ActionResult Create(List<UserDetail> uservm)
        {
           // if(!ModelState.IsValid)
           foreach(var user in uservm)
            {
                user.IsActive = true;
                user.EntryDate = DateTime.Now;
                db.UserDetails.Add(user);
                db.SaveChanges();
            }
         
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            UserDetail userdet = db.UserDetails.Find(id);
            UserDetailsViewModel uservm = new UserDetailsViewModel();
            uservm.userDetail = userdet;
            getRoles();
            return View(uservm);
        }
        [HttpPost]
        public ActionResult Edit(int id,UserDetailsViewModel uservm)
        {
            UserDetail userdet = db.UserDetails.Find(id);
            if(userdet.Id==id)
            {
                if(id==uservm.userDetail.Id)
                {
                    userdet.Name = uservm.userDetail.Name;
                  userdet.Password= uservm.userDetail.Password;
                    userdet.Role_Id = uservm.userDetail.Role_Id;
                    db.Entry(userdet).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index");
            
        }
        public ActionResult Delete(int id)
        {
            UserDetail userdet = db.UserDetails.Find(id);
            UserDetailsViewModel uservm = new UserDetailsViewModel();
            uservm.userDetail = userdet;
            return View(uservm);
        }
        [HttpPost]
        public ActionResult Delete(int id, UserDetailsViewModel uservm)
        {
            UserDetail userdet = db.UserDetails.Find(id);
            if (userdet.Id == id)
            {
                if (id == uservm.userDetail.Id)
                {
                    userdet.IsActive = false;
                    db.Entry(userdet).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }

      
        public JsonResult getEmployeeCode(string name)
        {


            //List<PersonalInfo> personalInfo = (from personal in db.PersonalInfo
            //                                   where personal.EmpCode.Contains(name)
            //          && personal.IsActive == true
            //                                   //select personal).ToList();
            //                                   select new PersonalInfo
            //                                   {
            //                                       Id = personal.Id,
            //                                       Name = personal.Name,
            //                                       EmpCode = personal.EmpCode,
            //                                       Email = personal.Email
            //                                   }).ToList();
            List<EmployeeList> personalInfo = (from personal in db.PersonalInfoes
                                               //join userdet in db.UserDetail
                                                
                                               where personal.Name.Contains(name)
//&& personal.IsActive == true 
&& !db.UserDetails.Any(p2 => p2.Emp_Id == personal.Id && p2.IsActive==true && personal.IsActive==true)
                                               select new EmployeeList
                                               {
                                                   Id = personal.Id,
                                                   Name = personal.Name,
                                                   EmpCode=personal.EmpCode,
                                                   Email=personal.Email
                                               }).ToList();
            return new JsonResult { Data = personalInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public void getRoles()
        {
            var val = new SelectList(db.Roles.ToList(), "Id", "Name");
            ViewData["rolelist"] = val;
        }
        public JsonResult getPersonalInfo(int id)
        {
            // db.AssignEvaluaters.Any(p2 => p2.AssignResource_Id == a.Id && p2.IsActive == true && a.IsActive == true)
            PersonalInfo personalInfo = db.PersonalInfoes.Find(id);
                //db.PersonalInfo.FirstOrDefault(a => !db.UserDetails.Any(p2 => p2.Emp_Id == a.Id && p2.IsActive == true && a.IsActive == true && a.Id == id));
            PersonalInfoDetails personaldetails = new PersonalInfoDetails();
            personaldetails.Name = personalInfo.Name;
            personaldetails.Email = personalInfo.Email;
            return new JsonResult { Data = personaldetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}