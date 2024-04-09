using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ValidacionConstanciaViewModel
    {
        public static ValidacionConstanciaViewModel PorCodigo(string codigo)
        {
            var viewModel = new ValidacionConstanciaViewModel();

            if (string.IsNullOrWhiteSpace(codigo)) return viewModel;

            using (var db = new Contexto())
            {
                var carta = (from c in db.Cartas
                             join f in db.Firmas on c.IDCarta equals f.IDCarta
                             where c.Solicitud.Codigo == codigo &&
                                   c.Estado == EstadosCarta.Firmada &&
                                   c.Consultable
                             select c).SingleOrDefault();

                if (carta == null) return viewModel;

                viewModel.Carta = carta;
                viewModel.Oficio = carta.NumeroExpediente;
            }

            return viewModel;
        }

        public static ValidacionConstanciaViewModel PorOficio(string oficio)
        {
            var viewModel = new ValidacionConstanciaViewModel {Oficio = oficio};

            if (string.IsNullOrWhiteSpace(oficio)) return viewModel;

            using (var db = new Contexto())
            {
                var carta = (from c in db.Cartas
                             join f in db.Firmas on c.IDCarta equals f.IDCarta
                             where c.NumeroExpediente == oficio &&
                                   c.Estado == EstadosCarta.Firmada &&
                                   c.Consultable
                             select c).SingleOrDefault();

                if (carta == null) return viewModel;

                viewModel.Carta = carta;
            }

            return viewModel;
        }

        [Required(ErrorMessage = "Requerido")]
        public string Oficio { get; set; }
        public string UrlDescargarCarta { get; set; }
        public Carta Carta { get; set; }
    }
}
