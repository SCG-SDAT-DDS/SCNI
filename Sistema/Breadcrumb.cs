using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema
{
    public class Breadcrumb
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Area { get; set; }
        public string Nombre { get; set; }
    }
}
