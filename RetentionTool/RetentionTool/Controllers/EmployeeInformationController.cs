using RetentionTool.Models;
using RetentionTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Controllers
{
    public class EmployeeInformationController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: EmployeeInformation
        public ActionResult Index()
        {
            List<PersonalInfoModel> details = (from a in db.PersonalInfoes
                                                          select new PersonalInfoModel
                                                          {
                                                              EmpCode =a.EmpCode,
                                                              Name = a.Name
                                                         }).ToList();
            ViewBag.details = details;
            return View();
        }

        public ActionResult Create()
        {

            return View();
        }
    }
}