using System.Collections.Generic;
using Datos;
using Datos.DTO.Infraestructura.ViewModels;

namespace Presentacion.Areas.Catalogo.Models.ViewModels
{
    public class EstadoViewModel : PaginacionViewModel
    {
        public Estado Estado { get; set; }
        public List<Estado> Estados { get; set; }
    }
}