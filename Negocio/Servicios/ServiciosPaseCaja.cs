using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Datos;
using Datos.Repositorios.PaseCaja;
using Sistema;

namespace Negocio.Servicios
{
    public class ServiciosPaseCaja
    {
        public int Guardar(Datos.PaseCaja paseCaja, byte[] archivo)
        {
            paseCaja.UbicacionArchivo = Archivo.GuardarPaseCaja(paseCaja.Folio, archivo);

            using (var db = new Contexto())
            {
                var repositorio = new RepositorioPaseCaja(db);
                repositorio.Guardar(paseCaja);
                db.SaveChanges();
            }

            new Task(() => Correo.EnviarPaseCaja(paseCaja.CorreoElectronico, paseCaja.Folio, archivo)).Start();

            return paseCaja.IDPaseCaja;
        }

        public KeyValuePair<string, byte[]> ObtenerRutaPaseCaja(int idPaseCaja)
        {
            using (var db = new Contexto())
            {
                var repositorio = new RepositorioPaseCaja(db);
                var paseCaja = repositorio.BuscarUnoSolo(p => p.IDPaseCaja == idPaseCaja);

                if (paseCaja != null)
                {
                    var rutaAbsoluta = Archivo.ObtenerRutaAbsolutaPaseCaja(paseCaja.UbicacionArchivo);

                    var archivo = File.Exists(rutaAbsoluta)
                        ? File.ReadAllBytes(rutaAbsoluta)
                        : new byte[0];

                    return new KeyValuePair<string, byte[]>(Path.GetFileName(rutaAbsoluta), archivo);
                }
            }

            return new KeyValuePair<string, byte[]>(null, new byte[0]);
        }
    }
}
