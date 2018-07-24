using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RetentionTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           // routes.MapMvcAttributeRoutes();
            //AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                namespaces:new string[] { "RetentionTool.Controllers" },
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
           // routes.RouteData.DataTokens["UseNamespaceFallback"] = false;


        }
    }
}
