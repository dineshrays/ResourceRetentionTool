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
                Session["userId"] = userResult.Emp_Id;
                Session["RoleId"] = userResult.Role_Id;
                return RedirectToAction("Index", "Module");

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