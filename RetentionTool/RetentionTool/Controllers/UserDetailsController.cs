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
           
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserDetailsViewModel uservm)
        {
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id,UserDetailsViewModel uservm)
        {
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, UserDetailsViewModel uservm)
        {
            return RedirectToAction("Index");
        }

    }
}