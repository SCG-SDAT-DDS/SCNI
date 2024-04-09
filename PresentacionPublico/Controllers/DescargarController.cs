using System.Web.Mvc;
using Negocio.Servicios;

namespace Presentacion.Controllers
{
    public class DescargarController : Controller
    {
        public FileResult Carta(int idCarta)
        {
            var serviciosCarta = new ServiciosCarta();
            var folioCartaBytes = serviciosCarta.GenerarArchivoCarta(idCarta);

            return File(folioCartaBytes.Value, System.Net.Mime.MediaTypeNames.Application.Octet,
                folioCartaBytes.Key + ".docx");
        }

        public FileResult Precarta(int idSolicitud, int[] idSanciones)
        {
            var serviciosCarta = new ServiciosCarta();
            var folioCartaBytes = serviciosCarta.GenerarCartaPreliminar(idSolicitud, idSanciones ?? new int[] {});

            return File(folioCartaBytes.Value, System.Net.Mime.MediaTypeNames.Application.Octet,
                folioCartaBytes.Key + ".docx");
        }

        public FileResult PaseCaja(int idPaseCaja)
        {
            var servicios = new ServiciosPaseCaja();
            var paseCajaBytes = servicios.ObtenerRutaPaseCaja(idPaseCaja);

            return File(paseCajaBytes.Value, System.Net.Mime.MediaTypeNames.Application.Octet,
                paseCajaBytes.Key + ".pdf");
        }
    }
}