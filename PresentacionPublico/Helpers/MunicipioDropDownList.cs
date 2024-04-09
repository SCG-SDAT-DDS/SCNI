using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios.Catalogos;

namespace Presentacion.Helpers
{
    public static class MunicipioDropDownList
    {
        public static MvcHtmlString MunicipioDdl<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            int idEstado, int idMunicipio)
        {
            var campoMunicipio = ObtenerCampoMunicipio((MemberExpression)expression.Body);
            var campoEstado = campoMunicipio.Replace("IDMunicipio", "IDEstado");

            helper.RenderPartial("_MunicipioDdl", new MunicipioViewModel(campoMunicipio, campoEstado));
            helper.ValidationMessageFor(expression, null, new { @class = "label label warning" });

            if (idEstado == 0)
            {
                return helper.DropDownListFor(expression, new[]
                    {
                        new SelectListItem{ Text = string.Empty, Value = string.Empty},
                    },
                    new { @class = "form-control input-sm", @disabled = true });
            }
            else
            {
                return helper.DropDownListFor(expression, MunicipioRepositorio.BuscarPorEstado(idEstado),
                                              new { @class = "form-control input-sm" });
            }
        }

        public static MvcHtmlString MunicipioDdl<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            int idEstado, int idMunicipio, string clase)
        {
            var campoMunicipio = ObtenerCampoMunicipio((MemberExpression)expression.Body);
            var campoEstado = campoMunicipio.Replace("IDMunicipio", "IDEstado");

            helper.RenderPartial("_MunicipioDdl", new MunicipioViewModel(campoMunicipio, campoEstado));
            helper.ValidationMessageFor(expression, null, new { @class = "label label warning" });

            if (idEstado == 0)
            {
                return helper.DropDownListFor(expression, new[]
                    {
                        new SelectListItem{ Text = string.Empty, Value = string.Empty},
                    },
                    new { @class = clase, @disabled = true });
            }
            else
            {
                return helper.DropDownListFor(expression, MunicipioRepositorio.BuscarPorEstado(idEstado),
                                              new { @class = clase });
            }
        }

        private static string ObtenerCampoMunicipio(MemberExpression memberExpression)
        {
            var expresionClase = memberExpression.ToString().Trim('{', '}');
            var campoMunicipio = expresionClase.Substring(expresionClase.IndexOf('.'), expresionClase.Length - 1);
            return campoMunicipio.Remove(0, 1);
        }
    }

}