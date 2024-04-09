using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class SolicitudViewModel : PaginacionViewModel
    {
        public SolicitudViewModel(string campoEntidadNombre, string campoEntidadTipo) {
            CampoEntidadNombre = campoEntidadNombre;
            CampoEntidadTipo = campoEntidadTipo;
        }

        public SolicitudViewModel() { }
        
        public Solicitud Solicitud { get; set; }
        public List<Solicitud> Solicitudes { get; set; }
        public List<Estado> Estados { get; set; }
        public string CampoEntidadNombre { get; set; }
        public string CampoEntidadTipo { get; set; }
        public string UrlDescargarArchivo { get; set; }
        public bool SinFirmar { get; set; }
        public bool? Pendientes { get; set; }
        public bool? Atrasadas { get; set; }
        public int IdEmpleado { get; set; }
        public int FiltroMes { get; set; }
        public int FiltroAnio { get; set; }
    }
}
