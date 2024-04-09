using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class PuestoViewModel : PaginacionViewModel
    {
        public Puesto Puesto { get; set; }
        public List<Puesto> Puestos { get; set; }
    }
}
