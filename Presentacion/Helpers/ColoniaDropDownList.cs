using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Datos.DTO.Infraestructura.ViewModels;
using Datos.Repositorios.Catalogos;

namespace Presentacion.Helpers
{
    public static class ColoniaDropDownList
    {

        public static MvcHtmlString ColoniaDdl<TModel, TProperty>(
    this HtmlHelper<TModel> helper,
    Expression<Func<TModel, TProperty>> expression,
    int idEstado, int idMunicipio, int idColonia)
        {
            var campoColonia = ObtenerCampoColonia((MemberExpression)expression.Body);
            var campoMunicipio = campoColonia.Replace("IDColonia", "IDMunicipio");

            helper.RenderPartial("_ColoniaDdl", new ColoniaViewModel(campoColonia, campoMunicipio));
            helper.ValidationMessageFor(expression, null, new { @class = "label label warning" });

            if (idMunicipio == 0)
            {
                return helper.DropDownListFor(expression, new[]
                    {
                        new SelectListItem{ Text = string.Empty, Value = string.Empty},
                    },
                    new { @class = "form-control input-sm", @disabled = true });
            }
            else
            {
                return helper.DropDownListFor(expression, ColoniaRepositorio.BuscarColoniasPorMunicipioId2(idEstado, idMunicipio, idColonia.ToString()),
                                              new { @class = "form-control input-sm" });
            }
        }

        public static MvcHtmlString ColoniaDdl<TModel, TProperty>(
    this HtmlHelper<TModel> helper,
    Expression<Func<TModel, TProperty>> expression,
    int idEstado, int idMunicipio, string clase)
        {
            var campoColonia = ObtenerCampoColonia((MemberExpression)expression.Body);
            var campoMunicipio = campoColonia.Replace("IDColonia", "IDMunicipio");

            helper.RenderPartial("_ColoniaDdl", new ColoniaViewModel(campoColonia, campoMunicipio));
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
                return helper.DropDownListFor(expression, ColoniaRepositorio.BuscarColoniasPorMunicipioId(idMunicipio),
                                              new { @class = clase });
            }
        }
        private static string ObtenerCampoColonia(MemberExpression memberExpression)
        {
            var expresionClase = memberExpression.ToString().Trim('{', '}');
            var campoColonia = expresionClase.Substring(expresionClase.IndexOf('.'), expresionClase.Length - 1);
            return campoColonia.Remove(0, 1);
        }
    }

}