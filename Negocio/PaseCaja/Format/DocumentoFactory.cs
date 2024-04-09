using System;
using System.Data;
using System.IO;

namespace Negocio.Repositorios.PaseCaja.Format
{
    public class DocumentoFactory
    {
        private static DocumentoEnum _tipoDocumento;
        public static MemoryStream Generar(DocumentoEnum documento, DataSet info, bool mostrarEx)
        {
            _tipoDocumento = documento;
            var doc = ObtenerDocumento();
            if (doc == null)
            {
                throw new Exception("El tipo de documento a generar no es válido.");
            }
            ((Documento)doc).MostrarEx = mostrarEx;
            var output = doc.Generar(info);
            return output;
        }

        private static IDocumento ObtenerDocumento()
        {
            var docAttribute = _tipoDocumento.GetAttribute<DocumentoInfoAttribute>();
            if (docAttribute == null)
            {
                return null;
            }

            var type = docAttribute.Type;
            var result = Activator.CreateInstance(type) as IDocumento;

            return result;
        }
    }
}
