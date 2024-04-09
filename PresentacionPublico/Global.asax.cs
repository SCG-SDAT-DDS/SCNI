using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentacion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
                .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
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
