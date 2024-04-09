using System.Web.Mvc;
using Sistema.Extensiones;

namespace Presentacion.Helpers
{
    public static class HiddenPersonalizado
    {
        public static MvcHtmlString HiddenCifrado(this HtmlHelper htmlHelper,
            string id, string value)
        {
            var htmlHidden = new TagBuilder("input");
            htmlHidden.Attributes["type"] = "hidden";

            htmlHidden.Attributes["id"] = id;
            htmlHidden.Attributes["value"] = value.EncodeTo64();
            return MvcHtmlString.Create(htmlHidden.ToString());
        }
    }
}