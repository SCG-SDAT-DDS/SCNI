
using System.IO;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class AccesoViewModel
    {
        public Usuario Usuario { get; set; }
        public string Digestion { get; set; }
        public string Pkcs7 { get; set; }
        public TiposInicio TipoInicio { get; set; }

    }

    public class ResultGuardarPfx
    {
        public bool Result { get; set; }
        public string NombreTabla { get; set; }
        public string Valor { get; set; }
        public bool HandleError { get; set; }

    }
}
