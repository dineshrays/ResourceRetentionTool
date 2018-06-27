using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RetentionTool.Models;
namespace RetentionTool.Controllers
{
    public class AssignResourceDetController : Controller
    {
        RetentionToolEntities db = new RetentionToolEntities();
        // GET: AssignResourceDet
        public ActionResult Index()
        {
            List<AssignResourceViewModel> assResDetvm = (from a in db.AssignResources
                                                      join ar in db.AssignResourcesDets
                                                      on a.Id equals ar.AssignResources_Id
                                                      where a.IsActive == true
                                                      select new AssignResourceViewModel()
                                                      {
                                                          AssignResourceId=a.Id,
                                                          modulename=a.Module.ModuleName,
                                                          Module_Id=a.Module_Id,
                                                          employeename=ar.PersonalInfo.Name,
                                                          Employee_Id=ar.Id
                                                      }).ToList();
            return View(assResDetvm);
        }

       
        public ActionResult Create()
        {
            getEmployees();

            return View();
        }

       
        [HttpPost]
        public ActionResult Create(AssignResourceViewModel assignResvm)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Edit(int id)
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Edit(int id, AssignResourceViewModel assignResvm)
        {
            try
            {
               

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult Delete(int id, AssignResourceViewModel assignResvm)
        //{
        //    try
        //    {

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public void getEmployees()
        {
            var val = new SelectList(db.PersonalInfoes.Where(a => a.IsActive == true).ToList(), "id", "Name");
            ViewData["employeeslist"] = val;
        }
    }
}
