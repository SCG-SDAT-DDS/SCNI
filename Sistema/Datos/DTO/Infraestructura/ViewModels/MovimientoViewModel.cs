using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class MovimientoViewModel : PaginacionViewModel
    {
        public MovimientoViewModel() {

        }

        public Movmiento Movimiento { get; set; }

        public List<Movmiento> Movimientos { get; set; }

    }

}
