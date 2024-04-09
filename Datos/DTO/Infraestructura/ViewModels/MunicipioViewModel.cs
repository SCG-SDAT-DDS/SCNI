using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class MunicipioViewModel : PaginacionViewModel
    {
        public MunicipioViewModel() {

        }
        public string CampoMunicipio { get; set; }
        public string CampoEstado { get; set; }

        public Municipio Municipio { get; set; }

        public List<Municipio> Municipios { get; set; }

        public MunicipioViewModel(string campoMunicipio, string campoEstado)
        {
            CampoMunicipio = campoMunicipio;
            CampoEstado = campoEstado;
        }
    }

}
