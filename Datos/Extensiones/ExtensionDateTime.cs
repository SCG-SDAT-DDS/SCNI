using System;
using System.Globalization;

namespace Datos.Extensiones
{
    public static class ExtensionDateTime
    {
        public static string ObtenerFechaCompleta(this DateTime fecha)
        {
            var fechaTexto = fecha.Day + " " + fecha.ToString(@"\de MMMM \de yyyy", new CultureInfo("es-ES"));

            return fechaTexto;
        }
    }
}
