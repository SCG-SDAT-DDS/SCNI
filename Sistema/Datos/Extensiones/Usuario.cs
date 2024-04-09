using System.ComponentModel.DataAnnotations;

namespace Datos
{
    [MetadataType(typeof(UsuarioMetaData))]
    public partial class Usuario
    {
    }

    public class UsuarioMetaData
    {
        [Display(Name = @"Usuario")]
        public string NombreUsuario { get; set; }
        [Display(Name = @"Contraseña")]
        public int Contrasena { get; set; }
        public byte[] Certificado { get; set; }
    }
}
