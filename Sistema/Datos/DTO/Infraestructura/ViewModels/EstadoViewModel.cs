using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class EstadoViewModel : PaginacionViewModel
    {
        public Estado Estado { get; set; }
        public List<Estado> Estados { get; set; }
    }
}
