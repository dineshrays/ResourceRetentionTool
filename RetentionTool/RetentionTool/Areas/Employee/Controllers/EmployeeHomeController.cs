using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;

namespace RetentionTool.Areas.Employee.Controllers
{
    public class EmployeeHomeController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        FetchDefaultIds fetchdet = new FetchDefaultIds();

        public ActionResult Index()
        {
           

            return View();
        }
    }
}