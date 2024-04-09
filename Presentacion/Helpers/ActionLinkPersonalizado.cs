using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistema.Extensiones;

namespace Presentacion.Helpers
{
    public static class ActionLinkPersonalizado
    {
        public static MvcHtmlString ActionLinkCifrado(this HtmlHelper htmlHelper,
            string linkText, string action, string controller, string area,
            string claseEnlace, string claseIconos,
            IDictionary<string, int> parametros = null)
        {
            var htmla = new TagBuilder("a");
            var ruta = ObtenerRuta(area, controller, action) + ObtenerParametros(parametros);

            htmla.Attributes["href"] = ruta;

            if (!string.IsNullOrEmpty(claseEnlace))
            {
                htmla.Attributes["class"] = claseEnlace;
            }
            if (!string.IsNullOrEmpty(claseIconos))
            {
                htmla.InnerHtml = string.Format("<i class='{0}'></i>", claseIconos);
            }
            htmla.Attributes["title"] = linkText;
            return MvcHtmlString.Create(htmla.ToString());
        }

        public static MvcHtmlString ActionLinkCifrado(this HtmlHelper htmlHelper,
            string linkText, string action, string controller, string area,
            string claseEnlace,
            IDictionary<string, int> parametros = null)
        {
            var htmla = new TagBuilder("a");
            htmla.Attributes["href"] = ObtenerRuta(area, controller, action) + ObtenerParametros(parametros);

            if (!string.IsNullOrEmpty(claseEnlace))
            {
                htmla.Attributes["class"] = claseEnlace;
            }
            htmla.Attributes["title"] = linkText;
            htmla.InnerHtml = linkText;
            return MvcHtmlString.Create(htmla.ToString());
        }

        public static MvcHtmlString ActionLinkCifrado(this HtmlHelper htmlHelper,
            string linkText, string action, string controller, string area,
            IDictionary<string, int> parametros = null)
        {
            var htmla = new TagBuilder("a");
            htmla.Attributes["href"] = ObtenerRuta(area, controller, action) + ObtenerParametros(parametros);
            htmla.Attributes["title"] = linkText;
            htmla.InnerHtml = linkText;
            return MvcHtmlString.Create(htmla.ToString());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper,
            string title, string id, string href, string claseEnlace, string claseIconos)
        {
            var htmla = new TagBuilder("a");
            htmla.Attributes["href"] = href;
            htmla.Attributes["id"] = id;
            if (!string.IsNullOrEmpty(claseEnlace))
            {
                htmla.Attributes["class"] = claseEnlace + " " + id;
            }
            if (!string.IsNullOrEmpty(claseIconos))
            {
                htmla.InnerHtml = string.Format("<i class='{0}'></i>", claseIconos);
            }
            htmla.Attributes["title"] = title;
            return MvcHtmlString.Create(htmla.ToString());
        }

        private static string ObtenerParametros(IEnumerable<KeyValuePair<string, int>> parametros)
        {
            if (parametros == null) return null;

            var urlParametros = "?";

            foreach (var parametro in parametros)
            {
                urlParametros +=
                    string.Format("{0}={1}&", parametro.Key,
                        HttpUtility.UrlEncode(parametro.Value.ToString(CultureInfo.InvariantCulture).EncodeTo64()));
            }

            var parametroDevolver = urlParametros.Remove(urlParametros.Length - 1);

            return parametroDevolver;
        }

        private static string ObtenerRuta(string area, string controller, string action)
        {
            if (!string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                return string.Format("/{0}/{1}/{2}", area, controller, action);
            }
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                return string.Format("/{0}/{1}", controller, action);
            }

            return !string.IsNullOrEmpty(action)
                ? string.Format("/{0}", action)
                : "#";
        }
    }
}