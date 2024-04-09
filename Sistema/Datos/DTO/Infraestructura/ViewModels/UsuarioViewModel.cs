using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Datos;
using Datos.DTO.Infraestructura.ViewModels;

namespace Datos.DTO.Infraestructura.ViewModels
{
    public class UsuarioViewModel: PaginacionViewModel
    {
        public Usuario Usuario { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}
