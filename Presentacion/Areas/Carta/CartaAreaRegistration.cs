using System.Web.Mvc;

namespace Presentacion.Areas.Carta
{
    public class CartaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Carta";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               name: "CustomRouteCarta",
               url: "Carta/{controller}/Scripts/firma/{*path}",
               defaults: new { controller = "CustomRedirect", action = "RedirectToScripts" }
           );

            context.MapRoute(
                "Carta_default",
                "Carta/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}