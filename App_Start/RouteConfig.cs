using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechMall
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "GoogleCallback",
               url: "signin-google",
               defaults: new { controller = "Account", action = "GoogleCallback" }
           );
            routes.MapRoute(
               name: "Error",
               url: "Account/Error",
               defaults: new { controller = "Account", action = "Error" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "TechMall.Controllers" }
            );
        }
    }
}
