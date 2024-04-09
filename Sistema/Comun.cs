using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema
{
    public static class Comun
    {
        public static int ObtenerEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Now;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        public static string GenerarGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
