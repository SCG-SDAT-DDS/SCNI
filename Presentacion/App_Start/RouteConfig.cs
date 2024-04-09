using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentacion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Firma/{*pathInfo}");

            // Agregar una regla de enrutamiento personalizada para manejar la redirección
            routes.MapRoute(
                name: "CustomRoute",
                url: "Login/Scritps/firma/{*path}",
                defaults: new { controller = "CustomRedirect", action = "RedirectToScripts" }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Inicio", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
