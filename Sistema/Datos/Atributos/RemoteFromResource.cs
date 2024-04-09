using System.Reflection;
using System.Resources;
using System.Web.Mvc;

namespace Datos.Atributos
{
    public class RemoteFromResource : RemoteAttribute
    {
        public RemoteFromResource(string controlador, string accion, string area, string recurso)
            : base(controlador, accion, area)
        {
            ErrorMessage = ObtenerValorRecurso(recurso);
        }

        private static string ObtenerValorRecurso(string idRecurso)
        {
            var rm = new ResourceManager("Resources.General", Assembly.Load("App_GlobalResources"));
            return rm.GetString(idRecurso);
        }
    }
}
