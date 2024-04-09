using System.Data;
using System.IO;

namespace Negocio.Repositorios.PaseCaja.Format
{
    public interface IDocumento
    {
        bool DefinirDoc();
        bool Encabezado();
        bool Detalle();
        bool Pie();
        MemoryStream Generar(DataSet info);
    }
}
