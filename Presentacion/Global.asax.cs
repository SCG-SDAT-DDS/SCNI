using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sistema;

namespace Presentacion
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            VariablesGlobales.DireccionSitio = Server.MapPath(@"");
        }

        void Application_PreSendRequestHeaders(Object sender, EventArgs e)
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);  // HTTP 1.1.
            //Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Response.Buffer = true;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "-1500"); // Proxies.
        }
    }
}
