using System.ComponentModel.DataAnnotations;

namespace Datos
{
    [MetadataType(typeof(EmpleadoMetaData))]
    public partial class Empleado
    {
    }

    public class EmpleadoMetaData
    {
        [Display(Name = @"Firma")]
        public string UrlFirma { get; set; }
        [Display(Name = @"Firmante Default")]
        public string DefaultFirma { get; set; }
    }
}
