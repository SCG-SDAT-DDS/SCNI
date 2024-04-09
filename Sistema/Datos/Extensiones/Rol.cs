using System.ComponentModel.DataAnnotations;

namespace Datos
{
    [MetadataType(typeof(RolMetaData))]
    public partial class Rol
    {
    }

    public class RolMetaData
    {
        [Display(Name = @"Inicio Sesión")]
        public TiposInicio TipoInicio { get; set; }
        [Display(Name = @"Pagina de Inicio")]
        public int PaginaInicio { get; set; }
        [Display(Name = @"Fecha de Creación")]
        public System.DateTime FechaCreacion { get; set; }
    }
}
