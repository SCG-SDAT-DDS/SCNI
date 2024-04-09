using System.Configuration;

namespace Sistema
{
    public static class VaribalesWebconfig
    {
        public static string UrlDocumentosSolicitante => ConfigurationManager.AppSettings[@"UrlDocumentosSolicitante"];

        public static string UrlPagoLineaPaseCaja => ConfigurationManager.AppSettings[@"UrlPagoLineaPaseCaja"];

        public static string UbicacionDocumentos => ConfigurationManager.AppSettings[@"UbicacionDocumentos"];
    }
}
