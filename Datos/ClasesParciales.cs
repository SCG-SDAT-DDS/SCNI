using System.ComponentModel.DataAnnotations;
using Datos.Atributos;

namespace Datos
{
    public partial class Menu
    {
        public bool PoseeOpcionMenu { get; set; }
    }
    public partial class Usuario
    {
        //[StringLength(50)]
        //[Required]
        //[Compare("Contrasena", ErrorMessage = @"Las contraseñas no coinciden")]
        //[DataType(DataType.Password)]
        //[DisplayNameFromResource("RepetirContrasena")]
        //[RegularExpressionFromResource("Contrasena", "ErrorFormatoContrasena")]
        public string RepetirContrasena { get; set; }

        //[StringLength(50)]
        //[Required]
        //[DataType(DataType.Password)]
        //[DisplayNameFromResource("ContrasenaAnterior")]
        //[RegularExpressionFromResource("Contrasena", "ErrorFormatoContrasena")]
        public string ContrasenaAnterior { get; set; }
    }
}
