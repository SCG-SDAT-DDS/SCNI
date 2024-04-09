using System.ComponentModel.DataAnnotations;

namespace Datos {
    [MetadataType(typeof(PlantillaMedaData))]
    public partial class Plantilla
    {
    }

    public class PlantillaMedaData
    {
        [Required]
        public string Descripcion { get; set; }
    }
}
