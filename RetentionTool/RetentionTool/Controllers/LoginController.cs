using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;

namespace RetentionTool.Controllers
{
    public class LoginController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Login
        public ActionResult Index()
        {
            getRoleDetails();
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserDetailsViewModel uservm)
        {
            //  UserDetail userdetails = new UserDetail();
            //  userdetails = uservm.userDetail;
            UserDetail userResult = db.UserDetails.FirstOrDefault(a => a.Email == uservm.userDetail.Email && a.Password==uservm.userDetail.Password && a.IsActive==true && a.Role_Id==uservm.userDetail.Role_Id);
            if(userResult!=null)
            {
                //return RedirectToRoute(new
                //{


                //});
                Session["RoleId"] = userResult.Role_Id;

                if (userResult.Emp_Id!=null)
                {
                    Session["userId"] = userResult.Emp_Id;
                    //return RedirectToAction("")

                    //return RedirectToAction("Index", "Module", new { Area = "Admin" });
                    //return RedirectToAction("Index", "EmployeeInformation", new { Area="Employee"});
                    return RedirectToAction("Index", "Training", new { Area = "Trainer" });

                    //return RedirectToAction("Index", "ProjectWorked", new { Area = "Manager" });

                }
                else
                {
                    Session["userId"] = "1";
                    return RedirectToAction("Index", "SearchEmpSkill", new { Area= "Admin" });
                        //View("~/Views/Admin/SearchEmpSkill/Index.cshtml");
                        //RedirectToAction("Index", "Module");
                }
               
               

            }
            else
            {
                return RedirectToAction("Index", "UserDetails");

            }

        }
        public void getRoleDetails()
        {
            var val = new SelectList(db.Roles.ToList(), "Id", "Name");
            ViewData["roledetails"] = val;
            
        }
    }
}