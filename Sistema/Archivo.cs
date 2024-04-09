using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Web;
using Sistema.Extensiones;

namespace Sistema
{
    public class Archivo
    {
        public enum Ruta
        {
            [Description(@"/Archivos/")]
            Archivos,
            [Description(@"/Archivos/FotosEmpleados/")]
            FotosEmpleados,
            [Description(@"/Archivos/FotosUsuarios/")]
            FotosUsuario,
            [Description(@"/Content/img/usuario.png")]
            FotoDefault,
            [Description(@"/Content/img/solicitudes/docs/no-document.png")]
            SolicitudDocumentosDefault,
            [Description(@"/Archivos/Documentos/")]
            SolicitudDocumentos,
            [Description(@"/Archivos/Firmas/")]
            ImagenesFirma,
            [Description(@"/Archivos/Plantillas/")]
            Plantillas,
            [Description(@"\Archivos/PasesCajas\")]
            PasesCajas
        }

        public static string ObtenerRutaServidor(Ruta ruta, string nombreArchivo)
        {
            var absolutaRuta = HttpContext.Current.Server.MapPath(ruta.Descripcion());

            if (ruta == Ruta.SolicitudDocumentos)
            {
                var carpetasFecha = DateTime.Now.Year + @"\" +
                                    DateTime.Now.Month + @"\" + 
                                    DateTime.Now.Day;

                absolutaRuta = Path.Combine(absolutaRuta, carpetasFecha);

                if (Directory.Exists(absolutaRuta) == false)
                {
                    Directory.CreateDirectory(absolutaRuta);
                }
            }

            return Path.Combine(absolutaRuta, nombreArchivo);
        }

        public static string ObtenerRutaFotoBaseDatos(HttpPostedFileBase archivo, Ruta ruta, string nombreFoto = null)
        {
            return archivo == null
                ? Ruta.FotoDefault.Descripcion()
                : ruta.Descripcion() + nombreFoto + Path.GetExtension(archivo.FileName);
        }

        public static bool GuardarImagen(HttpPostedFileBase archivo, Ruta ruta, string nombreFoto = null)
        {
            if (archivo == null) return false;
            var extension = Path.GetExtension(archivo.FileName)?.ToLower();

            if (extension != ".jpeg" && extension != ".jpg" && extension != ".png") return false;
            if (nombreFoto == null) nombreFoto = archivo.FileName;

            var rutaGuardar = ObtenerRutaServidor(ruta, nombreFoto + extension);

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            archivo.SaveAs(rutaGuardar);

            return true;
        }

        public static bool GuardarPapeleta(HttpPostedFileBase archivo, Ruta ruta, string nombrePapeleta = null)
        {
            if (archivo == null) return false;
            var extension = Path.GetExtension(archivo.FileName)?.ToLower();

            if (extension != ".jpeg" && extension != ".jpg" && extension != ".png" && extension != ".pdf") return false;
            if (nombrePapeleta == null) nombrePapeleta = archivo.FileName;

            var rutaGuardar = ObtenerRutaServidor(ruta, nombrePapeleta + extension);

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            archivo.SaveAs(rutaGuardar);

            return true;
        }

        public static bool GuardarPublicoPapeleta(byte[] archivo, Ruta ruta, string nombrePapeleta)
        {
            if (archivo == null) return false;
            
            var rutaGuardar = ObtenerRutaServidor(ruta, nombrePapeleta + ".pdf");

            var rutaAbsolutaCarpeta = HttpContext.Current.Server.MapPath(ruta.Descripcion());

            rutaGuardar = rutaGuardar.Replace(rutaAbsolutaCarpeta, string.Empty);

            rutaGuardar = VaribalesWebconfig.UbicacionDocumentos + rutaGuardar;

            var directorio = Path.GetDirectoryName(rutaGuardar);

            if (Directory.Exists(directorio) == false) Directory.CreateDirectory(directorio);

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            File.WriteAllBytes(rutaGuardar, archivo);
            
            return true;
        }

        public static bool GuardarPapeleta(byte[] archivo, Ruta ruta, string nombrePapeleta)
        {
            if (archivo == null) return false;

            var rutaGuardar = ObtenerRutaServidor(ruta, nombrePapeleta + ".pdf");

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            File.WriteAllBytes(rutaGuardar, archivo);

            return true;
        }

        public static string GuardarPlantilla(HttpPostedFileBase archivo)
        {
            if (archivo == null) return null;

            var extension = Path.GetExtension(archivo.FileName);

            if (extension != ".docx") return null;

            var nombrePlantilla = Guid.NewGuid() + extension;

            var ubicacionWebconfig = ConfigurationManager.AppSettings["UbicacionPlantillas"];

            var rutaGuardar = string.IsNullOrEmpty(ubicacionWebconfig)
                ? ObtenerRutaServidor(Ruta.Plantillas, nombrePlantilla)
                : ubicacionWebconfig + nombrePlantilla;

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            archivo.SaveAs(rutaGuardar);

            return nombrePlantilla;
        }

        public static string GuardarPaseCaja(string folio, byte[] archivo)
        {
            if (archivo == null) return null;
            
            var nombrePaseCaja = "PaseCaja_" + folio + ".pdf";

            var ubicacionWebconfig = ConfigurationManager.AppSettings["UbicacionPasesCajas"];

            var jerarquiaNombrePaseCaja = DateTime.Now.Year + @"\" + DateTime.Now.Month + @"\" +
                                          DateTime.Now.Day + @"\" + nombrePaseCaja;

            var rutaGuardar = string.IsNullOrEmpty(ubicacionWebconfig)
                ? ObtenerRutaServidor(Ruta.PasesCajas, jerarquiaNombrePaseCaja)
                : ubicacionWebconfig + nombrePaseCaja;

            var directorio = Path.GetDirectoryName(rutaGuardar);

            if (Directory.Exists(directorio) == false) Directory.CreateDirectory(directorio);

            if (File.Exists(rutaGuardar)) File.Delete(rutaGuardar);

            File.WriteAllBytes(rutaGuardar, archivo);

            return jerarquiaNombrePaseCaja;
        }

        public static string ObtenerRutaAbsolutaPaseCaja(string rutaRelativa)
        {
            var ubicacionWebconfig = ConfigurationManager.AppSettings["UbicacionPasesCajas"];

            var rutaAbsoluta = string.IsNullOrEmpty(ubicacionWebconfig)
                ? ObtenerRutaServidor(Ruta.PasesCajas, rutaRelativa)
                : ubicacionWebconfig + rutaRelativa;

            return rutaAbsoluta;
        }

        public static byte[] ObtenerPlantilla(string nombrePlantilla)
        {
            var ubicacionWebconfig = ConfigurationManager.AppSettings["UbicacionPlantillas"];
            var rutaPlantilla = string.IsNullOrEmpty(ubicacionWebconfig)
                ? VariablesGlobales.DireccionSitio + Ruta.Plantillas.Descripcion() + nombrePlantilla
                : ubicacionWebconfig + nombrePlantilla;
            if (File.Exists(rutaPlantilla))
            {
                return File.ReadAllBytes(rutaPlantilla);
            }

            return null;
        }

        public static string ObtenerRutaSolicitudesBaseDatos(HttpPostedFileBase archivo, Ruta ruta, string nombre = null)
        {
            if (archivo == null)
            {
                return Ruta.SolicitudDocumentosDefault.Descripcion();
            }

            var rutaAbsoluta = ObtenerRutaServidor(ruta, nombre) + Path.GetExtension(archivo.FileName);

            var rutaAplicacion = HttpContext.Current.Server.MapPath("~");

            return @"\" + rutaAbsoluta.Replace(rutaAplicacion, string.Empty);
        }

        public static string ObtenerRutaSolicitudesBaseDatos(byte[] archivo, Ruta ruta, string nombre = null)
        {
            if (archivo == null)
            {
                return Ruta.SolicitudDocumentosDefault.Descripcion();
            }

            var rutaAbsoluta = ObtenerRutaServidor(ruta, nombre) + ".pdf";

            var rutaAplicacion = HttpContext.Current.Server.MapPath("~");

            return @"\" + rutaAbsoluta.Replace(rutaAplicacion, string.Empty);
        }
    }
}
