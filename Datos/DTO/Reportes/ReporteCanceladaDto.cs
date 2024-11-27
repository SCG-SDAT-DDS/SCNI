using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Reportes
{
    public class ReporteCanceladaDto
    {
        public string FolioPaseACaja { get; set; }

        public string Solicitante { get; set; }

        public string FolioPago { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string MotivoCancelacion { get; set; }
        public string NumeroExpediente { get; set; }
    }
}
