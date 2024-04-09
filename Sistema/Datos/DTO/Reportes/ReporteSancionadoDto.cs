using System;

namespace Datos.DTO.Reportes
{
    public class ReporteSancionadoDto
    {
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string NumeroExpediente { get; set; }
        public TiposSancion TipoSancion { get; set; }
        public Origenes Origen { get; set; }
        public int SancionAno { get; set; }
        public int TiempoAnos { get; set; }
        public int TiempoMes { get; set; }
        public int TiempoDias { get; set; }
        public string Entidad { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaInscripcion { get; set; }
    }
}
