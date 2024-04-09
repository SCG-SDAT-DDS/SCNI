using System;

namespace Datos.DTO.Reportes
{
    public class ReporteSancionDto
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public DateTime? FechaInscripcion { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public DateTime? FechaEjecución { get; set; }
        public string NumeroExpediente { get; set; }
        public TiposSancion TipoSancion { get; set; }
        public Origenes Origen { get; set; }
        public int SancionAno { get; set; }
        public int TiempoAnos { get; set; }
        public int TiempoMes { get; set; }
        public int TiempoDias { get; set; }
        public int IdEntidad { get; set; }
        public string Entidad { get; set; }
        public TiposEntidad TipoEntidad { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
    }
}
