using System.Collections.Generic;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class TituloViewModel : PaginacionViewModel
    {
        public Titulo Titulo { get; set; }
        public List<Titulo> Titulos { get; set; }
    }
}
