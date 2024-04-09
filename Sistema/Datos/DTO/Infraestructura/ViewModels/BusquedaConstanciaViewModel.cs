using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class BusquedaConstanciaViewModel
    {
        public BusquedaConstanciaViewModel()
        {
        }

        public BusquedaConstanciaViewModel(string codigoSolicitud)
        {
            if (string.IsNullOrWhiteSpace(codigoSolicitud)) return;

            using (var db = new Contexto())
            {
                var cartaValidacion = (from s in db.Solicitud
                    join c in db.Cartas on s.IDSolicitud equals c.IDSolicitud into solicitudCarta
                    from sc in solicitudCarta.DefaultIfEmpty()
                    join f in db.Firmas on new {sc.IDCarta, Estado = (byte) sc.Estado} equals
                        new {f.IDCarta, Estado = (byte) EstadosCarta.Firmada} into cartaFirma
                    from cf in cartaFirma.DefaultIfEmpty()
                    where s.Codigo == codigoSolicitud &&
                          sc.Consultable
                    select new BusquedaConstanciaViewModel
                    {
                        Solicitud = s,
                        Carta = sc,
                        Firma = cf
                    }).SingleOrDefault();

                Codigo = codigoSolicitud;

                if (cartaValidacion == null) return;

                Solicitud = cartaValidacion.Solicitud;
                Carta = cartaValidacion.Carta;
                Firma = cartaValidacion.Firma;
            }
        }

        public string Codigo { get; set; }
        public Solicitud Solicitud { get; set; }
        public Carta Carta { get; set; }
        public Firma Firma { get; set; }
        public string UrlDescargarCarta { get; set; }

        [Display(Name = @"Folio Pago")]
        [Required(ErrorMessage = "Requerido")]
        public string Folio { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string Paterno { get; set; }
        public string Materno { get; set; }
    }
}
