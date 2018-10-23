using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RetentionTool.Models;

namespace RetentionTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = newCulture;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new ExceptionFilter());
        }

    }
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            ViewResult v = new ViewResult();
            v.ViewName = "Error";
            filterContext.Result = v;
            filterContext.ExceptionHandled = true;
            Exception e = filterContext.Exception;
            string controllerName = filterContext.Controller.ToString();
            v.TempData["ErrorMess"] = e.Message + controllerName;
            RetentionToolEntities dbEntities = new RetentionToolEntities();
            ErrorLog error = new ErrorLog();
            error.ErrorMessage = e.Message;
            error.ControllerName = controllerName;
            error.CreatedOn = DateTime.Now;
            dbEntities.ErrorLogs.Add(error);
            dbEntities.SaveChanges();
      
            //     string filePath = Path.Combine(Server.MapPath("../logfile.txt"),
            //     File.AppendAllText(@"D:\temp\logfile.txt",DateTime.Now+":"+v.TempData["ErrorMess"] + Environment.NewLine);
        }
    }
}
