using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class PersonaViewModel : PaginacionViewModel
    {
        public PersonaViewModel() { }
        public Persona Persona { get; set; }
        public List<Persona> Personas { get; set; }

    }
}
