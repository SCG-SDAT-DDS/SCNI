using System;
using System.IO;

namespace Sistema
{
    public class Log
    {
        private readonly string _formato;
        private const string NombreArchivo = "LogErrores.txt";

        public Log()
        {
            _formato = DateTime.Now.ToShortDateString() + " "
                + DateTime.Now.ToLongTimeString() + " ==> ";
        }

        public void AgregarError(string mensaje)
        {
            try
            {
                var rutaLog = Path.Combine(VariablesGlobales.DireccionSitio + @"\Logs\" + NombreArchivo);
                var sw = new StreamWriter(rutaLog, true);
                sw.WriteLine(_formato + mensaje);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                var mensajeError = ex.Message;
            }
        }
    }
}
