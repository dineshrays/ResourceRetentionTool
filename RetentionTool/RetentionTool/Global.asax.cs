using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RetentionTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
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
            v.TempData["ErrorMess"] = e.Message;
        }
    }
}
