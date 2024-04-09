using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Controllers
{
    public class CustomRedirectController : Controller
    {
        public ActionResult RedirectToScripts(string path)
        {
            // Redirigir a la segunda ruta con el path proporcionado
            string redirectTo = $"/Scripts/firma/{path}";
            return Redirect(redirectTo);
        }
    }
}