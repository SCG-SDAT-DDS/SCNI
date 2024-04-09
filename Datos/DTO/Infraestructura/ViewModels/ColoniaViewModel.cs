using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class ColoniaViewModel : PaginacionViewModel
    {
        public ColoniaViewModel(string campoMunicipio, string campoEstado)
        {
            CampoMunicipio = campoMunicipio;
            CampoEstado = campoEstado;
        }

        public ColoniaViewModel(){}
        public Colonia Colonia { get; set; }
        public List<Colonia> Colonias { get; set; }

        public string CampoMunicipio { get; set; }
        public string CampoEstado { get; set; }


    }
}
