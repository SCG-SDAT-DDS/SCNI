using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ValidacionViewModel : PaginacionViewModel
    {
        public Solicitud Solicitud { get; set; }
        public Persona Persona { get; set; }
        public List<Sancion> Sanciones { get; set; }
        public string IDSanciones { get; set; }
        public List<Sancion> SancionesSeleccion { get; set; }
        public string UrlDescargarArchivo { get; set; }
        public bool CartaGenerada { get; set; }
        public string Generar { get; set; }
    }
}
