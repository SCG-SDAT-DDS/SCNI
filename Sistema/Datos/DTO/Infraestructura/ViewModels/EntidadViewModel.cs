using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class EntidadViewModel : PaginacionViewModel
    {

        public EntidadViewModel(string campoMunicipio, string campoEstado)
        {
            CampoMunicipio = campoMunicipio;
            CampoEstado = campoEstado;
        }

        public EntidadViewModel() { }
        public Entidad Entidad { get; set; }
        public List<Entidad> Entidades { get; set; }

        public string CampoMunicipio { get; set; }
        public string CampoEstado { get; set; }

    }
}
