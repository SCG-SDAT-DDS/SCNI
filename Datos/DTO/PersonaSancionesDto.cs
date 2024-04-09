using System.Collections.Generic;

namespace Datos.DTO
{
    public class PersonaSancionesDto
    {
        public string Nombre { get; set; }
        public IEnumerable<SancionDto> Sanciones { get; set; } 
    }
}
