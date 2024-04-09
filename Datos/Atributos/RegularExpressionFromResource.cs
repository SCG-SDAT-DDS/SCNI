using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using System.Web.Mvc;

namespace Datos.Atributos
{
    public class RegularExpressionFromResource : RegularExpressionAttribute
    {
        public RegularExpressionFromResource(string idRecursoPatron, string idRecursoMensaje)
            : base(ObtenerExpresionRegularRecurso(idRecursoPatron))
        {
            ErrorMessage = ObtenerValorRecurso(idRecursoMensaje);
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RegularExpressionFromResource),
                typeof(RegularExpressionAttributeAdapter));
        }

        private static string ObtenerExpresionRegularRecurso(string idRecurso)
        {
            var rm = new ResourceManager("Resources.ExpresionesRegulares", Assembly.Load("App_GlobalResources"));
            return rm.GetString(idRecurso);
        }

        private static string ObtenerValorRecurso(string idRecurso)
        {
            var rm = new ResourceManager("Resources.General", Assembly.Load("App_GlobalResources"));
            return rm.GetString(idRecurso);
        }
    }
}
