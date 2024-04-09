using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace Datos.Atributos
{
    public class DisplayNameFromResource : DisplayNameAttribute
    {
        public DisplayNameFromResource(string idRecurso)
            : base(ObtenerValorRecurso(idRecurso))
        {
        }

        private static string ObtenerValorRecurso(string idRecurso)
        {
            var rm = new ResourceManager("Resources.General", Assembly.Load("App_GlobalResources"));
            return rm.GetString(idRecurso);
        }
    }
}
