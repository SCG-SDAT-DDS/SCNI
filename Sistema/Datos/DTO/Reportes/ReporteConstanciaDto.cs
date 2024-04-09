using System;

namespace Datos.DTO.Reportes
{
    public class ReporteConstanciaDto
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set;}
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Folio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaFirma { get; set; }
        public TiposCarta TipoCarta { get; set; }
        public MediosSolicitud Medio { get; set; }
        public TiposSolicitud TipoSolicitud { get; set; }
        public double? Precio { get; set; }
    }
}
