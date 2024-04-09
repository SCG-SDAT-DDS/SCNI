using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios.Solicitudes;

namespace Presentacion.Helpers
{
    public static class EntidadDropDownList
    {

        public static MvcHtmlString EntidadDdl<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            int idEntidadTipo, int idEntidadNombre)
        {
            var campoEntidad = ObtenerCampoEntidadNombre((MemberExpression)expression.Body);
            var campoEntidadTipo = campoEntidad.Replace("IDEntidad", "Tipo");

            helper.RenderPartial("_EntidadDdl", new SolicitudViewModel(campoEntidad, campoEntidadTipo));
            helper.ValidationMessageFor(expression, null, new { @class = "label label warning" });

            if (idEntidadTipo == 0)
            {
                return helper.DropDownListFor(expression, new[]
                    {
                        new SelectListItem{ Text = string.Empty, Value = string.Empty},
                    },
                    new { @class = "form-control input-sm", @disabled = true });
            }
            else
            {
                return helper.DropDownListFor(expression, SolicitudRepositorio.BuscarEntidadesPorTipo(idEntidadTipo),
                                              new { @class = "form-control input-sm" });
            }
        }

        public static MvcHtmlString EntidadDdl<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            int idEntidadTipo, int idEntidadNombre, string clase)
        {
            var campoEntidad = ObtenerCampoEntidadNombre((MemberExpression)expression.Body);
            var campoEntidadTipo = campoEntidad.Replace("IDEntidad", "Tipo");

            helper.RenderPartial("_EndtidadDdl", new SolicitudViewModel(campoEntidad, campoEntidadTipo));
            helper.ValidationMessageFor(expression, null, new { @class = "label label warning" });

            if (idEntidadTipo == 0)
            {
                return helper.DropDownListFor(expression, new[]
                    {
                        new SelectListItem{ Text = string.Empty, Value = string.Empty},
                    },
                    new { @class = clase, @disabled = true });
            }
            else
            {
                return helper.DropDownListFor(expression, SolicitudRepositorio.BuscarEntidadesPorTipo(idEntidadTipo),
                                              new { @class = clase });
            }
        }

        private static string ObtenerCampoEntidadNombre(MemberExpression memberExpression)
        {
            var expresionClase = memberExpression.ToString().Trim('{', '}');
            var campoEntidadNombre = expresionClase.Substring(expresionClase.IndexOf('.'), expresionClase.Length - 1);
            return campoEntidadNombre.Remove(0, 1);
        }

    }
}