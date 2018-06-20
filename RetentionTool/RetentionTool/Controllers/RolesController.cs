using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
using RetentionTool.ViewModel;
namespace RetentionTool.Controllers
{
    public class RolesController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: Roles
        public ActionResult Index()
        {
            List<Role> role = db.Roles.Where(a => a.IsActive == true).ToList();
            RolesViewModel roleviewmodel = new RolesViewModel();
            roleviewmodel.rolevm = role;
            return View(roleviewmodel);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Role role)
        {

            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id,Role role)
        {

            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, Role role)
        {

            return View();
        }

    }
}