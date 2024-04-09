using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.DTO.Servicios
{
    public class LstSanciones
    {
        public pagination pagination { get; set; }
        public List<result_sancion> results { get; set; }
    }

    public class pagination
    {
        public int pageSize { get; set; }
        public int page { get; set; }
        public int total { get; set; }
    }

    public class result_sancion
    {
        public string fecha_captura { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public char genero { get; set; }
        public result_institucion_dependencia institucion_dependencia { get; set; }
        public string puesto { get; set; }
        public string autoridad_sancionadora { get; set; }
        public string tipo_sancion { get; set; }
        public string tipo_falta { get; set; }
        public string causa { get; set; }
        public string expediente { get; set; }
        public result_resolucion resolucion { get; set; }
        public result_multa multa { get; set; }
        public result_inhabilitacion inhabilitacion { get; set; }
        public List<result_documento> documentos { get; set; }
    }

    public class result_institucion_dependencia
    {
        public string nombre { get; set; }

        public string siglas { get; set; }
    }

    public class result_resolucion
    {
        public string url { get; set; }
        public string fecha_notificacion { get; set; }
    }

    public class result_multa
    {
        public string monto { get; set; }
        public string unidad_moneda { get; set; }
    }

    public class result_inhabilitacion
    {
        public result_inhabilitacion(int? anios, int? meses, int? dias)
        {
            plazo = string.Empty;

            if (anios > 0)
            {
                plazo = anios + " años";
            }

            if (meses > 0)
            {
                plazo = (string.IsNullOrEmpty(plazo) == false ? ", " : string.Empty) + meses + " meses";
            }

            if (dias > 0)
            {
                plazo = (string.IsNullOrEmpty(plazo) == false ? ", " : string.Empty) + dias + " meses";
            }
        }

        public string plazo { get; set; }
        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
        public string observaciones { get; set; }
    }

    public class result_documento
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public string fecha { get; set; }
    }
}
